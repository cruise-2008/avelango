/**=========================================================
 * Module: demo-timepicker.js
 * Provides a simple demo for bootstrap ui timepicker
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('app.bootstrapui')
        .controller('TimepickerDemoCtrl', TimepickerDemoCtrl);
    TimepickerDemoCtrl.$inject = ['$scope'];
    function TimepickerDemoCtrl($scope) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
          vm.mytime = new Date();

          vm.hstep = 1;
          vm.mstep = 15;

          //vm.options = {
          //  hstep: [1, 2, 3],
          //  mstep: [1, 5, 10, 15, 25, 30]
          //};
          
          vm.ismeridian = false;
          $("#timepick").hide();
          $("#timepick").show('slow');
          //vm.toggleMode = function() {
          //  vm.ismeridian = ! vm.ismeridian;
          //};

          //vm.update = function() {
          //  var d = new Date();
          //  d.setHours( 14 );
          //  d.setMinutes( 0 );
          //  vm.mytime = d;
          //  };
         
          //vm.changed = function () {
          //  console.log('Time changed to: ' + vm.mytime);
          //};

          //vm.clear = function() {
          //  vm.mytime = null;
          //};
          $scope.$watch('tp.mytime', function (newValue, oldValue) {

              $scope.$emit('topicaltoHoursEvent', newValue || oldValue);
          });
        }
    }
})();