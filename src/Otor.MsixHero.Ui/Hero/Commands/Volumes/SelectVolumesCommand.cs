﻿using System.Collections.Generic;
using System.Linq;
using Otor.MsixHero.Appx.Volumes.Entities;
using Otor.MsixHero.Ui.Hero.Commands.Base;

namespace Otor.MsixHero.Ui.Hero.Commands.Volumes
{
    public class SelectVolumesCommand : UiCommand<IList<AppxVolume>>
    {
        public SelectVolumesCommand()
        {
            this.SelectedVolumePaths = new List<string>();
        }

        public SelectVolumesCommand(string volumePath)
        {
            this.SelectedVolumePaths = new List<string> { volumePath };
        }

        public SelectVolumesCommand(IList<string> volumePaths)
        {
            this.SelectedVolumePaths = volumePaths;
        }

        public SelectVolumesCommand(params string[] volumePaths)
        {
            this.SelectedVolumePaths = volumePaths.ToList();
        }

        public SelectVolumesCommand(IEnumerable<string> manifestPaths)
        {
            this.SelectedVolumePaths = manifestPaths.ToList();
        }

        public IList<string> SelectedVolumePaths { get; }
    }
}