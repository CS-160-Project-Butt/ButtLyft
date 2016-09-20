'user strict';

app.factory('downloadDataSource', ['$http', 'ngAuthSettings', 'resourceViewModel', 'messageService',
    function ($http, ngAuthSettings, resourceViewModel, messageService) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/resourcesDownload/';
        return new kendo.data.DataSource({
            //type: 'aspnetmvc-ajax',
            //type: "odata",
            transport: {                
                read: function (options) {
                    $http({
                        //url: serviceBase + 'Get/',                        
                        method: 'GET',
                        url: serviceBase + "?skip=" + options.data.skip + "&take=" + options.data.take + "&pageSize=" + options.data.pageSize +
                            "&page=" + options.data.page + "&sorting=" + JSON.stringify(options.data.sort) +
                            "&filter=" + JSON.stringify(options.data.filter)
                        //dataType: 'json',
                        //contentType: 'application/json',
                        //data: {
                        //    skip: options.data.skip,
                        //    take: options.data.take,
                        //    pageSize: options.data.pageSize,
                        //    page: options.data.page,
                        //    sorting: JSON.stringify(options.data.sort),
                        //    filter: JSON.stringify(options.data.filter)
                    }).
                        success(function (result) {
                            options.success(result);
                        }).
                        error(function (result) {
                            var dlg = null;
                            var msg = messageService.prepareResponseMessage(result);
                            $rootScope.$broadcast('dataSourceMessage', msg);
                            console.log(msg);
                        });
                    },
                //},
                parameterMap: function (data, operation) {
                    if (operation != "read" && data)
                        return JSON.stringify(data);
                }
            },
            //requestStart: function () {
            //    kendo.ui.progress($('#grid'), true);
            //},
            //requestEnd: function () {
            //    kendo.ui.progress($('#grid'), false);
            //},
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