using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class SearchNoteUniConCouCateType
    {
        public IEnumerable<SellerNote> sellerNotes { get; set; }
        public IEnumerable<NoteCategory> noteCategory { get; set; }
        public IEnumerable<NoteType> noteType { get; set; }
        public IEnumerable<Country> country { get; set; }
        public IEnumerable<SelectListItem> university { get; set; }
        public IEnumerable<SelectListItem> Cource { get; set; }
        public IEnumerable<SelectListItem> rating { get; set; }
        public string catgry { get; set; }
        public string typ { get; set; }
        public string cntry { get; set; }
        public string unvrsty { get; set; }
        public string cors { get; set; }
        public string rtng { get; set; }
    }
}