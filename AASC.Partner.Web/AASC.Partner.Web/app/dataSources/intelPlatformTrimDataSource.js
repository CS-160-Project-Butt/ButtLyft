'user strict';

app.factory('intelPlatformTrimDataSource', ['$http', 'ngAuthSettings', 'messageService',
    function ($http, ngAuthSettings, messageService) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/iotgRoadmap/getplatformtrims';
        return new kendo.data.DataSource({
            //type: 'aspnetmvc-ajax',
            transport: {
                read: {
                    url: serviceBase,
                    type: 'GET',
                    dataType: 'json',
                    contentType: 'application/json'
                }
                //read: function (options) {
                //    $http({
                //        method: 'GET',
                //        url: serviceBase + "?skip=" + options.data.skip + "&take=" + options.data.take + "&pageSize=" + options.data.pageSize +
                //            "&page=" + options.data.page + "&sorting=" + JSON.stringify(options.data.sort) +
                //            "&filter=" + JSON.stringify(options.data.filter)
                //    }).
                //    success(function (result) {
                //        options.success(result);
                //    }).
                //    error(function (result) {
                //        var dlg = null;
                //        var msg = messageService.prepareResultMessage(result);
                //        console.log(msg);
                //    });
                //},
                //parameterMap: function (data, operation) {
                //    if (operation != "read" && data)
                //        return JSON.stringify(data);
                //}
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
                model: {
                    fields: {
                        platform: { type: "string" },
                        trim: { type: "string" }
                    }
                }
            },
            error: function (e) {
                var msg = 'Error: ' + e.xhr.responseText;
                //alert(msg);
                console.log(msg);
            }
        });
    }]);