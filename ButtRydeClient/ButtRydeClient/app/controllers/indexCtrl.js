'use strict';
app.controller('indexCtrl', ['$location', 'authService',
    function ($location, authService) {  
        var vm = this;
        vm.logOut = function () {
            authService.logOut();
            $location.path('/login');
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

        vm.roles = [];

        vm.authentication = authService.authentication;

        authService.getUserRoles().then(function (resRoles) {
            vm.roles = resRoles.data;
            console.log(vm.roles);
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

        vm.loc = function () {
            return $location.path();
        };

    }]);