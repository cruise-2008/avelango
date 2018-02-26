(function () {
    'use strick';

    angular
        .module("app.global.map.location.service", [])
        .factory("$map", $map);

    $map.inject = ["$http", "$q", "$rootScope", "$cookie"];
    var vm = this;

    function $map($http, $q, $rootScope, $cookie) {
        return {
            autocomplite: function (id) {
                var inputFrom = document.getElementById(id);
                var autocompleteFrom = new window.google.maps.places.Autocomplete(inputFrom, {});
                window.google.maps.event.addListener(autocompleteFrom, 'place_changed', function () {
                    vm.showValidatemessage = false;
                    var place = autocompleteFrom.getPlace();
                    if (place) vm.pyrmont = [place.geometry.location.lat(), place.geometry.location.lng()];
                    setLocation();
                });

                function initialize() {
                    var pacs = document.getElementsByName('pac-input');
                    for (var i = 0; i < pacs.length; ++i) {
                        var autocomplete = new window.google.maps.places.Autocomplete(pacs[i]);
                    }
                }
                window.google.maps.event.addDomListener(window, 'load', initialize);
            },

            interval: function () {
                //это нужно для того чтобы функция myAutocomplite, использующая библиотеку гугл не выполнялась до загрузки этой библиотеки
                var interval = setInterval(function () {
                    if (window.google !== 'undefined') {
                        myAutocomplite();
                        clearInterval(interval);
                    }
                }, 100);
            },

            validation: function () {
                vm.showValidatemessage = true;
                if (!document.getElementById(id).value) vm.pyrmont = myplace ? [myplace.coords.latitude, myplace.coords.longitude] : [null, null];
            },

            setLocation: function () {
                if (angular.equals({}, $rootScope.user.geocode.location.place)) {
                    if (window.google.loader.ClientLocation) {
                        $rootScope.user.geocode.location.lat = window.google.loader.ClientLocation.latitude;
                        $rootScope.user.geocode.location.lng = window.google.loader.ClientLocation.longitude;
                        $rootScope.user.geocode.location.place = window.google.loader.ClientLocation.address;
                        $cookie.setCookie();
                    }
                }
            },

            getLocation: function () {
                if (angular.equals({}, $rootScope.user.geocode.location.place)) {
                    if (window.google.loader.ClientLocation) this.setLocation();
                }
                return $rootScope.user.geocode.location;
            },

            getPhoneCodeByLocation: function () {
                var shortCode = this.getLocation().place.country_code;
                var getCode = function () {
                    return $http({ method: "POST", url: "/Base/GetPhoneCode", data: JSON.stringify({ countryCode: shortCode }) }).then(function (response) {
                        if (response.data.IsSuccess === true && response.data.PhoneCode !== "") {
                            $rootScope.user.geocode.phoneCountryCode = response.data.PhoneCode;
                            $cookie.setCookie();
                            return response.data.PhoneCode;
                        }
                        return "+00";
                    });
                }
                var code = getCode();
                var deferred = $q.defer();
                deferred.resolve(code);
                return deferred.promise;
            }
        }
    }
})();