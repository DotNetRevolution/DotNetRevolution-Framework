using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Extension
{
    public static class GuidExtension
    {
        public static string AsString(this Guid guid)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(string.IsNullOrWhiteSpace(Contract.Result<string>()) == false);

            var guidString = Guid.NewGuid().ToString();
            Contract.Assume(string.IsNullOrWhiteSpace(guidString) == false);

            return guidString;
        }
    }
}
