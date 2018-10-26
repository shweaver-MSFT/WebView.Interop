# WebView.Interop for Windows Web Applications!

Progressive Web Applications are awesome! They allow a single JavaScript codebase to manifest itself as a native app on multiple platforms. This is super time and cost effective, because devs can spend less time duplicating the same feature on multiple native codebases. On Windows, we achieve this by loading the target web endpoint in a **WWAHost (Windows Web Application Host)** container. This provides the '*same*' experience as a native JavaScript application running on the platform.

So far so good. So what's the problem?

## WWAHost: The real story

You may have heard that JavaScript apps running in a WWAHost have access to the full set Windows APIs via the injected `window.Windows` object, and can behave like a first class app just like any C# equivalent because they share the same API set... 

This is a lie.

But a small one. The truth is, the WWAHost does not expose absolutely ALL that the platform has to offer. In a few cases, there are parts of the Windows Universal Platform that simply cannot be interacted with directly from JavaScript. For instance, the My People Contact Panel. Since no option exists on the platform for activating a WWAHost based app in the Contact Panel, this is seemingly a show stopper for apps wanting to leverage the feature, without also forcing them to use a different language (not JavaScript).

But you may be thinking, 

> "Hey! Why don't I just make a native XAML app with a full view WebView to load my PWA, then interop between the layers to provide support as needed?"

Those were my thoughts too...

## WWAHost != WebView

I mentioned that PWAs are run in a WWAHost, and a workaround is to use a WebView. Problem is, WWAHost and WebView are not the same thing, and will run the JavaScript app in a slightly different context. Because of this, some actions (like simply registering for activation events) in JavaScript will throw exceptions.

# Introducing WebView.Interop

WebView.Interop is an answer to this problem and consists of two primary parts:

## 1. WebView.Interop.UWP.HybridWebApplication 

HybridWebApplication extends Windows.UI.Xaml.Application and manages the eventing between the Xaml Application object (which is getting all of the lifecycle events) and the WebView. 

## 2. WebView.Interop.WebUIApplication

WebUIApplication may sound familiar. That it is because it shares a class name with the Windows.UI.WebUI.WebUIApplication [https://docs.microsoft.com/en-us/uwp/api/windows.ui.webui.webuiapplication]. It also extends the IWebUIApplication interface, giving it the same signature as well.

## How does it work?

During activation, the HybridWebApplication creates a new WebView and injects it into the window. It then creates a new WebView.Interop.WebUIApplication `[AllowForWeb]` object and injects that into the WebView. From there, the WebView is navigated to the endpoint and the JavaScript app activates within the WebView just as if it were living in a real WWAHost!

Tadaa! Now you can start integrating your PWA with ALL Windows Universal features, and even create new and exciting hybrid application types.
