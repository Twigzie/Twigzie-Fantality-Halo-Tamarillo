using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace Tamarillo.Classes.Helpers {

    public static class Steam {

        public static DirectoryInfo GetInstallDirectory() {
            try {

                if (string.IsNullOrEmpty(Config.Instance.Path) || Directory.Exists(Config.Instance.Path) == false) {

                    MessageBox.Show("Steam installation of Halo:MCC was not specified! Attempting to automatically locate steam installation...", "Installation", MessageBoxButton.OK, MessageBoxImage.Information);

                    var path = GetGameInstallPath();
                    if (string.IsNullOrEmpty(path) || Directory.Exists(path) == false) {

                        MessageBox.Show("Failed to automatically locate steam installation! Please locate your installation manually.", "Installation", MessageBoxButton.OK, MessageBoxImage.Information);

                        SELECT_PATH:
                        using (var FBD = new FolderBrowserDialog()) {
                            FBD.Description = "Select the root folder of your Steam installtion of Halo:MCC (ex: steamapps\\common\\Halo The Master Chief Collection)";
                            FBD.ShowNewFolderButton = false;
                            FBD.RootFolder = Environment.SpecialFolder.Desktop;
                            if (FBD.ShowDialog() != DialogResult.OK) {
                                MessageBox.Show("User canceled! Installation is required! The application will now exit.", "Installation", MessageBoxButton.OK, MessageBoxImage.Error);
                                Environment.Exit(0);
                            }
                            else {
                                if (Directory.GetFiles(FBD.SelectedPath, "*.map", SearchOption.AllDirectories).Length <= 0) {

                                    if (MessageBox.Show("The selected path did not contain any required files. Would you like to try again?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Error) != MessageBoxResult.Yes) {
                                        MessageBox.Show("User canceled! Installation is required! The application will now exit", "Installation", MessageBoxButton.OK, MessageBoxImage.Error);
                                        Environment.Exit(0);
                                    }
                                    else
                                        goto SELECT_PATH;

                                }
                                else {

                                    Config.Instance.Path = FBD.SelectedPath;
                                    Config.Instance.Save();

                                }

                            }

                        }

                    }
                    else
                    {

                        Config.Instance.Path = path;
                        Config.Instance.Save();
                        
                    }

                }
                
                return new DirectoryInfo(Config.Instance.Path);

            }
            catch (Exception ex) {
                throw ex;
            }

        }
        private static string GetGameInstallPath() {

            try {
                var path = Path.Combine("", Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string);
                if (path == null)
                    return null;

                return Path.Combine(path, "steamapps\\common\\Halo The Master Chief Collection");
            }
            catch {
                return null;
            }
        }

    }

}
