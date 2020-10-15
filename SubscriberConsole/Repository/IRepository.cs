using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriberConsole.Repository
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Создать и сохранить запись.
        /// </summary>
        /// <param name="item"></param>
        void CreateAndSave(T item);
    }
}