'user strict';

app.factory('resourcesDataSource', ['$http', 'ngAuthSettings', 'resourceViewModel', 'messageService', 'localStorageService',
    function ($http, ngAuthSettings, resourceViewModel, messageService, localStorageService) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/resources/';
        return new kendo.data.DataSource({
            //type: 'aspnetmvc-ajax',
            //type: "odata",
            transport: {
                update: function (options) {
                    var request = $http({
                        method: 'put',
                        url: serviceBase + 'put/' + options.data.id,
                        data: JSON.stringify(options.data)
                    });
                    request.then(handleSuccess, handleError);
                    function handleSuccess(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        alert(msg);
                        options.success(response.data);
                    };
                    function handleError(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        alert(msg);
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
                        alert(msg);
                        options.success(response.data);
                    };
                    function handleError(response) {
                        var msg = messageService.prepareResponseMessage(response);
                        alert(msg);
                        console.log(msg);
                    };
                },
                read: function (options) {
                    //alert(localStorageService.get('authorizationData').token);
                    $.ajax({
                        //url: serviceBase + 'Get/',
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
                model: resourceViewModel
            },
            error: function (e) {
                var msg = 'Error: ' + e.xhr.responseText;
                alert(msg);
                console.log(msg);
            }
        });
    }]);