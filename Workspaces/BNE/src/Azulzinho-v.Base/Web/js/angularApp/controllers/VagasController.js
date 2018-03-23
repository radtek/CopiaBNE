var VagasController = angular.module('VagasController', ['infinite-scroll']);

VagasController.controller('VagasController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', '$sce',
    function ($scope, $http, $location, $route, $rootScope, $filter, $sce) {

        $rootScope.isHome = true;

        $('#ModalDadosVaga').modal('hide');

        var store = this;

        store.vaga = {};
        store.empresa = {};
        store.curriculo = {}; //Recebe o curriculo (usuario) logado caso exista

        if (sessionStorage.curriculo != null && sessionStorage.curriculo != "null") {
            store.curriculo = angular.fromJson(sessionStorage.curriculo);
        }

        $rootScope.vagas = $rootScope.vagasPerfil;
        $scope.carregando = false;
        $scope.carregandoPorDemanda = false;

        if (($rootScope.vagas == null || $rootScope.vagas == undefined) && $location.$$url == '/Home' && !$rootScope.pesquisou) {
            $rootScope.temSpellCheck = false;
            $scope.carregando = true;
            $http.get(urlAPI + "Vaga/GetVagasAzulzinho?idFuncao=0&filtro=&start=0&rows=18")
                .success(function (data, status, headers, config) {
                    $rootScope.vagas = data;
                    $scope.carregando = false;
                })
                .error(function (data, status, headers, config) {
                    console.log(data);
                    $scope.carregando = false;
                });
        }

        //carregar as noticias do blog
        $http.get(urlAPI + "RSS")
        .success(function (data, status, headers, config) {
            $rootScope.posts = data;
            console.log('noticias => ', data);
        })
        .error(function (data, status, headers, config) {
            console.log(data);
        });

        $scope.formatarHtml = function (descricao) {
            return $sce.trustAsHtml(descricao);
        }

        //carregar as estatísticas
        $http.get(urlAPI + 'Estatistica')
            .success(function (data) {
                sessionStorage.estatisticas = angular.toJson(data);
                localStorage.setItem('estatisticas', angular.toJson(data));

            })
            .error(function (data) {
                console.log('Ocorreu um erro ao carregar as estatístiscas');
            });

        //Excuta uma busca  a partir de uma suggest.
        $scope.novaBusca = function (termo) {

            $('#filtroFuncao').val(termo);
            $scope.AtualizarListaVagas();
            $rootScope.temSpellCheck = false;
        }

        //redireciona para a busca de agencias no WordPress.
        $scope.buscarAgencias = function () {
            window.location.href = 'http://noticias.azulzinho.com.br/?s=' + $('#q').val() + '&post_type=agencias';
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
                    } else {
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
            var filtroFuncao = $('#filtroFuncao').val();
            var filtroLocalidade = $('#filtroLocalidade').val();
            var filtro = '';

            if (filtroFuncao != "" && filtroLocalidade != "") {
                filtro = filtroFuncao.concat(' ', filtroLocalidade);
            } else {
                filtro = filtro.concat(filtroFuncao, filtroLocalidade);
            }

            $http.get(urlAPI + "Vaga/GetVagasAzulzinho?filtro=" + encodeURIComponent(filtro) + "&start=0&rows=18")
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
        };

        $scope.AtualizarListaVagasByEnter = function (keyEvent) {

            if (keyEvent.charCode != 13)
                return;

            $scope.carregando = true;

            $rootScope.vagas = {};

            $("body").animate({ scrollTop: 0 }, "slow");

            var filtroFuncao = $('#filtroFuncao').val();
            var filtroLocalidade = $('#filtroLocalidade').val();
            var filtro = '';

            if (filtroFuncao != "" && filtroLocalidade != "") {
                filtro = filtroFuncao.concat(' ', filtroLocalidade);
            } else {
                filtro = filtro.concat(filtroFuncao, filtroLocalidade);
            }

            $http.get(urlAPI + "Vaga/GetVagasAzulzinho?filtro=" + encodeURIComponent(filtro) + "&start=0&rows=18")
                .success(function (data, status, headers, config) {

                    if (data.length >= 1) {
                        if (data[0].spellChecker !== undefined && data[0].spellChecker != null && data[0].spellChecker.length > 0) {
                            //mostrar o suggest
                            $rootScope.temSpellCheck = true;
                            $rootScope.sugest = data[0].spellChecker;
                        }

                        if (!$rootScope.temSpellCheck)
                            $rootScope.vagas = data;
                        else
                            $rootScope.vagas = {};


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
        };

        $scope.RedirecionarLogin = function () {
            //Armazena a vaga em uma variável temporária para efetuar a candidatura no final do cadastro ou login do candidato
            $('#ModalDadosVaga').modal('hide');
            sessionStorage.VagaTemp = angular.toJson(store.vaga);
            $rootScope.redirecionarMantendoOrigem('Login');
        };

        $scope.EnviarCandidatura = function () {

            if (store.curriculo.idCurriculo == null) {
                $scope.RedirecionarLogin();
                return;
            }

            //Esconde div erro
            $('#DivErroCandidatura').addClass('inativo');
            $('#DivCandidaturaJaEfetuada').addClass('inativo');

            //Inicia variaveis
            var idCurriculo = store.curriculo.idCurriculo;
            var idVaga = store.vaga.id;

            //Valida se o candidato já se candidatou a esta vaga
            $http.post(urlAPI + "Candidatura/VerificaCandidaturaVaga?idCurriculo=" + idCurriculo + "&idVaga=" + idVaga)
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

            $http.post(urlAPI + "Candidatura/EfetuarCandidaturaAzulzinho?idCurriculo=" + idCurriculo + "&idVaga=" + idVaga)
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
            $scope.verVip = true;
            $scope.verDadosVaga = false;

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

                var start = $rootScope.vagas.length;
                var rows = 12;
                var filtroFuncao = $('#filtroFuncao').val();
                var filtroLocalidade = $('#filtroLocalidade').val();
                var filtro = '';

                if (filtroFuncao != "" && filtroLocalidade != "") {
                    filtro = filtroFuncao.concat(' ', filtroLocalidade);
                } else {
                    filtro = filtro.concat(filtroFuncao, filtroLocalidade);
                }


                $http.get(urlAPI + "Vaga/GetVagasAzulzinho?filtro=" + encodeURIComponent(filtro) + "&start=" + start + "&rows=" + rows)
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
