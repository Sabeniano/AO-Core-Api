using System;
using AoApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AoApi.Data
{
    public class AOContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Workhours> Workhours { get; set; }

        public AOContext(DbContextOptions<AOContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Wallet>()
                .Property(e => e.PaymentMethod)
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumPaymentMethod)Enum.Parse(typeof(EnumPaymentMethod), v));
        }
    }
}
