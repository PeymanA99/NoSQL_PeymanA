@echo --- This run.bat will succeed when handling files because it uses
@echo --- the same current working directory than Visual Studio

cd TestExec\bin\debug
TestExec pause
cd ..\..

pause
