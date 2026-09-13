# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Wer.Winforms is a custom WinForms UI toolkit library with a demo application. It provides owner-drawn controls with a consistent theme (rounded corners, hover/pressed states, custom colors). Targets **.NET Framework 4.7.2** (not .NET Core/5+).

## Solution Structure

- **Wer.Winforms.Toolkit** — Class library containing all custom controls and theming
  - `WerTheme.cs` — Static theme class (colors, fonts, sizes, shared layout constants). All controls read from this.
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
- `WerButton` is the base button; color variants extend it with pre-set colors from `WerTheme`
- Controls use `[Category("Wer...")]` attributes to group properties in the VS Properties panel
- `[ToolboxItem(false)]` on base controls prevents them from appearing directly in the VS Toolbox
- All controls use `ControlStyles.OptimizedDoubleBuffer` for flicker-free rendering

## Theme System (`WerTheme.cs`)

All controls reference `WerTheme` for consistent styling. When creating new controls, use these instead of hardcoding colors/values.

### Brand Colors
| Property | Value | Usage |
|----------|-------|-------|
| `PrimaryColor` | `RGB(12, 124, 146)` — Teal | Buttons, focus borders, accents, edit buttons |
| `WarningColor` | `RGB(179, 58, 58)` — Red | Warning buttons, error states |
| `SuccessColor` | `RGB(40, 167, 69)` — Green | Success buttons |
| `OrangeColor` | `RGB(230, 126, 34)` — Orange | Orange buttons |
| `TextColor` | `RGB(33, 37, 41)` — Dark navy | All body text |
| `MutedColor` | `RGB(130, 130, 130)` | Secondary text |
| `DisabledColor` | `RGB(180, 180, 180)` | Disabled text |

### Shared Control Colors (new controls should use these)
| Property | Value | Usage |
|----------|-------|-------|
| `InputBorder` | `RGB(200, 210, 220)` | Normal input border |
| `InputBorderFocus` | `RGB(12, 124, 146)` | Focused input border (same as PrimaryColor) |
| `InputBg` | `White` | Input background |
| `InputBgDisabled` | `RGB(242, 242, 242)` | Disabled/read-only background |
| `PlaceholderColor` | `RGB(160, 170, 180)` | Placeholder text |
| `LabelColor` | `Black` | Normal label text |
| `LabelReadOnlyColor` | `RGB(200, 100, 20)` | Read-only label |
| `LabelDisabledColor` | `RGB(150, 150, 150)` | Disabled label |
| `RequiredStarColor` | `RGB(210, 50, 50)` | Required asterisk |
| `IconColor` | `RGB(140, 150, 160)` | Chevrons, icons |

### Shared Layout Constants
| Constant | Value | Usage |
|----------|-------|-------|
| `LabelHeight` | `20px` | Label height above input |
| `LabelGap` | `4px` | Gap between label and input |
| `InputBorderRadius` | `8px` | Rounded corners on all inputs |
| `InputPadH` | `10px` | Horizontal padding inside inputs |

### Fonts
| Property | Font | Usage |
|----------|------|-------|
| `HeadingFont` | Segoe UI 14pt Bold | H1 headings |
| `SubheadingFont` | Segoe UI 11pt Bold | H2/H3 headings |
| `BodyFont` | Segoe UI 9.75pt Regular | All body text, inputs, labels |
| `CaptionFont` | Segoe UI 8.25pt Regular | Small captions |
| `ButtonFont` | Segoe UI 9.75pt Regular | Button text |

### Visual Pattern (all form controls follow this)
- Label above input (black, 9.75pt)
- Required: red asterisk after label
- Read-only: orange label, gray background
- Disabled: gray label, gray background
- Input: white bg, rounded 8px border, `RGB(200, 210, 220)` border
- Focus: border changes to teal `RGB(12, 124, 146)`, 1.5px width
- Placeholder: gray `RGB(160, 170, 180)` text when empty
- Chevron: `▾` or `▼` for dropdowns, teal on focus
- Default control height: `LabelHeight + LabelGap + 36 = 60px`

## WerDataGrid

The data grid (`Controls/WerDataGrid.cs`) is a fully owner-drawn `UserControl` — **not** a `DataGridView` subclass.

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
- "No records found." in red when empty/no search results

### Supporting files
- `WerDataGridColumn.cs` — Column definition (PropertyName, HeaderText, Width, Alignment)
- `WerDataGridEditEventArgs.cs` — Event args (PrimaryKey, RowIndex)

## Numeric Control Value Types

All numeric controls return their native type with default 0 — no parsing needed:

```csharp
decimal amount = werCurrencyField1.Value;  // decimal, default 0
int qty        = werIntegerField1.Value;   // int, default 0
decimal pct    = werPercentField1.Value;   // decimal, default 0
```

- Placeholder ("0.00" / "0") shown as gray text when empty (TextBox hidden, placeholder painted)
- Click to activate TextBox, blur empty → placeholder reappears
- **Paste protection:** Ctrl+V blocked for non-numeric clipboard content on all three controls

## .NET Framework 4.7.2 Gotchas

- `FontStyle.SemiBold` does not exist — use `FontStyle.Bold`
- `TextBox.PlaceholderText` does not exist — use Win32 `EM_SETCUEBANNER` (0x1501) via P/Invoke, or hide TextBox and paint placeholder
- `DataGridView.DoubleBuffered` is protected — set via reflection
- Old-style `.csproj` files — manually list all `<Compile>` entries with `<SubType>`
- Designer serialization: VS designer serializes property values into `.Designer.cs` that override constructor settings. For complex controls, create in code (not designer) to maintain full control.
- Keep model/POCO classes in separate `.cs` files — VS designer cannot parse Form files that contain non-Form classes
- When changing `Value` property from nullable to non-nullable, fix designer `.Designer.cs` entries (e.g. `.Value = null` → `.Value = 0`)

## Controls Catalog

### Buttons
`WerButton` (base), `WerButtonPrimary`, `WerButtonWarning`, `WerButtonSuccess`, `WerButtonOrange`, `WerButtonOutlinedPrimary`, `WerButtonOutlinedWarning`, `WerButtonOutlinedSuccess`, `WerButtonOutlinedOrange`

### Form Fields
`WerTextField`, `WerCurrencyField`, `WerIntegerField`, `WerPercentField`, `WerDatePicker`, `WerTimePicker`, `WerPhoneField`, `WerTelephoneField`, `WerSearchField`, `WerComboBox`, `WerRichTextField`, `WerCopyTextField`

### Selectors
`WerDateRange` — Predefined date range dropdown (Past 7/30/60/90 Days, 6 Months, 1 Year). Access via `Result.StartDate` / `Result.EndDate`

### Display
`WerLabel`, `WerHeading`, `WerLink`, `WerCheckbox`, `WerRadioButton`, `WerToggle`, `WerStatusIndicator`, `WerDivider`, `WerBanner` (Info/Positive/Negative/Notice variants)

### Layout
`WerGroupBox`, `WerEmptyGroupBox`, `WerTabControl`, `WerTabPage`

### Data
`WerDataGrid` (owner-drawn data grid with search, pagination, edit buttons)
