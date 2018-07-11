(function () {

    var LogService = function () {

        // Log a simple string message
        this.Log = function (message) {
            console.log(message);
        };
    };

    var logService = new LogService();
    document.logger = logService;

})();