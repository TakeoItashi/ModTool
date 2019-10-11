using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModTool
{
    public struct Pixel
    {
        public short x, y;

        public Pixel(short _x, short _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Pixel pixel1, Pixel pixel2)
        {
            return (pixel1.x == pixel2.x && pixel1.y == pixel2.y);
        }

        // this is second one '!='
        public static bool operator !=(Pixel pixel1, Pixel pixel2)
        {
            return !(pixel1.x == pixel2.x && pixel1.y == pixel2.y);
        }

        // this is third one 'Equals'
        public override bool Equals(object pixel)
        {
            if (pixel is Pixel)
            {
                return (x == ((Pixel)pixel).x && y == ((Pixel)pixel).y);
            }
            else
            {
                return false;
            }
        }
    }
}
