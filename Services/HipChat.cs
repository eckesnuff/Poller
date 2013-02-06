using System.IO;
using HipChat;
using Newtonsoft.Json;

namespace Poller.Services {
    public class HipChat:IChat {
        private Settings _settings;
        public HipChat() {
            var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (var streamReader = new StreamReader(Path.Combine(currentDir, "hipchat.json"))) {
                _settings = new JsonSerializer().Deserialize<Settings>(new JsonTextReader(streamReader));
            }
        }
        public void Send(string message) {
            HipChatClient.SendMessage(_settings.APIKey, _settings.RoomName, _settings.SenderNick, message,
                                      HipChatClient.BackgroundColor.green);
        }

        public class Settings {
            public string SenderNick { get; set; }
            public string APIKey { get; set; }
            public string RoomName { get; set; }
        }
    }
}
