var app = angular.module('dashboard', ['ngRoute', 'ngResource', 'ngAudio']).config([
    '$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        $locationProvider.html5Mode(true);
        $routeProvider
            .when('/', { templateUrl: '/app/views/watcher.html', controller: 'WatcherController' })
            .otherwise({
                redirectTo: '/'
            });
    }]);