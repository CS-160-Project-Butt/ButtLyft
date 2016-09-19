'use strict';
app.factory('departmentListService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var departmentListServiceFactory = {};

    var _get = function (companyId) {

        return $http.get(serviceBase + 'api/departments/' + companyId).then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function (results) {
            return $q.reject(results);
        });
    };

    var _getExcludeSelf = function (companyId, departmentId) {

        return $http.get(serviceBase + 'api/departments/getdepartmentlistexcludeself/' + companyId + '/' + departmentId).then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function (results) {
            return $q.reject(results);
        });
    };

    var _getDepartmentList = function (companyId) {

        return $http.get(serviceBase + 'api/departments/getdepartmentlist/' + companyId).then(function (results) {
            if (results.data)
                return results.data;
            else
                return $q.reject(results);
        }, function (results) {
            return $q.reject(results);
        });
    };

    departmentListServiceFactory.get = _get;

    departmentListServiceFactory.getExcludeSelf = _getExcludeSelf;

    departmentListServiceFactory.getDepartmentList = _getDepartmentList;

    return departmentListServiceFactory;

}]);