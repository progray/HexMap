program HexMap;

uses
  Forms,
  MainUnit in 'MainUnit.pas' {frmMain},
  ClassUnit in 'ClassUnit.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
