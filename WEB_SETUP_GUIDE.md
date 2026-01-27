# OSA Web Server Setup Guide

## Quick Start

### To Start the Web Server:

1. **Double-click** `start-webserver.bat`
2. Wait for message: "Now listening on: http://172.16.42.118:5000"
3. **Keep this window open** (minimize it)
4. Share URL with coworkers: `http://172.16.42.118:5000`

### To Stop the Web Server:

1. **Double-click** `stop-webserver.bat`
2. Or press `Ctrl+C` in the server window

---

## First-Time Setup

### Step 1: Configure Firewall (One Time Only)

Run **Setup Firewall.bat** (or run this in PowerShell as Administrator):

```powershell
netsh advfirewall firewall add rule name="OSA Web Server 5000" dir=in action=allow protocol=TCP localport=5000 profile=private
```

### Step 2: Verify MySQL Service

Ensure MySQL is running:
- Press `Win + R`
- Type: `services.msc`
- Find "MySQL80" or similar
- Status should be "Running"
- If not, right-click → Start

### Step 3: Test Web Server

1. Run `start-webserver.bat`
2. Open browser on YOUR computer: `http://172.16.42.118:5000`
3. You should see the OSA Management System home page
4. **Success!** Now share URL with coworkers

---

## Accessing from Coworker Devices

### From Windows PC/Mac/Linux:
- Open any web browser
- Go to: `http://172.16.42.118:5000`

### From Mobile/Tablet:
- Open browser on device
- Go to: `http://172.16.42.118:5000`
- Make sure connected to same WiFi

### What Coworkers Can Do:
- ✅ View all Region Communication records
- ✅ Search and filter data
- ✅ View detailed record information
- ✅ Open PDF documents
- ✅ View statistics dashboard
- ✅ Print documents

### What Coworkers Cannot Do:
- ❌ Add new records (read-only)
- ❌ Edit existing records
- ❌ Delete records
- ❌ Need any software installation

---

## Troubleshooting

### Problem: "This site can't be reached"

**Solution:**
1. Check if `start-webserver.bat` is running
2. Verify firewall rule exists:
   ```cmd
   netsh advfirewall firewall show rule name="OSA Web Server 5000"
   ```
3. Ensure both devices on same network

### Problem: "Connection refused"

**Solution:**
1. Stop current server: run `stop-webserver.bat`
2. Wait 5 seconds
3. Start again: run `start-webserver.bat`

### Problem: Works on your PC but not coworker's

**Solution:**
1. Check coworker's network connection
2. Test ping from coworker's machine:
   ```cmd
   ping 172.16.42.118
   ```
3. If ping fails, check router settings

### Problem: Server stops when you close laptop

**Solution:**
- This is normal behavior
- Laptop must stay on and connected for web access
- Consider using dedicated server if 24/7 access needed

### Problem: Need to restart server every time

**Solution:**
- Create shortcut to `start-webserver.bat` in your Startup folder:
  1. Press `Win + R`, type: `shell:startup`
  2. Copy `start-webserver.bat` there
  - Server will auto-start on login (not recommended - keeps window open)

---

## Advanced: Run as Windows Service (Optional)

For automatic startup without window:

### Prerequisites:
- Download NSSM (Non-Sucking Service Manager)
- Run as Administrator

### Install as Service:
```cmd
nssm install OSAWebAPI "C:\Program Files\dotnet\dotnet.exe"
nssm set OSAWebAPI AppDirectory "C:\Users\legor\source\repos\OSA File Management System\OSAWebAPI"
nssm set OSAWebAPI Parameters "run --urls \"http://172.16.42.118:5000\""
nssm set OSAWebAPI Start SERVICE_AUTO_START
nssm start OSAWebAPI
```

### Remove Service:
```cmd
nssm stop OSAWebAPI
nssm remove OSAWebAPI confirm
```

---

## Daily Usage Workflow

### Morning Setup:
1. Turn on laptop
2. Connect to WiFi
3. Double-click `start-webserver.bat`
4. Minimize the window
5. Tell coworkers: "Web server is ready at http://172.16.42.118:5000"

### End of Day:
1. Double-click `stop-webserver.bat`
2. Or close the server window
3. Laptop can be turned off

### If You Restart Laptop:
1. Re-run `start-webserver.bat`
2. Share updated URL with coworkers (same URL)

---

## Network Information

### Your Static IP: `172.16.42.118`
### Web Server Port: `5000`
### Full URL: `http://172.16.42.118:5000`

### Database:
- MySQL Server: `172.16.42.118:3306`
- Database: `osasystem`
- User: `osa_network`

---

## File Locations

| File | Location |
|------|----------|
| Web Server Start | `OSA File Management System\start-webserver.bat` |
| Web Server Stop | `OSA File Management System\stop-webserver.bat` |
| Setup Firewall | `OSA File Management System\Setup Firewall.bat` |
| Web App Code | `OSA File Management System\OSAWebAPI\` |
| Desktop App Code | `OSA File Management System\OSA File Management System\` |

---

## Notes

- **No authentication required** - Anyone on your network can access
- **Read-only access** - Browsers can only view data
- **Real-time sync** - Changes in desktop app appear in browser (after page reload)
- **Multiple users** - Unlimited coworkers can access simultaneously
- **No browser limit** - Works on Chrome, Firefox, Safari, Edge, etc.
- **Cross-platform** - Windows, Mac, Linux, iOS, Android all work

---

## Support

For issues or questions:
1. Check this guide's troubleshooting section
2. Ensure all prerequisites are met
3. Verify network connectivity
4. Check MySQL and web server status

---

**Last Updated:** January 27, 2026
**Version:** 1.0
