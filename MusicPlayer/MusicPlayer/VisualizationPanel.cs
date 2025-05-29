using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MusicPlayer
{
    public enum VisualizationType
    {
        Bars,
        Circle,
        Wave,
        Particles
    }

    public class VisualizationPanel : Panel
    {
        private float[] currentMagnitudes;
        private float[] smoothedMagnitudes;
        private VisualizationType visualizationType = VisualizationType.Bars;
        private readonly Timer renderTimer;
        private readonly Random random = new Random();

        // Configuración de colores
        private readonly Color[] spectrumColors =
        {
            Color.FromArgb(255, 0, 255),     // Magenta
            Color.FromArgb(0, 255, 255),     // Cyan
            Color.FromArgb(0, 255, 0),       // Verde
            Color.FromArgb(255, 255, 0),     // Amarillo
            Color.FromArgb(255, 165, 0),     // Naranja
            Color.FromArgb(255, 0, 0)        // Rojo
        };

        public VisualizationType VisualizationType
        {
            get => visualizationType;
            set
            {
                visualizationType = value;
                Invalidate();
            }
        }

        public VisualizationPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer, true);

            renderTimer = new Timer { Interval = 16 }; // ~60 FPS
            renderTimer.Tick += (s, e) => Invalidate();
            renderTimer.Start();
        }

        public void UpdateVisualization(float[] magnitudes)
        {
            if (magnitudes == null) return;

            currentMagnitudes = (float[])magnitudes.Clone();

            // Suavizado para evitar cambios bruscos
            if (smoothedMagnitudes == null)
                smoothedMagnitudes = new float[magnitudes.Length];

            for (int i = 0; i < magnitudes.Length && i < smoothedMagnitudes.Length; i++)
            {
                smoothedMagnitudes[i] = smoothedMagnitudes[i] * 0.8f + magnitudes[i] * 0.2f;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (smoothedMagnitudes == null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            switch (visualizationType)
            {
                case VisualizationType.Bars:
                    DrawBars(g);
                    break;
                case VisualizationType.Circle:
                    DrawCircle(g);
                    break;
                case VisualizationType.Wave:
                    DrawWave(g);
                    break;
                case VisualizationType.Particles:
                    DrawParticles(g);
                    break;
            }
        }

        private void DrawBars(Graphics g)
        {
            var width = Width;
            var height = Height;
            var barCount = Math.Min(64, smoothedMagnitudes.Length / 8); // Reducir número de barras
            var barWidth = width / (float)barCount;

            for (int i = 0; i < barCount; i++)
            {
                // Promediar varias magnitudes por barra
                var startIdx = i * smoothedMagnitudes.Length / barCount;
                var endIdx = Math.Min((i + 1) * smoothedMagnitudes.Length / barCount, smoothedMagnitudes.Length);

                float avgMagnitude = 0;
                for (int j = startIdx; j < endIdx; j++)
                {
                    avgMagnitude += smoothedMagnitudes[j];
                }
                avgMagnitude /= (endIdx - startIdx);

                var barHeight = avgMagnitude * height * 4f;
                var x = i * barWidth;
                var y = height - barHeight;

                // Color basado en la frecuencia
                var colorIndex = (int)(i / (float)barCount * spectrumColors.Length);
                colorIndex = Math.Min(colorIndex, spectrumColors.Length - 1);
                var color = spectrumColors[colorIndex];

                // Intensidad basada en la magnitud
                var alpha = (int)(Math.Min(avgMagnitude * 2, 1) * 255);
                var brushColor = Color.FromArgb(alpha, color);

                using (var brush = new SolidBrush(brushColor))
                {
                    g.FillRectangle(brush, x + 1, y, barWidth - 2, barHeight);
                }

                // Efecto de brillo en la parte superior
                if (barHeight > 10)
                {
                    var glowColor = Color.FromArgb(alpha / 2, Color.White);
                    using (var glowBrush = new SolidBrush(glowColor))
                    {
                        g.FillRectangle(glowBrush, x + 1, y, barWidth - 2, 3);
                    }
                }
            }
        }

        private void DrawCircle(Graphics g)
        {
            var centerX = Width / 2f;
            var centerY = Height / 2f;
            var radius = Math.Min(Width, Height) / 3f;
            var barCount = 32; // Número fijo para coincidir con la animación
            float anguloRotacion = (float)(DateTime.Now.TimeOfDay.TotalMilliseconds * 0.02 % 360);
            float tiempo = (float)DateTime.Now.TimeOfDay.TotalSeconds;

            // Dibujar rayos animados (de tu clase Animacion)
            for (int i = 0; i < barCount; i++)
            {
                float anguloBase = i * (360f / barCount);
                float anguloTotal = anguloBase + anguloRotacion + 20 * (float)Math.Sin(tiempo * 3 + i);
                float anguloRad = anguloTotal * (float)Math.PI / 180f;

                float longitudBase = radius * 0.7f;
                float longitud = longitudBase * (0.7f + 0.3f * (float)Math.Sin(tiempo * 8 + i));

                float endX = centerX + (float)Math.Cos(anguloRad) * longitud;
                float endY = centerY + (float)Math.Sin(anguloRad) * longitud;

                Color rayoColor = ColorFromHsv((anguloRotacion + i * (360f / barCount)) % 360f, 0.8f, 0.9f);

                using (Pen pen = new Pen(Color.FromArgb(80, rayoColor), 2))
                {
                    g.DrawLine(pen, centerX, centerY, endX, endY);
                }
            }

            // Dibujar barras de audio reactivas
            if (smoothedMagnitudes.Length >= barCount)
            {
                for (int i = 0; i < barCount; i++)
                {
                    float angle = (float)(2 * Math.PI * i / barCount);
                    float magnitude = smoothedMagnitudes[i % smoothedMagnitudes.Length];
                    float barLength = magnitude * radius * 1.8f;

                    float startX = centerX + (float)Math.Cos(angle) * (radius * 0.5f);
                    float startY = centerY + (float)Math.Sin(angle) * (radius * 0.5f);
                    float endX = centerX + (float)Math.Cos(angle) * (radius * 0.5f + barLength);
                    float endY = centerY + (float)Math.Sin(angle) * (radius * 0.5f + barLength);

                    Color barColor = ColorFromHsv((i * (360f / barCount) + anguloRotacion * 0.5f) % 360f, 1f, 1f);
                    int alpha = 150 + (int)(magnitude * 100);

                    using (Pen pen = new Pen(Color.FromArgb(alpha, barColor), 4))
                    {
                        g.DrawLine(pen, startX, startY, endX, endY);
                    }
                }
            }

            // Círculo central pulsante
            float pulse = 0.8f + 0.2f * (float)Math.Sin(tiempo * 5);
            using (Brush brush = new SolidBrush(ColorFromHsv(anguloRotacion % 360f, 0.7f, 1f)))
            {
                float circleSize = radius * 0.15f * pulse;
                g.FillEllipse(brush, centerX - circleSize / 2, centerY - circleSize / 2, circleSize, circleSize);
            }
        }

        // Función auxiliar para colores HSV (necesaria)
        private Color ColorFromHsv(float hue, float saturation, float value)
        {
            int hi = (int)(hue / 60) % 6;
            float f = hue / 60 - hi;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);

            float r, g, b;
            switch (hi)
            {
                case 0: r = value; g = t; b = p; break;
                case 1: r = q; g = value; b = p; break;
                case 2: r = p; g = value; b = t; break;
                case 3: r = p; g = q; b = value; break;
                case 4: r = t; g = p; b = value; break;
                case 5: r = value; g = p; b = q; break;
                default: r = g = b = value; break;
            }

            return Color.FromArgb(255, (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        private void DrawWave(Graphics g)
        {
            if (smoothedMagnitudes.Length < 2) return;

            var width = Width;
            var height = Height;
            var centerY = height / 2f;

            // Aumentamos la amplitud para hacer las ondas más grandes
            float amplitudeBoost = 1.5f; // Factor de amplificación (ajustable)

            // Creamos puntos para la onda principal
            var points = new PointF[smoothedMagnitudes.Length];
            for (int i = 0; i < smoothedMagnitudes.Length; i++)
            {
                var x = i * width / (float)(smoothedMagnitudes.Length - 1);
                var y = centerY - (smoothedMagnitudes[i] * centerY * amplitudeBoost);
                points[i] = new PointF(x, Math.Max(0, Math.Min(height, y))); // Limitamos Y dentro de los bordes
            }

            // Configuración para el gradiente de color
            using (var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(width, 0),
                Color.Cyan,
                Color.Magenta))
            {
                // Dibujamos la onda principal con relleno
                if (points.Length > 1)
                {
                    // Creamos un path para el relleno
                    using (var path = new GraphicsPath())
                    {
                        path.AddLines(points);

                        // Añadimos línea de cierre para el relleno
                        path.AddLine(points[points.Length - 1].X, points[points.Length - 1].Y,
                                    points[points.Length - 1].X, centerY);
                        path.AddLine(points[0].X, centerY, points[0].X, points[0].Y);

                        // Rellenamos la onda
                        g.FillPath(brush, path);
                    }

                    // Contorno de la onda
                    using (var pen = new Pen(Color.Cyan, 3))
                    {
                        g.DrawLines(pen, points);
                    }
                }
            }

            // Onda reflejada con efecto de desvanecimiento
            var reflectedPoints = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                reflectedPoints[i] = new PointF(
                    points[i].X,
                    centerY + (centerY - points[i].Y));
            }

            using (var pen = new Pen(Color.FromArgb(150, Color.Cyan), 2))
            {
                if (reflectedPoints.Length > 1)
                {
                    g.DrawLines(pen, reflectedPoints);
                }
            }

            // Efecto de brillo/glow
            if (points.Length > 1)
            {
                using (var glowPen = new Pen(Color.FromArgb(50, Color.White), 8))
                {
                    g.DrawLines(glowPen, points);
                    g.DrawLines(glowPen, reflectedPoints);
                }
            }
        }

        private void DrawParticles(Graphics g)
        {
            var centerX = Width / 2f;
            var centerY = Height / 2f;
            var particleCount = Math.Min(100, smoothedMagnitudes.Length);

            for (int i = 0; i < particleCount; i++)
            {
                if (i >= smoothedMagnitudes.Length) break;

                var magnitude = smoothedMagnitudes[i];
                if (magnitude < 0.1f) continue; // Solo dibujar partículas activas

                // Posición basada en análisis de frecuencia
                var angle = (float)(2 * Math.PI * i / particleCount);
                var distance = magnitude * Math.Min(Width, Height) / 4f;
                var x = centerX + (float)Math.Cos(angle) * distance;
                var y = centerY + (float)Math.Sin(angle) * distance;

                // Tamaño y color basado en magnitud
                var size = magnitude * 20 + 2;
                var colorIndex = (int)(i / (float)particleCount * spectrumColors.Length);
                colorIndex = Math.Min(colorIndex, spectrumColors.Length - 1);
                var color = spectrumColors[colorIndex];
                var alpha = (int)(magnitude * 255);

                // Dibujar partícula con efecto de brillo
                using (var brush = new SolidBrush(Color.FromArgb(alpha, color)))
                {
                    g.FillEllipse(brush, x - size / 2, y - size / 2, size, size);
                }

                // Efecto de brillo exterior
                using (var glowBrush = new SolidBrush(Color.FromArgb(alpha / 3, Color.White)))
                {
                    var glowSize = size * 1.5f;
                    g.FillEllipse(glowBrush, x - glowSize / 2, y - glowSize / 2, glowSize, glowSize);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                renderTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}