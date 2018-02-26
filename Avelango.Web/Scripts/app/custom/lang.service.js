(function () {
    'use strict';

angular.module('lang.translate')



.factory('$translateUrlLoader', ['$q', '$http', '$location', 'urlSvc', function($q, $http, $location, urlSvc) {

    'use strict';

    return function (options) {

        if (!options || !options.url) {
            throw new Error('Couldn\'t use urlLoader since no url is given!');
        }

        var ourLocation = $location.path();

        switch (ourLocation) {
            case '/Account/Login':
                ourLocation = 'login';
                break;
            case '/MyUser/Parlour':
                ourLocation = 'usercabinet';
                break;
            case '/Users/Executors':
                ourLocation = 'executors';
                break;
            case '/':
                ourLocation = 'main';
                break;


            default:
                ourLocation = 'main';
        }


        var requestParams = {
            'lang' : options.key,
            'page': ourLocation
        };
        $http({ method: "GET", url: urlSvc.SetLang, params: { "lang": options.key } });

        return $http(angular.extend({
            url: options.url,
            params: requestParams,
            method: 'GET'
        }, options.$http))
          .then(function (result) {
              return result.data;
          }, function () {
              return $q.reject(options.key);
          });
    };
}])
})();

