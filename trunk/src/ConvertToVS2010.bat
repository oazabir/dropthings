REM Keep the target framework version to 3.5, don't update grade for backward compatibility
..\thirdparty\fart\fart.exe -r *.csproj ">4.0<" ">3.5<"
..\thirdparty\fart\fart.exe -r *.csproj ">3.5</O" ">4.0</O"
REM But do change the toolsversion. Otherwise VS 2010 will ask the projects to be upgraded every time
..\thirdparty\fart\fart.exe -r *.csproj "ToolsVersion=\"3.5\""  "ToolsVersion=\"4.0\""