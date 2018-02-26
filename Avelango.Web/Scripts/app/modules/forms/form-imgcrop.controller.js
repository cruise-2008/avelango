/**=========================================================
 * Module: form-imgcrop.js
 * Image crop controller
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('app.forms')
        .controller('ImageCropController', imageCropController);

    imageCropController.$inject = ['$scope', '$http', '$uibModal', '$rootScope', 'sweetAlert'];
    function imageCropController($scope, $http, $uibModal, $rootScope, sweetAlert) {
        var vm = this;

        activate();

        function activate() {
            var sendImgPath = function (pathes) {
                $rootScope.$emit("newImgPath", pathes);
            };

        vm.reset = function () {
            vm.myImage        = '';
            vm.myCroppedImage = '';
            vm.imgcropType    = 'square';
        };

        vm.errorMessage = function(message) {
                sweetAlert.swal('Cancelled', message, 'error');
            };
            
        vm.reset();
           
            $scope.sendImg = function () {
              var img = vm.myCroppedImage.replace('data:image/png;base64,', '');
              $http
                      .post('/MyUser/SetMyLogo', { image: img })
                      .then(function (response) {
                          console.log(response.data);
                      if (response.data.IsSuccess) {
                          var patches = [response.data.UserLogoPathMax,response.data.UserLogoPath];
                          sendImgPath(patches);
                          $rootScope.$emit("closeModal", true);
                      } else {
                          console.log('Error1');
                      }


                  }, function () {
                      console.log('Server Request Error');
                      vm.authMsg = 'Server Request Error';
                      });
              
          };

          var handleFileSelect = function (evt) {
            var file=evt.currentTarget.files[0];
            if (!file.type.match('image.*')) {
                return vm.errorMessage('Error, file must be a photo.');
            }
            if (file.size / 1024 / 1024 >= 4) {
                vm.reset();
                return vm.errorMessage('File size should be smaller than 4MB');
            }

            var reader = new FileReader();
            reader.onload = function (evt) {
              $scope.$apply(function(/*$scope*/){
                vm.myImage=evt.target.result;
              });
            };
            if(file)
              reader.readAsDataURL(file);
          };
          
          angular.element(document.querySelector('#fileInput')).on('change',handleFileSelect);
        }
    }
})();
