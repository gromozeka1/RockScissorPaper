using System;
using System.Security.Cryptography;

namespace RockScissorPaper
{
    /// <summary>
    /// HmacGenerator class.
    /// </summary>
    internal class HmacGenerator
    {
        private const int KeyLength = 32;
        private byte[] key;

        /// <summary>
        /// Gets a key value.
        /// </summary>
        public string Key
        {
            get
            {
                return Convert.ToHexString(this.key);
            }
        }

        /// <summary>
        /// GenerateHmac method.
        /// </summary>
        /// <param name="move">Move.</param>
        /// <returns>String representation of HMAC.</returns>
        public string GenerateHmac(int move)
        {
            using (var rngCrypt = new RNGCryptoServiceProvider())
            {
                this.key = new byte[KeyLength];
                rngCrypt.GetBytes(this.key);
            }

            using var hmac = new HMACSHA256(this.key);
            byte[] hashValue = hmac.ComputeHash(BitConverter.GetBytes(move));
            return Convert.ToHexString(hashValue);
        }
    }
}
