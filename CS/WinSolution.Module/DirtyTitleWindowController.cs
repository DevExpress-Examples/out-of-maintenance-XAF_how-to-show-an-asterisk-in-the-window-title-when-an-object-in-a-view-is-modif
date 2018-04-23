using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace WinSolution.Module {
    public class DirtyTitleWindowController : WindowController {
        public const char DirtyMark = '*';
        private WindowTemplateController windowTemplateController = null;
        protected override void OnActivated() {
            base.OnActivated();
            Window.ViewChanged += Window_ViewChanged;
            windowTemplateController = Window.GetController<WindowTemplateController>();
            windowTemplateController.CustomizeWindowCaption += windowTemplateController_CustomizeWindowCaption;
        }
        void Window_ViewChanged(object sender, ViewChangedEventArgs e) {
            if(Window.View is ObjectView) {
                Window.View.ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
                Window.View.ObjectSpace.Reloaded += ObjectSpace_Reloaded;
            }
        }
        void ObjectSpace_Reloaded(object sender, EventArgs e) {
            UpdateCaption();   
        }
        protected virtual void UpdateCaption() {
            if(windowTemplateController != null) {
                windowTemplateController.UpdateWindowCaption();
            }
        }
        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e) {
            if(((IObjectSpace)sender).IsModified) {
                UpdateCaption();
            }
        }
        private void windowTemplateController_CustomizeWindowCaption(object sender, CustomizeWindowCaptionEventArgs e) {
            if(Window.View != null) {
                e.WindowCaption.FirstPart = e.WindowCaption.FirstPart.TrimStart(DirtyMark);
                if(Window.View.ObjectSpace.IsModified) {
                    e.WindowCaption.FirstPart = String.Format("{0} {1}", DirtyMark, e.WindowCaption.FirstPart);
                }
            }
        }
        protected override void OnDeactivated() {
            Window.ViewChanged -= Window_ViewChanged;
            if(Window.View is ObjectView) {
                windowTemplateController.CustomizeWindowCaption -= windowTemplateController_CustomizeWindowCaption;
                Window.View.ObjectSpace.ModifiedChanged -= ObjectSpace_ModifiedChanged;
                Window.View.ObjectSpace.Reloaded -= ObjectSpace_Reloaded;
                windowTemplateController = null;
            }
            base.OnDeactivated();
        }
    }
}