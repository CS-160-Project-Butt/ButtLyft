'use strict';
app.factory('employeeListService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var employeeListServiceFactory = {};

    var _get = function (companyId) {

        return $http.get(serviceBase + 'api/employees/getemployeelist/' + companyId).then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function (results) {
            return $q.reject(results);
        });
    };

    var _getExcludeSelf = function (companyId, employeeId) {

        return $http.get(serviceBase + 'api/employees/getemployeelistexcludeself/' + companyId + '/' + employeeId).then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function (results) {
            return $q.reject(results);
        });
    };    

    employeeListServiceFactory.get = _get;

    employeeListServiceFactory.getExcludeSelf = _getExcludeSelf;

    return employeeListServiceFactory;

}]);