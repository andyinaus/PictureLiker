using System;
using Microsoft.EntityFrameworkCore;

namespace PictureLiker.DAL
{
    public class PictureLikerContext : DbContext
    {
        public PictureLikerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}