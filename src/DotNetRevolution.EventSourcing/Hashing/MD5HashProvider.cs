using System.Security.Cryptography;
using System.Text;

namespace DotNetRevolution.EventSourcing.Hashing
{
    public class MD5HashProvider : IHashProvider
    {
        public byte[] GetHash(string value)
        {
            return MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value));
        }
    }
}
