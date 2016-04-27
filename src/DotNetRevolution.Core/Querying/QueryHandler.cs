namespace DotNetRevolution.Core.Querying
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : class
    {
        public virtual bool Reusable => true;

        public abstract TResult Handle(TQuery query);

        public TResult1 Handle<TResult1>(IQuery<TResult1> query) where TResult1 : class
        {
            return Handle((TQuery)query) as TResult1;
        }
    }
}
