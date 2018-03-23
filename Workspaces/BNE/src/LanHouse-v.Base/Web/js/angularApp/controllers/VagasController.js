var VagasController = angular.module('VagasController', ['infinite-scroll']);

VagasController.controller('VagasController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', function ($scope, $http, $location, $route, $rootScope, $filter) {

    $rootScope.isHome = true;

    var store = this;
    
    store.vaga = {};
    store.empresa = {};
    store.curriculo = {}; //Recebe o curriculo (usuario) logado caso exista
    //$scope.pesquisou = false;

    if (sessionStorage.curriculo != null && sessionStorage.curriculo != "null") {
        store.curriculo = angular.fromJson(sessionStorage.curriculo);
        $rootScope.usuarioLogado = true;
        $rootScope.nomeUsuario = store.curriculo.nome;
    }

    $rootScope.vagas = $rootScope.vagasPerfil;
    $scope.carregando = false;
    $scope.carregandoPorDemanda = false;

    if (($rootScope.vagas == null || $rootScope.vagas == undefined) && $location.$$url == '/Vagas' && !$rootScope.pesquisou)
    {
        $rootScope.temSpellCheck = false;
        $scope.carregando = true;
        $http.get(urlAPI + "Vaga?idFuncao=0&filtro=&start=0&rows=20&geolocalizacao=" + sessionStorage.getItem("geoLocalizacao"))
            .success(function (data, status, headers, config) {
                $rootScope.vagas = data;
                $scope.carregando = false;
            })
            .error(function (data, status, headers, config) {
                console.log(data);
                $scope.carregando = false;
            });
    }

    //Excuta uma busca  a partir de uma suggest.
    $scope.novaBusca = function (termo) {

        $('#filtroVagas').val(termo);
        $scope.AtualizarListaVagas();
        $rootScope.temSpellCheck = false;
    }

    $scope.abrirModal = function () {
        $('#ModalDadosVaga').modal('show');
    };

    //Checar se o candidato logado já fez a candidatura para a vaga que será exibida no modal
    $scope.ChecarCandidatura = function () {

        //Inicia variaveis
        var idCurriculo = store.curriculo.idCurriculo;
        var idVaga = store.vaga.id;

        //Valida se o candidato já se candidatou a esta vaga
        $http.post(urlAPI + "Candidatura/VerificaCandidaturaVaga?idCurriculo=" + idCurriculo + "&IdVaga=" + idVaga)
            .success(function (data, status, headers, config) {
                if (data == false) {
                    $('#DivCandidaturaJaEfetuada').addClass('inativo');
                    $('#btnCandidatar').show();
                }else {
                    $('#DivCandidaturaJaEfetuada').removeClass('inativo');
                    $('#btnCandidatar').hide();
                }
            })
            .error(function (data, status, headers, config) {
                
            });

    }

    $scope.MaisDadosVaga = function (element) {
        store.vaga = {};
        store.vaga = element.vaga;
        $scope.verDadosVaga = true;
        $scope.verReg3 = false;
        $scope.verReg2 = false;
        $scope.verReg1 = false;
        $scope.verReg0 = false;
        $scope.verRegFim = false;
        $scope.verVip = false;
        $('#DivErroCandidatura').addClass('inativo');
        $('#DivCandidaturaJaEfetuada').addClass('inativo');

        if (store.curriculo.idCurriculo == null) {
            return;
        }

        $scope.ChecarCandidatura();
    };

    $scope.MaisDadosEmpresa = function () {
        store.empresa = {};
    };

    $scope.AtualizarListaVagas = function () {

        $scope.carregando = true;
        $rootScope.pesquisou = true;

        $("body").animate({ scrollTop: 0 }, "slow");
        $rootScope.vagas = {};
        var filtro = $('#filtroVagas').val();

        $http.get(urlAPI + "Vaga?idFuncao=0&filtro=" + encodeURIComponent(filtro) + "&start=0&rows=20&geolocalizacao=" + sessionStorage.getItem("geoLocalizacao"))
            .success(function (data, status, headers, config) {
                
                if (data.length >= 1) {
                    if (data[0].spellChecker !== undefined && data[0].spellChecker != null && data[0].spellChecker.length > 0) {
                        //mostrar o suggest
                        $rootScope.vagas = {};
                        $rootScope.temSpellCheck = true;
                        $rootScope.sugest = data[0].spellChecker;
                    }

                    if (!$rootScope.temSpellCheck)
                    $rootScope.vagas = data;

                    return;
                } else {
                    $rootScope.vagas = {};
                }
                
                $scope.carregando = false;
                $rootScope.pesquisou = false;
            })
            .error(function (data, status, headers, config) {
                console.log(data);
                $scope.carregando = false;
                $rootScope.pesquisou = false;
            });

        if ($location.path != '/Vagas')
            $rootScope.redirecionarMantendoOrigem('Vagas');
    };

    $scope.AtualizarListaVagasByEnter = function (keyEvent) {

        if (keyEvent.charCode != 13)
            return;

        $scope.carregando = true;

        $rootScope.vagas = {};

        $("body").animate({ scrollTop: 0 }, "slow");

        var filtro = $('#filtroVagas').val();

        $http.get(urlAPI + "Vaga?idFuncao=0&filtro=" + encodeURIComponent(filtro) + "&start=0&rows=20&geolocalizacao=" + sessionStorage.getItem("geoLocalizacao"))
            .success(function (data, status, headers, config) {

                if (data.length >= 1) {
                    if (data[0].spellChecker !== undefined && data[0].spellChecker != null && data[0].spellChecker.length > 0) {
                        //mostrar o suggest
                        $rootScope.vagas = {};
                        $rootScope.temSpellCheck = true;
                        $rootScope.sugest = data[0].spellChecker;
                    }

                    if (!$rootScope.temSpellCheck)
                    $rootScope.vagas = data;

                    return;
                } else {
                    $rootScope.vagas = {};
                }

                $scope.carregando = false;
                $rootScope.pesquisou = false;
            })
            .error(function (data, status, headers, config) {
                console.log(data);
                $scope.carregando = false;
                $rootScope.pesquisou = false;
            });

        if ($location.path != '/Vagas')
            $rootScope.redirecionarMantendoOrigem('Vagas');
    };

    $scope.RedirecionarLogin = function () {
        //Armazena a vaga em uma variável temporária para efetuar a candidatura no final do cadastro ou login do candidato
        $('#ModalDadosVaga').modal('hide');
        sessionStorage.VagaTemp = angular.toJson(store.vaga);
        $rootScope.redirecionarMantendoOrigem('Login');
    };

    $scope.EnviarCandidatura = function () {

        if (store.curriculo.idCurriculo == null)
        {
            $scope.RedirecionarLogin();
            return;
        }

        //Esconde div erro
        $('#DivErroCandidatura').addClass('inativo');
        $('#DivCandidaturaJaEfetuada').addClass('inativo');

        if (store.curriculo.qtdCandidatura <= 0 && store.curriculo.isVip == false)
        {
            $scope.verDadosVaga = false;
            $scope.verRegFim = true;
            return;
        }

        //Inicia variaveis
        var idCurriculo = store.curriculo.idCurriculo;
        var idVaga = store.vaga.id;
        
        //Valida se o candidato já se candidatou a esta vaga
        $http.post(urlAPI + "Candidatura/VerificaCandidaturaVaga?idCurriculo=" + idCurriculo + "&IdVaga=" + idVaga)
            .success(function (data, status, headers, config) {
                if (data == false)
                    $scope.ProcessarCandidatura();
                else
                    $scope.ProcessaCandidaturaJaExistente();
            })
            .error(function (data, status, headers, config) {
                $scope.ProcessaCandidaturaErro();
            });
    };

    $scope.ProcessarCandidatura = function () {
        //Inicia variaveis
        var idCurriculo = store.curriculo.idCurriculo;
        var idVaga = store.vaga.id;

        $http.post(urlAPI + "Candidatura/EfetuarCandidatura?idCurriculo=" + idCurriculo + "&IdVaga=" + idVaga)
            .success(function (data, status, headers, config) {
                if (data == true)
                    $scope.ProcessaCandidaturaOK();
                else
                    $scope.ProcessaCandidaturaErro();
            })
            .error(function (data, status, headers, config) {
                $scope.ProcessaCandidaturaErro();
            });
    };

    $scope.ProcessaCandidaturaOK = function () {

        var idCurriculo = store.curriculo.idCurriculo;

        //Currículo VIP
        if (store.curriculo.isVip)
        {
            $scope.verDadosVaga = false;
            $scope.verVip = true;
            return;
        }

        //Vaga Livre (não desconta o número de candidaturas
        if (!store.vaga.bneRecomenda)
        {
            store.curriculo.qtdCandidatura = store.curriculo.qtdCandidatura - 1;
            sessionStorage.curriculo = angular.toJson(store.curriculo);
        }
            
        //Mostra Popup pós candidatura
        switch (store.curriculo.qtdCandidatura) {
            case (0):
                $scope.verDadosVaga = false;
                $scope.verReg0 = true;
                break;
            case (1):
                $scope.verDadosVaga = false;
                $scope.verReg1 = true;
                break;
            case (2):
                $scope.verDadosVaga = false;
                $scope.verReg2 = true;
                break;
        }
    };

    $scope.ProcessaCandidaturaErro = function () {
        $('#DivErroCandidatura').removeClass('inativo');
    };

    $scope.ProcessaCandidaturaJaExistente = function () {
        $('#DivCandidaturaJaEfetuada').removeClass('inativo');
    };

    $scope.RedirecionaCompraVip = function () {
        $scope.FecharModal();
        $rootScope.redirecionarMantendoOrigem('Vip');
    };

    $scope.FecharModal = function () {
        $('#ModalDadosVaga').modal('hide');
    };

    //exibir campo de busca no header
    $('#tags, #btnTags').show();

    $scope.loadMore = function () {
        if ($rootScope.vagas !== null && $rootScope.vagas !== undefined) {
            $scope.carregandoPorDemanda = true;

            if ($rootScope.vagas.length == null)
                $rootScope.vagas.length = 0;

            var filtro = $('#filtroVagas').val();
            var start = $rootScope.vagas.length;
            var rows = 6;

            $http.get(urlAPI + "Vaga?idFuncao=0&filtro=" + encodeURIComponent(filtro) + "&start=" + start + "&rows=" + rows + '&geolocalizacao=' + sessionStorage.getItem("geoLocalizacao"))
                .success(function (data, status, headers, config) {
                    $.each(data, function (i, v) {
                        if (v.id > 0)
                        $rootScope.vagas.push(v);
                    })
                    $scope.carregandoPorDemanda = false;
                })
                .error(function (data, status, headers, config) {
                    console.log(data);
                    $scope.carregandoPorDemanda = false;
                });
        }
    };

    if (sessionStorage.VagaTemp != null) //Se existir vaga temp, o usuário foi redirecionado para o login no momento da candidatura, então realiza o processo agora
    {
        store.vaga = angular.fromJson(sessionStorage.VagaTemp);

        $scope.verDadosVaga = true;
        $('#ModalDadosVaga').modal('show');

        $scope.EnviarCandidatura();

        sessionStorage.removeItem("VagaTemp")
    } else if (store.curriculo.qtdCandidatura == 3) {
        $('#ModalDadosVaga').modal('show');
        $scope.verReg3 = true;
    }
}]);
