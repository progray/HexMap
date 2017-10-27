namespace Classes.Points
{
    public class PointD
    {
        public double X;
        public double Y;

        public PointD(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static PointD operator +(PointD a, PointD b)
        {
            return new PointD(a.X + b.X, a.Y + b.Y);
        }

        public static PointD operator -(PointD a, PointD b)
        {
            return new PointD(a.X - b.X, a.Y - b.Y);
        }

        public static PointD operator *(double k, PointD a)
        {
            return new PointD(k * a.X, k * a.Y);
        }
    }

    public class PointI
    {
        public int X;
        public int Y;

        public PointI(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static PointI operator +(PointI a, PointI b)
        {
            return new PointI(a.X + b.X, a.Y + b.Y);
        }

        public static PointI operator -(PointI a, PointI b)
        {
            return new PointI(a.X - b.X, a.Y - b.Y);
        }

        public static PointI operator *(int k, PointI a)
        {
            return new PointI(k * a.X, k * a.Y);
        }
    }

    class PointCube
    {
        public int X;
        public int Y;
        public int Z;

        public PointCube(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public static PointCube operator +(PointCube a, PointCube b)
        {
            return new PointCube(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static PointCube operator -(PointCube a, PointCube b)
        {
            return new PointCube(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static PointCube operator *(int k, PointCube a)
        {
            return new PointCube(k * a.X, k * a.Y, k * a.Z);
        }
    }
}
