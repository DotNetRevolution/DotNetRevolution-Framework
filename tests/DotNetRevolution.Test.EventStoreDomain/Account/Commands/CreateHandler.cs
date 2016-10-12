using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class CreateHandler : CommandHandler<Create>
    {
        public override void Handle(Create command)
        {
            var domainEvents = AccountAggregateRoot.Create(command);
        }

        public override Task HandleAsync(Create command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
