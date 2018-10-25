(function () {

    var logger = document.logger;
    var app = document.app;

    var ActivationService = function () {

        // Handle app launch activation
        function onLaunched(args) {
            logger.Log("Launch activated");

            var activation = Windows.ApplicationModel.Activation;

            if (args && (args.previousExecutionState === activation.ApplicationExecutionState.terminated || args.previousExecutionState === activation.ApplicationExecutionState.closedByUser)) {
                // TODO: Populate the UI with the previously saved application data
            }
            else {
                // TODO: Populate the UI with defaults
            }
        }

        // Handle protocol activation
        function onProtocolActivated(args) {
            logger.Log("Protocol Activated");

            var protocol = args.Uri.absoluteUri;
            // TODO: Handle protocol activation
        }

        // Handle shareTarget activation
        function onShareTargetActivated(args) {
            logger.Log("ShareTarget Activated");
            // TODO: Handle shareTarget activation
        }

        // When your application is activated with this contract, it will receive a ContactPanelActivatedEventArgs object. 
        // This contains the ID of the Contact that your application is trying to interact with on launch, and a ContactPanel object.
        // You should keep a reference to this ContactPanel object, which will allow you to interact with the panel.
        // https://docs.microsoft.com/en-us/windows/uwp/contacts-and-calendar/my-people-support#running-in-the-contact-panel
        function onContactPanelActivated(args) {
            logger.Log("ContectPanel Activated");

            var contactPanel = args.contactPanel;
        }

        // Handle a web activation
        function onWebActivated(args) {
            logger.Log("Web Activated");
        }

        // The most recent activation args
        this.LastActivationArgs;

        // Handles app activation
        this.OnActivated = function (args) {

            this.LastActivationArgs = args;

            if (!window.Windows) {
                onWebActivated(args);
                return;
            }

            if (args && args.kind !== undefined) {

                var activation = Windows.ApplicationModel.Activation;

                switch (args.kind) {
                    case activation.ActivationKind.launch: onLaunched(args); return;
                    case activation.ActivationKind.protocol: onProtocolActivated(args); return;
                    case activation.ActivationKind.shareTarget: onShareTargetActivated(args); return;
                    case activation.ActivationKind.contactPanel: onContactPanelActivated(args); return;
                }
            }

            logger.Log("Unhandled activation: Missing args - " + args);
            // TODO: Handle activation with invalid/missing args

        }.bind(this);
    };

    var activationService = new ActivationService();
    document.activation = activationService;

    // Register for activation
    if (window.Windows && window.Windows.UI && window.Windows.UI.WebUI) {
        try {
            // Native JavaScript or Progressive Web Application (WWAHost)
            // Registering for activation will throw an exception outside of WWAHost
            Windows.UI.WebUI.WebUIApplication.onactivated = activationService.OnActivated;
        }
        catch (e) {
            // Hybrid Web Application (WebView)
            // Register for activation on injected IWebUIApplication implementation
            WebUIApplication.onactivated = activationService.OnActivated;
        }
    }
    else {
        // Running on web, not UWP
        activationService.OnActivated();
    }
})();