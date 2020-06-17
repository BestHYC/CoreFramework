using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Framework
{
    public static class SecretHelper
    {
        public static String HMACSHA256(byte[] secretkey, byte[] message)
        {
            using (HMACSHA256 hmac = new HMACSHA256(secretkey))
            {
                // Compute the hash of the input file.
                byte[] hashValue = hmac.ComputeHash(message);
                // Copy the contents of the sourceFile to the destFile.
                return ToHex(hashValue);
            }
        }
        /// <summary>
        /// 通过HMACSHA256给UTF8的密匙加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static String HMACSHA256_UTF8(String key, String message)
        {
            byte[] secretkey = Encoding.UTF8.GetBytes("6q8y3jl5jMelsYwPDYyFp3iskvhJDFh6");
            return HMACSHA256(secretkey, Encoding.UTF8.GetBytes(message));
        }
        /// <summary>
        /// Gets an encoding for the operating system's current ANSI code page.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static String HMACSHA256_Default(String key, String message)
        {
            byte[] secretkey = Encoding.Default.GetBytes(key);
            return HMACSHA256(secretkey, Encoding.Default.GetBytes(message));
        }
        public static String ToHex(byte[] input)
        {
            if (input == null)
            {
                return "";
            }
            StringBuilder output = new StringBuilder(input.Length * 2);
            for (int i = 0; i < input.Length; i++)
            {
                int current = input[i] & 0xff;
                if (current < 16)
                {
                    output.Append('0');
                }
                //output.Append(Convert.ToString(current, 16));
                output.Append(current.ToString("X"));
            }
            return output.ToString();
        }
    }
}
