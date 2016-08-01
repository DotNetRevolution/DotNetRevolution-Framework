﻿using DotNetRevolution.Core.GuidGeneration;
using System;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class Command : ICommand
    {
        private readonly Guid _id = GuidGenerator.Default.Create();

        public Guid CommandId
        {
            get
            {
                return _id;
            }
        }
    }
}
