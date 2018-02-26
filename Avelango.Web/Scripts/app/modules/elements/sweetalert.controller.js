(function() {
    'use strict';

    angular
        .module('app.elements')
        .controller('sweetAlertController', sweetAlertController);

    sweetAlertController.$inject = ['sweetAlert', '$rootScope', '$scope'];
    function sweetAlertController(sweetAlert, $rootScope, $scope) {
        var vm = this;

        activate();

        function activate() {

          $scope.$on('ordersweetAlertEvent', function (event, data) {
              if (data === 'success') {
                  sweetAlert.swal('Good job!', 'Your message processing', 'success');
              } else {
                  sweetAlert.swal('Cancelled', 'Some server error', 'error');
              }
          });

          vm.demo1 = function () {
            sweetAlert.swal('Here\'s a message');
          };

          vm.demo2 = function() {
            sweetAlert.swal('Here\'s a message!', 'It\'s pretty, isn\'t it?');
          };

          vm.demo3 = function() {
            sweetAlert.swal('Good job!', 'You clicked the button!', 'success');
          };

          vm.demo4 = function() {
            sweetAlert.swal({   
              title: 'Are you sure?',   
              text: 'Your will not be able to recover this imaginary file!',   
              type: 'warning',   
              showCancelButton: true,   
              confirmButtonColor: '#DD6B55',   
              confirmButtonText: 'Yes, delete it!',
              closeOnConfirm: false
            },  function(){  
              sweetAlert.swal('Booyah!');
            });
          };

          vm.demo5 = function() {
            sweetAlert.swal({   
              title: 'Are you sure?',   
              text: 'Your will not be able to recover this imaginary file!',   
              type: 'warning',   
              showCancelButton: true,   
              confirmButtonColor: '#DD6B55',   
              confirmButtonText: 'Yes, delete it!',   
              cancelButtonText: 'No, cancel plx!',   
              closeOnConfirm: false,   
              closeOnCancel: false 
            }, function(isConfirm){  
              if (isConfirm) {     
                sweetAlert.swal('Deleted!', 'Your imaginary file has been deleted.', 'success');   
              } else {     
                sweetAlert.swal('Cancelled', 'Your imaginary file is safe :)', 'error');   
              } 
            });
          };

          vm.demo6 = function() {
            sweetAlert.swal({   
              title: 'Sweet!',   
              text: 'Here\'s a custom image.',   
              imageUrl: 'http://oitozero.com/img/avatar.jpg' 
            });
          };
        }
    }
})();
