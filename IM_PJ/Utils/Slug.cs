using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IM_PJ
{
    public class Slug
    {
        public static string ConvertToSlug(string value)
        {
            //First to lower case
            value = value.ToLower();

            //Remove all accents
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            value = value.Normalize(System.Text.NormalizationForm.FormD);
            value = regex.Replace(value, String.Empty);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}