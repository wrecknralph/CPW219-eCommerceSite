using Microsoft.EntityFrameworkCore;
using CPW219_eCommerceSite.Models;
using System.Reflection.Metadata;

namespace CPW219_eCommerceSite.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Users>(entity => { 
                entity.HasKey(k => k.UsersID);
            });
            modelBuilder.Entity<Users>().HasAlternateKey(c => c.UserName).HasName("IX_SingeColumn");
        }
    }
}
