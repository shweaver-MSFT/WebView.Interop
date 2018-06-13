using Windows.Foundation.Metadata;
using Windows.UI.WebUI;
using Windows.UI.Xaml.Controls;

namespace WebView.Interop
{
    [AllowForWeb]
    public sealed class WebUINavigatedEventArgs: IWebUINavigatedEventArgs
    {
        public WebUINavigatedEventArgs(WebViewNavigationCompletedEventArgs args)
        {
            // TODO: Figure out how to create the NavigatedOperation
        }

        public WebUINavigatedOperation NavigatedOperation => throw new System.NotImplementedException();
    }
}
