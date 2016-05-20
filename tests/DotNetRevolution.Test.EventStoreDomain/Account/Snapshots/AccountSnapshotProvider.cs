using DotNetRevolution.EventSourcing.Snapshotting;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Snapshots
{
    public class AccountSnapshotProvider : SnapshotProvider<AccountAggregateRoot>
    {
        public override Snapshot GetSnapshot(AccountAggregateRoot aggregateRoot)
        {
            return new Snapshot(new AccountSnapshot(aggregateRoot.Identity, aggregateRoot.Balance));
        }
    }
}
