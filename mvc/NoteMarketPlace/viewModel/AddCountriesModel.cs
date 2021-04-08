using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class AddCountriesModel
    {
        public Nullable<int> ID { get; set; }
        [Required]
        [Display(Name = "Country name*")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Country Code*")]
        public string Code { get; set; }
    }
}