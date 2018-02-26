(function () {
    'use strict';

    angular
        .module('app.modal.window.open.service', [])
        .factory('$modalWindowService', $modalWindowService);

    $modalWindowService.inject = ["$http", "$uibModal", "$compile", "$q"];

    function $modalWindowService($http, $uibModal, $compile, $q) {

        var modalInstance = {};

        return {
            openModal: function (urn, id, templateId, controller) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: urn
                }).success(function (data, scope) {
                    $("#" + id).append($compile(data)(scope));
                    modalInstance = $uibModal.open({
                        templateUrl: templateId,
                        controller: controller,
                        backdrop: 'static',
                        keyboard: false
                    });

                }).error(function () {

                });
                document.getElementById('main').classList.add("blur");
                deferred.resolve(modalInstance);
                return deferred.promise;
            },

            closeModal: function (templateId, controller) {
                if (modalInstance !== undefined) {
                    modalInstance.close({
                        templateUrl: templateId,
                        controller: controller
                    });
                }
                document.getElementById('main').classList.remove("blur");
                return;
            }
        };
    };

})();