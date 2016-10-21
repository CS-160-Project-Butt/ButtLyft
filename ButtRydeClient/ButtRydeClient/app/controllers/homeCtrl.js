'use strict';
app.controller('homeCtrl', ['$location', 'authService', 'NgMap', function ($location, authService, NgMap) {
    var vm = this;

    vm.map = null;
    vm.startAddress = [0,0];

    vm.mapCenter = [0, 0];


    //function initMap() {
    //    var map = new google.maps.Map(document.getElementById('map'), {
    //        zoom: 6,
    //        center: { lat: 20.291, lng: 153.027 },
    //        mapTypeId: 'terrain'
    //    }).then(function () {
    //        console.log("maploaded")
    //    });
    //}


    NgMap.getMap().then(function (map) { //this could be used as an initialize function
        vm.map = map;
        //vm.startAddress[0] = vm.map.getCenter().lat();
        //vm.startAddress[1] = vm.map.getCenter().lng();
        console.log(vm.startAddress);
        console.log(map);
        console.log('markers', map.markers);
        console.log('shapes', map.shapes);
        vm.onCenterChanged();//initialize center
    });

    //var lineSymbol = {
    //    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
    //};
    //var line = new google.maps.Polyline({
    //    path: [{ lat: 22.291, lng: 153.027 }, { lat: 18.291, lng: 153.027 }],
    //    icons: [{
    //        icon: lineSymbol,
    //        offset: '100%'
    //    }],
    //    map: map
    //});

    vm.reverseGeoCode = function () {


    }

    vm.onCenterChanged = function () {
        //vm.mapCenter = [null,null];
        if (vm.map != null) {
            vm.mapCenter[0] = vm.map.getCenter().lat();
            vm.mapCenter[1] = vm.map.getCenter().lng();

            //console.log(vm.mapCenter);
            vm.centerMarker = vm.mapCenter;
        }
    }


    vm.setStartAddress = function () {
        //vm.startAddress = lat + lng;
        console.log("your starting address is" + vm.mapCenter);
    }



















    function containsAny(source, target) {
        for (var i = 0; i < target.length; i++) {
            for (var j = 0; j < source.length; j++) {
                if (target[i] === source[j])
                    return true;
            }
        }
        return false;
    };

    vm.toLogin = function () {
        if (!authService.authentication.isAuth) {
            $location.path('/login');
        }
    }
    vm.toLogin();

    vm.roles = [];

    vm.authentication = authService.authentication;

    authService.getUserRoles().then(function (resRoles) {
        vm.roles = resRoles.data;
    });

    vm.allUsersList;
    authService.getAllUsers().then(function (users) {
        console.log(users);
        vm.allUsersList = users.data;

    });

    vm.isUserInRoles = function (roles) {
        var authorizedRoles = [];
        if (!angular.isArray(roles)) {
            authorizedRoles = [roles];
        } else {
            authorizedRoles = roles;
        }
        var result = containsAny($scope.authentication.roles, authorizedRoles);
        return result;
    };
}]);