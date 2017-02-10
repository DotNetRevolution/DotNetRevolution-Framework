using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionContext<T> : ProjectionContext, IProjectionContext<T>
    {
        public new T Data { get; }

        public ProjectionContext(T data, IProjectionContext projectionContext)
            : base(projectionContext)
        {
            Contract.Requires(projectionContext != null);
            Contract.Requires(data != null);

            Data = data;
        }
    }
}
