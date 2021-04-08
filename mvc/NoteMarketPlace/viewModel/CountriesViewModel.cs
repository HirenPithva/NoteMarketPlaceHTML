using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class CountriesViewModel
    {
        public int countryID { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public DateTime dateAdded { get; set; }
        public string addedBy { get; set; }
        public string active { get; set; }
    }
}