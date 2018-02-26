/**=========================================================
 * Module: upload.js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.forms')
        .controller('OnlyPhotoUploadController', OnlyPhotoUploadController);






    OnlyPhotoUploadController.$inject = ['$scope', '$http', '$rootScope'];
    function OnlyPhotoUploadController($scope, $http, $rootScope) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            $scope.model = {};
            $scope.selectedFile = [];
            $scope.uploadProgress = 0;

            vm.uploadFile = function (job) { 
                var formData = new FormData();
                for (var i = 0; i < $scope.selectedFile.length; i++) {
                    formData.append('files', $scope.selectedFile[i]);
                }
                formData.append('pk', JSON.stringify(job.Pk));

                var req = {
                    url: '/MyUser/AddPortfolioJobImage',
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
                                Images: response.data.Images,
                                job: job
                        };

                            $rootScope.$emit("newImgPortfolio", message);
                        }                       
                      }, function (error) {
                          vm.authMsg = 'Server Request Error';
                          console.log('Error' + error);
                      });
            };

            $scope.onFileSelec = function ($files) {
                console.log($files);
                $scope.uploadProgress = 0;
                $scope.selectedFile = $files;
            };
        }
    }
})();
