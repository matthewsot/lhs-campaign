using LHSCamp.Models;
using System.Collections.Generic;
using System.Linq;

namespace LHSCamp
{
    public class Config
    {
        public static string GetValue(string key, string defaultValue = "")
        {
            using (var db = new LCDB())
            {
                var val = db.Config.FirstOrDefault(a => a.Key == key);
                return (val == null) ? defaultValue : val.Value;
            }
        }

        public static Dictionary<string, string> GetValues(string[] keys)
        {
            using (var db = new LCDB())
            {
                var gotten = db.Config.Where(a => keys.Contains(a.Key));
                return gotten.Where(got => got != null).ToDictionary(got => got.Key, got => got.Value);
            }
        }
    }
}