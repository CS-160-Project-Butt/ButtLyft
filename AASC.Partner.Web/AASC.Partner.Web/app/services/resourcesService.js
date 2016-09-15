'use strict';
app.factory('resourcesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var resourcesServiceFactory = {};

    var _get = function () {

        return $http.get(serviceBase + 'api/resources').then(function (results) {
            return results;
        });
    };

    resourcesServiceFactory.get = _get;

    return resourcesServiceFactory;

}]);