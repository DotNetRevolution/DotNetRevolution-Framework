using System;

namespace DotNetRevolution.Core.Base
{
    /// <summary>
    /// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
    /// so this symbol will not be marked as unused (as well as by other usage inspections)
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class UsedImplicitlyAttribute : Attribute
    {
        public UsedImplicitlyAttribute()
            : this(ImplicitUseKinds.Default, ImplicitUseTargets.Default)
        {
        }

        public UsedImplicitlyAttribute(ImplicitUseKinds useKind, ImplicitUseTargets target)
        {
            UseKind = useKind;
            Target = target;
        }

        public UsedImplicitlyAttribute(ImplicitUseKinds useKind)
            : this(useKind, ImplicitUseTargets.Default)
        {
        }
        
        public UsedImplicitlyAttribute(ImplicitUseTargets target)
            : this(ImplicitUseKinds.Default, target)
        {
        }

        public ImplicitUseKinds UseKind { get; }

        /// <summary>
        /// Gets value indicating what is meant to be used
        /// </summary>
        public ImplicitUseTargets Target { get; }
    }
}