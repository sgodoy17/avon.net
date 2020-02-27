using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Component.Transversal.Cryptography
{
    public static class PasswordHasher
    {
        private const string ApplicationPepper = "jcRdrpCZB52WWZd2L4lbiS3y9MpRk8QJuWRGyXteImuO2abZ7m8CO4G9EdXB9WcwZPVM5nRXEHd4Yfqqm2qO8S53Y35Qt47";
        
        /// <summary>
        /// Generates a hash based on a 4 digit password, using the provided salt and the application constant <see cref="ApplicationPepper"/>. This is meant to be used on the client-side only. At the server side another hash is used, based on BCrypt. This method is expected to take about 20 ms per call.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetPinHashClientSide(string pin, string salt)
        {
            string saltString = ApplicationPepper + "." + salt;
            return GetKey(pin, saltString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="bcryptFactor"></param>
        public static string CreateDatabaseHash(string inputHash)
        {
            const int bcryptFactor = 8;
            return BCrypt.Net.BCrypt.HashPassword(inputHash, bcryptFactor);
        }

        /// <summary>
        /// Generates a new salt to be used with the PBKDF2, which is a new GUID, but this may be changed in the future
        /// </summary>
        /// <returns>A new GUID in string form</returns>
        public static string GenerateUserSalt()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="hashToCompareWith"></param>
        /// <returns></returns>
        [Pure]
        public static bool DatabaseAuthenticateUser(string inputHash, string hashToCompareWith)
        {
            return BCrypt.Net.BCrypt.Verify(inputHash, hashToCompareWith);
        }

        // NOTE: The iteration count should
        // be as high as possible without causing
        // unreasonable delay.  Note also that the password
        // and salt are byte arrays, not strings.  After use,
        // the password and salt should be cleared (with Array.Clear)
        // ReSharper disable once UnusedMember.Local
        private static byte[] Pbkdf2Sha512GetBytes(int dklen, byte[] password, byte[] salt, int iterationCount)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(password))
            {
                int hashLength = hmac.HashSize / 8;
                if ((hmac.HashSize & 7) != 0)
                    hashLength++;
                int keyLength = dklen / hashLength;
                if ((long)dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                    throw new ArgumentOutOfRangeException("dklen");
                if (dklen % hashLength != 0)
                    keyLength++;
                byte[] extendedkey = new byte[salt.Length + 4];
                Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
                using (var ms = new System.IO.MemoryStream())
                {
                    for (int i = 0; i < keyLength; i++)
                    {
                        extendedkey[salt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                        extendedkey[salt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                        extendedkey[salt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                        extendedkey[salt.Length + 3] = (byte)(((i + 1)) & 0xFF);
                        byte[] u = hmac.ComputeHash(extendedkey);
                        Array.Clear(extendedkey, salt.Length, 4);
                        byte[] f = u;
                        for (int j = 1; j < iterationCount; j++)
                        {
                            u = hmac.ComputeHash(u);
                            for (int k = 0; k < f.Length; k++)
                            {
                                f[k] ^= u[k];
                            }
                        }
                        ms.Write(f, 0, f.Length);
                        Array.Clear(u, 0, u.Length);
                        Array.Clear(f, 0, f.Length);
                    }
                    byte[] dk = new byte[dklen];
                    ms.Position = 0;
                    ms.Read(dk, 0, dklen);
                    ms.Position = 0;
                    for (long i = 0; i < ms.Length; i++)
                    {
                        ms.WriteByte(0);
                    }
                    Array.Clear(extendedkey, 0, extendedkey.Length);
                    return dk;
                }
            }
        }

        /// <summary>
        /// SHA512 of password
        /// SHA512 of Agility2Salt
        /// Rfc2898DeriveBytes of hashed password and hashed salt with 10.000 itterations
        /// Creates a AES256 key
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="itterations"></param>
        /// <returns></returns>
        public static string GetKey(string password, string salt)
        {
            if (password == null)
            {
                password = "";
            }
            if (salt == null)
            {
                salt = "";
            }

            const int itterations = 200;
            
            RijndaelManaged key = new RijndaelManaged { KeySize = 256 };

            byte[] sha512Bytes = Pbkdf2Sha512GetBytes(512, Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt),
                itterations);

            string base64Complete = Convert.ToBase64String(sha512Bytes);

            return base64Complete;
        }
    }
}