namespace PublisherConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PublisherConsole.Infractructure;
    using PublisherConsole.Models.Database;

    /// <summary>
    /// Сервис работы с очередью.
    /// </summary>
    public class QueueService : IQueueService
    {
        /// <summary>
        /// репозиторий работы с Сообщениями.
        /// </summary>
        private readonly IRepository<Message> messageRepo;

        /// <summary>
        /// Очередь сообщений.
        /// </summary>
        private Queue<Message> queueMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueService"/> class.
        /// </summary>
        /// <param name="messageRepo"> репозиторий работы с Сообщениями.</param>
        public QueueService(IRepository<Message> messageRepo)
        {
            this.messageRepo = messageRepo;
            this.queueMessage = new Queue<Message>();

            // загружаем в очередь не отправленные сообщения
            List<Message> listNotSendMessage = this.messageRepo.GetMessageToSendAsync().GetAwaiter().GetResult();
            listNotSendMessage.ForEach(x =>
            {
                this.queueMessage.Enqueue(x);
            });
        }

        public int GetCount()
        {
            return this.queueMessage.Count;
        }

        public void AddToQueue(Message message)
        {
            this.queueMessage.Enqueue(message);
        }

        public Message GetFirstItem()
        {
            return this.queueMessage.Peek();
        }

        public void RemoveFirstItem()
        {
            this.queueMessage.Dequeue();
        }
    }

}
