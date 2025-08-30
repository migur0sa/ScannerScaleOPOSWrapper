# ⚖️ Scanner/Scale OPOS Named Pipe Implementation

A lightweight C# program that interfaces with scanners and scanner/scale devices using OPOS. This project exposes **live weight data** and **scanned barcode values** via **named pipes**, enabling seamless integration with external applications (e.g., Python, Node.js, etc.).

---

## 🚀 Features

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

## 🛠 Requirements to build

- Windows OS
- .NET Framework 4.7.2+
- OPOS Common Control Objects (CCO) v1.14
  - Download from [1.14.001 CCO Installer (Windows Installer MSI)](http://monroecs.com/oposccos_current.htm)

---

## 📓 Notes:

The Windows installer can be found under the "releases". The application must be "Run as Administrator" to work properly. Future versions will run as a Windows Service. Currently, since there might be bugs, it will run as a console program. If running it as a service is necessary, it can be run using NSSM or a similar tool to function as a service.

---
