using System;

namespace Poller.Domain {
    public class Change {
        public string UserName { get; set; }
        public DateTime Time { get; set; }
        public string ProjectName { get; set; }
        public string Comment { get; set; }
        public string ChangeSetLink { get; set; }
        public string ChangeSet { get; set; }
    }
}
