# Wer.Winforms.Toolkit — Usage Guide

A custom WinForms UI toolkit with owner-drawn controls, consistent theming, and modern styling. Targets **.NET Framework 4.7.2**.

---

## Table of Contents

- [Getting Started](#getting-started)
- [Theme System (WerTheme)](#theme-system-wertheme)
- [Buttons](#buttons)
- [Form Fields](#form-fields)
  - [WerTextField](#wertextfield)
  - [WerComboBox](#wercombobox)
  - [WerRichTextField](#werrichtextfield)
  - [WerCopyTextField](#wercopytextfield)
  - [WerCurrencyField](#wercurrencyfield)
  - [WerIntegerField](#werintegerfield)
  - [WerPercentField](#werpercentfield)
  - [WerPhoneField](#werphonefield)
  - [WerTelephoneField](#wertelephonefield)
  - [WerSearchField](#wersearchfield)
- [Date & Time Pickers](#date--time-pickers)
  - [WerDatePicker](#werdatepicker)
  - [WerTimePicker](#wertimepicker)
  - [WerDateRange](#werdaterange)
  - [WerDateRangePicker](#werdaterangepicker)
- [Selection Controls](#selection-controls)
  - [WerCheckbox](#wercheckbox)
  - [WerRadioButton](#werradiobutton)
  - [WerToggle](#wertoggle)
  - [WerQuantitySelector](#werquantityselector)
- [Display Controls](#display-controls)
  - [WerLabel](#werlabel)
  - [WerHeading](#werheading)
  - [WerLink](#werlink)
  - [WerIcon](#wericon)
  - [WerBadge](#werbadge)
  - [WerStatusIndicator](#werstatusindicator)
  - [WerDivider](#werdivider)
  - [WerBanner](#werbanner)
  - [WerCard](#wercard)
  - [WerProgressBar](#werprogressbar)
  - [WerSpinner](#werspinner)
- [Data Grid](#data-grid)
- [Navigation](#navigation)
  - [WerLeftNavMenu](#werleftnavmenu)
  - [WerMenuButton](#wermenubutton)
  - [WerTopNav](#wertopnav)
- [Layout & Containers](#layout--containers)
  - [WerGroupBox](#wergroupbox)
  - [WerEmptyGroupBox](#weremptygroupbox)
  - [WerTabControl](#wertabcontrol)
- [Dialogs & Feedback](#dialogs--feedback)
  - [WerMessageBox](#wermessagebox)
  - [WerSnackbar](#wersnackbar)
- [WerForm (Base Form)](#werform-base-form)
- [WerIcons Reference](#wericons-reference)

---

## Getting Started

1. Add a reference to `Wer.Winforms.Toolkit` in your WinForms project.
2. Build the solution — controls appear in the Visual Studio Toolbox under their categories.
3. Drag controls onto forms, or create them in code.

```csharp
using Wer.Winforms.Toolkit.Controls;
```

**Build command:**
```bash
MSBuild.exe Wer.Winforms.sln -p:Configuration=Debug -verbosity:minimal
```

---

## Theme System (WerTheme)

All controls read from `WerTheme` — a static class providing colors, fonts, and layout constants. Customize these at app startup to change the entire look.

### Brand Colors

| Property | Default | Usage |
|----------|---------|-------|
| `PrimaryColor` | `RGB(12, 124, 146)` — Teal | Buttons, focus borders, accents |
| `WarningColor` | `RGB(179, 58, 58)` — Red | Warning buttons, error states |
| `SuccessColor` | `RGB(40, 167, 69)` — Green | Success buttons |
| `OrangeColor` | `RGB(230, 126, 34)` — Orange | Orange buttons |
| `BlueColor` | `RGB(33, 150, 243)` — Blue | Blue variant buttons |
| `TextColor` | `RGB(33, 37, 41)` — Dark | All body text |
| `MutedColor` | `RGB(130, 130, 130)` | Secondary/caption text |
| `DisabledColor` | `RGB(180, 180, 180)` | Disabled text |

### Input Colors (readonly)

| Property | Default | Usage |
|----------|---------|-------|
| `InputBorder` | `RGB(200, 210, 220)` | Normal input border |
| `InputBorderFocus` | `RGB(12, 124, 146)` | Focused input border |
| `InputBg` | `White` | Input background |
| `InputBgDisabled` | `RGB(242, 242, 242)` | Disabled/read-only bg |
| `PlaceholderColor` | `RGB(160, 170, 180)` | Placeholder text |
| `LabelColor` | `Black` | Label text |
| `LabelDisabledColor` | `RGB(150, 150, 150)` | Disabled label |
| `RequiredStarColor` | `RGB(210, 50, 50)` | Red asterisk |
| `IconColor` | `RGB(140, 150, 160)` | Chevrons, icons |

### Fonts

| Property | Details |
|----------|---------|
| `HeadingFont` | Segoe UI 14pt Bold |
| `SubheadingFont` | Segoe UI 11pt Bold |
| `BodyFont` | Segoe UI 9.75pt Regular |
| `CaptionFont` | Segoe UI 8.25pt Regular |
| `ButtonFont` | Segoe UI 9.75pt Semibold (falls back to Bold) |

### Layout Constants

| Constant | Value |
|----------|-------|
| `LabelHeight` | Dynamic (`Font.GetHeight() + 4`, min 20px) |
| `LabelGap` | 4px |
| `InputBorderRadius` | 8px |
| `InputPadH` | 10px |

### Customizing at Startup

```csharp
// In Program.cs or Main Form constructor
WerTheme.PrimaryColor = Color.FromArgb(0, 120, 212);   // Change accent color
WerTheme.FontFamily = "Inter";                          // Change font family
```

---

## Buttons

### WerButton (Base)

Owner-drawn button with rounded corners, hover/pressed states, and optional icon.

```csharp
var btn = new WerButton();
btn.Text = "Click Me";
btn.ButtonType = WerButtonType.Primary;   // Primary (filled) or Secondary (outlined)
btn.ColorType = WerButtonColor.Primary;   // Blue, Primary, Warning, Success, Orange
btn.BorderRadius = 20;                    // Corner radius in px
btn.IconCode = WerIcons.Add;              // Optional left icon
```

**Default size:** 100 x 36

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ButtonType` | `WerButtonType` | Primary | `Primary` = filled, `Secondary` = outlined |
| `ColorType` | `WerButtonColor` | Blue | Color variant |
| `BorderRadius` | int | 20 | Corner radius |
| `IconCode` | string | "" | Segoe MDL2 icon (use `WerIcons` constants) |

### Pre-configured Button Variants

Instead of setting `ButtonType` and `ColorType` manually, use these ready-made controls:

**Filled buttons:**
| Control | Color |
|---------|-------|
| `WerButtonPrimary` | Teal |
| `WerButtonWarning` | Red |
| `WerButtonSuccess` | Green |
| `WerButtonOrange` | Orange |

**Outlined buttons:**
| Control | Color |
|---------|-------|
| `WerButtonOutlinedPrimary` | Teal outline |
| `WerButtonOutlinedWarning` | Red outline |
| `WerButtonOutlinedSuccess` | Green outline |
| `WerButtonOutlinedOrange` | Orange outline |

```csharp
// Drag from toolbox or create in code
var saveBtn = new WerButtonPrimary { Text = "Save", IconCode = WerIcons.Save };
var deleteBtn = new WerButtonWarning { Text = "Delete", IconCode = WerIcons.Delete };
var cancelBtn = new WerButtonOutlinedPrimary { Text = "Cancel" };
```

---

## Form Fields

All form fields share these common properties:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ShowLabel` | bool | true | Show/hide label above input |
| `LabelText` | string | varies | Label text |
| `Required` | bool | false | Shows red asterisk after label |
| `ReadOnly` | bool | false | Gray background, no editing |

**Default size:** 350 x 60 (label + input)

**Visual pattern:** Label above input (black, 9.75pt) — rounded 8px border — teal focus border — gray placeholder when empty.

---

### WerTextField

Single-line text input with label, placeholder, and password support.

```csharp
werTextField1.LabelText = "Full Name";
werTextField1.Required = true;
werTextField1.Text = "John Doe";

// Password field
werTextField2.LabelText = "Password";
werTextField2.PasswordChar = '*';
```

| Property | Type | Default |
|----------|------|---------|
| `Text` | string | "" |
| `PasswordChar` | char | '\0' (none) |

**Event:** `TextChanged`

---

### WerComboBox

Dropdown selector with data binding support and owner-drawn items.

```csharp
// Simple string items (designer or code)
werComboBox1.LabelText = "Category";
werComboBox1.Placeholder = "Select...";
werComboBox1.ItemsArray = new[] { "Electronics", "Clothing", "Food" };

// Data binding
werComboBox1.DataSource = categoryList;
werComboBox1.DisplayMember = "Name";
werComboBox1.ValueMember = "Id";

// Read selection
string selected = werComboBox1.Text;
int index = werComboBox1.SelectedIndex;
object value = werComboBox1.SelectedValue;
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Placeholder` | string | "Select..." | Shown when nothing selected |
| `ItemsArray` | string[] | — | Designer-friendly item list |
| `DataSource` | object | null | List/DataTable binding |
| `DisplayMember` | string | "" | Property to display |
| `ValueMember` | string | "" | Property for value |
| `SelectedIndex` | int | -1 | Currently selected index |
| `SelectedItem` | object | — | Currently selected item |
| `SelectedValue` | object | — | Value of selected item |

**Event:** `SelectedIndexChanged`

---

### WerRichTextField

Multi-line rich text input with scrollbar.

```csharp
werRichTextField1.LabelText = "Notes";
werRichTextField1.Placeholder = "Enter notes...";
werRichTextField1.MaxLength = 500;
werRichTextField1.WordWrap = true;
string content = werRichTextField1.Text;
```

| Property | Type | Default |
|----------|------|---------|
| `Text` | string | "" |
| `Rtf` | string | — |
| `Placeholder` | string | "" |
| `WordWrap` | bool | true |
| `MaxLength` | int | 0 (unlimited) |

**Event:** `TextChanged`

---

### WerCopyTextField

Read-only text field with a copy-to-clipboard button.

```csharp
werCopyTextField1.LabelText = "API Key";
werCopyTextField1.Value = "sk-abc123def456";
werCopyTextField1.Copied += (s, e) => WerSnackbar.Success(this, "Copied!");
```

| Property | Type | Default |
|----------|------|---------|
| `Value` | string | "" |

**Event:** `Copied` — fires after text is copied to clipboard. Shows a teal checkmark flash for 1.5 seconds.

---

### WerCurrencyField

Numeric input with currency symbol prefix, auto-formatting, and paste protection.

```csharp
werCurrencyField1.LabelText = "Amount";
werCurrencyField1.CurrencySymbol = "$";
werCurrencyField1.DecimalPlaces = 2;
werCurrencyField1.MinValue = 0;
werCurrencyField1.MaxValue = 99999.99m;

decimal amount = werCurrencyField1.Value;  // returns decimal, default 0
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Value` | decimal | 0 | Current numeric value |
| `CurrencySymbol` | string | "P" | Symbol shown as prefix |
| `DecimalPlaces` | int | 2 | 0-4 decimal places |
| `MinValue` | decimal? | null | Minimum allowed value |
| `MaxValue` | decimal? | null | Maximum allowed value |

**Event:** `ValueChanged`

---

### WerIntegerField

Whole number input with optional negative support and paste protection.

```csharp
werIntegerField1.LabelText = "Quantity";
werIntegerField1.AllowNegative = false;
werIntegerField1.MinValue = 1;
werIntegerField1.MaxValue = 999;

int qty = werIntegerField1.Value;  // returns int, default 0
```

| Property | Type | Default |
|----------|------|---------|
| `Value` | int | 0 |
| `AllowNegative` | bool | false |
| `MinValue` | int? | null |
| `MaxValue` | int? | null |

**Event:** `ValueChanged`

---

### WerPercentField

Percentage input with `%` suffix, auto-clamped to range.

```csharp
werPercentField1.LabelText = "Tax Rate";
werPercentField1.DecimalPlaces = 2;
werPercentField1.MinValue = 0;
werPercentField1.MaxValue = 100;

decimal pct = werPercentField1.Value;  // returns decimal, default 0
```

| Property | Type | Default |
|----------|------|---------|
| `Value` | decimal | 0 |
| `DecimalPlaces` | int | 2 |
| `MinValue` | decimal? | 0 |
| `MaxValue` | decimal? | 100 |

**Event:** `ValueChanged`

---

### WerPhoneField

Philippine mobile number input with `+63` prefix and auto-formatting.

```csharp
werPhoneField1.LabelText = "Mobile Number";
werPhoneField1.Required = true;

string raw = werPhoneField1.RawNumber;  // digits only, e.g. "09171234567"
string formatted = werPhoneField1.Text; // "0917-123-4567"
```

| Property | Type | Description |
|----------|------|-------------|
| `RawNumber` | string | Digits only (max 11) |
| `Text` | string | Formatted with dashes |

Auto-formats as `09XX-XXX-XXXX`. Prefix `+63` painted left.

---

### WerTelephoneField

Landline number input with phone icon prefix and auto-formatting.

```csharp
werTelephoneField1.LabelText = "Office Number";

string raw = werTelephoneField1.RawNumber;  // digits only
string formatted = werTelephoneField1.Text; // "(0XX) XXX-XXXX"
```

| Property | Type | Description |
|----------|------|-------------|
| `RawNumber` | string | Digits only (max 10) |
| `Text` | string | Formatted as `(0XX) XXX-XXXX` |

---

### WerSearchField

Compact search input with magnifying glass icon. No label.

```csharp
werSearchField1.Placeholder = "Search products...";
werSearchField1.Search += (s, e) =>
{
    string query = werSearchField1.Text;
    FilterResults(query);
};
```

**Default size:** 240 x 36

| Property | Type | Default |
|----------|------|---------|
| `Text` | string | "" |
| `Placeholder` | string | "Search..." |

**Event:** `Search` — fires on Enter key or magnifying glass icon click.

---

## Date & Time Pickers

### WerDatePicker

Calendar dropdown with formatted display.

```csharp
werDatePicker1.LabelText = "Birth Date";
werDatePicker1.Required = true;

DateTime? date = werDatePicker1.Value;  // null if not selected
```

| Property | Type | Default |
|----------|------|---------|
| `Value` | DateTime? | null |

**Event:** `ValueChanged`

Displays as `"MMM. dd, yyyy"` (e.g. "Sep. 30, 2026"). Shows placeholder `"MM/DD/YYYY"` when null.

---

### WerTimePicker

Hour/minute selector with AM/PM toggle.

```csharp
werTimePicker1.LabelText = "Start Time";

TimeSpan? time = werTimePicker1.Value;  // null if not selected
```

| Property | Type | Default |
|----------|------|---------|
| `Value` | TimeSpan? | null |

**Event:** `ValueChanged`

Layout: `[HH v]` `:` `[MM v]` `[AM/PM]` — two dropdowns with a stacked AM/PM toggle button.

---

### WerDateRange

Predefined date range dropdown (e.g. "Past 30 Days").

```csharp
werDateRange1.LabelText = "View By";
werDateRange1.RangeChanged += (s, e) =>
{
    DateTime start = werDateRange1.Result.StartDate;
    DateTime end = werDateRange1.Result.EndDate;
    LoadData(start, end);
};
```

**Default options:** "Past 7 Days", "Past 30 Days", "Past 60 Days", "Past 90 Days", "Past 6 Months", "Past 1 Year"

| Property | Type | Default |
|----------|------|---------|
| `RangeOptions` | string[] | 6 defaults above |
| `SelectedIndex` | int | 1 (Past 30 Days) |
| `Result` | `DateRangeResult` | Computed start/end dates |

**Event:** `RangeChanged`

`DateRangeResult` has `StartDate` (today) and `EndDate` (computed past date).

---

### WerDateRangePicker

Two calendar pickers side by side (From — To).

```csharp
werDateRangePicker1.LabelText = "Report Period";
werDateRangePicker1.RangeChanged += (s, e) =>
{
    DateTime? from = werDateRangePicker1.StartDate;
    DateTime? to = werDateRangePicker1.EndDate;
    Reload(from, to);
};
```

**Default size:** 500 x 60

| Property | Type | Default |
|----------|------|---------|
| `StartDate` | DateTime? | null |
| `EndDate` | DateTime? | null |

**Event:** `RangeChanged`

---

## Selection Controls

### WerCheckbox

Custom-drawn checkbox with teal checked state.

```csharp
werCheckbox1.Text = "Include inactive users";
werCheckbox1.Checked = true;
werCheckbox1.CheckedChanged += (s, e) =>
{
    bool isChecked = werCheckbox1.Checked;
};
```

| Property | Type | Default |
|----------|------|---------|
| `Checked` | bool | false |

**Event:** `CheckedChanged`

Checked state: teal filled box with white checkmark.

---

### WerRadioButton

Custom-drawn radio button. Auto-unchecks siblings in the same parent container.

```csharp
werRadioButton1.Text = "Option A";
werRadioButton2.Text = "Option B";
werRadioButton1.Checked = true;

werRadioButton1.CheckedChanged += (s, e) =>
{
    if (werRadioButton1.Checked) { /* Option A selected */ }
};
```

| Property | Type | Default |
|----------|------|---------|
| `Checked` | bool | false |

**Event:** `CheckedChanged`

Checked state: teal circle border with teal dot inside. Clicking one radio button auto-unchecks all other `WerRadioButton` controls in the same parent.

---

### WerToggle

Animated on/off switch with smooth sliding knob.

```csharp
werToggle1.Text = "Dark Mode";
werToggle1.Checked = false;
werToggle1.CheckedChanged += (s, e) =>
{
    bool isOn = werToggle1.Checked;
};
```

| Property | Type | Default |
|----------|------|---------|
| `Checked` | bool | false |

**Event:** `CheckedChanged`

Keyboard: Space or Enter to toggle. Track animates from gray to teal; knob slides with smooth animation.

---

### WerQuantitySelector

Compact increment/decrement stepper: `[ - | value | + ]`

```csharp
werQuantitySelector1.LabelText = "Qty";
werQuantitySelector1.Minimum = 0;
werQuantitySelector1.Maximum = 99;
werQuantitySelector1.Step = 1;
werQuantitySelector1.Value = 1;

werQuantitySelector1.ValueChanged += (s, e) =>
    lblQty.Text = werQuantitySelector1.Value.ToString();
```

**Default size:** 140 x 60

| Property | Type | Default |
|----------|------|---------|
| `Value` | int | 0 |
| `Minimum` | int | 0 |
| `Maximum` | int | 999 |
| `Step` | int | 1 |

**Event:** `ValueChanged`

`-` button dims at Minimum, `+` dims at Maximum. Keyboard: Left/Down = decrement, Right/Up = increment.

---

## Display Controls

### WerLabel

Themed label with preset styles.

```csharp
werLabel1.Text = "Some text";
werLabel1.LabelStyle = WerLabelStyle.Body;
```

| Property | Type | Default |
|----------|------|---------|
| `LabelStyle` | `WerLabelStyle` | Body |

**Styles:**
| Style | Font | Color |
|-------|------|-------|
| `Heading` | 14pt Bold | TextColor |
| `Subheading` | 11pt Bold | TextColor |
| `Body` | 9.75pt Regular | TextColor |
| `Caption` | 8.25pt Regular | MutedColor |

---

### WerHeading

Heading label with H1/H2/H3 levels.

```csharp
werHeading1.Text = "Dashboard";
werHeading1.Level = WerHeadingLevel.H1;
```

| Property | Type | Default |
|----------|------|---------|
| `Level` | `WerHeadingLevel` | H1 |

| Level | Font Size |
|-------|-----------|
| `H1` | 20pt Bold |
| `H2` | 16pt Bold |
| `H3` | 13pt Bold |

---

### WerLink

Clickable link label with hover underline.

```csharp
werLink1.Text = "View Details";
werLink1.Click += (s, e) => OpenDetails();
```

| Property | Type | Default |
|----------|------|---------|
| `LinkColor` | Color | PrimaryColor (teal) |
| `HoverColor` | Color | Darker teal |
| `Underline` | bool | false |
| `UnderlineOnHover` | bool | true |

---

### WerIcon

Standalone icon using Segoe MDL2 Assets.

```csharp
werIcon1.IconCode = WerIcons.Person;
werIcon1.ForeColor = WerTheme.PrimaryColor;
werIcon1.IconSize = 20f;
```

**Default size:** 24 x 24

| Property | Type | Default |
|----------|------|---------|
| `IconCode` | string | WerIcons.Add |
| `IconSize` | float | 16f |

---

### WerBadge

Auto-sized pill badge with tinted background.

```csharp
werBadge1.Text = "Active";
werBadge1.BadgeColor = WerTheme.SuccessColor;
```

| Property | Type | Default |
|----------|------|---------|
| `BadgeColor` | Color | PrimaryColor |
| `AutoSize` | bool | true |

Pill shape with 15% alpha tinted fill and 30% alpha border.

---

### WerStatusIndicator

Status pill with colored dot indicator.

```csharp
werStatusIndicator1.Status = WerStatus.Positive;
werStatusIndicator1.Text = "Active";
```

| Property | Type | Default |
|----------|------|---------|
| `Status` | `WerStatus` | Neutral |
| `AutoSize` | bool | true |

**Statuses:**
| Status | Dot Color | Usage |
|--------|-----------|-------|
| `Neutral` | Gray | Default |
| `Informative` | Blue | Info state |
| `Positive` | Green | Active, success |
| `Notice` | Amber | Warning |
| `Negative` | Red | Error, inactive |

---

### WerDivider

Horizontal separator line.

```csharp
werDivider1.LineColor = Color.FromArgb(220, 220, 220);
werDivider1.LineThickness = 1;
```

**Default size:** 300 x 10

---

### WerBanner

Notification banner with colored top bar, icon, title, and body.

```csharp
// Use the pre-configured subclasses:
var info = new WerInfoBanner { Title = "Note", Body = "System will restart at midnight." };
var success = new WerPositiveBanner { Title = "Done", Body = "All records imported." };
var warning = new WerNoticeBanner { Title = "Warning", Body = "Disk space is low." };
var error = new WerNegativeBanner { Title = "Error", Body = "Connection failed." };
```

**Default size:** 400 x 70

| Property | Type | Default |
|----------|------|---------|
| `BannerType` | `WerBannerType` | varies by subclass |
| `Title` | string | "Title" |
| `Body` | string | "Banner text" |

**Subclasses:** `WerInfoBanner`, `WerPositiveBanner`, `WerNoticeBanner`, `WerNegativeBanner`

---

### WerCard

Dashboard stat card with accent bar.

```csharp
werCard1.Title = "Total Revenue";
werCard1.Value = "$125,000.00";
werCard1.Subtitle = "Last 30 days";
werCard1.AccentColor = WerTheme.SuccessColor;
```

**Default size:** 250 x 120

| Property | Type | Default |
|----------|------|---------|
| `Title` | string | "Total Revenue" |
| `Value` | string | "$0.00" |
| `Subtitle` | string | "" |
| `AccentColor` | Color | PrimaryColor |

---

### WerProgressBar

Rounded progress bar with percentage label.

```csharp
werProgressBar1.Value = 75;
werProgressBar1.Maximum = 100;
werProgressBar1.BarColor = WerTheme.PrimaryColor;
werProgressBar1.ShowLabel = true;  // shows "75%"
```

**Default size:** 300 x 24

| Property | Type | Default |
|----------|------|---------|
| `Value` | int | 0 |
| `Maximum` | int | 100 |
| `BarColor` | Color | PrimaryColor |
| `ShowLabel` | bool | true |

---

### WerSpinner

Animated loading spinner arc.

```csharp
werSpinner1.IsSpinning = true;
werSpinner1.SpinnerColor = WerTheme.PrimaryColor;
werSpinner1.Thickness = 3;
```

**Default size:** 40 x 40

| Property | Type | Default |
|----------|------|---------|
| `IsSpinning` | bool | false |
| `SpinnerColor` | Color | PrimaryColor |
| `Thickness` | int | 3 |

---

## Data Grid

### WerDataGrid

Fully owner-drawn data grid with search, pagination, sorting, tabs, and edit buttons. Accepts `List<T>`, `DataTable`, or any `IEnumerable`.

```csharp
// Basic setup
werDataGrid1.FieldOptions = new[] { "Name", "Qty", "Price", "Total", "OrderDate" };
werDataGrid1.PrimaryKeyColumn = "Id";
werDataGrid1.ShowEditColumn = true;
werDataGrid1.TotalAmountColumn = "Total";
werDataGrid1.PageSize = 25;
werDataGrid1.DataSource = products;

// Handle edit click
werDataGrid1.EditClicked += (s, e) =>
{
    object id = e.PrimaryKey;
    int row = e.RowIndex;
    OpenEditForm(id);
};

// Handle row selection (keyboard or mouse)
werDataGrid1.SelectedRowChanged += (s, id) => ShowPreview(id);

// Tabs
werDataGrid1.TabOptions = new[] { "All", "Active", "Archived" };
werDataGrid1.TabChanged += (s, e) =>
{
    string tab = werDataGrid1.TabSelectedText;
    FilterByTab(tab);
};

// Add button
werDataGrid1.ShowAddButton = true;
werDataGrid1.AddClicked += (s, e) => OpenAddForm();
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DataSource` | object | null | `List<T>`, `DataTable`, or `IEnumerable` |
| `FieldOptions` | string[] | null | Properties to display. `null` = all, `[]` = none |
| `PrimaryKeyColumn` | string | null | Property for edit/selection PK |
| `ShowEditColumn` | bool | false | Adds Edit button column |
| `TotalAmountColumn` | string | null | Numeric property to sum in footer |
| `PageSize` | int | 25 | Rows per page |
| `ShowFooterPagination` | bool | true | `false` = show all rows (POS mode) |
| `AllowSorting` | bool | true | Click headers to sort |
| `ShowHorizontalScroll` | bool | false | Horizontal scrollbar |
| `TabOptions` | string[] | null | Filter tabs above grid |
| `ShowAddButton` | bool | false | Add button in search bar |
| `SelectedRowId` | object | — | Read-only: PK of selected row |

**Events:**

| Event | Args | Description |
|-------|------|-------------|
| `EditClicked` | `WerDataGridEditEventArgs` | Edit button clicked |
| `SelectedRowChanged` | `EventHandler<object>` | Row selected (PK value) |
| `TabChanged` | `EventHandler` | Tab selection changed |
| `AddClicked` | `EventHandler` | Add button clicked |

**Auto-formatting:** `decimal`/`double`/`float` → `#,##0.00` (right-aligned), `DateTime` → `"MMM. dd, yyyy"`.

**Features:**
- Auto-fit column widths from content
- Column resize by dragging header borders
- Built-in search bar — filters across all visible columns
- Pagination footer with "1 - 25 of N" and nav buttons
- Keyboard navigation: Arrow keys, Page Up/Down
- Mouse wheel scrolling
- "No Records Found" message when empty
- Rounded container with subtle row separators

### Programmatic Columns

```csharp
werDataGrid1.SetColumns(new[]
{
    new WerDataGridColumn { PropertyName = "Name", HeaderText = "Product Name", Width = 200 },
    new WerDataGridColumn { PropertyName = "Price", HeaderText = "Unit Price", Width = 100,
                            Alignment = HorizontalAlignment.Right }
});
```

---

## Navigation

### WerLeftNavMenu

Left navigation panel that manages form hosting in a content panel.

```csharp
// Setup
werLeftNavMenu1.ContentPanel = panel1;  // Panel with Dock = Fill
werLeftNavMenu1.LogoText = "MyApp";
werLeftNavMenu1.LogoHeight = 60;

// Show form (cached — created once, reused)
werLeftNavMenu1.ShowForm<UsersForm>();
werLeftNavMenu1.ShowForm<UsersForm>("Users");  // with page title
```

**Default:** Dock = Left, Width = 220, White background.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ContentPanel` | Panel | null | Dock=Fill panel for hosting forms |
| `LogoText` | string | "" | Logo/app name in header |
| `LogoHeight` | int | 60 | Header height |

**Event:** `NavigationChanged`

**Form lifecycle:** Forms are cached by Type — created once and reused. All cached forms are disposed when the menu control is disposed.

---

### WerMenuButton

Navigation button — drag into `WerLeftNavMenu`. Supports accordion sub-menus.

```csharp
// Simple button — wire Click in designer
werMenuButton1.Text = "Dashboard";
werMenuButton1.IconCode = WerIcons.Home;

// Accordion sub-menu
werMenuButton2.Text = "Settings";
werMenuButton2.IconCode = WerIcons.Settings;
werMenuButton2.SubItems.Add(new SubMenuItem("Users",
    () => werLeftNavMenu1.ShowForm<UsersForm>("Users")));
werMenuButton2.SubItems.Add(new SubMenuItem("Roles",
    () => werLeftNavMenu1.ShowForm<RolesForm>("Roles")));
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `IconCode` | string | "" | Segoe MDL2 icon (use `WerIcons`) |
| `SubItems` | `List<SubMenuItem>` | empty | Accordion sub-menu items |

**SubMenuItem:**
```csharp
new SubMenuItem("Display Text", () => { /* action */ })
```

Active state: teal background + white bold text + left indicator bar.

---

### WerTopNav

Top navigation bar with avatar, username, and logout.

```csharp
werTopNav1.UserName = "Jim Cruz";     // Avatar shows "JC"
werTopNav1.PageTitle = "Dashboard";
werTopNav1.ShowLogout = true;
werTopNav1.LogoutClicked += (s, e) => Application.Exit();
```

**Default:** Dock = Top, Height = 48, White background.

| Property | Type | Default |
|----------|------|---------|
| `UserName` | string | "User" |
| `PageTitle` | string | "" |
| `ShowLogout` | bool | true |

**Event:** `LogoutClicked`

Avatar is a 32x32 circle colored by user's first initial, showing initials (first + last name).

---

### Layout Pattern (dock order matters!)

```csharp
// Add order in Controls: Fill first, Top second, Left last
this.Controls.Add(panel1);           // Dock = Fill (content)
this.Controls.Add(werTopNav1);       // Dock = Top
this.Controls.Add(werLeftNavMenu1);  // Dock = Left (full height)
```

---

## Layout & Containers

### WerGroupBox

Rounded container with heading, subheading, and divider line.

```csharp
werGroupBox1.HeadingText = "Customer Details";
werGroupBox1.SubheadingText = "Basic information";
werGroupBox1.HeaderHeight = 60;
```

**Default size:** 400 x 200

| Property | Type | Default |
|----------|------|---------|
| `HeadingText` | string | "Heading" |
| `SubheadingText` | string | "Subheading" |
| `BorderRadius` | int | 10 |
| `HeaderHeight` | int | 60 |
| `BodyBackColor` | Color | White |
| `HeaderBackColor` | Color | White |
| `DividerColor` | Color | RGB(220,220,220) |
| `BorderColor` | Color | RGB(200,210,220) |

Children are automatically pushed below the header via internal padding.

---

### WerEmptyGroupBox

Rounded container with no header — just a border. Drop controls directly inside.

```csharp
werEmptyGroupBox1.BorderRadius = 10;
werEmptyGroupBox1.BorderColor = Color.FromArgb(200, 210, 220);
```

**Default size:** 400 x 200, Padding = 10

| Property | Type | Default |
|----------|------|---------|
| `BorderRadius` | int | 10 |
| `BorderColor` | Color | RGB(200,210,220) |

---

### WerTabControl

Custom-painted tab control with teal underline indicator.

```csharp
// Add TabPages in the designer, then customize:
werTabControl1.SelectedIndexChanged += (s, e) =>
{
    int tab = werTabControl1.SelectedIndex;
};
```

Selected tab: teal text (bold) with 3px teal underline. Hover: light teal tint. Add standard `TabPage` controls in the designer.

---

## Dialogs & Feedback

### WerMessageBox

Modern styled message box with dimmed overlay. Static usage — no instance needed.

```csharp
// Simple messages
WerMessageBox.Success.Show("Record saved successfully.");
WerMessageBox.Error.Show("Something went wrong.", "Error");
WerMessageBox.Info.Show("Version 1.0.0");

// Confirmation dialogs
DialogResult result = WerMessageBox.Warning.Show(
    "Are you sure you want to delete this record?",
    "Confirm Delete",
    WerMessageButtons.YesNo);

if (result == DialogResult.Yes)
{
    DeleteRecord();
}

// Plain message (no icon)
WerMessageBox.Show("Plain message text.");
```

**Button options:** `WerMessageButtons.OK`, `OKCancel`, `YesNo`, `YesNoCancel`

**Icon types:** `WerMessageBox.Success`, `.Error`, `.Warning`, `.Info`

**Returns:** `DialogResult` (`OK`, `Cancel`, `Yes`, `No`)

Keyboard: Escape = Cancel/OK, Enter = Yes/OK.

---

### WerSnackbar

Auto-dismissing toast notification. Static usage — no instance needed.

```csharp
WerSnackbar.Success(this, "Record saved!");
WerSnackbar.Error(this, "Connection failed.");
WerSnackbar.Warning(this, "Unsaved changes.");
WerSnackbar.Info(this, "3 new messages.");
```

**Parameters:** `(Form owner, string message)`

Auto-dismisses after 3 seconds. Multiple snackbars stack vertically at the bottom-center of the owner form.

---

## WerForm (Base Form)

Base form class with theme defaults. Inherit instead of `Form` for consistent styling.

```csharp
// In your form class:
public partial class CustomerForm : WerForm
{
    public CustomerForm()
    {
        InitializeComponent();
    }
}

// In .Designer.cs, change the base class:
partial class CustomerForm : Wer.Winforms.Toolkit.Controls.WerForm
```

**Defaults applied automatically:**

| Setting | Value |
|---------|-------|
| BackColor | White |
| Font | Segoe UI 9.75pt |
| Padding | 16px all sides |
| DoubleBuffered | true |
| StartPosition | CenterScreen |
| ShowIcon | false |
| MinimizeBox | false |
| MaximizeBox | false |
| FormBorderStyle | FixedDialog |

---

## WerIcons Reference

Use `WerIcons` constants with any `IconCode` property. Renders using Segoe MDL2 Assets (built into Windows 10/11).

```csharp
button.IconCode = WerIcons.Save;
icon.IconCode = WerIcons.Person;
menuButton.IconCode = WerIcons.Home;
```

### Actions
`Add`, `Delete`, `Edit`, `Save`, `Cancel`, `Close`, `Check`, `Refresh`, `Undo`, `Redo`, `Copy`, `Paste`, `Cut`, `Print`, `Share`, `Download`, `Upload`, `Send`, `Search`, `Filter`, `Sort`

### Navigation
`Back`, `Forward`, `Up`, `Down`, `Home`, `Menu`, `More`, `Settings`, `ChevronLeft`, `ChevronRight`, `ChevronUp`, `ChevronDown`

### Status
`Info`, `Warning`, `Error`, `Success`, `Favorite`, `FavoriteFilled`, `Star`, `StarEmpty`, `Lock`, `Unlock`

### Objects
`Person`, `People`, `Mail`, `Phone`, `Calendar`, `Clock`, `Folder`, `Document`, `Photo`, `Camera`, `Globe`, `Link`, `Attach`, `Tag`, `Flag`, `Cart`, `Money`, `Database`, `Cloud`, `Key`

### View
`View`, `Hide`, `FullScreen`, `ExitFullScreen`, `ZoomIn`, `ZoomOut`, `List`, `Grid`

---

## Complete App Example

```csharp
public partial class MainForm : WerForm
{
    public MainForm()
    {
        InitializeComponent();

        // Navigation
        werLeftNavMenu1.ContentPanel = contentPanel;
        werLeftNavMenu1.LogoText = "My App";
        werTopNav1.UserName = "John Doe";
        werTopNav1.LogoutClicked += (s, e) => Application.Exit();

        // Menu buttons with sub-items
        btnDashboard.Text = "Dashboard";
        btnDashboard.IconCode = WerIcons.Home;
        btnDashboard.Click += (s, e) =>
            werLeftNavMenu1.ShowForm<DashboardForm>("Dashboard");

        btnSettings.Text = "Settings";
        btnSettings.IconCode = WerIcons.Settings;
        btnSettings.SubItems.Add(new SubMenuItem("Users",
            () => werLeftNavMenu1.ShowForm<UsersForm>("Users")));
        btnSettings.SubItems.Add(new SubMenuItem("Roles",
            () => werLeftNavMenu1.ShowForm<RolesForm>("Roles")));
    }
}

public partial class UsersForm : WerForm
{
    public UsersForm()
    {
        InitializeComponent();

        // Data grid
        werDataGrid1.FieldOptions = new[] { "Name", "Email", "Role", "Status" };
        werDataGrid1.PrimaryKeyColumn = "Id";
        werDataGrid1.ShowEditColumn = true;
        werDataGrid1.ShowAddButton = true;
        werDataGrid1.EditClicked += (s, e) => EditUser(e.PrimaryKey);
        werDataGrid1.AddClicked += (s, e) => AddUser();
    }

    private async void LoadUsers()
    {
        werSpinner1.IsSpinning = true;
        var users = await _userService.GetAllAsync();
        werDataGrid1.DataSource = users;
        werSpinner1.IsSpinning = false;
    }

    private void EditUser(object id)
    {
        var form = new UserEditForm();
        form.LoadUser(id);
        if (form.ShowDialog() == DialogResult.OK)
        {
            WerSnackbar.Success(this, "User updated!");
            LoadUsers();
        }
    }
}
```
