using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Poller.Services {
    public class Campfire: IChat {
        private Settings _settings;
        public Campfire() {
            var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (var streamReader = new StreamReader(Path.Combine(currentDir, "campfire.json"))) {
                _settings = new JsonSerializer().Deserialize<Settings>(new JsonTextReader(streamReader));
            }
        }
        public void Send(string message) {
            var jsonMessage = new SerialContext() {Message = new Message {Body = message}};
            var data = JsonConvert.SerializeObject(jsonMessage);
            using (var client = new WebClient()) {
                client.Credentials = new NetworkCredential(_settings.UserToken, "X");
                client.Headers.Add("Content-Type", "application/json");
                client.UploadData(string.Format("https://{0}.campfirenow.com/room/{1}/speak.json", _settings.AccountName,_settings.RoomNumber), "POST",
                                  Encoding.UTF8.GetBytes(data));
            }

        }
        public class Settings {
            public string UserToken { get; set; }
            public string RoomNumber { get; set; }
            public string AccountName { get; set; }
        }
        [DataContract]
        public class SerialContext {
            [DataMember(Name = "message")]
            public Message Message { get; set; }
        }
        [DataContract]
        public class Message {
            [DataMember(Name = "body")]
            public string Body { get; set; }
        }

    }
}