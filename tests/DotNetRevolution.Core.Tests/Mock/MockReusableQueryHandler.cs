using DotNetRevolution.Core.Query;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableQueryHandler<TQuery, TResult> : QueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : class
    {
        public override TResult Handle(TQuery query)
        {
            return default(TResult);
        }
    }
}
