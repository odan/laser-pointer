# Laser Pointer

A lightweight Windows tool that displays a **round, semi-transparent laser pointer** on your screen while holding a hotkey.
Perfect for presentations, screen recordings, online meetings, or highlighting something precisely.

<img src="pointer.png">

## Features

- Realistic laser pointer appearance with glow effect
- Highly transparent – easy to see through the circle
- Works on top of all applications (VS Code, Chrome, browsers, etc.)
- Lightweight background application
- System tray icon with exit option
- Easy to customize (size, color, transparency)

## Hotkey

**Right Ctrl Key** – Hold to show the laser pointer
Release to hide it immediately.

## Installation

### Requirements

- Windows 10 / 11
- .NET 6.0 or .NET 8.0 Desktop Runtime

### Simple Installation

1. Download the `.exe` file
2. Run `LaserPointer.exe`

### Build from Source

```
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

## How to Use

- Start the program
- Hold down the Right Ctrl key → Laser pointer appears
- Move your mouse to point at things
- Release the key → Pointer disappears


## License

MIT

