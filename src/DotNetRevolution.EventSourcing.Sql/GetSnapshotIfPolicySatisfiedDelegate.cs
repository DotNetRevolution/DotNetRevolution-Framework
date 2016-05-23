using DotNetRevolution.EventSourcing.Snapshotting;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal delegate Snapshot GetSnapshotIfPolicySatisfiedDelegate(EventProvider eventProvider);
}
