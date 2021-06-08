﻿// MSIX Hero
// Copyright (C) 2021 Marcin Otorowski
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// Full notice:
// https://github.com/marcinotorowski/msix-hero/blob/develop/LICENSE.md

using System;
using Otor.MsixHero.Appx.Psf.Entities;
using Otor.MsixHero.Appx.Psf.Entities.Descriptor;

namespace Otor.MsixHero.App.Controls.PackageExpert.Regions.Applications.ViewModels
{
    public class TracingPsfViewModel
    {
        private readonly PsfTracingRedirectionDescriptor definition;

        public TracingPsfViewModel(PsfTracingRedirectionDescriptor definition)
        {
            this.definition = definition;
        }

        public string BreaksOn
        {
            get
            {
                switch (this.definition.BreakOn)
                {
                    case TraceLevel.UnexpectedFailures:
                        return "Unexpected failures";
                    case TraceLevel.Always:
                        return "Always";
                    case TraceLevel.IgnoreSuccess:
                        return "IgnoreSuccess";
                    case TraceLevel.AllFailures:
                        return "All failures";
                    case TraceLevel.Ignore:
                        return "Ignore";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string OutputTo
        {
            get
            {
                switch (this.definition.TracingType)
                {
                    case TracingType.Console:
                        return "Write to console";
                    case TracingType.EventLog:
                        return "Write in Event Log";
                    default:
                        return "Default output";
                }
            }
        }
    }
}