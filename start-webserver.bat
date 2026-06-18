@echo off
echo ====================================
echo OSA Web Server Launcher
echo ====================================
echo.
echo Starting OSA Web API on http://192.168.1.90:5000
echo Starting Visitor Log API on http://192.168.1.90:5002
echo.
echo Press Ctrl+C to stop both servers
echo.
echo ====================================
echo.

cd /d "C:\Users\legor\source\repos\OSA File Management System\OSAWebAPI"
start /b dotnet run

cd /d "C:\Users\legor\source\repos\OSA File Management System\VisitorLogAPI"
start /b dotnet run

pause
