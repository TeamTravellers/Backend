using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class RoomAvailability
    {
        [Key]
        public int ID { get; set; }

        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public int Availability {  get; set; }

        [NotMapped]
        public Room room { get; set; }

    }
}
