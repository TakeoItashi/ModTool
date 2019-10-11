using ModTool.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ModTool {
    public class TestViewModel : ObjectBase {

        private int stride;
        private byte[] pixelByteArray;
        private bool[,] m_checkedPixels;
        private bool _canExecute;
        private RGBColor[,] m_pixelColors;
        private WriteableBitmap m_mapImage;
        private Dictionary<RGBColor, AreaInformation> m_provinces;

        public Application m_App;

        public WriteableBitmap MapImage {
            get => m_mapImage;
            set {
                SetProperty(ref m_mapImage, value);
            }
        }

        public ICommand ClickCommand {
            get {
                return new CommandBase(() => VisibleCheck(), _canExecute);
            }
        }

        public TestViewModel()
        {
            m_mapImage = new WriteableBitmap(new BitmapImage(new Uri(@"C:\Users\root\Desktop\Projekte\ModTool\ModTool\provinces.bmp")));
            //m_mapImage = new WriteableBitmap(new BitmapImage(new Uri(@:\Program Files (x86)\Steam\steamapps\common\Hearts of Iron IV\map\"Cprovinces.bmp")));
            m_provinces = new Dictionary<RGBColor, AreaInformation>();
            //ReadPixelDate();
            _canExecute = true;
        }

        private void VisibleCheck()
        {
            int height = m_mapImage.PixelHeight;
            int width = m_mapImage.PixelWidth;
            //         
            //         (Pixelwidth *                 Bits per Pixel + 7) / sizeof byte
            int nStride = (m_mapImage.PixelWidth * m_mapImage.Format.BitsPerPixel + 7) / 8;
            pixelByteArray = new byte[m_mapImage.PixelHeight * nStride];

            int PixelCount = m_mapImage.PixelHeight * m_mapImage.PixelWidth;
            int totalBytes = (PixelCount * m_mapImage.Format.BitsPerPixel + 7) / 8;

            m_mapImage.CopyPixels(pixelByteArray, nStride, 0);
            stride = (m_mapImage.PixelWidth * m_mapImage.Format.BitsPerPixel + 7) / 8;
            //stride = m_mapImage.PixelWidth * 4; //Width mal 4, weil in dem byte array eine Farbe von jeweils 4 aufeinander folgenden Pixeln dargestellt wird

            m_checkedPixels = new bool[height, width];
            m_pixelColors = new RGBColor[height, width];

            for (short y = 0; y < width - 1; y++)
            {
                for (short x = 0; x < height - 1; x++)
                {
                    if (false)
                    {
                        Console.Beep();
                    }
                    m_pixelColors[x, y] = GetPixelRGB(x, y);
                }
            }

            Task.Run(() =>
           {

               for (short y = 0; y < width - 1; y++)
               {
                   for (short x = 0; x < height - 1; x++)
                   {
                       if (!m_checkedPixels[x, y])
                       {
                           ColorIsland newIsland = new ColorIsland();
                           int k = 0;
                           FloodCheck(x, y, GetPixelRGB(x, y), ref newIsland, ref k);
                           if (!m_provinces.Keys.Contains(GetPixelRGB(x, y)))
                           {
                               m_provinces.Add(GetPixelRGB(x, y), new AreaInformation());
                           }
                           m_provinces[GetPixelRGB(x, y)].AllColorIslands.Add(newIsland);
                       }
                   }
               }
               m_checkedPixels = null;
           }
            );
        }

        public void ReadPixelDate()
        {
            int height = m_mapImage.PixelHeight;
            int width = m_mapImage.PixelWidth;
            //         
            //         (Pixelwidth *                 Bits per Pixel + 7) / sizeof byte
            int nStride = (m_mapImage.PixelWidth * m_mapImage.Format.BitsPerPixel + 7) / 8;
            pixelByteArray = new byte[m_mapImage.PixelHeight * nStride];

            int PixelCount = m_mapImage.PixelHeight * m_mapImage.PixelWidth;
            int totalBytes = (PixelCount * m_mapImage.Format.BitsPerPixel + 7) / 8;

            m_mapImage.CopyPixels(pixelByteArray, nStride, 0);
            stride = (m_mapImage.PixelWidth * m_mapImage.Format.BitsPerPixel + 7) / 8;
            //stride = m_mapImage.PixelWidth * 4; //Width mal 4, weil in dem byte array eine Farbe von jeweils 4 aufeinander folgenden Pixeln dargestellt wird

            m_checkedPixels = new bool[height, width];
            m_pixelColors = new RGBColor[height, width];

            for (short y = 0; y < width - 1; y++)
            {
                for (short x = 0; x < height - 1; x++)
                {
                    if (false)
                    {
                        Console.Beep();
                    }
                    m_pixelColors[x, y] = GetPixelRGB(x, y);
                }
            }

            for (short y = 0; y < width - 1; y++)
            {
                for (short x = 0; x < height - 1; x++)
                {
                    if (!m_checkedPixels[x, y])
                    {
                        ColorIsland newIsland = new ColorIsland();
                        int k = 0;
                        FloodCheck(x, y, m_pixelColors[x, y], ref newIsland, ref k);
                        if (!m_provinces.Keys.Contains(m_pixelColors[x, y]))
                        {
                            m_provinces.Add(m_pixelColors[x, y], new AreaInformation());
                        }
                        m_provinces[m_pixelColors[x, y]].AllColorIslands.Add(newIsland);
                    }
                }
            }
            m_checkedPixels = null;
        }

        //jede iteration überprüft eine island
        public void FloodCheck(short x, short y, RGBColor _checkColor, ref ColorIsland _island, ref int _count)
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
                    RGBColor newColor = new RGBColor
                    {
                        blue = (byte)(((int)m_pixelColors[x, y].blue) + 50 % 255),
                        green = (byte)(((int)m_pixelColors[x, y].green) + 50 % 255),
                        red = (byte)(((int)m_pixelColors[x, y].red) + 50 % 255)
                    };
                    SetPixelRGB(x, y, newColor);

                    UpdateMap();
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
                                newColor = new RGBColor
                                {
                                    blue = (byte)(((int)m_pixelColors[x + 1, y].blue) + 50 % 255),
                                    green = (byte)(((int)m_pixelColors[x + 1, y].green) + 50 % 255),
                                    red = (byte)(((int)m_pixelColors[x + 1, y].red) + 50 % 255)
                                };
                                SetPixelRGB((short)(x + 1), y, newColor);
                                UpdateMap();
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
                                newColor = new RGBColor
                                {
                                    blue = (byte)(((int)m_pixelColors[x - 1, y].blue) + 50 % 255),
                                    green = (byte)(((int)m_pixelColors[x - 1, y].green) + 50 % 255),
                                    red = (byte)(((int)m_pixelColors[x - 1, y].red) + 50 % 255)
                                };
                                SetPixelRGB((short)(x - 1), y, newColor);
                                UpdateMap();
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
                                newColor = new RGBColor
                                {
                                    blue = (byte)(((int)m_pixelColors[x, y+1].blue) + 50 % 255),
                                    green = (byte)(((int)m_pixelColors[x, y+1].green) + 50 % 255),
                                    red = (byte)(((int)m_pixelColors[x, y+1].red) + 50 % 255)
                                };
                                SetPixelRGB(x, (short)(y + 1), newColor);
                                UpdateMap();
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
                                newColor = new RGBColor
                                {
                                    blue = (byte)(((int)m_pixelColors[x, y-1].blue) + 50 % 255),
                                    green = (byte)(((int)m_pixelColors[x, y-1].green) + 50 % 255),
                                    red = (byte)(((int)m_pixelColors[x, y-1].red) + 50 % 255)
                                };
                                SetPixelRGB(x, (short)(y-1), newColor);
                                UpdateMap();
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

        public void UpdateMap()
        {
            m_App.Dispatcher.Invoke(new Action(() =>
            {
                int stride = (m_mapImage.Format.BitsPerPixel * (int)m_mapImage.PixelWidth + 7) / 8;
                Int32Rect rect = new Int32Rect(0, 0, (int)m_mapImage.PixelWidth, (int)m_mapImage.PixelHeight);
                m_mapImage.WritePixels(rect, pixelByteArray, stride, 0);
                OnPropertyChanged(nameof(MapImage));
            }), DispatcherPriority.ContextIdle);
        }

        public RGBColor GetPixelRGB(short x, short y)
        {

            int index = y * 4 + x * stride;
            if (false)
            {
                Console.WriteLine($"{x}, {y}, index: {index}");
            }
            RGBColor newColor = new RGBColor
            {
                blue = pixelByteArray[index],
                green = pixelByteArray[index + 1],
                red = pixelByteArray[index + 2],
            };
            return newColor;
        }

        public void SetPixelRGB(short x, short y, RGBColor _newColor)
        {
            int index = y * 4 + x * stride;
            pixelByteArray[index] = _newColor.blue;
            pixelByteArray[index + 1] = _newColor.green;
            pixelByteArray[index + 2] = _newColor.red;
        }
    }
}
