using ModTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodCheck {
    class Program {
        static void Main(string[] args)
        {
         
        }

        void FloodCheck(short x, short y, RGBColor _checkColor, ref ColorIsland _island, ref int _count)
        {
            //Flood - check(node,color, island):
            //1.If node ischecked, return.
            //2.If node.color is not equal to color, return.
            //3.Set ischecked for node to true
            //3.create Queue Q
            //4.Add node to island.pixels. 
            //5.Add node to the end of Q.
            //6.While Q is not empty:
            //7.Create new element n and Set n equal to the first element of Q.
            //8.Remove first element from Q.
            //9.If the color of the node to the west of n is equal to color,
            //             Add node to island.pixels and add that node to the end of Q.
            //      Else Add node to island.borderPixels
            //10.If the color of the node to the east of n is equal to color,
            //             Add node to island.pixels and add that node to the end of Q.
            //      Else Add node to island.borderPixels
            //11.If the color of the node to the north of n is equal to color,
            //            Add node to island.pixels and add that node to the end of Q.
            //      Else Add node to island.borderPixels
            //12.If the color of the node to the south of n is equal to color,
            //             Add node to island.pixels and add that node to the end of Q.
            //      Else Add node to island.borderPixels
            //13.Continue looping until Q is exhausted.
            //14.Return.

            _count++;
            //If the pixel has not been checked
            if (!m_checkedPixels[x, y])
            {
                //If Pixel has the right color
                if (GetPixelRGB(x, y) == _checkColor)
                {
                    m_checkedPixels[x, y] = true;
                    Queue<Pixel> queue = new Queue<Pixel>();
                    Pixel pixel = new Pixel(x, y);
                    _island.Pixels.Add(pixel);
                    queue.Enqueue(pixel);
                    while (queue.Count > 0)
                    {
                        _count++;
                        Pixel currentElement = queue.Dequeue();
                        //TODO müssen Rand pixel zu den Border Pixeln hinzugefügt werden?
                        if (x < (m_checkedPixels.GetLength(0) - 1))
                        {
                            if (GetPixelRGB((short)(x + 1), y) == _checkColor)
                            {
                                m_checkedPixels[x + 1, y] = true;
                                _island.Pixels.Add(new Pixel((short)(x + 1), y));
                            } else
                            {
                                _island.BorderPixels.Add(currentElement);
                            }
                        }
                        //TODO müssen Rand pixel zu den Border Pixeln hinzugefügt werden?
                        if (x > 0)
                        {
                            if (GetPixelRGB((short)(x - 1), y) == _checkColor)
                            {
                                m_checkedPixels[x - 1, y] = true;
                                _island.Pixels.Add(new Pixel((short)(x - 1), y));
                            } else
                            {
                                _island.BorderPixels.Add(currentElement);
                            }
                        }
                        //TODO müssen Rand pixel zu den Border Pixeln hinzugefügt werden?
                        if (y < (m_checkedPixels.GetLength(1) - 1))
                        {
                            if (GetPixelRGB(x, (short)(y + 1)) == _checkColor)
                            {
                                m_checkedPixels[x, y + 1] = true;
                                _island.Pixels.Add(new Pixel(x, (short)(y + 1)));
                            } else
                            {
                                _island.BorderPixels.Add(currentElement);
                            }
                        }
                        //TODO müssen Rand pixel zu den Border Pixeln hinzugefügt werden?
                        if (y > 0)
                        {

                            if (GetPixelRGB(x, (short)(y - 1)) == _checkColor)
                            {
                                m_checkedPixels[x, y - 1] = true;
                                _island.Pixels.Add(new Pixel(x, (short)(y - 1)));
                            } else
                            {
                                _island.BorderPixels.Add(currentElement);
                            }
                        }
                    }
                }
            }
        }

    }
}
