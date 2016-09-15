'use strict';
app.controller('companiesController', ['$http', '$scope', '$rootScope', 'companiesDataSource',
    'ngAuthSettings', 'dialogService', 'messageService', 'messageDataService',
    function ($http, $scope, $rootScope, companiesDataSource,
        ngAuthSettings, dialogService, messageService, messageDataService) {
        var vm = this;
        vm.dataSource = companiesDataSource;

        //vm.$on('dataSourceMessage', function (event, data) {
        //    $rootScope.$broadcast('ngMessage', data);
        //});

        vm.addMode = false;

        var onClick = function (event, delegate) {
            var grid = event.grid;
            var selectedRow = grid.select();
            var dataItem = grid.dataItem(selectedRow);
            if (vm.addMode)
                delegate(grid);
            else
                if (selectedRow.length > 0)
                    delegate(grid, selectedRow, dataItem);
                else
                    alert("Please select a row");
        };

        vm.isToolbarVisible = false;

        var toggleToolbar = function () {
            vm.isToolbarVisible = !vm.isToolbarVisible;
        };

        vm.toolbarTemplate = kendo.template($('#toolbar-company').html());

        vm.popupEditorTemplate = kendo.template($("#popup_editor").html());

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        vm.dropdownchange = function (e) {
            console.log(e);
        };

        vm.onChange = function (e) {
            var grid = e.sender;
            $rootScope.lastSelectedDataItem = grid.dataItem(grid.select());
        };

        vm.onDataBound = function (e) {
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

        vm.save = function (e) {
            onClick(e, function (grid) {
                grid.saveRow();
                function handleSuccess(data, status, headers, config) {
                    toggleToolbar();
                    vm.addMode = false;
                };
                function handleError(data, status, headers, config) {
                    $rootScope.ngMessage = status;
                };
            });
        };

        vm.cancel = function (e) {
            onClick(e, function (grid) {
                grid.cancelRow();
                toggleToolbar();
                vm.addMode = false;
            });
        };

        vm.new = function (e) {
            var grid = e.grid;
            grid.addRow();
            toggleToolbar();
            vm.addMode = true;
        };

        vm.edit = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                $('[name="createdBy.username"]').attr("readonly", true);
                toggleToolbar();
                vm.addMode = false;
            });
        };

        vm.destroy = function (event) {
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
                        vm.addMode = false;
                    });
                },
                function () {
                    alert('Aborted!');
                });            
        };

        vm.dataSource.bind('change', function (e) {
            if (e.action && e.action == 'sync') {
                vm.isToolbarVisible = false;
            }
        });        

        vm.readOnlyEditor = function (container, options) {
            var input = $('<input style="color: red;" />');
            input.attr("name", options.field);
            input.appendTo(container);
        };

    }]);