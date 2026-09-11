using System.Drawing;
using System.Windows.Forms.Design;

namespace Wer.Winforms.Toolkit.Design
{
    /// <summary>
    /// Designer for WerTabControl.
    /// - Makes the control a container so controls can be dropped into tab pages.
    /// - Passes tab-strip clicks through to the control so clicking a tab in
    ///   the designer switches the active page (allowing you to drop controls
    ///   into each tab separately).
    /// </summary>
    internal class WerTabControlDesigner : ParentControlDesigner
    {
        private const int StripHeight = 42; // must match WerTabControl.StripHeight

        protected override bool GetHitTest(Point point)
        {
            // Convert screen point to control-local coordinates
            var local = Control.PointToClient(point);

            // Forward clicks inside the tab strip to the control itself
            // so WerTabControl.OnMouseDown fires and switches SelectedIndex.
            return local.Y >= 0 && local.Y <= StripHeight;
        }
    }
}
