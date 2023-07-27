using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Tamarillo.Classes.Commands;
using Tamarillo.Classes.Entities.Maps;
using Tamarillo.Classes.Helpers;
using Tamarillo.Classes.Helpers.Extensions;
using Tamarillo.Classes.Models.Base;
using Tamarillo.Classes.Models.Tabs;
using Tamarillo.Controls.Tabs.Maps;
using static Tamarillo.Classes.Helpers.Extensions.BaseExtensions;

namespace Tamarillo.Classes.Models.Main {

    public class AppModel : Model {

        #region ISingleton

        static AppModel() {
            Instance = new AppModel() {
                Caption = $"Tamarillo (MCC Map Manager)",
                Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion,
                Status = "Ready"
            };
            Instance?.Initialize();
        }
        public static AppModel Instance {
            get;
            set;
        }

        #endregion

        public string Status {
            get;
            set;
        }
        public string Caption {
            get;
            set;
        }
        public string Version {
            get;
            set;
        }
        public DirectoryInfo Root {
            get;
            set;
        }

        public MapSettings H1Settings {
            get;
            private set;
        }
        public MapSettings H2Settings {
            get;
            private set;
        }
        public MapSettings H2ASettings {
            get;
            private set;
        }
        public MapSettings H3Settings {
            get;
            private set;
        }
        public MapSettings H3OSettings {
            get;
            private set;
        }
        public MapSettings HRSettings {
            get;
            private set;
        }
        public MapSettings H4Settings {
            get;
            private set;
        }

        public ObservableCollection<TabModel> Tabs {
            get;
            private set;
        }

        #region Tabs

        public ICommand OpenTabCommad => new RelayParamCommand((arg) => {

            try {
                var tab = new TabModel((Game)arg);
                if (Tabs.ContainsTab(tab.Type))
                    return;

                switch (tab.Type) {
                    case Game.H1:
                        tab.Name = "Halo CE";
                        tab.Content = new MapContainer(H1Settings);
                        Tabs.Add(tab);
                        break;
                    case Game.H2:
                        tab.Name = "Halo 2";
                        tab.Content = new MapContainer(H2Settings);
                        Tabs.Add(tab);
                        break;
                    case Game.H2A:
                        tab.Name = "Halo 2A";
                        tab.Content = new MapContainer(H2ASettings);
                        Tabs.Add(tab);
                        break;
                    case Game.H3:
                        tab.Name = "Halo 3";
                        tab.Content = new MapContainer(H3Settings);
                        Tabs.Add(tab);
                        break;
                    case Game.H3O:
                        tab.Name = "Halo 3 ODST";
                        tab.Content = new MapContainer(H3OSettings);
                        Tabs.Add(tab);
                        break;
                    case Game.HR:
                        tab.Name = "Halo Reach";
                        tab.Content = new MapContainer(HRSettings);
                        Tabs.Add(tab);
                        break;
                    case Game.H4:
                        tab.Name = "Halo 4";
                        tab.Content = new MapContainer(H4Settings);
                        Tabs.Add(tab);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex) {
                throw ex;
            }

        });
        public ICommand OnCloseTabCommand => new RelayParamCommand((arg) => {

            try {

                var source = (TabModel)arg;

                if (null == source || null == Tabs || Tabs.ContainsTab(source.ID) == false) {
                    return;
                }

                var index = Tabs.IndexOf(Tabs.FirstOrDefault(t => t.Name == source.Name && t.ID == source.ID));
                if (index >= 0) {
                    Tabs.RemoveAt(index);
                }

            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw ex;
            }

        });

        #endregion

        private void Initialize() {
            try {

                Status = "Initializing";
                Version = GetFileVersion(Assembly.GetExecutingAssembly());

                Root = Steam.GetInstallDirectory();

                H1Settings  = new MapSettings(Game.H1, Path.Combine(Root.FullName, "halo1\\maps"));
                H2Settings  = new MapSettings(Game.H2, Path.Combine(Root.FullName, "halo2\\h2_maps_win64_dx11"));
                H2ASettings = new MapSettings(Game.H2A, Path.Combine(Root.FullName, "groundhog\\maps"));
                H3Settings  = new MapSettings(Game.H3, Path.Combine(Root.FullName, "halo3\\maps"));
                HRSettings  = new MapSettings(Game.HR, Path.Combine(Root.FullName, "haloreach\\maps"));
                H4Settings  = new MapSettings(Game.H4, Path.Combine(Root.FullName, "halo4\\maps"));

                Tabs = new ObservableCollection<TabModel>();

            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw ex;
            }
            finally {
                Status = "Ready";
            }
        }

    }

}