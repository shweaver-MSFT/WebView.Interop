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

        private void ContactPanel_LaunchFullAppRequested(Windows.ApplicationModel.Contacts.ContactPanel sender, ContactPanelLaunchFullAppRequestedEventArgs args)
        {
            //System.NotSupportedException: 'Exception from HRESULT: 0x800A01B6'
            EventDispatcher.Dispatch(() => LaunchFullAppRequested?.Invoke(sender, args));

            /*
            System.Runtime.InteropServices.COMException
              HResult=0x8000001F
              Message=A COM call to an ASTA was blocked because the call chain originated in or passed through another ASTA. This call pattern is deadlock-prone and disallowed by apartment call control.

            A COM call (IID: {C50898F6-C536-5F47-8583-8B2C2438A13B}, method index: 3) to an ASTA (thread 24048) was blocked because the call chain originated in or passed through another ASTA (thread 15312). This call pattern is deadlock-prone and disallowed by apartment call control.
              Source=<Cannot evaluate the exception source>
              StackTrace:
            <Cannot evaluate the exception stack trace>
             */
            LaunchFullAppRequested?.Invoke(sender, args);
        }

        private void ContactPanel_Closing(Windows.ApplicationModel.Contacts.ContactPanel sender, ContactPanelClosingEventArgs args)
        {
            EventDispatcher.Dispatch(() => Closing?.Invoke(sender, args));
        }

        public void ClosePanel()
        {
            _contactPanel.ClosePanel();
        }
    }
}
