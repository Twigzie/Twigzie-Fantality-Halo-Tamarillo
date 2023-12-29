using System;
using System.Windows;
using System.Windows.Threading;
using Tamarillo.Classes.Helpers;
using Tamarillo.Controls.Windows.Dialogs;

namespace Tamarillo.App {
    public partial class Base : Application {

        private void OnStartup(object sender, StartupEventArgs e) {
            try {

                Config.Instance.Load();

                if (Config.Instance.PromptToRename) {
                    if (new Rename().ShowDialog() == false) {
                    }
                    else {
                    }
                }

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