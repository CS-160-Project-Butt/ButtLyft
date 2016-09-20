'use strict';
app.controller('resourcesController', ['$http', '$scope', '$rootScope', 'resourcesDataSource', 'ngAuthSettings', 'dialogService',
    function ($http, $scope, $rootScope, resourcesDataSource, ngAuthSettings, dialogService) {

        /*
        $scope.files = [];            
        resourcesService.get().then(function (results) {

            $scope.files = results.data;

        }, function (error) {
            //alert(error.data.message);
        });    
        */
        $scope.dataSource = resourcesDataSource;

        $scope.$on('dataSourceMessage', function (event, data) {
            $rootScope.$broadcast('ngMessage', data);
        });

        resourcesDataSource.filter({}); // reset filter on dataSource every time view is loaded                    

        $scope.addMode = false;

        var onClick = function (event, delegate) {
            var grid = event.grid;
            var selectedRow = grid.select();
            var dataItem = grid.dataItem(selectedRow);
            if ($scope.addMode)
                delegate(grid);
            else
                if (selectedRow.length > 0)
                    delegate(grid, selectedRow, dataItem);
                else {
                    $rootScope.ngMessage = { messageType: 'info', message: 'Please select a row', messageDetails: [] };
                    /*
                    dialogService.showDialog('Warning', 'Please select a row')
                        .then(
                        function () {
                            alert('Yes clicked');
                        },
                        function () {
                            alert('No clicked');
                        });

                    alert("Please select a row");
                    */
                }
                    
        };

        $scope.items = ['delete?'];

        $scope.isToolbarVisible = false;

        var toggleToolbar = function () {
            $scope.isToolbarVisible = !$scope.isToolbarVisible;
        };

        $scope.toolbarTemplate = kendo.template($('#toolbar-resources').html());

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        $scope.onChange = function (e) {
            var grid = e.sender;
            $rootScope.lastSelectedDataItem = grid.dataItem(grid.select());
        };

        $scope.onDataBound = function (e) {
            // check if there was a row that was selected
            if ($rootScope.lastSelectedDataItem == null) return;
            // get all rows
            var view = this.dataSource.view();
            // iterate through rows
            for (var i = 0; i < view.length; i++) {
                // find row with the lastSelectedId
                if (view[i].id == $rootScope.lastSelectedDataItem.id) {
                    var grid = e.sender;
                    // set the seected row
                    grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
                    break;
                }
            }
        };

        $scope.save = function (e) {
            onClick(e, function (grid) {
                grid.saveRow();
                function handleSuccess(data, status, headers, config) {
                    alert(status);
                    toggleToolbar();
                    alert('toggleToolbar');
                    $scope.addMode = false;
                };
                function handleError(data, status, headers, config) {
                    alert(status);
                };
            });
        };

        $scope.cancel = function (e) {
            onClick(e, function (grid) {
                //grid.dataSource.sync();
                grid.cancelRow();
                toggleToolbar();
                $scope.addMode = false;
            });
        };

        $scope.edit = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                $('[name="createdBy.username"]').attr("readonly", true);
                toggleToolbar();
                $scope.addMode = false;
            });
        };

        $scope.destroy = function (event) {
            var grid = event.grid;
            var selectedRow = grid.select();
            if (selectedRow.length == 0) {
                $rootScope.ngMessage = { messageType: 'info', message: 'Please select a row', messageDetails: [] };
                /*
                dialogService.showDialog('Warning', 'Please select a row')
                        .then(
                        function () {
                            alert('Yes clicked');
                        },
                        function () {
                            alert('No clicked');
                        });

                alert("Please select a row");
                */
                return;
            }
            dialogService.showDialog('Warning', 'You are going to delete a row')
                .then(
                function () {
                    onClick(event, function (grid, row, dataItem) {
                        grid.dataSource.remove(dataItem);
                        grid.dataSource.sync();
                        $scope.addMode = false;
                    });
                },
                function () {
                    $rootScope.ngMessage = { messageType: 'info', message: 'Action aborted!', messageDetails: [] };
                    //alert('Aborted!');
                });            
        };

        $scope.dataSource.bind('change', function (e) {
            if (e.action && e.action == 'sync') {
                $scope.isToolbarVisible = false;
            }
        });

        $scope.download = function (e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            $http.get(serviceBase + 'api/resources/download/' + dataItem.id, { responseType: 'arraybuffer' }).then(function (results) {
                openSaveAsDialog(dataItem.fileName, results.data, dataItem.mimeType);
                //return results;
            });
        };

        function openSaveAsDialog(filename, content, mediaType) {
            var blob = new Blob([content], { type: mediaType });
            saveAs(blob, filename);
        };

        $scope.readOnlyEditor = function (container, options) {
            var input = $('<input style="color: red;" />');
            input.attr("name", options.field);
            input.appendTo(container);
        };
    }]);