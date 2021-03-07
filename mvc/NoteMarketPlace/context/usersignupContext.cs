using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NoteMarketPlace.Models;

namespace NoteMarketPlace.context
{
    public class usersignupContext 
    {
        public DbSet<usersignup> usersignups { get; set; }
        public DbSet<Userlogin> userlogins { get; set; }
    }
}