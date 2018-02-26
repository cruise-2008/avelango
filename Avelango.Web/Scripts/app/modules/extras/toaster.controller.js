(function() {
    'use strict';

    angular
        .module('app.elements', [])
        .controller('ToasterDemoCtrl', ToasterDemoCtrl);

    ToasterDemoCtrl.$inject = ['$rootScope', '$translate', 'toaster'];

    function ToasterDemoCtrl($rootScope, $translate, toaster) {
        var vm = this;

        activate();

        function activate() {
           vm.toaster = {
              type:  'success',
              title: 'Title',
              text:  'Message'
          };

          vm.pop = function() {
            toaster.pop(vm.toaster.type, vm.toaster.title, vm.toaster.text);
          };

          $rootScope.$on('newTaskForModer', function (e, message) {
              vm.toaster.type = 'info';
              vm.toaster.title = '';
              $translate('NEWTASKTEXT').then(function (result) {
                  vm.toaster.text = result;
                  vm.pop();
              });
          });

          $rootScope.$on('taskConfirmed', function (e, message) {
              vm.toaster.type = 'success';
              var msg = JSON.parse(message);
              vm.toaster.title = msg.title;
              $translate('TASKCONFIRMEDTEXT').then(function (result) {
                  vm.toaster.text = result;
                  vm.pop();
              });
          });

          $rootScope.$on('taskDismissed', function (e, message) {
              vm.toaster.type = 'error';
              var msg = JSON.parse(message);
              vm.toaster.title = msg.title;
              $translate('TASKDISMISSEDTEXT').then(function (result) {
                  vm.toaster.text = result;
                  vm.pop();
              });
          });

          $rootScope.$on('taskBidded', function (e, message) {
              vm.toaster.type = 'info';
              var msg = JSON.parse(message);
              $translate('TASKBIDDEDTITLE').then(function (result) {
                  vm.toaster.title = result;
              });
              $translate('TASKBIDDEDTEXT').then(function (result) {
                  vm.toaster.text = msg.workerName + ' ' + result + ' "' + msg.title + '"';
                  vm.pop();
              });
          });

          $rootScope.$on('taskUnbidded', function (e, message) {
              vm.toaster.type = 'info';
              var msg = JSON.parse(message);
              $translate('TASKUNBIDDEDTITLE').then(function (result) {
                  vm.toaster.title = result;
              });
              $translate('TASKUNBIDDEDTEXT').then(function (result) {
                  vm.toaster.text = msg.workerName + " " + result + ' "' + msg.title + '"';
                  vm.pop();
              });
          });

          $rootScope.$on('customerChosenTheWorkers', function (e, message) {
              vm.toaster.type = 'success';
              var msg = JSON.parse(message);
              vm.toaster.title = '';
              $translate('CUSTOMERCHOSENTHEWORKERSTEXT').then(function (result) {
                  vm.toaster.text = result + ' "' + msg.title + '"';
                  vm.pop();
              });
          });

          $rootScope.$on('workerStartedTask', function (e, message) {
              vm.toaster.type = 'success';
              var msg = JSON.parse(message);
              vm.toaster.title = '';
              $translate('WORKERSTARTEDTASKTEXT').then(function (result) {
                  vm.toaster.text = msg.workerName + " " + result + ' "' + msg.title + '"';
                  vm.pop();
              });
          });


          $rootScope.$on('taskCompletedByCustomer', function (e, message) {
              vm.toaster.type = 'success';
              var msg = JSON.parse(message);
              vm.toaster.title = '';
              $translate('TASKCOMPLETEDBYCUSTOMERTEXT').then(function (result) {
                  vm.toaster.text = result + ' "' + msg.title + '"';
                  vm.pop();
              });
          });

          $rootScope.$on('taskCompletedByWorker', function (e, message) {
              vm.toaster.type = 'success';
              var msg = JSON.parse(message);
              vm.toaster.title = '';
              $translate('TASKCOMPLETEDBYWORKERTEXT').then(function (result) {
                  vm.toaster.text = result + ' "' + msg.title + '"';
                  vm.pop();
              });
          });
        }
    }
})();
