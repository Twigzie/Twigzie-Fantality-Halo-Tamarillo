using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tamarillo.Classes.Commands;
using Tamarillo.Classes.Helpers;
using Tamarillo.Classes.Models.Base;

namespace Tamarillo.Classes.Entities.Maps {

    public class MapSettings : Model {

        #region Private

        private int m_Count = 0;
        private object m_fileLock = new object();

        #endregion

        public int Count {
            get => m_Count;
            set => m_Count = value;
        }
        public string CountPrefix {
            get {
                if (m_Count <= 0)
                    return $"(0)";
                return $"({m_Count})";
            }
        }

        public Game Type {
            get;
            set;
        }
        public DirectoryInfo Root {
            get;
            set;
        }
        public ObservableCollection<MapFile> Maps {
            get;
            set;
        }

        public MapSettings() {
        }
        public MapSettings(Game type, string root) {

            Type = type;
            Root = new DirectoryInfo(root);
            Maps = new ObservableCollection<MapFile>();

            GetMapCount();

        }

        public ICommand RefreshMapsCommand => new RelayCommand(() => {
            try {
                GetMapCount();
                GetAvailableMaps();
            }
            catch (Exception ex) {
                throw ex;
            }
        });
        public ICommand ViewUninstalledMapsCommand => new RelayCommand(() => {
            try {

                if (Maps?.Count >= 1) {
                    foreach (var map in Maps) {
                        map.IsVisible = false;
                        if (map.IsRemoved)
                            map.IsVisible = true;
                    }
                    Maps = new ObservableCollection<MapFile>(Maps.OrderBy(map => map.Name));
                }

            }
            catch (Exception ex) {
                throw ex;
            }
        });
        public ICommand ViewInstalledMapsCommand => new RelayCommand(() => {
            try {

                if (Maps?.Count >= 1) {
                    foreach (var map in Maps) {
                        map.IsVisible = true;
                        if (map.IsRemoved)
                            map.IsVisible = false;
                    }
                    Maps = new ObservableCollection<MapFile>(Maps.OrderBy(map => map.Name));
                }

            }
            catch (Exception ex) {
                throw ex;
            }
        });
        public ICommand ViewAllMapsCommand => new RelayCommand(() => {
            try {

                if (Maps?.Count >= 1) {
                    foreach (var map in Maps)
                        map.IsVisible = true;
                    Maps = new ObservableCollection<MapFile>(Maps.OrderBy(map => map.Name).OrderBy(map => map.IsRemoved));
                }

            }
            catch (Exception ex) {
                throw ex;
            }
        });

        public ICommand LocateMapCommand => new RelayGenericParamCommand<MapFile>((map) => {
            try {
                if (File.Exists(map.Source.FullName) == false) {
                    MessageBox.Show($"Unable to locate the specified file!\r\n\nName: {map.Name}", "Oops", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else {
                    Process.Start("explorer.exe", $"/select, {map.Source}");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        });
        public ICommand UninstallMapCommand => new RelayGenericParamCommand<MapFile>((map) => {
            try {

                //Check if map exists
                if (File.Exists(map.Source.FullName) == false) {
                    MessageBox.Show($"Unable to locate the specified file!\r\n\nName: {map.Name}", "Oops", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //Confirm map removal
                if (MessageBox.Show($"Are you sure you want to remove the specified map?\r\n\nName: {map.Name}\r\n\nWarning: Any errors that occurr will need to be manually fixed, or re-downloaded via Steam, Continue?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;
                else {

                    var inputName = map.Source.Name;
                    var inputPath = map.Source.Directory.FullName;

                    var sourceName = Path.GetFileName(inputName);
                    var targetName = Path.Combine(inputPath, $"remove_{sourceName}");
                    var backupName = Path.Combine(inputPath, $"backup_{sourceName}");

                    var outputName = Path.Combine(inputPath, targetName);

                    //Ensure the Map is not installed already
                    if (File.Exists(outputName)) {
                        MessageBox.Show($"The specified Map is aready removed!\r\n\nName: {map.Name}", "Oops", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    //Check if a backup exists, if not, create one
                    if (!File.Exists(backupName)) {

                        MessageBox.Show($"The specified map does not have a backup, creating one!\r\n\nOld Name: {map.Source.FullName}\nNew Name: {backupName}", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);

                        lock (m_fileLock) {
                            try {
                                File.Copy(map.Source.FullName, backupName);
                            }
                            catch (IOException) {
                                MessageBox.Show($"There was an error creating a backup for the specified map!\r\n\nMake sure the file isnt being used and tray again.\r\n\nName:{map.Source.FullName}", "Oops", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                    }

                    if (MessageBox.Show($"Are you sure you want to uninstall the specified map?\r\n\nName: {map.Name}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                        return;
                    else {

                        var flag = false;
                        lock (m_fileLock) {
                            try {
                                File.Move(map.Source.FullName, outputName);
                                flag = true;
                            }
                            catch (IOException) {
                                flag = false;
                                MessageBox.Show($"There was an error uninstalling the specified map! Make sure the file isnt being used and tray again.\r\n\nName: {map.Name}","Oops",MessageBoxButton.OK,MessageBoxImage.Error);
                                return;
                            }
                        }

                        if (flag) {

                            MessageBox.Show($"The specified Map has been removed!\r\n\nName: {map.Name}", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);

                            map.IsRemoved = true;
                            RefreshMapsCommand.Execute(null);

                        }

                    }

                }

            }
            catch (Exception ex) {
                throw ex;
            }
        });
        public ICommand ReinstallMapCommand => new RelayGenericParamCommand<MapFile>((map) => {
            try {

                //Check if map exists
                if (File.Exists(map.Source.FullName) == false) {
                    MessageBox.Show($"Unable to locate the specified file!\r\n\nName: {map.Name}", "Oops", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //Confirm map restore
                if (MessageBox.Show($"Are you sure you want to restore the specified map?\r\n\nName: {map.Name}\r\n\nWarning: Any errors that occurr will need to be manually fixed, or re-downloaded via Steam, Continue?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;
                else {

                    var inputName = map.Source.Name;
                    var inputPath = map.Source.Directory.FullName;

                    var sourceName = Path.Combine(inputPath, inputName);
                    var targetName = Path.Combine(inputPath, inputName.Replace("remove_", ""));

                    var outputName = Path.Combine(inputPath, targetName);

                    //Ensure the Map is not installed already
                    if (File.Exists(outputName)) {
                        MessageBox.Show($"The specified Map is aready restored!\r\n\nName: {map.Name}", "Oops", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (MessageBox.Show($"Are you sure you want to restore the specified map?\r\n\nName: {map.Name}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                        return;
                    else {

                        var flag = false;
                        lock (m_fileLock) {
                            try {
                                File.Move(map.Source.FullName, outputName);
                                flag = true;
                            }
                            catch (IOException) {
                                flag = false;
                                MessageBox.Show($"There was an error restoring the specified map! Make sure the file isnt being used and tray again.\r\n\nName: {map.Name}", "Oops", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        if (flag) {

                            MessageBox.Show($"The specified Map has been restored!\r\n\nName: {map.Name}", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);

                            map.IsRemoved = false;
                            RefreshMapsCommand.Execute(null);

                        }

                    }

                }

            }
            catch (Exception ex) {
                throw ex;
            }
        });

        public void Initialize() {
            try {
                if (Count >= 1)
                    GetAvailableMaps();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw ex;
            }
        }
        private void GetMapCount() {
            try {
                Count = 0;
                foreach (var map in Directory.EnumerateFiles(Root.FullName, "*.map", SearchOption.TopDirectoryOnly).Where(file => MapInfo.FilteredMaps.Contains(Path.GetFileName(file)) == false)) {
                    if (Path.GetFileName(map).ToLower().Contains("backup_"))
                        continue;
                    Count++;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        private void GetAvailableMaps() {
            try {

                Maps = new ObservableCollection<MapFile>();

                Count = 0;
                foreach (var file in Directory.EnumerateFiles(Root.FullName, "*.map", SearchOption.TopDirectoryOnly).Where(file => MapInfo.FilteredMaps.Contains(Path.GetFileName(file)) == false)) {

                    if (Path.GetFileName(file).ToLower().Contains("backup_"))
                        continue;

                    var map = new MapFile(Type, file);
                    if (Maps.Contains(map))
                        continue;

                    if (Path.GetFileName(file).ToLower().Contains("remove_")) {
                        map.IsRemoved = true;
                    }

                    Maps.Add(map);

                }

                Maps = new ObservableCollection<MapFile>(Maps.OrderBy(map => map.Name).OrderBy(map => map.IsRemoved));

            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw ex;
            }
            finally {
                Count = (Maps?.Count >= 1) ? Maps.Count : 0;
            }
        }

    }

}