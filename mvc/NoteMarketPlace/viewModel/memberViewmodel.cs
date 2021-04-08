using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class memberViewmodel
    {
        public string first { get; set; }
        public string last { get; set; }
        public string email { get; set; }
        public DateTime joiningDate { get; set; }
        public int underreview { get; set; }
        public int publishednotes { get; set; }
        public int downloadnotes { get; set; }
        public int expenses { get; set; }
        public int earnings { get; set; }
        public int memberID { get; set; }
        public int noteid { get; set; }
    }
}