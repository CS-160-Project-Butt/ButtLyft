'use strict';
app.factory('signalService', ['$', function ($) {

    var hub = null;

    return {
        initialize: function () {
            connection = $.hubConnection("/dataHub", { useDefaultPath: false });

            this.hub = connection.createHubProxy('dataHub');

            this.hub.on('onHit', function (data) {
                console.log(data);
            })

        }
    }


}]);