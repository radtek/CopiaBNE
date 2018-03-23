'use strict';
var authInterceptorService = angular.module('authInterceptorService', []);

authInterceptorService.factory('authInterceptorService', ['$q', '$injector', '$location', 'localStorageService', '$rootScope', function ($q, $injector, $location, localStorageService, $rootScope) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {
        config.headers = config.headers || {};
        var authData = localStorageService.get('authorizationData');
        
        if (authData !== undefined && authData != null) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
        //return config || $q.when(config);
    }

    var _responseError = function (rejection) {

        if (rejection.status === 401) {

            var authService = $injector.get('authService');
            var authData = localStorageService.get('authorizationData');

            if (authData) {
                if (authData.useRefreshTokens) {
                    $location.path('/Login');
                    return $q.reject(rejection);
                }
            }

            //authService.logOut();
            //$location.path('/Login');
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);