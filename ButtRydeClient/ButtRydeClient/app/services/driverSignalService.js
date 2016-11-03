'use strict';
app.factory('driverSignalService', ['authService','$', function (authService, $) {

    var connection = null;
    var hub = null;
    var driverUser = null;
    var foreignRiderUser = null;
    var availableRiders = [];

    return {
        initialize: function () {
            driverUser = authService.authentication.userName;
            connection = $.hubConnection('http://localhost:1272/');
            hub = connection.createHubProxy('dataHub');
            connection.start();

            hub.on('onHit', function (data) {
                console.log(data);
            });
            hub.on('currentLocation', function (data) {
                console.log(data);
            });
            hub.on('receivePickMeUpSignal', function (rider, geocoords) {
                var found = false;

                angular.forEach(availableRiders, function (value, key) {
                    if (value.name == rider) {
                        value.name = rider;
                        value.location = angular.fromJson(geocoords);
                        found = true;
                    }
                });

                if (!found) {
                    var temp = {
                        name: rider,
                        location: angular.fromJson(geocoords)
                    }
                    drivers.push(temp);
                }
            });

        },
        hit: function () {
            hub.invoke('hit');
        },
        sendLocation: function(){
            hub.invoke('sendLocation');
        },
        broadcastLocation: function (geocoords) {
            hub.invoke('driverBroadcastLocation', driverUser, angular.toJson(geocoords));
        }
    //    addNote: function (note) { //invoking a method with data
    //    hub.invoke('addNote', note);
    //},
    }


}]);