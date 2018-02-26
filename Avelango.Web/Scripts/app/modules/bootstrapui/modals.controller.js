(function () {
    'use strict';

    angular
        .module('app.bootstrapui')
        .controller('ModalController', ModalController);

    ModalController.$inject = ['$filter','$uibModal', '$scope', '$resource', '$rootScope', '$http', '$compile'];
    function ModalController($filter, $uibModal, $scope, $resource, $rootScope, $http, $compile) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            var templUrl = '/myModalContent.html';
            var controller = ModalInstanceCtrl;

            vm.open = function (heroFromParlour) {
                
                $http({ url: "/Modal/Order", method: "GET" }).then(function (html) {
                    $("#orderDynamicContent").append($compile(html.data)($scope));

                    //if (heroFromParlour === 'img') {
                    //    templUrl = '/ModalUserImg.html';
                    //    controller = ModalInstanceCtrl;
                    //} else if (heroFromParlour === 'orderDesc') {
                    //    templUrl = '/ModalOrderDescription.html';
                    //    controller = ControllerMyOrder;
                    //    $rootScope.numberfororder = data;
                    //} else if (heroFromParlour === 'jobDescr') {
                    //    templUrl = '/ModalOrderDescription.html';
                    //    controller = ControllerMyOrder;
                    //    $rootScope.numberfororder = data;
                    //} else if (heroFromParlour === 'message') {
                    //    templUrl = '/ModalMessageChat.html';
                    //    controller = ControllerMyOrder;
                    //    $rootScope.userChatPk = data;
                    //}

                    var modalInstance = $uibModal.open({
                        templateUrl: templUrl,
                        controller: controller,
                        resolve:
                        {
                            editedTask: function() {
                                if (heroFromParlour) {
                                  //  var d = heroFromParlour.TopicalTo |date:'yyyy-MM-dd HH:mm';
                                    return heroFromParlour;
                                }
                                
                                else return "";
                            },
                            modalTitle: function() {
                                if (heroFromParlour) return "EDITORDER";
                                else return "CREATEORDER";
                            },
                            attachments: function() {
                                if (heroFromParlour) {
                                    return heroFromParlour.Attachments;
                                } else return "";
                            }
                        }
                    });

                    //$rootScope.$emit('som');

                    //var state = $('#modal-state');
                    //modalInstance.result.then(function () {
                    //    state.text('Modal dismissed with OK status');
                    //}, function () {
                    //    state.text('Modal dismissed with Cancel status');
                    //});
                });
            };

            vm.open2 = function (size) {

                var modalInstance = $uibModal.open({
                    templateUrl: '/myModalContent.html',
                    controller: ModalInstanceCtrl,
                    size: size
                });

                var state = $('#modal-state');

                modalInstance.result.then(function () {
                    state.text('Modal dismissed with OK status');
                }, function () {
                    state.text('Modal dismissed with Cancel status');
                });
            };

            vm.setImage = function (img) {
                var modalInstance = $uibModal.open({
                    template: '<img ng-src=' + img + ' alt="Portfolio image" />',
                    controller: ModalInstanceCtrl,
                    size: 'sm'
                });

                var state = $('#modal-state');
                modalInstance.result.then(function () {
                    state.text('Modal dismissed with OK status');
                }, function () {
                    state.text('Modal dismissed with Cancel status');
                });
            };

            ModalInstanceCtrl.$inject = ['$scope', '$uibModalInstance', '$rootScope', 'editedTask', 'modalTitle', 'attachments'];
            function ModalInstanceCtrl($scope, $uibModalInstance, $rootScope, editedTask, modalTitle, attachments) {

                $scope.editedTask = editedTask;

                $scope.initDate = editedTask.TopicalTo? new Date(editedTask.TopicalTo * 1): new Date();

                $scope.modalTitle = modalTitle;
                $scope.attachments = attachments;

                $scope.ok = function (forma) {
                    if (forma.$valid) {
                        $uibModalInstance.close('closed');
                    }
                };

                $scope.cancel = function () {
                  $uibModalInstance.dismiss('cancel');
                };

                $rootScope.$on("ChangesInOrders", function () {
                   
                });

                $rootScope.$on("closeModal", function () {
                    $uibModalInstance.close('closed');
                });
                
                $rootScope.$on("fileRemoved", function (event, data) {
                    $scope.attachments.splice(data.index, 1);
                });
            }

            ControllerMyOrder.$inject = ['$scope', '$uibModalInstance', '$rootScope'];
            function ControllerMyOrder($scope, $uibModalInstance, $rootScope) {
                var a = $rootScope.numberfororder;
            }
        }
    }
})();
