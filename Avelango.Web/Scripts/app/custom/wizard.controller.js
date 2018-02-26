(function () {
    'use strict';

    angular
        .module('app.wizard', [])
        .controller('OpenWizardCtrl', openWizardCtrl);


    openWizardCtrl.$inject = ['$q', '$scope', '$http', '$modalWindowService', '$compile', '$uibModal', '$map', '$login', '$rootScope', 'sweetAlert', '$timeout'];
    function openWizardCtrl($q, $scope, $http, $modalWindowService, $compile, $uibModal, $map, $login, $rootScope, sweetAlert, $timeout) {

        var vm = this;
        var sc = $scope;

        var templUrl = '/taskWizardContent.html';
        var controller = openWizardCtrl;
        var url = '/Wizard/TaskWizard';
        var id = 'orderDynamicContent';

        sc.taskBlock = false;
        sc.locationBlock = true;
        sc.numberBlock = true;
        sc.passBlock = true;
        sc.smsCodeBlock = true;
        sc.finishBlock = true;

        sc.authenticated = false;

        /// var openModalCallBack = function() {
        ///     $map.getAutoUserLocationPath();
        ///     $map.autocomplite('wizardsearch');
        /// }
        ///
        /// vm.openWizard = function () {
        ///     $modalWindowService.openModal(url, id, templUrl, controller, openModalCallBack);
        /// };

        //$map.getAutoUserLocationPath();
        //$map.autocomplite('wizardsearch');


        // var autocomplite;
        // var addGoogleMapping = function () {
        //      if (autocomplite == undefined) autocomplite = $map.autocomplite('wizardsearch');
        //  }

        sc.wizardClick = function (step, direction) {
            switch (step) {
                case 'task':
                    sc.taskBlock = true;
                    sc.locationBlock = false;
                    //addGoogleMapping();
                    break;
                case 'location':
                    if (sc.authenticated) {
                        if (direction) {
                            sc.locationBlock = true;
                            sc.finishBlock = false;
                        }
                        else {
                            sc.locationBlock = true;
                            sc.taskBlock = false;
                        }
                    } else {
                        if (direction) {
                            sc.locationBlock = true;
                            sc.numberBlock = false;
                        }
                        else {
                            sc.locationBlock = true;
                            sc.taskBlock = false;
                        }
                    }
                    break;
                case 'number':
                    if (direction) {
                        $login.sendPhone(sc, function (data) {
                            if (data.IsExist) {
                                //pass
                                sc.passBlock = false;
                                sc.numberBlock = true;
                            }
                            else {
                                if (data.SmsSended) {
                                    sc.smsCodeBlock = false;
                                    sc.numberBlock = true;
                                }
                                else {
                                    sweetAlert.swal('Неверный номер телефона!');
                                }
                            }
                        });
                    } else {
                        sc.numberBlock = true;
                        sc.locationBlock = false;
                    }
                    break;
                case 'pass':
                    if (direction) {
                        $login.checkPass(sc, function (data) {
                            if (data.IsEnabled) {
                                sc.passBlock = true;
                                sc.finishBlock = false;
                                sc.authenticated = true;
                                // ImagePath = imagePath, PublicKey
                            }
                        });
                        // sc.passBlock = true;
                        // sc.smsPassBlock = false;
                    } else {
                        sc.passBlock = true;
                        sc.numberBlock = false;
                    }
                    break;
                case 'smsCode':
                    if (direction) {
                        $login.sendSmsCode(sc, function (data) {
                            if (data.Success) {
                                sc.smsCodeBlock = true;
                                sc.finishBlock = false;
                                sc.authenticated = true;
                            } else {
                                sweetAlert.swal('Указан неверный пароль!');
                            }
                        });
                    } else {
                        sc.smsCodeBlock = true;
                        sc.passBlock = false;
                    }
                    break;
                case 'finish':
                    sc.finishBlock = true;
                    sc.locationBlock = false;
                    break;
            }
        }



        //vm.open2 = function (size) {
        //
        //    var modalInstance = $uibModal.open({
        //        templateUrl: '/taskWizardContent.html',
        //        controller: ModalInstanceCtrl,
        //        size: size
        //    });

        //var state = $('#modal-state');

        //modalInstance.result.then(function () {
        //    state.text('Modal dismissed with OK status');
        //}, function () {
        //    state.text('Modal dismissed with Cancel status');
        //});
        //};

        //vm.setImage = function (img) {
        //    var modalInstance = $uibModal.open({
        //        template: '<img ng-src=' + img + ' alt="Portfolio image" />',
        //        controller: ModalInstanceCtrl,
        //        size: 'sm'
        //    });

        //var state = $('#modal-state');
        //modalInstance.result.then(function () {
        //    state.text('Modal dismissed with OK status');
        //}, function () {
        //    state.text('Modal dismissed with Cancel status');
        //});
        //};


        controllerMyOrder.$inject = ['$scope', '$uibModalInstance', '$rootScope', '$map'];
        function controllerMyOrder($scope, $uibModalInstance, $rootScope, $map) {
            var a = $rootScope.numberfororder;
        }
    }
})();