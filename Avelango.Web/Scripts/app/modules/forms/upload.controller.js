/**=========================================================
 * Module: upload.js
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('app.forms')
        .controller('FileUploadController', FileUploadController);

    FileUploadController.$inject = ['FileUploader'];
    function FileUploadController(FileUploader) {
        var vm = this;

        activate();

        ////////////////

        function activate() {

            $scope.model = {};
            $scope.selectedFile = [];
            $scope.uploadProgress = 0;

            vm.uploadFile = function () {
                var formData = new FormData();
                for (var i = 0; i < $scope.selectedFile.length; i++) {
                    formData.append('files', $scope.selectedFile[i]);
                }
                formData.append('pk', JSON.stringify(null));
                formData.append('title', JSON.stringify(vm.title));
                formData.append('description', JSON.stringify(vm.description));
                var req = {
                    url: '/MyUser/PortfolioJobUpLoad',
                    method: 'POST',
                    withCredentials: false,
                    data: formData,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                };
                $http(req)
                    .then(function (response) {
                        console.log(response);
                    }, function (error) {
                        vm.authMsg = 'Server Request Error';
                        console.log('Error' + error);
                    });
                //$scope.upload = $upload.upload({
                //    url: '/MyUser/SetMyLogo',
                //    method: 'POST',
                //    data: angular.toJson($scope.model),
                //    file: file
                //}).progress(function (evt) {
                //    $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total, 10);
                //}).success(function (data) {

                //});
            };

            $scope.onFileSelect = function ($files) {
                console.log($files);
                $scope.uploadProgress = 0;
                $scope.selectedFile = $files;
            };





          var uploader = vm.uploader = new FileUploader({
             url: '/MyUser/SetMyLogo'
          });

          //// FILTERS

          //uploader.filters.push({
          //    name: 'customFilter',
          //    fn: function(/*item, options*/) {
          //        return this.queue.length < 10;
          //    }
          //});

          //// CALLBACKS

          //uploader.onWhenAddingFileFailed = function(item /*{File|FileLikeObject}*/, filter, options) {
          //    console.info('onWhenAddingFileFailed', item, filter, options);
          //};
          //uploader.onAfterAddingFile = function(fileItem) {
          //    console.info('onAfterAddingFile', fileItem);
          //};
          //uploader.onAfterAddingAll = function(addedFileItems) {
          //    console.info('onAfterAddingAll', addedFileItems);
          //};
          //uploader.onBeforeUploadItem = function(item) {
          //    console.info('onBeforeUploadItem', item);
          //};
          //uploader.onProgressItem = function(fileItem, progress) {
          //    console.info('onProgressItem', fileItem, progress);
          //};
          //uploader.onProgressAll = function(progress) {
          //    console.info('onProgressAll', progress);
          //};
          //uploader.onSuccessItem = function(fileItem, response, status, headers) {
          //    console.info('onSuccessItem', fileItem, response, status, headers);
          //};
          //uploader.onErrorItem = function(fileItem, response, status, headers) {
          //    console.info('onErrorItem', fileItem, response, status, headers);
          //};
          //uploader.onCancelItem = function(fileItem, response, status, headers) {
          //    console.info('onCancelItem', fileItem, response, status, headers);
          //};
          //uploader.onCompleteItem = function(fileItem, response, status, headers) {
          //    console.info('onCompleteItem', fileItem, response, status, headers);
          //};
          //uploader.onCompleteAll = function() {
          //    console.info('onCompleteAll');
          //};

          //console.info('uploader', uploader);
        }
    }
})();
