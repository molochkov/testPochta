using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTestPochta.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Метод расширения, генерирует строку с символами заданной длинны.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="countChars"></param>
        /// <returns></returns>
        public static string Generate(this string input, int countChars)
        {
            StringBuilder builder = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < countChars; i++)
            {
                builder.Append(Convert.ToChar(rand.Next(32, 127)));
            }
            return builder.ToString();
        }
    }
}
