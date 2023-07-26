using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using RestSharp;

namespace Tamarillo.Classes.Github {
    [DataContract]
    internal class Update {

        [DataMember(Name = "tag_name")]
        public string Version {
            get;
            set;
        }
        [DataMember(Name = "html_url")]
        public string Download {
            get;
            set;
        }

        [IgnoreDataMember]
        public Version Current {
            get => Assembly.GetExecutingAssembly().GetName().Version;
        }
        [IgnoreDataMember]
        public bool Available {
            get {
                if (string.IsNullOrEmpty(Version) || string.IsNullOrEmpty(Download))
                    return false;
                return new Version(Version).CompareTo(Current) > 0;
            }
        }

    }

    internal class Updater {

        public Updater() {
        }

        public async void CheckForUpdate() {
            var result = new Update();
            try {

                OnCheckUpdate?.Invoke();

                var client = new RestClient(new RestClientOptions() {
                    MaxTimeout = 10000
                });
                var request = new RestRequest($"https://api.github.com/repos/Twigzie/Fantality-Infinite-Mohawk/releases/latest", Method.Get);
                request.AddHeader("Connection", "Keep-Alive");
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Accept-Language", "en-US");

                var response = await client.ExecuteAsync(request);
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                    throw new Exception();

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content))) {

                    var source = new DataContractJsonSerializer(typeof(Update));
                    var target = (Update)source.ReadObject(stream);

                    if (target.Available)
                        OnCheckUpdateAvailable?.Invoke(target);
                    else {
                        OnCheckUpdateUnavailable?.Invoke();
                    }

                }

            }
            catch (Exception ex) {
                OnCheckUpdateError?.Invoke(ex.Message);
            }
        }

        #region Events

        public delegate void OnCheckUpdateEventHandler();
        public event OnCheckUpdateEventHandler OnCheckUpdate;

        public delegate void OnCheckUpdateErrorHandler(string data);
        public event OnCheckUpdateErrorHandler OnCheckUpdateError;

        public delegate void OnCheckUpdateAvailableHandler(Update data);
        public event OnCheckUpdateAvailableHandler OnCheckUpdateAvailable;

        public delegate void OnCheckUpdateUnavailableHandler();
        public event OnCheckUpdateUnavailableHandler OnCheckUpdateUnavailable;

        #endregion

    }

}