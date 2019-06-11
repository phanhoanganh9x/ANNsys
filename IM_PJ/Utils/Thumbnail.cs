using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace IM_PJ.Utils
{
    public class Thumbnail
    {
        public enum Size
        {
            Source,
            Micro,
            Small,
            Normal,
            Large
        }

        public static string getURL(string image, Size size)
        {
            if (String.IsNullOrEmpty(image))
            {
                return String.Empty;
            }

            var directory = String.Empty;
            switch (size)
            {
                case Size.Micro:
                    directory = "/85x85";
                    break;
                case Size.Small:
                    directory = "/160x160";
                    break;
                case Size.Normal:
                    directory = "/230x230";
                    break;
                case Size.Large:
                    directory = "/350x350";
                    break;
                default:
                    directory = String.Empty;
                    break;
            }
            return String.Format("/uploads/images{0}/{1}", directory, image);
        }

        public static bool create(string path_file, int with, int height)
        {
            if (!File.Exists(path_file))
            {
                return false;
            }

            var directory = Path.GetDirectoryName(path_file);
            var filename = Path.GetFileName(path_file);
            var image = Image.FromFile(path_file);

            var dir_thumb_1 = String.Format("{0}/{1}x{2}", directory, with, height);
            if (!Directory.Exists(dir_thumb_1))
            {
                Directory.CreateDirectory(dir_thumb_1);
            }
            var path_thumb_1 = String.Format("{0}/{1}", dir_thumb_1, filename);
            if (!File.Exists(path_thumb_1))
            {
                var thumb1 = image.GetThumbnailImage(with, height, () => false, IntPtr.Zero);
                thumb1.Save(path_thumb_1);
            }

            return true;
        }
    }
}