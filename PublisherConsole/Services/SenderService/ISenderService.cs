namespace PublisherConsole.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Сервис отправки сообщений из очереди.
    /// </summary>
    public interface ISenderService
    {
        /// <summary>
        /// Запуск сервиса.
        /// </summary> 
        Task Start();
    }
}
