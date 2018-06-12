using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WebView.Interop
{
    public sealed class WebViewPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var appWebView = new Windows.UI.Xaml.Controls.WebView();
            Content = appWebView;

            WebUIApplication.Bind(Application.Current, appWebView);
            appWebView.Navigate(e.Parameter as Uri);
        }
    }
}
