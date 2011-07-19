SET PACKAGE_FILE=DropthingsPackageV2-7-6.zip
SET ZIP="C:\Program Files\7-Zip\7z.exe"
SET PACKAGE_FOLDER=DropthingsPackage
SET WEBSITE_FOLDER=DropthingsPackage\Dropthings
rd /s /q %WEBSITE_FOLDER%
md %WEBSITE_FOLDER%
xcopy ..\src\Dropthings\*.* %WEBSITE_FOLDER%\ /E /Q
del %WEBSITE_FOLDER%\bin\*.pdb
del %WEBSITE_FOLDER%\App_Data\*.log
del %WEBSITE_FOLDER%\App_Data\*.txt
del %WEBSITE_FOLDER%\App_Data\*.mdf
del %WEBSITE_FOLDER%\App_Data\*.ldf

del %PACKAGE_FILE%
cd %PACKAGE_FOLDER%
%ZIP% a -tzip -r %PACKAGE_FILE% *.* 
move %PACKAGE_FILE% ..\%PACKAGE_FILE%
cd ..
fciv.exe -sha1 %PACKAGE_FILE% > sha1.txt
rd /s /q %WEBSITE_FOLDER%
REM "c:\Program Files (x86)\SIR\SIRCommandLine.exe" -s:d:\work\dropthings\GoogleCode\%PACKAGE_FILE%