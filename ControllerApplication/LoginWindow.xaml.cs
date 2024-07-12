using Microsoft.VisualBasic;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace ControllerApplication
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
            string twitchRedirectUri = Properties.Settings.Default.TwitchRedirectUri;
            string twitchClientId = Properties.Settings.Default.TwitchClientId;

            if (string.IsNullOrEmpty(twitchRedirectUri))
            {
                twitchRedirectUri = Interaction.InputBox("Enter Twitch Redirect URI:", "Input Required", "http://localhost");
                Properties.Settings.Default.TwitchRedirectUri = twitchRedirectUri;
                Properties.Settings.Default.Save(); // Save the setting
            }

            if (string.IsNullOrEmpty(twitchClientId))
            {
                twitchClientId = Interaction.InputBox("Enter Twitch Client ID:", "Input Required", "");
                Properties.Settings.Default.TwitchClientId = twitchClientId;
                Properties.Settings.Default.Save(); // Save the setting
            }

            if (string.IsNullOrEmpty(twitchRedirectUri) || string.IsNullOrEmpty(twitchClientId))
            {
                MessageBox.Show("Twitch Redirect URI and Client ID are required to proceed.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
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
