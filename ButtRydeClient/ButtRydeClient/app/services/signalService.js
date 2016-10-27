'use strict';
app.factory('signalService', ['$', function ($) {

    var connection = null;
    var hub = null;

    return {
        initialize: function () {
            connection = $.hubConnection('http://localhost:1272/');

            hub = connection.createHubProxy('dataHub');
            connection.start();
            hub.on('onHit', function (data) {
                console.log(data);
            })

        },
        hit: function () {
                hub.invoke('hit');
       
        }
    //    addNote: function (note) { //invoking a method with data
    //    hub.invoke('addNote', note);
    //},
    }


}]);