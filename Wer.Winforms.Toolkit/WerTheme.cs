using System.Drawing;

namespace Wer.Winforms.Toolkit
{
    public static class WerTheme
    {
        private static string _fontFamily = "Segoe UI";

        // Font sizes
        private static float _headingSize = 14f;
        private static float _subheadingSize = 11f;
        private static float _bodySize = 9.75f;
        private static float _captionSize = 8.25f;
        private static float _buttonSize = 9.75f;

        // Colors
        private static Color _primaryColor = Color.FromArgb(12, 124, 146);
        private static Color _warningColor = Color.FromArgb(179, 58, 58);
        private static Color _successColor = Color.FromArgb(40, 167, 69);
        private static Color _orangeColor = Color.FromArgb(230, 126, 34);
        private static Color _textColor = Color.FromArgb(33, 37, 41);
        private static Color _bodyColor = Color.FromArgb(12, 124, 146);
        private static Color _mutedColor = Color.FromArgb(130, 130, 130);
        private static Color _disabledColor = Color.FromArgb(180, 180, 180);
        private static Color _borderColor = Color.FromArgb(200, 200, 200);

        // Font family
        public static string FontFamily
        {
            get => _fontFamily;
            set => _fontFamily = value;
        }

        // Font sizes
        public static float HeadingSize { get => _headingSize; set => _headingSize = value; }
        public static float SubheadingSize { get => _subheadingSize; set => _subheadingSize = value; }
        public static float BodySize { get => _bodySize; set => _bodySize = value; }
        public static float CaptionSize { get => _captionSize; set => _captionSize = value; }
        public static float ButtonSize { get => _buttonSize; set => _buttonSize = value; }

        // Colors
        public static Color PrimaryColor { get => _primaryColor; set => _primaryColor = value; }
        public static Color WarningColor { get => _warningColor; set => _warningColor = value; }
        public static Color SuccessColor { get => _successColor; set => _successColor = value; }
        public static Color OrangeColor { get => _orangeColor; set => _orangeColor = value; }
        public static Color TextColor { get => _textColor; set => _textColor = value; }
        public static Color BodyColor { get => _bodyColor; set => _bodyColor = value; }
        public static Color MutedColor { get => _mutedColor; set => _mutedColor = value; }
        public static Color DisabledColor { get => _disabledColor; set => _disabledColor = value; }
        public static Color BorderColor { get => _borderColor; set => _borderColor = value; }

        // Pre-built fonts
        public static Font HeadingFont => new Font(_fontFamily, _headingSize, FontStyle.Bold);
        public static Font SubheadingFont => new Font(_fontFamily, _subheadingSize, FontStyle.Bold);
        public static Font BodyFont => new Font(_fontFamily, _bodySize, FontStyle.Regular);
        public static Font CaptionFont => new Font(_fontFamily, _captionSize, FontStyle.Regular);
        public static Font ButtonFont => new Font(_fontFamily, _buttonSize, FontStyle.Regular);
    }
}
