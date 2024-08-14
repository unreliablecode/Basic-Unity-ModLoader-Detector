# Basic Unity Anti-Mod Loader System

This Unity Anti-Cheat System is designed to detect common modding tools such as **MelonLoader** and **BepInEx**. It also checks for the presence of certain suspicious DLL files in the game directory, specifically `version.dll`, `winmm.dll`, and `winhttp.dll`. If any of these are detected, the game will automatically close.

## Features

- Detects the presence of **MelonLoader** and **BepInEx** directories.
- Checks for `version.dll`, `winmm.dll`, and `winhttp.dll` files in the game directory.
- Uses Object-Oriented Programming (OOP) principles for easy maintenance and extension.

## How It Works

The system is composed of a set of checks, each encapsulated in its own class, that look for specific files or directories. If any of these checks detect the presence of a cheat tool or suspicious file, the game will immediately exit.

### Directory Checks

- `MelonLoader` directory
- `BepInEx` directory

### File Checks

- `version.dll`
- `winmm.dll`
- `winhttp.dll`

## Implementation

This anti-cheat system is implemented as a Unity `MonoBehaviour` script.
