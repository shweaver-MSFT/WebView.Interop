using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace WebView.Interop
{
    public class HybridWebApplication : Application
    {
        private readonly Uri _source;
        private WebUIApplication webUIApplication;

        public HybridWebApplication(Uri source)
        {
            _source = source;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Windows.UI.Xaml.Controls.WebView appWebView))
            {
                appWebView = new Windows.UI.Xaml.Controls.WebView();
                webUIApplication = WebUIApplication.Bind(this, appWebView);

                Window.Current.Content = appWebView;

                appWebView.Navigate(_source);
            }

            if (e.PrelaunchActivated == false)
            {
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            webUIApplication?.OnActivated(e);
        }
    }
}
