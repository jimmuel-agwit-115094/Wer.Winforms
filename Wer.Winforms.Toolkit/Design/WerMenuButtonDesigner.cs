using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Wer.Winforms.Toolkit.Controls;

namespace Wer.Winforms.Toolkit.Design
{
    /// <summary>
    /// Designer for WerMenuButton — adds "Move Up" and "Move Down" verbs
    /// to reorder menu buttons inside WerLeftNavMenu at design time.
    /// </summary>
    internal class WerMenuButtonDesigner : ControlDesigner
    {
        private DesignerVerbCollection _verbs;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection
                    {
                        new DesignerVerb("Move Up", (s, e) => MoveButton(-1)),
                        new DesignerVerb("Move Down", (s, e) => MoveButton(1)),
                    };
                }
                return _verbs;
            }
        }

        private void MoveButton(int direction)
        {
            var btn = Control as WerMenuButton;
            if (btn == null) return;

            var parent = btn.Parent;
            if (parent == null) return;

            // With Dock=Top, higher child index = visually higher (top).
            // "Move Up" visually = increase child index.
            // "Move Down" visually = decrease child index.
            int currentIndex = parent.Controls.GetChildIndex(btn);
            int newIndex = currentIndex - direction; // invert: Dock=Top layout is reversed

            if (newIndex < 0 || newIndex >= parent.Controls.Count) return;

            // Use IDesignerHost transaction so the change is undoable and serialized
            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (host == null) return;

            using (var txn = host.CreateTransaction("Move Menu Button"))
            {
                // Notify the designer that the parent's Controls collection is changing
                var changeService = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                var prop = TypeDescriptor.GetProperties(parent)["Controls"];

                changeService?.OnComponentChanging(parent, prop);

                parent.Controls.SetChildIndex(btn, newIndex);

                changeService?.OnComponentChanged(parent, prop, null, null);

                txn.Commit();
            }
        }
    }
}
