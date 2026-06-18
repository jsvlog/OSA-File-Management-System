@echo off
echo Fixing VisitorLogAPI firewall rules...
echo.
powershell -ExecutionPolicy Bypass -Command "Get-NetFirewallRule -Direction Inbound | Where-Object { ($_ | Get-NetFirewallApplicationFilter).Program -Like '*VisitorLogAPI*' } | Remove-NetFirewallRule"
powershell -ExecutionPolicy Bypass -Command "New-NetFirewallRule -DisplayName 'VisitorLogAPI Allow' -Direction Inbound -Program 'C:\users\legor\source\repos\osa file management system\visitorlogapi\bin\debug\net9.0\visitorlogapi.exe' -Action Allow -Profile Any"
echo.
echo Done! Press any key to exit.
pause
