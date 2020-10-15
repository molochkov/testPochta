namespace PublisherConsole.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

   
    [Serializable]
    public class Message
    {
        [Key]
        public long Id { get; set; }
        
        /// <summary>
        /// Gets or sets дата создания записи.
        /// </summary>
        public DateTime DtCrerate { get; set; }
        
        /// <summary>
        /// Gets or sets дата отправки записи.
        /// </summary>
        public DateTime? DtSend { get; set; }

        /// <summary>
        /// Gets or sets сообщение.
        /// </summary>
        public string MessageSend { get; set; }

        /// <summary>
        /// Gets or sets хэш.
        /// </summary>
        public string Hash { get; set; }

    }
}
