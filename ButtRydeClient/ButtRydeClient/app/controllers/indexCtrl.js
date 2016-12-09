'use strict';
app.controller('indexCtrl', ['$scope','$route','$location', 'authService',
function ($scope,$route, $location, authService) {

    $scope.logOut = function () {
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

        $scope.roles = [];

        $scope.authentication = authService.authentication;

        authService.getUserRoles().then(function (resRoles) {
            $scope.roles = resRoles.data;
//            console.log($scope.roles);
        });

        $scope.isUserInRoles = function (roles) {
            var authorizedRoles = [];
            if (!angular.isArray(roles)) {
                authorizedRoles = [roles];
            } else {
                authorizedRoles = roles;
            }
            var result = containsAny($scope.authentication.roles, authorizedRoles);
            return result;
        };

        $scope.loc = function () {
            return $location.path();
        };

    }]);