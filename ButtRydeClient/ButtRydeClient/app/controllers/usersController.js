'use strict';
app.controller('usersController', ['$scope', '$location', 'authService', 'userService', function ($scope, $location, authService, userService) {
    
    $scope.changePasswordData = {
        oldPassword: "",
        newPassword: "",
        confirmPassword: ""
    };

    $scope.forgotPasswordBindingModel = {
        email: ""
    };

    $scope.email = '';

    $scope.message = '';

    $scope.forgotPassword = function () {

        userService.forgotPassword($scope.forgotPasswordBindingModel).then(function (response) {

            $location.path('/forgotPasswordConfirmation');

        },
        function (err) {
            $scope.message = err.data.message;
        });
    };

    $scope.resetPassword = function () {

        userService.resetPassword($scope.changePasswordData).then(function (response) {

            authService.logOut();

            $location.path('/resetPasswordConfirmation');

        },
        function (err) {
            var message = err.data.message;
            if (err.data.modelState)
            {
                $.each(err.data.modelState, function (i, item) {
                    $scope.message += item;
                });
            }
            $scope.message = message + $scope.message;
        });
    };
}]);