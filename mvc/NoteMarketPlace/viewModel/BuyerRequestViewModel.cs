using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class BuyerRequestViewModel
    {
        public int buyer { get; set; }
        public int noteID { get; set; }
        public string Title { get; set; }
        public string category { get; set; }
        public string emailID { get; set; }
        public string phoneNumber { get; set; }
        public bool sellType { get; set; }
        public int price { get; set; }
        public DateTime DownloadedTime { get; set; }
    }
}