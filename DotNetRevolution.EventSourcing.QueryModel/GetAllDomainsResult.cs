using System.Collections.Generic;
using DotNetRevolution.EventSourcing.Query;
using System.Collections.ObjectModel;

namespace DotNetRevolution.EventSourcing.QueryModel
{
    public class GetAllDomainsResult : GetAllDomains.IResult
    {
        public ICollection<GetAllDomains.IResultItem> Items { get; }

        public GetAllDomainsResult()
        {
            Items = new Collection<GetAllDomains.IResultItem>();
        }
    }
}
