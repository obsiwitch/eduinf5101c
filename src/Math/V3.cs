using System;

namespace ImageSynthesis {

    class V3 {

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public V3(V3 t) {
            X = t.X;
            Y = t.Y;
            Z = t.Z;
        }

        public V3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public V3(int x, int y, int z) {
            X = (float) x;
            Y = (float) y;
            Z = (float) z;
        }

        public float Norm1() {
            return Mathf.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float Norm2() {
            return X * X + Y * Y + Z * Z;
        }

        public void Normalize() {
            float n = Norm1();
            
            if (n == 0) { return; }
            
            X /= n;
            Y /= n;
            Z /= n;
        }
        
        /// Gets the reflected vector to the surface defined by normalVec for
        /// the current (incident) vector.
        public V3 ReflectedVector(V3 normalVec) {
            return 2 * (normalVec * this)
                     * normalVec - this;
        }
        
        public V3 RefractedVector(V3 normalVec, float n1, float n2) {
           float n;
           float cosI;
           if (normalVec * this < 0.0f) { // medium 1 -> medium 2
               n = n1 / n2;
               cosI = -normalVec * this;
           }
           else { // medium 2 -> medium 1
               n = n2 / n1;
               cosI = normalVec * this;
           }
           
           float sinT_pow2 = n * n * (1.0f - (cosI * cosI));
           float cosT = Mathf.Sqrt(1.0f - sinT_pow2);
           return n * this + (n * cosI - cosT) * normalVec;
        }

        public static V3 operator +(V3 a, V3 b) {
             return new V3(
                x: a.X + b.X,
                y: a.Y + b.Y,
                z: a.Z + b.Z
            );
        }

        public static V3 operator -(V3 a, V3 b) {
            return new V3(
                x: a.X - b.X,
                y: a.Y - b.Y,
                z: a.Z - b.Z
            );
        }

        public static V3 operator -(V3 a) {
            return new V3(
                x: -a.X,
                y: -a.Y,
                z: -a.Z
            );
        }

        /// Cross product
        public static V3 operator ^ (V3 a, V3 b) {
            return new V3(
                x: a.Y * b.Z - a.Z * b.Y,
                y: a.Z * b.X - a.X * b.Z,
                z: a.X * b.Y - a.Y * b.X
            );
        }

        /// Dot product
        public static float operator * (V3 a, V3 b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static V3 operator *(float a, V3 b) {
            return new V3(
                x: b.X * a,
                y: b.Y * a,
                z: b.Z * a
            );
        }

        public static V3 operator *(V3 b, float a) {
            return a * b;
        }

        public static V3 operator /(V3 b, float a) {
            return new V3(
                x: b.X / a,
                y: b.Y / a,
                z: b.Z / a
            );
        }
        
        override public string ToString() {
            return "(" + X + "," + Y + "," + Z + ")";
        }
    }
}
