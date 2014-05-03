namespace LHSCamp.Models
{
    public class AddCandidateModel
    {
        public int id { get; set; }
    }
    public class CandidateModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string profilePic { get; set; }
        public string coverPhoto { get; set; }
        public bool chosen { get; set; }
    }
}