(function () {

    var App = function () {

        this.TitleBarColor = { a: 255, r: 32, g: 32, b: 32 };
        this.TitleBarTextColor = { a: 255, r: 230, g: 230, b: 230 };

        // Initialize the TitleBar
        if (window.Windows) {
            var titleBar = Windows.UI.ViewManagement.ApplicationView.getForCurrentView().titleBar;
            titleBar.backgroundColor = this.TitleBarColor;
            titleBar.buttonBackgroundColor = this.TitleBarColor;
            titleBar.foregroundColor = this.TitleBarTextColor;
        }
    };

    var app = new App();
    document.app = app;

})();