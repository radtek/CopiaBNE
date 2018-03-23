var QuemSomosController = angular.module('QuemSomosController', []);

QuemSomosController.controller('QuemSomosController', ['$rootScope', function ($rootScope) {

    //esconder a barra de pesquisa 
    $rootScope.isHome = false;

}]);