using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirUygulamasi.Models
{
    public class CetUser: IdentityUser
    {
       public string Name { get; set; }
        public string Surname { get; set; }

        public virtual List<GezilecekSehirler> GezilecekSehirlers { get; set; }

    }
}
