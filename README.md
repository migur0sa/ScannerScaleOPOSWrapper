# 🧭 Zebra Scanner/Scale OPOS Named Pipe Program

A lightweight C# program that interfaces with Zebra scanner and scale devices using the OPOS standard. This project exposes **live weight data** and **scanned barcode values** via **named pipes**, enabling seamless integration with external applications (e.g., Python, Node.js, etc.).

It can work with other scanner/scales from other manufacturers, but it has only been tested with Zebra devices for now.

---

## 🚀 Features

- ✅ Connects to Zebra scanner and scale via OPOS (Zebra MP7001 tested)
- ✅ Streams live weight updates and barcode scans
- ✅ Communicates through named pipes for cross-language interoperability
- ✅ Graceful device initialization and cleanup
- ✅ Easy to extend for other OPOS-compatible devices

---

## 🛠 Requirements

- **Windows OS**
- [.NET Framework 4.7.2+]
- **OPOS Common Control Objects (CCO) v1.14**
  - Download from [1.14.001 CCO Installer (Windows Installer MSI)](http://monroecs.com/oposccos_current.htm)
- Zebra scanner/scale with OPOS drivers installed
---
