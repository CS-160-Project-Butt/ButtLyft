'use strict';
app.controller('accountCtrl', ['$location', 'authService', 'accountService', function ($location, authService, accountService) {
    var vm = this;
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

    vm.showAddition = {};
    vm.add = function (idx) {
        accountService.deposit(50)
        accountService.getBalance
        vm.showAddition[idx] = true
    };

}]);