using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModTool
{
    public class ColorIsland
    {
        public List<Pixel> Pixels, BorderPixels;
            
        public ColorIsland()
        {
            BorderPixels = new List<Pixel>();
            Pixels = new List<Pixel>();
        }
    }
}
