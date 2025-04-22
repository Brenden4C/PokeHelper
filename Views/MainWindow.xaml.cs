using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using PokeHelper.Classes;

namespace PokeHelper.Views 

{
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            MainFrame.Navigate(new HomePage());
        }


        /*
         * ===========================
         * Music Handling Methods
         * ===========================
         */

        public void PlayMusic(string fileName)
        {
            string musicDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Music");
            string musicPath = Path.GetFullPath(Path.Combine(musicDirectory, fileName));

            // Default music fallback
            string defaultMusicPath = Path.GetFullPath(Path.Combine(musicDirectory, "default.mp3"));

            // Use fallback if file doesn't exist
            if (!File.Exists(musicPath))
            {
                musicPath = defaultMusicPath;
            }

            MusicManager.Play(musicPath, true);
        }



        public void StopBackgroundMusic()
        {
            MusicManager.Stop();
        }

        public void SetMusicVolume(double volume)
        {
            MusicManager.SetVolume(volume);
        }
    }
}
