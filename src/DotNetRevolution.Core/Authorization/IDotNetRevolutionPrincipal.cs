using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authorization
{
    public class DotNetRevolutionPrincipal : ClaimsPrincipal
    {
        public DotNetRevolutionPrincipal()
        {
        }

        public DotNetRevolutionPrincipal(BinaryReader reader) : base(reader)
        {
        }

        public DotNetRevolutionPrincipal(IPrincipal principal) : base(principal)
        {
        }

        public DotNetRevolutionPrincipal(IIdentity identity) : base(identity)
        {
        }

        public DotNetRevolutionPrincipal(IEnumerable<ClaimsIdentity> identities) : base(identities)
        {
        }

        protected DotNetRevolutionPrincipal(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool CanExecuteCommand(Type commandType)
        {
            return true;
        }

        public bool CanExecuteQuery(Type queryType)
        {
            return true;
        }
    }
}
