using System.Drawing;

namespace ImageSynthesis {

    class Color {

        public static Color Red = new Color(1.0f, 0.0f, 0.0f);
        public static Color Green = new Color(0.0f, 1.0f, 0.0f);
        public static Color Blue = new Color(0.0f, 0.0f, 1.0f);
        public static Color Black = new Color(0.0f, 0.0f, 0.0f);
        public static Color White = new Color(1.0f, 1.0f, 1.0f);

        /// R,G,B in [0,1]
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        
        public Color(float r, float g, float b) {
            R = r;
            G = g;
            B = b;
        }

        public Color(Color c) {
            R = c.R;
            G = c.G;
            B = c.B;
        }
        
        public Color(byte r255, byte g255, byte b255) {
            R = (float) (r255 / 255.0);
            G = (float) (g255 / 255.0);
            B = (float) (b255 / 255.0);
        }
        
        public System.Drawing.Color To255() {
            return System.Drawing.Color.FromArgb(
                R255(), G255(), B255()
            );
        }

        public byte R255() {
            if (R > 1.0) { R = 1.0f; }
            if (R < 0.0) { R = 0.0f; }
            
            return (byte) (R * 255);
        }

        public byte G255() {
            if (G > 1.0) { G = 1.0f; }
            if (G < 0.0) { G = 0.0f; }
            
            return (byte) (G * 255);
        }

        public byte B255() {
            if (B > 1.0) { B = 1.0f; }
            if (B < 0.0) { B = 0.0f; }
            
            return (byte) (B * 255);
        }

        public float GreyLevel() {
            return (R + G + B) / 3.0f;
        }

        // Operators

        public static Color operator +(Color a, Color b) {
            return new Color(a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static Color operator -(Color a, Color b) {
            return new Color(a.R - b.R, a.G - b.G, a.B - b.B);
        }

        public static Color operator -(Color a) {
            return new Color(-a.R, -a.G, -a.B);
        }

        public static Color operator *(Color a, Color b) {
            return new Color(a.R * b.R, a.G * b.G, a.B * b.B);
        }

        public static Color operator *(float a, Color b) {
            return new Color(a * b.R, a * b.G, a * b.B);
        }

        public static Color operator *(Color b, float a) {
            return new Color(a * b.R, a * b.G, a * b.B);
        }

        public static Color operator /(Color b, float a) {
            return new Color(b.R / a, b.G / a, b.B / a);
        }
    }
}
