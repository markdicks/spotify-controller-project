using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace spotify_controller_project
{
    public partial class MainWindow : Window
    {
        private bool isLoggedIn = false;
        private string accessToken;

        public MainWindow()
        {
            InitializeComponent();
            LoadToken();
            UpdateLoginButton();
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

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Spotify play logic
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            // Spotify pause logic
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Spotify next track logic
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // Spotify previous track logic
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Spotify volume control logic
        }
    }
}
