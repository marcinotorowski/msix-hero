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

using Otor.MsixHero.App.Hero.Commands.Base;

namespace Otor.MsixHero.App.Hero.Events.Base
{
    public class UiExecutedPayload<TCommand, TResult> : UiPayload where TCommand : UiCommand<TResult>
    {
        public UiExecutedPayload(object sender, TCommand command, TResult result) : base(sender)
        {
            Result = result;
            this.Command = command;
        }

        public TCommand Command { get; }

        public TResult Result { get; }
    }

    public class UiExecutedPayload<TCommand> : UiPayload where TCommand : UiCommand
    {
        public UiExecutedPayload(object sender, TCommand command) : base(sender)
        {
            this.Command = command;
        }

        public TCommand Command { get; }
    }
}