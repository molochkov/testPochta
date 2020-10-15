namespace PublisherConsole
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using NATS.Client;
    using Newtonsoft.Json;
    using PublisherConsole.DAL;
    using PublisherConsole.Infractructure;
    using PublisherConsole.Models;
    using PublisherConsole.Models.Database;
    using PublisherConsole.Services;

    public class PublisherService : IHostedService
    {
        private readonly SettingsConfig settingsConfig;
        private readonly IMessageBrokerService messageBrokerService;
        private readonly IQueueService queueService;
        private readonly ISenderService senderService;
        private readonly IMessageGeneratorService messageGeneratorService;


        public PublisherService(
            SettingsConfig settingsConfig,
            IMessageBrokerService messageBrokerService,
            IQueueService queueService,
            ISenderService senderService,
            IMessageGeneratorService messageGeneratorService
            )
        {
            this.settingsConfig = settingsConfig;
            this.messageBrokerService = messageBrokerService;
            this.queueService = queueService;
            this.senderService = senderService;
            this.messageGeneratorService = messageGeneratorService;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Запускаем сервис генерации новых сообщений
            this.messageGeneratorService.Start();

            // Запускаем сервис отправки сообщений
            this.senderService.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
    }
}
