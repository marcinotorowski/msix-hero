﻿using otor.msixhero.lib.Infrastructure.Progress;

namespace otor.msixhero.lib.BusinessLayer.State
{
    public class BusyStatusChange : IBusyStatusChange
    {
        public BusyStatusChange(bool isBusy, string message, int progress)
        {
            IsBusy = isBusy;
            Message = message;
            Progress = progress;
        }

        public bool IsBusy { get; private set; }

        public string Message { get; private set; }

        public int Progress { get; private set; }
    }
}