/**=========================================================
 * Module: upload.js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.forms')
        .controller('PhotoUploadController', PhotoUploadController).directive('progressBar', [
        function () {
            return {
                link: function ($scope, el, attrs) {
                    $scope.$watch(attrs.progressBar, function (newValue) {
                        el.css('width', newValue.toString() + '%');
                    });
                }
            };
        }
        ]);


    PhotoUploadController.$inject = ['$scope', '$http', '$rootScope'];
    function PhotoUploadController($scope, $http, $rootScope) {
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
                        if (response.data.IsSuccess) {
                            var message = {
                                Title: vm.title,
                                Description: vm.description,
                                Images: response.data.Images,
                                Pk: response.data.Pk
                        };
                            $rootScope.$emit("newItemPortfolio", message);
                        }                       
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
        }
    }
})();
