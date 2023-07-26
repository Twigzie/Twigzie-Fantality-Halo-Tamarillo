using System;
using Tamarillo.Classes.Entities.Maps;

namespace Tamarillo.Controls.Tabs.Maps {

    public partial class MapContainer {

        public MapContainer(MapSettings settings) {

            DataContext = settings;
            InitializeComponent();

        }

        protected override void OnInitialized(EventArgs e) {

            base.OnInitialized(e);

            try {
                ((MapSettings)DataContext).Initialize();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }

        }

    }

}