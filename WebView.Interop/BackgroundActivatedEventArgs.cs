using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace WebView.Interop
{
    /// <summary>
    /// For whatever reason, BackgroundActivatedEventArgs doesn't extend IActivatedEventArgs like 
    /// all the other EventArgs classes in Windows.ApplicaitonModel.Activation.
    /// 
    /// This class mushes activation args with the BackgroundActivatedEventArgs for use during app activation in JavaScript.
    /// </summary>
    public sealed class BackgroundActivatedEventArgs : IActivatedEventArgs, IBackgroundActivatedEventArgs
    {
        private IActivatedEventArgs _activatedEventArgs;
        private IBackgroundActivatedEventArgs _backgroundActivatedEventArgs;

        // IActivatedEventArgs members
        public ActivationKind Kind => _activatedEventArgs.Kind;
        public ApplicationExecutionState PreviousExecutionState => _activatedEventArgs.PreviousExecutionState;
        public SplashScreen SplashScreen => _activatedEventArgs.SplashScreen;

        // IBackgroundActivatedEventArgs members
        public IBackgroundTaskInstance TaskInstance => _backgroundActivatedEventArgs.TaskInstance;

        public BackgroundActivatedEventArgs(IActivatedEventArgs activatedEventArgs, IBackgroundActivatedEventArgs backgroundActivatedEventArgs)
        {
            _activatedEventArgs = activatedEventArgs;
            _backgroundActivatedEventArgs = backgroundActivatedEventArgs;
        }
    }
}
