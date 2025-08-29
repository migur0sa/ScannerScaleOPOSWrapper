# 🧭 Zebra Scanner/Scale OPOS Named Pipe Wrapper

A lightweight C# program that interfaces with Zebra MP7000 scanner and scale devices using the OPOS standard. This project exposes **live weight data** and **scanned barcode values** via **named pipes**, enabling seamless integration with external applications (e.g., Python, Node.js, etc.).

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
- [.NET Framework 4.7.2+](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
- **OPOS Common Control Objects (CCO) v1.14**
  - Download from [UnifiedPOS CCO](https://monroecs.com/oposccos.htm)
- Zebra MP7000 scanner/scale with OPOS drivers installed
- Administrator privileges (required for device claiming)

---
