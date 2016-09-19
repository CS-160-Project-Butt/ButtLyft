'use strict';
app.controller('departmentsController', ['$http', '$scope', '$rootScope', 'departmentsDataSource',
    'companyListService', 'departmentListService', 'employeeListService', 'employeesDataSource', 'ngAuthSettings', 'dialogService', 'messageService',
    function ($http, $scope, $rootScope, departmentsDataSource, companyListService, departmentListService, employeeListService,
        employeesDataSource, ngAuthSettings, dialogService, messageService) {

        $scope.companySelected = {};

        $scope.parentDepartmentId = '';

        $scope.dataSource = departmentsDataSource;

        $scope.parentDepartments = {};

        $scope.$on('dataSourceMessage', function (event, data) {
            $rootScope.$broadcast('ngMessage', data);
        });

        $rootScope.$watch('refreshRequired', function () {
            if ($rootScope.refreshRequired == true)
            {
                var grid = $('#grid').data('kendoGrid').dataSource;
                grid.read();
                $rootScope.refreshRequired = false;
            }
        });

        $scope.$watch('companySelected', function () {
            if ($scope.companySelected && $scope.companySelected.id) {
                departmentsDataSource.filter({ field: 'companyId', operator: 'eq', value: $scope.companySelected.id }); // reset filter on dataSource every time view is loaded
                departmentListService.getDepartmentList($scope.companySelected.id).then(function (data) {
                    $scope.parentDepartments = data.data;
                }, function (error) {
                    $rootScope.ngMessage = messageService.prepareResponseMessage(error);
                });
            }                
        });
        
        if ($scope.companySelected.id)
        {
            departmentsDataSource.filter({ field: 'companyId', operator: 'eq', value: $scope.companySelected.id }); // reset filter on dataSource every time view is loaded
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

            //$scope.employeesDataSource = employeesDataSource;
            //$scope.employeesDataSource.read();
            //$scope.employees = $scope.employeesDataSource.data();
        };

        refreshCompanyDropDownListData();

        function refreshParentDepartmentDropDownListData(id) {
            if (id && id !== '') {
                departmentListService.getExcludeSelf($scope.companySelected.id, id).then(function (data) {
                    $scope.parentDepartments = data.data;
                }, function (error) {
                    $rootScope.ngMessage = messageService.prepareResponseMessage(error);
                });
            }
            else {
                departmentListService.getDepartmentList($scope.companySelected.id).then(function (data) {
                    $scope.parentDepartments = data.data;
                }, function (error) {
                    $rootScope.ngMessage = messageService.prepareResponseMessage(error);
                });
            }
        };

        function refreshDepartmentHeadAutoCompleteData(id) {
            if (id && id !== '') {
                employeeListService.getExcludeSelf($scope.companySelected.id, id).then(function (data) {
                    $scope.departmentHeads = data.data;
                }, function (error) {
                    $rootScope.ngMessage = messageService.prepareResponseMessage(error);
                });
            }
            else {
                employeeListService.get($scope.companySelected.id).then(function (data) {
                    $scope.departmentHeads = data.data;
                }, function (error) {
                    $rootScope.ngMessage = messageService.prepareResponseMessage(error);
                });
            }
        };

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

        $scope.toolbarTemplate = kendo.template($('#toolbar-department').html());

        $scope.popupEditorTemplate = kendo.template($("#popup_editor_department").html());

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
            //departmentsDataSource.filter({ field: 'companyId', operator: 'eq', value: $scope.companySelected.id })
        };

        $scope.cancel = function (e) {
            toggleToolbar();
            $scope.addMode = false;
        };

        $scope.new = function (e) {
            var grid = e.grid;
            grid.addRow();
            refreshParentDepartmentDropDownListData('');
            refreshDepartmentHeadAutoCompleteData('');

            //$scope.parentDepartment = { id: '', name: '' };

            $('[name="parentDepartmentId"]').val('');
            $('[name="parentDepartmentName"]').val('');

            toggleToolbar();
            $scope.addMode = true;
        };

        $scope.edit = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                $('[name="createdByUserName"]').attr("readonly", true);
                var id = $('[name="id"]').val();
                var parentDepartmentId = $('[name="parentDepartmentId"]').val();
                var parentDepartmentName = $('[name="parentDepartmentName"]').val();
                var departmentHeadEmployeeId = $('[name="departmentHeadEmployeeId"]').val();
                refreshParentDepartmentDropDownListData(id);
                refreshDepartmentHeadAutoCompleteData(departmentHeadEmployeeId);

                //$scope.parentDepartment = { id: parentDepartmentId, name: parentDepartmentName };

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

        //function grid_edit(e) {
        //    if (e.model.isNew() && !e.model.dirty) {
        //        e.container
        //            .find("input[company.name]")
        //            .val($scope.companySelected.name)
        //        .change();
        //    }
        //};

        $scope.handleChange = function (data, dataItem, columns) {
            dataItem.dirty = true;
            if (dataItem.parentDepartmentName &&
                dataItem.parentDepartmentName !== '') {
                var department = $.grep($scope.parentDepartments, function (e) {
                    return e.name == dataItem.parentDepartmentName;
                });
                dataItem.parentDepartmentId = department[0].id;
            } else {
                dataItem.parentDepartmentId = '';
            }            
        };

        $scope.handleAutoCompleteChange = function (data, dataItem, columns) {
            dataItem.dirty = true;
            if (dataItem.departmentHeadUserName &&
                dataItem.departmentHeadUserName !== "") {                
                // get employeeId
                var employee = $.grep($scope.departmentHeads, function(e) {
                    return e.userName == dataItem.departmentHeadUserName;
                });
                dataItem.departmentHeadEmployeeId = employee[0].id;
            }
            else {
                dataItem.departmentHeadEmployeeId = "";
                dataItem.departmentHeadUserName = "";
            }
        };
    }]);