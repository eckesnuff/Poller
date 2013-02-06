using System.Collections.Generic;
using Poller.Domain;

namespace Poller.Services {
    public interface ISourceRepository {
        List<Change> GetLatestchanges();
    }
}
