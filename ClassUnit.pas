unit ClassUnit;

interface

uses Types, Graphics, Math, SysUtils;

type
  // Ссылка на гекс
  PHex = ^THex;

  // Ссылка на правила подписания гекса
  PHexLabel = ^THexLabel;

  { Правильная фигура }
  TRightFigure = class
    // Количество вершин фигуры
    Count: integer;
    // Сторона фигуры
    Len: integer;
    // Радиус описанной окружности
    Radius: real;
    // Радиус вписанной окружности
    RadiusVpis: real;
    // Координаты центра фигуры
    GraphCoord0: TPoint;
    // Координаты вершин фигуры
    GraphCoord: array of TPoint;
    // Левый верхний угол квадрата, в который вписывается фигура
    LeftTop: TPoint;
    // Правый нижний угол квадрата, в который вписывается фигура
    RightBottom: TPoint;
    // Конструктор
    constructor Create(n,l: integer; c: TPoint);
    // Рисуем фигуру на канве
    procedure Paint(HDC: TCanvas);
    // Расстояние от центра фигуры до заданной точки на графике
    function GraphLen(X,Y: integer): real;
  end;

  { Метка с координатами гекса }
  THexLabel = record
    // Подписывать координаты гексов
    Enabled: boolean;
    // Позиция по горизонтали и вертикали (в % относительно центра гекса):
    // [-100% - левый (верхний) край, 0% - центр, 100% - правый (нижний) край]
    HPos,VPos: integer;
    // Используемый шрифт
    Font: TFont;
  end;

  // Направление движения (массив)
  TDirArray = record
    Ok: array of integer;
    All: array of integer;
  end;

  // Путь из начальной точки в конечную
  THexPath = record
    // ??? Деревянная структура пути
    // Ссылка на начальный гекс
    StartHex: PHex;
    // Ссылка на конечный гекс
    EndHex: PHex;
    // Массив направлений, следуя которым можно добраться из начального в конечный гекс
    // [1 цифра - вариант пути, 2 цифра - направление]
    Direction: array of array of integer;
  end;

  { Гекс }
  THex = class(TRightFigure)
    // Координаты гекса на карте
    Coord: TPoint;
    // Ссылки на соседние гексы (0-5 = соседние гексы, 6 = свой гекс)
    HexNear: array [0..6] of PHex;
    // Ссылка на правила подписи координат гекса
    HLabel: PHexLabel;
    // Конструктор
    constructor Create(l: integer; c,map: TPoint);
    // Деструктор
    destructor Destroy; override;
    // Получить номер соседнего гекса, который существует
    // [Direction - Выбранный соседний гекс от 0..5 ]
    // [Turn - Поворот, если гекс не существует]
    // [-1 = против часовой стрелки, +1 = по часовой стрелке]
    function GetHexNear(Direction,Turn: integer): integer;
    // Получить координаты соседнего гекса, реально существующего или мнимого
    // [Direction - Выбранный соседний гекс от 0..5]
    function HexNearCoord(Direction: integer): TPoint;
    // Рисуем гекс на канве
    procedure Paint(HDC: TCanvas);
  end;

  { Гексокарта }
  THexMap = class
    // Ширина и высота карты
    Width: integer;
    Height: integer;
    // Длина стороны гекса в графическом представлении
    HexLen: integer;
    // Информация о гексах
    Hex: array of array of THex;
    // Как следует подписывать координаты гексов
    HexLabel: THexLabel;
    // Следует ли показывать сетку из гексов
    HexVisible: boolean;
    // Конструктор
    constructor Create(l,w,h: integer);
    // Рисуем карту на канве
    procedure Paint(HDC: TCanvas);
    // Координаты гекса, над которым находится курсор
    // (перевод графических координат в логические координаты)
    function MouseToCoord(MouseX, MouseY: integer): TPoint;
    // Перевод логических координат в графические
    function CoordToGraph(Coord: TPoint): TPoint;
    // Расстояние в гексах между двумя гексами
    function HexBetween(C1,C2: TPoint): integer;
    // Добавляем направление в массив
    procedure AddDirection(var A: TDirArray; Direction: integer);
    // Удаляем направление из массива
    procedure DeleteDirection(var A: TDirArray; Num: integer);
    // Направление между двумя гексами
    function DirectionBetween(C1,C2: TPoint): TDirArray;
    // Направление между двумя гексами (с ограничением по массиву гексов)
    function DirectionBetween_InArray(C1,C2: TPoint; A{Array}: array of PHex): TDirArray;
    // Находится ли гекс среди элементов массива
    function HexInArray(A{Array}: array of PHex; H{Hex}: PHex): boolean;
    // Кратчайший путь между двумя гексами (логическое представление)
    function HexPathBetween(C1,C2: TPoint): THexPath;
    // Кратчайший путь между двумя гексами (графическое представление)
    function GraphPathBetween(C1,C2: TPoint): THexPath;
  end;

implementation

// Конструктор правильной фигуры
constructor TRightFigure.Create(n,l: integer; c: TPoint);
var
  i: integer;
  alpha: real;
begin
  // Количество вершин фигуры
  Count := n;
  // Сторона фигуры
  Len := l;
  // Координаты центра фигуры
  GraphCoord0.X := c.X;
  GraphCoord0.Y := c.Y;
  // Угол поворота
  alpha := (2 * pi) / Count;
  // Радиус описанной окружности
  Radius := Len / (2 * sin(alpha/2));
  // Радиус вписанной окружности
  RadiusVpis := Len / (2 * tan(alpha/2));
  // Координаты вершин фигуры
  SetLength(GraphCoord,Count);
  for i := 0 to n-1 do
    begin
      GraphCoord[i].X := round(GraphCoord0.X + Radius * cos(alpha*i));
      GraphCoord[i].Y := round(GraphCoord0.Y + Radius * sin(alpha*i));
    end;

  // Поиск минимальной и максимальной координаты X и Y
  LeftTop.X := GraphCoord[0].X;
  LeftTop.Y := GraphCoord[0].Y;
  RightBottom.X := GraphCoord[0].X;
  RightBottom.Y := GraphCoord[0].Y;
  for i:=1 to Count-1 do
  begin
    if GraphCoord[i].X < LeftTop.X then
      LeftTop.X := GraphCoord[i].X;
    if GraphCoord[i].Y < LeftTop.Y then
      LeftTop.Y := GraphCoord[i].Y;
    if GraphCoord[i].X > RightBottom.X then
      RightBottom.X := GraphCoord[i].X;
    if GraphCoord[i].Y > RightBottom.Y then
      RightBottom.Y := GraphCoord[i].Y;
  end;
end;

// Рисуем правильную фигуру на канве
procedure TRightFigure.Paint(HDC: TCanvas);
var
  i: integer;
begin
  with HDC do
  begin
    for i := 0 to Count do
      begin
        if i = 0 then MoveTo(GraphCoord[0].X,GraphCoord[0].Y) else
        if i = Count then LineTo(GraphCoord[0].X,GraphCoord[0].Y) else
        LineTo(GraphCoord[i].X,GraphCoord[i].Y);
      end;
  end;
end;

// Расстояние от центра фигуры до заданной точки на графике
function TRightFigure.GraphLen(X,Y: integer): real;
begin
  Result := sqrt(sqr(X - GraphCoord0.X) + sqr(Y - GraphCoord0.Y));
end;

// Конструктор гекса
constructor THex.Create(l: integer; c,map: TPoint);
begin
  inherited Create(6,l,c);
  // Координаты гекса на карте
  Coord := map;
end;

// Деструктор гекса
destructor THex.Destroy;
var
  i,j: integer;
begin
  // Удаляем гекс из списка соседних гексов
  for i:=0 to 5 do
    if HexNear[i] <> nil then
      for j:=0 to 6 do
        if (HexNear[i]^.HexNear[j] <> nil) and
        (Coord.X = HexNear[i]^.HexNear[j]^.Coord.X) and
        (Coord.Y = HexNear[i]^.HexNear[j]^.Coord.Y) then
          HexNear[i]^.HexNear[j] := nil;
  // Родительский метод
  inherited Destroy;
end;

// Получить номер соседнего гекса, который существует
// [Direction - Выбранный соседний гекс от 0..5 ]
// [Turn - Поворот, если гекс не существует]
// [-1 = против часовой стрелки, +1 = по часовой стрелке]
function THex.GetHexNear(Direction,Turn: integer): integer;
begin
  // Проверка границ
  if Direction < 0 then Direction := Direction + 6;
  if Direction > 5 then Direction := Direction - 6;
  // Ищем гекс, который существует
  while HexNear[Direction] = nil do
  begin
    // Меняем направление
    Direction := Direction + Turn;
    // Проверка границ
    if Direction < 0 then Direction := Direction + 6;
    if Direction > 5 then Direction := Direction - 6;
  end;
  // Вернём направление
  Result := Direction;
end;

// Получить координаты соседнего гекса, реально существующего или мнимого
// [Direction - Выбранный соседний гекс от 0..5]
function THex.HexNearCoord(Direction: integer): TPoint;
begin
  Result := Coord;
  case Direction of
    // Левый Верхний гекс
    0: begin
      Result.X := Coord.x-1;
      Result.Y := Coord.y-((Coord.x+1) mod 2);
    end;
    // Верхний гекс
    1: begin
      Result.X := Coord.x;
      Result.Y := Coord.y-1;
    end;
    // Правый Верхний гекс
    2: begin
      Result.X := Coord.x+1;
      Result.Y := Coord.y-((Coord.x+1) mod 2);
    end;
    // Правый Нижний гекс
    3: begin
      Result.X := Coord.x+1;
      Result.Y := Coord.y+(Coord.x mod 2);
    end;
    // Нижний гекс
    4: begin
      Result.X := Coord.x;
      Result.Y := Coord.y+1;
    end;
    // Левый Нижний гекс
    5: begin
      Result.X := Coord.x-1;
      Result.Y := Coord.y+(Coord.x mod 2);
    end;
  end;
end;

// Рисуем гекс на канве
procedure THex.Paint(HDC: TCanvas);
var
  str: string;
  textCoord: TPoint;
  TextW,TextH: integer;
begin
  inherited Paint(HDC);
  // Подписываем координаты гексов
  if HLabel^.Enabled then
    with HDC do
    begin
      // Переводим координаты гекса в строку
      str := IntToStr(Coord.X) + ',' + IntToStr(Coord.Y);
      // Половинная ширина и высота текста
      TextW := TextWidth(str) div 2;
      TextH := TextHeight(str) div 2;

      // По умолчанию текст выравнивается по середине гекса
      textCoord := GraphCoord0;
      // Выравние текста по левому краю
      if HLabel^.HPos < 0 then
        textCoord.X := GraphCoord0.X + round((HLabel^.HPos * (GraphCoord0.X - LeftTop.X - TextW) / 100));
      // Выравние текста по верхнему краю
      if HLabel^.VPos < 0 then
        textCoord.Y := GraphCoord0.Y + round((HLabel^.VPos * (GraphCoord0.Y - LeftTop.Y - TextH) / 100));
      // Выравние текста по правому краю
      if HLabel^.HPos > 0 then
        textCoord.X := GraphCoord0.X + round((HLabel^.HPos * (RightBottom.X - GraphCoord0.X - TextW) / 100));
      // Выравние текста по нижнему краю
      if HLabel^.VPos > 0 then
        textCoord.Y := GraphCoord0.Y + round((HLabel^.VPos * (RightBottom.Y - GraphCoord0.Y - TextH) / 100));

      // Рисуем строку на канве
      TextOut(textCoord.X - TextW,textCoord.Y - TextH, str);
    end;
end;

// Конструктор гексокарты
constructor THexMap.Create(l,w,h: integer);
var
  Coord,GraphCoord: TPoint;
  x,y: integer;
begin
  // Ширина и высота карты
  Width  := w;
  Height := h;
  // Длина стороны гекса в графическом представлении
  HexLen := l;
  // Информация о гексах
  SetLength(Hex,Width);
  for x:=0 to Width-1 do
    begin
      SetLength(Hex[x],Height);
      for y:=0 to Height-1 do
        begin
          Coord.X := x;
          Coord.Y := y;
          GraphCoord.X := round(HexLen*cos(pi/6)*2*(x+0.75)*cos(pi/6));
          GraphCoord.Y := round(HexLen*cos(pi/6)*2*(y+0.5+0.5*(x mod 2)));
          Hex[x,y] := THex.Create(HexLen,GraphCoord,Coord);
        end;
    end;
  // Информация о соседних гексах
  for x:=0 to Width-1 do
    for y:=0 to Height-1 do
      with Hex[x,y] do
        begin
          // Левый Верхний гекс
          if (x-1 >= 0) and (y-((x+1) mod 2) >= 0) then HexNear[0] := @Hex[x-1,y-((x+1) mod 2)] else HexNear[0] := nil;
          // Верхний гекс
          if (y-1 >= 0) then HexNear[1] := @Hex[x,y-1] else HexNear[1] := nil;
          // Правый Верхний гекс
          if (x+1 < Width) and (y-((x+1) mod 2) >= 0) then HexNear[2] := @Hex[x+1,y-((x+1) mod 2)] else HexNear[2] := nil;
          // Правый Нижний гекс
          if (x+1 < Width) and (y+(x mod 2) < Height) then HexNear[3] := @Hex[x+1,y+(x mod 2)] else HexNear[3] := nil;
          // Нижний гекс
          if (y+1 < Height) then HexNear[4] := @Hex[x,y+1] else HexNear[4] := nil;
          // Левый Нижний гекс
          if (x-1 >= 0) and (y+(x mod 2) < Height) then HexNear[5] := @Hex[x-1,y+(x mod 2)] else HexNear[5] := nil;
          // Свой гекс
          HexNear[6] := @Hex[x,y];
        end;
  // Как следует подписывать координаты гексов
  HexLabel.Enabled := False;
  HexLabel.HPos := 0;
  HexLabel.VPos := -95;
  HexLabel.Font := TFont.Create;
  HexLabel.Font.Name := 'MS Sans Serif';
  HexLabel.Font.Size := 8;
  // Следует ли показывать сетку из гексов
  HexVisible := False;
  // Передадим гексу ссылку на правила подписи координат
  for x:=0 to Width-1 do
    for y:=0 to Height-1 do
      with Hex[x,y] do
        HLabel := @HexLabel;
end;

// Рисуем карту на канве
procedure THexMap.Paint(HDC: TCanvas);
var
  i,j: integer;
  oldPenColor: TColor;
  oldFont: TFont;
  oldBrushStyle: TBrushStyle;
begin
  if HexVisible then
  begin
    // Меняем цвет карандаша
    oldPenColor := HDC.Pen.Color;
    HDC.Pen.Color := clBlack;
    // Меняем шрифт
    oldFont := HDC.Font;
    if HexLabel.Enabled then
      HDC.Font := HexLabel.Font;
    // Меняем стиль заливки
    oldBrushStyle := HDC.Brush.Style;
    HDC.Brush.Style := bsClear;

    // Рисуем гексы
    for i:=0 to Width-1 do
      for j:=0 to Height-1 do
        if Hex[i,j] <> nil then
          Hex[i,j].Paint(HDC);

    // Восстанавливаем старый цвет карандаша
    HDC.Pen.Color := oldPenColor;
    // Восстанавливаем старый шрифт
    HDC.Font := oldFont;
    // Восстанавливаем старый стиль заливки
    HDC.Brush.Style := oldBrushStyle;
  end;
end;

// Координаты гекса, над которым находится курсор
// (перевод графических координат в логические координаты)
function THexMap.MouseToCoord(MouseX,MouseY: integer): TPoint;
var
  i,                // счётчик
  ibeg, iend,       // Начальное и конечное значение счётчика (направление поиска)
  icur,             // Адаптированное значение счётчика (в пределах от 1 до 6)
  tmpx,tmpy,        // Временные координаты гекса (на период обхода соседних гексов)
  x,y: integer;     // Координаты гекса
  l,                // Расстояние от центра текущего гекса до курсора
  min,              // Минимальное расстояние от центра гекса до курсора
  r, rv: real;      // Радиусы описанной и вписанной окружности гекса
  IsBreak: boolean; // Флаг прерывания цикла (по входимости в описанный радиус)

begin
  // Начинаем свою работу с гекса расположенного в центре карты
  x := (Width div 2)  - 1;
  y := (Height div 2) - 1;
  // Инициализируем начальные данные (минимальное расстояние и радиусы окружностей)
  with Hex[x,y] do
    begin
      min := GraphLen(MouseX,MouseY);
      r   := Radius;
      rv  := RadiusVpis;
    end;
  // Инициализируем начальные данные
  // (направление поиска, флаг прерывания цикла и временные координаты)
  ibeg := 1;
  iend := 6;
  IsBreak := False;
  tmpx := x;
  tmpy := y;
  // Поиск гекса, расстояние от центра которого до курсора минимально
  while True do
    begin
      // Минимальное расстояние входит во вписанный радиус = выход
      if (min <= rv) or IsBreak then break;
      // Минимальное расстояние входит в описанный радиус = проверим соседние гексы
      if (min <= r) then IsBreak := True;
      // Поиск застопорился = выход, возвращаем (-1,-1)
      if (x = -1) or (y = -1) then break;
      // Сбрасываем временные координаты (цикл не прерван)
      if not IsBreak then
        begin
          tmpx := -1;
          tmpy := -1;
        end;
      // Проверка соседних гексов (поиск минимального расстояния)
      for i:=ibeg to iend do
      begin
        // Адаптируем значение счётчика (от 1 до 6)
        icur := i;
        if icur < 0 then icur := icur + 6;
        if icur > 5 then icur := icur - 6;
        // Соседний гекс существует
        if Hex[x,y].HexNear[icur] <> nil then
        with Hex[x,y].HexNear[icur]^ do
        begin
          // Вычисляем расстояние от центра текущего гекса до курсора
          l := GraphLen(MouseX,MouseY);
          // Расстояние минимально
          if l < min then
            begin
              // Логические координаты гекса
              tmpx := Coord.X;
              tmpy := Coord.Y;
              min := l;
              // Направление поиска
              ibeg := icur - 1;
              iend := icur + 1;
            end;
        end;
      end;
      // Записываем временные координаты в постоянные
      x := tmpx;
      y := tmpy;
    end;
  // Возвращаем результат
  Result.X := x;
  Result.Y := y;
end;

// Перевод логических координат в графические
function THexMap.CoordToGraph(Coord: TPoint): TPoint;
begin
  Result.X := round(HexLen*cos(pi/6)*2*(Coord.X+0.75)*cos(pi/6));
  Result.Y := round(HexLen*cos(pi/6)*2*(Coord.Y+0.5+0.5*(abs(Coord.X) mod 2)));
end;

// Расстояние в гексах между двумя гексами
function THexMap.HexBetween(C1,C2: TPoint): integer;
var
  dx,dy,dxy: integer;
begin
  // Это найденные опытным путём формулы (эмперически) - не удалять! :)
  dx := abs(C1.X-C2.X);
  dy := abs(C1.Y-C2.Y);

  dxy := 0;
  if ((C1.X mod 2 = 0) and (C1.Y>C2.Y)) or
  ((C1.X mod 2 = 1) and (C1.Y<C2.Y)) then
    dxy := (dx div 2)+(dx mod 2);

  if ((C1.X mod 2 = 0) and (C1.Y<C2.Y)) or
  ((C1.X mod 2 = 1) and (C1.Y>C2.Y)) then
    dxy := (dx div 2);
  if (dxy>dy) then dxy := dy;

  Result := dx+dy-dxy;
end;

// Добавляем направление в массив
procedure THexMap.AddDirection(var A: TDirArray; Direction: integer);
var
  L: integer;
begin
  L := Length(A.Ok);
  SetLength(A.Ok,L+1);
  A.Ok[L] := Direction;
  L := Length(A.All);
  SetLength(A.All,L+1);
  A.All[L] := Direction;
end;

// Удаляем направление из массива
procedure THexMap.DeleteDirection(var A: TDirArray; Num: integer);
var
  i,L: integer;
begin
  L := Length(A.Ok);
  for i:=Num+1 to L-1 do
    A.Ok[i-1] := A.Ok[i];
  SetLength(A.Ok,L-1);
end;

// Направление между двумя гексами (если возможно попасть из одного гекса в другой напрямую)
function THexMap.DirectionBetween(C1,C2: TPoint): TDirArray;
var
  dx,dy,dxy: integer;
  cnt: integer;
begin
  // Это найденные опытным путём формулы (эмперически) - не удалять! :)
  dx := abs(C1.X-C2.X);
  dy := abs(C1.Y-C2.Y);

  dxy := 0;
  if ((C1.X mod 2 = 0) and (C1.Y>C2.Y)) or
  ((C1.X mod 2 = 1) and (C1.Y<C2.Y)) then
    dxy := (dx div 2)+(dx mod 2);

  if ((C1.X mod 2 = 0) and (C1.Y<C2.Y)) or
  ((C1.X mod 2 = 1) and (C1.Y>C2.Y)) then
    dxy := (dx div 2);

  SetLength(Result.Ok,0);
  SetLength(Result.All,0);

  // Свой гекс
  if (dx=0) and (dy=0) then AddDirection(Result,6) else

  // Диагонали. Верхние/нижние гексы
  if (dx=0) and (C1.Y > C2.Y) then AddDirection(Result,1) else
  if (dx=0) and (C1.Y < C2.Y) then AddDirection(Result,4) else

  // Диагонали. Боковые гексы слева
  if (dy = dxy) and (dy>0) and (C1.X > C2.X) and (C1.Y > C2.Y) then AddDirection(Result,0) else
  if (dy = dxy) and (dy>0) and (C1.X > C2.X) and (C1.Y < C2.Y) then AddDirection(Result,5) else

  // Диагонали. Боковые гексы справа
  if (dy = dxy) and (dy>0) and (C1.X < C2.X) and (C1.Y > C2.Y) then AddDirection(Result,2) else
  if (dy = dxy) and (dy>0) and (C1.X < C2.X) and (C1.Y < C2.Y) then AddDirection(Result,3) else

  // Соседние гексы слева
  if (dx=1) and (dy=0) and (C1.X mod 2=0) and (C1.X > C2.X) then AddDirection(Result,5) else
  if (dx=1) and (dy=0) and (C1.X mod 2=1) and (C1.X > C2.X) then AddDirection(Result,0) else

  // Соседние гексы справа
  if (dx=1) and (dy=0) and (C1.X mod 2=0) and (C1.X < C2.X) then AddDirection(Result,3) else
  if (dx=1) and (dy=0) and (C1.X mod 2=1) and (C1.X < C2.X) then AddDirection(Result,2) else

  // Если координата X больше
  if (C1.X > C2.X) then
  begin
    // Если координата Y совпадает
    if (C1.Y = C2.Y) then
    begin
      AddDirection(Result,0);
      AddDirection(Result,5);
    end
    else
    // Если координата Y больше
    if (C1.Y > C2.Y) then
    begin
      AddDirection(Result,0);
      AddDirection(Result,1);
    end
    else
    // Если координата Y меньше
    begin
      AddDirection(Result,4);
      AddDirection(Result,5);
    end;
  end
  else

  // Если координата X меньше
  if (C1.X < C2.X) then
  begin
    // Если координата Y совпадает
    if (C1.Y = C2.Y) then
    begin
      AddDirection(Result,2);
      AddDirection(Result,3);
    end
    else
    // Если координата Y больше
    if (C1.Y > C2.Y) then
    begin
      AddDirection(Result,1);
      AddDirection(Result,2);
    end
    else
    // Если координата Y меньше
    begin
      AddDirection(Result,3);
      AddDirection(Result,4);
    end;
  end;

  // Проверка возможности движения в выбранных направлениях
  cnt := 0;
  while cnt < Length(Result.Ok) do
  begin
    // Если гекс не существует - удаляем направление движения к нему
    if (Hex[C1.X,C1.Y].HexNear[Result.Ok[cnt]] = nil) then
      DeleteDirection(Result,cnt)
    // Если гекс существует - переходим к следующему элементу
    else
      Inc(cnt);
  end;
end;

// Направление между двумя гексами (с ограничением по массиву гексов)
function THexMap.DirectionBetween_InArray(C1,C2: TPoint; A{Array}: array of PHex): TDirArray;
var
  cnt: integer;
begin
  // Направление между двумя гексами
  Result := DirectionBetween(C1,C2);

  // Проверка возможности движения в выбранных направлениях
  cnt := 0;
  while cnt < Length(Result.Ok) do
  begin
    // Если гекс входит в массив - переходим к следующему элементу
    if HexInArray(A,Hex[C1.X,C1.Y].HexNear[Result.Ok[cnt]]) then
      Inc(cnt)
    // Если гекс не входит в массив - удаляем направление движения к нему
    else
      DeleteDirection(Result,cnt);
  end;
end;

// Находится ли гекс среди элементов массива
function THexMap.HexInArray(A{Array}: array of PHex; H{Hex}: PHex): boolean;
var
  i: integer;
begin
  Result := False;
  for i:=Low(A) to High(A) do
  if A[i] = H then
    Result := True;
end;

// Кратчайший путь между двумя гексами (логическое представление)
function THexMap.HexPathBetween(C1,C2: TPoint): THexPath;
begin
// ???
end;

// Кратчайший путь между двумя гексами (графическое представление)
function THexMap.GraphPathBetween(C1,C2: TPoint): THexPath;
begin
// ???
end;

end.
