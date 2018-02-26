/**=========================================================
 * Module: demo-datepicker.js
 * Provides a simple demo for bootstrap datepicker
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('app.bootstrapui')
        .controller('DatepickerDemoCtrl', DatepickerDemoCtrl);
    DatepickerDemoCtrl.$inject = ['$scope', '$rootScope'];
    function DatepickerDemoCtrl($scope,$rootScope) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
          vm.today = function() {
            vm.dt = new Date();
          };
          vm.today();

          vm.clear = function () {
            vm.dt = null;
          };
          vm.disabled = function(date, mode) {
            return ( mode === 'day' && ( date.getDay() === 0 || date.getDay() === 6 ) );
          };

          vm.toggleMin = function() {
            vm.minDate = vm.minDate ? null : new Date();
          };
          vm.toggleMin();

          vm.open = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.opened = true;
          };

          vm.dateOptions = {
            disableWeekends: false,
            //minDate: new Date(),
            showWeeks: false,
            formatYear: 'yy',
            startingDay: 1
            //initDate: new Date('2017-06-22')
          };http://localhost:3300/../../../../Views/Modderator

          vm.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
          //vm.formats = ['MM-dd-yyyy'];
          vm.format = vm.formats[0];
          $scope.$watch('dp.dt', function (newValue, oldValue) {
              $scope.$emit('topicaltoDataEvent', newValue);
          });
        }
    }
})();

