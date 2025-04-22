using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokeHelper.Classes
{
    public static class MusicManager
    {
        private static MediaPlayer _player = new MediaPlayer();

        // Set the volume (0 to 1)
        public static void SetVolume(double value)
        {
            _player.Volume = value / 500;
        }

        // Play a music file from a path
        public static void Play(string filePath, bool loop = true)
        {
            _player.Open(new Uri(filePath, UriKind.RelativeOrAbsolute));
            _player.MediaEnded += (s, e) =>
            {
                if (loop)
                {
                    _player.Position = TimeSpan.Zero;
                    _player.Play();
                }
            };
            _player.Play();
        }

        public static void Pause()
        {
            _player.Pause();
        }

        public static void Stop()
        {
            _player.Stop();
        }

    }
}
