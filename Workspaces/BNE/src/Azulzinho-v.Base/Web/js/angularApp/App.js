(function () {

    var serviceBase = urlAPIAuth;
    
    var app = angular.module('Azulzinho', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'LoginController', 'authService', 'authInterceptorService', 'ui.mask', 'LanDirectives',
        'AzulzinhoController', 'VagasController', 'HeaderController', 'VipController', 'xeditable', 'ui.bootstrap', 'webcam', 'QuemSomosController', 'ngCookies', 'camposDirectives']);

    app.run(function (editableOptions) {
        editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
    });

    app.config(function ($sceDelegateProvider) {

        $sceDelegateProvider.resourceUrlWhitelist([
            'self',
            'localhost:26840/'
        ]);
    });

    app.config(['$httpProvider', function ($httpProvider) { $httpProvider.defaults.useXDomain = true; delete $httpProvider.defaults.headers.common['X-Requested-With']; }]);

    app.config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/Login', {
            templateUrl: 'views/login.html',
            controller: 'LoginController',
            controllerAs:'loginCon'
        })
        .when('/Cadastro', {
            templateUrl: 'views/curriculo/cadastro-Passo1.html',
            controller: 'CadastroCVController',
            controllerAs: 'cadCV'
        })
        .when('/CadastroPasso2', {
            templateUrl: 'views/curriculo/cadastro-Passo2.html',
            controller: 'CadastroCVController',
            controllerAs: 'cadCV'
        })
        .when('/CadastroPasso3', {
                templateUrl: 'views/curriculo/cadastro-Passo3.html',
                controller: 'CadastroCVController',
                controllerAs: 'cadCV'
        })
        .when('/CadastroPasso4', {
            templateUrl: 'views/curriculo/cadastro-Passo4.html',
            controller: 'CadastroCVController',
            controllerAs: 'cadCV'
        })
        .when('/EscolherModeloCurriculo', {
            templateUrl: 'views/curriculo/EscolherModeloCV.html',
            controller: 'CadastroCVController',
            controllerAs: 'imprimirCV'
        })
        .when('/Imprimir', {
            templateUrl: 'views/curriculo/EscolherModeloCV.html',
            controller: 'CadastroCVController',
            controllerAs: 'imprimirCV'
        })
        .when('/QuemSomos', {
            templateUrl: 'views/QuemSomos/QuemSomos.html',
            controller: 'QuemSomosController'
        })
            .when('/Empresas', {
                templateUrl: 'views/empresas/empresas.html',
                controller: 'EmpresasController',
                controllerAs: 'listaEmpresasControle'
            })
        .when('/Candidatar', {
            templateUrl: 'views/vagas/cadastro-vagas.html',

        })
        .when('/Vip', {
            templateUrl: 'views/telaMagica/telaMagica.html',
            controller: 'VipController',
            controllerAs: 'vipController'
        })
        .when('/naoEncontrado', {
            templateUrl: 'views/naoEncontrado.html',
            controller: 'naoEncontradoController'
            })
        .when('/Home', {
            templateUrl: 'views/Home.html',
            controller: 'VagasController',
            controllerAs: 'listaVagasControle'
            })
        .otherwise({
            redirectTo: '/Home'
        })
    }]);

    app.constant('ngAuthSettings', {
        apiServiceBaseUri: serviceBase,
        clientId: 'ngAuthApp'
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    });

    app.run(['authService', function (authService) {
        authService.fillAuthData();
    }]);

    app.controller('footerController', ['$scope', '$http', '$anchorScroll', function ($scope, $http, $anchorScroll) {
    
        $scope.templates = 
            [{name: 'footer.html',url:'views/footer.html'},
            {name: 'header.html',url:'views/header.html'}];

        $scope.template = $scope.templates[0];

        $scope.goToTop = function () {
            $anchorScroll();
        }

    }]);

    app.filter('truncate', function () {
        return function (text, length, end) {
            if (isNaN(length))
                length = 10;

            if (end === undefined)
                end = "...";

            if (text != null){
                if (text.length <= length || text.length - end.length <= length) {
                    return text;
                }
                else {
                    return String(text).substring(0, length - end.length) + end;
                }
        }
        };
    });

    app.filter('newlines', function () {
        return function (text) {
            return text.replace(/\n/g, '<br/>');
        }
    })
    .filter('noHTML', function () {
        return function (text) {
            return text
                    .replace(/&/g, '&amp;')
                    .replace(/>/g, '&gt;')
                    .replace(/</g, '&lt;');
        }
    });


})();