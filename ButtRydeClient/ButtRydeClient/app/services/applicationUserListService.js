'use strict';
app.factory('applicationUserListService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var applicationUserListServiceFactory = {};

    var _get = function () {

        return $http.get(serviceBase + 'api/applicationusers/getavailableuserslist').then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function (results) {
            return $q.reject(results);
        });
    };
    
    applicationUserListServiceFactory.get = _get;

    return applicationUserListServiceFactory;

}]);