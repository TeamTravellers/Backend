using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourPlace.Infrastructure.Data.Entities;
using Microsoft.SqlServer;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace YourPlace.Infrastructure.Data
{
    public class YourPlaceDbContext : IdentityDbContext<User>
    {
        public YourPlaceDbContext(DbContextOptions<YourPlaceDbContext> options) : base(options) { }
        
        //public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Suggestion> Suggestions { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<RoomAvailability> RoomsAvailability { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().Property(r => r.Type).HasConversion<string>();
            modelBuilder.Entity<RoomAvailability>().Property(r=>r.Type).HasConversion<string>();
           

            base.OnModelCreating(modelBuilder);
        }
    }
}
