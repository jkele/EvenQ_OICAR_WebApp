﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Ticket
    {
        public int IDTicket { get; set; }
        public string TicketQR { get; set; }
        public Member Member { get; set; }
        public string MemberId { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }
        public bool IsValid { get; set; }
    }
}
