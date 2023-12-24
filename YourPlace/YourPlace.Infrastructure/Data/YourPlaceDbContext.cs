using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourPlace.Infrastructure.Data.Entities;
using Microsoft.SqlServer;


namespace YourPlace.Infrastructure.Data
{
    public class YourPlaceDbContext : DbContext
    {
        public YourPlaceDbContext(DbContextOptions<YourPlaceDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Suggestion> Suggestions { get; set; }
    }
}
