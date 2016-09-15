﻿'user strict';

app.factory('departmentsDataSource', ['$http', '$rootScope', 'ngAuthSettings', 'departmentListViewModel', 'messageService', 'localStorageService',
    function ($http, $rootScope, ngAuthSettings, departmentListViewModel, messageService, localStorageService) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/departments/';
        return new kendo.data.DataSource({
            transport: {
                create: function (options) {
                    var request = $http({
                        method: 'post',
                        url: serviceBase,
                        data: JSON.stringify(options.data)
                    });
                    request.then(handleSuccess, handleError);
                    function handleSuccess(response) {
                        options.success(response.data);
                        $('#grid').data('kendoGrid').dataSource.read();
                    };
                    function handleError(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        $rootScope.ngMessage = msg;
                        options.error(response);
                    };
                },
                update: function (options) {
                    //if (options.data.parentDepartment.id.id) {
                    //    options.data.parentDepartment = { id : options.data.parentDepartment.id.id, name: options.data.parentDepartment.id.name };
                    //}
                    var request = $http({
                        method: 'put',
                        url: serviceBase + options.data.id,
                        data: JSON.stringify(options.data)
                    });
                    request.then(handleSuccess, handleError);
                    function handleSuccess(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        options.success(response.data);
                        $rootScope.refreshRequired = true;
                    };
                    function handleError(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        $rootScope.ngMessage = msg;
                        console.log(msg);
                        options.error();
                    };
                },
                destroy: function (options) {
                    var request = $http({
                        method: 'delete',
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
                        $rootScope.refreshRequired = true;
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
                        cache: false,
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
                model: departmentListViewModel
            },
            error: function (e) {
                var msg = 'Error: ' + e.xhr.responseText;
                alert(msg);
                console.log(msg);
            }
        });
    }]);