﻿'use strict';
app.controller('ThanksCtrl', ['$scope', 'authService', function ($scope, authService) {
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
}]);