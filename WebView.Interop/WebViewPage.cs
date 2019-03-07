using System;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebView.Interop
{
    internal class WebViewPage : Page
    {
        private readonly Uri _sourceUri = null;
        private readonly IActivatedEventArgs _activationArgs = null;
        private Windows.UI.Xaml.Controls.WebView _webView = null;
        private WebUIApplication _webApp = null;

        public WebViewPage(WebUIApplication webApp, Uri sourceUri, IActivatedEventArgs activationArgs)
        {
            _webApp = webApp;
            _sourceUri = sourceUri;
            _activationArgs = activationArgs;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        public void Load()
        {
            if (_webView == null)
            {
                _webView = CreateWebView();
                _webView.AddWebAllowedObject(_webApp.GetType().Name, _webApp);
                _webView.Source = _sourceUri;

                Content = _webView;
            }
        }

        public void Unload()
        {
            Content = null;
            UnwireWebViewDiagnostics(_webView);

            if (_webView != null)
            {
                _webView = null;
            }

            GC.Collect();
        }

        private Windows.UI.Xaml.Controls.WebView CreateWebView()
        {
            var wv = new Windows.UI.Xaml.Controls.WebView(WebViewExecutionMode.SeparateProcess);
            wv.Settings.IsJavaScriptEnabled = true;
            WireUpWebViewDiagnostics(wv);
            return wv;
        }

        private void WireUpWebViewDiagnostics(Windows.UI.Xaml.Controls.WebView webView)
        {
            webView.NavigationStarting += OnWebViewNavigationStarting;
            webView.SeparateProcessLost += OnWebViewSeparateProcessLost;
            webView.ScriptNotify += OnWebViewScriptNotify;
        }

        private void UnwireWebViewDiagnostics(Windows.UI.Xaml.Controls.WebView webView)
        {
            webView.NavigationStarting -= OnWebViewNavigationStarting;
            webView.SeparateProcessLost -= OnWebViewSeparateProcessLost;
            webView.ScriptNotify -= OnWebViewScriptNotify;
        }

        private async void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            if (sender is Windows.UI.Xaml.Controls.WebView wv)
            {
                //If you want to trigger an exteranl event without passing in a WinRT object, 
                // use window.external.notify("some string") which will call this method. The string will 
                // be accessible via e.Value. 
            }
        }

        private void OnWebViewSeparateProcessLost(Windows.UI.Xaml.Controls.WebView sender, WebViewSeparateProcessLostEventArgs args)
        {
            UnwireWebViewDiagnostics(sender);
        }

        private void OnWebViewNavigationStarting(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Debug.WriteLine(args.Uri?.ToString());
        }
    }
}
