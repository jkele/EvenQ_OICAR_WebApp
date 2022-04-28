using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Location
    {
        public int IDLocation { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Coordinates { get; set; }
    }
}
