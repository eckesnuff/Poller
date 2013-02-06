using System;
using System.Configuration;
using System.Threading;
using Poller.Domain;
using Poller.Services;

namespace Poller {
    public class Poller {
        private readonly ISourceRepository _sourceRepository;
        private readonly IChat _chat;
        private readonly ILogger _logger;
        private readonly IMessageGenerator _messageGenerator;
        private readonly int _pollingInterval;
        private bool _running;

        public Poller(ISourceRepository sourceRepository,IChat chat,ILogger logger,IMessageGenerator messageGenerator) {
            _sourceRepository = sourceRepository;
            _chat = chat;
            _logger = logger;
            _messageGenerator = messageGenerator;
            _pollingInterval = int.Parse(ConfigurationManager.AppSettings["PollingInterval"]);
        }
        public void Start() {
            _running = true;
            Runner();
        }
        public void Stop() {
            _running = false;
        }
        protected void Runner() {
            int error = 0;
            while (_running) {
                try {
                    foreach (var change in _sourceRepository.GetLatestchanges()) {
                        _chat.Send(_messageGenerator.GenerateMessage(change));
                    }
                    error = 0;
                }
                catch (Exception ex) {
                    error++;
                    _logger.Log(ex.ToString(), LogType.Error);
                    if(error==10) {
                        _logger.Log("Stopping do to many errors", LogType.Error);
                        _running = false;
                    }

                }
                Thread.Sleep(_pollingInterval*1000);
            }
        }
    }
}