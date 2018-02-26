(function() {
    'use strict';

    angular
        .module('app.translate')
        .config(translateConfig);

    translateConfig.$inject = ['$translateProvider'];
    function translateConfig($translateProvider) {
        //$translateProvider.useLoader('translateLoader');
        $translateProvider.useLoader('$translateUrlLoader', { url: '/I18N/GetContent'});
        $translateProvider.preferredLanguage(navigator.language || navigator.userLanguage);
        $translateProvider.useLocalStorage();
        $translateProvider.usePostCompiling(true);
        $translateProvider.useSanitizeValueStrategy('sanitizeParameters');
    }
})();