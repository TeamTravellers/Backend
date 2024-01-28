using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Tourism { get; set; }

        [Required]
        public string Atmosphere { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public decimal Pricing { get; set; }

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }

        [Required]
        public Hotel Hotel { get; set; }
        public Categories()
        {
            
        }
        public Categories(string location, string tourism, string atmosphere, string company, decimal pricing, int hotelID)
        {
            Location = location;
            Tourism = tourism;
            Atmosphere = atmosphere;
            Company = company;
            Pricing = pricing;
            HotelID = hotelID;
        }
    }
}
