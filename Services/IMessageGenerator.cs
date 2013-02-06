using Poller.Domain;

namespace Poller.Services {
    public interface IMessageGenerator {
        string GenerateMessage(Change change);
    }
}
