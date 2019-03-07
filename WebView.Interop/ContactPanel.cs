using System;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;
using Windows.UI;

namespace WebView.Interop
{
    [AllowForWeb]
    public sealed class ContactPanel
    {
        private Windows.ApplicationModel.Contacts.ContactPanel _contactPanel;

        public event EventHandler<Object> Closing;
        public event EventHandler<Object> LaunchFullAppRequested;

        public Color? HeaderColor
        {
            get => _contactPanel.HeaderColor;
            set => _contactPanel.HeaderColor = value;
        }

        public ContactPanel(Windows.ApplicationModel.Contacts.ContactPanel contactPanel)
        {
            _contactPanel = contactPanel;
            _contactPanel.LaunchFullAppRequested += ContactPanel_LaunchFullAppRequested;
            _contactPanel.Closing += ContactPanel_Closing;
        }

        public void ClosePanel() => _contactPanel.ClosePanel();

        private void ContactPanel_LaunchFullAppRequested(Windows.ApplicationModel.Contacts.ContactPanel sender, ContactPanelLaunchFullAppRequestedEventArgs args)
        {
            EventDispatcher.Dispatch(() => LaunchFullAppRequested?.Invoke(sender, args));
        }

        private void ContactPanel_Closing(Windows.ApplicationModel.Contacts.ContactPanel sender, ContactPanelClosingEventArgs args)
        {
            EventDispatcher.Dispatch(() => Closing?.Invoke(sender, args));
        }
    }
}
