'use strict';
app.controller('fileUploadController', ['$scope', 'FileUploader', 'localStorageService', function ($scope, FileUploader, localStorageService) {
    $scope.folder = 'ProductFiles';
    $scope.note = 'file for test';

    var uploader = $scope.uploader = new FileUploader({
        //url: serviceBase + 'api/files/upload',
        //formData: [{ "folder": $scope.folder, "note": $scope.note }]
    });

    // FILTERS
    uploader.filters.push({
        name: 'customFilter',
        fn: function (item, options) {
            return this.queue.length < 10;
        }
    });

    // CALLBACKS

    uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader.onAfterAddingFile = function (fileItem) {
        console.info('onAfterAddingFile', fileItem);
    };
    uploader.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader.onBeforeUploadItem = function (item) {
        item.headers = {
            'Authorization': 'Bearer ' + localStorageService.get('authorizationData').token
        };
        item.formData[0].folder = folder.value;
        item.formData[0].note = note.value;
        console.info('onBeforeUploadItem', item);
    };
    uploader.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };
    uploader.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };
    uploader.onSuccessItem = function (fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);
    };
    uploader.onErrorItem = function (fileItem, response, status, headers) {
        console.info('onErrorItem', fileItem, response, status, headers);
    };
    uploader.onCancelItem = function (fileItem, response, status, headers) {
        console.info('onCancelItem', fileItem, response, status, headers);
    };
    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
        alert("file uploaded!");
    };
    uploader.onCompleteAll = function () {
        console.info('onCompleteAll');
        //alert("file uploaded all!");
    };

    console.info('uploader', uploader);
}]);