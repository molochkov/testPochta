namespace PublisherConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using PublisherConsole.Infractructure;
    using PublisherConsole.Models;
    using PublisherConsole.Models.Database;
    using PublisherConsole.Services;

    /// <summary>
    /// Сервис отправки сообщений из очереди.
    /// </summary>
    public class SenderService : ISenderService
    {
        /// <summary>
        /// Сервис работы с очередью.
        /// </summary>
        private readonly IQueueService queueService;

        /// <summary>
        /// Сервис работы с брокером сообщений.
        /// </summary>
        private readonly IMessageBrokerService messageBrokerService;

        /// <summary>
        /// Настройки.
        /// </summary>
        private readonly SettingsConfig settingsConfig;

        /// <summary>
        /// Репозиторий работы с сообщениями.
        /// </summary>
        private readonly IRepository<Message> messageRepo;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<SenderService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SenderService"/> class.
        /// </summary>
        /// <param name="queueService">Сервис работы с очередью.</param>
        /// <param name="messageBrokerService">Сервис работы с брокером сообщений.</param>
        /// <param name="settingsConfig">Настройки.</param>
        /// <param name="messageRepo">Репозиторий работы с сообщениями.</param>
        public SenderService(
            IQueueService queueService,
            IMessageBrokerService messageBrokerService,
            SettingsConfig settingsConfig,
            IRepository<Message> messageRepo,
            ILogger<SenderService> logger)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.messageBrokerService = messageBrokerService;
            this.settingsConfig = settingsConfig;
            this.messageRepo = messageRepo;
        }

        /// <summary>
        /// Запуск сервиса.
        /// </summary>
        public async Task Start()
        {
            while (true)
            {
                // проверяем наличие сообщений в очереди
                if (this.queueService.GetCount() > 0)
                {
                    this.logger.LogInformation($"{DateTime.Now} QueueMessage.Count={this.queueService.GetCount()}");
                    try
                    {
                        // получаем первый элемент в очереди
                        var currentMessagetoSend = this.queueService.GetFirstItem();

                        // если брокер работает , то отправляем сообщение
                        if (this.messageBrokerService.GetStatus())
                        {
                            var returnMessage = await this.messageBrokerService.SendMessageAsync(currentMessagetoSend);

                            // от слушателя пришел ответ, сообщение он принял, удаляем из очереди сообщение
                            if (returnMessage != null && returnMessage.Id == currentMessagetoSend.Id)
                            {
                                this.logger.LogInformation($"{DateTime.Now} Сообщение id={returnMessage.Id} получено подписчиком.");
                                this.queueService.RemoveFirstItem();
                                currentMessagetoSend.DtSend = DateTime.Now;
                                this.messageRepo.UpdateAndSave(currentMessagetoSend);
                            }
                        }
                        else
                        {
                            // реконнект к брокеру сообщений
                            this.messageBrokerService.ReConnect();
                        }
                    }
                    catch (Exception ee)
                    {
                        this.logger.LogError($"{ee.Message}");
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
