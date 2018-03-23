var camposDirectives = angular.module('camposDirectives', []);

camposDirectives.directive('telefoneFixo', function () {
    return {
        restrict: 'EA',
        //templateUrl:  $sce.trustAsResourceUrl('http://localhost:26840/views/templates/TelefoneFixo.html'),
        templateUrl: 'views/templates/TelefoneFixo.html',
        link: function (scope, element, attrs) {
            scope.label = attrs.label;
            scope.maxfone = attrs.maxl;
            scope.nome = attrs.nome;
        }
    }
});

camposDirectives.directive('cidade', function () {
    return {
        restrict: 'EA',
        templateUrl: 'views/templates/Cidade.html'
    }
});

camposDirectives.directive('funcaoPretendida', function () {
    return {
        restrict: 'EA',
        templateUrl: 'views/templates/FuncaoPretendida.html'
    }
});