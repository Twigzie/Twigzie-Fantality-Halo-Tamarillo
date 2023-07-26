using System.Collections.ObjectModel;
using Tamarillo.Classes.Entities.Maps;
using Tamarillo.Classes.Helpers;
using Tamarillo.Classes.Models.Base;

namespace Tamarillo.Classes.Entities.Design {

    public class DesignSettings : Model {

        public ObservableCollection<MapFile> Maps {
            get;
            set;
        }

        public DesignSettings() {

            Maps = new ObservableCollection<MapFile>() { 
                new MapFile(Game.H3, @"C:\Program Files (x86)\Steam\steamapps\common\Halo The Master Chief Collection\halo3\maps\guardian.map") {
                    IsRemoved = true
                },
                new MapFile(Game.H3, @"C:\Program Files (x86)\Steam\steamapps\common\Halo The Master Chief Collection\halo3\maps\guardian.map")
            };

        }

    }

}