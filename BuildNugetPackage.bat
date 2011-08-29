@ECHO OFF

IF EXIST %CD%\nuget\lib GOTO CONTINUE1

mkdir nuget\lib

:CONTINUE1

IF EXIST %CD%\nuget\content GOTO CONTINUE2

mkdir nuget\content

:CONTINUE2

copy /Y EPiUtilities\Package.nuspec nuget

copy /Y EPiUtilities\web.config.transform nuget\content

copy /Y EPiUtilities\bin\Release\EPiUtilities.dll nuget\lib
copy /Y EPiUtilities\bin\Release\EPiUtilities.XML nuget\lib

cd nuget

nuget pack Package.nuspec

cd ..