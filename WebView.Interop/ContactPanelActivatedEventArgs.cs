using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;

namespace WebView.Interop
{
    [AllowForWeb]
    public sealed class ContactPanelActivatedEventArgs : IActivatedEventArgs
    {
        public Contact Contact { get; }
        public ContactPanel ContactPanel { get; }

        public ActivationKind Kind { get; }
        public ApplicationExecutionState PreviousExecutionState { get; }
        public SplashScreen SplashScreen { get; }

        public ContactPanelActivatedEventArgs(IActivatedEventArgs args)
        {
            var contactPanelArgs = args as Windows.ApplicationModel.Activation.ContactPanelActivatedEventArgs;
            Contact = contactPanelArgs.Contact;
            ContactPanel = new ContactPanel(contactPanelArgs.ContactPanel);

            Kind = args.Kind;
            PreviousExecutionState = args.PreviousExecutionState;
            SplashScreen = args.SplashScreen;
        }
    }
}
