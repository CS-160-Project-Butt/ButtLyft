'use strict';
app.controller('ClaFormCtrl', ['$q', '$http', '$location', '$anchorScroll', 'authService', 'claFormService', 'claFormViewModel', function ($q, $http, $location, $anchorScroll, authService, claFormService, claFormViewModel) {
    var vm = this;

    vm.claFormViewModel = {};
    vm.errorMessage = '';

    vm.tempProductList = {};//this is used for persistent data

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



    //in case we want pagination in the future///////////////////////////////////
    //vm.totalItems = 40;
    //vm.currentPage = 2;

    //vm.setPage = function (pageNo) {
    //    vm.currentPage = pageNo;
    //};

    ////vm.pageChanged = function () {
    ////    $log.log('Page changed to: ' + vm.currentPage);
    ////};

    //vm.maxSize = 5;
    //vm.bigTotalItems = 175;
    //vm.bigCurrentPage = 1;


    //vm.increaseSize = function () {
    //    vm.maxSize++;

    //}






}]);