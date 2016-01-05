using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Helper
{
    public static class BooleanHelper
    {
        private static readonly Collection<string> TrueValues;

        static BooleanHelper()
        {
            TrueValues = new Collection<string>
                {
                    "T",
                    "YES",
                    "Y",
                    "1"
                };
        }

        public static bool IsTrue(string value)
        {
            Contract.Requires(value != null);

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            bool parsedValue;

            return bool.TryParse(value, out parsedValue)
                ? parsedValue
                : TrueValues.Contains(value.Trim().ToUpperInvariant());
        }

        public static bool IsTrue(int value)
        {
            return value == 1;
        }
    }
}
