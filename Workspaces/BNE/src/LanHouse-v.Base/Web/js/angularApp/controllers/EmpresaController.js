var EmpresasController = angular.module('EmpresasController', ['infinite-scroll']);

EmpresasController.controller('EmpresasController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', function ($scope, $http, $location, $route, $rootScope, $filter) {

    $rootScope.isHome = true;
    $('#bntMaisEmpresas').hide();

    var store = this;

    store.empresa = {};
    store.curriculo = {}; //Recebe o curriculo (usuario) logado caso exista


    if (sessionStorage.curriculo !== undefined && sessionStorage.curriculo != "null") {
        store.curriculo = angular.fromJson(sessionStorage.curriculo);
        $rootScope.usuarioLogado = true;
        $rootScope.nomeUsuario = store.curriculo.nome;
    }

    $scope.primeiraVez = true;
    $scope.carregando = false;
    $scope.carregandoPorDemanda = false;
    
    $scope.CarregarEmpresas = function (rows) {

        if ($scope.carregando) {
            return;
        }

        $scope.carregando = true;

        if (rows == 6) {
            $('.DivProgresso').hide();
            $scope.carregandoPorDemanda = true;
        }
        
        var start = 0;

        if ($rootScope.empresas !== undefined && $rootScope.empresas != null)
            start = $rootScope.empresas.length;
        
        $('#divError').hide();
        $http.get(urlAPI + "Empresa?idFuncao=0&filtro=&start=" + start + "&rows=" + rows + "&geolocalizacao=" + sessionStorage.getItem("geoLocalizacao"))
            .success(function (data, status, headers, config) {
                if (rows == 21) {
                    $rootScope.empresas = data;
                } else {
                    $.each(data, function (i, v) {
                        $rootScope.empresas.push(v);
                    });
                }
                
            })
            .error(function (data, status, headers, config) {
                console.log(data);
                $scope.carregando = false;
                $('#divError').show();
            }).finally(function () {
                $scope.carregando = false;
                $scope.carregandoPorDemanda = false;
                $scope.primeiraVez = false;
            });
    }

    $scope.CarregarLogoEmpresa = function (cnpj) {
        
        if (cnpj !== undefined) {
            //logo
            $http.get(urlAPI + "FilialLogo?cnpj=" + cnpj, { responseType: 'arraybuffer' })

                 .success(function (data, status, headers, config) {

                     if (data.byteLength > 0) {
                         var arrayBufferView = new Uint8Array(data);
                         var blob = new Blob([arrayBufferView], { type: "image/png" });
                         var urlCreator = window.URL || window.webkitURL;
                         var imageUrl = urlCreator.createObjectURL(blob);

                         $scope.logoEmpresaVaga = imageUrl;
                     } else {
                         $scope.logoEmpresaVaga = 'images/bne-logo.png';
                     }

                     
                 })
                 .error(function (data, status, headers, config) {
                     $scope.logoEmpresaVaga = 'images/bne-logo.png';
                     console.log(data);
                 });
        }
    }

    //Carregar as 20 primeiras empresas
    if ($location.$$url == '/Empresas') {
        $scope.CarregarEmpresas(21);
    }

    //Carregar mais 6 empresas ao rolar a tela
    $scope.loadMore = function () {
        var totalEmpresasCarregadas = 0;

        if ($rootScope.empresas !== undefined && $rootScope.empresas != null) {
            totalEmpresasCarregadas = $rootScope.empresas.length;
            
            if (totalEmpresasCarregadas <= 63) {
                $scope.CarregarEmpresas(6);
            } else {
                $('#bntMaisEmpresas').show();
            }
        }
    }

    //carregar mais empresas no clique do botão
    $scope.CarregarMais = function () {
        $scope.CarregarEmpresas(6);
    }

    $scope.MaisDadosEmpresa = function (element) {
        
        //$scope.logoEmpresaVaga = null;

        //$scope.CarregarLogoEmpresa(element.empresa.cnpj);

        store.empresa = {};
        store.empresa = element.empresa;
        store.empresa.vagas = element.empresa.vagas;
        $scope.verDadosEmpresa = true;
        $scope.verReg3 = false;
        $scope.verReg2 = false;
        $scope.verReg1 = false;
        $scope.verRegFim = false;
        $scope.verVip = false;
        $('#DivErroEnviarEmail').addClass('inativo');
        $('#DivIntencaoJaEnviada').addClass('inativo');
    };

    $scope.FecharModal = function () {
        $('#ModalDadosEmpresa').modal('hide');
    };

    $scope.RedirecionarLogin = function () {
        //Armazena a emrpesa em uma variável temporária para efetuar o envio no final do cadastro ou login do candidato
        $('#ModalDadosEmpresa').modal('hide');
        sessionStorage.EmpresaTemp = angular.toJson(store.empresa);
        $rootScope.redirecionarMantendoOrigem('Login');
    };

    $scope.RedirecionaCompraVip = function () {
        $scope.FecharModal();
        $rootScope.redirecionarMantendoOrigem('Vip');
    };

    $scope.ProcessarIntencaoOK = function () {

        var idCurriculo = store.curriculo.idCurriculo;

        //Currículo VIP
        if (store.curriculo.isVip) {
            $scope.verDadosEmpresa = false;
            $scope.verVip = true;
            return;
        } else {
            store.curriculo.qtdEnvioCVEmpresa = store.curriculo.qtdEnvioCVEmpresa - 1;
            sessionStorage.curriculo = angular.toJson(store.curriculo);
        }

        //Mostra Popup pós candidatura
        switch (store.curriculo.qtdEnvioCVEmpresa) {
            case (1):
                $scope.verDadosEmpresa = false;
                $scope.verReg1 = true;
                break;
            case (2):
                $scope.verDadosEmpresa = false;
                $scope.verReg2 = true;
                break;
            case (3):
                $scope.verDadosEmpresa = false;
                $scope.verReg3 = true;
                break;
            default:
                $scope.verDadosEmpresa = false;
                $scope.verRegFim = true;
                break;
        }
    }

    $scope.ProcessarIntencaoErro = function () {
        $('#DivIntencaoJaEnviada').addClass('inativo');
        $('#DivErroEnviarEmail').removeClass('inativo');
    }

    $scope.ProcessarIntencao = function () {

        $('#divProcessandoEnvio').removeClass('inativo');
        $('#bntEnviarCV').attr('disabled', 'disabled');

        //Inicia variaveis
        var idCurriculo = store.curriculo.idCurriculo;
        var idEmpresa = store.empresa.id;

        $http.post(urlAPI + "Empresa/EfetuarEnvioCV?idCurriculo=" + idCurriculo + "&idEmpresa=" + idEmpresa + "&idCandidato=" + store.curriculo.idPessoaFisica + "&nomeCandidato=" + store.curriculo.nome)
            .success(function (data, status, headers, config) {
                if (data == true)
                    $scope.ProcessarIntencaoOK();
                else
                    $scope.ProcessarIntencaoErro();
            })
            .error(function (data, status, headers, config) {
                $scope.ProcessarIntencaoErro();
            }).finally(function () {
                $('#divProcessandoEnvio').addClass('inativo');
                $('#bntEnviarCV').removeAttr('disabled');
            });
    };

    $scope.ProcessaIntencaoJaExistente = function () {
        $('#DivIntencaoJaEnviada').removeClass('inativo');
    };

    //enviar Currículo para a empresa
    $scope.EnviarCurriculo = function () {

        //se não estiver logado vai redirecionar para a tela de login
        if (store.curriculo.idCurriculo == null) {
            $scope.RedirecionarLogin();
            return;
        }

        //Esconde div erro
        $('#DivErroEnviarEmail').addClass('inativo');

        //checar se pode enviar o CV para a empresa
        if (store.curriculo.qtdEnvioCVEmpresa <= 0 && store.curriculo.isVip == false) {
            $scope.verDadosEmpresa = false;
            $scope.verRegFim = true;
            return;
        }

        //Inicia variaveis
        var idCurriculo = store.curriculo.idCurriculo;
        var idEmpresa = store.empresa.id;

        //Valida se o candidato já envio o CV para a empresa
        $http.post(urlAPI + "Empresa/VerificaEnvioCurriculo?idCurriculo=" + idCurriculo + "&idEmpresa=" + idEmpresa)
            .success(function (data, status, headers, config) {
                if (data == "false")
                    $scope.ProcessarIntencao();
                else
                    $scope.dataEnvio = data;
                    $scope.ProcessaIntencaoJaExistente();
            })
            .error(function (data, status, headers, config) {
                $scope.ProcessarIntencaoErro();
            });

    }

    //Se existir empresa temp, o usuário foi redirecionado para o login no momento do envio, então realiza o processo agora
    if (sessionStorage.EmpresaTemp != null) 
    {
        store.empresa = angular.fromJson(sessionStorage.EmpresaTemp);

        $scope.verDadosEmpresa = true;
        $('#ModalDadosEmpresa').modal('show');

        $scope.EnviarCurriculo();

        sessionStorage.removeItem("EmpresaTemp")
    }

}]);

EmpresasController.filter('limparNome', function () {
    return function (text) {
        return text.replace('Ltda.', '').replace('Ltda', '');
    }
});