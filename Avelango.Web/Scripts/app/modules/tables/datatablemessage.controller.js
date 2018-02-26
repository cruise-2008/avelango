(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('DataTableControllerMessage', dataTableControllerMessage);

    dataTableControllerMessage.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', '$http', '$rootScope', 'urlSvc'];//,'DatatableUsers'
    function dataTableControllerMessage($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, $http, $rootScope, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            var url = urlSvc.GetMyChats;

            $resource(url, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                if (response.IsSuccess) {
                    response.Chats.forEach(function (item) {
                        item.LastAction = item.LastAction.replace(/\D+/g, '');
                    });
                    vm.messages = response.Chats;

                }
            });

            vm.dtOptions = DTOptionsBuilder.newOptions();
            vm.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0),
                DTColumnDefBuilder.newColumnDef(1),
                DTColumnDefBuilder.newColumnDef(2),
                DTColumnDefBuilder.newColumnDef(3).notSortable()
            ];

            vm.removeOrder = removeOrder;

            vm.openChat = function (chatPk, cullocutorPk) {

                $resource(urlSvc.GetChatMessages, { chatPk: chatPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function(response) {
                    if (response.IsSuccess) {
                        response.Messages.forEach(function(item) {
                            item.Created = item.Created.replace(/\D+/g, '');
                        });
                        vm.Chatmessages = response.Messages;
                        $scope.settingActive = 1;
                        vm.chosenChat = chatPk;
                       vm.chosenCollocutor = cullocutorPk;
                    }
                });

                $resource(urlSvc.SetMessageNotificationAsViewed, { userPk: cullocutorPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) { });
            };

            vm.sendMyMessage = function(message) {
                console.log(message);
                var formData = new FormData();
                if ($scope.selectedFile) {
                    for (var i = 0, len = $scope.selectedFile.length; i < len; i++) {
                        formData.append('files', $scope.selectedFile[i]);
                    }
                }

                formData.append('chatPk', JSON.stringify(vm.chosenChat));
                formData.append('collocutorPk', JSON.stringify(vm.chosenCollocutor));
                formData.append('text', JSON.stringify(message));
                var req = {
                    url: urlSvc.SendMessage,
                    method: 'POST',
                    withCredentials: false,
                    data: formData,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                };
                $http(req)
                    .then(function(response) {
                        if (response.data.IsSuccess) {
                            var date = +(new Date());
                            vm.Chatmessages.push({
                                Message: message,
                                Created: date,
                                MyOwn: true
                            });
                        } else {
                            vm.Chatmessages.push({ Message: "Server error" });
                        }
                    }, function(error) {
                        console.log('Error' + error);
                    });

            };
            $rootScope.$on('ChatMessage', function (e,message) {
                var date = +(new Date());
                vm.Chatmessages.push({
                    Message: message,
                    Created: date,
                    MyOwn: false
                });
                $scope.$apply();
            });
            function removeOrder(index, orderPk) {
                sweetAlert.swal({
                    title: 'Are you sure?',
                    text: 'Your will not be able to recover this!',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, delete it!',
                    cancelButtonText: 'Cancel',
                    closeOnConfirm: false,
                    closeOnCancel: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        $resource('', { pk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                            console.log(response);
                            if (response.IsSuccess) {
                                vm.messages.splice(index, 1);
                                sweetAlert.swal('Deleted!', 'Your message has been deleted.', 'success');
                            } else {
                                sweetAlert.swal('Cancelled', 'Some Error', 'error');
                            }
                        });
                    } else {
                        sweetAlert.swal('Cancelled', 'Your message is safe :)', 'error');
                    }
                });
            }

        }
    }
})();
