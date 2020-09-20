﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Otor.MsixHero.Appx.Packaging.Installation.Entities;
using Otor.MsixHero.Infrastructure.ThirdParty.Sdk;
using Timer = System.Timers.Timer;

namespace Otor.MsixHero.Appx.Diagnostic.RunningDetector
{
    public class RunningDetector : IRunningDetector, IDisposable
    {
        private readonly IList<Subscriber> observers = new List<Subscriber>();

        private readonly Timer timer = new Timer(2000);
        private IDictionary<string, string> consideredPackages;
        private HashSet<string> previouslyRunningAppIds;

        public IDisposable Subscribe(IObserver<ActivePackageFullNames> observer)
        {
            var returned = new Subscriber(this, observer);
            observers.Add(returned);
            return returned;
        }

        public async Task Listen(IList<InstalledPackage> installedPackages, CancellationToken cancellationToken)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsApps") + "\\";
            this.consideredPackages = new Dictionary<string, string>();

            foreach (var item in installedPackages.Where(p => p.ManifestLocation.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                this.consideredPackages[item.ManifestLocation.Substring(0, item.ManifestLocation.IndexOf('\\', path.Length + 1))] = item.PackageId;
            }

            this.timer.Elapsed -= this.TimerOnElapsed;
            this.timer.Elapsed += this.TimerOnElapsed;
            this.timer.Start();

            await this.DoCheck().ConfigureAwait(false);
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
#pragma warning disable 4014
            this.DoCheck();
#pragma warning restore 4014
        }

        private async Task DoCheck()
        {
            if (!this.timer.Enabled || this.consideredPackages == null)
            {
                return;
            }

            try
            {
                this.timer.Stop();

                var wrapper = new TaskListWrapper();
                var table = await wrapper.GetBasicAppProcesses("running").ConfigureAwait(false);

                var nowRunning = new HashSet<string>();
                foreach (var item in table)
                {
                    nowRunning.Add(item.PackageName);
                }

                if (this.previouslyRunningAppIds != null && nowRunning.SequenceEqual(this.previouslyRunningAppIds))
                {
                    return;
                }

                this.previouslyRunningAppIds = nowRunning;
                this.Publish(new ActivePackageFullNames(nowRunning));
            }
            finally
            {
                this.timer.Start();
            }
        }

        public Task StopListening(CancellationToken cancellationToken)
        {
            this.consideredPackages = null;
            this.timer.Elapsed -= this.TimerOnElapsed;
            this.timer.Stop();
            return Task.FromResult(true);
        }

        private void Publish(ActivePackageFullNames eventPayload)
        {
            foreach (var item in this.observers)
            {
                item.Notify(eventPayload);
            }
        }

        void IDisposable.Dispose()
        {
            foreach (var disposable in this.observers)
            {
                disposable.Dispose();
            }
        }

        private class Subscriber : IDisposable
        {
            private readonly RunningDetector parent;
            private readonly IObserver<ActivePackageFullNames> observer;

            public Subscriber(RunningDetector parent, IObserver<ActivePackageFullNames> observer)
            {
                this.parent = parent;
                this.observer = observer;
            }

            public void Notify(ActivePackageFullNames payload)
            {
                this.observer.OnNext(payload);
            }

            public void Dispose()
            {
                parent.observers.Remove(this);
            }
        }
    }
}