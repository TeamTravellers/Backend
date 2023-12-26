using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }
        public string ImageURL {  get; set; }

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }
    }
}
