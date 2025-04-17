using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PokeHelper.Classes
{
    public class ImageProcessing
    {
        public static Image GetTypeImage(string type)
        {
            // Use lowercase and trim just in case
            type = type?.ToLower().Trim();

            // Default to fallback image if needed
            string imagePath = $"Images/{type}.png";
            if (Application.GetResourceStream(new Uri(imagePath, UriKind.Relative)) == null)
            {
                imagePath = "Images/default.png";
            }

            Image image = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/{imagePath}", UriKind.Absolute)),
                Width = 100,
                Height = 100,
                Margin = new Thickness(5)
            };

            return image;
        }

    }
}
