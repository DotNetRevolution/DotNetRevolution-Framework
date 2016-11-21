using System.Collections.Generic;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Projections
{
    public class AccountProjectionState
    {
        public IList<Account> Accounts { get; }

        public AccountProjectionState()
        {
            Accounts = new List<Account>();
        }
    }
}
