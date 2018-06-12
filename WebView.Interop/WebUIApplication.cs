﻿using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Core;
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
        public event EventHandler<IActivatedEventArgs> Activated;

        // Occurs when the app has begins running in the background (no UI is shown for the app).
        public event EventHandler<IEnteredBackgroundEventArgs> EnteredBackground;

        // Occurs when the app is about to leave the background and before the app's UI is shown.
        public event EventHandler<ILeavingBackgroundEventArgs> LeavingBackground;

        // Occurs when the app is navigating.
        public event EventHandler<IWebUINavigatedEventArgs> Navigated;

        // Occurs when the app is resuming.
        public event EventHandler<Object> Resuming;

        // Occurs when the app is suspending.
        public event EventHandler<ISuspendingEventArgs> Suspending;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnActivated(IActivatedEventArgs e)
        {
            Activated?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnNavigated(IWebUINavigatedEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

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
        private CoreDispatcher m_dispatcher;

        private WebUIApplication(Application app, Windows.UI.Xaml.Controls.WebView webView)
        {
            var window = CoreWindow.GetForCurrentThread();
            m_dispatcher = window.Dispatcher;

            _app = app;
            _webView = webView;

            _app.EnteredBackground += App_EnteredBackground;
            _app.LeavingBackground += App_LeavingBackground;
            _app.Resuming += App_Resuming;
            _app.Suspending += App_Suspending;
            _app.UnhandledException += App_UnhandledException;

            _webView.NavigationStarting += WebView_NavigationStarting;
            _webView.NavigationCompleted += _webView_NavigationCompleted;


        }

        private void _webView_NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            Task.Run(async () =>
            {
                await m_dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    Navigated?.Invoke(this, null);
                }));
            });
        }

        private void WebView_NavigationStarting(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationStartingEventArgs e)
        {
            sender.AddWebAllowedObject("WebUIApplication", this);
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            Task.Run(async () =>
            {
                await m_dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => 
                {
                    Suspending?.Invoke(this, e);
                }));
            });
        }

        private void App_Resuming(object sender, object e)
        {
            Task.Run(async () =>
            {
                await m_dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    Resuming?.Invoke(this, e);
                }));
            });
        }

        private void App_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            Task.Run(async () =>
            {
                await m_dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    LeavingBackground?.Invoke(this, e);
                }));
            });
        }

        private void App_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            Task.Run(async () =>
            {
                await m_dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    EnteredBackground?.Invoke(this, e);
                }));
            });
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
    }
}