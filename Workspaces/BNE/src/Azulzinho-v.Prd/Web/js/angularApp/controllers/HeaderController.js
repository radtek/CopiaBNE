var HeaderController = angular.module('HeaderController', []);

HeaderController.controller('HeaderController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', '$routeParams', 'query', function ($scope, $http, $location, $route, $rootScope, $filter, $routeParams, query) {

    var store = this;
    $scope.filtro = query;

    if ($routeParams.filtro) {
        query.text = $routeParams.filtro;
    }
    

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