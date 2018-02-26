(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('DataTableControllerModer', dataTableControllerModer);

    dataTableControllerModer.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', 'urlSvc'];//,'DatatableUsers'
    function dataTableControllerModer($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            // Ajax
            var url = window.location.pathname;

            if (url === '/Addmin/Parlour/Moderators') {
                url = '/Addmin/GetUsersInfo';
            } else if (url === '/Addmin/Parlour/Users') {
                url = 'Addmin/GetModeratorsInfo';
            } else if (url === '/Addmin/Parlour') {
                url = '/Addmin/GetUsersInfo';
            //} else if (url === '/MyUser/Parlour') {
            //    url = '/Task/GetMyOrders';
            //} else {
            } else {
                url = null;
            }
            vm.url = url;
            //vm.heroes = DatatableUsers.init();
            $resource(url, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (persons) {
                if (persons.IsSuccess) {
                    persons.Orders.forEach(function (item) {
                        item.DateStart = item.DateStart.replace(/\D+/g, '');
                        item.DateEnd = item.DateEnd.replace(/\D+/g, '');
                    });
                    vm.heroes = persons.Orders;
                } else {
                    vm.heroes = persons;
                }
                
            });
            //if (!vm.heroes.length) {
            //    $resource(url, {}, { 'query': { method: 'POST', isArray: true } }).query().$promise.then(function(persons) {
            //        vm.heroes = persons;
            //    });
            //}
            //$http.post('/Addmin/GetUsersInfo').success(function(persons, status, headers, config) {
            //    console.log(persons);
            //    vm.heroes = persons;
            //})
            //.error(function (data, status, headers, config) {
            //    console.log(status);
            //});

            // Changing data

            //vm.heroes = [{
            //    'id': 860,
            //    'firstName': 'Superman',
            //    'lastName': 'Yoda'
            //  }, {
            //    'id': 870,
            //    'firstName': 'Ace',
            //    'lastName': 'Ventura'
            //  }, {
            //    'id': 590,
            //    'firstName': 'Flash',
            //    'lastName': 'Gordon'
            //  }, {
            //    'id': 803,
            //    'firstName': 'Luke',
            //    'lastName': 'Skywalker'
            //  }
            //];

            vm.dtOptions = DTOptionsBuilder.newOptions();
            vm.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0),
                DTColumnDefBuilder.newColumnDef(1),
                DTColumnDefBuilder.newColumnDef(2),
                DTColumnDefBuilder.newColumnDef(3),
                DTColumnDefBuilder.newColumnDef(4),
                DTColumnDefBuilder.newColumnDef(5),
                DTColumnDefBuilder.newColumnDef(6),
                DTColumnDefBuilder.newColumnDef(7),
                DTColumnDefBuilder.newColumnDef(8),
                DTColumnDefBuilder.newColumnDef(9),
                DTColumnDefBuilder.newColumnDef(10),
                DTColumnDefBuilder.newColumnDef(11),
                DTColumnDefBuilder.newColumnDef(12),
                DTColumnDefBuilder.newColumnDef(13).notSortable()
            ];
            vm.person2Add = _buildPerson2Add(1);
            vm.addPerson = addPerson;
            vm.modifyPerson = modifyPerson;
            vm.removePerson = removePerson;

            function _buildPerson2Add(id) {
                return {
                    id: id,
                    firstName: 'Foo' + id,
                    lastName: 'Bar' + id
                };
            }
            function addPerson() {


                vm.heroes.push(angular.copy(vm.person2Add));
                vm.person2Add = _buildPerson2Add(vm.person2Add.id + 1);
            }
            function modifyPerson(index) {



                vm.heroes.splice(index, 1, angular.copy(vm.person2Add));
                vm.person2Add = _buildPerson2Add(vm.person2Add.id + 1);

            }
            function removePerson(index, userId) {

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
                        $resource(urlSvc.RemoveUser, { id: userId }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                            console.log(response);
                            if (response.result) {
                                vm.heroes.splice(index, 1);
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
            //$scope.items = ['item1', 'item2', 'item3'];
            //vm.open = function (size) {

            //    //vm.heroes.splice(index, 1, angular.copy(vm.person2Add));
            //    //vm.person2Add = _buildPerson2Add(vm.person2Add.id + 1);

            //    var modalInstance = $uibModal.open({
            //        templateUrl: '/myModalContent.html',
            //        controller: ModalInstanceCtrl,
            //        size: size,
            //        resolve: {
            //            items: function () {
            //                return $scope.items; 
            //            }
            //        }
            //    });

            //    var state = $('#modal-state');
            //    modalInstance.result.then(function () {
            //        state.text('Modal dismissed with OK status');
            //    }, function () {
            //        state.text('Modal dismissed with Cancel status');
            //    });
            //};

            //// Please note that $uibModalInstance represents a modal window (instance) dependency.
            //// It is not the same as the $uibModal service used above.

            //ModalInstanceCtrl.$inject = ['$uibModalInstance', '$scope'];
            //function ModalInstanceCtrl($scope, $uibModalInstance, items) {
            //    $scope.items = items;
            //    $scope.ok = function () {
            //        $uibModalInstance.close('closed');
            //    };

            //    $scope.cancel = function () {
            //        $uibModalInstance.dismiss('cancel');
            //    };
            //}

        }
    }
})();
