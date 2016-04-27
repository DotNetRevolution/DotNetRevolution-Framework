using DotNetRevolution.Core.Commanding;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class Transaction
    {
        private readonly List<EventStream> _eventStreams = new List<EventStream>();

        public ICommand Command { [Pure] get; }

        public EventStreamCollection EventStreams
        {
            [Pure]
            get
            {
                return new EventStreamCollection(_eventStreams.ToArray());
            }
        }

        public Transaction(ICommand command, params EventStream[] eventStreams)
        {
            Contract.Requires(command != null);
            Contract.Requires(eventStreams != null);

            Command = command;

            foreach(var stream in eventStreams)
            {
                _eventStreams.Add(stream);
            }
        }
    }
}
