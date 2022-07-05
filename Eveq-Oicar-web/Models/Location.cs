using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Location
    {

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ID nemoze biti 0 niti negativan broj")]
        public int IDLocation { get; set; }


        [Required]
        public string City { get; set; }


        [Required]
        public string Street { get; set; }


        [Required]
        public string Coordinates { get; set; }
    }
}
