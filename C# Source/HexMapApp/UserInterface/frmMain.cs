using System;
using System.Drawing;
using System.Windows.Forms;
using Classes;
using Classes.HexMap;
using Classes.Points;

namespace UserInterface
{
    public partial class frmMain : Form
    {
        // Гексокарта
        HexMap HexMap;

        // Гекс с подсветкой
        Hex BacklightHex = null;

        // Выбранный гекс
        Hex SelectedHex = null;

        public frmMain()
        {
            InitializeComponent();

            // Создаём гексокарту
            HexMap = new HexMap(false, 10, 10, 30);

            // Установим начальную точку отображения карты
            HexMap.SetStartPoint(new PointD(50, 50));

            // Регистрируем обработчик события "Отрисовка гексокарты"
            HexMap.OnPaint += new EventHandler(HexMapPaint);

            // Добавляем привязку свойств гексокарты к элементам формы
            chkGridVisible.DataBindings.Add("Checked", HexMap, "GridVisible", false, DataSourceUpdateMode.OnPropertyChanged);
            numHexLength.DataBindings.Add("Value", HexMap, "HexLength", false, DataSourceUpdateMode.OnPropertyChanged);
            btnPointyTopped.DataBindings.Add("Checked", HexMap, "PointyTopped", false, DataSourceUpdateMode.OnPropertyChanged);
            numMapWidth.DataBindings.Add("Value", HexMap, "Width", false, DataSourceUpdateMode.OnPropertyChanged);
            numMapHeight.DataBindings.Add("Value", HexMap, "Height", false, DataSourceUpdateMode.OnPropertyChanged);
            chkHexLabelEnabled.DataBindings.Add("Checked", HexMap.HexLabel, "Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            barHPos.DataBindings.Add("Value", HexMap.HexLabel, "HPos", false, DataSourceUpdateMode.OnPropertyChanged);
            barVPos.DataBindings.Add("Value", HexMap.HexLabel, "VPos", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbHexCoordinateSystem.DataBindings.Add("SelectedIndex", HexMap.HexLabel, "HexCoordinateSystem_int", false, DataSourceUpdateMode.OnPropertyChanged);
            btnOffsetOdd.DataBindings.Add("Checked", HexMap, "OffsetCoordinateType_Odd", false, DataSourceUpdateMode.OnPropertyChanged);

            // Случайно заблокируем несколько гексов
            for (int i = 0; i < HexMap.Width * HexMap.Height * 0.2; i++)
            {
                Hex Hex = HexMap.Hex(RandomE.Next(HexMap.Width - 1), RandomE.Next(HexMap.Height - 1));
                if (Hex != null)
                {
                    Hex.Blocked = true;
                    Hex.SetBrush(new SolidBrush(Color.FromArgb(194, 215, 208)));
                }
            }

            // Включить подпись
            HexMap.HexLabel.Enabled = true;

            // Показать сетку
            HexMap.GridVisible = true;
        }

        // Обработчик события "Отрисовка гексокарты"
        private void HexMapPaint(object sender, EventArgs e)
        {
            Bitmap drawArea = new Bitmap(imgBoard.Width, imgBoard.Height);
            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.FromArgb(244, 244, 241));
            HexMap.Paint(drawArea, pen, brush);
            imgBoard.Image = drawArea;
        }

        // Установка шрифта
        private void btnHexLabelFont_Click(object sender, EventArgs e)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = HexMap.HexLabel.Font;
                if(fd.ShowDialog() == DialogResult.OK)
                {
                    HexMap.HexLabel.SetFont(fd.Font);
                }
            }
        }

        // Изменение размера холста
        private void imgBoard_SizeChanged(object sender, EventArgs e)
        {
            // Отрисовка гексокарты
            HexMapPaint(HexMap, EventArgs.Empty);
        }

        private void imgBoard_MouseMove(object sender, MouseEventArgs e)
        {
            // Гекс для подсветки
            Hex Hex = HexMap.Hex(HexCoordinate.PixelToOffset(new PointD(e.X, e.Y), HexMap));

            // Убираем подсветку с прежнего гекса
            if (BacklightHex != null && BacklightHex != Hex && BacklightHex.Brush != null 
                && ((SolidBrush)BacklightHex.Brush).Color == Color.FromArgb(179, 213, 230))
            {
                BacklightHex.SetBrush(null);
            }

            // Подсветим гекс
            if (Hex != null && Hex != BacklightHex && Hex.Brush == null)
            {
                Hex.SetBrush(new SolidBrush(Color.FromArgb(179, 213, 230)));
            }

            // Гекс с подсветкой
            if (BacklightHex != Hex)
            {
                BacklightHex = Hex;
            }
        }

        private void imgBoard_MouseClick(object sender, MouseEventArgs e)
        {
            // Гекс для выбора
            Hex Hex = HexMap.Hex(HexCoordinate.PixelToOffset(new PointD(e.X, e.Y), HexMap));

            // Выбран новый гекс
            if (Hex != null && Hex != SelectedHex && !Hex.Blocked)
            {
                // Убираем подсветку со всех гексов (кроме недоступных)
                foreach (Hex h in HexMap.Hex())
                    if (h != null && h.Brush != null && !h.Blocked)
                    {
                        h.SetBrush(null);
                    }

                // Подсветим выбранный гекс
                Hex.SetBrush(new SolidBrush(Color.FromArgb(226, 231, 234)));

                // Подсветим видимые гексы
                foreach (Hex h in HexMap.HexVisibles(Hex.MapCoord.Cube()))
                    if (h != null && (h.Brush == null || ((SolidBrush)h.Brush).Color == Color.FromArgb(222, 229, 216)))
                    {
                        h.SetBrush(new SolidBrush(Color.FromArgb(222, 229, 216)));
                    }

                // Запомним выбранный гекс
                SelectedHex = Hex;
            }

            /*
            for (int i = 0; i < HexMap.Width * HexMap.Height; i++)
            {
                Hex Hex = HexMap.Hex(rand.Next(HexMap.Width - 1), rand.Next(HexMap.Height - 1));
                if (Hex != null)
                {
                    Hex.Blocked = true;
                    Hex.SetBrush(new SolidBrush(Color.FromArgb(194, 215, 208)));
                }
            }
            
            foreach (Hex h in HexMap.HexReachable(HexCoordinate.PixelToCube(new PointD(e.X, e.Y), HexMap), 3))
                if (h != null)
                {
                    h.SetBrush(new SolidBrush(Color.FromArgb(226, 231, 234)));
                }
            */

            /*
            PointCube A = null;
            PointCube B = null;

            if (A == null)
            {
                A = HexCoordinate.PixelToCube(new PointD(e.X, e.Y), HexMap);

                foreach (Hex h in HexMap.HexRange(A, 2))
                    if (h != null)
                    {
                        h.SetBrush(new SolidBrush(Color.FromArgb(225, 232, 235)));
                    }
            }
            else if (B == null)
            {
                B = HexCoordinate.PixelToCube(new PointD(e.X, e.Y), HexMap);

                foreach (Hex h in HexMap.HexRange(B, 1))
                    if (h != null)
                    {
                        h.SetBrush(new SolidBrush(Color.FromArgb(225, 232, 235)));
                    }

                foreach (Hex h in HexMap.HexRangeIntersect(A, 2, B, 1))
                    if (h != null)
                    {
                        h.SetBrush(new SolidBrush(Color.FromArgb(194, 215, 208)));
                    }
            }
            */

            /*
            foreach (Hex h in HexMap.HexRange(HexCoordinate.PixelToCube(new PointD(e.X, e.Y), HexMap), 3))
                if (h != null)
                {
                    h.SetBrush(new SolidBrush(Color.FromArgb(226, 231, 234)));
                }
            */

            /*
            PointCube A = null;
            PointCube B = null;

            if (A == null)
            {
                A = HexCoordinate.PixelToCube(new PointD(e.X, e.Y), HexMap);
            }
            else if (B == null)
            {
                B = HexCoordinate.PixelToCube(new PointD(e.X, e.Y), HexMap);

                foreach (Hex h in HexMap.HexLine(A, B))
                    if (h != null)
                    {
                        h.SetBrush(new SolidBrush(Color.FromArgb(225, 232, 235)));
                    }
            }
            */

            /*
            // Координаты гекса
            PointI Offset = HexCoordinate.PixelToOffset(new PointD(e.X, e.Y), HexMap);

            // Гекс для установки цвета
            Hex Hex = HexMap.Hex(Offset);
            Hex.SetBrush(new SolidBrush(Color.FromArgb(194, 215, 208)));

            // Соседние гексы
            foreach (Hex h in HexMap.HexNeighbors(Offset))
                if (h != null)
                {
                    h.SetBrush(new SolidBrush(Color.FromArgb(232, 227, 232)));
                }

            // Диагональные гексы
            foreach (Hex h in HexMap.HexDiagonals(Offset))
                if (h != null)
                {
                    h.SetBrush(new SolidBrush(Color.FromArgb(222, 229, 216)));
                }
            */
        }
    }
}
