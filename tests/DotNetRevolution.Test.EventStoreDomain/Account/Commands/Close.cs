using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Close : Command
    {
        public Guid AccountId { get; }

        public Close(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
