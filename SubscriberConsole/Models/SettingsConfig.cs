namespace SubscriberConsole.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
        /// Gets or sets таймаут сервера NAT.
        /// </summary>
        public int NatTimeout { get; set; }
    }
}
