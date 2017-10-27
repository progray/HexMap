unit ClassUnit;

interface

uses Types, Graphics, Math, SysUtils;

type
  // ������ �� ����
  PHex = ^THex;

  // ������ �� ������� ���������� �����
  PHexLabel = ^THexLabel;

  { ���������� ������ }
  TRightFigure = class
    // ���������� ������ ������
    Count: integer;
    // ������� ������
    Len: integer;
    // ������ ��������� ����������
    Radius: real;
    // ������ ��������� ����������
    RadiusVpis: real;
    // ���������� ������ ������
    GraphCoord0: TPoint;
    // ���������� ������ ������
    GraphCoord: array of TPoint;
    // ����� ������� ���� ��������, � ������� ����������� ������
    LeftTop: TPoint;
    // ������ ������ ���� ��������, � ������� ����������� ������
    RightBottom: TPoint;
    // �����������
    constructor Create(n,l: integer; c: TPoint);
    // ������ ������ �� �����
    procedure Paint(HDC: TCanvas);
    // ���������� �� ������ ������ �� �������� ����� �� �������
    function GraphLen(X,Y: integer): real;
  end;

  { ����� � ������������ ����� }
  THexLabel = record
    // ����������� ���������� ������
    Enabled: boolean;
    // ������� �� ����������� � ��������� (� % ������������ ������ �����):
    // [-100% - ����� (�������) ����, 0% - �����, 100% - ������ (������) ����]
    HPos,VPos: integer;
    // ������������ �����
    Font: TFont;
  end;

  // ����������� �������� (������)
  TDirArray = record
    Ok: array of integer;
    All: array of integer;
  end;

  // ���� �� ��������� ����� � ��������
  THexPath = record
    // ??? ���������� ��������� ����
    // ������ �� ��������� ����
    StartHex: PHex;
    // ������ �� �������� ����
    EndHex: PHex;
    // ������ �����������, ������ ������� ����� ��������� �� ���������� � �������� ����
    // [1 ����� - ������� ����, 2 ����� - �����������]
    Direction: array of array of integer;
  end;

  { ���� }
  THex = class(TRightFigure)
    // ���������� ����� �� �����
    Coord: TPoint;
    // ������ �� �������� ����� (0-5 = �������� �����, 6 = ���� ����)
    HexNear: array [0..6] of PHex;
    // ������ �� ������� ������� ��������� �����
    HLabel: PHexLabel;
    // �����������
    constructor Create(l: integer; c,map: TPoint);
    // ����������
    destructor Destroy; override;
    // �������� ����� ��������� �����, ������� ����������
    // [Direction - ��������� �������� ���� �� 0..5 ]
    // [Turn - �������, ���� ���� �� ����������]
    // [-1 = ������ ������� �������, +1 = �� ������� �������]
    function GetHexNear(Direction,Turn: integer): integer;
    // �������� ���������� ��������� �����, ������� ������������� ��� �������
    // [Direction - ��������� �������� ���� �� 0..5]
    function HexNearCoord(Direction: integer): TPoint;
    // ������ ���� �� �����
    procedure Paint(HDC: TCanvas);
  end;

  { ���������� }
  THexMap = class
    // ������ � ������ �����
    Width: integer;
    Height: integer;
    // ����� ������� ����� � ����������� �������������
    HexLen: integer;
    // ���������� � ������
    Hex: array of array of THex;
    // ��� ������� ����������� ���������� ������
    HexLabel: THexLabel;
    // ������� �� ���������� ����� �� ������
    HexVisible: boolean;
    // �����������
    constructor Create(l,w,h: integer);
    // ������ ����� �� �����
    procedure Paint(HDC: TCanvas);
    // ���������� �����, ��� ������� ��������� ������
    // (������� ����������� ��������� � ���������� ����������)
    function MouseToCoord(MouseX, MouseY: integer): TPoint;
    // ������� ���������� ��������� � �����������
    function CoordToGraph(Coord: TPoint): TPoint;
    // ���������� � ������ ����� ����� �������
    function HexBetween(C1,C2: TPoint): integer;
    // ��������� ����������� � ������
    procedure AddDirection(var A: TDirArray; Direction: integer);
    // ������� ����������� �� �������
    procedure DeleteDirection(var A: TDirArray; Num: integer);
    // ����������� ����� ����� �������
    function DirectionBetween(C1,C2: TPoint): TDirArray;
    // ����������� ����� ����� ������� (� ������������ �� ������� ������)
    function DirectionBetween_InArray(C1,C2: TPoint; A{Array}: array of PHex): TDirArray;
    // ��������� �� ���� ����� ��������� �������
    function HexInArray(A{Array}: array of PHex; H{Hex}: PHex): boolean;
    // ���������� ���� ����� ����� ������� (���������� �������������)
    function HexPathBetween(C1,C2: TPoint): THexPath;
    // ���������� ���� ����� ����� ������� (����������� �������������)
    function GraphPathBetween(C1,C2: TPoint): THexPath;
  end;

implementation

// ����������� ���������� ������
constructor TRightFigure.Create(n,l: integer; c: TPoint);
var
  i: integer;
  alpha: real;
begin
  // ���������� ������ ������
  Count := n;
  // ������� ������
  Len := l;
  // ���������� ������ ������
  GraphCoord0.X := c.X;
  GraphCoord0.Y := c.Y;
  // ���� ��������
  alpha := (2 * pi) / Count;
  // ������ ��������� ����������
  Radius := Len / (2 * sin(alpha/2));
  // ������ ��������� ����������
  RadiusVpis := Len / (2 * tan(alpha/2));
  // ���������� ������ ������
  SetLength(GraphCoord,Count);
  for i := 0 to n-1 do
    begin
      GraphCoord[i].X := round(GraphCoord0.X + Radius * cos(alpha*i));
      GraphCoord[i].Y := round(GraphCoord0.Y + Radius * sin(alpha*i));
    end;

  // ����� ����������� � ������������ ���������� X � Y
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

// ������ ���������� ������ �� �����
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

// ���������� �� ������ ������ �� �������� ����� �� �������
function TRightFigure.GraphLen(X,Y: integer): real;
begin
  Result := sqrt(sqr(X - GraphCoord0.X) + sqr(Y - GraphCoord0.Y));
end;

// ����������� �����
constructor THex.Create(l: integer; c,map: TPoint);
begin
  inherited Create(6,l,c);
  // ���������� ����� �� �����
  Coord := map;
end;

// ���������� �����
destructor THex.Destroy;
var
  i,j: integer;
begin
  // ������� ���� �� ������ �������� ������
  for i:=0 to 5 do
    if HexNear[i] <> nil then
      for j:=0 to 6 do
        if (HexNear[i]^.HexNear[j] <> nil) and
        (Coord.X = HexNear[i]^.HexNear[j]^.Coord.X) and
        (Coord.Y = HexNear[i]^.HexNear[j]^.Coord.Y) then
          HexNear[i]^.HexNear[j] := nil;
  // ������������ �����
  inherited Destroy;
end;

// �������� ����� ��������� �����, ������� ����������
// [Direction - ��������� �������� ���� �� 0..5 ]
// [Turn - �������, ���� ���� �� ����������]
// [-1 = ������ ������� �������, +1 = �� ������� �������]
function THex.GetHexNear(Direction,Turn: integer): integer;
begin
  // �������� ������
  if Direction < 0 then Direction := Direction + 6;
  if Direction > 5 then Direction := Direction - 6;
  // ���� ����, ������� ����������
  while HexNear[Direction] = nil do
  begin
    // ������ �����������
    Direction := Direction + Turn;
    // �������� ������
    if Direction < 0 then Direction := Direction + 6;
    if Direction > 5 then Direction := Direction - 6;
  end;
  // ����� �����������
  Result := Direction;
end;

// �������� ���������� ��������� �����, ������� ������������� ��� �������
// [Direction - ��������� �������� ���� �� 0..5]
function THex.HexNearCoord(Direction: integer): TPoint;
begin
  Result := Coord;
  case Direction of
    // ����� ������� ����
    0: begin
      Result.X := Coord.x-1;
      Result.Y := Coord.y-((Coord.x+1) mod 2);
    end;
    // ������� ����
    1: begin
      Result.X := Coord.x;
      Result.Y := Coord.y-1;
    end;
    // ������ ������� ����
    2: begin
      Result.X := Coord.x+1;
      Result.Y := Coord.y-((Coord.x+1) mod 2);
    end;
    // ������ ������ ����
    3: begin
      Result.X := Coord.x+1;
      Result.Y := Coord.y+(Coord.x mod 2);
    end;
    // ������ ����
    4: begin
      Result.X := Coord.x;
      Result.Y := Coord.y+1;
    end;
    // ����� ������ ����
    5: begin
      Result.X := Coord.x-1;
      Result.Y := Coord.y+(Coord.x mod 2);
    end;
  end;
end;

// ������ ���� �� �����
procedure THex.Paint(HDC: TCanvas);
var
  str: string;
  textCoord: TPoint;
  TextW,TextH: integer;
begin
  inherited Paint(HDC);
  // ����������� ���������� ������
  if HLabel^.Enabled then
    with HDC do
    begin
      // ��������� ���������� ����� � ������
      str := IntToStr(Coord.X) + ',' + IntToStr(Coord.Y);
      // ���������� ������ � ������ ������
      TextW := TextWidth(str) div 2;
      TextH := TextHeight(str) div 2;

      // �� ��������� ����� ������������� �� �������� �����
      textCoord := GraphCoord0;
      // �������� ������ �� ������ ����
      if HLabel^.HPos < 0 then
        textCoord.X := GraphCoord0.X + round((HLabel^.HPos * (GraphCoord0.X - LeftTop.X - TextW) / 100));
      // �������� ������ �� �������� ����
      if HLabel^.VPos < 0 then
        textCoord.Y := GraphCoord0.Y + round((HLabel^.VPos * (GraphCoord0.Y - LeftTop.Y - TextH) / 100));
      // �������� ������ �� ������� ����
      if HLabel^.HPos > 0 then
        textCoord.X := GraphCoord0.X + round((HLabel^.HPos * (RightBottom.X - GraphCoord0.X - TextW) / 100));
      // �������� ������ �� ������� ����
      if HLabel^.VPos > 0 then
        textCoord.Y := GraphCoord0.Y + round((HLabel^.VPos * (RightBottom.Y - GraphCoord0.Y - TextH) / 100));

      // ������ ������ �� �����
      TextOut(textCoord.X - TextW,textCoord.Y - TextH, str);
    end;
end;

// ����������� ����������
constructor THexMap.Create(l,w,h: integer);
var
  Coord,GraphCoord: TPoint;
  x,y: integer;
begin
  // ������ � ������ �����
  Width  := w;
  Height := h;
  // ����� ������� ����� � ����������� �������������
  HexLen := l;
  // ���������� � ������
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
  // ���������� � �������� ������
  for x:=0 to Width-1 do
    for y:=0 to Height-1 do
      with Hex[x,y] do
        begin
          // ����� ������� ����
          if (x-1 >= 0) and (y-((x+1) mod 2) >= 0) then HexNear[0] := @Hex[x-1,y-((x+1) mod 2)] else HexNear[0] := nil;
          // ������� ����
          if (y-1 >= 0) then HexNear[1] := @Hex[x,y-1] else HexNear[1] := nil;
          // ������ ������� ����
          if (x+1 < Width) and (y-((x+1) mod 2) >= 0) then HexNear[2] := @Hex[x+1,y-((x+1) mod 2)] else HexNear[2] := nil;
          // ������ ������ ����
          if (x+1 < Width) and (y+(x mod 2) < Height) then HexNear[3] := @Hex[x+1,y+(x mod 2)] else HexNear[3] := nil;
          // ������ ����
          if (y+1 < Height) then HexNear[4] := @Hex[x,y+1] else HexNear[4] := nil;
          // ����� ������ ����
          if (x-1 >= 0) and (y+(x mod 2) < Height) then HexNear[5] := @Hex[x-1,y+(x mod 2)] else HexNear[5] := nil;
          // ���� ����
          HexNear[6] := @Hex[x,y];
        end;
  // ��� ������� ����������� ���������� ������
  HexLabel.Enabled := False;
  HexLabel.HPos := 0;
  HexLabel.VPos := -95;
  HexLabel.Font := TFont.Create;
  HexLabel.Font.Name := 'MS Sans Serif';
  HexLabel.Font.Size := 8;
  // ������� �� ���������� ����� �� ������
  HexVisible := False;
  // ��������� ����� ������ �� ������� ������� ���������
  for x:=0 to Width-1 do
    for y:=0 to Height-1 do
      with Hex[x,y] do
        HLabel := @HexLabel;
end;

// ������ ����� �� �����
procedure THexMap.Paint(HDC: TCanvas);
var
  i,j: integer;
  oldPenColor: TColor;
  oldFont: TFont;
  oldBrushStyle: TBrushStyle;
begin
  if HexVisible then
  begin
    // ������ ���� ���������
    oldPenColor := HDC.Pen.Color;
    HDC.Pen.Color := clBlack;
    // ������ �����
    oldFont := HDC.Font;
    if HexLabel.Enabled then
      HDC.Font := HexLabel.Font;
    // ������ ����� �������
    oldBrushStyle := HDC.Brush.Style;
    HDC.Brush.Style := bsClear;

    // ������ �����
    for i:=0 to Width-1 do
      for j:=0 to Height-1 do
        if Hex[i,j] <> nil then
          Hex[i,j].Paint(HDC);

    // ��������������� ������ ���� ���������
    HDC.Pen.Color := oldPenColor;
    // ��������������� ������ �����
    HDC.Font := oldFont;
    // ��������������� ������ ����� �������
    HDC.Brush.Style := oldBrushStyle;
  end;
end;

// ���������� �����, ��� ������� ��������� ������
// (������� ����������� ��������� � ���������� ����������)
function THexMap.MouseToCoord(MouseX,MouseY: integer): TPoint;
var
  i,                // �������
  ibeg, iend,       // ��������� � �������� �������� �������� (����������� ������)
  icur,             // �������������� �������� �������� (� �������� �� 1 �� 6)
  tmpx,tmpy,        // ��������� ���������� ����� (�� ������ ������ �������� ������)
  x,y: integer;     // ���������� �����
  l,                // ���������� �� ������ �������� ����� �� �������
  min,              // ����������� ���������� �� ������ ����� �� �������
  r, rv: real;      // ������� ��������� � ��������� ���������� �����
  IsBreak: boolean; // ���� ���������� ����� (�� ���������� � ��������� ������)

begin
  // �������� ���� ������ � ����� �������������� � ������ �����
  x := (Width div 2)  - 1;
  y := (Height div 2) - 1;
  // �������������� ��������� ������ (����������� ���������� � ������� �����������)
  with Hex[x,y] do
    begin
      min := GraphLen(MouseX,MouseY);
      r   := Radius;
      rv  := RadiusVpis;
    end;
  // �������������� ��������� ������
  // (����������� ������, ���� ���������� ����� � ��������� ����������)
  ibeg := 1;
  iend := 6;
  IsBreak := False;
  tmpx := x;
  tmpy := y;
  // ����� �����, ���������� �� ������ �������� �� ������� ����������
  while True do
    begin
      // ����������� ���������� ������ �� ��������� ������ = �����
      if (min <= rv) or IsBreak then break;
      // ����������� ���������� ������ � ��������� ������ = �������� �������� �����
      if (min <= r) then IsBreak := True;
      // ����� ������������ = �����, ���������� (-1,-1)
      if (x = -1) or (y = -1) then break;
      // ���������� ��������� ���������� (���� �� �������)
      if not IsBreak then
        begin
          tmpx := -1;
          tmpy := -1;
        end;
      // �������� �������� ������ (����� ������������ ����������)
      for i:=ibeg to iend do
      begin
        // ���������� �������� �������� (�� 1 �� 6)
        icur := i;
        if icur < 0 then icur := icur + 6;
        if icur > 5 then icur := icur - 6;
        // �������� ���� ����������
        if Hex[x,y].HexNear[icur] <> nil then
        with Hex[x,y].HexNear[icur]^ do
        begin
          // ��������� ���������� �� ������ �������� ����� �� �������
          l := GraphLen(MouseX,MouseY);
          // ���������� ����������
          if l < min then
            begin
              // ���������� ���������� �����
              tmpx := Coord.X;
              tmpy := Coord.Y;
              min := l;
              // ����������� ������
              ibeg := icur - 1;
              iend := icur + 1;
            end;
        end;
      end;
      // ���������� ��������� ���������� � ����������
      x := tmpx;
      y := tmpy;
    end;
  // ���������� ���������
  Result.X := x;
  Result.Y := y;
end;

// ������� ���������� ��������� � �����������
function THexMap.CoordToGraph(Coord: TPoint): TPoint;
begin
  Result.X := round(HexLen*cos(pi/6)*2*(Coord.X+0.75)*cos(pi/6));
  Result.Y := round(HexLen*cos(pi/6)*2*(Coord.Y+0.5+0.5*(abs(Coord.X) mod 2)));
end;

// ���������� � ������ ����� ����� �������
function THexMap.HexBetween(C1,C2: TPoint): integer;
var
  dx,dy,dxy: integer;
begin
  // ��� ��������� ������� ���� ������� (�����������) - �� �������! :)
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

// ��������� ����������� � ������
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

// ������� ����������� �� �������
procedure THexMap.DeleteDirection(var A: TDirArray; Num: integer);
var
  i,L: integer;
begin
  L := Length(A.Ok);
  for i:=Num+1 to L-1 do
    A.Ok[i-1] := A.Ok[i];
  SetLength(A.Ok,L-1);
end;

// ����������� ����� ����� ������� (���� �������� ������� �� ������ ����� � ������ ��������)
function THexMap.DirectionBetween(C1,C2: TPoint): TDirArray;
var
  dx,dy,dxy: integer;
  cnt: integer;
begin
  // ��� ��������� ������� ���� ������� (�����������) - �� �������! :)
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

  // ���� ����
  if (dx=0) and (dy=0) then AddDirection(Result,6) else

  // ���������. �������/������ �����
  if (dx=0) and (C1.Y > C2.Y) then AddDirection(Result,1) else
  if (dx=0) and (C1.Y < C2.Y) then AddDirection(Result,4) else

  // ���������. ������� ����� �����
  if (dy = dxy) and (dy>0) and (C1.X > C2.X) and (C1.Y > C2.Y) then AddDirection(Result,0) else
  if (dy = dxy) and (dy>0) and (C1.X > C2.X) and (C1.Y < C2.Y) then AddDirection(Result,5) else

  // ���������. ������� ����� ������
  if (dy = dxy) and (dy>0) and (C1.X < C2.X) and (C1.Y > C2.Y) then AddDirection(Result,2) else
  if (dy = dxy) and (dy>0) and (C1.X < C2.X) and (C1.Y < C2.Y) then AddDirection(Result,3) else

  // �������� ����� �����
  if (dx=1) and (dy=0) and (C1.X mod 2=0) and (C1.X > C2.X) then AddDirection(Result,5) else
  if (dx=1) and (dy=0) and (C1.X mod 2=1) and (C1.X > C2.X) then AddDirection(Result,0) else

  // �������� ����� ������
  if (dx=1) and (dy=0) and (C1.X mod 2=0) and (C1.X < C2.X) then AddDirection(Result,3) else
  if (dx=1) and (dy=0) and (C1.X mod 2=1) and (C1.X < C2.X) then AddDirection(Result,2) else

  // ���� ���������� X ������
  if (C1.X > C2.X) then
  begin
    // ���� ���������� Y ���������
    if (C1.Y = C2.Y) then
    begin
      AddDirection(Result,0);
      AddDirection(Result,5);
    end
    else
    // ���� ���������� Y ������
    if (C1.Y > C2.Y) then
    begin
      AddDirection(Result,0);
      AddDirection(Result,1);
    end
    else
    // ���� ���������� Y ������
    begin
      AddDirection(Result,4);
      AddDirection(Result,5);
    end;
  end
  else

  // ���� ���������� X ������
  if (C1.X < C2.X) then
  begin
    // ���� ���������� Y ���������
    if (C1.Y = C2.Y) then
    begin
      AddDirection(Result,2);
      AddDirection(Result,3);
    end
    else
    // ���� ���������� Y ������
    if (C1.Y > C2.Y) then
    begin
      AddDirection(Result,1);
      AddDirection(Result,2);
    end
    else
    // ���� ���������� Y ������
    begin
      AddDirection(Result,3);
      AddDirection(Result,4);
    end;
  end;

  // �������� ����������� �������� � ��������� ������������
  cnt := 0;
  while cnt < Length(Result.Ok) do
  begin
    // ���� ���� �� ���������� - ������� ����������� �������� � ����
    if (Hex[C1.X,C1.Y].HexNear[Result.Ok[cnt]] = nil) then
      DeleteDirection(Result,cnt)
    // ���� ���� ���������� - ��������� � ���������� ��������
    else
      Inc(cnt);
  end;
end;

// ����������� ����� ����� ������� (� ������������ �� ������� ������)
function THexMap.DirectionBetween_InArray(C1,C2: TPoint; A{Array}: array of PHex): TDirArray;
var
  cnt: integer;
begin
  // ����������� ����� ����� �������
  Result := DirectionBetween(C1,C2);

  // �������� ����������� �������� � ��������� ������������
  cnt := 0;
  while cnt < Length(Result.Ok) do
  begin
    // ���� ���� ������ � ������ - ��������� � ���������� ��������
    if HexInArray(A,Hex[C1.X,C1.Y].HexNear[Result.Ok[cnt]]) then
      Inc(cnt)
    // ���� ���� �� ������ � ������ - ������� ����������� �������� � ����
    else
      DeleteDirection(Result,cnt);
  end;
end;

// ��������� �� ���� ����� ��������� �������
function THexMap.HexInArray(A{Array}: array of PHex; H{Hex}: PHex): boolean;
var
  i: integer;
begin
  Result := False;
  for i:=Low(A) to High(A) do
  if A[i] = H then
    Result := True;
end;

// ���������� ���� ����� ����� ������� (���������� �������������)
function THexMap.HexPathBetween(C1,C2: TPoint): THexPath;
begin
// ???
end;

// ���������� ���� ����� ����� ������� (����������� �������������)
function THexMap.GraphPathBetween(C1,C2: TPoint): THexPath;
begin
// ???
end;

end.

