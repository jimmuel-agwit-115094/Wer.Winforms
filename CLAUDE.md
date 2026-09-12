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
  - `Product.cs` — Sample POCO class (keep model classes in separate files — VS designer fails when non-Form classes share a file with the Form)

## Build & Run

```bash
# Build entire solution (VS 2026 path)
"C:/Program Files/Microsoft Visual Studio/18/Professional/MSBuild/Current/Bin/MSBuild.exe" Wer.Winforms.sln -p:Configuration=Debug -verbosity:minimal

# Run demo app
./Wer.Winforms.Demo/bin/Debug/Wer.Winforms.Demo.exe
```

Or open `Wer.Winforms.sln` in Visual Studio and run the Demo project.

**Note:** When the demo is running or VS debugger is attached, the DLL is locked — close the app before rebuilding from CLI.

## Architecture

- Controls are **owner-drawn** — they override `OnPaint` with GDI+ rendering (`Graphics`, `GraphicsPath`, `SolidBrush`, etc.)
- Hover/pressed states tracked via `OnMouseEnter`/`OnMouseLeave`/`OnMouseDown`/`OnMouseUp` → `Invalidate()`
- `WerButton` is the base button; `WerButtonPrimary`, `WerButtonWarning`, `WerButtonSuccess`, `WerButtonOrange` are pre-configured color variants
- Outlined button variants: `WerButtonOutlinedPrimary`, `WerButtonOutlinedWarning`, `WerButtonOutlinedSuccess`, `WerButtonOutlinedOrange`
- Controls use `[Category("Wer...")]` attributes to group properties in the VS Properties panel
- `[ToolboxItem(false)]` on base controls prevents them from appearing directly in the VS Toolbox
- All controls use `ControlStyles.OptimizedDoubleBuffer` for flicker-free rendering

## WerDataGrid

The data grid (`Controls/WerDataGrid.cs`) is a fully owner-drawn `UserControl` — **not** a `DataGridView` subclass. This was a deliberate choice because DataGridView's internal painting pipeline fights against borderless styling at multiple levels.

### Key design decisions
- **Owner-drawn UserControl** with GDI+ — no DataGridView involved, zero cell border issues
- Converts any `IEnumerable<T>` to an internal `DataTable` via reflection for rendering
- Supports `DataTable` directly as well
- `InvalidateGrid()` — always use this instead of `Invalidate()` to avoid flickering the search box

### Simple API
```csharp
grid.FieldOptions = new[] { "Name", "Qty", "Price", "Total", "OrderDate" };
grid.PrimaryKeyColumn = "Id";
grid.ShowEditColumn = true;
grid.TotalAmountColumn = "Total";  // null = hidden
grid.PageSize = 25;                // default
grid.DataSource = products;        // List<T>, DataTable, IEnumerable
```

### Properties
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `FieldOptions` | `string[]` | `null` | Properties to display. `null` = all, `[]` = none |
| `PrimaryKeyColumn` | `string` | `null` | Property for Edit button's primary key (doesn't need to be visible) |
| `ShowEditColumn` | `bool` | `false` | Appends an Edit button column |
| `TotalAmountColumn` | `string` | `null` | Numeric property to sum in footer. String columns ignored |
| `PageSize` | `int` | `25` | Rows per page. Footer with pagination shown when rows > PageSize |
| `AllowSorting` | `bool` | `true` | Click header to sort |
| `DataSource` | `object` | `null` | Accepts `List<T>`, `DataTable`, `IEnumerable` |

### Events
- `EditClicked` → `WerDataGridEditEventArgs { PrimaryKey, RowIndex }`

### Features
- Auto-fit column widths from content measurement
- Column resize by dragging header borders
- Search field (WerSearchField) above the grid — filters across all visible columns
- Pagination footer with "1 – 25 of N", nav buttons, optional total amount
- Auto-formatting: `decimal`/`double`/`float` → `#,##0.00` (right-aligned), `DateTime` → `MMM. dd, yyyy`
- Rounded container, rounded selection highlight, subtle row separators

### Supporting files
- `WerDataGridColumn.cs` — Column definition (PropertyName, HeaderText, Width, Alignment)
- `WerDataGridEditEventArgs.cs` — Event args (PrimaryKey, RowIndex)

## .NET Framework 4.7.2 Gotchas

- `FontStyle.SemiBold` does not exist — use `FontStyle.Bold`
- `TextBox.PlaceholderText` does not exist — use Win32 `EM_SETCUEBANNER` (0x1501) via P/Invoke
- `DataGridView.DoubleBuffered` is protected — set via reflection: `typeof(DataGridView).InvokeMember("DoubleBuffered", ...)`
- Old-style `.csproj` files — manually list all `<Compile>` entries with `<SubType>`
- Designer serialization: VS designer serializes property values into `.Designer.cs` that override constructor settings. For complex controls, create in code (not designer) to maintain full control.
- Keep model/POCO classes in separate `.cs` files — VS designer cannot parse Form files that contain non-Form classes

## Controls Catalog

### Buttons
`WerButton` (base), `WerButtonPrimary`, `WerButtonWarning`, `WerButtonSuccess`, `WerButtonOrange`, `WerButtonOutlinedPrimary`, `WerButtonOutlinedWarning`, `WerButtonOutlinedSuccess`, `WerButtonOutlinedOrange`

### Form Fields
`WerTextField`, `WerCurrencyField`, `WerIntegerField`, `WerPercentField`, `WerDatePicker`, `WerPhoneField`, `WerTelephoneField`, `WerSearchField`, `WerComboBox`, `WerRichTextField`

### Display
`WerLabel`, `WerHeading`, `WerLink`, `WerCheckbox`, `WerRadioButton`, `WerStatusIndicator`, `WerDivider`, `WerBanner` (Info/Positive/Negative/Notice variants)

### Layout
`WerGroupBox`, `WerEmptyGroupBox`, `WerTabControl`, `WerTabPage`

### Data
`WerDataGrid` (owner-drawn data grid with search, pagination, edit buttons)
