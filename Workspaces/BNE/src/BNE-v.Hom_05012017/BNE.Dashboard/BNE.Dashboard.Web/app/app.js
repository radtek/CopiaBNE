var app = angular.module('dashboard', ['ngRoute', 'ngResource', 'ngAudio', 'filters']).config([
    '$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        $locationProvider.html5Mode(true);

        $routeProvider
            .when('/', { templateUrl: '/app/views/watcher.html', controller: 'WatcherController' })
            .when('/jobs', { templateUrl: '/app/views/job.html', controller: 'JobController' })
            .otherwise({
                redirectTo: '/'
            });
    }]);

angular.module('filters', []).filter('zpad', function () {
    return function (input, n) {
        if (input === undefined)
            input = ""
        if (input.length >= n)
            return input
        var zeros = "0".repeat(n);
        return (zeros + input).slice(-1 * n)
    };
});

app.filter('orderObjectBy', function () {
    return function (input, attribute) {
        if (!angular.isObject(input)) return input;

        var array = [];
        for (var objectKey in input) {
            array.push(input[objectKey]);
        }

        array.sort(function (a, b) {
            a = parseInt(a[attribute]);
            b = parseInt(b[attribute]);
            return a - b;
        });
        return array;
    }
});