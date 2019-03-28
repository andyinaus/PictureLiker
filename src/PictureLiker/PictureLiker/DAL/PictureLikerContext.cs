using Microsoft.EntityFrameworkCore;

namespace PictureLiker.DAL
{
    public class PictureLikerContext : DbContext
    {
        public PictureLikerContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
    }
}