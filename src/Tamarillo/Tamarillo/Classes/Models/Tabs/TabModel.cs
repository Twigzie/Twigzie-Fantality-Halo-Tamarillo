using System;
using Tamarillo.Classes.Helpers;
using Tamarillo.Classes.Helpers.Extensions;

namespace Tamarillo.Classes.Models.Tabs {

    public class TabModel {

        public TabModel() {
            ID = GenerateID();
        }
        public TabModel(Game type) {
            ID = GenerateID();
            Type = type;
        }

        public string ID {
            get;
        }
        public string Name {
            get;
            set;
        }
        public object Content {
            get;
            set;
        }
        public Game Type {
            get;
            set;
        }

        private string GenerateID() {
            try {
                var id = Guid.NewGuid().ToString();
                if (Locator.Instance.Application.Tabs?.ContainsTab(id) == true)
                    return GenerateID();
                return id;
            }
            catch {
                throw new Exception("Error generating 'TabID'");
            }
        }

    }

}