using EarlyBird.Models;
using Microsoft.EntityFrameworkCore;

namespace EarlyBird.Contexts
{
    public class ScheduleDbContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=gym.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}