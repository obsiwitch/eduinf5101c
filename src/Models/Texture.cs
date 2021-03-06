using System.IO;﻿
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageSynthesis.Models {

    class Texture {

        private int Width;
        private int Height;
        private Color[,] C;
        
        /// Amount of tiling in the u and v directions.
        private V2 TileUV;
        
        public Texture(string textureFile) :
            this(textureFile, new V2(1.0f, 1.0f))
        {}
        
        public Texture(string textureFile, V2 tileUV) {
            Bitmap bmp = Open(textureFile);
            
            Width = bmp.Width;
            Height = bmp.Height;
            
            FillColors(bmp);
            
            bmp.Dispose();
            
            TileUV = tileUV;
        }

        public Color Color(V2 uv) {
            return Interpolate(
                uv.U * Width,
                uv.V * Height
            );
        }

        /// Returns the partial derivative of h (the heightmap) with respect to
        /// u and v.
        public V2 Bump(V2 uv) {
            float x = uv.U * Height;
            float y = uv.V * Width;
            
            float vv = Interpolate(x, y).GreyLevel();
            float vx = Interpolate(x + 1, y).GreyLevel();
            float vy = Interpolate(x, y + 1).GreyLevel();
            
            V2 dh = new V2(
                u: vx - vv,
                v: vy - vv
            );
            
            return dh;
        }

        /// Opens an image texture file.
        private Bitmap Open(string textureFile) {
            string path = Path.Combine(
                Path.GetFullPath("resources"),
                "textures",
                textureFile
            );
            
            return new Bitmap(path);
        }

        /// Fills the Colors array using the given Bitmap.
        private void FillColors(Bitmap bmp) {
            BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb
            );
            
            C = new Color[Width, Height];
            
            unsafe {
                byte* ptr = (byte*) data.Scan0;
                for (int x = 0 ; x < Width ; x++) {
                    for (int y = 0 ; y < Height ; y++) {
                        C[x,y] = new Color(
                            b255: ptr[(x * 3) + y * data.Stride],
                            g255: ptr[(x * 3) + y * data.Stride + 1],
                            r255: ptr[(x * 3) + y * data.Stride + 2]
                        );
                    }
                }
            }
            
            bmp.UnlockBits(data);
        }
        
        /// Bilinear Filtering
        private Color Interpolate(float ux, float vy) {
            ux *= TileUV.U;
            vy *= TileUV.V;
            
            int x = (int) ux;
            int y = (int) vy;

            float cx = ux - x;
            float cy = vy - y;

            x %= Width;
            y %= Height;
            if (x < 0) { x += Width; }
            if (y < 0) { y += Height; }

            int xpu = (x + 1) % Width;
            int ypu = (y + 1) % Height;

            float ccx = cx * cx;
            float ccy = cy * cy;

            return C[x, y] * (1 - ccx) * (1 - ccy) +
                   C[xpu, y] * ccx * (1 - ccy) +
                   C[x, ypu] * (1 - ccx) * ccy +
                   C[xpu, ypu] * ccx * ccy;
        }
    }

}
