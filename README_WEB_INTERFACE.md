# OSA Management System - Web Interface

## Quick Start

### For Administrator (You):

1. **First Time Setup** (Run once):
   - Right-click `Setup Firewall.bat` → "Run as administrator"
   
2. **Daily Usage**:
   - Double-click `start-webserver.bat`
   - Minimize the window
   - Share URL with coworkers: `http://172.16.42.118:5000`

3. **To Stop**:
   - Double-click `stop-webserver.bat`
   - Or press Ctrl+C in server window

### For Coworkers:

**Just open this URL in any browser:**
```
http://172.16.42.118:5000
```

That's it! No installation needed.

---

## What You Created

| File | Purpose |
|------|---------|
| `start-webserver.bat` | Start web server (double-click this) |
| `stop-webserver.bat` | Stop web server (double-click this) |
| `Setup Firewall.bat` | Configure firewall (run once as admin) |
| `WEB_SETUP_GUIDE.md` | Complete setup and troubleshooting guide |
| `COWORKER_GUIDE.md` | Guide for coworkers (share with them) |
| `OSAWebAPI/` | Web application code |

---

## System Overview

You now have **TWO** ways to access your data:

### 1. Desktop Application (WPF)
- **File:** `OSA File Management System.exe`
- **Use for:** Adding, editing, deleting records
- **Access:** Only from your laptop (with MySQL installed)
- **Features:** Full CRUD, printing, advanced features

### 2. Web Browser Interface (NEW!)
- **URL:** `http://172.16.42.118:5000`
- **Use for:** Viewing, searching, dashboards
- **Access:** Any device on same network (Windows, Mac, Mobile)
- **Features:** Read-only view, filtering, statistics, PDF viewing

Both apps use the **same MySQL database** - data syncs automatically!

---

## Daily Workflow

### Morning:
1. Turn on laptop
2. Connect to WiFi
3. **Run `start-webserver.bat`** (keeps window open)
4. Tell coworkers: "Web access available at http://172.16.42.118:5000"

### During Day:
- Use desktop app to add/edit records
- Coworkers use web browser to view/search
- No manual sync needed - same database!

### Evening:
- Run `stop-webserver.bat` (optional)
- Turn off laptop (if desired)

---

## File Organization

```
OSA File Management System/
├── OSA File Management System/          # Desktop app (WPF)
│   ├── Model/
│   ├── ViewModel/
│   ├── View/
│   └── OSA File Management System.exe
│
├── OSAWebAPI/                          # Web app (NEW!)
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Views/
│   └── wwwroot/
│
├── start-webserver.bat                   # START web server
├── stop-webserver.bat                    # STOP web server
├── Setup Firewall.bat                    # Configure firewall
├── WEB_SETUP_GUIDE.md                   # Full documentation
├── COWORKER_GUIDE.md                   # For coworkers
└── README_WEB_INTERFACE.md               # This file
```

---

## Requirements

### Your Laptop (Administrator):
- ✅ Static IP: `172.16.42.118` (already configured)
- ✅ MySQL Server running (already configured)
- ✅ MySQL user `osa_network` created (already configured)
- ✅ Firewall port 5000 open (run `Setup Firewall.bat`)
- ✅ Web server running (`start-webserver.bat`)

### Coworker Devices:
- ✅ Any web browser
- ✅ Connected to same WiFi/network
- ✅ Nothing else needed!

---

## Troubleshooting Quick Reference

| Problem | Quick Fix |
|---------|------------|
| Can't access from coworker PC | Check `start-webserver.bat` is running |
| Firewall error | Run `Setup Firewall.bat` as admin |
| Can't connect | Verify both devices on same network |
| Server won't start | Check if MySQL service is running |
| Can't see data | Reload browser page |

**Full troubleshooting guide:** See `WEB_SETUP_GUIDE.md`

---

## Access URLs

| Service | URL | Port |
|---------|------|-------|
| **Web Application** | `http://172.16.42.118:5000` | 5000 |
| MySQL Database | `172.16.42.118` | 3306 |

---

## Data Flow

```
You (Desktop App) → MySQL Database ← Web Browser (Coworkers)
     ↑                              ↓
     │                          READ ONLY ACCESS
     │                              ↓
   ADD/EDIT                      VIEW/SEARCH/PRINT
```

**Key Point:** All data goes through MySQL database. Both apps read/write to same database.

---

## Features Comparison

| Feature | Desktop App | Web Browser |
|----------|--------------|--------------|
| Add Records | ✅ | ❌ |
| Edit Records | ✅ | ❌ |
| Delete Records | ✅ | ❌ |
| View Records | ✅ | ✅ |
| Search Records | ✅ | ✅ |
| Filter Records | ✅ | ✅ |
| Print Records | ✅ | ✅ |
| View PDFs | ✅ | ✅ |
| Statistics/Dashboard | ❌ | ✅ |
| Charts & Visuals | ❌ | ✅ |
| Mobile Access | ❌ | ✅ |
| Cross-Platform | ❌ (Windows only) | ✅ (Any OS) |
| Multi-User | ❌ (Single user) | ✅ (Unlimited) |

---

## Security Notes

- **No Authentication:** Web interface is open access for anyone on network
- **Same Network Only:** Requires devices on same WiFi to connect
- **Read-Only:** Web browsers cannot modify data
- **Firewall:** Only port 5000 is open (private network)
- **Database:** Secure MySQL connection with `osa_network` user

**If you need security later:**
- Add authentication (username/password)
- Implement HTTPS
- Restrict by IP address
- Add user roles and permissions

---

## Support Documentation

- **Setup & Admin Guide:** `WEB_SETUP_GUIDE.md`
- **Coworker Guide:** `COWORKER_GUIDE.md`
- **Desktop App Guide:** Existing documentation
- **AGENTS.md:** Developer guidelines

---

## Next Steps

### Immediate:
1. ✅ Run `Setup Firewall.bat` as administrator
2. ✅ Run `start-webserver.bat`
3. ✅ Test: Open `http://172.16.42.118:5000` in browser
4. ✅ Share URL with coworkers

### Optional (Future):
1. Add authentication for security
2. Enable HTTPS
3. Implement real-time updates with SignalR
4. Add more features to web interface
5. Deploy to dedicated server for 24/7 access

---

## Quick Commands Reference

```bash
# Start web server
cd "C:\Users\legor\source\repos\OSA File Management System\OSAWebAPI"
dotnet run --urls "http://172.16.42.118:5000"

# Or simply:
# Double-click: start-webserver.bat

# Stop web server
taskkill /F /IM dotnet.exe

# Or simply:
# Double-click: stop-webserver.bat

# Add firewall rule (as admin)
netsh advfirewall firewall add rule name="OSA Web Server 5000" dir=in action=allow protocol=TCP localport=5000 profile=private

# Check firewall rule
netsh advfirewall firewall show rule name="OSA Web Server 5000"

# Test connectivity
ping 172.16.42.118
```

---

## Version History

| Version | Date | Changes |
|---------|--------|---------|
| 1.0 | Jan 27, 2026 | Initial web interface release |
| | | RegionCom module browser access |
| | | Dashboard with charts |
| | | Search and filter functionality |
| | | PDF viewing support |

---

**Congratulations!** 🎉

Your OSA Management System now has a web-based interface that allows coworkers on any device to view and search your data!

For questions or issues, refer to the detailed guides or contact your system administrator.

---

**Last Updated:** January 27, 2026  
**Version:** 1.0
