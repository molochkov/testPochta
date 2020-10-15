namespace PublisherConsole
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PublisherConsole.DAL;
    using PublisherConsole.Infractructure;
    using PublisherConsole.Models;
    using PublisherConsole.Models.Database;
    using PublisherConsole.Services;

    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Сервис отправки сообщений запущен...");

            // Делаем миграцию в базу, если это необходимо
            PublisherDbContext context = new PublisherDbContext();
            context.Database.Migrate();

            var builder = CreateHostBuilder(args);
            await builder.RunConsoleAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    string envName = hostContext.HostingEnvironment.EnvironmentName;
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddLogging(configure => configure.AddConsole().AddFilter("Microsoft", LogLevel.None)
                    ).Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                    string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<PublisherDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

                    services.AddTransient<IRepository<Message>, MsSqlMessageRepository>();
                    services.AddSingleton<IQueueService, QueueService>();
                    services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
                    services.AddSingleton<ISenderService, SenderService>();
                    services.AddSingleton<IMessageGeneratorService, MessageGeneratorService>();
                    services.AddHostedService<PublisherService>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                    SettingsConfig settingsConfig = new SettingsConfig();
                    hostContext.Configuration.GetSection("SettingsConfig").Bind(settingsConfig);
                    services.AddSingleton(settingsConfig);

                    services.AddHostedService<PublisherService>();
                });
            return builder;
        }

    }

    public class BloggingContextFactory : IDesignTimeDbContextFactory<PublisherDbContext>
    {
        public PublisherDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PublisherDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=Publisher;Trusted_Connection=True;");

            return new PublisherDbContext(optionsBuilder.Options);
        }
    }
}
