app.controller('ViewEditModalInstance', ['$uibModalInstance', 'authService','claFormData', 'claFormService', function ($uibModalInstance,authService, claFormData, claFormService) {

    vm = this;
    vm.claFormViewModel = angular.copy(claFormData);

    vm.getSalesReps = claFormService.getSalesReps().then(function (data) {
        vm.claFormViewModel.salesReps = data.data.data;
        vm.pristineClaFormViewModel = angular.copy(vm.claFormViewModel);
    });

    vm.save = function () {
        if (angular.equals(vm.claFormViewModel, vm.pristineClaFormViewModel)) {
            $uibModalInstance.dismiss('cancel');
        }
        else { $uibModalInstance.close(vm.claFormViewModel); }
    };

    vm.cancel = function () {
        $uibModalInstance.dismiss('cancel');
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