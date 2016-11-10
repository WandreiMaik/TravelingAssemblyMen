using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingAssemblyMen.Model
{
    public static class ColorPicker
    {
        private static Color[] colors =
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Brown,
            Color.Orange,
            Color.Black,
            Color.DarkCyan,
            Color.Gray
        };

        private static int colorIndex = 0;

        public static Color NextColor
        {
            get
            {
                Color nextColor = colors[colorIndex];
                colorIndex++;
                colorIndex %= colors.Count();

                return nextColor;
            }
        }

        public static void Reset()
        {
            colorIndex = 0;
            return;
        }
    }
}
