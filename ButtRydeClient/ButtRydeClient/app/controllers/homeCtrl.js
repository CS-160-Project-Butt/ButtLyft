'use strict';
app.controller('homeCtrl', ['$timeout', '$location', 'authService', 'signalService', 'NgMap', function ($timeout, $location, authService, signalService, NgMap) {
    var vm = this;

    vm.map = null; //this is the object that ngmap gives us
    vm.startAddress = [0, 0]; //this is used to store the location of the rider's pickup point
    vm.endAddress = [0, 0]; // this is the location of the rider's destination
    vm.inputAddress = null; //user types in the textbox and queries where to go, then hits enter or presses search
    vm.mapCenter = [0, 0]; //position of the center of the map.
    vm.dragging = false; //bool: if user is done dragging the map, expand the marker

    NgMap.getMap().then(function (map) { //this could be used as an initialize function
        vm.map = map; //we get the map
        console.log(vm.startAddress);
        console.log(map); //checking out the map
        console.log('markers', map.markers);
        console.log('shapes', map.shapes);
        vm.processLocation();
        vm.onCenterChanged();//initialize the position of the center marker
        vm.onDragEnd();
    });

    /**
     * Used by the html map to alter/update the map location
     * when we start program, map center initialized to geolocation
     * after that its wherever the user drags to(number array pair), or the typed in address(string)
     * after the center is changed we have to update the position of the center marker
     * parameter: location - usually is null, if not null then force map position to be &location, update center marker
     */
    vm.processLocation = function (location) {
        if (vm.mapCenter[0] == 0 && vm.mapCenter[1] == 0) {
            navigator.geolocation.getCurrentPosition(function (position) {
                vm.mapCenter[0] = position.coords.latitude;
                vm.mapCenter[1] = position.coords.longitude;

                vm.onCenterChanged();
            }, function (err) { });
        }
        if (location != null) {
            vm.mapCenter = vm.inputAddress;
            vm.inputAddress = null;

            vm.onCenterChanged(vm.mapCenter);
        }
        return vm.mapCenter;
    }

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
        if (vm.map != null) {
            var temp = [0, 0];
            temp[0] = vm.map.getCenter().lat();
            temp[1] = vm.map.getCenter().lng();
            if (center != null) temp = center
            vm.centerMarker = temp;
        }
    }

    /**
     * once we figure out where we want to get picked up from we click the button and the button calls this and does things
     */
    vm.setStartAddress = function () {
        vm.startAddress = angular.copy(vm.centerMarker);
        console.log("your starting address is" + vm.centerMarker);
    }
    vm.setEndAddress = function () {
        vm.endAddress = angular.copy(vm.centerMarker);
        console.log("your ending address is" + vm.centerMarker);
    }

    signalService.initialize();

    vm.triggerCounter = function(){
        signalService.hit();
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