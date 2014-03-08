using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LHSCamp.Models
{
    public class UserNameModel
    {
        public string Username { get; set; }
    }
    public class SetEmailModel
    {
        public string email { get; set; }
    }
    public class SetReasonsModel
    {
        public string reasons { get; set; }
    }
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public string Position { get; set; }
    }
}