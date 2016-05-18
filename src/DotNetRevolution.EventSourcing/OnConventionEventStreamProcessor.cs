namespace DotNetRevolution.EventSourcing
{
    public class OnConventionEventStreamProcessor : EventStreamProcessor
    {
        protected override string GetMethodName(string domainEventName)
        {
            return string.Format("On{0}", domainEventName);
        }
    }
}
