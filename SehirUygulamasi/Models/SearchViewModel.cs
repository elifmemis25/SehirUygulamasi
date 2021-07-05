using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirUygulamasi.Models
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }
        public bool ShowAll { get; set; }

        public int CategoryId { get; set; }

        public List<GezilecekSehirler> Result { get; set; }
    }
}
