using NAudio.Gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayer
{
    public partial class FrmIndex : Form
    {
        private AudioPlayer audioPlayer;
        private VisualizationPanel visualizationPanel;
        private Timer uiUpdateTimer;
        private PlaylistManager playlistManager;
        private bool isUserDragging = false;
        public FrmIndex()
        {
            InitializeComponent();
            SetupAudioPlayer();
            SetupTimer();
            SetupControls();
            SetupVisualization();
            SetupPlaylistManager();
        }

        private void SetupControls()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            cmbVisualizationType.Items.AddRange(new[] { "Barras", "Círculo", "Onda" });
            cmbVisualizationType.SelectedIndex = 0;
            lblCurrentTime.Size = new Size(60, 20);
            lblTotalTime.Size = new Size(60, 20);
            volumenSlider.Size = new Size(100, 10);
        }

        private void SetupVisualization()
        {
            // Create visualization panel (not docked)
            visualizationPanel = new VisualizationPanel
            {
                BackColor = Color.Black,
                Location = new Point(0, 0)
            };

            // Handle form resize to adjust panel size
            this.Resize += UpdateVisualizationPanelSize;

            // Add panel to form
            this.Controls.Add(visualizationPanel);

            // Set initial size
            UpdateVisualizationPanelSize(this, EventArgs.Empty);
        }

        private void UpdateVisualizationPanelSize(object sender, EventArgs e)
        {
            const int rightPanelWidth = 230; 

            visualizationPanel.Size = new Size(
                this.ClientSize.Width - rightPanelWidth,
                this.ClientSize.Height
            );
        }

        private void SetupAudioPlayer()
        {
            audioPlayer = new AudioPlayer();
            audioPlayer.FftCalculated += AudioPlayer_FftCalculated;
            audioPlayer.PlaybackStateChanged += AudioPlayer_PlaybackStateChanged;
        }

        private void SetupTimer()
        {
            uiUpdateTimer = new Timer { Interval = 100 };
            uiUpdateTimer.Tick += UiUpdateTimer_Tick;
            uiUpdateTimer.Start();
        }

        // Event Handlers

        private void AudioPlayer_FftCalculated(object sender, float[] magnitudes)
        {
            visualizationPanel?.UpdateVisualization(magnitudes);
        }

        private void AudioPlayer_PlaybackStateChanged(object sender, PlaybackState state)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                btnPlay.Enabled = state != PlaybackState.Playing;
                btnPause.Enabled = state == PlaybackState.Playing;
                btnStop.Enabled = state != PlaybackState.Stopped;
            }));
        }

        private void UiUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (audioPlayer?.IsPlaying == true)
            {
                lblCurrentTime.Text = FormatTime(audioPlayer.CurrentTime);
                positionSlider.Value = Math.Min((int)audioPlayer.CurrentTime.TotalSeconds, positionSlider.Maximum);
            }
        }

        private string FormatTime(TimeSpan time)
        {
            return $"{(int)time.TotalMinutes:D2}:{time.Seconds:D2}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            audioPlayer?.Dispose();
            uiUpdateTimer?.Dispose();
            base.OnFormClosing(e);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Audio Files|*.mp3;*.wav;*.flac;*.m4a|All Files|*.*";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openDialog.FileName;

                        // Agregar el archivo al playlist manager
                        playlistManager.AddTrack(filePath);

                        // Obtener el índice del archivo recién agregado
                        int newTrackIndex = playlistManager.TrackCount - 1;

                        // Seleccionar la pista en el playlist manager
                        if (playlistManager.SelectTrack(newTrackIndex))
                        {
                            // Cargar el archivo en el reproductor
                            audioPlayer.Load(filePath);

                            // Actualizar etiquetas
                            lblFileName.Text = Path.GetFileName(filePath);
                            lblTotalTime.Text = FormatTime(audioPlayer.TotalTime);
                            positionSlider.Maximum = (int)audioPlayer.TotalTime.TotalSeconds;
                            positionSlider.Value = 0;

                            // Reproducir automáticamente
                            btnPlay_Click(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar el archivo: {ex.Message}");
                    }
                }
            }
        }


        private void btnPlay_Click(object sender, EventArgs e) => audioPlayer?.Play();
        private void btnPause_Click(object sender, EventArgs e) => audioPlayer?.Pause();
        private void btnStop_Click(object sender, EventArgs e) => audioPlayer?.Stop();

        private void cmbVisualizationType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (visualizationPanel != null)
            {
                visualizationPanel.VisualizationType = (VisualizationType)cmbVisualizationType.SelectedIndex;
            }
        }

        private void volumenSlider_ValueChanged_1(object sender, EventArgs e)
        {
            if (audioPlayer != null)
                audioPlayer.Volume = volumenSlider.Value / 100f;
        }

        private void positionSlider_ValueChanged_1(object sender, EventArgs e)
        {
            if (audioPlayer != null && !audioPlayer.IsPlaying)
            {
                audioPlayer.Position = TimeSpan.FromSeconds(positionSlider.Value);
            }
        }

        // Evento doble click en la lista de reproducción
        private void playlistBox_DoubleClick(object sender, EventArgs e)
        {
            // Verificar que hay un elemento seleccionado y que el índice es válido
            if (playlistBox.SelectedIndex >= 0 &&
                playlistBox.SelectedIndex < playlistManager.TrackCount)
            {
                try
                {
                    // Seleccionar la canción en el playlist manager
                    if (playlistManager.SelectTrack(playlistBox.SelectedIndex))
                    {
                        // Obtener la canción seleccionada
                        var selectedTrack = playlistManager.CurrentTrack;
                        if (selectedTrack != null && File.Exists(selectedTrack.FilePath))
                        {
                            // Cargar y reproducir la canción seleccionada
                            audioPlayer.Load(selectedTrack.FilePath);

                            // Actualizar etiquetas de la UI
                            lblFileName.Text = selectedTrack.DisplayName;
                            lblTotalTime.Text = FormatTime(audioPlayer.TotalTime);

                            positionSlider.Maximum = (int)audioPlayer.TotalTime.TotalSeconds;
                            positionSlider.Value = 0;

                            // Iniciar reproducción
                            audioPlayer.Play();
                        }
                        else
                        {
                            MessageBox.Show("El archivo no existe o no se pudo acceder a él.",
                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al reproducir el archivo: {ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Dibujo personalizado para los elementos de la playlist
        private void playlistBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= playlistManager.TrackCount)
                return;

            e.DrawBackground();

            var track = playlistManager.Tracks[e.Index];
            var isCurrentTrack = e.Index == playlistManager.CurrentTrackIndex;
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Colores
            Color textColor = Color.Black;
            Color backColor = e.BackColor;

            if (isCurrentTrack && !isSelected)
            {
                backColor = Color.LightBlue;
                textColor = Color.DarkBlue;
            }
            else if (isSelected)
            {
                backColor = SystemColors.Highlight;
                textColor = SystemColors.HighlightText;
            }

            // Dibujar fondo
            using (var brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            // Dibujar texto
            var displayText = $"{e.Index + 1:D2}. {track.DisplayName}";
            if (track.Duration != TimeSpan.Zero)
                displayText += $" [{track.FormattedDuration}]";

            using (var brush = new SolidBrush(textColor))
            {
                var textRect = new Rectangle(e.Bounds.X + 5, e.Bounds.Y,
                                           e.Bounds.Width - 10, e.Bounds.Height);

                e.Graphics.DrawString(displayText, e.Font, brush, textRect,
                                    StringFormat.GenericDefault);
            }

            // Indicador de canción actual
            if (isCurrentTrack)
            {
                using (var pen = new Pen(Color.DarkBlue, 2))
                {
                    e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y,
                                      e.Bounds.X, e.Bounds.Bottom - 1);
                }
            }

            e.DrawFocusRectangle();
        }

        // Manejo de teclas en la playlist
        private void playlistBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    // Reproducir canción seleccionada
                    if (playlistBox.SelectedIndex >= 0)
                    {
                        playlistBox_DoubleClick(sender, EventArgs.Empty);
                    }
                    break;

                case Keys.Delete:
                    // Eliminar canción seleccionada
                    if (playlistBox.SelectedIndex >= 0)
                    {
                        playlistManager.RemoveTrack(playlistBox.SelectedIndex);
                    }
                    break;

                case Keys.Space:
                    // Pausar/reanudar reproducción
                    if (audioPlayer.IsPlaying)
                        audioPlayer.Pause();
                    else
                        audioPlayer.Play();
                    e.Handled = true;
                    break;

                case Keys.Up:
                case Keys.Down:
                    // Navegación normal, no hacer nada especial
                    break;
            }
        }

        // Configuración del playlist manager
        private void SetupPlaylistManager()
        {
            playlistManager = new PlaylistManager();
            playlistManager.TrackChanged += PlaylistManager_TrackChanged;
            playlistManager.PlaylistUpdated += PlaylistManager_PlaylistUpdated;
        }

        // Evento cuando cambia la canción actual
        private void PlaylistManager_TrackChanged(object sender, AudioTrack track)
        {
            if (track != null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = $"Music Player - {track.DisplayName}";
                    lblFileName.Text = track.DisplayName;
                    lblTotalTime.Text = FormatTime(track.Duration);

                    // Usar la función segura para establecer el índice
                    SafeSetSelectedIndex(playlistManager.CurrentTrackIndex);

                    // Hacer scroll seguro
                    if (playlistManager.CurrentTrackIndex >= 0 &&
                        playlistManager.CurrentTrackIndex < playlistBox.Items.Count)
                    {
                        playlistBox.TopIndex = Math.Max(0,
                            playlistManager.CurrentTrackIndex - playlistBox.Height / playlistBox.ItemHeight / 2);
                    }

                    playlistBox.Invalidate();
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = "Music Player";
                    SafeSetSelectedIndex(-1);
                    playlistBox.Invalidate();
                });
            }
        }

        // Función adicional para manejar la selección segura del ListBox
        private void SafeSetSelectedIndex(int index)
        {
            if (playlistBox.Items.Count > 0 && index >= 0 && index < playlistBox.Items.Count)
            {
                try
                {
                    playlistBox.SelectedIndex = index;
                }
                catch (ArgumentOutOfRangeException)
                {
                    playlistBox.SelectedIndex = -1;
                }
            }
            else
            {
                playlistBox.SelectedIndex = -1;
            }
        }

        // Evento cuando se actualiza la playlist
        private void PlaylistManager_PlaylistUpdated(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                // Actualizar la lista visual
                RefreshPlaylistDisplay();

                // Actualizar estado de botones
                btnPrevious.Enabled = playlistManager.HasTracks;
                btnNext.Enabled = playlistManager.HasTracks;
                btnClearPlaylist.Enabled = playlistManager.HasTracks;
            });
        }

        // Método auxiliar para actualizar la visualización de la playlist
        private void RefreshPlaylistDisplay()
        {
            // Guardar el índice actual antes de limpiar
            int currentIndex = playlistManager.CurrentTrackIndex;

            // Limpiar la lista visual
            playlistBox.Items.Clear();

            // Agregar todas las pistas
            foreach (var track in playlistManager.Tracks)
            {
                playlistBox.Items.Add(track.DisplayName);
            }

            // Seleccionar el índice correcto solo si es válido
            if (currentIndex >= 0 && currentIndex < playlistBox.Items.Count)
            {
                try
                {
                    playlistBox.SelectedIndex = currentIndex;
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Si hay un error, no seleccionar nada
                    playlistBox.SelectedIndex = -1;
                }
            }
            else
            {
                playlistBox.SelectedIndex = -1;
            }
        }


        // Canción anterior
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            var previousTrack = playlistManager.GetPreviousTrack();
            if (previousTrack != null)
            {
                audioPlayer.Load(previousTrack.FilePath);
                audioPlayer.Play();
                lblFileName.Text = previousTrack.DisplayName;
                lblTotalTime.Text = FormatTime(audioPlayer.TotalTime);

                positionSlider.Maximum = (int)audioPlayer.TotalTime.TotalSeconds;
                positionSlider.Value = 0;
            }
        }

        // Siguiente canción
        private void btnNext_Click(object sender, EventArgs e)
        {
            var nextTrack = playlistManager.GetNextTrack();
            if (nextTrack != null)
            {
                audioPlayer.Load(nextTrack.FilePath);
                audioPlayer.Play();
                lblFileName.Text = nextTrack.DisplayName;
                lblTotalTime.Text = FormatTime(audioPlayer.TotalTime);

                positionSlider.Maximum = (int)audioPlayer.TotalTime.TotalSeconds;
                positionSlider.Value = 0;

            }
        }

        // Agregar archivos a la playlist
        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar archivos de audio";
                openFileDialog.Filter = FileHelper.GetAudioFileFilter();
                openFileDialog.Multiselect = true;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Mostrar progreso si hay muchos archivos
                        if (openFileDialog.FileNames.Length > 10)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            // Aquí podrías mostrar una barra de progreso
                        }

                        playlistManager.AddTracks(openFileDialog.FileNames);

                        // Si no hay reproducción activa y se agregaron canciones, cargar la primera
                        if (!audioPlayer.IsPlaying && playlistManager.HasTracks && playlistManager.CurrentTrack != null)
                        {
                            audioPlayer.Load(playlistManager.CurrentTrack.FilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar archivos: {ex.Message}",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        // Limpiar playlist
        private void btnClearPlaylist_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que quieres limpiar toda la playlist?",
                                        "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Detener reproducción si está activa
                if (audioPlayer.IsPlaying)
                {
                    audioPlayer.Stop();
                }

                playlistManager.ClearPlaylist();

                // Actualizar título
                this.Text = "Music Player";
            }
        }

        private void positionSlider_Scroll(object sender, EventArgs e)
        {
            if (audioPlayer != null)
            {
                audioPlayer.Position = TimeSpan.FromSeconds(positionSlider.Value);
                lblCurrentTime.Text = FormatTime(TimeSpan.FromSeconds(positionSlider.Value));
            }
        }
    }
}