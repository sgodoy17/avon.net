using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Crypto
{
    public static class RngCrypto
    {
        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] arrbyte = new byte[32];
            rngCsp.GetNonZeroBytes(arrbyte);
            return Convert.ToBase64String(arrbyte);
        }
    }
}
