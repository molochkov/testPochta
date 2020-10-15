namespace PublisherConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CoreTestPochta.Models;
    using NATS.Client;    
    using PublisherConsole.Models;
    using PublisherConsole.Models.Database;

    /// <summary>
    /// Сервис работы с брокером.
    /// </summary>
    public interface IMessageBrokerService
    {
        /// <summary>
        /// Получить статус подключения к брокеру.
        /// </summary>
        /// <returns>результат true/false.</returns>
        bool GetStatus();

        /// <summary>
        /// Переподключиться к брокеру.
        /// </summary>
        /// <returns>Если true - подключение успешно.</returns>
        bool ReConnect();

        /// <summary>
        /// Отравка сообщения через брокера.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Ответное сообщение.</returns>
        Task<MessageConfirm> SendMessageAsync(Message message);
    }
}
