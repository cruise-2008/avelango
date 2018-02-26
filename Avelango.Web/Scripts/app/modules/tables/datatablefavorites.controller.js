/**=========================================================
 * Module: datatable,js
 * Angular Datatable controller
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('DataTableControllerFavorites', dataTableControllerFavorites);

    dataTableControllerFavorites.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', 'urlSvc'];//,'DatatableUsers'
    function dataTableControllerFavorites($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {

            // Ajax
            var url = urlSvc.GetMyLikedUsers;


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
                DTColumnDefBuilder.newColumnDef(4).notSortable()

            ];
            vm.removeOrder = removeOrder;

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
                                sweetAlert.swal('Deleted!', 'Your favorite has been deleted.', 'success');
                            } else {
                                sweetAlert.swal('Cancelled', 'Some Error', 'error');
                            }
                        });
                    } else {
                        sweetAlert.swal('Cancelled', 'Your favorite is safe :)', 'error');
                    }
                });
            }


        }
    }
})();
