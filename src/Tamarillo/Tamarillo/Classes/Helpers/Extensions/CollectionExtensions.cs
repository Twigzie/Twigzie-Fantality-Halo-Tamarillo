using System.Collections.ObjectModel;
using System.Linq;
using Tamarillo.Classes.Models.Tabs;

namespace Tamarillo.Classes.Helpers.Extensions {

    public static class CollectionExtensions {

        public static bool ContainsTab(this ObservableCollection<TabModel> source, string tab) {
            return source?.FirstOrDefault(t => t.ID == tab) != null;
        }
        public static bool ContainsTab(this ObservableCollection<TabModel> source, Game type) {
            return source?.FirstOrDefault(t => t.Type == type) != null;
        }

    }

}