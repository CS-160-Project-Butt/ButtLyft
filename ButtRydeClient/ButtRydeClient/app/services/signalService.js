'use strict';
app.factory('signalService', ['$', function ($) {

    var hub = null;
    var proxy;
    var connection;

    return {
        initialize: function () {
            connection = $.hubConnection();

            this.hub = connection.createHubProxy('dataHub');

            this.hub.on('onHit', function (data) {
                console.log(data);
            })

        }
    }


}]);