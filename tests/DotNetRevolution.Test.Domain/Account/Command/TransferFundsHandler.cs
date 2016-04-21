using System;
using DotNetRevolution.Core.Command;

namespace DotNetRevolution.Test.Domain.Account.Command
{
    public class TransferFundsHandler : CommandHandler<TransferFunds>
    {
        public override void Handle(TransferFunds command)
        {
            throw new NotImplementedException();
        }
    }
}
