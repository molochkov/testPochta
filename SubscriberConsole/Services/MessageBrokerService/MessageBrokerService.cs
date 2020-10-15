namespace SubscriberConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CoreTestPochta.Extensions;
    using CoreTestPochta.Models;
    using Microsoft.Extensions.Logging;
    using NATS.Client;
    using Newtonsoft.Json;
    using SubscriberConsole.Models;
    using SubscriberConsole.Models.Database;
    using SubscriberConsole.Repository;


    /// <summary>
    /// Сервис работы с брокером.
    /// </summary>
    public class MessageBrokerService
    {
        private readonly SettingsConfig settingsConfig;
        private readonly IRepository<Message> messageRepo;
        private readonly ILogger<MessageBrokerService> logger;
        private readonly IMapper mapper;


        private CancellationTokenSource cts;
        private IConnection subConnection;
        private Task responder;

        /// <summary>
        /// Статус брокера.
        /// </summary>
        private bool status;


        private static readonly object messageLocker = new object();


        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBrokerService"/> class.
        /// </summary>
        /// <param name="settingsConfig">настройки</param>
        /// <param name="logger">Логгер</param>
        /// <param name="messageRepo">репозиторий для сообщений</param>
        public MessageBrokerService(SettingsConfig settingsConfig, ILogger<MessageBrokerService> logger, IRepository<Message> messageRepo, IMapper mapper)
        {
            this.settingsConfig = settingsConfig;
            this.status = this.InitNat();
            this.logger = logger;
            this.messageRepo = messageRepo;
            this.mapper = mapper;
        }

        public async Task Start()
        {
            while (true)
            {
                if (!this.status)
                {
                    InitNat();
                }


                Thread.Sleep(5000);
            }
        }


        private void ResponderWork()
        {
            using (var s = this.subConnection.SubscribeSync(this.settingsConfig.QueueName))
            {
                while (!cts.IsCancellationRequested)
                {

                    if (Monitor.IsEntered(messageLocker))
                    {
                        Monitor.Wait(messageLocker);
                    }

                    try
                    {
                        Monitor.Enter(messageLocker);
                        var inMess = s.NextMessage();
                        var message = (MessageTransfer)inMess.Data.Deserialize<MessageTransfer>();
                        var serializeMessage = JsonConvert.SerializeObject(message);
                        this.logger.LogInformation($"{DateTime.Now} Пришло сообщение {serializeMessage}");
                        if (!cts.IsCancellationRequested)
                        {
                            MessageConfirm mc = new MessageConfirm();
                            mc.Id = message.Id;
                            this.subConnection.Publish(inMess.Reply, mc.Serialize());
                            this.subConnection.Flush();
                        }

                        // сохраняем в базе полученное сообщение
                        var messageToSave = this.mapper.Map<Message>(message);
                        messageToSave.DtComing = DateTime.Now;
                        messageToSave.Id = 0;
                        this.messageRepo.CreateAndSave(messageToSave);

                        Monitor.PulseAll(messageLocker);
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError($"{DateTime.Now} {e.Message}");
                        this.status = false;
                    }
                    finally
                    {
                        Monitor.Exit(messageLocker);
                    }

                }
            }
        }

        /// <summary>
        /// Инициализация подключения к брокеру.
        /// </summary>
        /// <returns>Если true - подключение успешно</returns>
        private bool InitNat()
        {
            try
            {
                this.cts = new CancellationTokenSource();
                var pubOptions = this.GetOptions();
                var cnFac = new ConnectionFactory();
                this.subConnection = cnFac.CreateConnection(pubOptions);

                responder = Task.Factory.StartNew(
                                ResponderWork,
                                cts.Token,
                                TaskCreationOptions.LongRunning,
                                TaskScheduler.Default);
                this.status = true;
            }
            catch (Exception)
            {
                this.status = false;
            }

            return this.status;
        }

        Options GetOptions()
        {
            var options = ConnectionFactory.GetDefaultOptions();
            options.NoEcho = true;
            return options;
        }


    }
}
