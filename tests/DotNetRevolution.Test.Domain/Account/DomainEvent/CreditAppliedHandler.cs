using System;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Test.Domain.Account.DomainEvent
{
    public class CreditAppliedHandler : DomainEventHandler<CreditApplied>
    {
        public override void Handle(CreditApplied domainEvent)
        {
            Console.WriteLine(string.Format("Credit applied for Account {0} with amount {1:$0.00} setting current balance to {2:$0000.00}", 
                domainEvent.AccountId, 
                domainEvent.Amount, 
                domainEvent.Balance));
        }
    }
}
