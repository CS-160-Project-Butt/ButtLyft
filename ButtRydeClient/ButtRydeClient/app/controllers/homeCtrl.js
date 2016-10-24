'use strict';
app.controller('homeCtrl', ['$timeout','$location', 'authService', 'NgMap', function ($timeout,$location, authService, NgMap) {
    var vm = this;

    vm.map = null;
    vm.startAddress = [0, 0];
    vm.inputAddress = null;
    vm.mapCenter = [0, 0];
    vm.dragging = false;

    vm.processLocation = function (location) {
        if (vm.mapCenter[0] == 0 && vm.mapCenter[1] == 0) {
            navigator.geolocation.getCurrentPosition(function (position) {
                vm.mapCenter[0] = position.coords.latitude;
                vm.mapCenter[1] = position.coords.longitude;
            }, function (err) { });
        }
        if (location != null) {
            vm.mapCenter = vm.inputAddress;
            console.log(vm.mapCenter)
            vm.inputAddress = null;
            vm.onCenterChanged(vm.mapCenter);
        }
        return vm.mapCenter;
    }

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

    vm.onDragEnd = function () {
        vm.dragging = false;
    }
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

    vm.setStartAddress = function () {
        vm.startAddress = vm.mapCenter;
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