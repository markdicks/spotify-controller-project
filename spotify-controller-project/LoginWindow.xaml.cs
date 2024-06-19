using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace spotify_controller_project
{
    public partial class LoginWindow : Window
    {
        public string AccessToken { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginWithTwitch_Click(object sender, RoutedEventArgs e)
        {
            // Open Twitch login page in default browser
            OpenTwitchLoginPage();
        }

        private void OpenTwitchLoginPage()
        {
            // Retrieve Twitch API credentials from configuration or environment variables
            string twitchRedirectUri = Environment.GetEnvironmentVariable("TWITCH_REDIRECT_URI");
            string twitchClientId = Environment.GetEnvironmentVariable("TWITCH_CLIENT_ID");

            if (string.IsNullOrEmpty(twitchRedirectUri) || string.IsNullOrEmpty(twitchClientId))
            {
                MessageBox.Show("Please ensure you have filled in the environment variables to use this feature.", "Missing Environment Variables", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string twitchAuthUrl = $"https://id.twitch.tv/oauth2/authorize" +
                $"?client_id={twitchClientId}" +
                $"&redirect_uri={Uri.EscapeDataString(twitchRedirectUri)}" +
                $"&response_type=token" +
                $"&scope=viewing_activity_read";

            OpenUrlInBrowser(twitchAuthUrl);
        }

        private void OpenUrlInBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening browser: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Check if AccessToken is set
            if (!string.IsNullOrEmpty(AccessToken))
            {
                // Store AccessToken in application settings or use it as needed
                Properties.Settings.Default.AccessToken = AccessToken;
                Properties.Settings.Default.Save();
                MessageBox.Show("Login successful! Access token stored.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Handle navigation to capture access token from URL fragment
        private void BrowserNavigated(object sender, NavigationEventArgs e)
        {
            // Check if navigation URL contains access token
            string fragment = e.Uri.Fragment;
            if (!string.IsNullOrEmpty(fragment) && fragment.StartsWith("#access_token="))
            {
                // Extract access token from fragment
                AccessToken = fragment.Substring("#access_token=".Length);
                AccessToken = AccessToken.Split('&')[0]; // Remove any additional parameters

                // Close the window after retrieving access token
                Close();
            }
        }
    }
}
