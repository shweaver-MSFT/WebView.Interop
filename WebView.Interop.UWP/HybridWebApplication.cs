using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace WebView.Interop.UWP
{
    public class HybridWebApplication : Application
    {
        private readonly Uri _source;
        private WebUIApplication _webUIApplication;

        public HybridWebApplication(Uri source)
        {
            _source = source;
            _webUIApplication = new WebUIApplication(this);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            _webUIApplication.Launch(_source, e);
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            _webUIApplication.OnActivated(e);
        }
    }
}
