namespace SubscriberConsole
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SubscriberConsole.DAL;
    using SubscriberConsole.Models;
    using SubscriberConsole.Models.Database;
    using SubscriberConsole.Repository;
    using SubscriberConsole.Services;

    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Сервис приема сообщений запущен...");

            // Делаем миграцию в базу, если это необходимо
            SubscriberDbContext context = new SubscriberDbContext();
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
                    services.AddDbContext<SubscriberDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
                    services.AddTransient<IRepository<Message>, MsSqlMessageRepository>();                  
                    services.AddSingleton<MessageBrokerService>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                    SettingsConfig settingsConfig = new SettingsConfig();
                    hostContext.Configuration.GetSection("SettingsConfig").Bind(settingsConfig);
                    services.AddSingleton(settingsConfig);

                    services.AddHostedService<SubscriberService>();
                });


            return builder;
        }

    }

    public class BloggingContextFactory : IDesignTimeDbContextFactory<SubscriberDbContext>
    {
        public SubscriberDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SubscriberDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database= Subscriber;Trusted_Connection=True;");

            return new SubscriberDbContext(optionsBuilder.Options);
        }
    }
}
