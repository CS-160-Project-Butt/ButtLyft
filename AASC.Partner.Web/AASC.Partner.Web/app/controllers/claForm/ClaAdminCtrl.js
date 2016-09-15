'use strict';
app.controller('ClaAdminCtrl', ['$q', '$rootScope', '$http', '$location', 'authService', 'ngAuthSettings', 'claFormService',
    'claFormViewModel', 'claFormDataSource', 'dialogService', 'messageService', 'messageDataService',
    function ($q, $rootScope, $http, $location, authService, ngAuthSettings, claFormService,
        claFormViewModel, claFormDataSource, dialogService, messageService, messageDataService) {
        var vm = this;

        vm.claFormViewModel = {};
        vm.errorMessage = '';

        vm.tempProductList = {};//this is used for persistent data

        vm.redirectNewForm = function () {
            $location.path('/claform');
        };
        vm.redirectMySelf = function () {
            window.location.reload();
        };

        vm.submitForm = function () {

            var deferred = $q.defer();

            angular.forEach(vm.tempProductList, function (value, key) {
                if (!angular.isNumber(value.quantity)) value.quantity = 0;
            });

            if (!angular.isNumber(vm.claFormViewModel.otherQuantity)) vm.claFormViewModel.otherQuantity = 0;

            vm.claFormViewModel.productList = vm.tempProductList;//populating the model to be sent to server

            $http.post(serviceBase + 'api/cla/addcla', vm.claFormViewModel).success(function (response) {

                deferred.resolve(response);

                $location.path('/claform/thanks');

            }).error(function (err, status) {
                deferred.reject(err);
                if (err) {
                    vm.errorMessage = err.message; // "Something went wrong";
                }

                //alert("Something went wrong");
            });
            return deferred.promise;
        }


        vm.getDevices = claFormService.getDevices().then(function (data) {
            vm.claFormViewModel.deviceCategories = data.data.data;
        });

        vm.getTypes = claFormService.getProductTypes().then(function (data) {
            vm.claFormViewModel.productTypes = data.data.data;
        });

        vm.getProducts = claFormService.getProductList().then(function (data) {
            vm.tempProductList = data.data.data;
            angular.forEach(vm.tempProductList, function (value, key) {
                value.quantity = null;
            });
        });

        vm.getSalesReps = claFormService.getSalesReps().then(function (data) {
            vm.claFormViewModel.salesReps = data.data.data;
        });













        vm.dataSource = claFormDataSource;
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
                    { field: "id", title: "Id", hidden: true },
                    { field: "companyName", title: "Company" },
                    { field: "taxID", title: "Tax ID", hidden: true },
                    { field: "salesContact", title: "Sales Contact" },
                    { field: "address", title: "Address", hidden: true },
                    { field: "city", title: "City", hidden: true },
                    { field: "country", title: "Country", hidden: true },
                    { field: "postCode", title: "Postal Code", hidden: true },
                    { field: "signerFirstName", title: "Signer's First Name" },
                    { field: "signerLastName", title: "Signer's Last Name", hidden: true },
                    { field: "signerEmail", title: "Signer's Email" },
                    { field: "signerJobTitle", title: "Signer's Job Title", hidden: true },
                    { field: "signerPhoneNumber", title: "Signer's Phone Number", hidden: true },
                    { field: "technicalFirstName", title: "Technical's First Name", hidden: true },
                    { field: "technicalLastName", title: "Technical's Last Name", hidden: true },
                    { field: "technicalEmail", title: "Technical's Email", hidden: true },
                    { field: "technicalJobTitle", title: "Technical's Job Title", hidden: true },
                    { field: "technicalPhoneNumber", title: "Technical's Phone Number", hidden: true },
                    { field: "deviceCategories", title: "Device Categories", hidden: true },
                    { field: "productList", title: "Product List", hidden: true },
                    { field: "cLANumber", title: "CLA Number" },
                    { field: "cLAStatus", title: "CLA Status", hidden: true },
                    { field: "cLAStatusDate", title: "CLA Status Date", hidden: true },
                    { field: "customerERPID", title: "Customer ERPID", hidden: true },
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
                $location.path('/claform/view/' + $('[name="id"]').val());

            });

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


        vm.setProductList = function () {
            $('[name="productList"]').val(angular.toJson(vm.editorTempProductList));
            $('[name="productList"]').trigger("change");
        }


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

