using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Member
    {
        public Member(string uID, string firstName, string lastName, string refferalCode, bool isAdmin, int numberOfRefferals, bool membershipValid)
        {
            UID = uID;
            FirstName = firstName;
            LastName = lastName;
            RefferalCode = refferalCode;
            IsAdmin = isAdmin;
            NumberOfRefferals = numberOfRefferals;
            MembershipValid = membershipValid;
        }

        public Member()
        {

        }

        public string UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefferalCode { get; set; }
        public bool IsAdmin { get; set; }
        public int NumberOfRefferals { get; set; }
        public bool MembershipValid { get; set; }


    }
}
