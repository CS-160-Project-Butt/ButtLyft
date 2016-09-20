'use strict';
app.controller('iotgRoadmapController', ['$http', '$scope', '$rootScope', 'iotgRoadmapDataSource',
    'intelRoadmapStatusDataSource', 'intelPlatformDataSource', 'intelPlatformTrimDataSource', 'intelPlatformTrimCodeNameDataSource',
    'intelModelLevelDataSource', 'intelModelLevelCategoryDataSource', 'intelModelLevelCategorySubcategoryDataSource',
    'intelMarketSegmentDataSource', 'ngAuthSettings', 'dialogService',
    function ($http, $scope, $rootScope, iotgRoadmapDataSource, intelRoadmapStatusDataSource, intelPlatformDataSource,
        intelPlatformTrimDataSource, intelPlatformTrimCodeNameDataSource,
        intelModelLevelDataSource, intelModelLevelCategoryDataSource, intelModelLevelCategorySubcategoryDataSource,
        intelMarketSegmentDataSource, ngAuthSettings, dialogService) {

        $scope.dataSource = iotgRoadmapDataSource;

        $scope.$on('dataSourceMessage', function (event, data) {
            $rootScope.$broadcast('ngMessage', data);
        });

        iotgRoadmapDataSource.filter({}); // reset filter on dataSource every time view is loaded                            

        function refreshDropDownListData() {
            // get platforms
            $scope.platformDataSource = intelPlatformDataSource;
            $scope.platformDataSource.read();
            $scope.platforms = $scope.platformDataSource.data();

            // get market segments
            $scope.marketSegmentDataSource = intelMarketSegmentDataSource;
            $scope.marketSegmentDataSource.read();
            $scope.marketSegments = $scope.marketSegmentDataSource.data();

            // get roadmap status
            $scope.roadmapStatusDataSource = intelRoadmapStatusDataSource;
            $scope.roadmapStatusDataSource.read();
            $scope.roadmapStatus = $scope.roadmapStatusDataSource.data();

            // get platform trims
            $scope.platformTrimDataSource = intelPlatformTrimDataSource;
            $scope.platformTrimDataSource.read();
            $scope.platformTrims = $scope.platformTrimDataSource.data();

            // get platform trim codenames
            $scope.platformTrimCodeNameDataSource = intelPlatformTrimCodeNameDataSource;
            $scope.platformTrimCodeNameDataSource.read();
            $scope.platformTrimCodeNames = $scope.platformTrimCodeNameDataSource.data();

            // get levels
            $scope.levelDataSource = intelModelLevelDataSource;
            $scope.levelDataSource.read();
            $scope.levels = $scope.levelDataSource.data();

            // get level categories
            $scope.levelCategoryDataSource = intelModelLevelCategoryDataSource;
            $scope.levelCategoryDataSource.read();
            $scope.levelCategories = $scope.levelCategoryDataSource.data();

            // get level category subcategories
            $scope.levelCategorySubcategoryDataSource = intelModelLevelCategorySubcategoryDataSource;
            $scope.levelCategorySubcategoryDataSource.read();
            $scope.levelCategorySubCategories = $scope.levelCategorySubcategoryDataSource.data();
        }

        refreshDropDownListData();

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
                else
                    alert("Please select a row");
        };

        $scope.items = ['delete?'];

        $scope.isToolbarVisible = false;

        var toggleToolbar = function () {
            $scope.isToolbarVisible = !$scope.isToolbarVisible;
        };

        $scope.toolbarTemplate = kendo.template($('#toolbar-iotgroadmap').html());

        $scope.popupEditorTemplate = kendo.template($("#popup_editor").html());

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        $scope.dropdownchange = function (e) {
            console.log(e);
        };

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
                    //alert('toggleToolbar');
                    $scope.addMode = false;
                };
                function handleError(data, status, headers, config) {
                    $rootScope.ngMessage = status;
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

        //$scope.details = function (e) {
        //    onClick(e, function (grid, row, dataItem) {
        //        $location.url('/iotgRoadmap/edit/' + dataItem.id);
        //    });
        //};

        $scope.new = function (e) {
            // refresh drop down list data
            refreshDropDownListData();

            var grid = e.grid;
            grid.addRow();
            toggleToolbar();
            $scope.addMode = true;
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
                dialogService.showDialog('Warning', 'Please select a row')
                    .then(
                    function () {
                        alert('Yes clicked');
                    },
                    function () {
                        alert('No clicked');
                    });
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
                    alert('Aborted!');
                });
            //var modalInstance = $uibModal.open({
            //    animation: true,
            //    templateUrl: 'myModalContent.html',
            //    controller: 'ModalInstanceController',
            //    sizr: 'lg',
            //    resolve: {
            //        items: function () { return $scope.items; },
            //        name: function () { return 'Are you sure you want to delete this?'; }
            //    }
            //});

            //modalInstance.result.then(function (selectedItem) {
            //    $scope.selected = selectedItem;
            //    onClick(event, function (grid, row, dataItem) {
            //        grid.dataSource.remove(dataItem);
            //        grid.dataSource.sync();
            //        $scope.addMode = false;
            //    });
            //}, function () {
            //    console.log('Modal dismissed.');
            //});
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