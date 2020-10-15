namespace SubscriberConsole.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using SubscriberConsole.Models.Database;

    public class SubscriberDbContext : DbContext
    {

        public DbSet<Message> Messages { get; set; }

        public SubscriberDbContext()
        {

        }

        public SubscriberDbContext(DbContextOptions<SubscriberDbContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Subscriber;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
