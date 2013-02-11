using System;
using System.Net;
using Poller.Domain;

namespace Poller.Services {
    /// <summary>
    /// Message generator using the url shortener is.gd
    /// </summary>
    /// <remarks>
    /// 2013-02-11 marcus: Created
    /// </remarks>
    public class ShortenedLinkMessageGenerator : IMessageGenerator {
        private readonly ILogger _logger;
        /// <summary>
        /// Generates the message using a shirtened url, falls back to the long url if shortener is unreachable
        /// </summary>
        /// <param name="change">The change.</param>
        /// <returns>A message with a shortened url</returns>
        public string GenerateMessage(Change change) {
            try {
                var responseString = new WebClient().DownloadString(string.Format("http://is.gd/create.php?format=simple&url={0}", change.ChangeSetLink));
                return string.Format("[{0}]: {1} commited changeset {2}, with comment: {3} ({4})",
                                                 change.ProjectName, change.UserName,change.ChangeSet,
                                                 String.IsNullOrEmpty(change.Comment) ? "LMF" : change.Comment,
                                                 responseString);
            } catch(WebException exception) {
                _logger.Log("Link could not be shortened, using the full url: " + exception.Message, LogType.Error);

                return string.Format("[{0}]: {1} commited changeset {2}, with comment: {3} ({4})",
                                 change.ProjectName, change.UserName, change.ChangeSet,
                                 String.IsNullOrEmpty(change.Comment) ? "LMF" : change.Comment,
                                 change.ChangeSetLink);
            }
        }
        public ShortenedLinkMessageGenerator(ILogger logger) {
            _logger = logger;
        }
    }
}