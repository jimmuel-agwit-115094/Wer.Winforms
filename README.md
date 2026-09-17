# Wer.Winforms.Toolkit

Modern owner-drawn WinForms UI toolkit for **.NET Framework 4.7.2**. 40+ styled controls with consistent theming — rounded corners, clean typography, and modern visual design for business applications.

## Installation

```
Install-Package Wer.Winforms.Toolkit
```

## Controls

### Buttons
| Control | Description |
|---------|-------------|
| `WerButtonPrimary` | Teal filled button |
| `WerButtonSuccess` | Green filled button |
| `WerButtonWarning` | Red filled button |
| `WerButtonOrange` | Orange filled button |
| `WerButtonOutlinedPrimary` | Teal outlined button |
| `WerButtonOutlinedSuccess` | Green outlined button |
| `WerButtonOutlinedWarning` | Red outlined button |
| `WerButtonOutlinedOrange` | Orange outlined button |

### Form Fields
| Control | Description |
|---------|-------------|
| `WerTextField` | Single-line text input with label |
| `WerCurrencyField` | Decimal currency input (`Value` → `decimal`) |
| `WerIntegerField` | Integer input (`Value` → `int`) |
| `WerPercentField` | Percentage input (`Value` → `decimal`) |
| `WerDatePicker` | Styled date picker |
| `WerTimePicker` | Styled time picker |
| `WerPhoneField` | Phone number input |
| `WerTelephoneField` | Telephone input |
| `WerSearchField` | Search input with icon |
| `WerComboBox` | Styled combo box dropdown |
| `WerRichTextField` | Multi-line rich text input |
| `WerCopyTextField` | Read-only field with copy button |

### Selectors
| Control | Description |
|---------|-------------|
| `WerDateRange` | Predefined date range dropdown (Past 7/30/60/90 days, etc.) |
| `WerDateRangePicker` | Two-calendar from/to date picker |
| `WerToggle` | On/off toggle switch |
| `WerQuantitySelector` | `[ − \| 0 \| + ]` inline quantity stepper with min/max/step |

### Display
| Control | Description |
|---------|-------------|
| `WerLabel` | Styled label |
| `WerHeading` | Page/section heading |
| `WerLink` | Clickable link label |
| `WerCheckbox` | Styled checkbox |
| `WerRadioButton` | Styled radio button |
| `WerStatusIndicator` | Colored status pill |
| `WerDivider` | Horizontal rule divider |
| `WerBanner` | Info/Positive/Negative/Notice banner |
| `WerBadge` | Pill-shaped badge with tinted background |
| `WerIcon` | Segoe MDL2 icon (70+ named constants via `WerIcons`) |
| `WerCard` | Stat card with title, value, subtitle, accent bar |

### Layout
| Control | Description |
|---------|-------------|
| `WerGroupBox` | Styled group box with rounded border |
| `WerEmptyGroupBox` | Group box without title |
| `WerTabControl` | Styled tab control |
| `WerTabPage` | Tab page for use with `WerTabControl` |

### Navigation
| Control | Description |
|---------|-------------|
| `WerLeftNavMenu` | Left navigation panel with form hosting |
| `WerMenuButton` | Navigation item for `WerLeftNavMenu` |
| `WerTopNav` | Top navigation bar with avatar and logout |

### Data
| Control | Description |
|---------|-------------|
| `WerDataGrid` | Owner-drawn data grid with search, pagination, sort, tabs, and edit buttons |

### Feedback
| Control | Description |
|---------|-------------|
| `WerSnackbar` | Auto-dismiss toast notifications |
| `WerMessageBox` | Modern styled modal message box |
| `WerProgressBar` | Rounded progress bar with label |
| `WerSpinner` | Animated loading arc |

---

## Usage Examples

### Snackbar Toasts
```csharp
WerSnackbar.Success(this, "Record saved successfully.");
WerSnackbar.Error(this, "Something went wrong.");
WerSnackbar.Warning(this, "Connection unstable.");
WerSnackbar.Info(this, "3 new messages.");
```

### Message Box
```csharp
WerMessageBox.Success.Show("Record saved.");
WerMessageBox.Error.Show("Something went wrong.", "Oops");

var result = WerMessageBox.Warning.Show("Delete this?", "Confirm", WerMessageButtons.YesNo);
if (result == DialogResult.Yes) { /* ... */ }
```

### Data Grid
```csharp
werDataGrid1.PrimaryKeyColumn = "Id";
werDataGrid1.ShowEditColumn = true;
werDataGrid1.FieldOptions = new[] { "Name", "Category", "Qty", "Price" };
werDataGrid1.TotalAmountColumn = "Price";
werDataGrid1.PageSize = 25;
werDataGrid1.DataSource = myList;

werDataGrid1.EditClicked += (s, args) =>
    MessageBox.Show("Edit: " + args.PrimaryKey);

// Keyboard row selection + SelectedRowId
werDataGrid1.SelectedRowChanged += (s, id) =>
    lblSelected.Text = id?.ToString();

// POS / full-list mode — show all rows, no pagination footer
werDataGrid1.ShowFooterPagination = false;
```

### Quantity Selector
```csharp
werQuantitySelector1.LabelText = "Qty";
werQuantitySelector1.Minimum   = 0;
werQuantitySelector1.Maximum   = 99;
werQuantitySelector1.Step      = 1;
werQuantitySelector1.Value     = 1;

werQuantitySelector1.ValueChanged += (s, e) =>
    lblQty.Text = werQuantitySelector1.Value.ToString();
```

### Navigation
```csharp
werLeftNavMenu1.ContentPanel = panel1;
werLeftNavMenu1.LogoText = "MyApp";
werTopNav1.UserName = "Jane Cruz";
werTopNav1.LogoutClicked += (s, e) => Application.Exit();

// In menu button click handler
werLeftNavMenu1.ShowForm<UsersForm>("User Management");
```

### Numeric Fields
```csharp
decimal amount = werCurrencyField1.Value;  // decimal, default 0
int qty        = werIntegerField1.Value;   // int, default 0
decimal pct    = werPercentField1.Value;   // decimal, default 0
```

### Card
```csharp
werCard1.Title = "Total Revenue";
werCard1.Value = "$125,000.00";
werCard1.Subtitle = "Last 30 days";
werCard1.AccentColor = WerTheme.SuccessColor;
```

### Toggle
```csharp
werToggle1.Text = "Enable notifications";
werToggle1.Checked = true;
werToggle1.CheckedChanged += (s, e) => UpdateSetting(werToggle1.Checked);
```

### Progress & Spinner
```csharp
werProgressBar1.Value = 75;
werProgressBar1.BarColor = WerTheme.PrimaryColor;
werProgressBar1.ShowLabel = true;   // shows "75%"

werSpinner1.IsSpinning = true;
```

---

## Theme

All controls use `WerTheme` for consistent styling:

| Token | Value | Usage |
|-------|-------|-------|
| `PrimaryColor` | `RGB(12, 124, 146)` — Teal | Buttons, focus borders, accents |
| `SuccessColor` | `RGB(40, 167, 69)` — Green | Success buttons |
| `WarningColor` | `RGB(179, 58, 58)` — Red | Warning buttons, errors |
| `OrangeColor` | `RGB(230, 126, 34)` — Orange | Orange buttons |
| `TextColor` | `RGB(33, 37, 41)` — Dark navy | All body text |
| `FontFamily` | `"Segoe UI"` | All controls |
| `InputBorderRadius` | `8px` | Rounded corners |

---

## Requirements

- .NET Framework 4.7.2
- Windows 10 / 11 (uses Segoe UI and Segoe MDL2 Assets fonts)

## License

MIT
