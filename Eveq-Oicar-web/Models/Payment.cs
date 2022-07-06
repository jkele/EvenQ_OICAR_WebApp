using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Payment
    {
        public Payment(DateTime dateBought, DateTime dateValid, bool isMembership, string uID)
        {
            DateBought = dateBought;
            DateValid = dateValid;
            IsMembership = isMembership;
            UID = uID;
        }

        public Payment(DateTime dateBought, bool isMembership, string uID)
        {
            DateBought = dateBought;
            IsMembership = isMembership;
            UID = uID;
        }

        public int IDPayment { get; set; }

        public DateTime DateBought { get; set; }
        public DateTime DateValid { get; set; }

        public bool IsMembership { get; set; }


        public string UID { get; set; }
    }
}
