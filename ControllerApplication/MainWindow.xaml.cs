using System;
using System.Windows;
using System.Threading.Tasks;
using Windows.Media.Control;
using NAudio.CoreAudioApi;
using spotify_controller_project;

namespace ControllerApplication
{
    public partial class MainWindow : Window
    {
        private GlobalSystemMediaTransportControlsSessionManager _mediaManager;
        private bool _continueUpdating = true;
        private MMDevice _audioDevice;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMediaManager();
            InitializeAudioDevice();
            StartBackgroundMediaUpdateTask();
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

        private void InitializeAudioDevice()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            _audioDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            VolumeSlider.Value = _audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
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

        private void StartBackgroundMediaUpdateTask()
        {
            Task.Run(async () =>
            {
                while (_continueUpdating)
                {
                    UpdateCurrentSong();
                    await Task.Delay(1000); // Update every second
                }
            });
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
            if (_audioDevice != null)
            {
                _audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)(e.NewValue / 100.0);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _continueUpdating = false;
            base.OnClosed(e);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the LoginWindow and show it as a dialog
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            // After the LoginWindow is closed, check the AccessToken
            if (!string.IsNullOrEmpty(loginWindow.AccessToken))
            {
                MessageBox.Show("Login successful!");
            }
            else
            {
                MessageBox.Show("Login failed or was cancelled.");
            }
        }
    }
}
