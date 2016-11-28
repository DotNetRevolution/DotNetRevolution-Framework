using DotNetRevolution.Core.Base;

namespace DotNetRevolution.Core.Metadata
{
    public class Meta : ValueObject<Meta>
    {
        public string Key { get; }

        public string Value { get; }

        public Meta(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
