using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace BackgroundService.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon? _notifyIcon;

        public App()
        {
            NamedPipesClient.Instance.InitializeAsync().ContinueWith(r => 
                MessageBox.Show($"Error while connecting to pipe server: {r.Exception}"),
                TaskContinuationOptions.OnlyOnFaulted);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NamedPipesClient.Instance.Dispose();
            _notifyIcon?.Dispose();
            base.OnExit(e);
        }
    }
}