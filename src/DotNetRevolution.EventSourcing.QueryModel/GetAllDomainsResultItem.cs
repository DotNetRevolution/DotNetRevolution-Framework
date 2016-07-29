using DotNetRevolution.EventSourcing.Query;

namespace DotNetRevolution.EventSourcing.QueryModel
{
    public class GetAllDomainsResultItem : GetAllDomains.IResultItem
    {
        public string Description { get; }

        public string Name { get; }

        public GetAllDomainsResultItem(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
