using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class UserLoginRemember
    {
        public Userlogin userlogins { get; set; }
        [Display(Name ="Remember Me")]
        public bool rememberMe { get; set; }
    }
}