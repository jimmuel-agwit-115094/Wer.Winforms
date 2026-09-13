# Wer.Winforms.Toolkit

A modern, owner-drawn WinForms UI toolkit for **.NET Framework 4.7.2**. Provides 30+ styled controls with consistent theming — rounded corners, clean typography, hover states, and a cohesive visual language for business applications.

## Installation

```
Install-Package Wer.Winforms.Toolkit
```

Or add a reference to `Wer.Winforms.Toolkit.dll` in your WinForms project.

## Quick Start

```csharp
using Wer.Winforms.Toolkit;
using Wer.Winforms.Toolkit.Controls;

// All controls appear in the VS Toolbox automatically.
// Drag and drop, or create in code:

var btn = new WerButtonPrimary();
btn.Text = "Save";
btn.Click += (s, e) => { /* handle click */ };
```

## Theme System

All controls read from `WerTheme` for consistent styling. Customize before creating controls:

```csharp
// Change the primary accent color
WerTheme.PrimaryColor = Color.FromArgb(52, 152, 219);

// Change font family
WerTheme.FontFamily = "Inter";
```

### Default Theme

| Token | Value | Usage |
|-------|-------|-------|
| `PrimaryColor` | `RGB(12, 124, 146)` — Teal | Buttons, focus borders, accents |
| `WarningColor` | `RGB(179, 58, 58)` — Red | Warning buttons, errors |
| `SuccessColor` | `RGB(40, 167, 69)` — Green | Success buttons |
| `OrangeColor` | `RGB(230, 126, 34)` — Orange | Orange buttons |
| `TextColor` | `RGB(33, 37, 41)` — Dark navy | All body text |
| `FontFamily` | `"Segoe UI"` | All controls |
| `BodySize` | `9.75pt` | Standard text |
| `InputBorder` | `RGB(200, 210, 220)` | Input borders |
| `InputBorderFocus` | `RGB(12, 124, 146)` | Focused input borders |
| `InputBorderRadius` | `8px` | Rounded corners |

---

## Controls Reference

### Buttons

All buttons inherit from `WerButton` and provide pre-configured color variants.

#### Filled Buttons

| Control | Color | Usage |
|---------|-------|-------|
| `WerButtonPrimary` | Teal | Primary actions |
| `WerButtonSuccess` | Green | Confirmations |
| `WerButtonWarning` | Red | Destructive actions |
| `WerButtonOrange` | Orange | Caution actions |

#### Outlined Buttons

| Control | Color | Usage |
|---------|-------|-------|
| `WerButtonOutlinedPrimary` | Teal border | Secondary actions |
| `WerButtonOutlinedSuccess` | Green border | Soft confirm |
| `WerButtonOutlinedWarning` | Red border | Soft warning |
| `WerButtonOutlinedOrange` | Orange border | Soft caution |

```csharp
var btn = new WerButtonPrimary();
btn.Text = "Save Record";
btn.Size = new Size(160, 36);
btn.Click += (s, e) => SaveRecord();

// Disabled state
btn.Enabled = false; // Renders flat gray automatically
```

**Properties (all buttons):**

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ButtonColor` | `Color` | varies | Background color |
| `HoverColor` | `Color` | varies | Hover background |
| `PressedColor` | `Color` | varies | Pressed background |
| `BorderColor` | `Color` | `Empty` | Border color (outlined buttons) |
| `BorderWidth` | `int` | `0` / `2` | Border width |
| `BorderRadius` | `int` | `20` | Corner radius |

---

### Form Fields

All form fields follow the same visual pattern: label above, rounded bordered input below, focus highlight, placeholder support, required asterisk, read-only and disabled states.

#### WerTextField

Basic text input with label.

```csharp
werTextField1.LabelText = "Full Name";
werTextField1.Required = true;
werTextField1.PasswordChar = '*'; // for password fields

string name = werTextField1.Text;
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"TextField Label"` | Label above input |
| `Required` | `bool` | `false` | Shows red asterisk |
| `ReadOnly` | `bool` | `false` | Gray bg, no editing |
| `PasswordChar` | `char` | `'\0'` | Mask character |
| `Text` | `string` | `""` | Current value |

**Event:** `TextChanged`

---

#### WerCurrencyField

Decimal input with currency symbol prefix, thousand separators, and placeholder.

```csharp
werCurrencyField1.LabelText = "Amount";
werCurrencyField1.CurrencySymbol = "₱";
werCurrencyField1.Required = true;

// Returns decimal — no parsing needed
decimal amount = werCurrencyField1.Value; // default 0
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"Currency Label"` | Label |
| `CurrencySymbol` | `string` | `"₱"` | Prefix symbol |
| `DecimalPlaces` | `int` | `2` | Decimal precision (0-4) |
| `MinValue` | `decimal?` | `null` | Minimum allowed |
| `MaxValue` | `decimal?` | `null` | Maximum allowed |
| `Value` | `decimal` | `0` | Current value — **no parsing needed** |
| `Required` | `bool` | `false` | Shows red asterisk |
| `ReadOnly` | `bool` | `false` | Gray bg, no editing |

**Event:** `ValueChanged`

**Placeholder:** Shows gray `0.00` when empty. Clears on focus.  
**Paste protection:** Blocks non-decimal Ctrl+V.  
**Auto-format:** Formats with thousand separators on blur (e.g. `125,999.50`).

---

#### WerIntegerField

Integer-only input with placeholder.

```csharp
werIntegerField1.LabelText = "Quantity";
werIntegerField1.AllowNegative = true;

// Returns int — no parsing needed
int qty = werIntegerField1.Value; // default 0
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"Integer Label"` | Label |
| `AllowNegative` | `bool` | `false` | Allow minus sign |
| `MinValue` | `int?` | `null` | Minimum allowed |
| `MaxValue` | `int?` | `null` | Maximum allowed |
| `Value` | `int` | `0` | Current value — **no parsing needed** |

**Event:** `ValueChanged`  
**Paste protection:** Blocks non-integer Ctrl+V.

---

#### WerPercentField

Percentage input with `%` suffix, value validation.

```csharp
werPercentField1.LabelText = "Tax Rate";

// Returns decimal — no parsing needed
decimal pct = werPercentField1.Value; // default 0
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"Percent Label"` | Label |
| `DecimalPlaces` | `int` | `2` | Decimal precision |
| `MinValue` | `decimal?` | `null` | Minimum |
| `MaxValue` | `decimal?` | `100` | Maximum |
| `Value` | `decimal` | `0` | Current value — **no parsing needed** |

**Event:** `ValueChanged`  
**Paste protection:** Blocks non-decimal Ctrl+V.

---

#### WerDatePicker

Calendar date picker with custom painted face.

```csharp
werDatePicker1.LabelText = "Order Date";
werDatePicker1.Required = true;

DateTime? date = werDatePicker1.Value; // null if not set
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"DatePicker Label"` | Label |
| `Required` | `bool` | `false` | Shows asterisk |
| `ReadOnly` | `bool` | `false` | No editing |
| `Value` | `DateTime?` | `null` | Selected date |

**Event:** `ValueChanged`  
**Placeholder:** Shows `MM/DD/YYYY` when no date selected.  
**Display format:** `MMM. dd, yyyy` (e.g. `Sep. 01, 2025`).

---

#### WerTimePicker

Time selector with hour/minute dropdowns and AM/PM toggle.

```csharp
werTimePicker1.LabelText = "Start Time";

// Set time
werTimePicker1.Value = new TimeSpan(14, 30, 0); // 2:30 PM

// Get time
TimeSpan? time = werTimePicker1.Value;
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"TimePicker Label"` | Label |
| `Required` | `bool` | `false` | Shows asterisk |
| `ReadOnly` | `bool` | `false` | No editing |
| `Value` | `TimeSpan?` | `null` | Selected time |

**Event:** `ValueChanged`  
**Layout:** `[ HH ▾ ] : [ MM ▾ ] AM PM`

---

#### WerPhoneField

Philippine mobile number input with auto-formatting.

```csharp
werPhoneField1.LabelText = "Mobile Number";

string formatted = werPhoneField1.Text;      // "0917-123-4567"
string raw = werPhoneField1.RawNumber;        // "09171234567"
```

**Format:** `09XX-XXX-XXXX` with `+63` prefix display.

---

#### WerTelephoneField

Philippine landline number input with auto-formatting.

```csharp
werTelephoneField1.LabelText = "Office Phone";

string formatted = werTelephoneField1.Text;   // "(02) 123-4567"
string raw = werTelephoneField1.RawNumber;     // "021234567"
```

**Format:** `(0XX) XXX-XXXX` with `☎` icon prefix.

---

#### WerComboBox

Styled dropdown selector.

```csharp
werComboBox1.LabelText = "Category";
werComboBox1.Placeholder = "Select category...";
werComboBox1.Items.AddRange(new[] { "Electronics", "Furniture", "Accessories" });

string selected = werComboBox1.Text;
int index = werComboBox1.SelectedIndex;
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"ComboBox Label"` | Label |
| `Placeholder` | `string` | `"Select..."` | Placeholder text |
| `Items` | `ObjectCollection` | empty | Dropdown items |
| `ItemsArray` | `string[]` | `null` | Designer-friendly items |
| `SelectedIndex` | `int` | `-1` | Selected index |

**Event:** `SelectedIndexChanged`

---

#### WerSearchField

Search input with magnifying glass icon.

```csharp
werSearchField1.TextChanged += (s, e) =>
{
    FilterResults(werSearchField1.Text);
};
```

---

#### WerRichTextField

Multi-line rich text editor with scrollbar.

```csharp
werRichTextField1.LabelText = "Description";
werRichTextField1.WordWrap = true;

string text = werRichTextField1.Text;
string rtf = werRichTextField1.Rtf;
```

---

#### WerCopyTextField

Read-only text field with copy-to-clipboard button. Ideal for displaying IDs.

```csharp
werCopyTextField1.LabelText = "Record ID";
werCopyTextField1.Value = "abc-123-def-456";

werCopyTextField1.Copied += (s, e) => ShowToast("Copied!");
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"CopyText Label"` | Label |
| `Value` | `string` | `""` | Read-only text to copy |

**Event:** `Copied` — fires after clipboard copy.  
**Feedback:** Copy icon turns to teal `✓` for 1.5 seconds after copy.

---

### Selectors

#### WerDateRange

Predefined date range dropdown for filtering.

```csharp
werDateRange1.LabelText = "View By";

// Access computed dates
DateTime startDate = werDateRange1.Result.StartDate;  // today
DateTime endDate = werDateRange1.Result.EndDate;      // e.g. 30 days ago

// React to changes
werDateRange1.RangeChanged += (s, e) =>
{
    var r = werDateRange1.Result;
    ReloadData(r.StartDate, r.EndDate);
};
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `LabelText` | `string` | `"View By"` | Label |
| `RangeOptions` | `string[]` | See below | Custom range labels |
| `SelectedIndex` | `int` | `1` | Selected range index |
| `SelectedRange` | `string` | `"Past 30 Days"` | Selected range text |
| `Result` | `DateRangeResult` | computed | `.StartDate` and `.EndDate` |

**Default ranges:** Past 7 Days, Past 30 Days, Past 60 Days, Past 90 Days, Past 6 Months, Past 1 Year

**Event:** `RangeChanged`

---

#### WerToggle

Modern toggle switch.

```csharp
werToggle1.Text = "Enable notifications";

// Get/set state — no parsing needed
bool isOn = werToggle1.Checked;
werToggle1.Checked = true;

werToggle1.CheckedChanged += (s, e) =>
{
    UpdateSetting(werToggle1.Checked);
};
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Checked` | `bool` | `false` | Toggle state |
| `Text` | `string` | `""` | Label next to toggle |

**Event:** `CheckedChanged`  
**Keyboard:** Space / Enter toggles.  
**Animation:** Smooth knob slide with color interpolation.

---

### Display Controls

#### WerLabel

Styled label with body/subheading variants.

```csharp
werLabel1.Text = "Hello World";
werLabel1.LabelStyle = WerLabelStyle.Subheading; // bold 11pt
```

---

#### WerHeading

Large heading text.

```csharp
werHeading1.Text = "Dashboard";
werHeading1.Level = WerHeadingLevel.H1; // 20pt bold
// H2 = 16pt bold, H3 = 13pt bold
```

---

#### WerLink

Styled hyperlink label.

```csharp
werLink1.Text = "View Details";
werLink1.UnderlineOnHover = true;
werLink1.Click += (s, e) => OpenDetails();
```

---

#### WerCheckbox

Custom-drawn checkbox.

```csharp
werCheckbox1.Text = "I agree to terms";
bool agreed = werCheckbox1.Checked;
```

---

#### WerRadioButton

Custom-drawn radio button. Auto-unchecks siblings.

```csharp
werRadio1.Text = "Option A";
werRadio2.Text = "Option B";
// Checking one auto-unchecks the other in the same parent
```

---

#### WerStatusIndicator

Colored pill badge for status display.

```csharp
werStatus1.Status = WerStatus.Positive;
werStatus1.Text = "Active";
```

| Status | Colors |
|--------|--------|
| `Neutral` | Gray |
| `Informative` | Blue |
| `Positive` | Green |
| `Notice` | Amber |
| `Negative` | Red |

---

#### WerBanner

Alert/notification banner with colored top bar.

```csharp
var banner = new WerInfoBanner();
banner.Title = "Information";
banner.Text = "Your changes have been saved.";
```

| Variant | Color |
|---------|-------|
| `WerInfoBanner` | Teal |
| `WerPositiveBanner` | Green |
| `WerNoticeBanner` | Amber |
| `WerNegativeBanner` | Red |

---

#### WerDivider

Horizontal line separator.

```csharp
werDivider1.LineColor = Color.FromArgb(220, 220, 220);
```

---

### Layout Controls

#### WerGroupBox

Rounded card with header (heading + subheading) and body panel.

```csharp
werGroupBox1.HeadingText = "Personal Info";
werGroupBox1.SubheadingText = "Enter your details";
// Add child controls to werGroupBox1.BodyPanel
```

#### WerEmptyGroupBox

Simple rounded card container with no header.

#### WerTabControl / WerTabPage

Styled tab control with clean rendering.

---

### Data Grid

#### WerDataGrid

Full-featured data grid with modern UI. Owner-drawn — no DataGridView.

```csharp
// Minimal setup
werDataGrid1.DataSource = products;  // List<T>, DataTable, IEnumerable

// Control which columns display
werDataGrid1.FieldOptions = new[] { "Name", "Category", "Qty", "Price", "Total" };

// Primary key for edit events (doesn't need to be a visible column)
werDataGrid1.PrimaryKeyColumn = "Id";

// Edit button column
werDataGrid1.ShowEditColumn = true;
werDataGrid1.EditClicked += (s, args) =>
{
    int id = (int)args.PrimaryKey;
    OpenEditForm(id);
};

// Pagination
werDataGrid1.PageSize = 25;

// Total amount in footer (numeric columns only)
werDataGrid1.TotalAmountColumn = "Total";

// Tab filters
werDataGrid1.TabOptions = new[] { "All", "Active", "On Hold" };
werDataGrid1.TabChanged += (s, e) =>
{
    if (werDataGrid1.TabSelected == 0) LoadAll();
    else if (werDataGrid1.TabSelected == 1) LoadActive();
};

// Horizontal scroll for many columns
werDataGrid1.ShowHorizontalScroll = true;
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DataSource` | `object` | `null` | `List<T>`, `DataTable`, `IEnumerable` |
| `FieldOptions` | `string[]` | `null` | Columns to show. `null` = all |
| `PrimaryKeyColumn` | `string` | `null` | PK property for edit events |
| `ShowEditColumn` | `bool` | `false` | Show Edit button column |
| `PageSize` | `int` | `25` | Rows per page |
| `TotalAmountColumn` | `string` | `null` | Numeric column to sum in footer |
| `AllowSorting` | `bool` | `true` | Click headers to sort |
| `ShowHorizontalScroll` | `bool` | `false` | Horizontal scrollbar |
| `TabOptions` | `string[]` | `null` | Filter tabs above grid |
| `TabSelected` | `int` | `0` | Selected tab index |
| `TabSelectedText` | `string` | `""` | Selected tab label |

| Event | Args | Description |
|-------|------|-------------|
| `EditClicked` | `WerDataGridEditEventArgs` | Edit button clicked. `.PrimaryKey`, `.RowIndex` |
| `TabChanged` | `EventArgs` | Tab selection changed |

**Features:**
- Auto-fit column widths from content measurement
- Column resize by dragging header borders
- Built-in search field (filters across all visible columns)
- Pagination footer with navigation buttons
- Auto-formatting: `decimal` → `#,##0.00`, `DateTime` → `MMM. dd, yyyy`
- Rounded container, rounded selection highlight, subtle row separators
- *No Records Found* message when empty

---

## Common Patterns

### All form fields share these properties:

| Property | Type | Description |
|----------|------|-------------|
| `LabelText` | `string` | Label above the input |
| `Required` | `bool` | Red asterisk indicator |
| `ReadOnly` | `bool` | Gray background, no editing |
| `Enabled` | `bool` | Disabled state (inherited) |

### Visual states:

| State | Label | Border | Background |
|-------|-------|--------|------------|
| Normal | Black | `RGB(200, 210, 220)` | White |
| Focused | Black | Teal `RGB(12, 124, 146)` 1.5px | White |
| Required | Black + red `*` | Normal | White |
| Read-only | Orange | Gray | `RGB(242, 242, 242)` |
| Disabled | Gray | None | `RGB(242, 242, 242)` |

---

## Requirements

- .NET Framework 4.7.2
- Windows Forms
- System.Drawing
- System.Windows.Forms
- System.Design (for designer support)

## License

MIT
