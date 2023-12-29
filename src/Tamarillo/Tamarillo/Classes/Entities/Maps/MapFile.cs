using System.IO;
using System.Runtime.Serialization;
using Tamarillo.Classes.Helpers;
using Tamarillo.Classes.Models.Base;

namespace Tamarillo.Classes.Entities.Maps {

    [DataContract]
    public class MapFile : Model {

        [DataMember(Name = "name", Order = 0)]
        public string Name {
            get;
            private set;
        }
        [DataMember(Name = "last_installed", Order = 1)]
        public int LastInstalled {
            get;
            set;
        }
        [DataMember(Name = "last_uninstalled", Order = 2)]
        public int LastUninstalled {
            get;
            set;
        }

        public Game Type {
            get;
            private set;
        }
        public string Image {
            get;
            private set;
        }
        public FileInfo Source {
            get;
            private set;
        }

        public bool IsVisible {
            get;
            set;
        }
        public bool IsBackup {
            get;
            set;
        }
        public bool IsRemoved {
            get;
            set;
        }

        public MapFile() {

            Name = "Unknown";
            Image = "map_unknown";

        }
        public MapFile(Game type, string source) {

            
            Source = new FileInfo(source);

            IsVisible = true;
            IsBackup = Source.Name.Contains("backup_");
            IsRemoved = Source.Name.Contains("remove_");

            Name = MapInfo.GetMapName(type, Path.GetFileNameWithoutExtension(Source.FullName));
            Image = MapInfo.GetMapImage(type, Path.GetFileNameWithoutExtension(Source.FullName));

        }

    }

}