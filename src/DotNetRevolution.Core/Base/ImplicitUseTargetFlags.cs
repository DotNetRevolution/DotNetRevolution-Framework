using System;

namespace DotNetRevolution.Base
{
    /// <summary>
    /// Specify what is considered used implicitly when marked with <see cref="ImplicitUseAttribute"/> or <see cref="Annotations.UsedImplicitlyAttribute"/>
    /// </summary>
    [Flags]
    public enum ImplicitUseTargetFlags
    {
        Default = Itself,

        Itself = 1,

        /// <summary>
        /// Members of entity marked with attribute are considered used
        /// </summary>
        Members = 2,

        /// <summary>
        /// Entity marked with attribute and all its members considered used
        /// </summary>
        WithMembers = Itself | Members
    }
}