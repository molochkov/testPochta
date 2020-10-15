using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CoreTestPochta.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// метод расширения, сериализует объект в byte
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(this object obj)
        {
            byte[] data = null;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, obj);
                data = ms.ToArray();
            }

            return data;
        }

        /// <summary>
        /// метод расширения, десереализует из массива byte в необходимый объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datasss"></param>
        /// <returns></returns>
        public static object Deserialize<T>(this byte[] datasss)
        {
            byte[] datas = datasss;
            using (MemoryStream ms = new MemoryStream(datas))
            {
                BinaryFormatter binaryFormatter2 = new BinaryFormatter();
                var objDeserialized = (T)binaryFormatter2.Deserialize(ms);
                return objDeserialized;
            }
        }
    }
}
