@echo off
echo ====================================
echo OSA Web Server Launcher
echo ====================================
echo.
echo Starting OSA Web API on http://192.168.1.90:5000
echo.
echo Press Ctrl+C to stop the server
echo.
echo ====================================
echo.

cd /d "C:\Users\legor\source\repos\OSA File Management System\OSAWebAPI"

dotnet run --urls "http://192.168.1.90:5000"

pause
