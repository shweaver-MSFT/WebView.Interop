using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace WebView.Interop
{
    internal static class EventDispatcher
    {
        private static CoreDispatcher _dispatcher => CoreWindow.GetForCurrentThread()?.Dispatcher;

        public static async void Dispatch(Action action)
        {
            // already in UI thread:
            if (_dispatcher == null || _dispatcher.HasThreadAccess)
            {
                await Task.Run(action);
            }
            // not in UI thread, ensuring UI thread:
            else
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            }
        }
    }
}
