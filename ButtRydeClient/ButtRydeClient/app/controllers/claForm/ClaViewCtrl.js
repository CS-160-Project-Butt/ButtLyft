'use strict';
app.controller('ClaViewCtrl', ['$q', '$routeParams', '$http', '$location', '$anchorScroll', '$uibModal', 'authService', 'claFormService', 'claFormViewModel',
    function ($q, $routeParams, $http, $location, $anchorScroll, $uibModal, authService, claFormService, claFormViewModel) {
        var vm = this;

        vm.claFormViewModel = {};
        vm.errorMessage = '';
        vm.editMode = false;
        vm.claStatuses = [
            { status: "User Submitted" },
            { status: "Approved" }
        ]


        //controls whether the form is capable of change from user input
        vm.toggleEdit = function () {


            if (vm.editMode == true) {
                vm.claFormViewModel = angular.copy(vm.cleanClaForm);
            }
            vm.editMode = !vm.editMode;

            if (vm.isUserInRoles(null)) {
                vm.editMode = false;
            }

        }

        vm.toDisplayName = function (name) {
            var temp;
            var temp = name.replace(/([A-Z])/g, ' $1').trim();
            return temp;
        }

        vm.animationsEnabled = true;

        vm.toAdminPage = function () {
            $location.path('/claform/admin');
        };

        vm.getDisplayDate = function (date) {
            return new Date(date).toString();
        }

        vm.submitForm = function () {
            if (!angular.equals(vm.claFormViewModel, vm.cleanClaForm)) {
                if (!angular.equals(vm.claFormViewModel.claStatus, vm.cleanClaForm.claStatus)) {
                    vm.claFormViewModel.claStatusDate = new Date();
                }

                claFormService.updateData(vm.claFormViewModel).then(function () {
                    vm.cleanClaForm = vm.claFormViewModel;
                    vm.toggleEdit();

                });

            }
            else { console.log("they are the same!") };
        }

        //initialization of the view
        var init = function () {

            vm.getSalesReps = claFormService.getSalesReps().then(function (data) {
                vm.claFormViewModel.salesReps = data.data.data;
                vm.cleanClaForm = angular.copy(vm.claFormViewModel);

            });
            $routeParams.edit ? vm.editMode = true : vm.editMode = false;
            if (vm.isUserInRoles(null)) {
                vm.editMode = false;
            }
            var deferred = $q.defer();

            $http.get(serviceBase + '/api/cla/getcla?id=' + $routeParams.id).success(function (claData) {
                vm.claFormViewModel = claData.data[0];
                deferred.resolve(claData.data[0]);
                vm.claFormViewModel.deviceCategories = angular.fromJson(vm.claFormViewModel.deviceCategories);
                vm.claFormViewModel.productList = angular.fromJson(vm.claFormViewModel.productList);
                vm.claFormViewModel.productTypes = vm.productTypes;
                //json boolean parsing
                //angular.forEach(vm.claFormViewModel.deviceCategories, function (value, key) {

                //    if (value.Selected == "true")
                //    { value.Selected = true; }
                //    else { value.Selected = false; }
                //});

            }).error(function (err, status) {
                deferred.reject(err);
                if (err) {
                    vm.errorMessage = err.message; // "Something went wrong";
                }

                //alert("Something went wrong");
            });
            return deferred.promise;
        };
        vm.getStatuses = claFormService.getStatuses().then(function (data) {
            vm.claStatuses = data.data.data;
            console.log(data.data.data);
        });

        vm.getDevices = claFormService.getDevices().then(function (data) {
            vm.deviceCategories = data.data.data;
        }).then(function () {

            vm.getTypes = claFormService.getProductTypes().then(function (data) {
                vm.productTypes = data.data.data;
            }).then(function () {

                vm.getProducts = claFormService.getProductList().then(function (data) {
                    vm.tempProductList = data.data.data;
                    angular.forEach(vm.tempProductList, function (value, key) {
                        //value.quantity = null;
                    })

                }).then(function () {

                    init().then(function () {

                        vm.getSalesReps = claFormService.getSalesReps().then(function (data) {
                            vm.claFormViewModel.salesReps = data.data.data;
                            vm.cleanClaForm = angular.copy(vm.claFormViewModel);
                        });
                    });

                });
            });
        });



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

            if (roles == null) {
                if (vm.authentication.roles.length > 0) {
                    return false;
                }
                return true;
            }


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