using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using static Tamarillo.Classes.Helpers.Paths;

namespace Tamarillo.Classes.Helpers {

    [DataContract]
    public class Config {

        static Config() {
            Instance = new Config();
        }

        [IgnoreDataMember]
        internal static Config Instance {
            get;
            set;
        }

        [DataMember(IsRequired = true, Name = "path", Order = 0)]
        public string Path {
            get; set;
        }

        public bool Load() {
            try {

                if (File.Exists(Settings)) {

                    var serializer = new DataContractJsonSerializer(typeof(Config));
                    using (var reader = new FileStream(Settings, FileMode.Open, FileAccess.Read)) {
                        var temp = serializer.ReadObject(reader) as Config;
                        if (temp == null)
                            return false;
                        else {
                            Path = temp.Path;
                        }
                    }

                }
                else {

                    Path = "";

                    return Save();

                }

                return true;

            }
            catch {
                return false;
            }
        }
        public bool Save() {
            try {

                var serializer = new DataContractJsonSerializer(typeof(Config));
                using (var stream = new FileStream(Settings, FileMode.Create, FileAccess.Write))
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, false, true))
                    serializer.WriteObject(writer, this);

                return true;

            }
            catch {
                return false;
            }
        }

    }

}