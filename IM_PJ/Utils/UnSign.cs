using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IM_PJ
{
    public class UnSign
    {
        public static string convert(string value)
        {
            //First to lower case
            value = value.ToLower();

            //Remove all accents
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            value = value.Normalize(System.Text.NormalizationForm.FormD);
            value = regex.Replace(value, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            return value;
        }
    }
}