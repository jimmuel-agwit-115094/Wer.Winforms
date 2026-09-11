# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Wer.Winforms is a custom WinForms UI toolkit library with a demo application. It provides owner-drawn controls with a consistent theme (rounded corners, hover/pressed states, custom colors). Targets **.NET Framework 4.7.2** (not .NET Core/5+).

## Solution Structure

- **Wer.Winforms.Toolkit** — Class library containing all custom controls and theming
  - `WerTheme.cs` — Static theme class (colors, fonts, sizes). All controls read from this.
  - `Controls/` — Custom controls, all inheriting from WinForms base types (`Control`, `UserControl`, `ComboBox`, etc.)
  - `Design/` — VS designer support (e.g., `WerTabControlDesigner`)
- **Wer.Winforms.Demo** — WinExe app that references the toolkit for testing controls visually

## Build & Run

```bash
# Build entire solution
msbuild Wer.Winforms.sln

# Run demo app
msbuild Wer.Winforms.Demo/Wer.Winforms.Demo.csproj && ./Wer.Winforms.Demo/bin/Debug/Wer.Winforms.Demo.exe
```

Or open `Wer.Winforms.sln` in Visual Studio and run the Demo project.

## Architecture

- Controls are **owner-drawn** — they override `OnPaint` with GDI+ rendering (`Graphics`, `GraphicsPath`, `SolidBrush`, etc.)
- Hover/pressed states tracked via `OnMouseEnter`/`OnMouseLeave`/`OnMouseDown`/`OnMouseUp` → `Invalidate()`
- `WerButton` is the base button; `WerButtonPrimary` and `WerButtonWarning` are pre-configured variants that set colors from `WerTheme`
- Controls use `[Category("Wer...")]` attributes to group properties in the VS Properties panel
- `[ToolboxItem(false)]` on base controls prevents them from appearing directly in the VS Toolbox
- All controls use `ControlStyles.OptimizedDoubleBuffer` for flicker-free rendering
