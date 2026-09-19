using System;
using System.ComponentModel;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// One entry in a WerMenuButton sub-menu.
    /// Text is configurable in the VS Collection Editor.
    /// Action is set in code (e.g. Form_Load).
    ///
    /// Example:
    ///   werMenuButton1.SubItems.Add(new SubMenuItem("Users",
    ///       () => werLeftNavMenu1.ShowForm&lt;UsersForm&gt;("Users")));
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SubMenuItem
    {
        public SubMenuItem() { }

        public SubMenuItem(string text, Action action = null)
        {
            Text   = text;
            Action = action;
        }

        [Category("SubMenuItem")]
        [DefaultValue("Sub Item")]
        public string Text { get; set; } = "Sub Item";

        /// <summary>Invoked when the sub-item is clicked. Set in code.</summary>
        [Browsable(false)]
        public Action Action { get; set; }

        public override string ToString() =>
            string.IsNullOrWhiteSpace(Text) ? "Sub Item" : Text;
    }
}
