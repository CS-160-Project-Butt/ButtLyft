'use strict';
app.controller('applicationUsersController', ['$http', '$scope', '$rootScope', 'applicationUsersDataSource',
    'ngAuthSettings', 'dialogService',
    function ($http, $scope, $rootScope, applicationUsersDataSource, ngAuthSettings, dialogService) {

        $scope.dataSource = applicationUsersDataSource;

        //applicationUsersDataSource.filter({}); // reset filter on dataSource every time view is loaded                            
        $scope.$on('dataSourceMessage', function (event, data) {
            $rootScope.$broadcast('ngMessage', data);
        });

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

        $scope.isToolbarVisible = false;

        var toggleToolbar = function () {
            $scope.isToolbarVisible = !$scope.isToolbarVisible;
        };

        $scope.gridColumns = [
            { field: "id", title: "Id", hidden: true },
            { field: "userName", title: "User Name" },
            { field: "firstName", title: "First Name" },
            { field: "lastName", title: "Last Name" },
            { field: "email", title: "Email" },
            { field: "isActive", title: "Is Active", template: "<input type='checkbox' #= isActive ? 'checked': '' # disabled />" },
            { command: ["edit"], title: "" }
        ];

        $scope.popupEditorTemplate = kendo.template($("#popup_editor_applicationUser").html());

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
                    toggleToolbar();
                    $scope.addMode = false;
                };
                function handleError(data, status, headers, config) {
                    $rootScope.ngMessage = status;
                };
            });
        };

        $scope.cancel = function (e) {
            onClick(e, function (grid) {
                grid.cancelRow();
                toggleToolbar();
                $scope.addMode = false;
            });
        };
       
        $scope.edit = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                toggleToolbar();
                $scope.addMode = false;
            });
        };        

        $scope.dataSource.bind('change', function (e) {
            if (e.action && e.action == 'sync') {
                $scope.isToolbarVisible = false;
            }
        });
        
    }]);