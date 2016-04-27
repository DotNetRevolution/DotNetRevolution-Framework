using DotNetRevolution.Core.Querying;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableQueryHandler<TQuery, TResult> : QueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : class
    {
        public override TResult Handle(TQuery query)
        {
            return default(TResult);
        }
    }
}
