#region .NET Framework
using System;
using System.Text.RegularExpressions;
#endregion

namespace IM_PJ.Utils
{
    public class AnnImage
    {
        public static string extractImage(string url)
        {
            var result = String.Empty;
            var rgx = new Regex(@"[\w\d_\-\.]+$");
            var match = rgx.Match(url);

            if (match.Success)
                result = match.Value;

            return result;
        }
    }
}