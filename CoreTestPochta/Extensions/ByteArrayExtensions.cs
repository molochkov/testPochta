using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CoreTestPochta.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Метод расширения, считае хэш для массима Byte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CalculateHash(this byte[] input)
        {
            MD5 md5Hasher = MD5.Create();
            StringBuilder sBuilder = new StringBuilder();
            byte[] data = md5Hasher.ComputeHash(input);
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
