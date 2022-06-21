using Microsoft.AspNetCore.Http;
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

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public byte[] PosterImage { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false,
        DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime Date { get; set; }

        public Location Location { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Event mora imati id lokacije koja nije 0")]
        public int? LocationId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Odredite cijenu eventa da bude veca od 0")]
        public float TicketPrice { get; set; }

    }

}
