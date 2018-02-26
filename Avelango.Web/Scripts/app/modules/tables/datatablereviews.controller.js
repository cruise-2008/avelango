(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('DataTableControllerReviews', dataTableControllerReviews);

    dataTableControllerReviews.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', 'urlSvc'];//,'DatatableUsers'
    function dataTableControllerReviews($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {

            // Ajax
            var url = urlSvc.GetMyResponses;


            //vm.heroes = DatatableUsers.init();
            $resource(url, {}, { 'query': { method: 'POST', isArray: true } }).query().$promise.then(function (persons) {
                vm.messages = persons;
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


            vm.dtOptions = DTOptionsBuilder.newOptions();
            vm.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0),
                DTColumnDefBuilder.newColumnDef(1),
                DTColumnDefBuilder.newColumnDef(2),
                DTColumnDefBuilder.newColumnDef(3),
                DTColumnDefBuilder.newColumnDef(4),
                DTColumnDefBuilder.newColumnDef(5).notSortable()
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


        }
    }
})();
