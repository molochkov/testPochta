using System;
using System.Collections.Generic;
using System.Text;

namespace PublisherConsole.Models
{
    /// <summary>
    /// модель настроек.
    /// </summary>
    public class SettingsConfig
    {
        /// <summary>
        /// Gets or sets название очереди.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets период генерации новых сообщений,сек.
        /// </summary>
        public int IntervalGenerateMessage { get; set; }

        /// <summary>
        /// Gets or sets длинна генерируемого сообщения.
        /// </summary>
        public int LengthString { get; set; }

        /// <summary>
        /// Gets or sets таймаут сервера NAT.
        /// </summary>
        public int NatTimeout { get; set; }
    }
}
