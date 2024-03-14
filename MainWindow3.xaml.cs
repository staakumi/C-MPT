using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PraktosikCSharp3
{
    
    public partial class MainWindow : Window
    {
        public string SelectedTrack { get; private set; }

        public HistoryWindow(List<string> playlist)
        {
            InitializeComponent();
            PopulateHistory(playlist);
        }

        private void PopulateHistory(List<string> playlist)
        {
            foreach (string track in playlist)
            {
                Button button = new Button
                {
                    Content = System.IO.Path.GetFileName(track),
                    Tag = track,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                button.Click += (sender, e) =>
                {
                    SelectedTrack = (sender as Button).Tag.ToString();
                    DialogResult = true;
                };
                historyPanel.Children.Add(button);
            }
        }

        private MediaElement mediaElement2 = new MediaElement();
        private List<string> playlist = new List<string>();
        private int currentTrackIndex = 0;
        private bool isPlaying = false;
        private bool isRepeatMode = false;
        private bool isShuffleMode = false;
        private Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
            InitializeMediaPlayer();
            LoadMusicFromFolder();
        }
        private void InitializeMediaPlayer()
        {
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.MediaEnded += (sender, e) => PlayNextTrack();
            sliderPosition.ValueChanged += (sender, e) => UpdateTrackPosition();
            sliderVolume.ValueChanged += (sender, e) => UpdateVolume();
        }
        private async void LoadMusicFromFolder()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio files (*.mp3, *.wav, *.m4a)|*.mp3;*.wav;*.m4a";
            if (openFileDialog.ShowDialog() == true)
            {
                string directoryPath = Path.GetDirectoryName(openFileDialog.FileName);
                foreach (string file in Directory.GetFiles(directoryPath).Where(file => file.EndsWith(".mp3") || file.EndsWith(".wav") || file.EndsWith(".m4a")))
                {
                    playlist.Add(file);
                }
                await PlayTrack(playlist.First());
            }
        }
        private async Task PlayTrack(string trackPath)
        {
            mediaElement.Source = new Uri(trackPath);
            mediaElement.Play();
            isPlaying = true;
        }
        private void PlayNextTrack()
        {
            if (isRepeatMode)
            {
                PlayTrack(playlist[currentTrackIndex]);
            }
            else
            {
                currentTrackIndex = isShuffleMode ? random.Next(playlist.Count) : (currentTrackIndex + 1) % playlist.Count;
                PlayTrack(playlist[currentTrackIndex]);
            }
        }
        private void UpdateTrackPosition()
        {
            mediaElement.Position = TimeSpan.FromSeconds(sliderPosition.Value);
        }
        private void UpdateVolume()
        {
            mediaElement.Volume = sliderVolume.Value;
        }
        private void ButtonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                mediaElement.Pause();
                isPlaying = false;
            }
            else
            {
                mediaElement.Play();
                isPlaying = true;
            }
        }
        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;
            PlayTrack(playlist[currentTrackIndex]);
        }
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            currentTrackIndex = isShuffleMode ? random.Next(playlist.Count) : (currentTrackIndex + 1) % playlist.Count;
            PlayTrack(playlist[currentTrackIndex]);
        }
        private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
        {
            isRepeatMode = !isRepeatMode;
        }
        private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
        {
            isShuffleMode = !isShuffleMode;
        }
        private async void ShowListeningHistory()
        {
            HistoryWindow historyWindow = new HistoryWindow(playlist);
            if (historyWindow.ShowDialog() == true)
            {
                await PlayTrack(historyWindow.SelectedTrack);
            }
        }
        private async Task UpdateTrackPositionThread()
        {
            while (true)
            {
                await Task.Delay(1000);
                sliderPosition.Dispatcher.Invoke(() => { sliderPosition.Value = mediaElement.Position.TotalSeconds; });
            }
        }
        private async Task UpdateTimerThread()
        {
            while (true)
            {
                await Task.Delay(1000);
                TimeSpan remainingTime = mediaElement.NaturalDuration.TimeSpan - mediaElement.Position;
                timerLabel.Dispatcher.Invoke(() => { timerLabel.Text = $"Remaining: {remainingTime:mm\\:ss}"; });
                currentSecondLabel.Dispatcher.Invoke(() => { currentSecondLabel.Text = $"Current: {mediaElement.Position:mm\\:ss}"; });
            }
        }
        private void ButtonChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            LoadMusicFromFolder();
        }
        private void ButtonShowHistory_Click(object sender, RoutedEventArgs e)
        {
            ShowListeningHistory();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            mediaElement.Stop();
        }
        private void InitializeThreads()
        {
            Task.Run(() => UpdateTrackPositionThread());
            Task.Run(() => UpdateTimerThread());
        }

    }
}
