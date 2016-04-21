using DotNetRevolution.Core.Query;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockQueryHandler<TQuery, TResult> : QueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : class, new()
    {
        public override bool Reusable
        {
            get
            {
                return false;
            }
        }

        public override TResult Handle(TQuery query)
        {
            return new TResult();
        }
    }
}
