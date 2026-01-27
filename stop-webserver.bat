@echo off
echo ====================================
echo Stop OSA Web Server
echo ====================================
echo.

taskkill /F /IM dotnet.exe /FI "WINDOWTITLE eq OSAWebAPI*" >nul 2>&1

if %ERRORLEVEL% EQU 0 (
    echo Web server stopped successfully
) else (
    echo No web server running or already stopped
)

echo.
echo ====================================
timeout /t 2 >nul
