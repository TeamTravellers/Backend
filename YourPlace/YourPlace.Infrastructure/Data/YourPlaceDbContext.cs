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
using YourPlace.Infrastructure.Data.Enums;


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
            modelBuilder.Entity<Categories>().Property(r => r.Location).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Tourism).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Atmosphere).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Company).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Pricing).HasConversion<string>();

            modelBuilder.Entity<Image>().HasOne(h => h.Hotel).WithMany().HasForeignKey(x => x.HotelID).IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Suggestion>().HasOne(h => h.User).WithMany().HasForeignKey(x => x.UserID).IsRequired().OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
