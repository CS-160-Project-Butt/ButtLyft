'use strict';
app.factory('driverSignalService', ['authService','$', function (authService, $) {

    var connection = null;
    var hub = null;
    var driverUser = null;
    var foreignRiderUser = null;
    var availableRiders = [];
    var riderInfo = {};

    var selfService = {
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
            hub.on('receiveBoardCastConfirmSignal', function (rider, geocoords) {
                var found = false;
                console.log(rider + " is at " + angular.fromJson(geocoords));
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
                    availableRiders.push(temp);

                    console.log(availableRiders);
                }
            });
            hub.on('collectRiderAgreementSignal', function (rider, driver) {
                if (foreignRiderUser == null && driver == driverUser) {
                    foreignRiderUser = rider;
                    riderInfo.name = rider;
                    angular.forEach(availableRiders, function (value, key) {
                        if (value.name = rider) {
                            riderInfo.location = value.location;
                        }
                    })
                    console.log(riderInfo);
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
        },
        getRiders: function () {
            return availableRiders;
        },
        getRiderInfo: function () {
            return riderInfo;
        },
        queryRider: function (riderName, drivercoords) {
            hub.invoke('queryRider', driverUser, riderName, angular.toJson(drivercoords));
        }

    //    addNote: function (note) { //invoking a method with data
    //    hub.invoke('addNote', note);
    //},
    }

    return selfService;
}]);