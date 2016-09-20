'use strict';
app.factory('rolesService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var rolesServiceFactory = {};

    var _roles = [];

    function containsAny(source, target) {
        for (var i = 0; i < target.length; i++) {
            for (var j = 0; j < source.length; j++) {
                if (target[i] === source[j])
                    return true;
            }
        }
        return false;
    };

    function getUserRoles() {
        //var authData = localStorageService.get('authorizationData');
        var deferred = $q.defer();

        $http.get(serviceBase + 'api/roles/getuserroles').then(function (results) {
            //$http.get(serviceBase + 'api/roles/getuserroles', { headers: { 'Content-Type': 'application/json', 'Accept': 'application/json', 'Authorization': 'bearer ' + authData.token } }).then(function (results) {
            _roles = results.data;
            //return results;
            deferred.resolve(results);
        });
        return deferred.promise;
    };

    var _getUserRoles = function () {
        return getUserRoles();
    };

    var _isUserInRoles = function (authorizedRoles) {
        getUserRoles().then(function () {
            if (!angular.isArray(authorizedRoles)) {
                authorizedRoles = [authorizedRoles];
            }
            return containsAny(_roles, authorizedRoles);
        });
    };

    rolesServiceFactory.getUserRoles = _getUserRoles;

    rolesServiceFactory.isUserInRoles = _isUserInRoles;

    return rolesServiceFactory;

}]);