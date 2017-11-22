; 脚本用 Inno Setup 脚本向导生成。
; 查阅文档获取创建 INNO SETUP 脚本文件详细资料!

[Setup]
AppName=远程屏幕监控管理端
AppVerName=远程屏幕监控管理端 0.9
AppPublisher=李胜利
AppPublisherURL=http://www.lishengli.xin/
AppSupportURL=http://www.lishengli.xin/
AppUpdatesURL=http://www.lishengli.xin/
DefaultDirName={pf}\远程屏幕监控管理端
DefaultGroupName=远程屏幕监控管理端
OutputDir=D:\lishengli\RemoteScreenViewer\安装程序生成
OutputBaseFilename=远程屏幕监控管理端
SetupIconFile=D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\screenViewerServerIco.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "ch"; MessagesFile: "compiler:Languages\简体中文.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\RemoteScreenViewerController.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\screenViewerServerIco.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\RemoteScreenViewer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\RemoteScreenViewerController\bin\Debug\Config.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\lishengli\RemoteScreenViewer\安装程序生成\dotnetfx_2.0.50727.42.exe";  DestDir: "{tmp}"; Flags: ignoreversion dontcopy
; 注意: 不要在任何共享系统文件中使用“Flags: ignoreversion”

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
  if MsgBox('系统检测到您没有安装.Net Framework2.0，是否现在安装？', mbConfirmation, MB_YESNO) = idYes then
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
      MsgBox('未能成功安装.Net Framework2.0运行环境，系统将无法运行，本安装程序即将退出！',mbInformation,MB_OK);
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
Name: "{group}\远程屏幕监控管理端"; Filename: "{app}\RemoteScreenViewerController.exe"
Name: "{group}\{cm:UninstallProgram,远程屏幕监控管理端}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\远程屏幕监控管理端"; Filename: "{app}\RemoteScreenViewerController.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\RemoteScreenViewerController.exe"; Description: "{cm:LaunchProgram,远程屏幕监控管理端}"; Flags: nowait postinstall skipifsilent

