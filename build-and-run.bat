@echo off
echo ======================================
echo Building Daily Operations Dashboard
echo ======================================
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo Please install .NET SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo Building project...
dotnet build

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Build failed! Please check errors above.
    pause
    exit /b 1
)

echo.
echo ======================================
echo Build successful! Running program...
echo ======================================
echo.

dotnet run

echo.
echo ======================================
echo Program finished
echo ======================================
pause

