using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Tamarillo.Classes.Helpers;

namespace Tamarillo.App {
    public partial class Base : Application {

        private void OnStartup(object sender, StartupEventArgs e) {
            try {

                Config.Instance.Load();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Tamarillo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Environment.Exit(0);
            }
        }
        private void OnException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            try {

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Tamarillo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Environment.Exit(0);
            }
        }

    }

}