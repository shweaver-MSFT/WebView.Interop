using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Xaml;

namespace WebView.Interop
{
    [AllowForWeb]
    public sealed class WebUIApplication
    {
        private bool _isInBackground = false;
        private Application _app;
        public IActivatedEventArgs LaunchArgs { get; private set; }

        // Occurs when the app is activated.
        public event EventHandler<Object> Activated;

        // Occurs when the app has begins running in the background (no UI is shown for the app).
        public event EventHandler<Object> EnteredBackground;

        // Occurs when the app is about to leave the background and before the app's UI is shown.
        public event EventHandler<Object> LeavingBackground;

        // Occurs when the app is resuming.
        public event EventHandler<Object> Resuming;

        // Occurs when the app is suspending.
        public event EventHandler<Object> Suspending;

        // Occurs when the app creates a new window.
        public event EventHandler<Object> WindowCreated;

        public WebUIApplication(Application app)
        {
            _app = app;

            _app.EnteredBackground += App_EnteredBackground;
            _app.LeavingBackground += App_LeavingBackground;
            _app.Resuming += App_Resuming;
            _app.Suspending += App_Suspending;
            _app.UnhandledException += App_UnhandledException;
        }

        ~WebUIApplication()
        {
            LaunchArgs = null;

            if (Window.Current.Content != null)
            {
                Window.Current.Content = null;
            }

            if (_app != null)
            {
                _app.EnteredBackground -= App_EnteredBackground;
                _app.LeavingBackground -= App_LeavingBackground;
                _app.Resuming -= App_Resuming;
                _app.Suspending -= App_Suspending;
                _app.UnhandledException -= App_UnhandledException;
                _app = null;
            }
        }

        private WebViewPage CreateWebViewPage(Uri sourceUri, IActivatedEventArgs args)
        {
            var webViewPage = new WebViewPage(this, sourceUri, args);
            return webViewPage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        [DefaultOverload]
        public void Launch(Uri source, IActivatedEventArgs e)
        {
            if (e != null)
            {
                LaunchArgs = e;
            }

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is WebViewPage))
            {
                Window.Current.Content = CreateWebViewPage(source, LaunchArgs);
            }

            if (!(e is IPrelaunchActivatedEventArgs) ||
                e is IPrelaunchActivatedEventArgs && (e as IPrelaunchActivatedEventArgs).PrelaunchActivated == false)
            {
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void Launch(Uri source, ContactPanelActivatedEventArgs e)
        {
            Window.Current.Content = CreateWebViewPage(source, e);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Activation handlers
        /// </summary>
        /// <param name="e"></param>
        public void Activate(IActivatedEventArgs e) => EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));

        public void BackgroundActivate(Windows.ApplicationModel.Activation.BackgroundActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, new BackgroundActivatedEventArgs(LaunchArgs, e)));
        }

        public void CachedFileUpdaterActivate(CachedFileUpdaterActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));
        }

        public void FileActivate(FileActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));
        }

        public void FileOpenPickerActivate(FileOpenPickerActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));
        }

        public void FileSavePickerActivate(FileSavePickerActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));
        }

        public void SearchActivate(SearchActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));
        }

        public void ShareTargetActivate(ShareTargetActivatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => Activated?.Invoke(this, e));
        }

        public void OnWindowCreated(WindowCreatedEventArgs e)
        {
            EventDispatcher.Dispatch(() => WindowCreated?.Invoke(this, e));
        }

        public void OnEnteredBackground(Windows.ApplicationModel.EnteredBackgroundEventArgs args)
        {
            _isInBackground = true;

            if (Window.Current.Content is WebViewPage webViewPage)
            {
                webViewPage.Unload();
            }
        }

        public void OnLeavingBackground(Windows.ApplicationModel.LeavingBackgroundEventArgs args)
        {
            if (_isInBackground && Window.Current.Content is WebViewPage webViewPage)
            {
                webViewPage.Load();
            }

            _isInBackground = false;
        }

        /// <summary>
        /// Enable or disable the operating system's ability to prelaunch your app.
        /// </summary>
        /// <param name="value"></param>
        public void EnablePrelaunch(bool value)
        {
            CoreApplication.EnablePrelaunch(value);
        }

        /// <summary>
        /// Restart the app.
        /// </summary>
        /// <param name="launchArguments"></param>
        /// <returns></returns>
        public IAsyncOperation<AppRestartFailureReason> RequestRestartAsync(string launchArguments)
        {
            return CoreApplication.RequestRestartAsync(launchArguments);
        }

        /// <summary>
        /// Restart the app in the context of a different user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="launchArguments"></param>
        /// <returns></returns>
        public IAsyncOperation<AppRestartFailureReason> RequestRestartForUserAsync(User user, String launchArguments)
        {
            return CoreApplication.RequestRestartForUserAsync(user, launchArguments);
        }

        #region Application event handlers
        private void App_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            EventDispatcher.Dispatch(() => EnteredBackground?.Invoke(this, e));
        }

        private void App_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            EventDispatcher.Dispatch(() => LeavingBackground?.Invoke(this, e));
        }

        private void App_Resuming(object sender, object e)
        {
            EventDispatcher.Dispatch(() => Resuming?.Invoke(this, e));
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            EventDispatcher.Dispatch(() => Suspending?.Invoke(this, e));
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

            // TODO: Fix error bubbling
            //await _webView.InvokeScriptAsync("eval", new string[] { $"throw new Error('{number}', '{description}')" });

            e.Handled = true;
        }
        #endregion Application event handlers
    }
}
