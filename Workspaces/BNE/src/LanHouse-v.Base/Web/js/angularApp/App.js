(function () {

    var serviceBase = urlAPIAuth;
    
    var app = angular.module('LanHouse', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'LoginController', 'authService', 'authInterceptorService', 'ui.mask', 'LanDirectives',
        'LanControllers', 'VagasController', 'HeaderController', 'VipController', 'xeditable', 'ui.bootstrap', 'webcam', 'EmpresasController', 'ngCookies', 'camposDirectives', 'breadcrumbsDirectives']);

    app.run(function (editableOptions) {
        editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
    });

    //app.config(function ($sceDelegateProvider) {

    //    $sceDelegateProvider.resourceUrlWhitelist([
    //        'self',
    //        'localhost:26840/'
    //    ]);
    //});

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
        .when('/Vagas', {
            templateUrl: 'views/vagas/vagas.html',
            controller: 'VagasController',
            controllerAs: 'listaVagasControle'
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
            controller: 'HomeController'
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

    app.controller('footerController', ['$scope', '$http', function ($scope, $http) {
    
        $scope.templates = 
            [{name: 'footer.html',url:'views/footer.html'},
            {name: 'header.html',url:'views/header.html'}];

        $scope.template = $scope.templates[0];

    }]);

    app.controller('CarregarFilialController', ['$q', '$scope', '$http', '$location', 'authService', '$rootScope', '$cookieStore', function ($q, $scope, $http, $location, $authService, $rootScope, $cookieStore) {

        var store = this;
        store.Filial = [];

        $('#loading').show();

        //pegar dados do cookie se existir
        nomeLan = localStorage.getItem('nomeLan');

        $scope.PegarImagemdoCache = function () {

            var arrayBufferView = new Uint8Array(angular.toJson(localStorage.getItem('blob')));
            var blob = new Blob([arrayBufferView], { type: "image/png" });
            var urlCreator = window.URL || window.webkitURL;
            var imageUrl = urlCreator.createObjectURL(blob);

            $scope.pathLogoFilial = imageUrl;
        }

        //Carregar a logo da Filial
        $scope.CarregarLogoFilial = function () {

            //logo
            $http.get(urlAPI + "FilialLogo?cnpj=" + sessionStorage.getItem('cnpj'), { responseType: 'arraybuffer' })

                 .success(function (data, status, headers, config) {

                     var arrayBufferView = new Uint8Array(data);
                     var blob = new Blob([arrayBufferView], { type: "image/png" });
                     var urlCreator = window.URL || window.webkitURL;
                     var imageUrl = urlCreator.createObjectURL(blob);

                     $scope.pathLogoFilial = imageUrl;

                 })
                 .error(function (data, status, headers, config) {
                     console.log('Ocorreu um erro ao carregar a logomarca');
                     $('#mydiv').hide();
                 });
        }

        $rootScope.title = "Lan House BNE";

        if (origem == 'escolas.bne.com.br')
            $rootScope.title = "Escolas BNE";
        else if(origem == 'parceiro.bne.com.br')
            $rootScope.title = "parceiros BNE";


        if (lan == '/' && nomeLan !== undefined && nomeLan != null)
        {
            //setar os dados na session para usar enquanto o candidato estive na Lan
            sessionStorage.setItem('nomeFantasia', localStorage.getItem('nomeFantasia'));
            sessionStorage.setItem('cnpj', localStorage.getItem('cnpj'));
            sessionStorage.setItem('origem', localStorage.getItem('origem'));
            sessionStorage.setItem('geoLocalizacao', localStorage.getItem('geoLocalizacao'));
            sessionStorage.estatisticas = localStorage.getItem('estatisticas');

            $rootScope.geoLocalizacao = localStorage.getItem('geoLocalizacao');

            $scope.pathLogoFilial = localStorage.getItem('logoLan');
            //$scope.CarregarLogoFilial();

            $('#mydiv').hide();
            return;
        }
        
        var defer = $q.defer();
        $http.get(urlAPI + "Filial?partialName=" + lan.replace('/',''))
            .success(function (data, status, headers, config) {

                $('#mydiv').hide();

                if (data === undefined || data == null) {
                    $location.path('/naoEncontrado');
                } else {
                    store.Filial = data;

                    sessionStorage.setItem('nomeFantasia', store.Filial.nomeFantasia);
                    sessionStorage.setItem('cnpj', store.Filial.cnpj);
                    sessionStorage.setItem('geoLocalizacao', store.Filial.geoLocalizacao);
                    $rootScope.geoLocalizacao = store.Filial.geoLocalizacao;
                    
                    //guarda os dados da Lan no cache para nao precisar ir na base toda vez.
                    localStorage.setItem('nomeLan', lan);
                    localStorage.setItem('nomeFantasia', store.Filial.nomeFantasia);
                    localStorage.setItem('cnpj', store.Filial.cnpj);
                    localStorage.setItem('origem', store.Filial.idFilialOrigem);

                    localStorage.setItem('geoLocalizacao', store.Filial.geoLocalizacao);
                    localStorage.setItem('logoLan', store.Filial.logoLan);

                    $scope.pathLogoFilial = store.Filial.logoLan;
                    //$scope.CarregarLogoFilial();

                    //carregar as estatísticas
                    $http.get(urlAPI + 'Estatistica')
                        .success(function (data) {
                            sessionStorage.estatisticas = angular.toJson(data);
                            localStorage.setItem('estatisticas', angular.toJson(data));

                        })
                        .error(function (data) {
                            console.log('Ocorreu um erro ao carregar as estatístiscas');
                        });
                }
            })
            .error(function (data, status, headers, config) {
                $('#mydiv').hide();
                console.log('Ocorreu um erro ao carregar o site, tente novamente.');
            });
    }]);

    app.controller('ImprimirCVController', ['$scope', '$http', function ($scope, $http) {

        $(document).ready(function () {
            //Ajustando o scroll
              $('#CvTradicionalModal').scroll(function () {
                //$("#divSlowDown").stop().animate({ "top": $(window).scrollTop() + ($(screen)[0].height / 4) + "px" }, { duration: 700, queue: false });
                var vTop = ($(window).height() / 2) - $("#divSlowDown").height() / 2 + $(top.window).scrollTop();

                //Ajustando a altura para não estourar o layout
                if (vTop < 80)
                    vTop = 80;
                else if (vTop > 830)
                    vTop = 825;

                $("#divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });              
            });
        });
    }]);

    app.directive('tooltip', function () {
        return {
            restrict: 'A',
            link:function(scope,element,attrs)
            {
                $(element)
                    .attr('title', scope.$eval(attrs.tooltip))
            }
        }
    });

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