# ⚖️ Scanner/Scale OPOS Named Pipe Implementation

A lightweight C# program that interfaces with scanners and scanner/scale devices using OPOS. This project exposes **live weight data** and **scanned barcode values** via **named pipes**, enabling seamless integration with external applications (e.g., Python, Node.js, etc.).

---

## 🚀 Features:

- ✅ Connects to scanner or scanner/scales via OPOS (Zebra MP7001 tested)
- ✅ Streams live weight updates and barcode scans
- ✅ Communicates through named pipes for cross-language interoperability
- ✅ Graceful device initialization and cleanup
- ✅ Easy to extend for other OPOS-compatible devices
  
---

## 🧑‍💻 Sample Python Client App - Code:

```
import os

pipe_name = r'\\.\pipe\ScannerScaleOPOSPipe'

with open(pipe_name, 'r') as pipe:
    while True:
        line = pipe.readline().strip()
        if line:
            print({line})
```
---

## ⚙️ Scanner/Scale Settings and Cable Used For Testing:

**Model: Zebra MP7000**
- IBM Table Top USB (Powered USB Single Cable)
- Code 128 Enable
- Code 39 Enable
- GS1 Databar Omnidirectional Enable
- GS1 Databar Expanded Enable
- UPCA Transmit Check Digit Disable
- Convert UPCE to UPCA Enable
- Scale Pole Display Disable
- Transmit Symbol Code ID Enable
  
---

## ⛏️ Settings.ini:

There is a Settings.ini file, which controls the logical device name, scale enable, and debug mode. 

This file is located in the installation folder "C:\Program Files (x86)\Scanner Scale OPOS Wrapper".

```
[GENERAL]
DEBUG=1
SCANNER_NAME=ZEBRA_SCANNER
SCALE_NAME=ZEBRA_SCALE
SCALE_ENABLED=1

;DEBUG=1 enables (1) or disables (0) output of debug information to the console.
;ScannerName is the name of the scanner device to use.
;ScaleName is the name of the scale device to use.
;ScaleEnabled enables (1) or disables (0) the scale functionality.
;ScannerScaleOPOSPipe is the default name of the pipe used to communicate with the OPOS service.
```

## 🛠 Requirements to Build:

- Windows OS
- .NET Framework 4.7.2+
- OPOS Common Control Objects (CCO) v1.14
  - Download from [1.14.001 CCO Installer (Windows Installer MSI)](http://monroecs.com/oposccos_current.htm)

---

## ⬇️ Windows Installer:

[Download](https://github.com/migur0sa/ScannerScaleOPOSWrapper/releases)

**Notes:**
***The application must be "Run as Administrator" to work properly. Future versions will run as a Windows Service. Currently, since there might be bugs, it will run as a console program. If running it as a service is necessary, it can be run using NSSM or a similar tool to function as a service.***

---
