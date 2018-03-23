var app = angular.module('ApiGateway', ['frapontillo.bootstrap-switch', 'ngRoute', 'ngResource', 'ui.bootstrap', 'chart.js'])

    .config([
    '$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

        $locationProvider.html5Mode(true);

        $routeProvider
            .when('/', { templateUrl: 'app/view/dashboard.html', controller: 'DashboardController' })
            .when('/dashboard', { templateUrl: 'app/view/dashboard.html', controller: 'DashboardController' })
            .when('/apis', { templateUrl: 'app/view/apis.html', controller: 'ApiController' })
            .when('/apis/:urlPreffix', { templateUrl: 'app/view/api.html', controller: 'ApiController' })
            .when('/sistemas', { templateUrl: 'app/view/sistemas.html', controller: 'SistemaController' })
            .otherwise({
                redirectTo: '/'
            });

        $httpProvider.interceptors.push(function ($q, $rootScope) {
            return {
                'responseError': function (rejection) {
                    $rootScope.$broadcast('showModelValidationErrors', rejection);
                    return $q.reject(rejection);

                }
            };
        });

    }]);

app.controller('MenuCtrl', function ($mdMedia, $scope, $timeout, $mdSidenav, $log) {
    $scope.$watch(function () { return $mdMedia('gt-md'); }, function (big) {
        $scope.bigScreen = big;
    });
    $scope.screenIsSmall = $mdMedia('gt-md');

    $scope.toggle = function () {
        $mdSidenav('left').open()
          .then(function () {
              $log.debug("close LEFT is done");
          });
    };
}).controller('LeftCtrl', function ($scope, $timeout, $mdSidenav, $log) {
    $scope.close = function () {
        $mdSidenav('left').close()
          .then(function () {
              $log.debug("close LEFT is done");
          });
    };
}).controller('RightCtrl', function ($scope, $timeout, $mdSidenav, $log) {
    $scope.close = function () {
        $mdSidenav('right').close()
          .then(function () {
              $log.debug("close RIGHT is done");
          });
    };
});

app.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };

            scope.$watch(scope.isLoading, function (v) {
                if (v) {
                    elm.show();
                } else {
                    elm.hide();
                }
            });
        }
    };

}]);

app.directive('errorPanel', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            elm.hide();

            scope.$on('showModelValidationErrors', function (event, errors) {
                var errorsArray = new Array();

                if (errors.data.Message) errorsArray.push(errors.data.Message);

                for (var propertyName in errors.data.ModelState) {
                    // propertyName is what you want
                    // you can get the value like this: myObject[propertyName]
                    for (var errorMessage in errors.data.ModelState[propertyName]) {
                        errorsArray.push(errors.data.ModelState[propertyName][errorMessage]);
                    }
                }

                if (errors.data.ExceptionMessage) {
                    var exception = errors.data;
                    while (exception) {
                        var msg = exception.ExceptionMessage;
                        if (msg && $.inArray(msg, errorsArray) < 0) errorsArray.push(msg);
                        exception = exception.InnerException;
                    }

                }

                scope.errors = errorsArray;
                elm.show();
                $timeout(function () {
                    elm.hide();
                    scope.errors = new Array();
                }, 3000);
            });
        }
    };

}]);

app.filter('scapeBars', function () {
    return function (input) {
        return input.replace(/\//gi, "_");
    };
});
