using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.EventSourcing.Commanding
{
    public class AnnounceTransactionCommand : Command
    {
        public Guid TransactionGuid { get; }

        public AnnounceTransactionCommand(Guid transactionGuid)
        {
            TransactionGuid = transactionGuid;
        }
    }
}