using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IM_PJ
{
    public class Slug
    {
        public static string ConvertToSlug(string value, bool isFile = false, string extension = "")
        {
            if (isFile)
            {
                if (String.IsNullOrEmpty(extension))
                    extension = Path.GetExtension(value);
                //Get file name
                value = Path.GetFileNameWithoutExtension(value);
            }

            //First to lower case
            value = value.ToLower();

            //Remove all accents
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            value = value.Normalize(System.Text.NormalizationForm.FormD);
            value = regex.Replace(value, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            //Replace spaces or _
            value = Regex.Replace(value, @"[\s_\.]", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value + extension;
        }
    }
}