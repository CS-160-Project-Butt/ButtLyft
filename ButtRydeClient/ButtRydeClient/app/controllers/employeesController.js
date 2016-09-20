'use strict';
app.controller('employeesController', ['$http', '$scope', '$rootScope', 'employeesDataSource', 'applicationUserListService',
    'companyListService', 'ngAuthSettings', 'dialogService', 'messageService',
    function ($http, $scope, $rootScope, employeesDataSource, applicationUserListService,
        companyListService, ngAuthSettings, dialogService, messageService) {

        $scope.companySelected = {};

        $scope.dataSource = employeesDataSource;

        $scope.$on('dataSourceMessage', function (event, data) {
            $rootScope.$broadcast('ngMessage', data);
        });

        applicationUserListService.get().then(function (data) {
            $scope.applicationUsers = data.data;
        }, function (error) {
            $rootScope.ngMessage = messageService.prepareResponseMessage(error);
        });

        $rootScope.$watch('refreshRequired', function () {
            if ($rootScope.refreshRequired == true) {
                var grid = $('#grid').data('kendoGrid').dataSource;
                grid.read();
                $rootScope.refreshRequired = false;
            }
        });

        $scope.$watch('companySelected', function () {
            if ($scope.companySelected && $scope.companySelected.id) {
                employeesDataSource.filter({ field: 'companyId', operator: 'eq', value: $scope.companySelected.id }); // reset filter on dataSource every time view is loaded                
            }
        });

        if ($scope.companySelected.id) {
            employeesDataSource.filter({ field: 'companyId', operator: 'eq', value: $scope.companySelected.id }); // reset filter on dataSource every time view is loaded
        }

        $scope.addMode = false;

        function refreshCompanyDropDownListData() {
            companyListService.get()
                .then(function (data) {
                    $scope.companies = data.data;
                    $scope.companySelected = data.data[0]; // set first item as default
                }, function (error) {
                    $rootScope.ngMessage = messageService.prepareResponseMessage(error);
                });
        };

        refreshCompanyDropDownListData();

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

        $scope.toolbarTemplate = kendo.template($('#toolbar-employee').html());

        $scope.popupEditorTemplate = kendo.template($("#popup_editor_employee").html());

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
            
        };

        $scope.cancel = function (e) {
            toggleToolbar();
            $scope.addMode = false;
        };

        $scope.new = function (e) {
            var grid = e.grid;
            grid.addRow();

            toggleToolbar();
            $scope.addMode = true;
        };

        $scope.edit = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                $('[name="createdByUserName"]').attr("readonly", true);
                var id = $('[name="id"]').val();

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
                    $scope.addMode = false;
                    onClick(event, function (grid, row, dataItem) {
                        grid.dataSource.remove(dataItem);
                        grid.dataSource.sync();
                    });
                },
                function () {
                    alert('Aborted!');
                });
        };

        $scope.dataSource.bind('change', function (e) {
            if (e.action && e.action == 'sync') {
                $scope.isToolbarVisible = false;
            }
            if (e.action && e.action == 'add') {
                e.items[0].companyId = $scope.companySelected.id;
                e.items[0].companyName = $scope.companySelected.name;
            }
        });

        $scope.readOnlyEditor = function (container, options) {
            var input = $('<input style="color: red;" />');
            input.attr("name", options.field);
            input.appendTo(container);
        };

    }]);