using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Mapping;
using System;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMap());
        }

        public DbSet<Product> Products { get; set; }

    }
}
