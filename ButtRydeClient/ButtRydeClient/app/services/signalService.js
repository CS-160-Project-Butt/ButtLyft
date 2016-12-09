'use strict';
app.factory('signalService', ['authService', '$', function (authService, $) {

    var connection = null;
    var hub = null;
    var riderUser = null;
    var foreignDriverUser = null;
    var driverConnected = false;

    var myCoords = null;
    var destinationCoords = null;
    var driverInfo = {};
    var drivers = [];

    var pickupSignal = false;

    var selfService = {
        initialize: function () {
 //           console.log('hello')
            riderUser = authService.authentication.userName;
            connection = $.hubConnection('http://localhost:1272/');
            hub = connection.createHubProxy('dataHub');
            connection.start();

            hub.on('onHit', function (data) {
//                console.log(data);
            });

            hub.on('currentMessage', function (data) {
//                console.log(data);
            });

            hub.on('receiveLocation', function (driver, geocoords) {

                if (pickupSignal == true && driver == foreignDriverUser) {
                   
                    myCoords = angular.copy(angular.fromJson(geocoords));


                } else {

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
                }

            });

            hub.on('collectDriverSignal', function (driver, rider, coords) {
                if (foreignDriverUser == null && rider == riderUser) {
                    foreignDriverUser = driver;
                    driverInfo.name = driver;
                    driverInfo.location = angular.fromJson(coords);
//                    console.log(driverInfo);
                    selfService.riderAgreementSignal();
                }
            });
            hub.on('getPickupSignal', function (me) {

//                console.log(riderUser);
//                console.log(me);
                if (riderUser == me) {
                    pickupSignal = true;
                    driverInfo.pickupSignal = true;
                    selfService.sendDestinationCoordinate();
                }
            });
            hub.on('getDropOffSignal', function (me) {

                if (riderUser == me) {
                    driverInfo.dropOffSignal = true;
                }
            });
        },
        setCurrentUser: function (user) {
            currentUser = user;
        },
        hit: function () {
            hub.invoke('hit');
        },
        sendMessage: function (message) {
            hub.invoke('sendMessage', message);
        },
        getDrivers: function () {
            return drivers;
        },
        getDriverInfo: function () {
            return driverInfo;
        },
        getMyCoords: function () {
            return myCoords;
        },
        setDestinationCoords: function (data) {
//            console.log(data)
            destinationCoords = data;
        },
        boardCastConfirmSignal: function (geocoords) { //driver tells everyone that it is ok for pickup
//            console.log(geocoords)
            hub.invoke('boardCastConfirmSignal', riderUser, angular.toJson(geocoords))
        },
        riderAgreementSignal: function () {
//            console.log(foreignDriverUser);
            hub.invoke('riderAgreementSignal', riderUser, foreignDriverUser)
        },
        sendDestinationCoordinate: function () {
            hub.invoke('broadcastDestinationCoord', foreignDriverUser, angular.toJson(destinationCoords))
        }

        //    addNote: function (note) { //invoking a method with data
        //    hub.invoke('addNote', note);
        //},
    }

    return selfService;
}]);