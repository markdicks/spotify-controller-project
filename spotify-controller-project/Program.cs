using System;
using System.Windows;
using DotNetEnv;

namespace spotify_controller_project
{
    internal static class Program
    {
        [STAThread] // Required attribute for WPF applications
        static void Main()
        {
            // Load .env file into environment variables
            DotNetEnv.Env.Load();

            var app = new Application();

            var mainWindow = new MainWindow();

            app.MainWindow = mainWindow;

            app.Run(mainWindow);
        }
    }
}
