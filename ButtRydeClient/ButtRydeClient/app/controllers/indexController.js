'use strict';
app.controller('indexController', ['$q', '$timeout', '$http', '$scope', '$rootScope', '$location', 'authService',
    function ($q, $timeout, $http, $scope, $rootScope, $location, authService) {

        $scope.notf1Options = {
            position: {
                pinned: true,
            },
            width: "50%",
            autoHideAfter: 0,
            stacking: 'top',
            templates: [{
                type: "error",
                template: $("#errorTemplate").html()
            }, {
                type: "info",
                template: $("#emailTemplate").html()
            }, {
                type: "upload-success",
                template: $("#successTemplate").html()
            }]
        };        

        function showMessage(title, ngMessage, templateType) {
            var d = new Date();
            var date = kendo.toString(d, 'HH:MM:ss.') + kendo.toString(d.getMilliseconds(), "000");

            var notification = $("#popupNotification").kendoNotification({
                position: {
                    pinned: true,
                },
                width: "50%",
                autoHideAfter: 0,
                stacking: 'top',
                templates: [{
                    type: "error",
                    template: $("#errorTemplate").html()
                }, {
                    type: "info",
                    template: $("#emailTemplate").html()
                }, {
                    type: "upload-success",
                    template: $("#successTemplate").html()
                }]
            }).data("kendoNotification");
            if (notification) {
                notification.show({
                    title: title,
                    time: date,
                    message: ngMessage.message,
                    kValue: date
                }, templateType);
                for (var i = 0; i < ngMessage.messageDetails.length; i++) {
                    notification.error({
                        title: title,
                        time: date,
                        message: $rootScope.ngMessage.messageDetails[i],
                        kValue: date
                    }, templateType);
                }
            }
        }

        $scope.$on('ngMessage', function (event, data) {
            if (data !== undefined) {
                $scope.ngMessage = data.message;
            }

            if (data !== undefined && data.message !== '') {
                if (data.messageType === 'error') {
                    showMessage('Error', data, 'error');
                }
                if (data.messageType === 'info') {
                    showMessage('Info', data, 'info');
                }
                if (data.messageType === 'upload-success') {
                    showMessage('Success', data, 'upload-success');
                }
                if (data.messageType === 'authorization') {
                    showMessage('No Authorization', data, 'error');
                }

                authService.logOut();
                //$location.path('/login');
                $timeout(function () {
                    $location.path('/login');
                }, 0);
                $rootScope.ngMessage = undefined
            }
        });        

        $scope.logOut = function () {
            $rootScope.ngMessage = undefined;
            authService.logOut();
            $location.path('/home');
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
            //$scope.roles = resRoles.data;
            //alert('roles got!');
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