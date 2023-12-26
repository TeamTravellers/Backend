﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelID { get; set; }

        [Required] 
        public string HotelName { get; set; }

        [Required]
        public string Address { get; set;}

        [Required]
        public string Town { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string Details { get; set; }
        
    }
}