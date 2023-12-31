﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Tamarillo.Classes.Entities.Maps;
using static Tamarillo.Classes.Helpers.Paths;
using MessageBox = System.Windows.MessageBox;

namespace Tamarillo.Classes.Helpers {

    [DataContract]
    public class Config {

        static Config() {
            Instance = new Config();
        }

        [IgnoreDataMember]
        internal static Config Instance {
            get;
            set;
        }

        [DataMember(IsRequired = true, Name = "path", Order = 0)]
        public string Root {
            get; set;
        }
        [DataMember(IsRequired = true, Name = "rename_prompt", Order = 1)]
        public bool PromptToRename {
            get; set;
        }
        [DataMember(IsRequired = true, Name = "installed_maps", Order = 2)]
        public List<MapFile> InstalledMaps {
            get; set;
        }
        [DataMember(IsRequired = true, Name = "uninstalled_maps", Order = 3)]
        public List<MapFile> UninstalledMaps {
            get; set;
        }

        public bool Load() {
            try {

                if (File.Exists(Settings)) {

                    var serializer = new DataContractJsonSerializer(typeof(Config));
                    using (var reader = new FileStream(Settings, FileMode.Open, FileAccess.Read)) {
                        if (!(serializer.ReadObject(reader) is Config temp))
                            return false;
                        else {
                            Root = temp.Root;
                            PromptToRename = temp.PromptToRename;
                            InstalledMaps = temp.InstalledMaps;
                            UninstalledMaps = temp.UninstalledMaps;
                        }
                    }

                }
                else {

                    Root = "";
                    PromptToRename = true;
                    InstalledMaps = new List<MapFile>();
                    UninstalledMaps = new List<MapFile>();

                    return Save();

                }

                return true;

            }
            catch {
                return false;
            }
        }
        public bool Save() {
            try {

                var serializer = new DataContractJsonSerializer(typeof(Config));
                using (var stream = new FileStream(Settings, FileMode.Create, FileAccess.Write))
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, false, true))
                    serializer.WriteObject(writer, this);

                return true;

            }
            catch {
                return false;
            }
        }

        public static void AddInstalledMap(MapFile map) {
            try {

                if (Instance.InstalledMaps.Contains(map))
                    return;

                map.LastInstalled = DateTime.UtcNow.Millisecond;
                Instance.InstalledMaps.Add(map);

            }
            catch (Exception ex) {
                throw ex;
            }
        }
        public static void RemoveInstalledMap(MapFile map) {
            try {

                if (Instance.UninstalledMaps.Contains(map) == false)
                    return;

                map.LastUninstalled = DateTime.UtcNow.Millisecond;
                Instance.InstalledMaps.Add(map);

            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static DirectoryInfo GetInstallDirectory() {
            try {

                if (string.IsNullOrEmpty(Instance.Root) || Directory.Exists(Instance.Root) == false) {

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
                                MessageBox.Show("User canceled! Installation selection is required! The application will now exit.", "Installation", MessageBoxButton.OK, MessageBoxImage.Error);
                                Environment.Exit(0);
                            }
                            else {
                                if (Directory.GetFiles(FBD.SelectedPath, "*.map", SearchOption.AllDirectories).Length <= 0) {

                                    if (MessageBox.Show("The selected path did not contain any required files. Would you like to try again?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Error) != MessageBoxResult.Yes) {
                                        MessageBox.Show("User canceled! Installation selection is required! The application will now exit", "Installation", MessageBoxButton.OK, MessageBoxImage.Error);
                                        Environment.Exit(0);
                                    }
                                    else
                                        goto SELECT_PATH;

                                }
                                else {

                                    Instance.Root = FBD.SelectedPath;
                                    Instance.Save();

                                }

                            }

                        }

                    }
                    else {

                        Instance.Root = path;
                        Instance.Save();

                    }

                }

                return new DirectoryInfo(Instance.Root);

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