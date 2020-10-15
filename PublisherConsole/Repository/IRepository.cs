namespace PublisherConsole.Infractructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using PublisherConsole.Models.Database;

    public interface IRepository<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Получить последнюю запись.
        /// </summary>
        /// <returns></returns>
        Task<T> GetLastRowAsync();

        /// <summary>
        /// получить список не отправленных сообщений.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetMessageToSendAsync();

        /// <summary>
        /// Создать и сохранить запись.
        /// </summary>
        /// <param name="item"></param>
        void CreateAndSave(T item);

        /// <summary>
        /// Изменить и сохранить запись.
        /// </summary>
        /// <param name="item"></param>
        void UpdateAndSave(T item);
    }
}
