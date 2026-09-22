using System;
using System.Drawing;
using System.Linq;

namespace Wer.Winforms.Toolkit
{
    public enum WerButtonType
    {
        Primary,
        Secondary
    }

    public enum WerButtonColor
    {
        Primary,
        Warning,
        Success,
        Orange
    }

    public static class WerTheme
    {
        // ── Font family ─────────────────────────────────────────
        private static string _fontFamily = "Segoe UI";

        public static string FontFamily
        {
            get => _fontFamily;
            set => _fontFamily = value;
        }

        // ── Font sizes ──────────────────────────────────────────
        private static float _headingSize = 14f;
        private static float _subheadingSize = 11f;
        private static float _bodySize = 9.75f;
        private static float _captionSize = 8.25f;
        private static float _buttonSize = 9.75f;

        public static float HeadingSize { get => _headingSize; set => _headingSize = value; }
        public static float SubheadingSize { get => _subheadingSize; set => _subheadingSize = value; }
        public static float BodySize { get => _bodySize; set => _bodySize = value; }
        public static float CaptionSize { get => _captionSize; set => _captionSize = value; }
        public static float ButtonSize { get => _buttonSize; set => _buttonSize = value; }

        // ── Brand / semantic colors ─────────────────────────────
        private static Color _primaryColor = Color.FromArgb(12, 124, 146);
        private static Color _warningColor = Color.FromArgb(179, 58, 58);
        private static Color _successColor = Color.FromArgb(40, 167, 69);
        private static Color _orangeColor = Color.FromArgb(230, 126, 34);
        private static Color _textColor = Color.FromArgb(33, 37, 41);
        private static Color _bodyColor = Color.FromArgb(12, 124, 146);
        private static Color _mutedColor = Color.FromArgb(130, 130, 130);
        private static Color _disabledColor = Color.FromArgb(180, 180, 180);
        private static Color _borderColor = Color.FromArgb(200, 200, 200);

        public static Color PrimaryColor { get => _primaryColor; set => _primaryColor = value; }
        public static Color WarningColor { get => _warningColor; set => _warningColor = value; }
        public static Color SuccessColor { get => _successColor; set => _successColor = value; }
        public static Color OrangeColor { get => _orangeColor; set => _orangeColor = value; }
        public static Color TextColor { get => _textColor; set => _textColor = value; }
        public static Color BodyColor { get => _bodyColor; set => _bodyColor = value; }
        public static Color MutedColor { get => _mutedColor; set => _mutedColor = value; }
        public static Color DisabledColor { get => _disabledColor; set => _disabledColor = value; }
        public static Color BorderColor { get => _borderColor; set => _borderColor = value; }

        // ── Shared control colors (used across all form controls) ───
        // These are the common colors duplicated in every control.
        // Reference these instead of hardcoding Color.FromArgb(...).

        /// <summary>Input border normal state: RGB(200, 210, 220)</summary>
        public static readonly Color InputBorder = Color.FromArgb(200, 210, 220);

        /// <summary>Input border focus state: same as PrimaryColor RGB(12, 124, 146)</summary>
        public static readonly Color InputBorderFocus = Color.FromArgb(12, 124, 146);

        /// <summary>Input background normal: White</summary>
        public static readonly Color InputBg = Color.White;

        /// <summary>Input background disabled/read-only: RGB(242, 242, 242)</summary>
        public static readonly Color InputBgDisabled = Color.FromArgb(242, 242, 242);

        /// <summary>Placeholder text color: RGB(160, 170, 180)</summary>
        public static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);

        /// <summary>Label normal color: Black</summary>
        public static readonly Color LabelColor = Color.Black;

        /// <summary>Label read-only/required color: RGB(200, 100, 20)</summary>
        public static readonly Color LabelReadOnlyColor = Color.FromArgb(200, 100, 20);

        /// <summary>Label disabled color: RGB(150, 150, 150)</summary>
        public static readonly Color LabelDisabledColor = Color.FromArgb(150, 150, 150);

        /// <summary>Required asterisk color: RGB(210, 50, 50)</summary>
        public static readonly Color RequiredStarColor = Color.FromArgb(210, 50, 50);

        /// <summary>Chevron/icon normal color: RGB(140, 150, 160)</summary>
        public static readonly Color IconColor = Color.FromArgb(140, 150, 160);

        // ── Shared control layout constants ──────────────────────
        /// <summary>Label height above input: 20px</summary>
        public const int LabelHeight = 20;

        /// <summary>Gap between label and input: 4px</summary>
        public const int LabelGap = 4;

        /// <summary>Rounded corner radius for inputs: 8px</summary>
        public const int InputBorderRadius = 8;

        /// <summary>Horizontal padding inside inputs: 10px</summary>
        public const int InputPadH = 10;

        // ── Button color resolver ────────────────────────────
        public static Color GetButtonColor(WerButtonColor color)
        {
            switch (color)
            {
                case WerButtonColor.Primary: return _primaryColor;
                case WerButtonColor.Warning: return _warningColor;
                case WerButtonColor.Success: return _successColor;
                case WerButtonColor.Orange:  return _orangeColor;
                default: return _primaryColor;
            }
        }

        // ── Pre-built fonts ─────────────────────────────────────
        public static Font HeadingFont => new Font(_fontFamily, _headingSize, FontStyle.Bold);
        public static Font SubheadingFont => new Font(_fontFamily, _subheadingSize, FontStyle.Bold);
        public static Font BodyFont => new Font(_fontFamily, _bodySize, FontStyle.Regular);
        public static Font CaptionFont => new Font(_fontFamily, _captionSize, FontStyle.Regular);
        // "Segoe UI Semibold" is a distinct face on Windows 10/11 — weight 600 without full bold.
        // Cached on first call; falls back to Bold if the semibold face is unavailable.
        private static bool? _semiboldAvailable;
        public static Font ButtonFont
        {
            get
            {
                if (_semiboldAvailable == null)
                {
                    string semibold = _fontFamily + " Semibold";
                    _semiboldAvailable = new System.Drawing.Text.InstalledFontCollection().Families
                        .Any(f => f.Name.Equals(semibold, StringComparison.OrdinalIgnoreCase));
                }
                return _semiboldAvailable == true
                    ? new Font(_fontFamily + " Semibold", _buttonSize, FontStyle.Regular)
                    : new Font(_fontFamily, _buttonSize, FontStyle.Bold);
            }
        }
    }
}
