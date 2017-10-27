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
    // ���������� ����������� ����
    procedure HexPathShow;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  HexMap: THexMap;
  // ��������� � �������� ����� (����)
  StartCoord,EndCoord: TPoint;
  // ��������� � �������� ����� (����������� ����������)
  StartGraph,EndGraph: TPoint;
  // [FALSE - ������������� ������ ���� / TRUE - ������������� ������ ����]
  IsFinalHex: boolean;
  // ���������� �������
  CursorCoord: TPoint;
  frmMain: TfrmMain;

implementation

{$R *.dfm}

procedure TfrmMain.ImageBoardMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
var
  HexCoord: TPoint;
begin
  // � ��������� ���� ������� ���������� ������� � ���������� ����� ��� ���
  if HexMap = nil then exit;
  CursorCoord.X := X;
  CursorCoord.Y := Y;
  HexCoord := HexMap.MouseToCoord(X,Y);
  Caption := 'Mouse (' + IntToStr(X) + ',' + IntToStr(Y) + ') Coord (' + IntToStr(HexCoord.X) + ',' + IntToStr(HexCoord.Y) + ')';
end;

// ���������� ����������� ����
procedure TfrmMain.HexPathShow;
begin
{???
  SetLength(HexArray,0);
  // ���������� ����
  if PathType.ItemIndex = 0 then
  begin
    HexList.Items.Clear;
    HexArray := HexMap.HexPathBetween(StartCoord,EndCoord);
    for i:=0 to Length(HexArray)-1 do
      HexList.Items.Add(IntToStr(HexArray[i]^.Coord.X) + ':' + IntToStr(HexArray[i]^.Coord.Y));
  end
  else
  // ����������� ����
  if PathType.ItemIndex = 1 then
  begin
    HexList.Items.Clear;
    HexArray := HexMap.HexPathBetween(StartGraph,EndGraph);
    for i:=0 to Length(HexArray)-1 do
      HexList.Items.Add(IntToStr(HexArray[i]^.Coord.X) + ':' + IntToStr(HexArray[i]^.Coord.Y));
  end;}
end;

// ��������� � �������� �����
procedure TfrmMain.ImageBoardClick(Sender: TObject);
var
  oldPenColor,
  oldBrushColor: TColor;
  HexCoord: TPoint;
begin
  // �������� ������ ��������
  oldPenColor := ImageBoard.Canvas.Pen.Color;
  oldBrushColor := ImageBoard.Canvas.Brush.Color;

  // ��������� ����������� ���������� � ���������� �����
  HexCoord := HexMap.MouseToCoord(CursorCoord.X,CursorCoord.Y);

  // ������ ������ ���� - ��������� ��������� ����� (����)
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
  // ������ ������ ���� - ��������� �������� ����� (���� � ������� �� ������ ����� � �������)
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
      // ���������� ����������� ����
      HexPathShow;
    end;

  // ����������� ������ ��������
  ImageBoard.Canvas.Pen.Color := oldPenColor;
  ImageBoard.Canvas.Brush.Color := oldBrushColor;
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  // ������������� ������ ����
  IsFinalHex := False;

  // ������� ����������
  HexMap := THexMap.Create(30,10,10);

  // ������ ����������
  ImageBoard.Canvas.Brush.Color := clWhite;
  ImageBoard.Canvas.Pen.Color := clBlack;
  ImageBoard.Canvas.Rectangle(0,0,ImageBoard.Width,ImageBoard.Height);
  HexMap.Paint(ImageBoard.Canvas);
end;

end.
