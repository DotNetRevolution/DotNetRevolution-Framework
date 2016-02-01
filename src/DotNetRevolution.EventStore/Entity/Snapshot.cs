//using System;
//using DotNetRevolution.Core;
//using DotNetRevolution.Base;

//namespace DotNetRevolution.EventStore.Entity
//{
//    public class Snapshot : Transaction
//    {
//        public string Data { get; private set; }

//        [UsedImplicitly]
//        private Snapshot()
//        {
//        }

//        public Snapshot(Guid eventProviderId, string user, string data)
//            : base(eventProviderId, user, string.Empty, string.Empty)
//        {
//            EnsureArgument.NotNullOrWhiteSpace(data, "data");
            
//            Data = data;
//        }
//    }
//}