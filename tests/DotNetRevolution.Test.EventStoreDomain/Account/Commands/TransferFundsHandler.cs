using System;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class TransferFundsHandler : CommandHandler<TransferFunds>
    {
        public override void Handle(TransferFunds command)
        {
            throw new NotImplementedException();
        }
    }
}
