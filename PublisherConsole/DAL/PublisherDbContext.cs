namespace PublisherConsole.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using PublisherConsole.Models.Database;

    public class PublisherDbContext : DbContext
    {

        public DbSet<Message> Messages { get; set; }


        public PublisherDbContext(DbContextOptions<PublisherDbContext> options)
           : base(options)
        {
        }

        public PublisherDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Publisher;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
