object frmMain: TfrmMain
  Left = 244
  Top = 63
  Width = 847
  Height = 608
  Caption = 'frmMain'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object ImageBoard: TImage
    Left = 8
    Top = 8
    Width = 481
    Height = 553
    OnClick = ImageBoardClick
    OnMouseMove = ImageBoardMouseMove
  end
  object HexList: TListBox
    Left = 494
    Top = 72
    Width = 170
    Height = 488
    ItemHeight = 13
    TabOrder = 0
  end
  object PathType: TRadioGroup
    Left = 494
    Top = 7
    Width = 176
    Height = 59
    Caption = #1050#1088#1072#1090#1095#1072#1081#1096#1080#1081' '#1087#1091#1090#1100
    ItemIndex = 0
    Items.Strings = (
      #1051#1086#1075#1080#1095#1077#1089#1082#1080#1081
      #1043#1088#1072#1092#1080#1095#1077#1089#1082#1080#1081)
    TabOrder = 1
  end
end
