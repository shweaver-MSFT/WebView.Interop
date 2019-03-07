using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace WebView.Interop.UWP
{
    public class HybridWebApplication : Application
    {
        private readonly Uri _source;
        private readonly Uri _contactPanelSource;
        private readonly WebUIApplication _webUIApplication;
        private IActivatedEventArgs _activationArgs;

        public HybridWebApplication(Uri source, Uri contactPanelSource = null)
        {
            _source = source;
            _contactPanelSource = contactPanelSource;
            _webUIApplication = new WebUIApplication(this);

            EnteredBackground += HybridWebApplication_EnteredBackground;
            LeavingBackground += HybridWebApplication_LeavingBackground;
        }

        private void HybridWebApplication_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            _webUIApplication.OnEnteredBackground(e);
        }

        private void HybridWebApplication_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            _webUIApplication.OnLeavingBackground(e);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            OnActivated(e);
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            // The ContactPanel cannot be interacted with from JS. Call Launch to load the ContactPanel page in a new WebView.
            if (e.Kind == ActivationKind.ContactPanel && _contactPanelSource != null)
            {
                _webUIApplication.Launch(_contactPanelSource, new ContactPanelActivatedEventArgs(e as Windows.ApplicationModel.Activation.ContactPanelActivatedEventArgs));
                return;
            }

            _activationArgs = e;

            if (e.PreviousExecutionState != ApplicationExecutionState.Running || e.PreviousExecutionState != ApplicationExecutionState.Suspended)
            {
                _webUIApplication.Launch(_source, e);
            }
            else
            {
                _webUIApplication.Activate(e);
            }
        }

        protected void Reactivate()
        {
            _webUIApplication.Activate(_activationArgs);
        }

        protected override void OnBackgroundActivated(Windows.ApplicationModel.Activation.BackgroundActivatedEventArgs args)
        {
            _webUIApplication.BackgroundActivate(args);
            base.OnBackgroundActivated(args);
        }

        protected override void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            _webUIApplication.CachedFileUpdaterActivate(args);
            base.OnCachedFileUpdaterActivated(args);
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            _webUIApplication.FileActivate(args);
            base.OnFileActivated(args);
        }

        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            _webUIApplication.FileOpenPickerActivate(args);
            base.OnFileOpenPickerActivated(args);
        }

        protected override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            _webUIApplication.FileSavePickerActivate(args);
            base.OnFileSavePickerActivated(args);
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            _webUIApplication.SearchActivate(args);
            base.OnSearchActivated(args);
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            _webUIApplication.ShareTargetActivate(args);
            base.OnShareTargetActivated(args);
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            _webUIApplication.OnWindowCreated(args);
            base.OnWindowCreated(args);
        }
    }
}
