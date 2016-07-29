using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace DotNetRevolution.Core.Hashing
{
    public class MD5HashProvider : IHashProvider
    {
        private readonly Encoding _encoding;

        public MD5HashProvider(Encoding encoding)
        {
            Contract.Requires(encoding != null);

            _encoding = encoding;
        }

        public byte[] GetHash(string value)
        {
            return MD5.Create().ComputeHash(_encoding.GetBytes(value));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_encoding != null);
        }
    }
}
