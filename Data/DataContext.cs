using Forum_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum_Application_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        //TABLES
        public DbSet<User> Users { get; set; }
        public DbSet<ForumThread> Threads { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForumThread>().HasOne(c => c.User).WithMany(u => u.Threads).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(c => c.Thread).WithMany(u => u.Comments).HasForeignKey(c => c.ThreadId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
