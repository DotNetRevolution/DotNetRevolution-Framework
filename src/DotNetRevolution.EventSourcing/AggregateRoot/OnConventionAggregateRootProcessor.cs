namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    public class OnConventionAggregateRootProcessor : AggregateRootProcessor
    {
        protected override string GetMethodName(string domainEventName)
        {
            return string.Format("On{0}", domainEventName);
        }
    }
}
