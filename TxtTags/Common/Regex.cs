using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TxtTags.Common
{
    public class RegexHelper
    {
        public static string Search(string input, string pattern)
        {
            Regex r = new Regex(pattern);
            Match match = r.Match(input);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }


    }
}
