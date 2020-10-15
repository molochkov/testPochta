using SubscriberConsole.DAL;
using SubscriberConsole.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriberConsole.Repository
{

    public class MsSqlMessageRepository : IRepository<Message>
    {
        private readonly SubscriberDbContext context;

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlMessageRepository"/> class.
        /// </summary>
        /// <param name="context">контекст базы</param>
        public MsSqlMessageRepository(SubscriberDbContext context)
        {
            this.context = context;
        }

        public void CreateAndSave(Message item)
        {
                this.context.Add(item);
                this.context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
