namespace PublisherConsole.Infractructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PublisherConsole.DAL;
    using PublisherConsole.Models.Database;

    public class MsSqlMessageRepository : IRepository<Message>
    {
        private readonly PublisherDbContext context;

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlMessageRepository"/> class.
        /// </summary>
        /// <param name="context">контекст базы</param>
        public MsSqlMessageRepository(PublisherDbContext context)
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

        public async Task<Message> GetLastRowAsync()
        {
            return await this.context.Messages.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetMessageToSendAsync()
        {
            return await this.context.Messages.Where(x => x.DtSend == null).OrderBy(x => x.Id).ToListAsync();
        }

        public void UpdateAndSave(Message item)
        {
            this.context.Entry(item).State = EntityState.Modified;
            this.context.SaveChanges();
        }
    }
}
