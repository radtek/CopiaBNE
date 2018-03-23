var VagaController = angular.module('VagaController', []);

VagaController.controller('VagaController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', '$sce', 'authService', '$routeParams',
    function ($scope, $http, $location, $route, $rootScope, $filter, $sce, $authService, $routeParams) {
        $scope.atribuicoes = '';

        var idvaga = $routeParams.idvaga;

        $rootScope.isHome = true;

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

        $scope.logOut = function () {
            $authService.logOut();
            $rootScope.usuarioLogado = false;
            sessionStorage.curriculo = null;
            $location.path('/home');
        }

        $scope.authentication = authService.authentication;

        $scope.formatarHtml = function (descricao) {
            return $sce.trustAsHtml(descricao);
        }

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

        $scope.CarregarVaga = function (idvaga) {
            $http.get(urlAPI + "Vaga/" + idvaga).then(function (response) {
                console.log('data => ', response.data);
                $scope.atribuicoes = $sce.trustAsHtml(response.data.atribuicoes);
                $scope.MaisDadosVaga(response.data);

            })
        };

        $scope.MaisDadosVaga = function (element) {

            //console.log('vaga => ', element);

            store.vaga = {};
            store.vaga = element;
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

            console.log('vaga => ', scope.vaga);


            $scope.ChecarCandidatura();
        };

        $scope.MaisDadosEmpresa = function () {
            store.empresa = {};
        };

        if (idvaga != null) {
            $scope.CarregarVaga(idvaga);
        }

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

        //exibir campo de busca no header
        $('#tags, #btnTags').show();

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
