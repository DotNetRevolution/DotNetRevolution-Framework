using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public class ProjectionPosition : ValueObject<ProjectionPosition>
    {
        public int Value { get; }

        public ProjectionPosition(int position)
        {          
            Value = position;
        }

        public static implicit operator int(ProjectionPosition position)
        {
            Contract.Requires(position != null);

            return position.Value;
        }

        public static implicit operator ProjectionPosition(int value)
        {
            Contract.Ensures(Contract.Result<ProjectionPosition>() != null);

            return new ProjectionPosition(value);
        }
    }
}
