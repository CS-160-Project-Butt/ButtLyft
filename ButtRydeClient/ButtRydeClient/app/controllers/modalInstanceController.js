'use strict';

app.controller('ModalInstanceController', function ($scope, $uibModalInstance, items, name) {
    $scope.items = items;
    $scope.name = name;
    $scope.selected = {
        item: 'delete?'
    };

    $scope.ok = function () {
        $uibModalInstance.close($scope.selected.item);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});