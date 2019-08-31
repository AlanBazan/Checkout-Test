using Checkout.Consumer.SqlServer.Mapping;
using Checkout.Core;
using Checkout.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Checkout.SqlServerConsumer
{
    public class CheckoutContext : DbContext
    {
        public DbSet<Navigation> Navigations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationDefaults.SQLSERVER_CONNECTIONSTRING);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NavigationMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
