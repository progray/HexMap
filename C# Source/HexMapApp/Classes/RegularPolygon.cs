using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Classes.Points;

namespace Classes.RegularPolygon
{
    // Правильный многоугольник
    class RegularPolygon
    {
        // Количество вершин
        private byte verticesCount;

        // Длина стороны
        private double sideLength;
        public double SideLength
        {
            get
            {
                return this.sideLength;
            }
            set
            {
                if (this.sideLength == value) return;

                this.sideLength = value;
                this.circumRadius = this.sideLength / (2.0 * Math.Sin(this.ExteriorAngle() / 2.0));
                this.SetPoints();
            }
        }

        // Поворот многоугольника относительно оси (в радианах)
        private double startAngle;
        public double StartAngle
        {
            get
            {
                return this.startAngle;
            }
            set
            {
                if (this.startAngle == value) return;

                this.startAngle = value;
                this.SetPoints();
            }
        }

        // Радиус описанной окружности
        private double circumRadius;

        // Координаты центра
        private PointD point0;
        public PointD Point0
        {
            get
            {
                return this.point0;
            }
            set
            {
                if (this.point0 == value) return;

                this.SetPointsDeltaXY(this.point0.X - value.X, this.point0.Y - value.Y);
                this.point0 = value;
            }
        }

        // Координаты вершин
        private List<PointD> points;

        // Левый верхний угол квадрата, в который вписывается фигура
        public PointD LeftTop;

        // Правый нижний угол квадрата, в который вписывается фигура
        public PointD RightBottom;

        public RegularPolygon(byte VerticesCount, double SideLength, PointD Point0, double StartAngle)
        {
            // Выявление ошибок
            if (VerticesCount < 3)
            {
                MessageBox.Show("У правильного многоугольника должно быть не менее трёх вершин!",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            if (SideLength <= 0.0)
            {
                MessageBox.Show("Длина стороны должна быть больше 0!",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Количество вершин
            this.verticesCount = VerticesCount;

            // Координаты центра
            this.point0 = Point0;

            // Поворот многоугольника
            this.startAngle = StartAngle;

            // Координаты вершин фигуры (инициализируем координатами центра для начала)
            this.points = new List<PointD>();
            for (int i = 0; i < this.verticesCount; i++)
            {
                this.points.Add(new PointD(this.Point0.X, this.Point0.Y));
            }

            // Координаты квадрата, в который вписывается фигура (инициализируем координатами центра для начала)
            this.LeftTop = new PointD(Point0.X, Point0.Y);
            this.RightBottom = new PointD(Point0.X, Point0.Y);

            // Длина стороны (происходит вычисление радиуса описанной окружности и установка вершин фигуры)
            this.SideLength = SideLength;
        }

        // Рисуем правильный многоугольник на канве
        public void Paint(Bitmap canvas, Pen pen, PointD startPoint)
        {
            // Выявление ошибок
            if (points == null || verticesCount != points.Count)
            {
                MessageBox.Show("Не заданы вершины правильного многоугольника!",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Рисование
            Graphics g;
            g = Graphics.FromImage(canvas);

            g.DrawLine(pen,
                (float)(points[0].X + startPoint.X),
                (float)(points[0].Y + startPoint.Y),
                (float)(points[verticesCount - 1].X + startPoint.X),
                (float)(points[verticesCount - 1].Y + startPoint.Y));
            
            for (int i = 1; i < verticesCount; i++)
            {
                g.DrawLine(pen,
                    (float)(points[i - 1].X + startPoint.X),
                    (float)(points[i - 1].Y + startPoint.Y),
                    (float)(points[i].X + startPoint.X),
                    (float)(points[i].Y + startPoint.Y));
            }
            
            g.Dispose();
        }

        // Координаты вершин фигуры
        private void SetPoints()
        {
            // Координаты квадрата, в который вписывается фигура
            this.LeftTop.X = this.point0.X;
            this.LeftTop.Y = this.point0.Y;
            this.RightBottom.X = this.point0.X;
            this.RightBottom.Y = this.point0.Y;

            for (int i = 0; i < this.verticesCount; i++)
            {
                // Координаты вершин фигуры
                this.points[i].X = this.point0.X + this.circumRadius * Math.Cos(this.ExteriorAngle() * (double)i + this.startAngle);
                this.points[i].Y = this.point0.Y + this.circumRadius * Math.Sin(this.ExteriorAngle() * (double)i + this.startAngle);

                // Координаты квадрата, в который вписывается фигура
                if (this.LeftTop.X > this.points[i].X) this.LeftTop.X = this.points[i].X;
                if (this.LeftTop.Y > this.points[i].Y) this.LeftTop.Y = this.points[i].Y;
                if (this.RightBottom.X < this.points[i].X) this.RightBottom.X = this.points[i].X;
                if (this.RightBottom.Y < this.points[i].Y) this.RightBottom.Y = this.points[i].Y;
            }
        }

        // Координаты вершин фигуры (смещение по X и Y)
        private void SetPointsDeltaXY(double DeltaX, double DeltaY)
        {
            for (int i = 0; i < this.verticesCount; i++)
            {
                // Координаты вершин фигуры
                this.points[i].X = this.points[i].X - DeltaX;
                this.points[i].Y = this.points[i].Y - DeltaY;
            }

            // Координаты квадрата, в который вписывается фигура
            this.LeftTop.X = this.LeftTop.X - DeltaX;
            this.LeftTop.Y = this.LeftTop.Y - DeltaY;
            this.RightBottom.X = this.RightBottom.X - DeltaX;
            this.RightBottom.Y = this.RightBottom.Y - DeltaY;
        }

        // Внешний угол
        private double ExteriorAngle()
        {
            return 2.0 * Math.PI / (double)this.verticesCount;
        }

        // Радиус вписанной окружности
        private double Apothem()
        {
            return this.sideLength / (2.0 * Math.Tan(this.ExteriorAngle() / 2.0));
        }

        // Расстояние от центра фигуры до заданной точки на графике
        private double Distance(double X, double Y)
        {
            return Math.Sqrt(Math.Pow(X - this.point0.X, 2) + Math.Pow(Y - this.point0.Y, 2));
        }
        private double Distance(PointD Point)
        {
            return Math.Sqrt(Math.Pow(Point.X - this.point0.X, 2) + Math.Pow(Point.Y - this.point0.Y, 2));
        }
    }

    // Гекс
    class Hex : RegularPolygon
    {
        // Ориентация гекса
        public bool PointyTopped
        {
            get
            {
                return ToppedFromStartAngle(this.StartAngle);
            }
            set
            {
                if (ToppedFromStartAngle(this.StartAngle) == value) return;

                this.StartAngle = StartAngleFromTopped(value);
            }
        }

        public Hex(bool pointyTopped, double hexLength, PointD point0, PointI mapCoord)
            : base(6, hexLength, point0, StartAngleFromTopped(pointyTopped))
        {
        }

        // Угол поворота относительно оси в зависимости от ориентации гекса
        private static double StartAngleFromTopped(bool pointyTopped)
        {
            return (pointyTopped ? Math.PI / 6.0 : 0.0);
        }

        // Ориентация гекса в зависимости от угла поворота относительно оси
        private static bool ToppedFromStartAngle(double startAngle)
        {
            return (startAngle != 0.0);
        }
    }
}
