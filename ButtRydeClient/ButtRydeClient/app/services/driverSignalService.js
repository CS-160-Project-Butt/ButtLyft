'use strict';
app.factory('driverSignalService', ['authService','$', function (authService, $) {

    var connection = null;
    var hub = null;
    var driverUser = null;

    return {
        initialize: function () {
            driverUser = authService.authentication.userName;
            connection = $.hubConnection('http://localhost:1272/');
            hub = connection.createHubProxy('dataHub');
            connection.start();

            hub.on('onHit', function (data) {
                console.log(data);
            })
            hub.on('currentLocation', function(data){
                console.log(data);
            })

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