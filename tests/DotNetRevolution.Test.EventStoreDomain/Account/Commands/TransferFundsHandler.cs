﻿using System;
using DotNetRevolution.Core.Commanding;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class TransferFundsHandler : CommandHandler<TransferFunds>
    {
        public override void Handle(TransferFunds command)
        {
            throw new NotImplementedException();
        }

        public override Task HandleAsync(TransferFunds command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
