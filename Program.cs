using System.ServiceProcess;
using System.Threading;
using Poller.Services;
using StructureMap;

namespace Poller {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            ObjectFactory.Initialize(x => x.PullConfigurationFromAppConfig = true);
            var poller = new Poller(
                ObjectFactory.GetInstance<ISourceRepository>(),
                ObjectFactory.GetInstance<IChat>(),
                ObjectFactory.GetInstance<ILogger>(),
                ObjectFactory.GetInstance<IMessageGenerator>());

            ServiceBase.Run(new WindowsService(poller));
        }
    }
}
