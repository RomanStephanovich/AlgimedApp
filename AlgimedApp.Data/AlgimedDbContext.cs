using AlgimedApp.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace AlgimedApp.Data
{
    public class AlgimedDbContext : DbContext
    {
        public AlgimedDbContext(DbContextOptions<AlgimedDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mode> Modes { get; set; }
        public DbSet<Step> Steps { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mode>()
                .HasKey(m => m.ID);
            modelBuilder.Entity<Step>()
                .HasKey(s => s.ID);
            modelBuilder.Entity<Step>()
                .HasOne(s => s.Mode)
                .WithMany(m => m.Steps)
                .HasForeignKey(s => s.ModeId);
        }
    }
}