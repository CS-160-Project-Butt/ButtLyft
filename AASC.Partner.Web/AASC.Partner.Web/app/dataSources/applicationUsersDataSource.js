﻿'user strict';

app.factory('applicationUsersDataSource', ['$http', '$rootScope', 'ngAuthSettings', 'applicationUserViewModel', 'messageService', 'localStorageService',
    function ($http, $rootScope, ngAuthSettings, applicationUserViewModel, messageService, localStorageService) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/applicationusers/';
        return new kendo.data.DataSource({
            transport: {                
                update: function (options) {
                    var request = $http({
                        method: 'put',
                        url: serviceBase + options.data.id,
                        data: JSON.stringify(options.data)
                    });
                    request.then(handleSuccess, handleError);
                    function handleSuccess(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        options.success(response.data);
                    };
                    function handleError(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        $rootScope.ngMessage = msg;
                        console.log(msg);
                        options.error();
                    };
                },                
                read: function (options) {
                    $.ajax({
                        headers: { 'Authorization': 'Bearer ' + localStorageService.get('authorizationData').token },
                        url: serviceBase,
                        type: 'GET',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: {
                            skip: options.data.skip,
                            take: options.data.take,
                            pageSize: options.data.pageSize,
                            page: options.data.page,
                            sorting: JSON.stringify(options.data.sort),
                            filter: JSON.stringify(options.data.filter)
                        },
                        success: function (result) {
                            options.success(result);
                        },
                        error: function (result) {
                            var dlg = null;
                            var msg = messageService.prepareResponseMessage(result);
                            $rootScope.$broadcast('dataSourceMessage', msg);
                            options.error(result);
                        }
                    });
                },
                parameterMap: function (data, operation) {
                    if (operation != "read" && data)
                        return JSON.stringify(data);
                }
            },
            batch: false,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 10,
            schema: {
                data: function (data) {
                    return data.data;
                },
                total: function (data) {
                    return data.total;
                },
                model: applicationUserViewModel
            },
            error: function (e) {
                var msg = 'Error: ' + e.xhr.responseText;
                alert(msg);
                console.log(msg);
            }
        });
    }]);