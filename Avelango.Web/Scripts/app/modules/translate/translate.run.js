(function() {
    'use strict';

    angular
        .module('app.translate')
        .run(translateRun)
        //.factory('translateLoader', ['$q', '$http', '$rootScope', function ($q, $http, $rootScope) {
        //    return function() {
        //        var deferred = $q.defer();
        //        var page = /[^/]*$/.exec(window.location.href)[0];
        //        var mergedTranslates = $http({ url: "/I18N/GetContent", method: 'POST', data: { lang: navigator.language, page: page } }).then(function (response) {
        //            $rootScope.grouptranslationsTree = response.data.Groups;
        //            var grouptTranslations = {};
        //            angular.forEach(response.data.Groups, function (value) {
        //                grouptTranslations[value.Name] = value.Text;
        //                angular.forEach(value.SubGroups, function (value) {
        //                    grouptTranslations[value.Name] = value.Text;
        //                });
        //            });
        //            return angular.extend(grouptTranslations, response.data.Content);
        //        });
        //        deferred.resolve(mergedTranslates);
        //        return deferred.promise;
        //    };
        //}]);


    translateRun.$inject = ['$rootScope', '$translate'];
    function translateRun($rootScope, $translate){
      $rootScope.language = {
        listIsOpen: false,
        available: {
          'en': 'English',
          'es': 'Español',
          'de': 'Deutsch',
          'fr': 'Français',
          'ru': 'Русский',
          'ua': 'Українська'
        },
        init: function () {
          var proposedLanguage = $translate.proposedLanguage() || $translate.use();
          var preferredLanguage = $translate.preferredLanguage();
          $rootScope.language.selected = $rootScope.language.available[ (proposedLanguage || preferredLanguage) ];
        },
        set: function (localeId) {
          $translate.use(localeId);
          $rootScope.language.selected = $rootScope.language.available[localeId];
          $rootScope.language.listIsOpen = ! $rootScope.language.listIsOpen;
        }
      };
      $rootScope.language.init(); 
    }
})();