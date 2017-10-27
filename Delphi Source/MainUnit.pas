unit MainUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Math, ClassUnit;

type
  TfrmMain = class(TForm)
    ImageBoard: TImage;
    HexList: TListBox;
    PathType: TRadioGroup;
    procedure ImageBoardMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure ImageBoardClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    // Построение кратчайшего пути
    procedure HexPathShow;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  HexMap: THexMap;
  // Начальная и конечная точки (гекс)
  StartCoord,EndCoord: TPoint;
  // Начальная и конечная точки (графические координаты)
  StartGraph,EndGraph: TPoint;
  // [FALSE - устанавливаем первый гекс / TRUE - устанавливаем второй гекс]
  IsFinalHex: boolean;
  // Координаты курсора
  CursorCoord: TPoint;
  frmMain: TfrmMain;

implementation

{$R *.dfm}

procedure TfrmMain.ImageBoardMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
var
  HexCoord: TPoint;
begin
  // В заголовке окна выводим координаты курсора и координаты гекса под ним
  if HexMap = nil then exit;
  CursorCoord.X := X;
  CursorCoord.Y := Y;
  HexCoord := HexMap.MouseToCoord(X,Y);
  Caption := 'Mouse (' + IntToStr(X) + ',' + IntToStr(Y) + ') Coord (' + IntToStr(HexCoord.X) + ',' + IntToStr(HexCoord.Y) + ')';
end;

// Построение кратчайшего пути
procedure TfrmMain.HexPathShow;
begin
{???
  SetLength(HexArray,0);
  // Логический путь
  if PathType.ItemIndex = 0 then
  begin
    HexList.Items.Clear;
    HexArray := HexMap.HexPathBetween(StartCoord,EndCoord);
    for i:=0 to Length(HexArray)-1 do
      HexList.Items.Add(IntToStr(HexArray[i]^.Coord.X) + ':' + IntToStr(HexArray[i]^.Coord.Y));
  end
  else
  // Графический путь
  if PathType.ItemIndex = 1 then
  begin
    HexList.Items.Clear;
    HexArray := HexMap.HexPathBetween(StartGraph,EndGraph);
    for i:=0 to Length(HexArray)-1 do
      HexList.Items.Add(IntToStr(HexArray[i]^.Coord.X) + ':' + IntToStr(HexArray[i]^.Coord.Y));
  end;}
end;

// Начальная и конечная точки
procedure TfrmMain.ImageBoardClick(Sender: TObject);
var
  oldPenColor,
  oldBrushColor: TColor;
  HexCoord: TPoint;
begin
  // Сохраним старые значения
  oldPenColor := ImageBoard.Canvas.Pen.Color;
  oldBrushColor := ImageBoard.Canvas.Brush.Color;

  // Переводим графические координаты в координаты гекса
  HexCoord := HexMap.MouseToCoord(CursorCoord.X,CursorCoord.Y);

  // Первый щелчок мыши - установка начальной точки (флаг)
  if not IsFinalHex and (HexCoord.X <> -1) and (HexCoord.Y <> -1) then
    begin
      StartCoord := HexCoord;
      StartGraph := CursorCoord;
      IsFinalHex := True;
      with ImageBoard.Canvas do
        begin
          Brush.Color := clWhite;
          Pen.Color := clBlack;
          Rectangle(0,0,ImageBoard.Width,ImageBoard.Height);
          HexMap.Paint(ImageBoard.Canvas);

          Pen.Color := clRed;
          MoveTo(StartGraph.X,StartGraph.Y);
          LineTo(StartGraph.X,StartGraph.Y-50);
          LineTo(StartGraph.X+25,StartGraph.Y-40);
          LineTo(StartGraph.X,StartGraph.Y-30);
        end;
    end
  else
  // Второй щелчок мыши - установка конечной точки (флаг и стрелка от одного флага к другому)
  if IsFinalHex and (HexCoord.X <> -1) and (HexCoord.Y <> -1) then
    begin
      EndCoord := HexCoord;
      EndGraph := CursorCoord;
      IsFinalHex := False;
      with ImageBoard.Canvas do
        begin
          Pen.Color := clRed;
          MoveTo(EndGraph.X,EndGraph.Y);
          LineTo(EndGraph.X,EndGraph.Y-50);
          LineTo(EndGraph.X+25,EndGraph.Y-40);
          LineTo(EndGraph.X,EndGraph.Y-30);

          MoveTo(StartGraph.X,StartGraph.Y);
          LineTo(EndGraph.X,EndGraph.Y);
        end;
      // Построение кратчайшего пути
      HexPathShow;
    end;

  // Восстановим старые значения
  ImageBoard.Canvas.Pen.Color := oldPenColor;
  ImageBoard.Canvas.Brush.Color := oldBrushColor;
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  // Устанавливаем первый гекс
  IsFinalHex := False;

  // Создаем гексокарту
  HexMap := THexMap.Create(30,10,10);

  // Рисуем гексокарту
  ImageBoard.Canvas.Brush.Color := clWhite;
  ImageBoard.Canvas.Pen.Color := clBlack;
  ImageBoard.Canvas.Rectangle(0,0,ImageBoard.Width,ImageBoard.Height);
  HexMap.Paint(ImageBoard.Canvas);
end;

end.
