'use strict';
app.factory('signalService', ['authService','$', function (authService, $) {

    var connection = null;
    var hub = null;
    var riderUser = null;
    var foreignDriverUser = null;

    var drivers = [];
    return {
        initialize: function () {

            riderUser = authService.authentication.userName;
            connection = $.hubConnection('http://localhost:1272/');
            hub = connection.createHubProxy('dataHub');
            connection.start();

            hub.on('onHit', function (data) {
                console.log(data);
            });

            hub.on('currentMessage', function (data) {
                console.log(data);
            });

            hub.on('receiveLocation', function (driver, geocoords) {
                var found = false;

                angular.forEach(drivers, function (value, key) {
                    if (value.name == driver) {
                        value.name = driver;
                        value.location = angular.fromJson(geocoords);
                        found = true;
                    }
                });

                if (!found) {
                    var temp = {
                        name: driver,
                        location: angular.fromJson(geocoords)
                    }
                    drivers.push(temp);
                }

            });
        },
        setCurrentUser: function (user) {
            currentUser = user;
        },
        hit: function () {
            hub.invoke('hit');
        },
        sendMessage: function(message){
            hub.invoke('sendMessage', message);
        },
        getDrivers: function () {
            return drivers;
        },
        pickMeUpSignal: function (geocoords) { //driver tells everyone that it is ok for pickup
            hub.invoke('pickMeUpSignal', riderUser, angular.toJson(geocoords))
        }
    //    addNote: function (note) { //invoking a method with data
    //    hub.invoke('addNote', note);
    //},
    }


}]);