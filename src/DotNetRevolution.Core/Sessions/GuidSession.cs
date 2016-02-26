﻿using DotNetRevolution.Core.Extension;
using System;

namespace DotNetRevolution.Core.Sessions
{
    public class GuidSession : Session
    {
        public GuidSession()
            : base(Guid.NewGuid().AsString())
        {
        }
    }
}