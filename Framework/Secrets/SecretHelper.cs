using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            byte[] secretkey = Encoding.UTF8.GetBytes(key);
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
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">8位字符的密钥字符串</param>
        /// <param name="iv">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string data, string key, string iv)
        {
            if (String.IsNullOrWhiteSpace(key)) key = "";
            if (String.IsNullOrWhiteSpace(iv)) iv = "";
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static String DESDecrypt(string data, string key, string iv)
        {
            if (String.IsNullOrWhiteSpace(key)) key = "";
            if (String.IsNullOrWhiteSpace(iv)) iv = "";
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return "";
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            String result = sr.ReadToEnd();
            return result;
        }
    }
}
