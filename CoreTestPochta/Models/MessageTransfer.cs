using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTestPochta.Models
{
    [Serializable]
    public class MessageTransfer
    {      
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets дата создания записи.
        /// </summary>
        public DateTime DtCrerate { get; set; }

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
