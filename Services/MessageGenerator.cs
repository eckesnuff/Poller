using System;
using Poller.Domain;

namespace Poller.Services {
    public class MessageGenerator : IMessageGenerator {
        public string GenerateMessage(Change change) {
            return string.Format("[{0}]: {1} commited changeset <a href=\"{2}\">{3}</a>, with comment: {4}",
                                 change.ProjectName, change.UserName, change.ChangeSetLink, change.ChangeSet,
                                 String.IsNullOrEmpty(change.Comment) ? "..." : change.Comment);
        }
    }
}