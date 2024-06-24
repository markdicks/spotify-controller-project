using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using System.Net;

namespace spotify_controller_project
{
    public partial class MainWindow : Window
    {
        private bool isLoggedIn = false;
        private string accessToken;
        private DispatcherTimer playbackTimer;
        private DispatcherTimer songUpdateTimer;
        private const string SpotifyLocalServerUrl = "https://127.0.0.1:8800/";

        public MainWindow()
        {
            InitializeComponent();
            LoadToken();
            UpdateLoginButton();
            FetchCurrentlyPlayingTrack();
        }



        private void LoadToken()
        {
            accessToken = Properties.Settings.Default.AccessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                isLoggedIn = true;
            }
        }

        private void SaveToken(string token)
        {
            Properties.Settings.Default.AccessToken = token;
            Properties.Settings.Default.Save();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (isLoggedIn)
            {
                Logout();
            }
            else
            {
                var loginWindow = new LoginWindow();
                if (loginWindow.ShowDialog() == true)
                {
                    accessToken = loginWindow.AccessToken;
                    isLoggedIn = true;
                    SaveToken(accessToken);
                }
            }
            UpdateLoginButton();
        }

        private void Logout()
        {
            accessToken = string.Empty;
            isLoggedIn = false;
            Properties.Settings.Default.AccessToken = string.Empty;
            Properties.Settings.Default.Save();
            UpdateLoginButton();
        }

        private void UpdateLoginButton()
        {
            LoginButton.Content = isLoggedIn ? "Logout" : "Login";
        }

        private async Task FetchCurrentlyPlayingTrack()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Example: GET request to fetch currently playing track
                    var response = await client.GetAsync($"{SpotifyLocalServerUrl}/current-track");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        // Parse content to get track information
                        var trackInfo = ParseTrackInfo(content);

                        // Example: Display track information
                        if (trackInfo != null)
                        {
                            string artist = trackInfo.Artist;
                            string trackName = trackInfo.Name;
                            MessageBox.Show($"Currently Playing: {trackName} by {artist}");
                        }
                        else
                        {
                            MessageBox.Show("No track currently playing.");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to fetch currently playing track: {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private TrackInfo ParseTrackInfo(string content)
        {
            // Example method to parse JSON response to track info object
            // Implement based on your local server's response structure
            // Example assumes JSON structure like: { "artist": "Artist Name", "name": "Track Name" }
            try
            {
                dynamic trackJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
                if (trackJson != null)
                {
                    return new TrackInfo
                    {
                        Artist = trackJson.artist,
                        Name = trackJson.name
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to parse track info: {ex.Message}");
            }
            return null;
        }

        // Example class to represent track information
        private class TrackInfo
        {
            public string Artist { get; set; }
            public string Name { get; set; }
        }

        private async Task SendSpotifyLocalRequest(string command)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:4380/{command}");
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode(); // Ensure HTTP 200 OK response

                // Optionally, you can read the response content if needed
                // var responseContent = await response.Content.ReadAsStringAsync();
            }
        }

        private void StartPlaybackTimer()
        {
            if (playbackTimer == null)
            {
                playbackTimer = new DispatcherTimer();
                playbackTimer.Interval = TimeSpan.FromSeconds(1); // Update every second
                playbackTimer.Tick += PlaybackTimer_Tick;
            }

            playbackTimer.Start();
        }

        private void StopPlaybackTimer()
        {
            playbackTimer?.Stop();
        }

        private async void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:4380/status");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var statusInfo = JsonConvert.DeserializeObject<dynamic>(content);

                    // Update slider position based on current playback position
                    double progress = (double)statusInfo.position * 100 / (double)statusInfo.duration;
                    PlaybackSlider.Value = progress;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update playback slider: {ex.Message}");
                StopPlaybackTimer(); // Stop timer on error to prevent continuous updates
            }
        }

        private async Task UpdateCurrentSong()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:4380/track");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var trackInfo = JsonConvert.DeserializeObject<dynamic>(content);

                    // Update the label with the current track name (assuming CurrentSongLabel is a Label control)
                    Dispatcher.Invoke(() =>
                    {
                        CurrentSongLabel.Content = $"Currently Playing: {trackInfo.track_name}";
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get current song: {ex.Message}");
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SendSpotifyLocalRequest("play");
                await UpdateCurrentSong();
                StartPlaybackTimer(); // Start timer to update playback slider
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to play: {ex.Message}");
            }
        }

        private async void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Send pause request to Spotify local server
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsync("http://localhost:4380/pause", null);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to pause: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SendSpotifyLocalRequest("next");
                await UpdateCurrentSong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to skip to next track: {ex.Message}");
            }
        }

        private async void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SendSpotifyLocalRequest("previous");
                await UpdateCurrentSong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to skip to previous track: {ex.Message}");
            }
        }

        private async void PlaybackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                // Get current track duration asynchronously
                long trackDuration = await GetCurrentTrackDuration();

                // Calculate the new position in milliseconds based on the slider value
                var newPosition = (long)((e.NewValue / 100) * trackDuration);

                // Send seek request to Spotify local server
                await SeekPlayback(newPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to seek playback: {ex.Message}");
            }
        }

        private async Task SeekPlayback(long newPosition)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsync($"http://localhost:4380/seek?position_ms={newPosition}", null);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to seek playback: {ex.Message}");
            }
        }

        private async Task<long> GetCurrentTrackDuration()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:4380/status");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var statusInfo = JsonConvert.DeserializeObject<dynamic>(content);

                    return (long)statusInfo.duration;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get current track duration: {ex.Message}");
                return 0;
            }
        }

    }
}
