var HeaderController = angular.module('HeaderController', []);

//var urlAPI = 'http://' + window.location.hostname + ':9515/api/';

HeaderController.controller('HeaderController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', function ($scope, $http, $location, $route, $rootScope, $filter) {

    var store = this;
    

    $scope.templates =
    [{ name: 'footer.html', url: 'views/footer.html' },
    { name: 'header.html', url: 'views/header.html' }];

    $scope.template = $scope.templates[1];

    $rootScope.urlorigem = $location.path();

    $rootScope.redirecionar = function (urlDestino) {
        $rootScope.urlorigem = urlDestino;
        $location.path('/' + urlDestino);
    }

    $rootScope.redirecionarMantendoOrigem = function (urlDestino) {
        $rootScope.urlorigem = $location.path();
        $location.path('/' + urlDestino);
    }


}]);