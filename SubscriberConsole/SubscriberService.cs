namespace SubscriberConsole
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
    using Microsoft.Extensions.Logging;
    using NATS.Client;
    using Newtonsoft.Json;
    using SubscriberConsole.DAL;

    using SubscriberConsole.Models;
    using SubscriberConsole.Models.Database;
    using SubscriberConsole.Services;

    public class SubscriberService : IHostedService
    {
        private readonly MessageBrokerService messageBrokerService;

        public SubscriberService(
            MessageBrokerService messageBrokerService
            )
        {
            this.messageBrokerService = messageBrokerService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.messageBrokerService.Start().GetAwaiter().GetResult();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
    }
}
