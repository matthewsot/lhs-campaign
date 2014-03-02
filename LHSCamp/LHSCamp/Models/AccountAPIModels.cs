using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LHSCamp.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public string Position { get; set; }
    }
}