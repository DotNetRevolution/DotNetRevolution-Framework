using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing
{
    public class TransactionEventSequence : ValueObject<TransactionEventType>
    {
        private int _currentValue = 0;

        public int CurrentValue
        {
            get
            {
                return _currentValue;
            }
        }

        public int Increment()
        {
            return _currentValue++;
        }
    }
}
