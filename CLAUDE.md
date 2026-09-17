# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Rules

**Every code change MUST follow these rules:**

1. **Best approach first** — Before implementing, evaluate alternatives. Choose the simplest, most performant, and most maintainable approach. Don't over-engineer.
2. **Memory leak check** — Every control, form, event handler, cached object, and GDI+ resource must be reviewed for leaks:
   - Dispose GDI+ objects (`Font`, `Brush`, `Pen`, `GraphicsPath`) — use `using` blocks
   - Unwire event handlers in `OnControlRemoved` / `Dispose`
   - Cached forms must be disposed when parent disposes
   - Don't create new Font/Brush objects in `OnPaint` without `using`
   - `Controls.Clear()` before adding new embedded forms
3. **No duplicate instances** — Forms, timers, event subscriptions must not stack on repeated calls
4. **Dispose pattern** — Override `Dispose(bool)` when control holds cached forms, timers, or unmanaged resources
5. **Test visually** — Build and verify in the demo app before marking done

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
| `LabelColor` | `Black` | Normal label text (also read-only) |
| `LabelDisabledColor` | `RGB(150, 150, 150)` | Disabled label |
| `RequiredStarColor` | `RGB(210, 50, 50)` | Required asterisk |
| `IconColor` | `RGB(140, 150, 160)` | Chevrons, icons |

### Shared Layout Constants
| Constant | Value | Usage |
|----------|-------|-------|
| `LabelHeight` | dynamic (`Font.GetHeight() + 4`, min 20px) | Label height above input — scales with font |
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
- Label above input (black, 9.75pt) — `ShowLabel` property to hide (default true)
- Required: red asterisk after label
- Read-only: **black label** (not orange), gray background, no editing
- Disabled: gray label, gray background
- Input: white bg, rounded 8px border, `RGB(200, 210, 220)` border
- Focus: border changes to teal `RGB(12, 124, 146)`, 1.5px width
- Placeholder: gray `RGB(160, 170, 180)` text when empty
- Chevron: `▾` or `▼` for dropdowns, teal on focus
- Default control size: `350 × 60px`
- Font property scales label height, prefix width, and inner TextBox automatically

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
| `ShowFooterPagination` | `bool` | `true` | `false` = render all rows at once, no footer (POS / full-list mode) |
| `AllowSorting` | `bool` | `true` | Click header to sort |
| `ShowHorizontalScroll` | `bool` | `false` | Horizontal scrollbar when columns exceed width |
| `TabOptions` | `string[]` | `null` | Filter tabs above grid (via Properties collection editor) |
| `TabSelected` | `int` | `0` | Selected tab index |
| `DataSource` | `object` | `null` | Accepts `List<T>`, `DataTable`, `IEnumerable` |
| `SelectedRowId` | `object` | — | Read-only. Returns PrimaryKeyColumn value of selected row |

### Events
- `EditClicked` → `WerDataGridEditEventArgs { PrimaryKey, RowIndex }`
- `SelectedRowChanged` → `EventHandler<object>` — fires on keyboard or mouse row change; arg is the PK value
- `TabChanged` → tab selection changed

### Features
- Auto-fit column widths from content measurement
- Column resize by dragging header borders
- Search field (WerSearchField) above the grid — filters across all visible columns
- Pagination footer with "1 – 25 of N", nav buttons, optional total amount
- Auto-formatting: `decimal`/`double`/`float` → `#,##0.00` (right-aligned), `DateTime` → `MMM. dd, yyyy`
- Rounded container, rounded selection highlight, subtle row separators
- "No Records Found" in light gray italic when empty/no search results
- Tab filter options in rounded group box above grid
- Vertical scroll when page rows exceed visible area

### Supporting files
- `WerDataGridColumn.cs` — Column definition (PropertyName, HeaderText, Width, Alignment)
- `WerDataGridEditEventArgs.cs` — Event args (PrimaryKey, RowIndex)

## Navigation System

### WerLeftNavMenu
Left navigation panel (`Panel` subclass, auto Dock=Left). Holds `WerMenuButton` controls. Manages form hosting in a content panel.

```csharp
// Setup
werLeftNavMenu1.ContentPanel = panel1;  // panel with Dock=Fill
werLeftNavMenu1.LogoText = "MyApp";

// In each button's Click handler:
werLeftNavMenu1.ShowForm<UsersForm>();
```

**Properties:** `ContentPanel`, `LogoText`, `LogoHeight` (default 60), `NavWidth`  
**Event:** `NavigationChanged`  
**Form lifecycle:** Forms cached by Type — created once, reused. All disposed on control Dispose(). No memory leaks.

### WerMenuButton
Navigation button control — drag into `WerLeftNavMenu`. Double-click in designer to add Click handler. Auto-docks top. Shows active state (teal highlight + left indicator bar).

### WerTopNav
Top navigation bar (Dock=Top, 48px). Shows avatar circle with user initials, username, logout link.

```csharp
werTopNav1.UserName = "Jim Cruz";  // avatar shows "JC"
werTopNav1.PageTitle = "Dashboard";
werTopNav1.LogoutClicked += (s, e) => Application.Exit();
```

### Layout pattern (dock order matters)
```csharp
// Add order in Controls: Fill first, Top second, Left last
this.Controls.Add(panel1);           // Dock=Fill (content)
this.Controls.Add(werTopNav1);       // Dock=Top
this.Controls.Add(werLeftNavMenu1);  // Dock=Left (full height)
```

## WerForm
Base form class with theme defaults. Inherit instead of `Form` for consistent styling.

```csharp
public partial class UsersForm : WerForm { ... }
```

**Defaults:** White bg, Segoe UI 9.75pt, 16px padding, double-buffered, no icon, no minimize/maximize (configurable via Properties).

## WerMessageBox
Modern styled message box with dimmed overlay.

```csharp
WerMessageBox.Success.Show("Record saved.");
WerMessageBox.Error.Show("Something went wrong.", "Oops");
WerMessageBox.Warning.Show("Continue?", "Confirm", WerMessageButtons.OKCancel);
WerMessageBox.Info.Show("Version 1.0");
```

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
- `TextBox.PlaceholderText` does not exist — hide TextBox and paint placeholder in OnBorderPaint
- `DataGridView.DoubleBuffered` is protected — set via reflection
- Old-style `.csproj` files — manually list all `<Compile>` entries with `<SubType>`
- Designer serialization: VS designer serializes property values into `.Designer.cs` that override constructor settings. For complex controls, create in code (not designer) to maintain full control.
- Keep model/POCO classes in separate `.cs` files — VS designer cannot parse Form files that contain non-Form classes
- When changing `Value` property from nullable to non-nullable, fix designer `.Designer.cs` entries (e.g. `.Value = null` → `.Value = 0`)
- Dock order in `Controls.Add()` matters — Fill first, then Top, then Left for correct layout
- `PrefixWidth` on currency/phone/telephone fields uses `TextRenderer.MeasureText` for font scaling

## Controls Catalog

### Buttons
`WerButton` (base), `WerButtonPrimary`, `WerButtonWarning`, `WerButtonSuccess`, `WerButtonOrange`, `WerButtonOutlinedPrimary`, `WerButtonOutlinedWarning`, `WerButtonOutlinedSuccess`, `WerButtonOutlinedOrange`

### Form Fields
`WerTextField`, `WerCurrencyField`, `WerIntegerField`, `WerPercentField`, `WerDatePicker`, `WerTimePicker`, `WerPhoneField`, `WerTelephoneField`, `WerSearchField`, `WerComboBox`, `WerRichTextField`, `WerCopyTextField`

All form fields share: `LabelText`, `Required`, `ReadOnly`, `ShowLabel` (default true), `Font` scaling

### Selectors
`WerDateRange` — Predefined date range dropdown (Past 7/30/60/90 Days, 6 Months, 1 Year). Access via `Result.StartDate` / `Result.EndDate`
`WerDateRangePicker` — Two calendar pickers (From — To) with label. Access via `StartDate` / `EndDate`
`WerQuantitySelector` — `[ − | value | + ]` inline stepper. Properties: `Value`, `Minimum`, `Maximum`, `Step`. Event: `ValueChanged`

### Display
`WerLabel`, `WerHeading`, `WerLink`, `WerCheckbox`, `WerRadioButton`, `WerToggle`, `WerStatusIndicator`, `WerDivider`, `WerBanner` (Info/Positive/Negative/Notice variants), `WerBadge`, `WerIcon`

### Layout
`WerGroupBox`, `WerEmptyGroupBox`, `WerTabControl`, `WerTabPage`

### Navigation
`WerLeftNavMenu` (left nav panel), `WerMenuButton` (nav item), `WerTopNav` (top bar with avatar color by initial)

### Data
`WerDataGrid` (owner-drawn data grid with search, pagination, tabs, edit buttons, add button, horizontal scroll)

### Feedback
`WerSnackbar` (auto-dismiss toast: Success/Error/Warning/Info), `WerProgressBar` (rounded bar with label), `WerSpinner` (animated loading arc)

### Dashboard
`WerCard` — Stat card with title, value, subtitle, colored accent bar

### Dialogs
`WerMessageBox` (modern message box: Success/Error/Warning/Info), `WerForm` (themed base form)

### Quantity Selector
`WerQuantitySelector` — compact `[ − | 0 | + ]` stepper. Default size 140×60px. Keyboard: ←/→ or ↑/↓.

## New Controls API

### WerSnackbar
```csharp
WerSnackbar.Success(this, "Record saved!");
WerSnackbar.Error(this, "Failed to connect.");
WerSnackbar.Warning(this, "Unsaved changes.");
WerSnackbar.Info(this, "3 new messages.");
```
Auto-dismiss after 3s. Stacks vertically at bottom-center. No instance needed.

### WerCard
```csharp
werCard1.Title = "Total Revenue";
werCard1.Value = "$125,000.00";
werCard1.Subtitle = "Last 30 days";
werCard1.AccentColor = WerTheme.SuccessColor;
```

### WerProgressBar
```csharp
werProgressBar1.Value = 75;         // 0-100
werProgressBar1.BarColor = WerTheme.PrimaryColor;
werProgressBar1.ShowLabel = true;   // shows "75%"
```

### WerQuantitySelector
```csharp
werQuantitySelector1.LabelText = "Qty";
werQuantitySelector1.Minimum   = 0;
werQuantitySelector1.Maximum   = 99;
werQuantitySelector1.Step      = 1;
werQuantitySelector1.Value     = 1;

werQuantitySelector1.ValueChanged += (s, e) =>
    lblQty.Text = werQuantitySelector1.Value.ToString();
```
- `−` dims when at `Minimum`, `+` dims when at `Maximum`
- Keyboard: `←` / `↓` decrement · `→` / `↑` increment
- Supports `ReadOnly`, `Required`, `ShowLabel`

### WerDataGrid — Keyboard & POS mode
```csharp
// Row selection via keyboard
werDataGrid1.SelectedRowChanged += (s, id) =>
    LoadDetail(id);                       // id is the PrimaryKeyColumn value

// POS / full-list mode (shows all rows, no pagination footer)
werDataGrid1.ShowFooterPagination = false;

// Async load pattern (safe — await before setting DataSource)
var orders = await _service.GetOrders();
werDataGrid1.DataSource = orders;
```

### WerSpinner
```csharp
werSpinner1.IsSpinning = true;
werSpinner1.SpinnerColor = WerTheme.PrimaryColor;
```

### WerBadge
```csharp
werBadge1.Text = "Active";
werBadge1.BadgeColor = WerTheme.SuccessColor;
```
Auto-sizes to text. Pill shape with tinted background.

### WerDateRangePicker
```csharp
werDateRangePicker1.LabelText = "Period";
DateTime? from = werDateRangePicker1.StartDate;
DateTime? to = werDateRangePicker1.EndDate;
werDateRangePicker1.RangeChanged += (s, e) => Reload();
```

### WerIcon + WerIcons
```csharp
werIcon1.IconCode = WerIcons.Person;   // standalone icon
werIcon1.ForeColor = WerTheme.PrimaryColor;

// On buttons:
btnAdd.IconCode = WerIcons.Add;        // icon + text on any WerButton
btnAdd.Text = "Add User";
```
Uses Segoe MDL2 Assets (built into Windows 10/11). 70+ named constants in `WerIcons`.
