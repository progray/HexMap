using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Classes.ArrayExtensions;
using Classes.Points;

namespace Classes.HexMap
{
    // Константы
    static class Constants
    {
        // Количество направлений
        public const byte HexDirectionCount = 6;
    }

    // Гексокарта
    class HexMap
    {
        // Ширина карты
        private int width;
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (this.width == value) return;

                // Ширина карты
                this.width = value;

                // Устанавливаем новую размерность массива
                hexes = hexes.ResizeArray(this.width, this.height);

                // Создаём гексы
                this.CreateHexes(false);

                // Генерируем событие "Отрисовка гексокарты"
                this.OnPaint(this, EventArgs.Empty);
            }
        }

        // Высота карты
        private int height;
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                if (this.height == value) return;

                // Высота карты
                this.height = value;

                // Устанавливаем новую размерность массива
                hexes = hexes.ResizeArray(this.width, this.height);

                // Создаём гексы
                this.CreateHexes(false);

                // Генерируем событие "Отрисовка гексокарты"
                this.OnPaint(this, EventArgs.Empty);
            }
        }

        // Длина гекса
        private double hexLength;
        public double HexLength
        {
            get
            {
                return this.hexLength;
            }
            set
            {
                if (this.hexLength == value) return;

                // Длина гекса
                this.hexLength = value;

                // Устанавливаем новую длину для всех гексов
                foreach (Hex hex in this.hexes)
                {
                    hex.SideLength = this.hexLength;
                }

                // Устанавливаем новые координаты центра для всех гексов
                this.SetHexPoint0();

                // Генерируем событие "Отрисовка гексокарты"
                this.OnPaint(this, EventArgs.Empty);
            }
        }

        // Ориентация гексов
        private bool pointyTopped;
        public bool PointyTopped
        {
            get
            {
                return this.pointyTopped;
            }
            set
            {
                if (this.pointyTopped == value) return;

                // Ориентация гексов
                this.pointyTopped = value;

                // Устанавливаем новую ориентацию для всех гексов
                foreach (Hex hex in this.hexes)
                {
                    hex.PointyTopped = this.pointyTopped;
                }

                // Устанавливаем новые координаты центра для всех гексов
                this.SetHexPoint0();

                // Генерируем событие "Отрисовка гексокарты"
                this.OnPaint(this, EventArgs.Empty);
            }
        }

        // Тип офсетной системы координат
        private OffsetCoordinateType offsetCoordinateType;
        public OffsetCoordinateType OffsetCoordinateType
        {
            get
            {
                return this.offsetCoordinateType;
            }
            set
            {
                if (this.offsetCoordinateType == value) return;

                // Тип офсетной системы координат
                this.offsetCoordinateType = value;

                // Устанавливаем новые координаты центра для всех гексов
                this.SetHexPoint0();

                // Генерируем событие "Отрисовка гексокарты"
                this.OnPaint(this, EventArgs.Empty);
            }
        }
        public bool OffsetCoordinateType_Odd
        {
            get
            {
                return (this.offsetCoordinateType == OffsetCoordinateType.Odd);
            }
            set
            {
                this.OffsetCoordinateType = (value ? OffsetCoordinateType.Odd : OffsetCoordinateType.Even);
            }
        }

        // Начальная точка отображения карты
        private PointD startPoint;
        public PointD StartPoint
        {
            get
            {
                return this.startPoint;
            }
        }

        // Информация о гексах
        private Hex[,] hexes;

        // Событие "Отрисовка гексокарты"
        public event EventHandler OnPaint = delegate {};

        // Следует ли показывать сетку из гексов
        private bool gridVisible;
        public bool GridVisible
        {
            get
            {
                return this.gridVisible;
            }
            set
            {
                if (this.gridVisible == value) return;

                // Следует ли показывать сетку из гексов
                this.gridVisible = value;

                // Генерируем событие "Отрисовка гексокарты"
                this.OnPaint(this, EventArgs.Empty);
            }
        }

        // Настройки подписи координатов гекса
        public HexLabel HexLabel;

        public HexMap(bool PointyTopped, int Width, int Height, double HexLength)
        {
            // Выявление ошибок
            if (Width <= 0 || Height <= 0)
            {
                MessageBox.Show("Размерность карты должна быть больше 0!",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Система координат
            this.offsetCoordinateType = OffsetCoordinateType.Odd;

            // Ориентация гексов
            this.pointyTopped = PointyTopped;

            // Ширина и высота карты
            this.width = Width;
            this.height = Height;

            // Длина стороны гекса в графическом представлении
            this.hexLength = HexLength;

            // Информация о гексах
            this.hexes = new Hex[this.width, this.height];
            this.CreateHexes(true);

            // Следует ли показывать сетку из гексов
            this.GridVisible = false;

            // Настройки подписи координатов гекса
            this.HexLabel = new HexLabel();

            // Регистрируем обработчик события "Изменение настроек подписи координатов гекса"
            this.HexLabel.OnChanged += new EventHandler(HexLabelChanged);
        }

        // Информация о гексах (в виде списка)
        public List<Hex> HexList()
        {
            List<Hex> hexList = new List<Hex>();

            foreach(Hex h in hexes)
            {
                hexList.Add(h);
            }

            return hexList;
        }

        // Информация о гексах (в виде массива)
        public Hex[,] Hex()
        {
            return this.hexes;
        }

        // Информация о гексе (Офсетные координаты)
        public Hex Hex(int Q, int R)
        {
            if (Q < 0 || Q > this.hexes.GetUpperBound(0)) return null;
            if (R < 0 || R > this.hexes.GetUpperBound(1)) return null;
            return this.hexes[Q, R];
        }

        // Информация о гексе (Офсетные координаты)
        public Hex Hex(PointI Offset)
        {
            return this.Hex(Offset.X, Offset.Y);
        }

        // Информация о гексе (Кубические координаты)
        public Hex Hex(int X, int Y, int Z)
        {
            return this.Hex(new PointCube(X, Y, Z));
        }

        // Информация о гексе (Кубические координаты)
        public Hex Hex(PointCube Cube)
        {
            return this.Hex(HexCoordinate.CubeToOffset(Cube, this));
        }

        // Информация о массиве гексов (Офсетные координаты)
        public Hex[] Hex(PointI[] Offset)
        {
            Hex[] hex = new Hex[Offset.Length];
            for (int i = 0; i < Offset.Length; i++)
            {
                hex[i] = this.Hex(Offset[i]);
            }
            return hex;
        }

        // Информация о массиве гексов (Кубические координаты)
        public Hex[] Hex(PointCube[] Cube)
        {
            Hex[] hex = new Hex[Cube.Length];
            for (int i = 0; i < Cube.Length; i++)
            {
                hex[i] = this.Hex(Cube[i]);
            }
            return hex;
        }

        // Соседний гекс (Офсетные координаты)
        public Hex HexNeighbor(int Q, int R, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeNeighbor(Q, R, Direction, this));
        }

        // Соседний гекс (Офсетные координаты)
        public Hex HexNeighbor(PointI Offset, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeNeighbor(Offset, Direction, this));
        }

        // Соседний гекс (Кубические координаты)
        public Hex HexNeighbor(int X, int Y, int Z, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeNeighbor(X, Y, Z, Direction));
        }

        // Соседний гекс (Кубические координаты)
        public Hex HexNeighbor(PointCube Cube, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeNeighbor(Cube, Direction));
        }

        // Соседние гексы (Офсетные координаты)
        public Hex[] HexNeighbors(int Q, int R)
        {
            return this.Hex(HexCoordinate.CubeNeighbors(Q, R, this));
        }
        
        // Соседние гексы (Офсетные координаты)
        public Hex[] HexNeighbors(PointI Offset)
        {
            return this.Hex(HexCoordinate.CubeNeighbors(Offset, this));
        }

        // Соседние гексы (Кубические координаты)
        public Hex[] HexNeighbors(int X, int Y, int Z)
        {
            return this.Hex(HexCoordinate.CubeNeighbors(X, Y, Z));
        }

        // Соседние гексы (Кубические координаты)
        public Hex[] HexNeighbors(PointCube Cube)
        {
            return this.Hex(HexCoordinate.CubeNeighbors(Cube));
        }

        // Диагональный гекс (Офсетные координаты)
        public Hex HexDiagonal(int Q, int R, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeDiagonal(Q, R, Direction, this));
        }

        // Диагональный гекс (Офсетные координаты)
        public Hex HexDiagonal(PointI Offset, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeDiagonal(Offset, Direction, this));
        }

        // Диагональный гекс (Кубические координаты)
        public Hex HexDiagonal(int X, int Y, int Z, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeDiagonal(X, Y, Z, Direction));
        }

        // Диагональный гекс (Кубические координаты)
        public Hex HexDiagonal(PointCube Cube, byte Direction)
        {
            return this.Hex(HexCoordinate.CubeDiagonal(Cube, Direction));
        }

        // Диагональные гексы (Офсетные координаты)
        public Hex[] HexDiagonals(int Q, int R)
        {
            return this.Hex(HexCoordinate.CubeDiagonals(Q, R, this));
        }

        // Диагональные гексы (Офсетные координаты)
        public Hex[] HexDiagonals(PointI Offset)
        {
            return this.Hex(HexCoordinate.CubeDiagonals(Offset, this));
        }

        // Диагональные гексы (Кубические координаты)
        public Hex[] HexDiagonals(int X, int Y, int Z)
        {
            return this.Hex(HexCoordinate.CubeDiagonals(X, Y, Z));
        }

        // Диагональные гексы (Кубические координаты)
        public Hex[] HexDiagonals(PointCube Cube)
        {
            return this.Hex(HexCoordinate.CubeDiagonals(Cube));
        }

        // Линия гексов (Офсетные координаты)
        public Hex[] HexLine(PointI A, PointI B)
        {
            return this.Hex(HexCoordinate.CubeLine(A, B, this));
        }

        // Линия гексов (Кубические координаты)
        public Hex[] HexLine(PointCube A, PointCube B)
        {
            return this.Hex(HexCoordinate.CubeLine(A, B));
        }

        // Диапазон гексов (Офсетные координаты)
        public Hex[] HexRange(PointI Offset, int Range)
        {
            return this.Hex(HexCoordinate.CubeRange(Offset, Range, this));
        }

        // Диапазон гексов (Кубические координаты)
        public Hex[] HexRange(PointCube Cube, int Range)
        {
            return this.Hex(HexCoordinate.CubeRange(Cube, Range));
        }

        // Пересечение диапазонов гексов (Офсетные координаты)
        public Hex[] HexRangeIntersect(PointI A, int ARange, PointI B, int BRange)
        {
            return this.Hex(HexCoordinate.CubeRangeIntersect(A, ARange, B, BRange, this));
        }

        // Пересечение диапазонов гексов (Кубические координаты)
        public Hex[] HexRangeIntersect(PointCube A, int ARange, PointCube B, int BRange)
        {
            return this.Hex(HexCoordinate.CubeRangeIntersect(A, ARange, B, BRange));
        }

        // Достижимые гексы (Офсетные координаты)
        public Hex[] HexReachable(PointI Offset, int Range)
        {
            return this.HexReachable(HexCoordinate.OffsetToCube(Offset, this), Range);
        }

        // Достижимые гексы (Кубические координаты)
        public Hex[] HexReachable(PointCube Cube, int Range)
        {
            List<Hex> hexReachable = new List<Hex>();
            List<PointCube> currentLevel = new List<PointCube>();
            List<PointCube> nextLevel = new List<PointCube>();

            hexReachable.Add(this.Hex(Cube));
            currentLevel.Add(Cube);

            for (int i = 0; i < Range; i++)
            {
                foreach(PointCube cube in currentLevel)
                    foreach(Hex h in this.HexNeighbors(cube))
                        if (h != null && !h.Blocked && !hexReachable.Contains(h))
                        {
                            hexReachable.Add(h);
                            nextLevel.Add(h.MapCoord.Cube());
                        }
                currentLevel.Clear();
                currentLevel.AddRange(nextLevel);
                nextLevel.Clear();
            }

            return hexReachable.ToArray();
        }

        // Получить гекс после поворота на 60 градусов (Офсетные координаты)
        public Hex HexRotate60(PointI Offset, PointI Center, byte Rotate60)
        {
            return this.Hex(HexCoordinate.CubeRotate60(Offset, Center, Rotate60, this));
        }

        // Получить гекс после поворота на 60 градусов (Кубические координаты)
        public Hex HexRotate60(PointCube Cube, PointCube Center, byte Rotate60)
        {
            return this.Hex(HexCoordinate.CubeRotate60(Cube, Center, Rotate60));
        }

        // Кольцо гексов (Офсетные координаты)
        public Hex[] HexRing(PointI Offset, int Range)
        {
            return this.Hex(HexCoordinate.CubeRing(Offset, Range, this));
        }

        // Кольцо гексов (Кубические координаты)
        public Hex[] HexRing(PointCube Cube, int Range)
        {
            return this.Hex(HexCoordinate.CubeRing(Cube, Range));
        }

        // Спираль гексов (Офсетные координаты)
        public Hex[] HexSpiral(PointI Offset, int Range)
        {
            return this.Hex(HexCoordinate.CubeSpiral(Offset, Range, this));
        }

        // Спираль гексов (Кубические координаты)
        public Hex[] HexSpiral(PointCube Cube, int Range)
        {
            return this.Hex(HexCoordinate.CubeSpiral(Cube, Range));
        }

        // Видимость гекса (Офсетные координаты)
        public bool HexVisible(PointI Start, PointI Target)
        {
            return this.HexVisible(HexCoordinate.OffsetToCube(Start, this), HexCoordinate.OffsetToCube(Target, this));
        }

        // Видимость гекса (Кубические координаты)
        public bool HexVisible(PointCube Start, PointCube Target)
        {
            int count = HexCoordinate.HexDistance(Start, Target);

            for (byte i = 0; i <= count; i++)
            {
                Hex hex = this.Hex(HexCoordinate.CubeLinearInterpolation(Start, Target, 1.0 / count * i));
                if (hex == null || hex.Blocked)
                {
                    return false;
                }
            }

            return true;
        }

        // Видимые гексы (Офсетные координаты)
        public Hex[] HexVisibles(PointI Start)
        {
            return this.HexVisibles(HexCoordinate.OffsetToCube(Start, this));
        }

        // Видимые гексы (Кубические координаты)
        public Hex[] HexVisibles(PointCube Start)
        {
            List<Hex> hexVisibles = new List<Hex>();

            foreach (Hex h in hexes)
                if (h != null && HexVisible(Start, h.MapCoord.Cube()))
                {
                    hexVisibles.Add(h);
                }

            return hexVisibles.ToArray();
        }

        // Установим начальную точку отображения карты
        public void SetStartPoint(PointD StartPoint)
        {
            // Начальная точка отображения карты
            this.startPoint = StartPoint;

            // Генерируем событие "Отрисовка гексокарты"
            this.OnPaint(this, EventArgs.Empty);
        }

        // Обработчик события "Отрисовка гекса"
        public void HexPaint(object sender, EventArgs e)
        {
            // Генерируем событие "Отрисовка гексокарты"
            this.OnPaint(this, EventArgs.Empty);
        }

        // Рисуем карту на канве
        public void Paint(Bitmap Canvas, Pen Pen, Brush Brush)
        {
            // Проверим необходимость и возможность для рисования
            if (!this.GridVisible || Canvas == null || Pen == null || Brush == null)
            {
                return;
            }

            // Подготовим новый холст
            Graphics g;
            g = Graphics.FromImage(Canvas);

            // Рисуем гексы
            foreach (Hex hex in this.hexes)
                if (hex != null)
                {
                    hex.Paint(g, Pen, Brush, this.startPoint);
                }

            g.Dispose();
        }

        // Вычислим координаты центра для гекса с координатами хранения Q,R
        private PointD CalcHexPoint0(int q, int r)
        {
            return CalcHexPoint0(HexCoordinate.OffsetToCube(new PointI(q, r), this));
        }

        // Вычислим координаты центра для гекса (Кубические координаты)
        private PointD CalcHexPoint0(PointCube Cube)
        {
            double x = HexLength * (this.pointyTopped ? (Math.Sqrt(3.0) * (Cube.X + 0.5 * Cube.Z)) : (1.5 * Cube.X));
            double y = HexLength * (this.pointyTopped ? (1.5 * Cube.Z) : (Math.Sqrt(3.0) * (Cube.Z + 0.5 * Cube.X)));

            return new PointD(x, y);
        }
        
        // Создание гексов
        private void CreateHexes(bool overwriteExist)
        {
            for (int q = 0; q < this.width; q++)
                for (int r = 0; r < this.height; r++)
                    if (overwriteExist || this.hexes[q, r] == null)
                    {
                        // Создаём гекс
                        this.hexes[q, r] = new Hex(
                            this,
                            new PointI(q, r),
                            this.pointyTopped,
                            this.hexLength,
                            this.CalcHexPoint0(q, r));

                        // Регистрируем обработчик события "Отрисовка гекса"
                        this.hexes[q, r].OnPaint += new EventHandler(HexPaint);
                    }
        }

        // Установим координаты центра для всех гексов
        private void SetHexPoint0()
        {
            for (int q = 0; q < this.width; q++)
                for (int r = 0; r < this.height; r++)
                    if (this.hexes[q, r] != null)
                    {
                        this.hexes[q, r].Point0 = this.CalcHexPoint0(q, r);
                    }
        }

        // Обработчик события "Изменение настроек подписи координатов гекса"
        private void HexLabelChanged(object sender, EventArgs e)
        {
            this.OnPaint(sender, e);
        }
    }

    // Гекс
    class Hex
    {
        // Количество вершин
        private const byte verticesCount = 6;

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

                // Длина стороны
                this.sideLength = value;

                // Радиус описанной окружности
                this.circumRadius = this.sideLength / (2.0 * Math.Sin(this.ExteriorAngle() / 2.0));

                // Координаты вершин
                this.SetPoints();
            }
        }

        // Радиус описанной окружности
        private double circumRadius;

        // Поворот гекса относительно оси (в радианах)
        private double startAngle = 0.0;

        // Ориентация гекса
        private bool pointyTopped = false;
        public bool PointyTopped
        {
            get
            {
                return this.pointyTopped;
            }
            set
            {
                if (this.pointyTopped == value) return;

                // Ориентация гекса
                this.pointyTopped = value;

                // Поворот гекса относительно оси (в радианах)
                this.startAngle = this.pointyTopped ? Math.PI / 6.0 : 0.0;

                // Координаты вершин
                this.SetPoints();
            }
        }

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

                // Координаты вершин
                this.SetPointsDeltaXY(this.point0.X - value.X, this.point0.Y - value.Y);
                
                // Координаты центра
                this.point0 = value;
            }
        }

        // Координаты вершин
        private List<PointD> points;

        // Левый верхний угол квадрата, в который вписывается фигура
        public PointD LeftTop;

        // Правый нижний угол квадрата, в который вписывается фигура
        public PointD RightBottom;

        // Гексокарта
        private HexMap hexMap;

        // Координаты гекса на карте
        private HexCoordinate mapCoord;
        public HexCoordinate MapCoord
        {
            get
            {
                return this.mapCoord;
            }
        }

        // Подпись координат гекса
        private bool HexLabelEnabled
        {
            get
            {
                if (this.hexMap == null)
                {
                    return false;
                }
                if (this.hexMap.HexLabel == null)
                {
                    return false;
                }
                if (this.mapCoord == null)
                {
                    return false;
                }
                
                return this.hexMap.HexLabel.Enabled;
            }
        }

        // Используемое перо
        private Pen pen;
        public Pen Pen
        {
            get
            {
                return this.pen;
            }
        }

        // Используемая кисть
        private Brush brush;
        public Brush Brush
        {
            get
            {
                return this.brush;
            }
        }

        // Событие "Отрисовка гекса"
        public event EventHandler OnPaint = delegate { };

        // Гекс недоступен
        private bool blocked;
        public bool Blocked
        {
            get
            {
                return this.blocked;
            }
            set
            {
                if (this.blocked == value) return;

                this.blocked = value;
            }
        }

        public Hex(HexMap HexMap, PointI MapCoord, bool PointyTopped, double SideLength, PointD Point0)
        {
            // Выявление ошибок
            if (SideLength <= 0.0)
            {
                MessageBox.Show("Длина стороны должна быть больше 0!",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Гексокарта
            this.hexMap = HexMap;

            // Координаты гекса на карте
            this.mapCoord = new HexCoordinate(HexMap, MapCoord.X, MapCoord.Y);

            // Координаты центра
            this.point0 = Point0;

            // Ориентация гекса
            this.PointyTopped = PointyTopped;

            // Координаты вершин фигуры (инициализируем координатами центра для начала)
            this.points = new List<PointD>();
            for (byte i = 0; i < verticesCount; i++)
            {
                this.points.Add(new PointD(Point0.X, Point0.Y));
            }

            // Координаты квадрата, в который вписывается фигура (инициализируем координатами центра для начала)
            this.LeftTop = new PointD(Point0.X, Point0.Y);
            this.RightBottom = new PointD(Point0.X, Point0.Y);

            // Длина стороны (происходит вычисление радиуса описанной окружности и установка вершин фигуры)
            this.SideLength = SideLength;

            // Гекс недоступен
            this.blocked = false;
        }
        
        // Используемое перо
        public void SetPen(Pen Pen)
        {
            this.pen = Pen;

            this.OnPaint(this, EventArgs.Empty);
        }
        
        // Используемая кисть
        public void SetBrush(Brush Brush)
        {
            this.brush = Brush;

            this.OnPaint(this, EventArgs.Empty);
        }
        
        // Рисуем гекс на канве
        public void Paint(Graphics Graphics, Pen Pen, Brush Brush, PointD StartPoint)
        {
            // Выявление ошибок
            if (this.points == null || verticesCount != this.points.Count)
            {
                MessageBox.Show("Не заданы вершины гекса!",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // Используемое перо
            if (this.pen != null) Pen = this.pen;

            // Используемая кисть
            if (this.brush != null) Brush = this.brush;

            // Заполнение гекса
            PointF[] CurvePoints = new PointF[this.points.Count];
            byte PointsCount = 0;
            foreach (PointD p in this.points)
            {
                CurvePoints[PointsCount++] = new PointF((float)(p.X + StartPoint.X), (float)(p.Y + StartPoint.Y));
            }
            Graphics.FillPolygon(Brush, CurvePoints);

            // Обводка гекса
            Graphics.DrawLine(Pen,
                (float)(this.points[0].X + StartPoint.X),
                (float)(this.points[0].Y + StartPoint.Y),
                (float)(this.points[verticesCount - 1].X + StartPoint.X),
                (float)(this.points[verticesCount - 1].Y + StartPoint.Y));
            
            for (byte i = 1; i < verticesCount; i++)
            {
                Graphics.DrawLine(Pen,
                    (float)(this.points[i - 1].X + StartPoint.X),
                    (float)(this.points[i - 1].Y + StartPoint.Y),
                    (float)(this.points[i].X + StartPoint.X),
                    (float)(this.points[i].Y + StartPoint.Y));
            }

            // Подписываем координаты гекса
            if (this.HexLabelEnabled)
            {
                // Переводим координаты гекса в строку
                string str = "";
                switch (this.hexMap.HexLabel.HexCoordinateSystem)
                {
                    case HexCoordinateSystem.Offset:
                        {
                            PointI Offset = this.mapCoord.Offset();
                            str = String.Format("{0},{1}",
                                Offset.X,
                                Offset.Y);
                            break;
                        }

                    case HexCoordinateSystem.Cube:
                        {
                            PointCube Cube = this.mapCoord.Cube();
                            str = String.Format("{0},{1},{2}",
                                Cube.X,
                                Cube.Y,
                                Cube.Z);
                            break;
                        }

                    case HexCoordinateSystem.Axial:
                        {
                            PointI Axial = this.mapCoord.Axial();
                            str = String.Format("{0},{1}",
                                Axial.X,
                                Axial.Y);
                            break;
                        }
                }

                // Измеряем строку
                SizeF sizeStr = Graphics.MeasureString(str, this.hexMap.HexLabel.Font);

                // Половинная ширина и высота текста
                sizeStr.Height /= 2;
                sizeStr.Width /= 2;

                // Выравние текста
                PointD pointStr = new PointD(this.Point0.X, this.Point0.Y);
                pointStr.X += this.hexMap.HexLabel.HPos * (pointStr.X - this.LeftTop.X - sizeStr.Width) / 100.0 - sizeStr.Width + StartPoint.X;
                pointStr.Y += this.hexMap.HexLabel.VPos * (pointStr.Y - this.LeftTop.Y - sizeStr.Height) / 100.0 - sizeStr.Height + StartPoint.Y;

                // Рисование
                SolidBrush brush = new SolidBrush(Color.Black);
                Graphics.DrawString(str, this.hexMap.HexLabel.Font, this.hexMap.HexLabel.Brush, (float)pointStr.X, (float)pointStr.Y);
            }
        }

        // Координаты вершин фигуры
        private void SetPoints()
        {
            // Координаты квадрата, в который вписывается фигура
            this.LeftTop.X = this.point0.X;
            this.LeftTop.Y = this.point0.Y;
            this.RightBottom.X = this.point0.X;
            this.RightBottom.Y = this.point0.Y;

            for (byte i = 0; i < verticesCount; i++)
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
            for (byte i = 0; i < verticesCount; i++)
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
            return 2.0 * Math.PI / (double)verticesCount;
        }

        // Радиус вписанной окружности
        private double Apothem()
        {
            return this.sideLength / (2.0 * Math.Tan(this.ExteriorAngle() / 2.0));
        }

        // Расстояние от центра гекса до заданной точки на графике
        private double Distance(double X, double Y)
        {
            return Math.Sqrt(Math.Pow(X - this.point0.X, 2.0) + Math.Pow(Y - this.point0.Y, 2.0));
        }

        // Расстояние от центра гекса до заданной точки на графике
        private double Distance(PointD Point)
        {
            return Math.Sqrt(Math.Pow(Point.X - this.point0.X, 2.0) + Math.Pow(Point.Y - this.point0.Y, 2.0));
        }
    }

    // Координаты гекса
    class HexCoordinate
    {
        // Координаты хранения гекса в массиве (совпадают с Офсетными координатами)
        private int q;
        private int r;

        // Ориентация гекса
        private bool PointyTopped
        {
            get
            {
                // Выявление ошибок
                if (this.hexMap == null)
                {
                    MessageBox.Show("Невозможно определить ориентацию гекса!",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }

                return this.hexMap.PointyTopped;
            }
        }

        // Тип офсетной системы координат
        private OffsetCoordinateType OffsetCoordinateType
        {
            get
            {
                // Выявление ошибок
                if (this.hexMap == null)
                {
                    MessageBox.Show("Невозможно определить тип офсетной системы координат!",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return OffsetCoordinateType.Odd;
                }

                return this.hexMap.OffsetCoordinateType;
            }
        }

        // Гексокарта
        private HexMap hexMap;

        public HexCoordinate(HexMap HexMap, int Q, int R)
        {
            this.hexMap = HexMap;
            this.q = Q;
            this.r = R;
        }

        // Офсетные координаты
        public PointI Offset()
        {
            return new PointI(q, r);
        }

        // Кубические координаты
        public PointCube Cube()
        {
            int x = q - (this.PointyTopped ? ((r + (int)this.OffsetCoordinateType * (r & 1)) / 2) : (0));
            int z = r - (this.PointyTopped ? (0) : ((q + (int)this.OffsetCoordinateType * (q & 1)) / 2));
            int y = (-x - z);

            return new PointCube(x, y, z);
        }

        // Осевые координаты
        public PointI Axial()
        {
            PointCube Cube = this.Cube();

            return new PointI(Cube.X, Cube.Z);
        }

        // Кубические координаты -> Осевые координаты
        public static PointI CubeToAxial(PointCube Cube)
        {
            int q = Cube.X;
            int r = Cube.Z;

            return new PointI(q, r);
        }

        // Осевые координаты -> Кубические координаты
        public static PointCube AxialToCube(PointI Axial)
        {
            int x = Axial.X;
            int z = Axial.Y;
            int y = (-x - z);

            return new PointCube(x, y, z);
        }

        // Кубические координаты -> Офсетные координаты
        public static PointI CubeToOffset(PointCube Cube, HexMap HexMap)
        {
            int q = Cube.X + (HexMap.PointyTopped ? ((Cube.Z + (int)HexMap.OffsetCoordinateType * (Cube.Z & 1)) / 2) : (0));
            int r = Cube.Z + (HexMap.PointyTopped ? (0) : ((Cube.X + (int)HexMap.OffsetCoordinateType * (Cube.X & 1)) / 2));

            return new PointI(q, r);
        }

        // Офсетные координаты -> Кубические координаты
        public static PointCube OffsetToCube(PointI Offset, HexMap HexMap)
        {
            int x = Offset.X - (HexMap.PointyTopped ? ((Offset.Y + (int)HexMap.OffsetCoordinateType * (Offset.Y & 1)) / 2) : (0));
            int z = Offset.Y - (HexMap.PointyTopped ? (0) : ((Offset.X + (int)HexMap.OffsetCoordinateType * (Offset.X & 1)) / 2));
            int y = (-x - z);

            return new PointCube(x, y, z);
        }

        // Кубические направления
        public static PointCube[] CubeDirections()
        {
            return new PointCube[Constants.HexDirectionCount] {
                 new PointCube(+1,-1,0)
                ,new PointCube(+1,0,-1)
                ,new PointCube(0,+1,-1)
                ,new PointCube(-1,+1,0)
                ,new PointCube(-1,0,+1)
                ,new PointCube(0,-1,+1)
            };
        }

        // Кубические диагонали
        public static PointCube[] CubeDiagonals()
        {
            return new PointCube[Constants.HexDirectionCount] {
                 new PointCube(+2,-1,-1)
                ,new PointCube(+1,+1,-2)
                ,new PointCube(-1,+2,-1)
                ,new PointCube(-2,+1,+1)
                ,new PointCube(-1,-1,+2)
                ,new PointCube(+1,-2,+1)
            };
        }

        // Вращение на 60 градусов
        public static PointCube CubeRotate60(PointCube Cube, byte Rotate60)
        {
            switch (Rotate60)
            {
                case 0: return Cube;
                case 1: return new PointCube(-Cube.Z, -Cube.X, -Cube.Y);
                case 2: return new PointCube(Cube.Y, Cube.Z, Cube.X);
                case 3: return new PointCube(-Cube.X, -Cube.Y, -Cube.Z);
                case 4: return new PointCube(Cube.Z, Cube.X, Cube.Y);
                case 5: return new PointCube(-Cube.Y, -Cube.Z, -Cube.X);
            }
            return Cube;
        }

        // Соседний гекс (Офсетные координаты)
        public static PointCube CubeNeighbor(int Q, int R, byte Direction, HexMap HexMap)
        {
            return CubeNeighbor(new PointI(Q, R), Direction, HexMap);
        }

        // Соседний гекс (Офсетные координаты)
        public static PointCube CubeNeighbor(PointI Offset, byte Direction, HexMap HexMap)
        {
            return CubeNeighbor(OffsetToCube(Offset, HexMap), Direction);
        }

        // Соседний гекс (Кубические координаты)
        public static PointCube CubeNeighbor(int X, int Y, int Z, byte Direction)
        {
            return CubeNeighbor(new PointCube(X, Y, Z), Direction);
        }

        // Соседний гекс (Кубические координаты)
        public static PointCube CubeNeighbor(PointCube Cube, byte Direction)
        {
            if (Direction < 0 || Direction >= Constants.HexDirectionCount) return null;
            return Cube + CubeDirections()[Direction];
        }

        // Соседние гексы (Офсетные координаты)
        public static PointCube[] CubeNeighbors(int Q, int R, HexMap HexMap)
        {
            return CubeNeighbors(new PointI(Q, R), HexMap);
        }

        // Соседние гексы (Офсетные координаты)
        public static PointCube[] CubeNeighbors(PointI Offset, HexMap HexMap)
        {
            return CubeNeighbors(OffsetToCube(Offset, HexMap));
        }

        // Соседние гексы (Кубические координаты)
        public static PointCube[] CubeNeighbors(int X, int Y, int Z)
        {
            return CubeNeighbors(new PointCube(X, Y, Z));
        }

        // Соседние гексы (Кубические координаты)
        public static PointCube[] CubeNeighbors(PointCube Cube)
        {
            PointCube[] cubeNeighbors = new PointCube[Constants.HexDirectionCount];
            for (byte i = 0; i < Constants.HexDirectionCount; i++)
            {
                cubeNeighbors[i] = Cube + CubeDirections()[i];
            }
            return cubeNeighbors;
        }

        // Диагональный гекс (Офсетные координаты)
        public static PointCube CubeDiagonal(int Q, int R, byte Direction, HexMap HexMap)
        {
            return CubeDiagonal(new PointI(Q, R), Direction, HexMap);
        }

        // Диагональный гекс (Офсетные координаты)
        public static PointCube CubeDiagonal(PointI Offset, byte Direction, HexMap HexMap)
        {
            return CubeDiagonal(OffsetToCube(Offset, HexMap), Direction);
        }

        // Диагональный гекс (Кубические координаты)
        public static PointCube CubeDiagonal(int X, int Y, int Z, byte Direction)
        {
            return CubeDiagonal(new PointCube(X, Y, Z), Direction);
        }

        // Диагональный гекс (Кубические координаты)
        public static PointCube CubeDiagonal(PointCube Cube, byte Direction)
        {
            if (Direction < 0 || Direction >= Constants.HexDirectionCount) return null;
            return Cube + CubeDiagonals()[Direction];
        }

        // Диагональные гексы (Офсетные координаты)
        public static PointCube[] CubeDiagonals(int Q, int R, HexMap HexMap)
        {
            return CubeDiagonals(new PointI(Q, R), HexMap);
        }

        // Диагональные гексы (Офсетные координаты)
        public static PointCube[] CubeDiagonals(PointI Offset, HexMap HexMap)
        {
            return CubeDiagonals(OffsetToCube(Offset, HexMap));
        }

        // Диагональные гексы (Кубические координаты)
        public static PointCube[] CubeDiagonals(int X, int Y, int Z)
        {
            return CubeDiagonals(new PointCube(X, Y, Z));
        }

        // Диагональные гексы (Кубические координаты)
        public static PointCube[] CubeDiagonals(PointCube Cube)
        {
            PointCube[] cubeDiagonals = new PointCube[Constants.HexDirectionCount];
            for (byte i = 0; i < Constants.HexDirectionCount; i++)
            {
                cubeDiagonals[i] = Cube + CubeDiagonals()[i];
            }
            return cubeDiagonals;
        }

        // Линия гексов (Офсетные координаты)
        public static PointCube[] CubeLine(PointI A, PointI B, HexMap HexMap)
        {
            return CubeLine(OffsetToCube(A, HexMap), OffsetToCube(B, HexMap));
        }

        // Линия гексов (Кубические координаты)
        public static PointCube[] CubeLine(PointCube A, PointCube B)
        {
            int count = HexDistance(A, B);

            PointCube[] cubeLine = new PointCube[count + 1];

            for (byte i = 0; i <= count; i++)
            {
                cubeLine[i] = CubeLinearInterpolation(A, B, 1.0 / count * i);
            }

            return cubeLine;
        }

        // Диапазон гексов (Офсетные координаты)
        public static PointCube[] CubeRange(PointI Offset, int Range, HexMap HexMap)
        {
            return CubeRange(OffsetToCube(Offset, HexMap), Range);
        }

        // Диапазон гексов (Кубические координаты)
        public static PointCube[] CubeRange(PointCube Cube, int Range)
        {
            List<PointCube> cubeRange = new List<PointCube>();

            for (int dx = -Range; dx <= Range; dx++)
                for (int dy = Math.Max(-Range, -dx - Range); dy <= Math.Min(Range, -dx + Range); dy++)
                {
                    int dz = (-dx - dy);
                    cubeRange.Add(Cube + new PointCube(dx, dy, dz));
                }

            return cubeRange.ToArray();
        }

        // Пересечение диапазонов гексов (Офсетные координаты)
        public static PointCube[] CubeRangeIntersect(PointI A, int ARange, PointI B, int BRange, HexMap HexMap)
        {
            return CubeRangeIntersect(OffsetToCube(A, HexMap), ARange, OffsetToCube(B, HexMap), BRange);
        }

        // Пересечение диапазонов гексов (Кубические координаты)
        public static PointCube[] CubeRangeIntersect(PointCube A, int ARange, PointCube B, int BRange)
        {
            List<PointCube> cubeRange = new List<PointCube>();

            int x_min = Math.Max(A.X - ARange, B.X - BRange);
            int x_max = Math.Min(A.X + ARange, B.X + BRange);

            int y_min = Math.Max(A.Y - ARange, B.Y - BRange);
            int y_max = Math.Min(A.Y + ARange, B.Y + BRange);

            int z_min = Math.Max(A.Z - ARange, B.Z - BRange);
            int z_max = Math.Min(A.Z + ARange, B.Z + BRange);

            for (int x = x_min; x <= x_max; x++)
                for (int y = Math.Max(y_min, -x - z_max); y <= Math.Min(y_max, -x - z_min); y++)
                {
                    int z = (-x - y);
                    cubeRange.Add(new PointCube(x, y, z));
                }

            return cubeRange.ToArray();
        }

        // Получить гекс после поворота на 60 градусов (Офсетные координаты)
        public static PointCube CubeRotate60(PointI Offset, PointI Center, byte Rotate60, HexMap HexMap)
        {
            return CubeRotate60(OffsetToCube(Offset, HexMap), OffsetToCube(Center, HexMap), Rotate60);
        }

        // Получить гекс после поворота на 60 градусов (Кубические координаты)
        public static PointCube CubeRotate60(PointCube Cube, PointCube Center, byte Rotate60)
        {
            // Какой поворот необходимо выполнить
            Rotate60 %= Constants.HexDirectionCount;
            if (Rotate60 < 0) Rotate60 += Constants.HexDirectionCount;

            // Поворот
            Cube = Center + HexCoordinate.CubeRotate60(Cube - Center, Rotate60);

            // Получим гекс
            return Cube;
        }

        // Кольцо гексов (Офсетные координаты)
        public static PointCube[] CubeRing(PointI Offset, int Range, HexMap HexMap)
        {
            return CubeRing(OffsetToCube(Offset, HexMap), Range);
        }

        // Кольцо гексов (Кубические координаты)
        public static PointCube[] CubeRing(PointCube Cube, int Range)
        {
            List<PointCube> cubeRing = new List<PointCube>();

            Cube = Cube + Range * HexCoordinate.CubeDirections()[4];

            if (Range == 0) cubeRing.Add(Cube);

            for (byte i = 0; i < Constants.HexDirectionCount; i++)
                for (int j = 0; j < Range; j++)
                {
                    cubeRing.Add(Cube);
                    Cube = CubeNeighbor(Cube, i);
                }

            return cubeRing.ToArray();
        }

        // Спираль гексов (Офсетные координаты)
        public static PointCube[] CubeSpiral(PointI Offset, int Range, HexMap HexMap)
        {
            return CubeSpiral(OffsetToCube(Offset, HexMap), Range);
        }

        // Спираль гексов (Кубические координаты)
        public static PointCube[] CubeSpiral(PointCube Cube, int Range)
        {
            List<PointCube> cubeSpiral = new List<PointCube>();
            for (int i = 0; i <= Range; i++)
            {
                cubeSpiral.AddRange(CubeRing(Cube, i));
            }
            return cubeSpiral.ToArray();
        }

        // Расстояние в гексах (Кубические координаты)
        public static int HexDistance(PointCube A, PointCube B)
        {
            return (Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y) + Math.Abs(A.Z - B.Z)) / 2;
        }

        // Расстояние в гексах (Офсетные координаты)
        public static int HexDistance(PointI A, PointI B, HexMap HexMap)
        {
            return HexDistance(
                HexCoordinate.OffsetToCube(A, HexMap),
                HexCoordinate.OffsetToCube(B, HexMap));
        }

        // Координаты центра гекса (Кубические координаты)
        public static PointD CalcHexPoint0(PointCube Cube, HexMap HexMap)
        {
            double x = HexMap.HexLength * (HexMap.PointyTopped ? (Math.Sqrt(3.0) * (Cube.X + 0.5 * Cube.Z)) : (1.5 * Cube.X));
            double y = HexMap.HexLength * (HexMap.PointyTopped ? (1.5 * Cube.Z) : (Math.Sqrt(3.0) * (Cube.Z + 0.5 * Cube.X)));

            return new PointD(x, y);
        }

        // Округление кубических координат
        public static PointCube CubeRound(double X, double Y, double Z)
        {
            int rx = (int)Math.Round(X, MidpointRounding.AwayFromZero);
            int ry = (int)Math.Round(Y, MidpointRounding.AwayFromZero);
            int rz = (int)Math.Round(Z, MidpointRounding.AwayFromZero);

            double dx = Math.Abs(rx - X);
            double dy = Math.Abs(ry - Y);
            double dz = Math.Abs(rz - Z);
            double max = Math.Max(dx, Math.Max(dy, dz));

            if (dx == max) dx += RandomE.NextDouble();
            if (dy == max) dy += RandomE.NextDouble();
            if (dz == max) dz += RandomE.NextDouble();

            if (dx > dy && dx > dz)
            {
                rx = (-ry - rz);
            }
            else if (dy > dz)
            {
                ry = (-rx - rz);
            }
            else
            {
                rz = (-rx - ry);
            }

            return new PointCube(rx, ry, rz);
        }

        // Координаты пикселя -> Кубические координаты
        public static PointCube PixelToCube(PointD Pixel, HexMap HexMap)
        {
            Pixel -= HexMap.StartPoint;
            double q = (HexMap.PointyTopped ? ((Pixel.X * Math.Sqrt(3.0) - Pixel.Y) / 3.0) : ((Pixel.X * 2.0) / 3.0)) / HexMap.HexLength;
            double r = (HexMap.PointyTopped ? ((Pixel.Y * 2.0) / 3.0) : ((Pixel.Y * Math.Sqrt(3.0) - Pixel.X) / 3.0)) / HexMap.HexLength;

            return HexCoordinate.CubeRound(q, (-q - r), r);
        }

        // Координаты пикселя -> Осевые координаты
        public static PointI PixelToAxial(PointD Pixel, HexMap HexMap)
        {
            return HexCoordinate.CubeToAxial(HexCoordinate.PixelToCube(Pixel, HexMap));
        }

        // Координаты пикселя -> Офсетные координаты
        public static PointI PixelToOffset(PointD Pixel, HexMap HexMap)
        {
            return HexCoordinate.CubeToOffset(HexCoordinate.PixelToCube(Pixel, HexMap), HexMap);
        }

        // Линейная интерполяция
        private static double LinearInterpolation(double A, double B, double T)
        {
            return A + (B - A) * T;
        }

        // Линейная интерполяция (Кубические координаты)
        public static PointCube CubeLinearInterpolation(PointCube A, PointCube B, double T)
        {
            return CubeRound(
                LinearInterpolation(A.X, B.X, T),
                LinearInterpolation(A.Y, B.Y, T),
                LinearInterpolation(A.Z, B.Z, T));
        }
    }

    // Система координат
    enum HexCoordinateSystem
    {
        Offset = 0,
        Cube = 1,
        Axial = 2
    }

    // Тип офсетной системы координат
    enum OffsetCoordinateType
    {
        Odd = -1,
        Even = 1
    }

    // Настройки подписи координатов гекса
    class HexLabel
    {
        // Подписывать координаты гекса
        private bool enabled;
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (this.enabled == value) return;

                // Подписывать координаты гекса
                this.enabled = value;

                // Генерируем событие "Изменение настроек подписи координатов гекса"
                this.OnChanged(this, EventArgs.Empty);
            }
        }

        // Используемый шрифт
        private Font font;
        public Font Font
        {
            get
            {
                return this.font;
            }
        }

        // Используемая кисть
        private Brush brush;
        public Brush Brush
        {
            get
            {
                return this.brush;
            }
        }

        // Система координат
        private HexCoordinateSystem hexCoordinateSystem;
        public HexCoordinateSystem HexCoordinateSystem
        {
            get
            {
                return this.hexCoordinateSystem;
            }
            set
            {
                if (this.hexCoordinateSystem == value) return;

                // Система координат
                this.hexCoordinateSystem = value;

                // Генерируем событие "Изменение настроек подписи координатов гекса"
                this.OnChanged(this, EventArgs.Empty);
            }
        }
        public int HexCoordinateSystem_int
        {
            get
            {
                return (int)this.hexCoordinateSystem;
            }
            set
            {
                if (this.hexCoordinateSystem == (HexCoordinateSystem)value) return;

                // Система координат
                this.hexCoordinateSystem = (HexCoordinateSystem)value;

                // Генерируем событие "Изменение настроек подписи координатов гекса"
                this.OnChanged(this, EventArgs.Empty);
            }
        }

        // Позиция по горизонтали (в % относительно центра гекса)
        // -100% - левый край
        //    0% - центр
        // +100% - правый край
        private double hpos;
        public double HPos
        {
            get
            {
                return this.hpos;
            }
            set
            {
                if (this.hpos == value) return;

                // Позиция по горизонтали
                this.hpos = value;

                // Генерируем событие "Изменение настроек подписи координатов гекса"
                this.OnChanged(this, EventArgs.Empty);
            }
        }

        // Позиция по вертикали (в % относительно центра гекса)
        // -100% - верхний край
        //    0% - центр
        // +100% - нижний край
        private double vpos;
        public double VPos
        {
            get
            {
                return this.vpos;
            }
            set
            {
                if (this.vpos == value) return;

                // Позиция по вертикали
                this.vpos = value;

                // Генерируем событие "Изменение настроек подписи координатов гекса"
                this.OnChanged(this, EventArgs.Empty);
            }
        }

        // Событие "Изменение настроек подписи координатов гекса"
        public event EventHandler OnChanged = delegate {};

        public HexLabel()
        {
            // Подписывать координаты гекса
            this.Enabled = false;

            // Используемый шрифт
            this.font = new Font(SystemFonts.DefaultFont, FontStyle.Regular);

            // Используемая кисть
            this.brush = new SolidBrush(Color.Black);

            // Система координат
            this.hexCoordinateSystem = HexCoordinateSystem.Offset;

            // Позиция по горизонтали
            this.hpos = 0.0;

            // Позиция по вертикали
            this.vpos = 0.0;
        }

        // Установка шрифта
        public void SetFont(Font Font)
        {
            // Используемый шрифт
            this.font = Font;
            
            // Генерируем событие "Изменение настроек подписи координатов гекса"
            this.OnChanged(this, EventArgs.Empty);
        }
    }
}