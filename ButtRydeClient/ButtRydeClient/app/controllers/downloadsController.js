'use strict';
app.controller('downloadsController', ['$http', '$scope', '$rootScope', 'downloadDataSource', 'resourcesService', 'ngAuthSettings',
    function ($http, $scope, $rootScope, downloadDataSource, resourcesService, ngAuthSettings) {
       
        //$scope.files = [];            
        //resourcesService.get().then(function (results) {

        //    $scope.files = results.data.data;

        //}, function (error) {
        //    //alert(error.data.message);
        //});    


        //$scope.download = function (id, filename, mimeType) {
        //    $http.get(serviceBase + 'api/resources/download/' + id).then(function (results) {
        //        openSaveAsDialog(filename, results.data, mimeType);
        //        //return results;
        //    });
        //};

        //function openSaveAsDialog(filename, content, mediaType) {
        //    var blob = new Blob([content], { type: mediaType });
        //    saveAs(blob, filename);
        //};
        $scope.dataSource = downloadDataSource;

        $scope.$on('dataSourceMessage', function (event, data) {
            $rootScope.$broadcast('ngMessage', data);
        });

        downloadDataSource.filter([
            //{ "field": "fileFolder", "operator": "eq", "value": "abc" },
            { "field": "isPublished", "operator": "eq", "value": true }
        ]); // reset filter on dataSource every time view is loaded                    

        $scope.addMode = false;

        //var onClick = function (event, delegate) {
        //    var grid = event.grid;
        //    var selectedRow = grid.select();
        //    var dataItem = grid.dataItem(selectedRow);
        //    if ($scope.addMode)
        //        delegate(grid);
        //    else
        //        if (selectedRow.length > 0)
        //            delegate(grid, selectedRow, dataItem);
        //        else
        //            alert("Please select a row");
        //};

        $scope.items = ['delete?'];

        //$scope.isToolbarVisible = true;

        //var toggleToolbar = function () {
        //    $scope.isToolbarVisible = !$scope.isToolbarVisible;
        //};

        $scope.toolbarTemplate = kendo.template($('#toolbar-downloads').html());

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

        //$scope.save = function (e) {
        //    onClick(e, function (grid) {
        //        grid.saveRow();
        //        function handleSuccess(data, status, headers, config) {
        //            alert(status);
        //            toggleToolbar();
        //            alert('toggleToolbar');
        //            $scope.addMode = false;
        //        };
        //        function handleError(data, status, headers, config) {
        //            alert(status);
        //        };
        //    });
        //};

        //$scope.cancel = function (e) {
        //    onClick(e, function (grid) {
        //        //grid.dataSource.sync();
        //        grid.cancelRow();
        //        toggleToolbar();
        //        $scope.addMode = false;
        //    });
        //};

        //$scope.edit = function (e) {
        //    onClick(e, function (grid, row) {
        //        grid.editRow(row);
        //        $('[name="createdBy.username"]').attr("readonly", true);
        //        toggleToolbar();
        //        $scope.addMode = false;
        //    });
        //};

        //$scope.destroy = function (event) {
        //    var grid = event.grid;
        //    var selectedRow = grid.select();
        //    if (selectedRow.length == 0) {
        //        alert("Please select a row");
        //        return;
        //    }
        //    var modalInstance = $uibModal.open({
        //        animation: true,
        //        templateUrl: 'myModalContent.html',
        //        controller: 'ModalInstanceController',
        //        sizr: 'lg',
        //        resolve: {
        //            items: function () { return $scope.items; },
        //            name: function () { return 'Are you sure you want to delete this?'; }
        //        }
        //    });

        //    modalInstance.result.then(function (selectedItem) {
        //        $scope.selected = selectedItem;
        //        onClick(event, function (grid, row, dataItem) {
        //            grid.dataSource.remove(dataItem);
        //            grid.dataSource.sync();
        //            $scope.addMode = false;
        //        });
        //    }, function () {
        //        console.log('Modal dismissed.');
        //    });
        //};

        $scope.dataSource.bind('change', function (e) {
            if (e.action && e.action == 'sync') {
                //$scope.isToolbarVisible = false;
            }
        });

        $scope.download = function (e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            $http.get(serviceBase + 'api/resourcesDownload/download/' + dataItem.id, { responseType: 'arraybuffer' }).then(function (results) {
                openSaveAsDialog(dataItem.fileName, results.data, dataItem.mimeType);
                //return results;
            });
        };

        function openSaveAsDialog(filename, content, mediaType) {
            var blob = new Blob([content], { type: mediaType });
            saveAs(blob, filename);
        };
    }]);