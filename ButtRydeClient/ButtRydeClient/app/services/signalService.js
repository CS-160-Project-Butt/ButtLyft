'use strict';
app.factory('signalService', ['$', function ($) {

    var connection = null;
    var hub = null;
    var currentUser = null;
    var drivers = [];
    return {
        initialize: function () {
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
        }
    //    addNote: function (note) { //invoking a method with data
    //    hub.invoke('addNote', note);
    //},
    }


}]);