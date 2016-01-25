using System;

namespace DotNetRevolution.Core.Session
{
    public class GuidSession : Session
    {
        public GuidSession()
            : base(Guid.NewGuid().ToString())
        {
        }
    }
}