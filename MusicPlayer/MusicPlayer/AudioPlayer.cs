using NAudio.Wave;
using NAudio.Dsp;
using System;

namespace MusicPlayer
{
    public enum PlaybackState
    {
        Stopped,
        Playing,
        Paused
    }

    public class AudioPlayer : IDisposable
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private readonly AudioAnalyzer analyzer;
        private float volume = 0.5f;
        private bool isPositionChanging = false;

        public event EventHandler<float[]> FftCalculated;
        public event EventHandler<PlaybackState> PlaybackStateChanged;
        public event EventHandler TrackEnded;

        public TimeSpan TotalTime => audioFile?.TotalTime ?? TimeSpan.Zero;
        public TimeSpan CurrentTime => audioFile?.CurrentTime ?? TimeSpan.Zero;
        public bool IsPlaying => outputDevice?.PlaybackState == NAudio.Wave.PlaybackState.Playing;
        public string CurrentFilePath { get; private set; }

        public float Volume
        {
            get => volume;
            set
            {
                volume = Math.Max(0, Math.Min(1, value));
                if (outputDevice != null)
                    outputDevice.Volume = volume;
            }
        }

        public TimeSpan Position
        {
            get => audioFile?.CurrentTime ?? TimeSpan.Zero;
            set
            {
                if (audioFile != null && !isPositionChanging)
                {
                    isPositionChanging = true;
                    try
                    {
                        // Permitir cambio de posición incluso durante reproducción
                        audioFile.CurrentTime = value;
                    }
                    finally
                    {
                        isPositionChanging = false;
                    }
                }
            }
        }

        public AudioPlayer(int fftLength = 1024)
        {
            analyzer = new AudioAnalyzer(fftLength);
            analyzer.FftCalculated += (sender, magnitudes) => FftCalculated?.Invoke(this, magnitudes);
        }

        public void Load(string filePath)
        {
            Dispose(); // Liberar recursos previos

            CurrentFilePath = filePath;
            audioFile = new AudioFileReader(filePath);
            outputDevice = new WaveOutEvent();
            outputDevice.Volume = volume;

            // Configurar eventos
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;

            // Inicializar con el proveedor de muestras
            outputDevice.Init(new SampleProvider(audioFile, analyzer));

            OnPlaybackStateChanged(PlaybackState.Stopped);
        }

        public void Play()
        {
            outputDevice?.Play();
            OnPlaybackStateChanged(PlaybackState.Playing);
        }

        public void Pause()
        {
            outputDevice?.Pause();
            OnPlaybackStateChanged(PlaybackState.Paused);
        }

        public void Stop()
        {
            outputDevice?.Stop();
            if (audioFile != null)
                audioFile.Position = 0;
            OnPlaybackStateChanged(PlaybackState.Stopped);
        }

        private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            OnPlaybackStateChanged(PlaybackState.Stopped);

            // Si la canción terminó naturalmente (no fue detenida manualmente)
            if (audioFile != null && audioFile.Position >= audioFile.Length - 1000) // Margen de error
            {
                TrackEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnPlaybackStateChanged(PlaybackState state)
        {
            PlaybackStateChanged?.Invoke(this, state);
        }

        public void Dispose()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
            outputDevice = null;
            audioFile = null;
        }
    }

    public class SampleProvider : ISampleProvider
    {
        private readonly AudioFileReader source;
        private readonly AudioAnalyzer analyzer;

        public SampleProvider(AudioFileReader source, AudioAnalyzer analyzer)
        {
            this.source = source;
            this.analyzer = analyzer;
            this.WaveFormat = source.WaveFormat;
        }

        public WaveFormat WaveFormat { get; }

        public int Read(float[] buffer, int offset, int count)
        {
            var samplesRead = source.Read(buffer, offset, count);

            // Enviar muestras al analizador
            for (int i = 0; i < samplesRead; i++)
            {
                analyzer.AddSample(buffer[offset + i]);
            }

            return samplesRead;
        }
    }

    public class AudioAnalyzer
    {
        private readonly Complex[] fftBuffer;
        private readonly int fftLength;
        private int bufferPosition;
        private readonly object lockObject = new object();

        public float[] Magnitudes { get; private set; }
        public event EventHandler<float[]> FftCalculated;

        public AudioAnalyzer(int fftLength = 1024)
        {
            this.fftLength = fftLength;
            fftBuffer = new Complex[fftLength];
            Magnitudes = new float[fftLength / 2];
        }

        public void AddSample(float value)
        {
            lock (lockObject)
            {
                fftBuffer[bufferPosition].X = value;
                fftBuffer[bufferPosition].Y = 0;
                bufferPosition++;

                if (bufferPosition >= fftLength)
                {
                    bufferPosition = 0;
                    ComputeFFT();
                    FftCalculated?.Invoke(this, (float[])Magnitudes.Clone());
                }
            }
        }

        private void ComputeFFT()
        {
            // Aplicar ventana de Hanning para reducir el "leakage"
            ApplyHanningWindow();

            // Calcular FFT
            FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2), fftBuffer);

            // Calcular magnitudes y aplicar escala logarítmica
            for (int i = 0; i < fftLength / 2; i++)
            {
                var magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);

                // Escala logarítmica para mejor visualización
                Magnitudes[i] = (float)(Math.Log10(magnitude * 1000 + 1) / 4.0);

                // Normalizar entre 0 y 1
                Magnitudes[i] = Math.Max(0, Math.Min(1, Magnitudes[i]));
            }
        }

        private void ApplyHanningWindow()
        {
            for (int i = 0; i < fftLength; i++)
            {
                var window = 0.5 * (1 - Math.Cos(2 * Math.PI * i / (fftLength - 1)));
                fftBuffer[i].X *= (float)window;
            }
        }
    }
}