using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace ControllerApplication
{
    public partial class LoginWindow : Window
    {
        public string AccessToken { get; private set; }
        private TwitchCallbackListener _callbackListener;

        public LoginWindow()
        {
            InitializeComponent();

            // Initialize the HTTP listener
            string redirectUri = Properties.Settings.Default.TwitchRedirectUri;
            _callbackListener = new TwitchCallbackListener(redirectUri, OnAccessTokenReceived);
            _callbackListener.Start();
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
            string twitchRedirectUri = Properties.Settings.Default.TwitchRedirectUri; // Ensure this is 'http://localhost:3000/'
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
                // Modify to launch the browser in incognito mode, if supported
                startInfo.Arguments = "--incognito"; // For Chrome
                                                     // For Firefox or other browsers, use their specific arguments if needed
                                                     // startInfo.Arguments = "-private-window"; // For Firefox
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
