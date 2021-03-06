﻿using System.Windows;
using bytePassion.CodeLineCounter.ViewModel;

namespace bytePassion.CodeLineCounter
{

    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindowViewModel = new MainWindowViewModel();
            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };

            mainWindow.Show();            
        }
    }
}
