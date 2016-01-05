namespace DotNetRevolution.Core.Query
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : class
    {
        public virtual bool Reusable => true;

        public abstract TResult Handle(TQuery query);
        
        public object Handle(object query)
        {
            return Handle((TQuery) query);
        }
    }
}
