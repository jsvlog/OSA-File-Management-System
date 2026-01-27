@echo off
echo ====================================
echo OSA Web Server - Firewall Setup
echo ====================================
echo.
echo This script will configure Windows Firewall to allow
echo web server traffic on port 5000.
echo.
echo Administrator privileges are required.
echo.

:: Check for administrator privileges
net session >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo ERROR: This script requires administrator privileges.
    echo.
    echo Right-click this file and select "Run as administrator"
    echo.
    pause
    exit /b 1
)

:: Add firewall rule
echo Adding firewall rule for port 5000...
netsh advfirewall firewall add rule name="OSA Web Server 5000" dir=in action=allow protocol=TCP localport=5000 profile=private

if %ERRORLEVEL% EQU 0 (
    echo.
    echo SUCCESS: Firewall rule added successfully!
    echo.
    echo Port 5000 is now open for private network access.
) else (
    echo.
    echo ERROR: Failed to add firewall rule.
    echo You may need to configure this manually.
)

echo.
echo ====================================
echo Setup Complete!
echo ====================================
echo.
echo You can now:
echo 1. Run 'start-webserver.bat' to start the web server
echo 2. Share URL http://172.16.42.118:5000 with coworkers
echo.
pause
