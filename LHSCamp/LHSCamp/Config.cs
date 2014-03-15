using LHSCamp.Models;
using System.Collections.Generic;
using System.Linq;

namespace LHSCamp
{
    public class Config
    {
        public static string GetValue(string Key, string Default = "")
        {
            using (LCDB db = new LCDB())
            {
                var val = db.Config.FirstOrDefault(a => a.Key == Key).Value;
                if (val == null)
                {
                    return Default;
                }
                return val;
            }
        }

        public static Dictionary<string, string> GetValues(string[] Keys)
        {
            using (LCDB db = new LCDB())
            {
                var gotten = db.Config.Where(a => Keys.Contains(a.Key));
                Dictionary<string, string> Resp = new Dictionary<string, string>();
                foreach (var Got in gotten)
                {
                    if (Got != null)
                    {
                        Resp.Add(Got.Key, Got.Value);
                    }
                }
                return Resp;
            }
        }
    }
}