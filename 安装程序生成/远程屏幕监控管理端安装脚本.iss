; �ű��� Inno Setup �ű������ɡ�
; �����ĵ���ȡ���� INNO SETUP �ű��ļ���ϸ����!

[Setup]
AppName=Զ����Ļ��ع����
AppVerName=Զ����Ļ��ع���� 0.9
AppPublisher=��ʤ��
AppPublisherURL=http://www.lishengli.xin/
AppSupportURL=http://www.lishengli.xin/
AppUpdatesURL=http://www.lishengli.xin/
DefaultDirName={pf}\Զ����Ļ��ع����
DefaultGroupName=Զ����Ļ��ع����
OutputDir=D:\lishengli\RemoteScreenViewer\��װ��������
OutputBaseFilename=Զ����Ļ��ع����
SetupIconFile=D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\screenViewerServerIco.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "ch"; MessagesFile: "compiler:Languages\��������.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\RemoteScreenViewerController.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\screenViewerServerIco.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\RemoteScreenViewer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\Config.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\��װ��������\dotnetfx_2.0.50727.42.exe";  DestDir: "{tmp}"; Flags: ignoreversion dontcopy
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Code]
function InitializeSetup:Boolean;
//var Isbl:boolean;
//var Isstr:string;
//var MyProgChecked:Boolean;
var Path:string;
ResultCode:Integer;

begin
if RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v2.0') then
  begin
  Result := true;
  end
else
  begin
  if MsgBox('ϵͳ��⵽��û�а�װ.Net Framework2.0���Ƿ����ڰ�װ��', mbConfirmation, MB_YESNO) = idYes then
    begin
    ExtractTemporaryFile('dotnetfx_2.0.50727.42.exe');
    Path := ExpandConstant('{tmp}\dotnetfx_2.0.50727.42.exe');
//    if FileOrDirExists(Path) then
//      begin
    Exec(Path,'/q:a /c:"install /l /q"','',SW_SHOWNORMAL,ewWaitUntilTerminated,ResultCode);
    if RegKeyExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework\policy\v2.0') then
      begin
      Result :=true;
      end
    else
      begin
      MsgBox('δ�ܳɹ���װ.Net Framework2.0���л�����ϵͳ���޷����У�����װ���򼴽��˳���',mbInformation,MB_OK);
      Result :=false;
      end;
    end
  else
    begin
    Result := false;
    end;
  end;
end;

[Icons]
Name: "{group}\Զ����Ļ��ع����"; Filename: "{app}\RemoteScreenViewerController.exe"
Name: "{group}\{cm:UninstallProgram,Զ����Ļ��ع����}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\Զ����Ļ��ع����"; Filename: "{app}\RemoteScreenViewerController.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\RemoteScreenViewerController.exe"; Description: "{cm:LaunchProgram,Զ����Ļ��ع����}"; Flags: nowait postinstall skipifsilent

