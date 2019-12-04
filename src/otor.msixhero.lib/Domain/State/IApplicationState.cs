﻿using otor.msixhero.lib.Infrastructure.Configuration;

namespace otor.msixhero.lib.Domain.State
{
    public interface IApplicationState
    {
        IPackageListState Packages { get; }  
        
        bool IsElevated { get; }

        bool IsSelfElevated { get; }
    }
}