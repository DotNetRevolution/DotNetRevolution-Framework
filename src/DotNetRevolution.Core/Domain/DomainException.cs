namespace DotNetRevolution.Core.Domain
{
//    [Serializable]
//    public class DomainException : Exception
//    {
//        public IEnumerable<string> Reasons { get; }

//        public IEnumerable<string> Suggestions { get; }

//        public DomainException(IEnumerable<string> reasons, IEnumerable<string> suggestions)
//            : this(reasons, suggestions, null)
//        {
//        }

//        public DomainException(IEnumerable<string> reasons, IEnumerable<string> suggestions, Exception innerException)
//            : base("Domain exception occurred.", innerException)
//        {
//            if (reasons == null)
//            {
//                reasons = new string[0];
//            }

//            if (suggestions == null)
//            {
//                suggestions = new string[0];
//            }

//            Suggestions = suggestions;
//            Reasons = reasons;
//        }

//        protected DomainException(SerializationInfo info, StreamingContext context)
//            : base(info, context)
//        {
//            DomainEvent = info.GetValue("DomainEvent", typeof(DomainEvent)) as DomainEvent;
//        }

//        public override void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            EnsureArgument.NotNull(info, "info");

//            info.AddValue("DomainEvent", DomainEvent);

//            base.GetObjectData(info, context);
//        }
//    }
}
