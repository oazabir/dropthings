SET DEST=Dropthings-2.7.6-src
SET ZIP="C:\Program Files\7-Zip\7z.exe"
SET SRC=..
rd %DEST% /s /q

md %DEST%
md %DEST%\src
md %DEST%\ThirdParty

del %SRC%\src\Dropthings\App_Data\*.log /Q
rd %SRC%\src\TestResults /s /q

xcopy %SRC%\src\*.* %DEST%\src /E /Q /EXCLUDE:ExcludeFilesFromRelease.txt
xcopy %SRC%\thirdparty\*.* %DEST%\Thirdparty /E /Q /EXCLUDE:ExcludeFilesFromRelease.txt
xcopy %SRC%\src\Dropthings\bin\*.dll.refresh %DEST%\src\Dropthings\bin\ /Q

del %DEST%.zip
%ZIP% a %DEST%.zip %DEST%
rd %DEST% /s /q