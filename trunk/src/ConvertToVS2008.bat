REM Downgrade all platform version
..\thirdparty\fart\fart.exe -r *.csproj ">4.0<" ">3.5<"
REM Downgrade toolsversion to 3.5, otherwise VS 2008 will barf
..\thirdparty\fart\fart.exe -r *.csproj "ToolsVersion=\"4.0\""  "ToolsVersion=\"3.5\""