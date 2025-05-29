using System;
using System.IO;
using System.Linq;

namespace MusicPlayer
{
    public static class FileHelper
    {
        // Extensiones de archivo soportadas
        public static readonly string[] SupportedExtensions =
        {
            ".mp3", ".wav", ".flac", ".m4a", ".aac", ".ogg", ".wma"
        };

        /// <summary>
        /// Verifica si un archivo es un formato de audio soportado
        /// </summary>
        public static bool IsAudioFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return SupportedExtensions.Contains(extension);
        }

        /// <summary>
        /// Obtiene el filtro para el diálogo de apertura de archivos
        /// </summary>
        public static string GetAudioFileFilter()
        {
            var extensions = string.Join(";", SupportedExtensions.Select(ext => $"*{ext}"));
            return $"Archivos de Audio|{extensions}|Todos los archivos|*.*";
        }

        /// <summary>
        /// Valida que un archivo existe y es accesible
        /// </summary>
        public static bool ValidateAudioFile(string filePath, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(filePath))
            {
                errorMessage = "La ruta del archivo está vacía";
                return false;
            }

            if (!File.Exists(filePath))
            {
                errorMessage = "El archivo no existe";
                return false;
            }

            if (!IsAudioFile(filePath))
            {
                errorMessage = "El formato de archivo no es compatible";
                return false;
            }

            try
            {
                // Verificar que el archivo no esté en uso
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Si llegamos aquí, el archivo es accesible
                }
            }
            catch (UnauthorizedAccessException)
            {
                errorMessage = "No tienes permisos para acceder al archivo";
                return false;
            }
            catch (IOException)
            {
                errorMessage = "El archivo está siendo usado por otra aplicación";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error al acceder al archivo: {ex.Message}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Obtiene información básica del archivo de audio
        /// </summary>
        public static AudioFileInfo GetAudioFileInfo(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var fileInfo = new FileInfo(filePath);

            return new AudioFileInfo
            {
                FileName = fileInfo.Name,
                FilePath = fileInfo.FullName,
                FileSize = fileInfo.Length,
                LastModified = fileInfo.LastWriteTime,
                Extension = fileInfo.Extension.ToLowerInvariant()
            };
        }
    }

    /// <summary>
    /// Información básica de un archivo de audio
    /// </summary>
    public class AudioFileInfo
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime LastModified { get; set; }
        public string Extension { get; set; }

        public string FormattedFileSize
        {
            get
            {
                if (FileSize < 1024)
                    return $"{FileSize} B";
                else if (FileSize < 1024 * 1024)
                    return $"{FileSize / 1024:F1} KB";
                else
                    return $"{FileSize / (1024 * 1024):F1} MB";
            }
        }
    }
}