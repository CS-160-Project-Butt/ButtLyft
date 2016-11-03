'use strict';
app.controller('driverCtrl', ['$scope', '$interval', '$timeout', '$location', 'authService', 'driverSignalService', 'NgMap', function ($scope, $interval, $timeout, $location, authService, driverSignalService, NgMap) {
    var vm = this;

    driverSignalService.initialize(); //inits the signalservice factory
    vm.startAddress = [0, 0]; //this is used to store the location of the rider's pickup point
    vm.endAddress = [0, 0]; // this is the location of the rider's destination
    vm.inputAddress = null; //user types in the textbox and queries where to go, then hits enter or presses search
   // vm.mapCenter = [0, 0]; //position of the center of the map.
    vm.centerMarker = null;
    vm.dragging = false; //bool: if user is done dragging the map, expand the marker
    vm.riders = driverSignalService.getRiders();
    vm.riderInfo = driverSignalService.getRiderInfo();
    vm.driverStartLocation = [0,0];

    vm.riderLocation = [0, 0];

    vm.placeChanged = function () {
        vm.place = this.getPlace();
        console.log('location', vm.place.geometry.location);
        vm.map.setCenter(vm.place.geometry.location);
    }

    vm.init = function () {
        navigator.geolocation.getCurrentPosition(function (position) {

            //vm.mapCenter[0] = position.coords.latitude;
            //vm.mapCenter[1] = position.coords.longitude;

            NgMap.getMap('rider').then(function (map) {
                vm.map = map; //we get the map
                vm.initBool = false;
                //console.log(vm.map); //checking out the map
                //console.log('markers', map.markers);
                //console.log('shapes', map.shapes);

                vm.onCenterChanged();//initialize the position of the center marker
                vm.onDragEnd();

            });
        }, function (err) { });
    }
    vm.init();

    /**
     * when we are done dragging the map or if idle we have to expand the little box thing
     */
    vm.onDragEnd = function () {
        vm.dragging = false;
    }

    /**
     * this function is usually called when the user drags on the map
     * it will sync the location of the center marker to the map screen center
     * parameter: center, usually null, if not null then we will set the pos of marker to the argument(&center) instead
     */
    vm.onCenterChanged = function (center) {

        vm.dragging = true;
        var temp = [0, 0];
        temp[0] = vm.map.getCenter().lat();
        temp[1] = vm.map.getCenter().lng();
        if (center != null) temp = center
        if (temp[0] != null && temp[1] != null) vm.centerMarker = temp;

    }

    /**
    * Get the current location of the driver and broadcast it to all the riders every x seconds.
    */

    vm.stop;
    vm.signalInterval = function() {
        if ( angular.isDefined(vm.stop) ) return;

        vm.stop = $interval(function () {
            driverSignalService.broadcastLocation(vm.centerMarker);
            
        }, 100);
    };
    vm.signalInterval();

    vm.stopInterval = function() {
        if (angular.isDefined(stop)) {
            $interval.cancel(stop);
            stop = undefined;
        }
    };

    $scope.$on('$destroy', function() {
        // Make sure that the interval is destroyed too
        vm.stopInterval();
    });

    vm.displayRiderInfo = false;
    vm.driverOnRoute = false;
    vm.driverRiderSync = false;
    vm.riderPickedUp = false;
    vm.rider = {};
    vm.displayRider = function (data, name, location) {
        vm.displayRiderInfo = true;
        vm.rider.name = name;
        vm.rider.location = location;
        console.log(vm.rider);
        if (data != 'data') {
            $scope.$apply();
        }
    }
    vm.origin = null;
    vm.destination = null;
    vm.acceptRider = function (rider) {
        vm.driverOnRoute = true;
        vm.displayRiderInfo = false;
        driverSignalService.queryRider(rider, vm.centerMarker);
        vm.origin = angular.copy(vm.centerMarker);
        
        console.log(vm.origin)

        var temp = angular.copy(vm.centerMarker);
        $timeout(function () {
            vm.location = vm.riderInfo.location;


            $timeout(function () {


                vm.onCenterChanged(temp);
                vm.map.setCenter({ lat: temp[0], lng: temp[1] })
            }, 500);
        }, 1000);
    }

    vm.declineRider = function () {
        vm.displayRiderInfo = false;
    }

    vm.calcDistance = function (x,y) {
        if (x == null || y == null) {
            return null
        }
        var xsq = (x[1] - y[1]) * (x[1] - y[1]);
        var ysq = (x[0] - y[0]) * (x[0] - y[0]);
        var result = Math.sqrt(xsq + ysq);
        return result;
    }

    vm.pickupRider = function () {
        vm.riderPickedUp = true;
        var temp = angular.copy(vm.centerMarker);
        driverSignalService.queryRider(rider, vm.centerMarker);
        $timeout(function () {

            vm.driverStartLocation = angular.copy(vm.centerMarker);
            vm.location = vm.riderInfo.destination;
            $timeout(function () {

                vm.onCenterChanged(temp);
                vm.map.setCenter({ lat: temp[0], lng: temp[1] })
            }, 500);
        }, 1000);

        driverSignalService.pickupRider();
        vm.origin = angular.copy(vm.centerMarker)
        vm.driverRiderSync = true;
    }

    vm.dropOffRider = function () {

        $location.path('/userDetails');
        driverSignalService.dropOffRider();

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