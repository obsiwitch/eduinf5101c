﻿using System.Drawing;
using System.Drawing.Imaging;

namespace ImageSynthesis {

    enum DisplayMode { SLOW, FAST};

    class Canvas {

        private const int REFRESH = 1000;

        private static int pxCounter = 0;

        private static Bitmap Bmp;
        public static DisplayMode Mode { get; set; }
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        private static BitmapData data;
        private static ZBuffer ZBuffer;

        public static Bitmap Init(int w, int h, DisplayMode mode) {
            Width = w;
            Height = h;
            Mode = mode;
            
            Bmp = new Bitmap(Width, Height);
            ZBuffer = new ZBuffer(Width, Height);
            
            return Bmp;
        }
 
        private static void DrawFastPixel(int x, int y, Color c) {
            unsafe {
                byte* ptr = (byte*) data.Scan0;
                ptr[(x * 3) + y * data.Stride    ] = c.B255();
                ptr[(x * 3) + y * data.Stride + 1] = c.G255();
                ptr[(x * 3) + y * data.Stride + 2] = c.R255();
            }
        }

        private static void DrawSlowPixel(int x, int y, Color c) {
            Bmp.SetPixel(x, y, c.To255());
            
            Program.Form.PictureBoxInvalidate();
            pxCounter++;
            
            // Force redraw every REFRESH px
            if (pxCounter > REFRESH) {
               Program.Form.PictureBoxRefresh();
               pxCounter = 0;
            }
         }

        public static void Refresh(Color c) {
            ZBuffer.clear();
            
            Graphics g = Graphics.FromImage(Bmp);
            g.Clear(c.To255());
            
            if (Mode == DisplayMode.FAST) {
                data = Bmp.LockBits(
                    new Rectangle(0, 0, Bmp.Width, Bmp.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb
                );
            }
        }

        /// Draw a pixel at the position specified by the p vector.
        /// p's coordinates are in the orthonormal basis specified in the
        /// assignement with Y being the viewing direction.
        public static void DrawPixel(V3 p, Color c) {
            DrawPixel((int) p.X, (int) p.Z, p.Y, c);
        }

        /// Draw a pixel on the screen at the following position:
        /// xScreen = x
        /// yScreen = screenHeight - y
        /// A pixel is drawn only if it is inside the screen (canvas), and
        /// if it is not hidden by another already drawn pixel (check the
        /// ZBuffer).
        public static void DrawPixel(int x, int y, float z, Color c) {
            int xScreen = x;
            int yScreen = Height - y;
            
            bool inScreen = (xScreen >= 0) && (xScreen < Width) &&
                            (yScreen >= 0) && (yScreen < Height);
            
            if (inScreen) {
                bool canDraw = ZBuffer.Set(xScreen, yScreen, z);
                
                if (canDraw) {
                    if (Mode == DisplayMode.SLOW) {
                        DrawSlowPixel(xScreen, yScreen, c);
                    }
                    else {
                        DrawFastPixel(xScreen, yScreen, c);
                    }
                }
            }
        }

        public static void Show() {
            if (Mode == DisplayMode.FAST) {
                Bmp.UnlockBits(data);
            }
            
            Program.Form.PictureBoxInvalidate();
        }
    }
}
