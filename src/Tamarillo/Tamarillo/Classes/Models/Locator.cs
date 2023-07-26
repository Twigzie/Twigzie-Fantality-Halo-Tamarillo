using Tamarillo.Classes.Models.Main;

namespace Tamarillo.Classes.Models {

    public sealed class Locator {

        #region ISingleton

        static Locator() {
            Instance = new Locator();
        }
        public static Locator Instance {
            get;
        }

        #endregion

        public AppModel Application => AppModel.Instance;

    }

}