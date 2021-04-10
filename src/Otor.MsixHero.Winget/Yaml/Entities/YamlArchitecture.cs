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

using YamlDotNet.Serialization;

namespace Otor.MsixHero.Winget.Yaml.Entities
{
    public enum YamlArchitecture
    {
        // ReSharper disable once InconsistentNaming
        [YamlMember(Alias = "none")]
        None = 0,

        // ReSharper disable once InconsistentNaming
        [YamlMember(Alias = "x64")]
        X64,

        // ReSharper disable once InconsistentNaming
        [YamlMember(Alias = "x86")]
        X86,

        // ReSharper disable once InconsistentNaming
        [YamlMember(Alias = "arm")]
        Arm,

        // ReSharper disable once InconsistentNaming
        [YamlMember(Alias = "arm64")]
        Arm64,
        
        [YamlMember(Alias = "neutral")]
        Neutral
    }
}