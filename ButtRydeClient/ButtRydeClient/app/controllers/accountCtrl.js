'use strict';
app.controller('accountCtrl', ['$location', 'authService', 'accountService', function ($location, authService, accountService) {
    var vm = this;


    vm.typedThing = 0;
    vm.accountBalance
    vm.message = "";


    vm.deposit = function (number) {



        if (accountService.deposit(number) == true)
        {
            vm.message = "You have deposited $" + number + "!"
            vm.updateBalance()
        }
        else
            vm.message = "Your deposit was denied!"
    }


    vm.withdraw = function (number) {
        if (accountService.withdraw(number) == true) {
            vm.message = "You have withdrawn $" + number + "!"
            vm.updateBalance();
        }


        else
            vm.message = "Your withdrawal was denied!"
    }


    vm.updateBalance = function () {
        vm.accountBalance = accountService.getBalance()

        setTimeout(function () {
            $("div.alert").fadeTo(500, 0).slideUp(500, function () {
                $(this).remove();
            });
        }, 3000);
    }
    vm.updateBalance();










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
//        console.log(users);
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


