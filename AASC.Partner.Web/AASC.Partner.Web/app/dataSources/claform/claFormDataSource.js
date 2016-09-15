'user strict';

app.factory('claFormDataSource', ['$http', '$rootScope', 'ngAuthSettings', 'claFormViewModel', 'messageService', 'localStorageService',
    function ($http, $rootScope, ngAuthSettings, claFormViewModel, messageService, localStorageService) {
        
        var serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/cla/';


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
                        alert(msg);
                        console.log(msg);
                    };
                },
                read: function (options) {
                    //$.ajax({
                    //    headers: { 'Authorization': 'Bearer ' + localStorageService.get('authorizationData').token },
                    //    url: serviceBase,
                    //    type: 'GET',
                    //    dataType: 'json',
                    //    contentType: 'application/json',
                    //    data: {
                    //        skip: options.data.skip,
                    //        take: options.data.take,
                    //        pageSize: options.data.pageSize,
                    //        page: options.data.page,
                    //        sorting: JSON.stringify(options.data.sort),
                    //        filter: JSON.stringify(options.data.filter)
                    //    },
                    //    success: function (result) {
                    //        console.log(result);
                    //        options.success(result);
                    //    },
                    //    error: function (result) {
                    //        var dlg = null;
                    //        var msg = messageService.prepareResponseMessage(result);
                    //        $rootScope.$broadcast('dataSourceMessage', msg);
                    //        options.error(result);
                    //    }
                    //});
                    options.success({ "data": [{ "id": "0ae715a5-0eed-4004-bc8c-957a51c7dcbb", "companyName": "JeffCoooooooooooooooooooooo", "taxID": "10239124236534", "salesContact": "Mike Arcure", "address": "1451 sdfijsod sokf", "city": "Milpitas", "country": "United States", "postCode": "10923", "signerFirstName": "Jeffffffffffffffffffffffffffffffffffffffffff", "signerLastName": "Li", "signerEmail": "jeff.li@sjsu.edu", "signerJobTitle": "Intern", "signerPhoneNumber": "510-1231-12312", "technicalFirstName": "Eric", "technicalLastName": "Shih", "technicalEmail": "eric.shih@advantech.com", "technicalJobTitle": "Commander", "technicalPhoneNumber": "1-408-123-636346", "deviceCategories": "[{\"device\": \"Arcade/Consumer Gaming Device\", \"selected\": false},{\"device\": \"ATM\", \"selected\": false},{\"device\": \"Casino Gaming Device\", \"selected\": false},{\"device\": \"CCTV/Surveillance/Security\", \"selected\": false},{\"device\": \"Digital Picture Frame\", \"selected\": true},{\"device\": \"Digital Signage\", \"selected\": true},{\"device\": \"Electronic Payment Terminal\", \"selected\": false},{\"device\": \"Feature Phone\", \"selected\": false},{\"device\": \"Hand Held Data Terminal/PDA\", \"selected\": false},{\"device\": \"Industrial Automation Device\", \"selected\": false},{\"device\": \"Kiosk\", \"selected\": true},{\"device\": \"Media Player\", \"selected\": false},{\"device\": \"Medical Device\", \"selected\": false},{\"device\": \"Navigation Device\", \"selected\": true},{\"device\": \"Network Projector\", \"selected\": true},{\"device\": \"Point of Sale Device\", \"selected\": false},{\"device\": \"Printing Device\", \"selected\": false},{\"device\": \"Security/Surveillance Device\", \"selected\": true},{\"device\": \"Server\", \"selected\": true},{\"device\": \"Set-Top Box\", \"selected\": false},{\"device\": \"Telephone Device\", \"selected\": true},{\"device\": \"Test and Measurement Device\", \"selected\": false},{\"device\": \"Thin Client Device\", \"selected\": false},{\"device\": \"Other Device\", \"selected\": false}]", "productList": "[{\"productType\": \"Standard\", \"product\": \"XP\", \"quantity\": 12},{\"productType\": \"Standard\", \"product\": \"2009\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"8\", \"quantity\": 345},{\"productType\": \"Pro\", \"product\": \"2000\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"XP\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"7\", \"quantity\": 346},{\"productType\": \"Pro\", \"product\": \"8\", \"quantity\": 457},{\"productType\": \"Pro\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"2009\", \"quantity\": 346},{\"productType\": \"POSReady\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8.1\", \"quantity\": 1},{\"productType\": \"CE Compact\", \"product\": \"CE5\", \"quantity\": 346},{\"productType\": \"CE Compact\", \"product\": \"CE6\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE7\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"Compact 2013\", \"quantity\": 1111}]", "otherType": "hello", "otherQuantity": 500, "claNumber": null, "claStatus": "User Submitted", "claStatusDate": null, "customerERPID": null, "createdDate": "2016-07-19T23:59:23.143" }, { "id": "2e802d3e-59fa-4bea-a3e9-1b1fb9753049", "companyName": "jeff", "taxID": "10239", "salesContact": "Mike Arcure", "address": "1451 sdfijsod sokf", "city": "fsoejfs", "country": "fosejfoije", "postCode": "109238", "signerFirstName": "qalkdmaw", "signerLastName": "wkoefmwe", "signerEmail": "sekmflse@gmail.com", "signerJobTitle": "sldkfds", "signerPhoneNumber": "23094", "technicalFirstName": "fmksem", "technicalLastName": "salkemf", "technicalEmail": "Jeff@gmail.com", "technicalJobTitle": "slmef", "technicalPhoneNumber": "wsekflse", "deviceCategories": "[{\"device\": \"Arcade/Consumer Gaming Device\", \"selected\": true},{\"device\": \"ATM\", \"selected\": true},{\"device\": \"Casino Gaming Device\", \"selected\": true},{\"device\": \"CCTV/Surveillance/Security\", \"selected\": true},{\"device\": \"Digital Picture Frame\", \"selected\": true},{\"device\": \"Digital Signage\", \"selected\": false},{\"device\": \"Electronic Payment Terminal\", \"selected\": false},{\"device\": \"Feature Phone\", \"selected\": false},{\"device\": \"Hand Held Data Terminal/PDA\", \"selected\": false},{\"device\": \"Industrial Automation Device\", \"selected\": true},{\"device\": \"Kiosk\", \"selected\": true},{\"device\": \"Media Player\", \"selected\": false},{\"device\": \"Medical Device\", \"selected\": false},{\"device\": \"Navigation Device\", \"selected\": false},{\"device\": \"Network Projector\", \"selected\": false},{\"device\": \"Point of Sale Device\", \"selected\": false},{\"device\": \"Printing Device\", \"selected\": false},{\"device\": \"Security/Surveillance Device\", \"selected\": false},{\"device\": \"Server\", \"selected\": false},{\"device\": \"Set-Top Box\", \"selected\": false},{\"device\": \"Telephone Device\", \"selected\": false},{\"device\": \"Test and Measurement Device\", \"selected\": false},{\"device\": \"Thin Client Device\", \"selected\": false},{\"device\": \"Other Device\", \"selected\": false}]", "productList": "[{\"productType\": \"Standard\", \"product\": \"XP\", \"quantity\": 12},{\"productType\": \"Standard\", \"product\": \"2009\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"8\", \"quantity\": 345},{\"productType\": \"Pro\", \"product\": \"2000\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"XP\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"7\", \"quantity\": 346},{\"productType\": \"Pro\", \"product\": \"8\", \"quantity\": 457},{\"productType\": \"Pro\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"2009\", \"quantity\": 346},{\"productType\": \"POSReady\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE5\", \"quantity\": 346},{\"productType\": \"CE Compact\", \"product\": \"CE6\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE7\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"Compact 2013\", \"quantity\": 0}]", "otherType": "hello", "otherQuantity": 500, "claNumber": null, "claStatus": "User Submitted", "claStatusDate": null, "customerERPID": null, "createdDate": "2016-07-17T00:39:32.907" }, { "id": "51db8c97-8107-4d22-92b1-5ba1d1f1722a", "companyName": "jeff", "taxID": "10239", "salesContact": "Mike Arcure", "address": "1451 sdfijsod sokf", "city": "fsoejfs", "country": "fosejfoije", "postCode": "109238", "signerFirstName": "qalkdmaw", "signerLastName": "wkoefmwe", "signerEmail": "sekmflse@gmail.com", "signerJobTitle": "sldkfds", "signerPhoneNumber": "23094", "technicalFirstName": "fmksem", "technicalLastName": "salkemf", "technicalEmail": "Jeff@gmail.com", "technicalJobTitle": "slmef", "technicalPhoneNumber": "wsekflse", "deviceCategories": "[{\"device\": \"Arcade/Consumer Gaming Device\", \"selected\": true},{\"device\": \"ATM\", \"selected\": true},{\"device\": \"Casino Gaming Device\", \"selected\": true},{\"device\": \"CCTV/Surveillance/Security\", \"selected\": true},{\"device\": \"Digital Picture Frame\", \"selected\": true},{\"device\": \"Digital Signage\", \"selected\": false},{\"device\": \"Electronic Payment Terminal\", \"selected\": false},{\"device\": \"Feature Phone\", \"selected\": false},{\"device\": \"Hand Held Data Terminal/PDA\", \"selected\": false},{\"device\": \"Industrial Automation Device\", \"selected\": true},{\"device\": \"Kiosk\", \"selected\": true},{\"device\": \"Media Player\", \"selected\": false},{\"device\": \"Medical Device\", \"selected\": false},{\"device\": \"Navigation Device\", \"selected\": false},{\"device\": \"Network Projector\", \"selected\": false},{\"device\": \"Point of Sale Device\", \"selected\": false},{\"device\": \"Printing Device\", \"selected\": false},{\"device\": \"Security/Surveillance Device\", \"selected\": false},{\"device\": \"Server\", \"selected\": false},{\"device\": \"Set-Top Box\", \"selected\": false},{\"device\": \"Telephone Device\", \"selected\": false},{\"device\": \"Test and Measurement Device\", \"selected\": false},{\"device\": \"Thin Client Device\", \"selected\": false},{\"device\": \"Other Device\", \"selected\": false}]", "productList": "[{\"productType\": \"Standard\", \"product\": \"XP\", \"quantity\": 12},{\"productType\": \"Standard\", \"product\": \"2009\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"8\", \"quantity\": 345},{\"productType\": \"Pro\", \"product\": \"2000\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"XP\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"7\", \"quantity\": 346},{\"productType\": \"Pro\", \"product\": \"8\", \"quantity\": 457},{\"productType\": \"Pro\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"2009\", \"quantity\": 346},{\"productType\": \"POSReady\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE5\", \"quantity\": 346},{\"productType\": \"CE Compact\", \"product\": \"CE6\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE7\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"Compact 2013\", \"quantity\": 0}]", "otherType": "hello", "otherQuantity": 500, "claNumber": null, "claStatus": "User Submitted", "claStatusDate": null, "customerERPID": null, "createdDate": "2016-07-17T00:39:33.95" }, { "id": "7a087b72-8542-4cbc-abb3-b44bffbed4df", "companyName": "jeff", "taxID": "10239", "salesContact": "Mike Arcure", "address": "1451 sdfijsod sokf", "city": "fsoejfs", "country": "fosejfoije", "postCode": "109238", "signerFirstName": "qalkdmaw", "signerLastName": "wkoefmwe", "signerEmail": "sekmflse@gmail.com", "signerJobTitle": "sldkfds", "signerPhoneNumber": "23094", "technicalFirstName": "fmksem", "technicalLastName": "salkemf", "technicalEmail": "Jeff@gmail.com", "technicalJobTitle": "slmef", "technicalPhoneNumber": "wsekflse", "deviceCategories": "[{\"device\": \"Arcade/Consumer Gaming Device\", \"selected\": true},{\"device\": \"ATM\", \"selected\": true},{\"device\": \"Casino Gaming Device\", \"selected\": true},{\"device\": \"CCTV/Surveillance/Security\", \"selected\": true},{\"device\": \"Digital Picture Frame\", \"selected\": true},{\"device\": \"Digital Signage\", \"selected\": false},{\"device\": \"Electronic Payment Terminal\", \"selected\": false},{\"device\": \"Feature Phone\", \"selected\": false},{\"device\": \"Hand Held Data Terminal/PDA\", \"selected\": false},{\"device\": \"Industrial Automation Device\", \"selected\": true},{\"device\": \"Kiosk\", \"selected\": true},{\"device\": \"Media Player\", \"selected\": false},{\"device\": \"Medical Device\", \"selected\": false},{\"device\": \"Navigation Device\", \"selected\": false},{\"device\": \"Network Projector\", \"selected\": false},{\"device\": \"Point of Sale Device\", \"selected\": false},{\"device\": \"Printing Device\", \"selected\": false},{\"device\": \"Security/Surveillance Device\", \"selected\": false},{\"device\": \"Server\", \"selected\": false},{\"device\": \"Set-Top Box\", \"selected\": false},{\"device\": \"Telephone Device\", \"selected\": false},{\"device\": \"Test and Measurement Device\", \"selected\": false},{\"device\": \"Thin Client Device\", \"selected\": false},{\"device\": \"Other Device\", \"selected\": false}]", "productList": "[{\"productType\": \"Standard\", \"product\": \"XP\", \"quantity\": 12},{\"productType\": \"Standard\", \"product\": \"2009\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"Standard\", \"product\": \"8\", \"quantity\": 345},{\"productType\": \"Pro\", \"product\": \"2000\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"XP\", \"quantity\": 0},{\"productType\": \"Pro\", \"product\": \"7\", \"quantity\": 346},{\"productType\": \"Pro\", \"product\": \"8\", \"quantity\": 457},{\"productType\": \"Pro\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"2009\", \"quantity\": 346},{\"productType\": \"POSReady\", \"product\": \"7\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8\", \"quantity\": 0},{\"productType\": \"POSReady\", \"product\": \"Industry 8.1\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE5\", \"quantity\": 346},{\"productType\": \"CE Compact\", \"product\": \"CE6\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"CE7\", \"quantity\": 0},{\"productType\": \"CE Compact\", \"product\": \"Compact 2013\", \"quantity\": 0}]", "otherType": "hello", "otherQuantity": 500, "claNumber": null, "claStatus": "User Submitted", "claStatusDate": null, "customerERPID": null, "createdDate": "2016-07-17T00:37:57.687" }], "total": 4 })
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
                model: claFormViewModel
            },
            error: function (e) {
                var msg = 'Error: ' + e.xhr.responseText;
                //alert(msg);
                //console.log(msg);
            }
        });
    }]);