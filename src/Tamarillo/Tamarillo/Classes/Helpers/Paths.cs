using System.IO;
using System.Reflection;

namespace Tamarillo.Classes.Helpers {

    public static class Paths {

        public static string AppPath {
            get {
                var temp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(temp))
                    return "";
                return temp;
            }
        }
        public static string Settings {
            get => Path.Combine(AppPath, "settings.json");
        }

    }
}
