namespace PublisherConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CoreTestPochta.Extensions;
    using Newtonsoft.Json;
    using PublisherConsole.Infractructure;
    using PublisherConsole.Models;
    using PublisherConsole.Models.Database;
    using PublisherConsole.Services;

    public class MessageGeneratorService : IMessageGeneratorService
    {
        /// <summary>
        /// Сервис работы с очередью.
        /// </summary>
        private readonly IQueueService queueService;

        /// <summary>
        /// Настройки.
        /// </summary>
        private readonly SettingsConfig settingsConfig;

        /// <summary>
        /// Репозиторий работы с сообщениями.
        /// </summary>
        private readonly IRepository<Message> messageRepo;

        /// <summary>
        /// локер.
        /// </summary>
        private static readonly object messageLocker = new object();

        /// <summary>
        /// Таймер.
        /// </summary>
        private Timer timer;
        private TimerCallback tmCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGeneratorService"/> class.
        /// </summary>
        /// <param name="queueService">Сервис работы с очередью.</param>
        /// <param name="settingsConfig">Настройки.</param>
        /// <param name="messageRepo">Репозиторий работы с сообщениями.</param>
        public MessageGeneratorService(
            IQueueService queueService,
            SettingsConfig settingsConfig,
            IRepository<Message> messageRepo)
        {
            this.queueService = queueService;
            this.settingsConfig = settingsConfig;
            this.messageRepo = messageRepo;
        }

        public void Start()
        {
            this.tmCallback = new TimerCallback(this.GenerateMessage);
            this.timer = new Timer(this.tmCallback, 0, 0, this.settingsConfig.IntervalGenerateMessage);
        }

        /// <summary>
        /// Калбэк таймера.
        /// </summary>
        /// <param name="obj">Входящий параметр</param>
        private void GenerateMessage(object obj)
        {
            if (Monitor.IsEntered(messageLocker))
            {
                Monitor.Wait(messageLocker);
            }

            try
            {
                Monitor.Enter(messageLocker);

                // Создаем сообщение
                Message message = new Message();

                // Генерация случайной строки
                message.MessageSend = string.Empty.Generate(this.settingsConfig.LengthString);
                message.DtCrerate = DateTime.Now;

                // Расчитываем Хэш сумму записи совмещенную с хешом последней записи
                message.Hash = this.CalculateHash(message).GetAwaiter().GetResult();

                // Сохраняем в базу
                this.messageRepo.CreateAndSave(message);

                // Добавляем в очередь на отправку
                this.queueService.AddToQueue(message);
                Monitor.PulseAll(messageLocker);
            }
            finally
            {
                Monitor.Exit(messageLocker);
            }
        }
        /// <summary>
        /// Расчет хэш  суммы.
        /// </summary>
        /// <param name="model">сообщение.</param>
        /// <returns>Расчитанный хэш.</returns>
        private async Task<string> CalculateHash(Message model)
        {
            // сериализуем текущую запись и считае хэш
            string result = model.Serialize().CalculateHash();
            var lastM = await this.messageRepo.GetLastRowAsync();
            if (lastM != null)
            {
                // если в базе есть предыдущая запись, берем ее хеш,
                // делаем строку из двух хэешей, и считаем хеш полученной строки
                result = $"{lastM.Hash}__{result}".Serialize().CalculateHash();
            }

            return result;
        }
    }
}
