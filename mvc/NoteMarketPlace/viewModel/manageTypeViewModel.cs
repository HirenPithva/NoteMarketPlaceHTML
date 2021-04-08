using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class manageTypeViewModel
    {
        public int typeID { get; set; }
        public string Type{ get; set; }
        public string description { get; set; }
        public DateTime dateAdded { get; set; }
        public string addedBy { get; set; }
        public string active { get; set; }
    }
}