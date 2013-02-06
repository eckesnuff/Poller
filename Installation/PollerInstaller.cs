using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Poller.Installation {
    [RunInstaller(true)]
    public class PollerInstaller : Installer {
        public PollerInstaller() {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //set the privileges
            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "Poller";
            serviceInstaller.StartType = ServiceStartMode.Manual;

            //must be the same as what was set in Program's constructor
            serviceInstaller.ServiceName = "Poller";

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
