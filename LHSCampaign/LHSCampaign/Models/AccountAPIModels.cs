namespace LHSCampaign.Models
{
    public class UserNameModel
    {
        public string username { get; set; }
    }

    public class SetEmailModel
    {
        public string email { get; set; }
    }

    public class SetPositionModel
    {
        public string position { get; set; }
    }

    public class SetReasonsModel
    {
        public string reasons { get; set; }
    }

    public class SetSocialModel
    {
        public string facebook { get; set; }
        public string youtube { get; set; }
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