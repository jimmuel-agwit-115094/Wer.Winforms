using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit
{
    public enum WerThemeMode { Light, Dark }

    public static class WerTheme
    {
        // ── Mode ────────────────────────────────────────────────────
        public static WerThemeMode Mode { get; private set; } = WerThemeMode.Light;
        public static bool IsDark => Mode == WerThemeMode.Dark;

        /// <summary>Fires after the theme mode changes. Controls subscribe to this to refresh.</summary>
        public static event EventHandler ThemeChanged;

        public static void SetDarkMode()  => SetMode(WerThemeMode.Dark);
        public static void SetLightMode() => SetMode(WerThemeMode.Light);

        public static void SetMode(WerThemeMode mode)
        {
            if (Mode == mode) return;
            Mode = mode;
            ThemeChanged?.Invoke(null, EventArgs.Empty);

            // Refresh all open forms
            foreach (Form f in Application.OpenForms)
            {
                f.BackColor = SurfaceColor;
                f.ForeColor = TextColor;
                f.Invalidate(true);
                f.Update();
            }
        }

        // ── Font family ─────────────────────────────────────────────
        public static string FontFamily { get; set; } = "Segoe UI";

        // ── Font sizes ───────────────────────────────────────────────
        public static float HeadingSize    { get; set; } = 14f;
        public static float SubheadingSize { get; set; } = 11f;
        public static float BodySize       { get; set; } = 9.75f;
        public static float CaptionSize    { get; set; } = 8.25f;
        public static float ButtonSize     { get; set; } = 9.75f;

        // ── Pre-built fonts ──────────────────────────────────────────
        public static Font HeadingFont    => new Font(FontFamily, HeadingSize,    FontStyle.Bold);
        public static Font SubheadingFont => new Font(FontFamily, SubheadingSize, FontStyle.Bold);
        public static Font BodyFont       => new Font(FontFamily, BodySize,       FontStyle.Regular);
        public static Font CaptionFont    => new Font(FontFamily, CaptionSize,    FontStyle.Regular);
        public static Font ButtonFont     => new Font(FontFamily, ButtonSize,     FontStyle.Regular);

        // ── Brand / semantic colors (same in both themes) ────────────
        public static Color PrimaryColor { get; set; } = Color.FromArgb(12, 124, 146);
        public static Color WarningColor { get; set; } = Color.FromArgb(179, 58, 58);
        public static Color SuccessColor { get; set; } = Color.FromArgb(40, 167, 69);
        public static Color OrangeColor  { get; set; } = Color.FromArgb(230, 126, 34);

        // ── Surface tokens ───────────────────────────────────────────
        /// <summary>Main background (form, panel, card body)</summary>
        public static Color SurfaceColor =>
            IsDark ? Color.FromArgb(16, 16, 20) : Color.White;

        /// <summary>Elevated surface — cards, panels, grid container</summary>
        public static Color SurfaceAltColor =>
            IsDark ? Color.FromArgb(38, 38, 48) : Color.FromArgb(248, 249, 250);

        /// <summary>Container / card border</summary>
        public static Color SurfaceBorderColor =>
            IsDark ? Color.FromArgb(56, 56, 70) : Color.FromArgb(218, 222, 228);

        // ── Text tokens ──────────────────────────────────────────────
        public static Color TextColor =>
            IsDark ? Color.FromArgb(230, 230, 238) : Color.FromArgb(33, 37, 41);

        public static Color MutedColor =>
            IsDark ? Color.FromArgb(148, 148, 165) : Color.FromArgb(130, 130, 130);

        public static Color DisabledColor =>
            IsDark ? Color.FromArgb(80, 80, 96) : Color.FromArgb(180, 180, 180);

        // ── Input tokens ─────────────────────────────────────────────
        public static Color InputBg =>
            IsDark ? Color.FromArgb(38, 38, 48) : Color.White;

        public static Color InputBgDisabled =>
            IsDark ? Color.FromArgb(28, 28, 36) : Color.FromArgb(242, 242, 242);

        public static Color InputBorder =>
            IsDark ? Color.FromArgb(56, 56, 70) : Color.FromArgb(200, 210, 220);

        /// <summary>Focus border — same teal in both themes</summary>
        public static Color InputBorderFocus => PrimaryColor;

        public static Color PlaceholderColor =>
            IsDark ? Color.FromArgb(90, 90, 108) : Color.FromArgb(160, 170, 180);

        // ── Label tokens ─────────────────────────────────────────────
        public static Color LabelColor =>
            IsDark ? Color.FromArgb(205, 205, 215) : Color.Black;

        public static Color LabelReadOnlyColor => Color.FromArgb(200, 100, 20);

        public static Color LabelDisabledColor =>
            IsDark ? Color.FromArgb(80, 80, 96) : Color.FromArgb(150, 150, 150);

        public static Color RequiredStarColor => Color.FromArgb(210, 50, 50);

        public static Color IconColor =>
            IsDark ? Color.FromArgb(110, 110, 130) : Color.FromArgb(140, 150, 160);

        // ── DataGrid tokens ──────────────────────────────────────────
        /// <summary>Grid header — slightly darker than the card surface</summary>
        public static Color GridHeaderBg =>
            IsDark ? Color.FromArgb(26, 26, 34) : Color.FromArgb(248, 249, 250);

        public static Color GridHeaderText =>
            IsDark ? Color.FromArgb(158, 158, 178) : Color.FromArgb(55, 65, 81);

        public static Color GridHeaderSep =>
            IsDark ? Color.FromArgb(48, 48, 60) : Color.FromArgb(228, 231, 235);

        /// <summary>Row separator — visible but not harsh</summary>
        public static Color GridRowSep =>
            IsDark ? Color.FromArgb(46, 46, 58) : Color.FromArgb(238, 240, 243);

        public static Color GridSelectedBg =>
            IsDark ? Color.FromArgb(16, 52, 78) : Color.FromArgb(234, 245, 252);

        public static Color GridEditBtnHover =>
            IsDark ? Color.FromArgb(12, 42, 64) : Color.FromArgb(240, 250, 252);

        // ── Nav tokens ───────────────────────────────────────────────
        public static Color NavBg =>
            IsDark ? Color.FromArgb(12, 12, 16) : Color.FromArgb(30, 30, 30);

        public static Color NavText =>
            IsDark ? Color.FromArgb(165, 165, 185) : Color.FromArgb(200, 200, 200);

        public static Color NavActiveText => Color.White;

        public static Color MenuNormalText =>
            IsDark ? Color.FromArgb(145, 145, 165) : Color.FromArgb(75, 85, 100);

        public static Color MenuActiveBg =>
            IsDark ? Color.FromArgb(16, 52, 78) : Color.FromArgb(236, 247, 250);

        public static Color MenuHoverBg =>
            IsDark ? Color.FromArgb(36, 36, 46) : Color.FromArgb(246, 248, 250);

        public static Color TopNavBg =>
            IsDark ? Color.FromArgb(26, 26, 34) : Color.White;

        public static Color TopNavBorder =>
            IsDark ? Color.FromArgb(48, 48, 60) : Color.FromArgb(228, 231, 235);

        public static Color LinkHoverColor =>
            IsDark ? Color.FromArgb(100, 190, 210) : Color.FromArgb(8, 86, 102);

        public static Color ProgressTrack =>
            IsDark ? Color.FromArgb(50, 53, 60) : Color.FromArgb(235, 238, 242);

        public static Color ButtonDisabledBg =>
            IsDark ? Color.FromArgb(55, 58, 65) : Color.FromArgb(230, 232, 236);

        public static Color ButtonDisabledText =>
            IsDark ? Color.FromArgb(100, 105, 115) : Color.FromArgb(170, 175, 182);

        public static Color ToggleTrackOff =>
            IsDark ? Color.FromArgb(80, 85, 95) : Color.FromArgb(170, 175, 182);

        public static Color ToggleTrackDisabled =>
            IsDark ? Color.FromArgb(55, 58, 65) : Color.FromArgb(210, 212, 215);

        public static Color ToggleKnob =>
            IsDark ? Color.FromArgb(220, 222, 226) : Color.White;

        // ── Shared layout constants ───────────────────────────────────
        public const int LabelHeight      = 20;
        public const int LabelGap         = 4;
        public const int InputBorderRadius = 8;
        public const int InputPadH        = 10;

        // ── Legacy aliases (kept for backwards compat) ────────────────
        public static Color BodyColor      => PrimaryColor;
        public static Color BorderColor    => SurfaceBorderColor;
    }
}
