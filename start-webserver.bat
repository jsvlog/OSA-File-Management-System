@echo off
echo ====================================
echo OSA Web Server Launcher
echo ====================================
echo.
echo Starting OSA Web API on http://172.16.42.118:5000
echo.
echo Press Ctrl+C to stop the server
echo.
echo ====================================
echo.

cd /d "C:\Users\legor\source\repos\OSA File Management System\OSAWebAPI"

dotnet run --urls "http://172.16.42.118:5000"

pause
