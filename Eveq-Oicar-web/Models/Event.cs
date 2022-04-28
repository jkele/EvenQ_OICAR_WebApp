using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Event
    {

        public int IDEvent { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public byte[] PosterImage { get; set; }

        public DateTime Date { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }
        public float TicketPrice { get; set; }
    }
}
