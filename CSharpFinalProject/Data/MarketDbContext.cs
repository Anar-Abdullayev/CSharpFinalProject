using CSharpFinalProject.EntityConfigurations;
using CSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace CSharpFinalProject.Data
{
    internal class MarketDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MarketDbSqlConnection"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SellHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SellHistory> SellHistories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
