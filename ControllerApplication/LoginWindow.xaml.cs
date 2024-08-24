using System;
using System.Diagnostics;
using System.Windows;

namespace ControllerApplication
{
    public partial class LoginWindow : Window
    {
        public string AccessToken { get; private set; }
        private TwitchCallbackListener _callbackListener;

        public LoginWindow()
        {
            InitializeComponent();

            // Check if TwitchRedirectUri and TwitchClientId are set
            if (string.IsNullOrEmpty(Properties.Settings.Default.TwitchRedirectUri) ||
                string.IsNullOrEmpty(Properties.Settings.Default.TwitchClientId))
            {
                PromptForTwitchSettings();
            }

            // Initialize the HTTP listener
            string redirectUri = Properties.Settings.Default.TwitchRedirectUri;
            _callbackListener = new TwitchCallbackListener(redirectUri, OnAccessTokenReceived);
            _callbackListener.Start();
        }

        private void PromptForTwitchSettings()
        {
            // Prompt the user for the missing information
            string twitchRedirectUri = PromptUserForInput("Enter the Twitch Redirect URI:");
            string twitchClientId = PromptUserForInput("Enter the Twitch Client ID:");

            if (string.IsNullOrEmpty(twitchRedirectUri) || string.IsNullOrEmpty(twitchClientId))
            {
                MessageBox.Show("Both Twitch Redirect URI and Client ID are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close(); // Close the window if the user doesn't provide the necessary information
            }
            else
            {
                // Save the values to settings
                Properties.Settings.Default.TwitchRedirectUri = twitchRedirectUri;
                Properties.Settings.Default.TwitchClientId = twitchClientId;
                Properties.Settings.Default.Save();
            }
        }

        private string PromptUserForInput(string message)
        {
            // Simple input dialog implementation (could be replaced with a custom input dialog)
            InputDialog inputDialog = new InputDialog(message);
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.Input;
            }
            return null;
        }

        private void LoginWithTwitch_Default_Click(object sender, RoutedEventArgs e)
        {
            OpenTwitchLoginPage("default");
        }

        private void LoginWithTwitch_Private_Click(object sender, RoutedEventArgs e)
        {
            OpenTwitchLoginPage("private");
        }

        private void OpenTwitchLoginPage(string browserType)
        {
            string twitchRedirectUri = Properties.Settings.Default.TwitchRedirectUri;
            string twitchClientId = Properties.Settings.Default.TwitchClientId;

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

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = twitchAuthUrl,
                UseShellExecute = true
            };

            if (browserType == "private")
            {
                startInfo.Arguments = "--incognito"; // For Chrome
            }

            Process.Start(startInfo);
        }

        private void OnAccessTokenReceived(string accessToken)
        {
            AccessToken = accessToken;

            // Save the access token to settings
            Properties.Settings.Default.TwitchAccessToken = AccessToken;
            Properties.Settings.Default.Save();

            // Close the window after retrieving access token
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Login successful! Access token stored.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            // Ensure the listener is stopped when the window is closed
            _callbackListener.Stop();
            base.OnClosed(e);
        }
    }
}
