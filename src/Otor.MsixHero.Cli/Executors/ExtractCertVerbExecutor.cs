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
using System.Threading.Tasks;
using Otor.MsixHero.Appx.Signing;
using Otor.MsixHero.Cli.Helpers;
using Otor.MsixHero.Cli.Verbs;
using Otor.MsixHero.Infrastructure.Logging;
using Otor.MsixHero.Infrastructure.ThirdParty.Exceptions;

namespace Otor.MsixHero.Cli.Executors
{
    public class ExtractCertVerbExecutor : VerbExecutor<ExtractCertVerb>
    {
        private readonly ISigningManager signingManager;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExtractCertVerbExecutor));

        public ExtractCertVerbExecutor(ExtractCertVerb verb, ISigningManager signingManager, IConsole console) : base(verb, console)
        {
            this.signingManager = signingManager;
        }

        public override async Task<int> Execute()
        {
            Logger.Info($"Extracting certificate from [{this.Verb.File}]...");

            try
            {
                await this.signingManager.ExtractCertificateFromMsix(this.Verb.File, this.Verb.Output).ConfigureAwait(false);
                await this.Console.WriteSuccess("Certificate has been extracted.");
                await this.Console.ShowCertSummary(signingManager, this.Verb.File);
                return 0;
            }
            catch (SdkException e)
            {
                Logger.Error(e);
                await this.Console.WriteError(e.Message);
                return e.ExitCode;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                await this.Console.WriteError(e.Message);
                return 1;
            }
        }
    }
}