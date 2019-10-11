using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModTool
{
    public struct RGBColor
    {
        public byte red, blue, green;

        public static bool operator ==(RGBColor color1, RGBColor color2)
        {
            return (color1.red == color2.red
                    && color1.green == color2.green
                    && color1.blue == color2.blue);
        }

        // this is second one '!='
        public static bool operator !=(RGBColor color1, RGBColor color2)
        {
            return !(color1.red == color2.red
                    && color1.green == color2.green
                    && color1.blue == color2.blue);
        }

        // this is third one 'Equals'
        public override bool Equals(object color)
        {
            if (color is RGBColor)
            {

                return (red == ((RGBColor)color).red
                     && green == ((RGBColor)color).green
                     && blue == ((RGBColor)color).blue);
            }
            else
            {
                return false;
            }
        }
    }
}