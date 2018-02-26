/**=========================================================
 * Module: datatable,js
 * Angular Datatable controller
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('DataTableControllerGroups', dataTableControllerGroups);

    dataTableControllerGroups.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', 'urlSvc'];//,'DatatableUsers'
    function dataTableControllerGroups($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {

            // Ajax
            var url = '/MyUser/GetMyGroups';

            //vm.heroes = DatatableUsers.init();
            $resource(url, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                if (response.IsSuccess) {
                    
                    if (response.Subscribed) {
                    var chekedGroups = JSON.parse(response.Subscribed);
                        var groups = [];
                        angular.forEach(response.Groups, function(value, key) {
                            var self = this;
                            var nv = function newvalue(value) {
                                value.SubGroups.forEach(function(item, i, arr) {
                                    if (chekedGroups.indexOf(item.Name) >= 0) {
                                        value.SubGroups[i].Cheked = true;
                                    }
                                });
                                return value;
                            }
                            self.push(nv(value));
                        }, groups);
                        vm.groups = groups;
                    } else {
                        vm.groups = response.Groups;
                    }
                    vm.EmailDelivery = response.EmailDelivery;
                    vm.SmsDelivery = response.SmsDelivery;
                    vm.PushUpDelivery = response.PushUpDelivery;
                }                
            });
            vm.sendCheckedGroups = function() {
                var log = [];
                angular.forEach(vm.groups, function (value, key) {
                    var self = this;
                    value.SubGroups.forEach(function (item, i, arr) {
                        if (item.Cheked === true) {
                            self.push(item.Name);
                        }
                    });                  
                }, log);
                var chekedgroups = JSON.stringify(log);

                $resource(urlSvc.SetMyGroups, { groups: chekedgroups, emailDelivery: vm.EmailDelivery, smsDelivery: vm.SmsDelivery, pushUpDelivery: vm.PushUpDelivery }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Added!', 'Your groups has been added.', 'success');
                    } else {
                        sweetAlert.swal('Cancelled', 'Some Error', 'error');
                    }
                });
            };
           
        }
    }
})();
