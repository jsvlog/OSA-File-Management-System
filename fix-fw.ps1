Get-NetFirewallRule -Direction Inbound | Where-Object { ($_ | Get-NetFirewallApplicationFilter).Program -Like '*VisitorLogAPI*' } | Remove-NetFirewallRule
New-NetFirewallRule -DisplayName "VisitorLogAPI Allow" -Direction Inbound -Program 'C:\users\legor\source\repos\osa file management system\visitorlogapi\bin\debug\net9.0\visitorlogapi.exe' -Action Allow -Profile Any
