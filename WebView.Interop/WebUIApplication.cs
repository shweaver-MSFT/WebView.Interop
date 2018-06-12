using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.WebUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebView.Interop
{
    [AllowForWeb]
    public sealed class WebUIApplication
    {
        /// <summary>
        /// Binds a new WebUIAppication instance to a WebView.
        /// </summary>
        /// <param name="webView"></param>
        /// <returns></returns>
        public static WebUIApplication Bind(Application app, Windows.UI.Xaml.Controls.WebView webView)
        {
            return new WebUIApplication(app, webView);
        }

        // Occurs when the app is activated.
        public event ActivatedEventHandler Activated;

        // Occurs when the app has begins running in the background (no UI is shown for the app).
        public event Windows.UI.WebUI.EnteredBackgroundEventHandler EnteredBackground;

        // Occurs when the app is about to leave the background and before the app's UI is shown.
        public event Windows.UI.WebUI.LeavingBackgroundEventHandler LeavingBackground;

        // Occurs when the app is navigating.
        public event NavigatedEventHandler Navigated;

        // Occurs when the app is resuming.
        public event ResumingEventHandler Resuming;

        // Occurs when the app is suspending.
        public event Windows.UI.WebUI.SuspendingEventHandler Suspending;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void EnablePrelaunch(bool value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="launchArguments"></param>
        /// <returns></returns>
        public IAsyncOperation<AppRestartFailureReason> RequestRestartAsync(string launchArguments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="launchArguments"></param>
        /// <returns></returns>
        public IAsyncOperation<AppRestartFailureReason> RequestRestartForUserAsync(User user, String launchArguments)
        {
            throw new NotImplementedException();
        }

        private Application _app;
        private Windows.UI.Xaml.Controls.WebView _webView;

        private WebUIApplication(Application app, Windows.UI.Xaml.Controls.WebView webView)
        {
            _app = app;
            _webView = webView;

            _app.EnteredBackground += App_EnteredBackground;
            _app.LeavingBackground += App_LeavingBackground;
            _app.Resuming += App_Resuming;
            _app.Suspending += App_Suspending;
            _app.UnhandledException += App_UnhandledException;

            _webView.NavigationStarting += WebView_NavigationStarting;
        }

        private async void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // Windows Runtime HRESULTs in the range over 0x80070000 are converted to JavaScript errors 
            // by taking the hexadecimal value of the low bits and converting it to a decimal. 
            // For example, the HRESULT 0x80070032 is converted to the decimal value 50, and the JavaScript error is SCRIPT50. 
            // The HRESULT 0x80074005 is converted to the decimal value 16389, and the JavaScript error is SCRIPT16389. 
            // https://docs.microsoft.com/en-us/scripting/javascript/reference/javascript-run-time-errors

            var hResult = e.Exception.HResult;
            var lowBits = hResult & 0xFF;
            var number = "SCRIPT" + int.Parse(Convert.ToString(lowBits), System.Globalization.NumberStyles.HexNumber);
            var description = e.Message;

            await _webView.InvokeScriptAsync("eval", new string[] { $"throw new Error('{number}', '{description}')" });
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            Suspending?.Invoke(this, e);
        }

        private void App_Resuming(object sender, object e)
        {
            Resuming?.Invoke(this);
        }

        private void App_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            LeavingBackground?.Invoke(this, e);
        }

        private void App_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            EnteredBackground?.Invoke(this, e);
        }

        private void WebView_NavigationStarting(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationStartingEventArgs args)
        {
            sender.AddWebAllowedObject("WebView.WebUIApplication", this);
        }
    }
}
