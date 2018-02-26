(function () {
    'use strict';

    var module = angular.module('app.pages');

    module.controller('OrderFormController', orderFormController);

    orderFormController.$inject = ['$http', '$rootScope', '$location', '$scope', '$rootScope', 'urlSvc'];
    function orderFormController($http, $location, $scope, $rootScope, urlSvc) {

        var vm = this;
        activate();
        
        function activate() {

            vm.order = {};
            vm.authMsg = '';
            vm.selectedFile = [];
            vm.removedAttachments = [];

            vm.myValidation = function () {
                vm.showValidatemessage = true;
            }
            
            var myAutocomplite = function () {
                var inputFrom = document.getElementById('search');
                var autocompleteFrom = new google.maps.places.Autocomplete(inputFrom, {});
                google.maps.event.addListener(autocompleteFrom, 'place_changed', function (e) {
                    vm.place = autocompleteFrom.getPlace();
                    vm.showValidatemessage = false;
                });
            }

            var interval = setInterval(function () {//это нужно для того чтобы функциЯ myAutocomplite, использующая библиотеку гугл не выполнялась до загрузки этой библиотеки
                if (google != 'undefined') {
                    myAutocomplite();
                    clearInterval(interval);
                }
            }, 100);

            $rootScope.$on('topicaltoDataEvent', function(event, data) {
                vm.order.topicaltoData = data.getFullYear() + "-" + ("0" + (data.getMonth() + 1)).slice(-2) + "-" + ("0" + data.getDate()).slice(-2);
            });

            $rootScope.$on('topicaltoHoursEvent', function(event, data) {
                vm.order.topicaltoHours = data.getHours() + "-" + data.getMinutes();
            });
            
            vm.selectFiles = function () {
                var fileUploader = document.getElementById('file-uploader');
                if (fileUploader.files.length != 0) {
                    for (var i = 0; i < fileUploader.files.length; i++)
                        vm.selectedFile[i] = fileUploader.files[i];
                }
            }
            
            vm.removeAttachment = function (url, index) {//Удаляем файл из аттачментов
                vm.removedAttachments.push(url);
                $rootScope.$emit('fileRemoved', {
                    fileIdx: index
                });
            }

            vm.creatOrEdit = function (editedTask) {// создать или редактировать заказ
                
                var fileUploader = document.getElementById('file-uploader');
                    if (fileUploader.files.length != 0) {
                        for (var i = 0; i < fileUploader.files.length; i++)
                            vm.selectedFile[i] = fileUploader.files[i];
                    }

                    if (document.getElementById('search').value === editedTask.PlaceName) {//на тот случай если местоположение при редактировании осталось нетронутым
                        vm.place = { name: editedTask.PlaceName, id: editedTask.Place }
                    }

                    var formData = new FormData();

                    formData.append('name', JSON.stringify(vm.order.name));
                    formData.append('description', JSON.stringify(vm.order.description));
                    formData.append('price', JSON.stringify(vm.order.price));
                    formData.append('placeLat', JSON.stringify(vm.place.geometry.location.Lat()));
                    formData.append('placeLng', JSON.stringify(vm.place.geometry.location.Lng()));
                    formData.append('placeName', JSON.stringify(vm.place.name));
                    formData.append('topicaltoData', JSON.stringify(vm.order.topicaltoData));
                    formData.append('topicaltoHours', JSON.stringify(vm.order.topicaltoHours));

                    for (var i = 0; i < vm.selectedFile.length; i++) {
                        formData.append('files', vm.selectedFile[i]);
                    }
                    
                    var req;

                    if (!editedTask) {
                        req = {
                            url: urlSvc.AddTask,
                            method: 'POST',
                            withCredentials: false,
                            data: formData,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        };
                    } else {
                        formData.append('taskPk', JSON.stringify(editedTask.PublicKey));

                        if (vm.removedAttachments.length != 0) {
                            for (var k = 0; k < vm.removedAttachments.length; k++) {
                                formData.append('removedAttachmentsPathes', vm.removedAttachments[k]);
                            }
                        }

                        req = {
                            url: urlSvc.MyOrderEdit,
                            method: 'POST',
                            withCredentials: false,
                            data: formData,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        };
                    }

                    if (vm.orderForm.$valid && vm.place) {
                        $http(req).then(function (response) {
                            if (!response.data) {
                                vm.authMsg = 'Incorrect credentials.';
                            } else {
                                vm.authMsg = 'GOOD';
                                $rootScope.$emit('ordersweetAlertEvent', 'success');
                            }
                        }, function () {
                            vm.authMsg = 'Server Request Error';
                            $rootScope.$emit('ordersweetAlertEvent', 'error');
                           });
                    }
                    else {
                        vm.orderForm.order_name.$dirty = true;
                        vm.orderForm.order_description.$dirty = true;
                        vm.orderForm.order_price.$dirty = true;
                        vm.orderForm.order_data.$dirty = true;
                    }
            };
        }
    }
})();
