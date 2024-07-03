using System;
using System.Windows;
using Windows.Media.Control;


namespace ControllerApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private GlobalSystemMediaTransportControlsSessionManager _mediaManager;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMediaManager();
        }

        private async void InitializeMediaManager()
        {
            try
            {
                _mediaManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                _mediaManager.CurrentSessionChanged += MediaManager_CurrentSessionChanged;
                UpdateCurrentSong();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    CurrentSongTextBlock.Text = $"Error: {ex.Message}";
                });
            }
        }

        private void MediaManager_CurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
        {
            UpdateCurrentSong();
        }

        private async void UpdateCurrentSong()
        {
            var currentSession = _mediaManager.GetCurrentSession();
            if (currentSession != null)
            {
                var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();
                Dispatcher.Invoke(() =>
                {
                    CurrentSongTextBlock.Text = $"Currently Playing: {mediaProperties.Title} by {mediaProperties.Artist}";
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CurrentSongTextBlock.Text = "Currently Playing: None";
                });
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var currentSession = _mediaManager.GetCurrentSession();
            currentSession?.TryPlayAsync();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var currentSession = _mediaManager.GetCurrentSession();
            currentSession?.TryPauseAsync();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var currentSession = _mediaManager.GetCurrentSession();
            currentSession?.TrySkipNextAsync();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            var currentSession = _mediaManager.GetCurrentSession();
            currentSession?.TrySkipPreviousAsync();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Volume control logic implementation
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Login functionality is not needed for local media control.");
        }
    }
}
