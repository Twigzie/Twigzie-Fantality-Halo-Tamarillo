using System.Diagnostics;
using System.Reflection;

namespace Tamarillo.Classes.Helpers.Extensions {

    public static class BaseExtensions {

        public static string GetFileVersion(Assembly assembly) {
            return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
        }

    }

}