using System;

namespace DotNetRevolution.Core.Base
{
    [Flags]
    public enum ImplicitUseTargets
    {
        Default = Itself,
        Itself = 1,        
        Members = 2,        
        WithMembers = Itself | Members
    }
}