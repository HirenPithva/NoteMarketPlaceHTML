using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class NoteDetailsViewModel
    {
        public SellerNote sellerNote { get; set; }
        public IEnumerable<ReviewList> reviewLists { get; set; }
       public int overallRating { get; set; }
        public int TotalReview { get; set; }
        public int Inappropriate { get; set; }
        public string Imgpath { get; set; }
    }
    public class ReviewList
    {
        public string profileImagePathP { get; set; }
        public string fullName { get; set; }
        public int rating { get; set; }
        public string comments { get; set; }
    }
}