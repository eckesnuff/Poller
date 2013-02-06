using System;
using Poller.Domain;

namespace Poller.Services {
    class NoLinksMessageGenerator:IMessageGenerator {
        public string GenerateMessage(Change change) {
            return string.Format("[{0}]: {1} commited changeset {2}, with comment: {3}",
                                 change.ProjectName, change.UserName,  change.ChangeSet,
                                 String.IsNullOrEmpty(change.Comment) ? "LMF" : change.Comment);
        }
    }
}
