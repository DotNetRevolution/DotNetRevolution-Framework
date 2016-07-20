using DotNetRevolution.Core.Querying;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Query
{
    public class GetAllDomains : Query<GetAllDomains.IResult>
    {
        public interface IResult
        {
            ICollection<IResultItem> Items { [Pure] get; }
        }

        public interface IResultItem
        {
            string Name { [Pure] get; }

            string Description { [Pure] get; }
        }
    }
}
