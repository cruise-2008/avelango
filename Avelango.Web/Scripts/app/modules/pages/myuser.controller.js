/**=========================================================
 * Module: .js
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('app.pages')
        .controller('MyUserController', myUserController);

    myUserController.$inject = ['sweetAlert', '$map','$http', '$rootScope', '$location', '$scope', '$resource', '$timeout', 'urlSvc', '$uibModal'];

    function myUserController(sweetAlert, $map, $http, $scope, $rootScope, $location, $resource, $timeout, urlSvc, $uibModal) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            vm.account = {};

            vm.BirthdayYears = [];
            var currentYear = new Date().getFullYear() - 16;
            for (var i = 0; i < 85; ++i)
                vm.BirthdayYears[i] = currentYear - i;

            vm.BirthdayMonthes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

            var daysCount;
            vm.month = function(month, year) {
                if (month && year) {
                    switch (month) {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        daysCount = 31
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        daysCount = 30
                        break;
                    case 2:
                        if ((year % 4 == 0 && year % 100 !== 0) || year % 400 == 0) daysCount = 29
                        else daysCount = 28;
                        break;
                    default:
                    }
                    vm.Birthdaydays = [];
                    for (var i = 0; i < daysCount; i++)
                        vm.Birthdaydays[i] = i + 1;
                }
            }

            var autocomplite = $map.autocomplite('search');
            
            //var myAutocomplite = function() {
            //    var inputFrom = document.getElementById('search');
            //    var autocompleteFrom = new google.maps.places.Autocomplete(inputFrom, {});
            //    google.maps.event.addListener(autocompleteFrom, 'place_changed', function() {
            //        vm.showValidatemessage = false;
            //    });
            //}
            //
            //var interval = setInterval(function() { //это нужно для того чтобы функция myAutocomplite, использующая библиотеку гугл не выполнялась до загрузки этой библиотеки
            //    if (google != 'undefined') {
            //        myAutocomplite();
            //        clearInterval(interval);
            //    }
            //}, 100);

            //vm.myValidation = function() {
            //    vm.showValidatemessage = true;
            //}

            vm.path = 'mymessages';

            $scope.$on("newImgPath", function(event, bigimgpath) {
                vm.account.UserLogoPathMax = bigimgpath[0];
            });

            vm.editPersonalData = function() {
                vm.count = true;
                vm.authMsg = '';
                if (vm.userForm.$valid) {
                    vm.account.Gender = vm.account.Gender === 1 ? true : false
                    $http
                        .post(urlSvc.SetMyInfo, {
                            name: vm.account.Name,
                            surName: vm.account.SurName,
                            birthday: vm.account.selectedBirthdayYear + '-' + vm.account.selectedBirthdayMonth + '-' + vm.account.selectedBirthdayDay,
                            gender: vm.account.Gender,
                            place: vm.account.chosenPlace,
                            phone1: vm.account.Phone1,
                            phone2: vm.account.Phone2,
                            phone3: vm.account.Phone3,
                            email: vm.account.Email,
                            skype: vm.account.Skype
                        })
                        .then(function(response) {
                            if (response.data) {
                                sweetAlert.swal('Good job!', 'Your message processing', 'success');
                                console.log(response.data);
                            } else {
                                vm.authMsg = 'Incorect data';
                            }

                        }, function() {
                            vm.authMsg = 'Server Request Error';
                        });
                } else {
                    vm.userForm.account_Email.$dirty = true;
                    vm.userForm.account_Phone1.$dirty = true;
                    vm.userForm.account_Phone2.$dirty = true;
                    vm.userForm.account_Phone3.$dirty = true;
                }

            };

            var controller = ModalInstanceCtrl;

            ModalInstanceCtrl.$inject = ['$scope', '$uibModalInstance'];

            function ModalInstanceCtrl($scope, $uibModalInstance) {}

            vm.open = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/ModalUserImg.html',
                    controller: controller
                });
            }
            
            

            vm.timoutforblock = function () {
                $timeout(function () {
                    vm.successPassvord = false;
                    vm.authMsgPass = '';
                }, 3000);
            };

            vm.changepassword = function () {
                vm.count = true;
                vm.authMsgPass = '';

                if (vm.changepasswordForm.$valid) {
                    $http
                      .post(urlSvc.PasswordRecovery, { Password: vm.newPassword })
                      .then(function (response) {
                          // assumes if ok, response is an object with some data, if not, a string with error
                          // customize according to your api
                          if (!response.data.IsEnabled) {
                              vm.authMsgPass = 'Incorrect credentials.';
                              vm.timoutforblock();
                          } else {
                              vm.passvordfarm = false;
                              vm.successPassvord = 'Your message sended';
                              vm.timoutforblock();
                          }

                      }, function () {
                          vm.authMsgPass = 'Server Request Error';
                      });
                }
                else {
                    // set as dirty if the user click directly to login so we show the validation messages
                    /*jshint -W106*/
                    vm.changepasswordForm.input_password.$dirty = true;
                }
            };

            var url;
            vm.portfolio = {};
            vm.portfolio.Jobs = [];
            vm.request = {};

            $scope.$on("newItemPortfolio", function (e, message) {
                vm.portfolio.Jobs.push(message);
                vm.opennewtask = true;
            });

            $scope.$on("newImgPortfolio", function (e, message) {
                var jobnum = vm.portfolio.Jobs.indexOf(message.job);
                for (var i = 0, len = message.Images.length; i < len; i++) {
                    vm.portfolio.Jobs[jobnum].Images.push({ Small: message.Images[i].Small, Large: message.Images[i].Large });
                }

            });

            vm.setImage = function (imageUrl) {
                vm.mainImageUrl = imageUrl;
                console.log(imageUrl);
            };

            vm.sendchenges = function (option, $index) {
                switch (option) {
                    case 'userinfo':
                        url = urlSvc.AddPortfolioData;
                        vm.request = {
                            url: url,
                            method: 'POST',
                            data: {
                                title: angular.element(document.querySelector('#inputnewTitle')).val(),
                                description: vm.portfolio.textarea
                            }
                        };
                        break;
                    case 'portfolio':
                        url = urlSvc.ChangePortfolioJobData;
                        vm.request = {
                            url: url,
                            method: 'POST',
                            data: {
                                title: angular.element(document.querySelector('#inputnewTitleJob' + $index)).val(),
                                description: vm.portfolio.Jobs[$index].Description,
                                pk: vm.portfolio.Jobs[$index].Pk
                            }

                        };
                        break;
                    case 5:
                        alert('Перебор');
                        break;
                    default:
                        url = urlSvc.GetPortfolioData;
                        vm.request = {
                            url: url,
                            method: 'POST'
                        };
                };

                $http(vm.request)
                      .then(function (response) {

                          if (response.data.Method === 'cpjd') {
                              vm.portfolio.Jobs[$index].Title = angular.element(document.querySelector('#inputnewTitleJob' + $index)).val(),

                              vm.openeditfoto[$index] = true;

                          } else if (response.data.IsSuccess) {

                              for (var i = 0; i < response.data.Portfolio.Jobs.length; i++) {
                                    vm.portfolio.Jobs.push(response.data.Portfolio.Jobs[i])
                              }


                              // vm.portfolio.Title = vm.request.data.title;
                              // vm.portfolio.Description = vm.request.data.description;
                              // vm.openedit = true;
                          } else if (response.data.Title) {
                              vm.portfolio = response.data;
                              vm.portfolio.textarea = vm.portfolio.Description;

                          }

                      }, function (error) {
                          vm.authMsg = 'Server Request Error';
                          console.log('Error' + error);
                      });

            };

            vm.sendchenges();

            vm.deljob = function (jobPk, index) {
                sweetAlert.swal({
                    title: 'Are you sure?',
                    text: 'Your will not be able to recover this user!',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, delete it!',
                    cancelButtonText: 'Cancel',
                    closeOnConfirm: false,
                    closeOnCancel: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        $resource(urlSvc.RemovePortfolioJob, { pk: jobPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                            if (response.IsSuccess) {
                                vm.portfolio.Jobs.splice(index, 1);
                                sweetAlert.swal('Deleted!', 'Your job has been deleted.', 'success');
                            } else {
                                sweetAlert.swal('Cancelled', 'Some Error', 'error');
                            }
                        });
                    } else {
                        sweetAlert.swal('Cancelled', 'Your job is safe :)', 'error');
                    }

                });
            }

            vm.removePhoto = function (index, photopath, arr) {
                console.log(arr);
                sweetAlert.swal({
                    title: 'Are you sure?',
                    text: 'Your will not be able to recover this user!',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, delete it!',
                    cancelButtonText: 'Cancel',
                    closeOnConfirm: false,
                    closeOnCancel: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        $resource(urlSvc.RemovePortfolioJobImage, { imageSmall: photopath.Small, imageLarge: photopath.Large }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                            if (response.IsSuccess) {
                                var indexJob = vm.portfolio.Jobs.indexOf(arr);
                                delete vm.portfolio.Jobs[indexJob].Images[index];
                                sweetAlert.swal('Deleted!', 'Your user has been deleted.', 'success');
                            } else {
                                sweetAlert.swal('Cancelled', 'Some Error', 'error');
                            }
                        });
                    } else {
                        sweetAlert.swal('Cancelled', 'Your user is safe :)', 'error');
                    }
                });
            }
        }
    }
})();
