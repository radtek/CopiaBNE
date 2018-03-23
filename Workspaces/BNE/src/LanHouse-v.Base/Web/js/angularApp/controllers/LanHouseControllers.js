var LanControllers = angular.module('LanControllers', []);

LanControllers.controller('CadastroCVController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', '$anchorScroll', 'localStorageService', 'authService',
    function ($scope, $http, $location, $route, $rootScope, $filter, $anchorScroll, localStorageService, $authService) {
    'use strict';

    var store = this;
    store.curriculo = {};
    store.mesesAno = null;
    store.modeloCV = 'CVBNE';
    store.urlWeb = window.location.hostname;

    //identificar a rota para iniciar a tela
    var route = $location.path();

    $rootScope.isHome = false;
    $rootScope.usuarioLogado = false;
    $rootScope.nomeUsuario = '';
    $scope.Nascimentovalido = true;

    store.areaBNE = {};

    $scope.TesteDirective = function (text) {
        alert(text);
    }

        //rota para template dos campos
    $scope.campos = [];
    $scope.campos.telefonefixo = 'views/templates/TelefoneFixo.Html';
    $scope.campos.cidade = 'views/templates/Cidade.Html';


    $('#tags, #btnTags').hide();

        //pegar a authorization
    var authorization_user = localStorageService.get('authorizationData');

    //carregar as deficiencias no combo
    $scope.CarregarDeficiencias = function () {
        
        //var config = {
        //    headers: {
        //        'Authorization': 'bearer ' + authorization_user.token,
        //        'Accept': 'application/json;odata=verbose'
        //    }
        //};
        //$http.get(urlAPI + 'Deficiencia', config)
        $http.get(urlAPI + 'Deficiencia')
            .success(function (data) {
                store.deficiencias = data;
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //carregar as areas do BNE no combo
    $scope.CarregarAreasBNE = function () {
        $http.get(urlAPI + 'AreaBNE')
            .success(function (data) {
                store.areaBNE = data;
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //carregar as situações da formação no combo
    $scope.CarregarSituacaoFormacao = function () {
        $http.get(urlAPI + 'SituacaoFormacao')
            .success(function (data) {
                store.SituacaoFormacao = data;
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //carregar os estados civis no combo
    $scope.CarregarEstadoCivil = function () {
        
        $http.get(urlAPI + 'EstadoCivil')
            .success(function (data) {
                store.estadoCivil = data;
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //carregar os idiomas no combo
    $scope.CarregarIdiomas = function () {
        $http.get(urlAPI + 'Idioma')
            .success(function (data) {
                store.idiomas = data;
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //carregar as escolaridades do BNE no combo
    $scope.CarregarEscolaridade = function () {
        $http.get(urlAPI + 'Escolaridade')
        .success(function (data) {
            store.escolaridade = data;
            console.log(data);
        })
        .error(function (data) {
            console.log(data);
        });
    }

    //carregar as especializações do BNE no combo
    $scope.CarregarEscolaridadeEspecializacao = function () {
        $http.get(urlAPI + 'EscolaridadeEspecializacao')
        .success(function (data) {
            store.especializacao = data;
        })
        .error(function (data) {
            console.log(data);
        });
    }

    //Define os meses do ano para usar no passo 2
    $scope.DefinirMesesAno = function () {
        store.mesesAno = [
        { id: 1, text: 'Janeiro' },
        { id: 2, text: 'Fevereiro' },
        { id: 3, text: 'Março' },
        { id: 4, text: 'Abril' },
        { id: 5, text: 'Maio' },
        { id: 6, text: 'Junho' },
        { id: 7, text: 'Julho' },
        { id: 8, text: 'Agosto' },
        { id: 9, text: 'Setembro' },
        { id: 10, text: 'Outubro' },
        { id: 11, text: 'Novembro' },
        { id: 12, text: 'Dezembro' }
        ];
    }

    //tratar exibição das experiencias na tela passo 2
    $scope.tratarExperienciasdaBase = function () {
        store.experiencias = angular.fromJson(sessionStorage.Experiencias);
    };

    //carregar o valor do salário mínimo
    $scope.CarregarValorSalarioMinimo = function () {
        $http.get(urlAPI + 'Parametro')
            .success(function (data) {
                store.salarioMinimo = data;
                sessionStorage.setItem('salarioMinimo', data);
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //usar para validar a navegação nas telas.
    $scope.$watch('$viewContentLoaded', function () {

    });

    $scope.PreencherExperiencias = function () {
        //Última Experiência
        store.curriculo.idExperiencia = store.experiencias[0].idExperienciaProfissional;
        store.curriculo.empresa = store.experiencias[0].razao;
        store.curriculo.atividades = store.experiencias[0].desAtividades;
        store.curriculo.funcaoEmpresa = store.experiencias[0].funcaoEmpresa;
        store.curriculo.idAreaBNE = store.experiencias[0].idAreaBNE;
        store.curriculo.anoInicio = $filter('date')(store.experiencias[0].dataAdmissao, 'yyyy');
        store.curriculo.mesInicio = store.mesesAno[$filter('date')(store.experiencias[0].dataAdmissao, 'MM') - 1].id;

        
        if (store.experiencias[0].dataDemissao !== undefined && store.experiencias[0].dataDemissao != null) {
            store.curriculo.empregoAtual = 'false';
            store.curriculo.anoSaida = $filter('date')(store.experiencias[0].dataDemissao, 'yyyy');
            store.curriculo.mesSaida = store.mesesAno[$filter('date')(store.experiencias[0].dataDemissao, 'MM') - 1].id;

            $scope.isEmpregoAtual = false;
        }
        else {
            store.curriculo.empregoAtual = true;
            store.curriculo.empregoAtual = 'true';
        }

        //penultima Experiência
        if (store.experiencias.length > 1) {
            $scope.addPenultimoEmprego = true;

            store.curriculo.idExperienciape = store.experiencias[1].idExperienciaProfissional;
            store.curriculo.empresape = store.experiencias[1].razao;
            store.curriculo.atividadespe = store.experiencias[1].desAtividades;
            store.curriculo.funcaoEmpresape = store.experiencias[1].funcaoEmpresa;
            store.curriculo.idAreaBNEpe = store.experiencias[1].idAreaBNE;
            store.curriculo.anoIniciope = $filter('date')(store.experiencias[1].dataAdmissao, 'yyyy');
            store.curriculo.mesIniciope = store.mesesAno[$filter('date')(store.experiencias[1].dataAdmissao, 'MM') - 1].id;

            if (store.experiencias[1].dataDemissao !== undefined && store.experiencias[1].dataDemissao != null) {
                store.curriculo.anoSaidape = $filter('date')(store.experiencias[1].dataDemissao, 'yyyy');
                store.curriculo.mesSaidape = store.mesesAno[$filter('date')(store.experiencias[1].dataDemissao, 'MM') - 1].id;
            }
        }
        else {
            $scope.addPenultimoEmprego = false;
            store.curriculo.idExperienciape = "";
            store.curriculo.empresape = "";
            store.curriculo.atividadespe = "";
            store.curriculo.funcaoEmpresape = "";
            store.curriculo.idAreaBNEpe = "";
            store.curriculo.anoIniciope = "";
            store.curriculo.mesIniciope = "";
            store.curriculo.anoSaidape = "";
            store.curriculo.mesSaidape = "";
        }

        //terceira Experiência
        if (store.experiencias.length > 2) {
            $scope.addOutroEmprego = true;

            store.curriculo.idExperiencia3 = store.experiencias[2].idExperienciaProfissional;
            store.curriculo.empresa3 = store.experiencias[2].razao;
            store.curriculo.atividades3 = store.experiencias[2].desAtividades;
            store.curriculo.funcaoEmpresa3 = store.experiencias[2].funcaoEmpresa;
            store.curriculo.idAreaBNE3 = store.experiencias[2].idAreaBNE;
            store.curriculo.anoInicio3 = $filter('date')(store.experiencias[2].dataAdmissao, 'yyyy');
            store.curriculo.mesInicio3 = store.mesesAno[$filter('date')(store.experiencias[2].dataAdmissao, 'MM') - 1].id;

            if (store.experiencias[2].dataDemissao !== undefined && store.experiencias[2].dataDemissao != null) {
                store.curriculo.anoSaida3 = $filter('date')(store.experiencias[2].dataDemissao, 'yyyy');
                store.curriculo.mesSaida3 = store.mesesAno[$filter('date')(store.experiencias[2].dataDemissao, 'MM') - 1].id;
            }
        } else {
            $scope.addOutroEmprego = false;
            store.curriculo.idExperiencia3 = "";
            store.curriculo.empresa3 = "";
            store.curriculo.atividades3 = "";
            store.curriculo.funcaoEmpresa3 = "";
            store.curriculo.idAreaBNE3 = "";
            store.curriculo.anoInicio3 = "";
            store.curriculo.mesInicio3 = "";
            store.curriculo.anoSaida3 = "";
            store.curriculo.mesSaida3 = "";
        }
    }

    $scope.CarregarFotoCandidato = function (idCandidato) {
                    
            $http.get(urlAPI + "Curriculo/GetFotoCandidato?idCandidato=" + idCandidato, { responseType: 'arraybuffer' })
        .success(function (data, status, headers, config) {
            $scope.exibirwebcam = false;
            $scope.disablebtnImg = true;

            var arrayBufferView = new Uint8Array(data);
            var blob = new Blob([arrayBufferView], { type: "image/jpg" });
            var urlCreator = window.URL || window.webkitURL;
            var imageUrl = urlCreator.createObjectURL(blob);

            store.curriculo.imgCandidato = imageUrl;
            $scope.imgCandidato = imageUrl;
        })
        .error(function (data, status, headers, config) {
            console.log('erro ao carregar foto do candidato ', data);
        });
        
    }

    $scope.selecionarMesAnoExperiencia = function () {
        
        var selected = $filter('filter')(store.mesesAno, { id: store.curriculo.mesInicio });
        return (store.curriculo.mesInicio && selected.length) ? selected[0].text : '';
    };
    $scope.selecionarMesAnoExperienciaSaida = function () {

        if (store.curriculo.empregoAtual =='true') {
            store.curriculo.anoSaida = null;
            store.curriculo.mesSaida = null;
            return 'Atual';
        }
        var selected = $filter('filter')(store.mesesAno, { id: store.curriculo.mesSaida });
        return (store.curriculo.mesSaida && selected.length) ? selected[0].text : 'Atual';
    };

    $scope.selecionarMesAnoExperienciaPE = function () {

        var selected = $filter('filter')(store.mesesAno, { id: store.curriculo.mesIniciope });
        return (store.curriculo.mesIniciope && selected.length) ? selected[0].text : '';
    };
    $scope.selecionarMesAnoExperienciaSaidaPE = function () {

        var selected = $filter('filter')(store.mesesAno, { id: store.curriculo.mesSaidape });
        return (store.curriculo.mesSaidape && selected.length) ? selected[0].text : '';
    };

    $scope.selecionarMesAnoExperiencia3 = function () {

        var selected = $filter('filter')(store.mesesAno, { id: store.curriculo.mesInicio3 });
        return (store.curriculo.mesInicio3 && selected.length) ? selected[0].text : '';
    };

    $scope.selecionarMesAnoExperienciaSaida3 = function () {

        var selected = $filter('filter')(store.mesesAno, { id: store.curriculo.mesSaida3 });
        return (store.curriculo.mesSaida3 && selected.length) ? selected[0].text : '';
    };

    $scope.retornarLabelFuncaoExercida = function () {
        if (store.curriculo.empresa3 != '' && store.curriculo.empresa3 != null) {
            return 'Função exercida';
        }
    }

    $scope.retornarLabelAtribuicoes = function () {
        if (store.curriculo.empresa3 != '' && store.curriculo.empresa3 != null) {
            return 'Atribuições';
        }
    }

    $scope.contarVagasPerfilCandidato = function () {

        store.curriculo = angular.fromJson(sessionStorage.curriculo)
        if (store.curriculo.funcoes == null) {
            $rootScope.idFuncaoPretendida = store.curriculo.funcao;
        }
        else {
            if (store.curriculo.funcoes[0].idFuncao != null)
                $rootScope.idFuncaoPretendida = store.curriculo.funcoes[0].idFuncao;
            else
                $rootScope.idFuncaoPretendida = store.curriculo.funcoes[0].des_Funcao;
        }

        $http.get(urlAPI + "Vaga?filtro=&start=0&rows=10000&geolocalizacao=" + $rootScope.geoLocalizacao + "&idFuncao=" + $rootScope.idFuncaoPretendida)
            .success(function (data, status, headers, config) {
                $scope.totalVagas = data.length;
                $rootScope.vagasPerfil = data;
            })
            .error(function (data, status, headers, config) {
                console.log(data);
            });
    }

    //Tratar a quebra de linha dos textos grandes
    $scope.tratarTexto = function (texto, quebrarCom) {
        //checar se o texto tem espaço em branco
        var resultado = new Array(parseInt(texto.length / quebrarCom));

        for (var x = 0; x < texto.length / quebrarCom; x++) {
            resultado[x] = texto.subString(0 + x * quebrarCom, (x + 1) * quebrarCom) + ' ';
        }

        return resultado;
    }

    $scope.BuscarMediaSalarial = function () {
        var cidade = store.curriculo.cidade;
        var funcao = store.curriculo.funcao;

        //checar se o candidato preencheu a cidade e a função
        if (cidade != '' && funcao != '') {
            $http.post(urlAPI + 'PesquisaSalarial/FazerPesquisaSalarial?cidade=' + cidade.replace('/', '-') + '&funcao=' + encodeURIComponent(funcao)).success(function (res) {
                $('#mediaSalarialFuncao').html(res);
                $rootScope.fezMedia = true;
            }).error(function () {
                $('#mediaSalarialFuncao').html('Ocorreu um erro ao carregar a médial salárial. Selecione a cidade e a função novamente.');
            });
        } else {
            $('#mediaSalarialFuncao').html('Preencha a Cidade e a Função para visualizar a média salárial.');
        }
    }

    //Mostra sugestão de atividade para a função em experiências no passo 2
    $scope.CarregarSugestaoAtividade = function (campoTexto,divDestino,funcao) {
        
        if (campoTexto != '') {

            $http.post(urlAPI + 'AutoCompleteFuncao/GetSugestaoAtividades?query=' + funcao).success(function (res) {
                
                $('#' + divDestino).html(res);

                if (campoTexto == 'txtAtividade') {
                    $scope.mostrarSugestao = false;
                }
                else if (campoTexto == 'txtAtividadepe')
                    $scope.mostrarSugestaope = true;
                else
                    $scope.mostrarSugestao3 = true;

            }).error(function () {
                $scope.mostrarSugestao = false;
                $('#' + divDestino).html('');
            });

            
        }

    }

    //Inicializar CVs nas telas
    $scope.InicializarCVPasso1 = function () {

        if(store.curriculo.cpf.length >11)
            store.curriculo.cpf = store.curriculo.cpf.replace('-', '').replace('.','').replace('.','');

        //carregar função
        if (store.curriculo.funcoes != undefined && store.curriculo.funcoes != null) {
            if (store.curriculo.funcoes.length > 0) {
                if (store.curriculo.funcao == null) {
                    store.curriculo.funcao = store.curriculo.funcoes[0].des_Funcao;
                }
                $scope.BuscarMediaSalarial();
            }
        } 

        //deficiencia
        if (store.curriculo.temDeficiencia == true) {
            $scope.isDeficiencia = true;
            store.curriculo.temDeficiencia = 'true';
        } else {
            $scope.isDeficiencia = false;
            store.curriculo.temDeficiencia = 'false';
        }

        
    }

    $scope.InicializarCVPasso2 = function () {
        //se to CV tem experiencia vai preencher os campos
        if (store.curriculo.temExperienciaProfissional) {
            $scope.tratarExperienciasdaBase();
            $scope.temExperienciaProfissional = true;
            $scope.PreencherExperiencias();
        }

        //if (store.curriculo.empregoAtual) {
        //    $scope.isEmpregoAtual = 'true';
        //} else {
        //    $scope.isEmpregoAtual = 'false';
        //}

        //abrir campos do Penultimo emprego
        if (store.curriculo.empresaPE != '' && store.curriculo.empresaPE != null) {
            $scope.addPenultimoEmprego = true;
        }

        if (store.curriculo.empresa3 != '' && store.curriculo.empresa3 != null) {
            $scope.addOutroEmprego = true;
        }
    }

    $scope.InicializarCVPasso3 = function () {
        //formações

        if (sessionStorage.Formacoes !== 'undefined') {
            store.formacoes = angular.fromJson(sessionStorage.Formacoes);

            //analisar formações para exibir os botoes adicionais
            if (store.formacoes !== 'undefined' && store.formacoes != null) {
                $scope.informouFormacao = true;

                //checar se tem formação superior para exibir o botão de especializações
                var formacaoSuperior = store.formacoes.filter(function (item) { return item.idEscolaridade >= 12 });

                if (formacaoSuperior[0] != null) {
                    $scope.mostrarEspecializacao = true;
                }
            } else {
                $scope.informouFormacao = false;
            }
        } else {
            store.formacoes = [];
        }

        //cursos
        if (sessionStorage.Cursos !== 'undefined') {
            store.cursos = angular.fromJson(sessionStorage.Cursos);
        } else {
            store.cursos = [];
        }

        //idiomas candidato
        if (sessionStorage.idiomasCandidato != null) {
            store.idiomasCandidato = angular.fromJson(sessionStorage.idiomasCandidato);
        } else {
            store.idiomasCandidato = [];
        }

        //abrir campos da segunda formação
        if (store.curriculo.IdNivelFormacao2 != '' && store.curriculo.IdNivelFormacao2 != null) {
            $scope.addOutraFormacao = true;
        } else {
            $scope.addOutraFormacao = false;
        }
    }

    $scope.InicializarCVPasso4 = function () {

        //recupera imagem do candidato caso tenha enviado.
        if(store.curriculo.idPessoaFisica != null)
            $scope.CarregarFotoCandidato(store.curriculo.idPessoaFisica);

        if (store.curriculo.imgCandidato != null) {
            $scope.exibirwebcam = false;
            $scope.disablebtnImg = true;
        }

        //recuperar os periodos de trabalho
        if (sessionStorage.periodosLivres != null) {
            store.periodosLivres = null;
            store.periodosLivres = angular.fromJson(sessionStorage.periodosLivres);
        }
        else {
            store.periodosLivres = [
        { id: 'M', text: 'Manhã', status: 'true' },
        { id: 'T', text: 'Tarde', status: 'true' },
        { id: 'N', text: 'Noite', status: 'true' },
        { id: 'F', text: 'Fim de Semana', status: 'true' }
            ]
        }
    }


    //Inicializar telas
    $scope.IniciarPasso1 = function () {
        $scope.CarregarDeficiencias();
        $scope.CarregarValorSalarioMinimo();
    }

    $scope.IniciarPasso2 = function () {
        $scope.DefinirMesesAno();
        $scope.CarregarAreasBNE();
    }

    $scope.IniciarPasso3 = function () {
        //variavel pra definir se a formação deve ser preenchida por completo.
        $scope.is3Grau = false;
        $scope.mostrarEspecializacao = false;

        $scope.CarregarSituacaoFormacao();
        $scope.CarregarIdiomas();
        $scope.CarregarEscolaridade();
        $scope.CarregarEscolaridadeEspecializacao();
    }

    $scope.IniciarPasso4 = function () {
        //ocultar a WebCam ao abrir passo 4
        $scope.exibirwebcam = true;

        $scope.CarregarEstadoCivil();
        store.curriculo.alterouImagem = false;
    }

    $scope.IniciarEscolherCV = function () {
        $scope.CarregarIdiomas();
        $scope.DefinirMesesAno();
        $scope.CarregarEstadoCivil();
        $scope.CarregarValorSalarioMinimo();

        if ($scope.imgCandidato == null && store.curriculo.imgCandidato == null) {
            $scope.CarregarFotoCandidato(store.curriculo.idPessoaFisica);
        }

        //data atual usada na exibição do curriculos tradicional e moderno.
        $scope.date = new Date();

        if (sessionStorage.numeroPeriodos != '' && sessionStorage.numeroPeriodos !== 'undefined') {
            store.numeroPeriodos = sessionStorage.numeroPeriodos;
        }

        store.periodosLivres = angular.fromJson(sessionStorage.periodosLivres)

        //contar a vagas próximas ao perfil do candidato
        $scope.contarVagasPerfilCandidato();

        //cursos
        if (sessionStorage.Cursos !== 'undefined') {
            store.cursos = angular.fromJson(sessionStorage.Cursos);
            store.curriculo.cursos = angular.fromJson(sessionStorage.Cursos);

        } else {
            store.cursos = [];
        }

        if (sessionStorage.Formacoes !== 'undefined') {
            store.curriculo.formacoes = angular.fromJson(sessionStorage.Formacoes);
            store.formacoes = angular.fromJson(sessionStorage.Formacoes);
        }

        if (store.curriculo.temExperienciaProfissional) {
            $scope.tratarExperienciasdaBase();
            $scope.temExperienciaProfissional = true;
            $scope.PreencherExperiencias();
        }

        //carregar função
        if (store.curriculo.funcoes !== undefined && store.curriculo.funcoes != null) {
            if (store.curriculo.funcoes.length > 0)
                store.curriculo.funcao = store.curriculo.funcoes[0].des_Funcao;
        }

        //idiomas
        store.curriculo.idiomasCandidato = angular.fromJson(sessionStorage.idiomasCandidato);
    }

    $scope.InicarTela = function () {

        if (route == '/Cadastro') {
            $scope.IniciarPasso1();
        }
        else if(route == '/CadastroPasso2')
        {
            $scope.IniciarPasso2();
        }
        else if (route == '/CadastroPasso3') {
            $scope.IniciarPasso3();
        }
        else if (route == '/CadastroPasso4') {
            $scope.IniciarPasso4();
        } else {
            //imprimir curriculo
            //$scope.IniciarEscolherCV();
        }
    }

        //pegar a authorization
        var authorization = localStorageService.get('authorizationData');
    
    //se fez login vai iniciar a tela
    if (authorization != null) {
        if (sessionStorage.curriculo !== undefined && sessionStorage.curriculo != null) {
            $scope.InicarTela();
        } else {
            $authService.logOut();
            $rootScope.usuarioLogado = false;
            sessionStorage.curriculo = null;
            $location.path('/Login');
        }
    }
    else {
        $location.path('/Login');
    }

    //inicializar o store para curriculo
    if (sessionStorage.curriculo !== undefined && sessionStorage.curriculo != null)
    {
        store.curriculo = angular.fromJson(sessionStorage.curriculo);

        if (store.curriculo != null) {
            $rootScope.usuarioLogado = true;
            if (store.curriculo.nome != null)
                $rootScope.nomeUsuario = store.curriculo.nome;

            $('#upPasso1').html('');

            if (route == '/Cadastro') {
                $scope.InicializarCVPasso1();
            }
            else if (route == '/CadastroPasso2') {
                $scope.InicializarCVPasso2();
            }
            else if (route == '/CadastroPasso3') {
                $scope.InicializarCVPasso3();
            }
            else if (route == '/CadastroPasso4') {
                $scope.InicializarCVPasso4();
            } else {//imprimir curriculo
                $scope.IniciarEscolherCV();
            }
        }
    }
    else {
        //store.curriculo = {};
        store.Curso = [];
        $scope.addPenultimoEmprego = false;
        $scope.addOutroEmprego = false;
        $scope.isEmpregoAtual = false;

        store.curriculo.nome = sessionStorage.LoginNome;
        store.curriculo.cpf = sessionStorage.cpf;
        store.curriculo.dataNascimento = sessionStorage.dtaNascimento;
        store.curriculo.temDeficiencia = 'false'

        //variavel pra definir se tem exp. profissional.
        $scope.temExperienciaProfissional = false;

        //seleciona Perídio da manha e tarde por default
        store.curriculo.periodoManha = true;
        store.curriculo.periodoTarde = true;

        store.curriculo.mesInicio = 1; //seta janeiro no combobox de meses para início.
        store.curriculo.mesSaida = 1; //seta janeiro no combobox de meses para saída.

        store.periodosLivres = [
        { id: 'M', text: 'Manhã', status: 'true' },
        { id: 'T', text: 'Tarde', status: 'true' },
        { id: 'N', text: 'Noite', status: 'true' },
        { id: 'F', text: 'Fim de Semana', status: 'true' }
        ]
    }
    
    store.deficiencias = [];
    store.areaBNE = {};
    store.escolaridade = {};
    store.especializacao = {};
    
    store.idiomas = [];
    store.estadoCivil = {};
    store.enviaremail = {};
    store.Escolaridade1 = {};
    store.SituacaoFormacao = {};
    store.isDeficiente = [{ id: 'true', text: ' Sim' }, { id: 'false', text: ' Não' }];
    store.niveisIdiomas = [{ id: '1', text: 'Básico' }, { id: '2', text: 'Intermediário' }, { id: '3', text: 'Avançado' }, { id: '4', text: 'Fluente' }];
    store.sexos = [{ id: '1', text: 'Masculino' }, { id: '2', text: 'Feminino' }];

   //angular x-editable
    $scope.showEstadoCivil = function () {
        var selected = $filter('filter')(store.estadoCivil, { id: store.curriculo.idEstadoCivil });
        return (store.curriculo.idEstadoCivil && selected.length) ? selected[0].text : 'Selecione...';
    };

    $scope.exibirSexoCandidato = function (idSexo) {
        var selected = $filter('filter')(store.sexos, { id: idSexo });
        return (idSexo && selected.length) ? selected[0].text : 'não informado';
    };


    $scope.exibirNivelIdiomaCandidato = function (idNivel) {
        var selected = $filter('filter')(store.niveisIdiomas, { id: idNivel });
        return (idNivel && selected.length) ? selected[0].text : 'não informado';
    };

    $scope.calcularIdadeCandidato = function () {
        return CalcularIdade(store.curriculo.dataNascimento);
    };

    //atualiza os periodos selecionados para trabalhar a partir da visualização/edição do CV
    $scope.atualizarPeriodos = function () {

        $scope.mostrarPeriodosTrabalho();
        $scope.persistirCVnaSession();

        $scope.salvarCurriculo('divCVElegante');
    }

    $scope.ativarPeriodo = function () {

        if(store.curriculo.periodos =='M')
        {
            store.curriculo.periodoManha = true;
            $scope.atualizarPeriodos();
            return;
        }
        if (store.curriculo.periodos == 'T') {
            store.curriculo.periodoTarde =true;
            $scope.atualizarPeriodos();
            return;
        }
        if (store.curriculo.periodos == 'N') {
            store.curriculo.periodoNoite = true;
            $scope.atualizarPeriodos();
            return;
        }
        if (store.curriculo.periodos == 'F') {
            store.curriculo.periodoFimdeSemana = true;
            $scope.atualizarPeriodos();
            return;
        }
    }
    
    //exibir periodos de trabalho na escolha do CV e no próprio CV
    $scope.mostrarPeriodosTrabalho = function () {
        var retorno = '';
        var periodos = [];
        var indice = 0;
        sessionStorage.numeroPeriodos = 0;

        if (store.curriculo.periodoManha) {
            periodos[indice] = 'manhã';
            indice++;

            if (store.periodosLivres != '' && store.periodosLivres !== undefined)
                var obj = store.periodosLivres.filter(function (item) { return item.id == 'M' });
            obj[0].status = 'false';
        }
        else {
            var obj = store.periodosLivres.filter(function (item) { return item.id == 'M' });
            obj[0].status = 'true';
        }
        if (store.curriculo.periodoTarde) {
            periodos[indice] = 'tarde';
            indice++;

            var obj = store.periodosLivres.filter(function (item) { return item.id == 'T' });
            obj[0].status = 'false';
        }
        else {
            var obj = store.periodosLivres.filter(function (item) { return item.id == 'T' });
            obj[0].status = 'true';
        }
        if (store.curriculo.periodoNoite) {
            periodos[indice] = 'noite';
            indice++;

            var obj = store.periodosLivres.filter(function (item) { return item.id == 'N' });
            obj[0].status = 'false';

        }
        else {
            var obj = store.periodosLivres.filter(function (item) { return item.id == 'N' });
            obj[0].status = 'true';
        }
        if (store.curriculo.periodoFimdeSemana) {
            periodos[indice] = 'final de semana';
            indice++;

            var obj = store.periodosLivres.filter(function (item) { return item.id == 'F' });
            obj[0].status = 'false';
        } else {
            var obj = store.periodosLivres.filter(function (item) { return item.id == 'F' });
            obj[0].status = 'true';
        }

        sessionStorage.numeroPeriodos = indice;
        store.numeroPeriodos = indice;
        
        //salvar periodos na session
        sessionStorage.periodosLivres = angular.toJson(store.periodosLivres);
        store.periodosLivres = angular.fromJson(sessionStorage.periodosLivres)

        var retorno = "";

        if (periodos.length >= 2) {
            retorno = periodos.slice(0, (periodos.length - 1)).join(', ');
            retorno += ' e ' + periodos[periodos.length - 1];
        } else {
            if(periodos.length == 1)
                retorno = periodos[0];
        }
       

        store.curriculo.periodosTrabalhar = retorno;
    };

    $scope.abrirEditarPeriodos = function(CV) {

        if (store.numeroPeriodos < 4) {
            $('#addPeriodo_' + CV).show();
            $('#liEditPeriodos_' + CV).attr("style", "background-color: #ccc;");
        }
    }

    $scope.fecharEditarPeriodos = function(CV) {
        $('#liEditPeriodos_' + CV).attr("style", "background-color: #fff;");
        $('#addPeriodo_' + CV).hide();
    }

    //exibir deficiência se tiver
    $scope.ExibirDeficiencia = function () {

        var objDeficiencia = $filter('filter')(store.deficiencias, { id: store.curriculo.IdDeficiencia });

        if (typeof objDeficiencia !== 'undefined' && objDeficiencia.length > 0)
            return 'Possui deficiência' + objDeficiencia[0].text;
        else
            return '';
    }

    //angular x-editable
    $scope.ValNomeCompleto = function ($data) {
        var retorno = ValidarNome($data);

        return retorno ? true : 'Preencha o seu nome completo.';
    }

    $scope.ValPretensaoSalarial = function ($data) {
        var valorSalarioMinimo = sessionStorage.getItem('salarioMinimo');
        var retorno = ValidarPretensaoMaiorqueSalarioMinimo($data, valorSalarioMinimo);

        return retorno ? true : 'Sua pretensão deve ser maior que o salário mínimo (R$' + valorSalarioMinimo + ')';
    }

    //angular x-editable
    $scope.ValCidade = function ($data) {
        var retorno = $scope.ValidarCidade($data);
        return retorno ? true : 'Cidade inválida.';
    }

    //angular x-editable
    $scope.ValCelular = function ($data) {
        var retorno = ValidarCelular($data);

        return retorno ? true : 'Número de celular inválido.';
    }

    //angular x-editable
    $scope.ValTelefone = function ($data) {
        var retorno = ValidarTelefone($data);

        return retorno ? true : 'Número de telefone inválido.';
    }

    $scope.pegarEstadoCivil = function () {

        var objEstadoCivil = store.estadoCivil.filter(function (item) { return item.id == store.curriculo.idEstadoCivil });
        return objEstadoCivil[0].text;
    };

    $scope.checarNivelIdioma = function (idIdioma, nivel) {
        var obj = store.idiomasCandidato.filter(function (item) { return item.idIdioma == idIdioma });
        return obj[0].nivel == nivel;
    }

    $scope.setarNivelIdioma = function (idIdioma,nivel) {
        var obj = store.idiomasCandidato.filter(function (item) { return item.idIdioma == idIdioma });

        obj[0].nivel = nivel;
    }

    $scope.inserirnaGridIdiomas = function () {

        if (store.idiomasCandidato == null) {
            store.idiomasCandidato = [];
        } else {
            var objChecar = store.idiomasCandidato.filter(function (item) { return item.idIdioma == store.curriculo.IdIdioma });
        }

        if (objChecar === undefined || objChecar[0] == undefined) {

            var objInserir = store.idiomas.filter(function (item) { return item.idIdioma == store.curriculo.IdIdioma });

            if (objInserir[0].text != null) {
                objInserir[0].idIdiomaCandidato =0,
                objInserir[0].nivel = store.curriculo.NivelIdioma;
                objInserir[0].nivelTexto = $scope.PegarNivelTexto(store.curriculo.NivelIdioma);

                store.idiomasCandidato.push(objInserir[0]);
            }
        }

        store.curriculo.IdIdioma = null;
        $scope.mostrarIdiomas = false;
    };

    $scope.PegarNivelTexto = function (idNivel) {

        switch(idNivel)
        {
            case "1":
                return "Básico";
            case "2":
                return "Intermediário";
            case "3":
                return "Avançado";
            case "4":
                return "Fluente";
        }
    }

    $scope.removerIdioma = function (Idioma) {
        store.idiomasCandidato.splice(store.idiomasCandidato.indexOf(Idioma), 1);

        if (Idioma.idIdiomaCandidato > 0) {
            $scope.removerIdiomaBase(Idioma.idIdiomaCandidato);
        }
    }

    $scope.removerIdiomaBase = function (idIdiomaCandidato) {
            $http.post(urlAPI + "Curriculo/DeletarIdioma?idIdioma=" + idIdiomaCandidato)
                .success(function (data, status, headers, config) {
                })
                .error(function (data, status, headers, config) {
                });
    }

    $scope.atualizarGridIdiomas = function (idIdiomaRemover) {
        var obj = store.curriculo.idiomasCandidato.filter(function (item) { return item.idIdioma == idIdiomaRemover });
        var nivel = obj[0].nivel;
        var idIdioma = store.curriculo.IdIdioma;

        $scope.removerIdioma(obj[0]);
        $scope.inserirnaGridIdiomas();
        $scope.setarNivelIdioma(idIdioma, nivel);
    }

    $scope.abrirAddIdioma = function (CV,id) {

            $('#addIdioma_' + CV + id).show();
            $('#btnRemoverIdioma_' + CV + id).show();
            $('.addIdioma_' + id).attr("style", "background-color: #ccc;");
    }

    $scope.fecharAddIdioma = function (CV,id) {
        $('#addIdioma_' + CV + id).hide();
        $('#btnRemoverIdioma_' + CV + id).hide();
        $('.addIdioma_' + id).attr("style", "background-color: #fff;");
    }

    $scope.TemDeficiencia = function () {

        $scope.isDeficiencia = true;
        //store.curriculo.temDeficiencia = true;
    };

    $scope.NaoTemDeficiencia = function () {
        $scope.isDeficiencia = false;
        store.curriculo.idDeficiencia = null;
        //store.curriculo.temDeficiencia = false;
    };

    $scope.init = function () {
        $('.grid').animate({
            top: '+=96'
        }, 500);
    };

    $scope.init3 = function () {
        $('.gridPasso3').animate({
            top: '+=84'
        }, 500);
    };

    $scope.init4 = function () {
        $('.gridPasso4').animate({
            top: '+=91'
        }, 500);
    };

    $scope.salvarCadastroPasso1 = function () {

        $scope.salvandoPasso1 = true;
        store.curriculo.idOrigem = localStorage.getItem('origem');

        $scope.persistirCVnaSession();

        $http.post(urlAPI + "Curriculo/SalvarCV?passoCadastro=1", sessionStorage.curriculo)
            .success(function (data, status, headers, config) {
                $('#upPasso1').html('Sucesso!');
                sessionStorage.curriculo = angular.toJson(data);
                //store.curriculo = sessionStorage.curriculo;
            })
            .error(function (data, status, headers, config) {
                $('#upPasso1').html('Erro no salvar!');
            });

        $location.path('CadastroPasso2');
    };

    $scope.salvarCadastroPasso2 = function () {

        //define se tem expe. profisisonal.
        store.curriculo.temExperienciaProfissional = $scope.temExperienciaProfissional;

        if (store.curriculo.idPessoaFisica == 0 || store.curriculo.idPessoaFisica === undefined) {
            var cv = angular.fromJson(sessionStorage.curriculo)
            store.curriculo.idPessoaFisica = cv.idPessoaFisica;
            store.curriculo.idCurriculo = cv.idCurriculo;
            store.curriculo.qtdCandidatura = cv.qtdCandidatura;

        }

        $scope.persistirCVnaSession();

        $http.post(urlAPI + "Curriculo/SalvarCV?passoCadastro=2", sessionStorage.curriculo)
            .success(function (data, status, headers, config) {

                sessionStorage.curriculo = angular.toJson(data);
                store.curriculo = sessionStorage.curriculo;
                sessionStorage.Experiencias = angular.toJson(data.experiencias);

                $location.path('CadastroPasso3');
            })
            .error(function (data, status, headers, config) {
                console.log('erro passo 2 ');
            });
    };

    $scope.salvarCadastroPasso3 = function () {
        
        $scope.persistirCVnaSession();

        //persistir dados na sessionStorage
        sessionStorage.Formacoes = angular.toJson(store.formacoes);
        sessionStorage.Cursos = angular.toJson(store.cursos);
        sessionStorage.idiomasCandidato = angular.toJson(store.idiomasCandidato);

        store.curriculo.formacoes = angular.fromJson(store.formacoes);
        store.curriculo.cursos = angular.fromJson(store.cursos);
        store.curriculo.idiomasCandidato = angular.fromJson(store.idiomasCandidato);

        $http.post(urlAPI + "Curriculo/SalvarCV?passoCadastro=3", store.curriculo)
            .success(function (data, status, headers, config) {
                sessionStorage.curriculo = angular.toJson(data);
                store.curriculo = sessionStorage.curriculo;
            })
            .error(function (data, status, headers, config) {
                console.log(data);
            });

        $location.path('CadastroPasso4');
    };

    $scope.salvarCadastroPasso4 = function () {

        $scope.mostrarPeriodosTrabalho();

        store.curriculo.imgCandidato = $scope.imgCandidato;
        sessionStorage.imgCandidato = $scope.imgCandidato;

        $scope.persistirCVnaSession();

        $http.post(urlAPI + "Curriculo/SalvarCV?passoCadastro=4", store.curriculo)
            .success(function (data, status, headers, config) {
                store.curriculo = sessionStorage.curriculo;
            })
            .error(function (data, status, headers, config) {
            });

        $location.path('EscolherModeloCurriculo');
    };

    $scope.salvarCurriculo = function (divMensagem) {

        $('#' + divMensagem + 'SalvarSucesso').html('<img src="images/gif-load.gif" /> Salvando alterações...');
        $('#' + divMensagem + 'SalvarSucesso').removeClass('inativo');
        $('#' + divMensagem + 'SalvarSucesso').addClass('alert-info');

        $('#' + divMensagem + 'Erro').addClass('inativo');

        $scope.persistirCVnaSession();

        store.curriculo.salvarTudo = true;

        $http.post(urlAPI + "Curriculo/SalvarCV?passoCadastro=0", store.curriculo) //Todos os dados do CV
            .success(function (data, status, headers, config) {
                $('#' + divMensagem + 'SalvarSucesso').removeClass('alert-info');
                $('#' + divMensagem + 'SalvarSucesso').addClass('alert-success');
                $('#' + divMensagem + 'SalvarSucesso').html('Salvo com sucesso!');

                setTimeout("$('#divCVEleganteSalvarSucesso').hide()", 5000);
                setTimeout("$('#divCVModernoSalvarSucesso').hide()", 5000);
                setTimeout("$('#divCVBNESalvarSucesso').hide()", 5000);

            })
            .error(function (data, status, headers, config) {
                $('#divSalvarErro').removeClass('inativo');
                $('#divSalvarSucesso').addClass('inativo');
            });

    }

    $scope.persistirCVnaSession = function () {

        sessionStorage.curriculo = angular.toJson(store.curriculo);
        //sessionStorage.Formacoes = angular.toJson(store.formacoes);
        //sessionStorage.Cursos = angular.toJson(store.cursos);
    }

    $scope.addImage = function () {

        var f = document.getElementById('file').files[0];
        var fd = new FormData();
        fd.append('file', f);

        if (f != null) {

            $('#divProcessandoEnvio').removeClass('inativo');
            $('#pnBotoesImagemPC').hide();

            $http.post(urlAPI + 'ImageUpload', fd, {
                transformRequest: angular.identity, headers: { 'Content-Type': undefined }
            })
            .success(function (data) {

                $scope.imgCandidato = 'images/candidatos/' + data;
                $scope.disablebtnImg = true;
                store.curriculo.alterouImagem = true;
            })
            .error(function (data) {
                if (data == '406') {
                    alert('Formato de arquivo inválido, por favor selecione uma imagem JPG ou PNG')
                }

                console.log('error =>');
            }).finally(function () {
                $('#divProcessandoEnvio').addClass('inativo');
                $('#pnBotoesImagemPC').show();
            });
        } else {
            alert('Selecione um arquivo!');
        }
    }

    $scope.delImage = function () {
        $scope.disablebtnImg = false;
        $scope.imgCandidato = '';
    }

    $scope.go = function (path) {
       $location.path(path);
    };

    $scope.fecharModal = function (path) {

        //fechar o modal
        //$('#CvTradicionalModal').modal('toggle');

        //redirecionar para a rota
        $scope.go(path);
    }
    
    //Habilita campos para adicionar o penultimo emprego.
    $scope.adicionarPenultimoEmprego = function () {
        $scope.addPenultimoEmprego = true;

        store.curriculo.MesInicioPE = 1;
        store.curriculo.MesSaidaPE = 1;
        //$scope.gotoPenultimoEmprego();
    }

    //Habilitar campos para adicionar a terceira experiência
    $scope.adicionarOutroEmprego = function () {
        $scope.addOutroEmprego = true;
        store.curriculo.MesInicio3 = 1;
        store.curriculo.MesSaida3 = 1;
    }

    $scope.RemoverExperienciadaBase = function (idExperiencia) {
        $http.post(urlAPI + "Curriculo/DeletarExperiencia?idExperiencia=" + idExperiencia)
            .success(function (data, status, headers, config) {
            })
            .error(function (data, status, headers, config) {
            });
    }

    //Desabilitar campos do penultimo emprego
    $scope.RemoverPenultimoEmprego = function () {

        //remover da base se estiver persistido.
        if(store.curriculo.idExperienciape > 0)
        {
            $scope.RemoverExperienciadaBase(store.curriculo.idExperienciape);

            $scope.addPenultimoEmprego = false;
            store.curriculo.EmpresaPE = '';
        }
    }

    //Desabilitar campos do penultimo emprego
    $scope.RemoverOutroEmprego = function () {

        //remover da base se estiver persistido.
        if (store.curriculo.idExperiencia3 > 0) {
            $scope.RemoverExperienciadaBase(store.curriculo.idExperiencia3);

            $scope.addOutroEmprego = false;
            store.curriculo.Empresa3 = '';
        }
    }

    $scope.gotoPenultimoEmprego = function () {

        $location.hash('pnPenultimoEmprego');

        $anchorScroll();
    }

    $scope.eEmpregoAtual = function () {
        $scope.isEmpregoAtual = true;
        store.curriculo.anoSaida = null;
        store.curriculo.mesSaida = null;
    }

    $scope.naoeEmpregoAtual = function () {
        $scope.isEmpregoAtual = false;
    }

    $scope.AddEspecializacaoPasso3 = function () {

        if (store.curriculo.IdNivelEspecializacao != null) {

            var objFormacao = {
                idFormacao: 0,
                id: store.curriculo.IdNivelEspecializacao,
                idEscolaridade: store.curriculo.IdNivelEspecializacao,
                nivel: $scope.getNivelEspecializacao(),
                grau: 4,
                curso: store.curriculo.cursoEspecializacao,
                instituicao: store.curriculo.instituicaoEspecializacao,
                anoConclusao: store.curriculo.anoConclusaoEspecializacao,
                cidadeFormacao: store.curriculo.cidadeFormacaoEspecializacao
            }

            if (store.formacoes == null) {
                store.formacoes = [];
                store.formacoes.push(objFormacao);
            } else {
                store.formacoes.push(objFormacao);
            }

            store.curriculo.IdNivelEspecializacao = '';
            store.curriculo.cursoEspecializacao = '';
            store.curriculo.instituicaoEspecializacao = '';
            store.curriculo.anoConclusaoEspecializacao = '';
            store.curriculo.cidadeFormacaoEspecializacao = '';

            $scope.mostrarpnEspecializacao = false;
        } else {
            alert('Selecione o nível da Especialização!');
        }
    }

    //adicionar formação na lista de formações
    $scope.AddFormacaoPasso3 = function () {

        if (store.curriculo.IdNivelFormacao != null) {

            $scope.informouFormacao = true;

            //$scope.mostrarEspecializacao = $scope.isFormacaoCompleta;

            var objFormacao = {
                idFormacao: 0,
                id: store.curriculo.IdNivelFormacao,
                idEscolaridade: store.curriculo.IdNivelFormacao,
                nivel: $scope.getNivelEscolaridade(),
                grau: store.Escolaridade1,
                curso: store.curriculo.curso,
                instituicao: store.curriculo.instituicao,
                anoConclusao: store.curriculo.anoConclusao,
                cidadeFormacao: store.curriculo.cidadeFormacao,
                situacao: $scope.getSituacaoEscolaridade()
            }

            if (store.formacoes == null) {
                store.formacoes = [];
                store.formacoes.push(objFormacao);
            } else {
                store.formacoes.push(objFormacao);
            }

            if (objFormacao.grau == 3 && (objFormacao.nivel == 'Superior Completo' || objFormacao.nivel == 'Tecnólogo Completo'))
            $scope.mostrarEspecializacao = true;

            //limpar campos escolaridade
            $scope.formPasso3.$setPristine();

            store.curriculo.IdNivelFormacao = '';
            store.curriculo.curso = '';
            store.curriculo.instituicao = '';
            store.curriculo.anoConclusao = '';
            store.curriculo.cidadeFormacao = '';
            store.curriculo.idSituacaoFormacao = '';

            $scope.isSelecionouEscolaridade = false;
            $scope.is3Grau = false;
            $scope.isFormacaoCompleta = false;
        }
    }

    //retorna o nome do nível da escolaridade
    $scope.getNivelEscolaridade = function () {
        var nivel = '';
        var objChecar = store.escolaridade.filter(function (item) { return item.id == store.curriculo.IdNivelFormacao });

        if (objChecar[0] != undefined) {
            var objInserir = store.escolaridade.filter(function (item) { return item.id == store.curriculo.IdNivelFormacao });

            if (objInserir[0].text != null)
            nivel = objInserir[0].text;
        }

        return nivel;
    }

    //retorna o nome do nível da especializacao
    $scope.getNivelEspecializacao = function () {
        var nivel = '';
        var objChecar = store.especializacao.filter(function (item) { return item.id == store.curriculo.IdNivelEspecializacao });

        if (objChecar[0] != undefined) {
            var objInserir = store.especializacao.filter(function (item) { return item.id == store.curriculo.IdNivelEspecializacao });

            if (objInserir[0].text != null)
                nivel = objInserir[0].text;
        }

        return nivel;
    }

    //retorna o id do grau da escolaridade
    $scope.getGrauEscolaridade = function () {
        var grau = '';
        var objChecar = store.escolaridade.filter(function (item) { return item.id == store.curriculo.IdNivelFormacao });

        if (objChecar[0] != undefined) {
            var objInserir = store.escolaridade.filter(function (item) { return item.id == store.curriculo.IdNivelFormacao });

            if (objInserir[0].text != null)
                nivel = objInserir[0].Idf_Grau_Escolaridade;
        }

        return grau;
    }

    //retorna o texto da situação da escolaridade
    $scope.getSituacaoEscolaridade = function () {
        var nivel = '';
        var objChecar = store.SituacaoFormacao.filter(function (item) { return item.id == store.curriculo.IdSituacaoFormacao });

        if (objChecar[0] != undefined) {
            var objInserir = store.SituacaoFormacao.filter(function (item) { return item.id == store.curriculo.IdSituacaoFormacao });

            if (objInserir[0].text != null)
                nivel = objInserir[0].text;
        }

        return nivel;
    }

    $scope.removerFormacao = function (formacao) {
        store.formacoes.splice(store.formacoes.indexOf(formacao), 1);

        if (formacao.idFormacao > 0) {
            $scope.RemoverFormacaoBase(formacao.idFormacao);
        }

        if (store.formacoes.length == 0) {
            $scope.informouFormacao = false;
        }
    }

    $scope.RemoverFormacaoBase = function (idFormacao) {
        $http.post(urlAPI + "Curriculo/DeletarFormacao?idFormacao=" + idFormacao)
            .success(function (data, status, headers, config) {
            })
            .error(function (data, status, headers, config) {
            });
    }

    $scope.removerCurso = function (curso) {
        store.cursos.splice(store.cursos.indexOf(curso), 1);

        if (curso.idFormacao > 0) {
            $scope.RemoverFormacaoBase(curso.idFormacao);
        }
    }

    //Habilita campos para adicionar outra formação acadêmica.
    $scope.adicionarOutraFormacao = function () {

        //checar escolaridade
        $scope.checarEscolaridade();
        $scope.addOutraFormacao = true;
    }

    $scope.checarEscolaridade = function () {

        if (store.curriculo.IdNivelFormacao != null) {
            $http.get(urlAPI + 'Escolaridade3Grau/' + store.curriculo.IdNivelFormacao)
            .success(function (data) {
                //checa se é 3º grau completo, se sim carrega as especializações
                if (data == 'True') {
                    $scope.isFormacaoCompleta = true;
                    $scope.AjustarCamposEscolaridade();
                } else {
                    //store.escolaridadeMaisEspecializacao = store.escolaridade;
                    $scope.isFormacaoCompleta = false;
                    $scope.AjustarCamposEscolaridade();
                }
            })
            .error(function (data) {
                console.log(data);
            });
        } else {
            store.escolaridadeMaisEspecializacao = store.escolaridade;
            $scope.isFormacaoCompleta = false;
            $scope.AjustarCamposEscolaridade();
        }
    }

    $scope.removerOutraFormacao = function () {
        $scope.addOutraFormacao = false;
        $scope.limparCamposFormacao2();
    }

    $scope.limparCamposFormacao2 = function () {

        store.curriculo.IdNivelFormacao2 = null;
        store.curriculo.instituicao2 = null;
        store.curriculo.CidadeFormacao2 = null;
        store.curriculo.AnoConclusao2 = null;
        store.curriculo.curso2 = null;
    }

    $scope.AjustarCamposEscolaridade = function () {

        $http.get(urlAPI + 'EscolaridadeGrau/' + store.curriculo.IdNivelFormacao)
            .success(function (data) {
                store.Escolaridade1 = data;

                $scope.isSelecionouEscolaridade = true;

                if (store.Escolaridade1 >= 3) {
                    $scope.is3Grau = true;
                } else {
                    $scope.is3Grau = false;
                }
            })
            .error(function (data) {
                console.log(data);
            });
    }

    //fitrar as escolaridades
    $scope.filtrarEscolaridade = function (location) {
        if (location !== undefined && location != null)
        return location.grau <= 3;
    }

    //fitrar as especializações
    $scope.filtrarEspecializacao = function (location) {
        if (location !== undefined && location != null)
            return location.grau == 4;
    }

    $scope.temExperiencia = function () {
        $scope.temExperienciaProfissional = true;
        store.curriculo.temExperienciaProfissional = true;
    }

    $scope.naoTemExperiencia = function () {
        $scope.temExperienciaProfissional = false;
        store.curriculo.temExperienciaProfissional = false;
        $scope.salvarCadastroPasso2();
    }

    $scope.checarFormacao = function (id) {

        store.curriculo.IdNivelFormacao = id;

        $scope.checarEscolaridade();
        $scope.isSelecionouEscolaridade = true;
    };

    $scope.selecionarFormacao = function () {
        $scope.checarEscolaridade();
    };

    $scope.validarAno = function (ano) {
        $rootScope.anoinvalido = ValidarAno(ano);
    };

    $scope.adicionarCurso = function () {
        
        if (store.curriculo.cursoInstituicao  != null) {
            var objCurso = {
                idFormacao: 0,
                id: 0,
                nivel: 'Curso Complementar',
                curso: store.curriculo.cursoNome,
                instituicao: store.curriculo.cursoInstituicao,
                anoConclusao: store.curriculo.cursoAnoConclusao,
                cidadeFormacao: store.curriculo.cursoCidadeFormacao,
                cargaHoraria: store.curriculo.cursoCargaHoraria
            }

            if (store.cursos == null) {
                store.cursos = [];
                store.cursos.push(objCurso);
            } else {
                store.cursos.push(objCurso);
            }

            //limpar campos escolaridade
            $scope.formPasso3.$setPristine();

            store.curriculo.cursoNome = '';
            store.curriculo.cursoInstituicao = '';
            store.curriculo.cursoAnoConclusao = '';
            store.curriculo.cursoCidadeFormacao = '';
            store.curriculo.cursoCargaHoraria = '';

            $scope.mostrarCursos = false;
        }
    }

    $scope.exibirPanelAddCursos = function () {
        $scope.mostrarCursos = true;
    }

    $scope.exibirPanelAddIdiomas = function () {
        $scope.mostrarIdiomas = true;
    }

    $scope.exibirPanelAddEspecializacao = function () {
        $scope.mostrarpnEspecializacao = true;
    }

    $scope.checarFormacao2 = function () {

        if (store.curriculo.IdNivelFormacao2 > 3 && store.curriculo.IdNivelFormacao2 < 8) {
            $scope.isFormacao2Completa = false;
        } else {
            $scope.isFormacao2Completa = true;
        }
    };

    $scope.setarModeloCurriculoEnviarEmail = function (modelo) {
        store.modeloCV = modelo;

        $('.alert-success').hide();
        $('.alert-warning').hide();
    }

    $scope.salvarPDFCurriculo = function (modelo) {

        var html = $('#' + modelo).html();
        $http.post(urlAPI + "HtmltoPdf/Post", JSON.stringify({modelo:modelo, html:html}), {responseType:'arraybuffer'})
            .success(function (data) {
                var file = new Blob([data], { type: 'application/pdf' });
                var fileURL = window.URL.createObjectURL(file);
                
                window.open(fileURL);
            });
    }

    $scope.imprimirCurriculo = function (modelo) {

        $('.divAcoesMenu').addClass('inativo');
        $('.divImprimirCV').removeClass('inativo');

        var html = $('#' + modelo).html().replace('<i class="fa fa-envelope-o"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/email.jpg" />').replace('<i class="fa fa-user"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/user.jpg" />').replace('<i class="fa fa-home"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/cidade.jpg" />').replace('<i class="fa fa-phone"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/fone.jpg" />');
        $http.post(urlAPI + "HtmltoPdf/Post", JSON.stringify({ modelo: modelo, html: html, candidato:store.curriculo.idPessoaFisica }), { responseType: 'arraybuffer' })
            .success(function (data) {
                var file = new Blob([data], { type: 'application/pdf' });
                var fileURL = window.URL.createObjectURL(file);

                window.open(fileURL);

                $('.divAcoesMenu').removeClass('inativo');
                $('.divImprimirCV').addClass('inativo');


            }).error(function () {
                $('.divAcoesMenu').removeClass('inativo');
                $('.divImprimirCV').addClass('inativo');

            });
    }

    $scope.enviarCurriculoEmail = function () {

        $('#divEnviando').show();
        $('.btn-email').hide();
        $('.alert-success').hide();
        $('.alert-warning').hide();

        var html = $('#' + store.modeloCV).html().replace('<i class="fa fa-envelope-o"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/email.jpg" />').replace('<i class="fa fa-user"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/user.jpg" />').replace('<i class="fa fa-home"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/cidade.jpg" />').replace('<i class="fa fa-phone"></i>', '<img src="http://teste.lanhouse.bne.com.br/images/fone.jpg" />');;
        var nome = store.curriculo.nome;
        var email = $('#txtEmailDestinatario').val();
        var assunto = $('#txtAssunto').val();
        var mensagem = $('#txtMensagem').val();

        $http.post(urlAPI + "EnviarCVEmail/Post", JSON.stringify({ email: email, nome: nome, assunto: assunto, mensagem: mensagem, modelo: store.modeloCV, html: html, candidato: store.curriculo.idPessoaFisica }))
            .success(function (data) {
                $('#divEnviando').hide();
                $('.btn-email').show();
                $('.alert-success').show();
                $('.alert-warning').hide();
            })
        .error(function (data) {
            $('#divEnviando').hide();
            $('.btn-email').show();
            $('.alert-success').hide();
            $('.alert-warning').show();
        });
    }

    $scope.mostrarMediaSalarioFuncao = function () {
        $('#avisoMediaSalario').show();
    }

    $scope.ocultarMediaSalarioFuncao = function () {
        $('#avisoMediaSalario').hide();
    }

    $scope.mostrarAvisoObservacao = function () {
        $('#avisoObservacao').show();
    }

    $scope.ocultarAvisoObservacao = function () {
        $('#avisoObservacao').hide();
    }

    //WebCam
    $scope.exibirWebCam = function () {
        $scope.exibirwebcam = true;
    }

    $scope.ocultarWebCam = function () {
        $scope.exibirwebcam = false;
    }

    var _video = null,
        patData = null;

    $scope.showDemos = false;
    $scope.edgeDetection = false;
    $scope.mono = false;
    $scope.invert = false;

    $scope.patOpts = { x: 0, y: 0, w: 25, h: 25 };

    $scope.webcamError = false;
    $scope.onError = function (err) {
        $scope.$apply(
            function () {
                $scope.webcamError = err;
            }
        );
    };

    $scope.onSuccess = function (videoElem) {
        // The video element contains the captured camera data
        //_video = videoElem;
        _video = $('body').find('video')[0];
        $scope.$apply(function () {
            $scope.patOpts.w = _video.width;
            $scope.patOpts.h = _video.height;
            $scope.showDemos = true;
        });
    };

    $scope.onStream = function (stream, videoElem) {
        // You could do something manually with the stream.
    };

    $scope.tirarFotoWebCam = function () {
        var patCanvas = document.querySelector('#snapshot');
        if (!patCanvas) return;

        patCanvas.width = _video.width;
        patCanvas.height = _video.height;
        var ctxPat = patCanvas.getContext('2d');

        var idata = getVideoData($scope.patOpts.x, $scope.patOpts.y, $scope.patOpts.w, $scope.patOpts.h);
        ctxPat.putImageData(idata, 0, 0);

        $scope.snapshotData = '';
        sendSnapshotToServer(patCanvas.toDataURL());
        //sendSnapshotToServer(idata);

        patData = idata;

        $scope.tiroufoto = true;
    }

    var getVideoData = function getVideoData(x, y, w, h) {
        var hiddenCanvas = document.createElement('canvas');
        hiddenCanvas.width = _video.width;
        hiddenCanvas.height = _video.height;
        var ctx = hiddenCanvas.getContext('2d');
        ctx.drawImage(_video, 0, 0, _video.width, _video.height);
        return ctx.getImageData(x, y, w, h);
    };

    var sendSnapshotToServer = function sendSnapshotToServer(imgBase64) {
        $scope.snapshotData = imgBase64;
    };

    $scope.addImageByWebCam = function () {

        var blob = null;
        blob = dataURItoBlob($scope.snapshotData);

        var dateNow = new Date();
        var ano = dateNow.getFullYear();
        var mes = dateNow.getMonth() +1;
        var dia = dateNow.getUTCDate();
        var hora = dateNow.getHours();
        var minuto = dateNow.getMinutes();
        var segundos = dateNow.getSeconds();

        var registroFoto = ano + '-' + mes + '-' + dia + '-' + hora + '-' + minuto + '-' + segundos;

        var fd = new FormData();
        fd.append('blog', blob, 'candidato_foto_' + registroFoto + '.png');

        $http.post(urlAPI + 'ImageUpload', fd, {
            transformRequest: angular.identity, headers: { 'Content-Type': undefined }
        })
        .success(function (data) {

            $scope.exibirwebcam = false;
            $scope.tiroufoto = false;
            $scope.imgCandidato = 'images/candidatos/' + data;
            store.curriculo.imgCandidato = 'images/candidatos/' + data;
            store.curriculo.alterouImagem = true;

            $scope.disablebtnImg = true;
            
        })
        .error(function (data) {
            console.log('error =>');
        });
    }

    $scope.naoGostei = function () {
        $scope.tiroufoto = false;
    }

    $scope.abrirCombo = function () {

        if (!$scope.informouFormacao) {
            setTimeout(function () {
                $('#ddlNivel').simulate('mousedown');
            }, 1000);
        }
    }

    $scope.hideMsg = function (element) {
        $('#' + element + 'Erro').addClass('inativo');
        $('#' + element + 'SalvarSucesso').addClass('inativo');
    };
}]);

LanControllers.controller('HomeController', ['$scope', '$http', '$location', '$rootScope', 'authService', function ($scope, $http, $location, $rootScope, $authService) {
    $rootScope.isHome = true;
    $('#tags, #btnTags').show();

    $scope.logOut = function () {
        $authService.logOut();
        $rootScope.usuarioLogado = false;
        sessionStorage.curriculo = null;
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;
}]);

LanControllers.controller('ImprimirCVController', ['$scope', '$http', '$location', function ($scope, $http, $location) {

    var store = this;
    store.urlWeb = window.location.hostname;
    //store.curriculo = angular.fromJson(sessionStorage.curriculo);

    if (store.curriculo.empregoAtual == 'true') {
        store.curriculo.anoSaida = null;
        store.curriculo.mesSaida = null;
    }

    $scope.imprimirCurriculo = function (modelo) {

        var html = $('#' + modelo).html();
        $http.post(urlAPI + "HtmltoPdf/Post", JSON.stringify({ modelo: modelo, html: html }), { responseType: 'arraybuffer' })
            .success(function (data) {
                var file = new Blob([data], { type: 'application/pdf' });
                var fileURL = window.URL.createObjectURL(file);

                window.open(fileURL);
            });
    }
}]);