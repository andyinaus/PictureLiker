using Microsoft.EntityFrameworkCore;
using PictureLiker.DAL.Entities;

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

            builder.Entity<Preference>()
                .Ignore(p => p.Id)
                .HasKey(p => new {p.PictureId, p.UserId});
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Preference> Preferences { get; set; }
    }
}