using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Newtonsoft.Json;
using Change = Poller.Domain.Change;

namespace Poller.Services {
    public class Tfs:ISourceRepository {
        private Settings _settings;
        private DateTime _lastCheckedTime;
        public Tfs() {
            var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (currentDir != null)
                using(var streamReader = new StreamReader(Path.Combine(currentDir,"tfs.json"))) {
                    _settings = new JsonSerializer().Deserialize<Settings>(new JsonTextReader(streamReader));
                    _lastCheckedTime = DateTime.Now;
                }
        }


        public virtual List<Change> GetLatestchanges() {
            var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(_settings.TeamServer));
            var vcs = tfs.GetService<VersionControlServer>();
            var changes = vcs.QueryHistory("$/", VersionSpec.Latest, 0, RecursionType.Full, null,
                                           new DateVersionSpec(_lastCheckedTime), VersionSpec.Latest, 100, true,
                                           false).Cast<Changeset>();
            _lastCheckedTime = DateTime.Now;
            changes = FilterChanges(changes);
            return TransformChanges(changes).ToList();
        }

        protected virtual IEnumerable<Change> TransformChanges(IEnumerable<Changeset> changes) {
            foreach(var changeSet in changes) {
                yield return new Change
                {
                    UserName = changeSet.Committer,
                    Comment = changeSet.Comment,
                    ProjectName = GetChangedProject(changeSet.Changes),
                    Time = changeSet.CreationDate,
                    ChangeSetLink = string.Format("https://team.meridium.se/TSWA/UI/Pages/Scc/ViewChangeset.aspx?changeset={0}",changeSet.ChangesetId),
                    ChangeSet = changeSet.ChangesetId.ToString(CultureInfo.InvariantCulture)
                };
            }
        }

        private string GetChangedProject(Microsoft.TeamFoundation.VersionControl.Client.Change[] changes) {
            var changeSource = changes[0].Item.ServerItem;
            var match = Regex.Match(changeSource, @"\$/([^/]+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        protected virtual IEnumerable<Changeset> FilterChanges(IEnumerable<Changeset> changes) {
            foreach (var changeSet in changes) {
                if (_settings.Members.Any(x=>x.Equals(changeSet.Committer,StringComparison.InvariantCultureIgnoreCase))) {
                    yield return changeSet;
                }
            }
        }
        public class Settings {
            public List<string> Members { get; set; }
            public string TeamServer { get; set; }
        }

    }
}
