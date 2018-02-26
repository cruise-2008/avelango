(function () {
    'use strict';

    angular
        .module('app.user.info.controller', ['ui.bootstrap'])
        .controller('UserInfoCtrl', userInfoCtrl);

    userInfoCtrl.$inject = ['$http', '$rootScope', '$map', '$scope', 'urlSvc', '$uibModal', '$compile', 'sweetAlert'];
    function userInfoCtrl($http, $rootScope, $map, $scope, urlSvc, $uibModal, $compile, sweetAlert) {

        var vm = $scope;
        var vmm = this;
        vm.activate = false;

        vm.autocomplite = {};



        //*START CHANGE AVATAR*//

        vm.reset = function () {
            vm.myImage = '';
            vm.myCroppedImage = '';
            vm.imgcropType = 'square';
        };
        vm.reset();

       // vm.selectedFiles = {};

        vm.checkaddHandler = false;

        vm.addHandler = function () {
            if (vm.checkaddHandler === true) {
                return;
            } else {
                var handleFileSelect = function (evt) {
                    var file = evt.currentTarget.files[0];
                    if (!file.type.match('image.*')) {
                        return sweetAlert.swal('Error, file must be a photo.');
                    }
                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            $scope.myImage = evt.target.result;
                        });
                    };
                    reader.readAsDataURL(file);
                };
                angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);
                vm.checkaddHandler = true;
            }
        }




        vm.close = function () {
            $scope.modalInstance.close({
                templateUrl: '/UserImgPath.html',
                controller: userInfoCtrl,
            });
        }


        vm.sendImg = function () {
            var img = vm.myCroppedImage.replace('data:image/png;base64,', '');
            $http
                .post('/MyUser/SetMyLogo', { image: img })
                .then(function (response) {
                    console.log(response.data);
                    if (response.data.IsSuccess) {
                        $rootScope.changeImage(response.data.UserLogoPathMax);
                        vm.close();
                        sweetAlert.swal('Аватарка изменена успешно, я Вас поздравляю.');
                    } else {
                        console.log('Error1');
                    }
                });
        };










        //*END CHANGE AVATAR*//

        $scope.$on('userInfo', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {

                    $http({
                        url: urlSvc.GetMyInfo,
                        method: 'GET'
                    }).then(function (data) {
                        if (data.data) {
                            $scope.user.Name = data.data.Name;
                            $scope.user.SurName = data.data.SurName;
                            $scope.user.Birthday = data.data.Birthday;
                            $scope.user.Gender = data.data.Gender,
                            $scope.user.PlaceName = data.data.PlaceName,
                            $scope.user.PlaceLat = data.data.PlaceLat,
                            $scope.user.PlaceLng = data.data.PlaceLng,
                            $scope.user.Phone1 = data.data.Phone1,
                            $scope.user.Phone2 = data.data.Phone2,
                            $scope.user.Phone3 = data.data.Phone3,
                            $scope.user.Email = data.data.Email,
                            $scope.user.Skype = data.data.Skype,
                            vmm.UserLogoPathMax = data.data.UserLogoPathMax;
                            //$scope.IsActive = data.data.IsActive; //                  !!!!for moderation ban or not ban user check!!!
                        }
                    });

                    $rootScope.changeImage = function (imgPath) {
                        vmm.UserLogoPathMax = imgPath;
                        $rootScope.$broadcast('changeImgLayout', imgPath);   //for emit change ava in layout
                    }



                    //*Start Birthday Picker *//

                    //var today = new Date(2000, 0, 1);
                    $scope.AvailableDate = new Date(2000, 0, 1);

                    $scope.dateFormat = 'dd-MM-yyyy';
                    $scope.availableDateOptions = {
                        formatYear: 'yy',
                        startingDay: 1,
                        minDate: new Date(1940, 0, 30),
                        maxDate: new Date(2010, 11, 31),
                        showWeeks: false
                    };
                    $scope.availableDatePopup = {
                        opened: false
                    };

                    $scope.OpenAvailableDate = function () {
                        $scope.availableDatePopup.opened = !$scope.availableDatePopup.opened;
                    };

                    //*End Birthday Picker *//



                    /*START INFO USER*/
                    $scope.user = {};
                    
                    vm.setNewUserInfo = function () {
                    
                        if (true) {

                            var ff = vm.autocomplite;

                            //*START PlaceName Latituta Langituda*//
                            vm.place = $map.getLocation(location);
                            //*END PlaceName Latituta Langituda*//


                            $http
                                .post(urlSvc.SetMyInfo, {
                                    name: $scope.user.Name,
                                    surName: $scope.user.SurName,
                                    birthday: $scope.user.birthday,
                                    gender: $scope.user.Gender, 
                                    placeName: vm.place.place, //$scope.user.PlaceName,
                                    placeLat: vm.place.lat,//$scope.user.PlaceLat == undefined ? 0 : $scope.user.PlaceLat, 
                                    placeLng: vm.place.lng,//$scope.user.PlaceLng == undefined ? 0 : $scope.user.PlaceLng, 
                                    phone2: $scope.user.Phone2,
                                    phone3: $scope.user.Phone3,
                                    email: $scope.user.Email,
                                    skype: $scope.user.Skype
                                })
                                .then ( function (response) {
                                    if (response.data) {
                                        sweetAlert.swal('Response successfully');
                                        console.log(response.data);
                                    } else {
                                        sweetAlert.swal('Incorect Data');
                                    }
                                },
                                function () {
                                    $scope.authMsg = 'Server Request Error';
                                });
                        } 
                    };
                    
                    /*END INFO USER*/



                    //*START Location*//
                    vm.autocomplite = $map.autocomplite('inpt5');
                    //*END Location*//





                    //*START MODAL WINDOW*//
                    vm.open = function () {

                        $http({
                            method: 'GET',
                            url: '/MyUser/ChangeAvatarModule'
                        })
                            .success(function (response) {
                                $("#changeAvatarModule").append($compile(response)($scope));
                                $rootScope.modalInstance = $uibModal.open({
                                    templateUrl: '/UserImgPath.html',
                                    controller: userInfoCtrl,
                                    backdrop: 'static',
                                    keyboard: false
                                });
                            })
                            .error(function () {
                                sweetAlert.swal('Error, sorry');

                            });
                    }
                    //*END MODAL WINDOW*//













                    vm.activate = true;
                }
            }
        });
    }

})();