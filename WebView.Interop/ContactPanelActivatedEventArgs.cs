using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;

namespace WebView.Interop
{
    [AllowForWeb]
    public sealed class ContactPanelActivatedEventArgs : IActivatedEventArgs
    {
        private Windows.ApplicationModel.Activation.ContactPanelActivatedEventArgs _contactPanelActivatedEventArgs;

        // IActivatedEventArgs members
        public ActivationKind Kind => _contactPanelActivatedEventArgs.Kind;
        public ApplicationExecutionState PreviousExecutionState => _contactPanelActivatedEventArgs.PreviousExecutionState;
        public SplashScreen SplashScreen => _contactPanelActivatedEventArgs.SplashScreen;

        // IContactPanelActivatedEventArgs members
        public Contact Contact => _contactPanelActivatedEventArgs.Contact;
        public ContactPanel ContactPanel { get; }

        public ContactPanelActivatedEventArgs(Windows.ApplicationModel.Activation.ContactPanelActivatedEventArgs contactPanelActivatedEventArgs)
        {
            _contactPanelActivatedEventArgs = contactPanelActivatedEventArgs;
            ContactPanel = new ContactPanel(contactPanelActivatedEventArgs.ContactPanel);
        }
    }
}
