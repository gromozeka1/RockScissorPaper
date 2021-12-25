using System;
using System.Security.Cryptography;

namespace RockScissorPaper
{
    class HmacGenerator
    {
        private const int keyLength = 32;
        private byte[] key;

        public string GenerateHmac(int move)
        {
            using (var rngCrypt = new RNGCryptoServiceProvider())
            {
                this.key = new byte[keyLength];
                rngCrypt.GetBytes(this.key);
            }
            using var hmac = new HMACSHA256(this.key);
            byte[] hashValue = hmac.ComputeHash(BitConverter.GetBytes(move));
            return Convert.ToHexString(hashValue);
        }

        public string Key
        {
            get
            {
                return Convert.ToHexString(this.key);
            }
        }
    }
}
