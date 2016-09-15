'use strict';
app.factory('companyListService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var companyListServiceFactory = {};

    var _get = function () {

        return $http.get(serviceBase + 'api/companies/getcompanylist').then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function(results) {
            return $q.reject(results);
        });
    };

    companyListServiceFactory.get = _get;

    return companyListServiceFactory;

}]);