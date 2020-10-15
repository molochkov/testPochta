namespace PublisherConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CoreTestPochta.Extensions;
    using CoreTestPochta.Models;
    using NATS.Client;    
    using PublisherConsole.Models;
    using PublisherConsole.Models.Database;

    /// <summary>
    /// Сервис работы с брокером.
    /// </summary>
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly SettingsConfig settingsConfig;
        private CancellationTokenSource cts;
        private IConnection pubConnection;
        private readonly IMapper mapper;

        /// <summary>
        /// Статус брокера.
        /// </summary>
        private bool status;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBrokerService"/> class.
        /// </summary>
        /// <param name="settingsConfig">Настройки.</param>
        public MessageBrokerService(SettingsConfig settingsConfig, IMapper mapper)
        {
            this.settingsConfig = settingsConfig;
            this.mapper = mapper;
            this.status = this.InitNat();
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
                this.pubConnection = cnFac.CreateConnection(pubOptions);
                this.status = true;
            }
            catch (Exception)
            {
                this.status = false;
            }

            return this.status;
        }

        /// <summary>
        /// Получить статус подключения к брокеру.
        /// </summary>
        /// <returns>true/false.</returns>
        public bool GetStatus()
        {
            return this.status;
        }

        /// <summary>
        /// Переподключиться к брокеру.
        /// </summary>
        /// <returns>Если true - подключение успешно</returns>
        public bool ReConnect()
        {
            return this.InitNat();
        }

        public async Task<MessageConfirm> SendMessageAsync(Message message)
        {
            // создаем сообщение для оправки на сервер NAT
            Msg msgSend = new Msg();
            msgSend.Subject = this.settingsConfig.QueueName;
            msgSend.Data = this.mapper.Map<MessageTransfer>(message).Serialize();
            try
            {
                Msg responseMesage = await this.pubConnection.RequestAsync(msgSend, this.settingsConfig.NatTimeout);
                return (MessageConfirm)responseMesage.Data.Deserialize<MessageConfirm>();
            }
            catch (Exception e)
            {
                status = false;
            }
            return null;
        }

        Options GetOptions()
        {
            var options = ConnectionFactory.GetDefaultOptions();
            options.NoEcho = true;
            return options;
        }


    }
}
