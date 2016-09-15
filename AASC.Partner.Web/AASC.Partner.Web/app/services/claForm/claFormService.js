'use strict';
app.service('claFormService', ['$http', '$q', 'ngAuthSettings', 'localStorageService', function ($http, $q, ngAuthSettings, localStorageService) {

    var editID = "testid";

    this.setEditID = function (id) {
        editID = id;
    };

    this.getEditID = function () {
        return editID
    };
    
    this.getDevices = function () {
        return this.getApiData('api/cla/getdevices');
    };

    this.getProductTypes = function () {
        return this.getApiData('api/cla/getproducttypes');
    };

    this.getProductList = function () {
        return this.getApiData('api/cla/getproducts');
    };

    this.getSalesReps = function () {
        return this.getApiData('api/cla/getsalesreps');
    };

    this.getApiData = function (apiString) {
        return $http.get(serviceBase + apiString).success(function (results) {
            return results.data;
        });
    };

    this.updateData = function (data) {
        return $http.post(serviceBase + 'api/cla/updateclaform', data).success(function (results) {
            return results.data;
        });
    };

    //var oauthToken = 'oauth/token';
    //var oauthToken = 'Token';

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var claData = {};

    var submitCLA = function () {

        populateData();

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/cla', cladata).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

}]);