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

                using (var steamKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam")) {

                    if (steamKey == null)
                        throw new Exception("Unable to locate Steam installation.");

                    var steamPath = steamKey.GetValue("SteamPath");
                    if (steamPath == null)
                        throw new Exception("Unable to locate 'SteamPath' using the specified installation.");

                    var steamInstall = steamPath.ToString();
                    if (string.IsNullOrEmpty(steamInstall) || Directory.Exists(steamInstall) == false)
                        throw new Exception("Unable to locate directory using the specified 'SteamPath'.");

                    var steamTarget = Path.Combine(steamInstall, @"steamapps\common\Halo The Master Chief Collection");
                    if (Directory.Exists(steamTarget) == false)
                        throw new Exception("Unable to locate directory for 'Halo: The Master Chief Collection'.");

                    Process.Start(steamTarget);

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