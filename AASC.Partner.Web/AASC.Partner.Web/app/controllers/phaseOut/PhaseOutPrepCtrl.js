'use strict';
app.controller('PhaseOutPrepCtrl', ['$q', '$rootScope', '$http', '$location', 'authService', 'ngAuthSettings', 'phaseOutService',
    'phaseOutPrepViewModel', 'phaseOutPrepDataSource', 'dialogService', 'messageService', 'messageDataService',
    function ($q, $rootScope, $http, $location, authService, ngAuthSettings, phaseOutService,
        phaseOutPrepViewModel, phaseOutPrepDataSource, dialogService, messageService, messageDataService) {
        var vm = this;

        vm.claFormViewModel = {};
        vm.errorMessage = '';

        vm.tempProductList = {};//this is used for persistent data

        vm.redirectMySelf = function () {
            window.location.reload();
        };


        //vm.getDevices = claFormService.getDevices().then(function (data) {
        //    vm.claFormViewModel.deviceCategories = data.data.data;
        //});

        //vm.getTypes = claFormService.getProductTypes().then(function (data) {
        //    vm.claFormViewModel.productTypes = data.data.data;
        //});

        //vm.getProducts = claFormService.getProductList().then(function (data) {
        //    vm.tempProductList = data.data.data;
        //    angular.forEach(vm.tempProductList, function (value, key) {
        //        value.quantity = null;
        //    });
        //});

        //vm.getSalesReps = claFormService.getSalesReps().then(function (data) {
        //    vm.claFormViewModel.salesReps = data.data.data;
        //});













        vm.dataSource = phaseOutPrepDataSource;
        //applicationUsersDataSource.filter({}); // reset filter on dataSource every time view is loaded                            
        ////scope.$on('dataSourceMessage', function (event, data) {
        ////    $rootScope.$broadcast('ngMessage', data);
        ////});

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

        vm.toggleToolbar = function () {
            vm.isToolbarVisible = !vm.isToolbarVisible;
        };

        vm.setGridColumns = function (grid) {
            vm.gridColumns = angular.copy(grid);
        };

        vm.getGridColumns = function () {
            vm.tempGrid = angular.copy(vm.gridColumns);
            return vm.tempGrid;
        };
        vm.gridColumns = [
                    { field: "phased", title: "Phase Out"},
                    { field: "id", title: "Id", hidden: true },
                    { field: "productName", title: "Product Name"},
                    { field: "partNumber", title: "Part Number"},
                    { field: "description", title: "Description", hidden: true },
                    { field: "plmStatus", title: "PLM Status", hidden: true },
                    { field: "productFamily", title: "Product Family"},
                    { field: "lastBuyTime", title: "Last Buy Time", hidden: true },
                    { field: "replacement", title: "Replacement"},
                    { field: "createdBy", title: "Created By"},
                    { field: "createdDate", title: "Created Date", hidden: true }
        ];

        vm.tempGrid = angular.copy(vm.gridColumns);

        vm.toolbarTemplate = kendo.template($('#toolbar-claform').html());

        vm.popupEditorTemplate = kendo.template($("#popup_editor_cla").html());

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
            var view = vm.dataSource.view();
            // iterate through rows
            for (var i = 0; i < view.length; i++) {
                // find row with the lastSelectedId
                if (view[i].id == $rootScope.lastSelectedDataItem.id) {
                    var grid = e.sender;
                    // set the selected row
                    grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
                    break;
                }
            }


        };

        vm.save = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);

                grid.saveRow();
                function handleSuccess(data, status, headers, config) {
                    vm.toggleToolbar();
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
                vm.toggleToolbar();
                vm.addMode = false;
            });
        };


        vm.editorTempDeviceCategories;
        vm.editorTempProductList;

        vm.view = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                $location.path('/phaseout/view/' + $('[name="id"]').val());

            });

        }

        vm.setProductList = function () {
            $('[name="productList"]').val(angular.toJson(vm.editorTempProductList));
            $('[name="productList"]').trigger("change");
        }

        vm.togglePhased = function (e) {       
            var grid = $("#grid").data("kendoGrid");
            var selectedRows = $(".k-state-selected", "#grid");

            if (selectedRows.length > 0) {
                var selectedItem = grid.dataItem(selectedRows[0]);

                phaseOutService.togglePhased(selectedItem.id);
                //vm.dataSource.transport.read();
            }
            $("#grid").data("kendoGrid").dataSource.read(e);

        };

        vm.newProduct = function (e) {

            phaseOutService.generateProduct();

            $("#grid").data("kendoGrid").dataSource.read(e);

        }


        vm.edit = function (e) {
            onClick(e, function (grid, row) {
                grid.editRow(row);
                $('[name="createdBy.username"]').attr("readonly", true);

                vm.editorTempDeviceCategories = angular.fromJson($('[name="deviceCategories"]').val());

                //json boolean parsing
                //angular.forEach(vm.editorTempDeviceCategories, function (value, key) {

                //    if (value.Selected == "true")
                //    { value.Selected = true; }
                //    else { value.Selected = false; }

                //});
                //$('[name="deviceCategories"]')

                vm.toggleToolbar();
                vm.addMode = false;

                vm.editorTempProductList = angular.fromJson($('[name="productList"]').val());


                //json boolean parsing
                //angular.forEach(vm.editorTempProductList, function (value, key) {
                //    //if (value.Selected == "true")
                //    //{ value.Selected = true; }
                //    //else { value.Selected = false; }

                //});




            });
        };




        //applies the devices json strings with the entered amounts in the editor
        vm.setDeviceCategories = function () {

            //string to bool conversion + dirty flag setting
            //angular.forEach(vm.editorTempDeviceCategories, function (value, key) {
            //    if (value.Selected == true)
            //    { value.Selected = "true"; }
            //    else { value.Selected ="false"; }
            //});

            //var grid = $("#grid").data("kendoGrid");
            //var dataRow = grid.dataSource._data;
            //var deviceId = $('[name="id"]').val();


            //bool to string conversion
            //angular.forEach(dataRow, function (value, key) {
            //    if (value.id == deviceId) {
            //        value.dirty = true;
            //    }
            //});
            // var thign = vm.grid.get(deviceID);
            // thign.dirty = true;  // set it as dirty
            //grid.saveChanges();
            //grid.saveRow();


            angular.forEach(vm.editorTempDeviceCategories, function (value, key) {
                if (value.selected == "true")
                { value.selected = true; }
                else { value.selected = false; }
            });

            $('[name="deviceCategories"]').val(angular.toJson(vm.editorTempDeviceCategories));
            $('[name="deviceCategories"]').trigger("change");

        }


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













        function containsAny(source, target) {
            for (var i = 0; i < target.length; i++) {
                for (var j = 0; j < source.length; j++) {
                    if (target[i] === source[j])
                        return true;
                }
            }
            return false;
        };

        vm.roles = [];

        vm.authentication = authService.authentication;

        authService.getUserRoles().then(function (resRoles) {
            vm.roles = resRoles.data;
        });

        vm.isUserInRoles = function (roles) {
            var authorizedRoles = [];
            if (!angular.isArray(roles)) {
                authorizedRoles = [roles];
            } else {
                authorizedRoles = roles;
            }
            var result = containsAny(vm.authentication.roles, authorizedRoles);
            return result;
        };
    }]);

