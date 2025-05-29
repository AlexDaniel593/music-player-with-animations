using MusicPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPlayer
{
    public class PlaylistManager
    {
        private List<AudioTrack> tracks = new List<AudioTrack>();
        private int currentTrackIndex = -1;
        private Random random = new Random();

        public event EventHandler<AudioTrack> TrackChanged;
        public event EventHandler PlaylistUpdated;

        public List<AudioTrack> Tracks => new List<AudioTrack>(tracks);
        public int CurrentTrackIndex => currentTrackIndex;
        public AudioTrack CurrentTrack => currentTrackIndex >= 0 && currentTrackIndex < tracks.Count
            ? tracks[currentTrackIndex] : null;
        public int TrackCount => tracks.Count;
        public bool HasTracks => tracks.Count > 0;

        public bool Shuffle { get; set; } = false;
        public bool Repeat { get; set; } = false;

        public void AddTrack(string filePath)
        {
            if (FileHelper.ValidateAudioFile(filePath, out string error))
            {
                var track = new AudioTrack
                {
                    FilePath = filePath,
                    FileName = Path.GetFileNameWithoutExtension(filePath),
                    Extension = Path.GetExtension(filePath).ToLowerInvariant(),
                    FileSize = new FileInfo(filePath).Length,
                    AddedDate = DateTime.Now
                };

                tracks.Add(track);

                // Si es la primera canción, hacerla actual
                if (tracks.Count == 1)
                {
                    currentTrackIndex = 0;
                    OnTrackChanged(track);
                }

                OnPlaylistUpdated();
            }
        }

        public void AddTracks(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                AddTrack(filePath);
            }
        }

        public void RemoveTrack(int index)
        {
            if (index >= 0 && index < tracks.Count)
            {
                tracks.RemoveAt(index);

                // Ajustar índice actual si es necesario
                if (currentTrackIndex == index)
                {
                    if (tracks.Count == 0)
                    {
                        currentTrackIndex = -1;
                        OnTrackChanged(null);
                    }
                    else if (currentTrackIndex >= tracks.Count)
                    {
                        currentTrackIndex = tracks.Count - 1;
                        OnTrackChanged(CurrentTrack);
                    }
                    else
                    {
                        OnTrackChanged(CurrentTrack);
                    }
                }
                else if (currentTrackIndex > index)
                {
                    currentTrackIndex--;
                }

                OnPlaylistUpdated();
            }
        }

        public void ClearPlaylist()
        {
            tracks.Clear();
            currentTrackIndex = -1;
            OnTrackChanged(null);
            OnPlaylistUpdated();
        }

        public bool SelectTrack(int index)
        {
            if (index >= 0 && index < tracks.Count)
            {
                currentTrackIndex = index;
                OnTrackChanged(tracks[index]);
                return true;
            }
            return false;
        }

        public AudioTrack GetNextTrack()
        {
            if (tracks.Count == 0) return null;

            if (Shuffle)
            {
                // Modo aleatorio
                var nextIndex = random.Next(tracks.Count);
                currentTrackIndex = nextIndex;
            }
            else
            {
                // Modo secuencial
                currentTrackIndex++;
                if (currentTrackIndex >= tracks.Count)
                {
                    if (Repeat)
                    {
                        currentTrackIndex = 0;
                    }
                    else
                    {
                        currentTrackIndex = tracks.Count - 1;
                        return null; // Fin de la playlist
                    }
                }
            }

            var nextTrack = tracks[currentTrackIndex];
            OnTrackChanged(nextTrack);
            return nextTrack;
        }

        public AudioTrack GetPreviousTrack()
        {
            if (tracks.Count == 0) return null;

            if (Shuffle)
            {
                // En modo aleatorio, ir a una canción aleatoria
                var prevIndex = random.Next(tracks.Count);
                currentTrackIndex = prevIndex;
            }
            else
            {
                // Modo secuencial
                currentTrackIndex--;
                if (currentTrackIndex < 0)
                {
                    if (Repeat)
                    {
                        currentTrackIndex = tracks.Count - 1;
                    }
                    else
                    {
                        currentTrackIndex = 0;
                        return null;
                    }
                }
            }

            var prevTrack = tracks[currentTrackIndex];
            OnTrackChanged(prevTrack);
            return prevTrack;
        }

        public void MoveTrack(int fromIndex, int toIndex)
        {
            if (fromIndex >= 0 && fromIndex < tracks.Count &&
                toIndex >= 0 && toIndex < tracks.Count && fromIndex != toIndex)
            {
                var track = tracks[fromIndex];
                tracks.RemoveAt(fromIndex);
                tracks.Insert(toIndex, track);

                // Ajustar índice actual
                if (currentTrackIndex == fromIndex)
                {
                    currentTrackIndex = toIndex;
                }
                else if (fromIndex < currentTrackIndex && toIndex >= currentTrackIndex)
                {
                    currentTrackIndex--;
                }
                else if (fromIndex > currentTrackIndex && toIndex <= currentTrackIndex)
                {
                    currentTrackIndex++;
                }

                OnPlaylistUpdated();
            }
        }

        private void OnTrackChanged(AudioTrack track)
        {
            TrackChanged?.Invoke(this, track);
        }

        private void OnPlaylistUpdated()
        {
            PlaylistUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public class AudioTrack
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public DateTime AddedDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }

        public string DisplayName => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Artist)
            ? $"{Artist} - {Title}"
            : FileName;

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

        public string FormattedDuration => Duration != TimeSpan.Zero
            ? $"{(int)Duration.TotalMinutes:D2}:{Duration.Seconds:D2}"
            : "--:--";
    }
}