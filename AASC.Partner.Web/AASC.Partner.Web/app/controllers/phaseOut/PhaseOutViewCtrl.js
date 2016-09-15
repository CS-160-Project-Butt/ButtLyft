'use strict';
app.controller('PhaseOutViewCtrl', ['$q', '$routeParams', '$http', '$location', '$anchorScroll', '$uibModal', 'authService', 'phaseOutService', 'claFormViewModel',
    function ($q, $routeParams, $http, $location, $anchorScroll, $uibModal, authService, phaseOutService, claFormViewModel) {
        var vm = this;

        vm.product = {};
        vm.errorMessage = '';

        vm.toDisplayName = function (name) {
            var temp;
            var temp = name.replace(/([A-Z])/g, ' $1').trim();
            return temp;
        }

        vm.animationsEnabled = true;

        vm.toAdminPage = function () {
            $location.path('/phaseout/execution');
        };

        vm.open = function (size) {
            //var modalInstance = $uibModal.open({
            //    animation: vm.animationsEnabled,
            //    templateUrl: '/app/views/claform/viewEditModal.html',
            //    controller: 'ViewEditModalInstance as vm',
            //    size: size,
            //    resolve: {
            //        claFormData: function () {
                        
            //            return vm.claFormViewModel;
            //        }
            //    }
            //});

            //modalInstance.result.then(function (formData) {
            //    vm.claFormViewModel = formData;
            //    claFormService.updateData(formData)
            //}, function () {
            //    console.log("dismissed")
            //});

        }


        vm.product = {};
        //initialization of the view
        var init = function () {

            vm.product = {};


            vm.product = phaseOutService.getProduct($routeParams.id);
            


            //var deferred = $q.defer();

            //$http.get(serviceBase + '/api/cla/getcla?id=' + $routeParams.id).success(function (claData) {
            //    vm.claFormViewModel = claData.data[0];
            //    deferred.resolve(claData.data[0]);
            //    vm.claFormViewModel.deviceCategories = angular.fromJson(vm.claFormViewModel.deviceCategories);
            //    vm.claFormViewModel.productList = angular.fromJson(vm.claFormViewModel.productList);

            //    //json boolean parsing
            //    //angular.forEach(vm.claFormViewModel.deviceCategories, function (value, key) {

            //    //    if (value.Selected == "true")
            //    //    { value.Selected = true; }
            //    //    else { value.Selected = false; }
            //    //});

            //}).error(function (err, status) {
            //    deferred.reject(err);
            //    if (err) {
            //        vm.errorMessage = err.message; // "Something went wrong";
            //    }

            //    //alert("Something went wrong");
            //});
            //return deferred.promise;
        };
        init();

        //vm.getSalesReps = claFormService.getSalesReps().then(function (data) {
        //    vm.claFormViewModel.salesReps = data.data.data;
        //});







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
