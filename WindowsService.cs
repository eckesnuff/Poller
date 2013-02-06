using System.ServiceProcess;
using System.Threading;

namespace Poller {
    public partial class WindowsService : ServiceBase {
        private readonly Poller _poller;
        private Thread _thread;

        public WindowsService(Poller poller) {
            ServiceName = "Poller";
            _poller = poller;
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            _thread = new Thread(_poller.Start);
            _thread.IsBackground = true;
            _thread.Name = "Poller worker thread";
            _thread.Start();
        }

        protected override void OnStop() {
            _thread.Abort();
        }
        
    }
}
