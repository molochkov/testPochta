namespace PublisherConsole.Services
{
    using PublisherConsole.Models.Database;

    /// <summary>
    /// Сервис работы с очередью.
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// Получить кол-во сообщений в очереди.
        /// </summary>
        /// <returns>Кол-во элементов.</returns>
        int GetCount();

        /// <summary>
        /// Добавить сообщение в очередь.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void AddToQueue(Message message);

        /// <summary>
        /// Получить первый элемент из очереди.
        /// </summary>
        /// <returns>Сообщение.</returns>
        Message GetFirstItem();

       /// <summary>
       /// Удалить первый элемент из очереди
       /// </summary>
        void RemoveFirstItem();

    }

}
