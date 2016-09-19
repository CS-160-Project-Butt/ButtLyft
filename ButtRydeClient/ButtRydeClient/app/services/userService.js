'use strict';
app.factory('userService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var userServiceFactory = {};

    var _forgotPassword = function (forgotPasswordBindingModel) {

        return $http.post(serviceBase + 'api/accounts/forgotPassword', forgotPasswordBindingModel).then(function (results) {
            return results;
        });
    };
    
    var _resetPassword = function (changePasswordData) {
        return $http.post(serviceBase + 'api/accounts/changePassword', changePasswordData).then(function (results) {
            return results;
        });
    };

    userServiceFactory.forgotPassword = _forgotPassword;

    userServiceFactory.resetPassword = _resetPassword;

    return userServiceFactory;

}]);