'use strict';
var LoginController = angular.module('LoginController', []);
LoginController.controller('LoginController', ['$http', '$rootScope', '$scope', '$location', 'localStorageService', 'authService', 'ngAuthSettings',
    function ($http, $rootScope, $scope, $location,$localStorageService, authService, ngAuthSettings) {

        var store = this;

    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };

    $rootScope.usuarioLogado = false;
    $rootScope.isHome = true;
    $scope.message = "";
    $scope.Nascimentovalido = true;

    //esconder a barra de pesquisa 
    $rootScope.isHome = false;

    //mostrar os números
    store.numeros = angular.fromJson(sessionStorage.estatisticas);

    $scope.validarDados = function () {
        if (CalcularIdade($scope.loginData.password) > 13) {
            $scope.login();
        } else {
            $scope.Nascimentovalido = false;
        }
    }

    $scope.login = function () {

        $('#bntLogin').attr('disabled', 'disabled');
        $('#divProcessandoLogin').removeClass('inativo');

        authService.login($scope.loginData).then(function (response) {
            
            $http.get(urlAPI + 'Curriculo/GetCurriculo?cpf=' + $scope.loginData.userName + '&dataNascimento=' + $scope.loginData.password + '&nome=' + $scope.loginData.nome)
            .success(function (data) {
                if (data != null) {

                    $rootScope.curriculo = data;
                    sessionStorage.curriculo = angular.toJson(data);
                    sessionStorage.Experiencias = angular.toJson(data.experiencias);

                    sessionStorage.idiomasCandidato = angular.toJson(data.idiomasCandidato);
                    sessionStorage.Formacoes = angular.toJson(data.formacoes);
                    sessionStorage.Cursos = angular.toJson(data.cursos);

                    $('#divProcessandoLogin').html('<img src="images/gif-load.gif" /> Redirecionando...')
                    $location.path($rootScope.urlorigem);

                } else {//se não tiver CVredireciona para o cadastro.
                    sessionStorage.curriculo = {};
                    $location.path('/Cadastro');
                }
            })
            .error(function (data) {
                $('#divProcessandoLogin').removeClass('inativo');
                $('#bntLogin').removeAttr('disabled');
                console.log(data);
                $location.path($rootScope.urlorigem);
            });
        },
         function (err) {
             $('#bntLogin').removeAttr('disabled');
             $('#divProcessandoLogin').html('Ops! Ocorreu um erro no login. Tente novamente');
             console.log('erro no login =>',err.error_description);
             $scope.message = err.error_description;
         });
    };

    $scope.authExternalProvider = function (provider) {

        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {

        $scope.$apply(function () {

            if (fragment.haslocalaccount == 'False') {

                authService.logOut();

                authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                $location.path('/associate');

            }
            else {
                //Obtain access token and redirect to orders
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(function (response) {

                    $location.path('/orders');

                },
             function (err) {
                 $scope.message = err.error_description;
             });
            }

        });
    }
}]);
