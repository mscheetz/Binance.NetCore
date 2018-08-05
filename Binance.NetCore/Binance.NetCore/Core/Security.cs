using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Binance.NetCore.Core
{
    public class Security
    {
        /// <summary>
        /// Get HMAC Signature
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="keySecret">Api key secret</param>
        /// <returns>string of signed message</returns>
        public string GetBinanceHMACSignature(string message, string keySecret)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] messageBytes = encoding.GetBytes(message);
            byte[] keyBytes = encoding.GetBytes(keySecret);
            HMACSHA256 crypotgrapher = new HMACSHA256(keyBytes);

            byte[] bytes = crypotgrapher.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
