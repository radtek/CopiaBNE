var camposDirectives = angular.module('camposDirectives', []);

camposDirectives.directive('nomeCompleto', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/NomeCompleto.html',
        link: function (scope, element, attrs, model) {
            scope.nomeValido = true;
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            scope.eMaxl = attrs.eMaxl;
            scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;

            var _input = $(element.find('input'));
            _input.bind('blur', function () {
                scope.$apply(function () {
                    var retorno = ValidarNome($(_input).val());
                    scope.nomeValido = retorno;
                });
            });
        }
    }
});

camposDirectives.directive('ddd', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/DDD.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            scope.eMaxl = attrs.eMaxl;
            scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('celular', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/Celular.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            //scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('nascimento', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/DataNascimento.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            //scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('email', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/Email.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            //scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('cpf', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/CPF.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            //scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('cidade', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/Cidade.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            //scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('telefoneFixo', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/TelefoneFixo.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('sexo', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/Sexo.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('deficiencia', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/Deficiencia.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            scope.eNome = attrs.eNome;
        }
    }
});

camposDirectives.directive('pretensao', function ($parse) {
    return {
        restrict: 'E',
        required: ['^ngModel'],
        //scope: { value: '=ngModel' },
        templateUrl: 'views/templates/PretensaoSalarial.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired == null || attrs.eRequired === undefined) ? '' : 'required';
            scope.eNome = attrs.eNome;
        }
    }
});






camposDirectives.directive('buscarMedia', ['$http', '$rootScope', function ($http, $rootScope) {

    return {
        require: 'ngModel',
        controller: function ($scope) { },
        link: function (scope, ele, attrs) {

            if (attrs.buscarMedia !== undefined && attrs.buscarMedia != "") {
                var data = attrs.buscarMedia;
                data = angular.fromJson(data);

                var _input = $(ele.find('input'));

                _input.bind('blur', function () {
                    scope.$apply(function () {
                        var cidade = $('#' + data.campocidade).val();
                        var funcao = $('#' + data.campofuncao).val();

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
                    });
                });
            }
        }
    }
}]);

camposDirectives.directive('validarDatasExperienciaProfissional', [function ($scope) {
    return {
        require: 'ngModel',
        controller: function ($scope) { },
        link: function (scope, element, attrs, ctrl) {

            if (attrs.validarDatasExperienciaProfissional !== undefined && attrs.validarDatasExperienciaProfissional != "")
            {
                var data = attrs.validarDatasExperienciaProfissional;
                data = angular.fromJson(data);

                var _input = $(element.find('input'));

                _input.bind('blur', function () {
                    scope.$apply(function () {
                        ctrl.$setValidity(data.anoInicio, ValidarDataDemissao($('#' + data.mesInicio).val(), $('#' + data.anoInicio).val(), $('#' + data.mesSaida).val(), $('#' + data.anoSaida).val()));
                    });
                });
            }
        }
    }
}]);

camposDirectives.directive('validarCampoVazio', [function ($scope) {
    return {
        require: 'ngModel',
        controller: function ($scope) { },
        link: function (scope, element, attrs, ctrl) {

            if (attrs.validarCampoVazio !== undefined && attrs.validarCampoVazio != "") {

                var _input = $(element.find('input'));

                _input.bind('blur', function () {
                    scope.$apply(function () {
                        ctrl.$setValidity('curso', $(element.find('input')).val().length > 4);
                    });
                });
            }
        }
    }
}]);

camposDirectives.directive('carregarSugestaoAtividades', [function ($scope) {
    return {
        require: 'ngModel',
        controller: function ($scope) { },
        link: function (scope,element,attrs,model) {
            if (attrs.carregarSugestaoAtividades !== undefined && attrs.carregarSugestaoAtividades != "")
            {
                var data = attrs.carregarSugestaoAtividades;
                data = angular.fromJson(data);

                var _input = $(element.find('input'));

                _input.bind('blur', function () {
                    scope.$apply(function () {
                        scope.CarregarSugestaoAtividade(data.txtDestino, data.divSugestao, model.$viewValue);
                    });
                });
            }
        }
    }
}]);

camposDirectives.directive('inputGenerico', function ($parse) {
    return {
        restrict: 'E',
        require: ['^ngModel','?autocompleteCurso'],
        scope: {value: '=ngModel'},
        templateUrl: 'views/templates/CampoTextoGenerico.html',
        link: function (scope, element, attrs, model) {
            scope.eLabel = attrs.eLabel;
            scope.eRequired = (!attrs.eRequired ==null || attrs.eRequired === undefined)? '':'required';
            scope.eMaxl = attrs.eMaxl;
            //scope.eMinl = attrs.eMinl;
            scope.eNome = attrs.eNome;
            
            //element.on('focusout', function () {
            //        model.$setViewValue($(element.find('input')).val());
            //});
        }
    }
});

camposDirectives.directive('textareaGenerico', function ($parse) {
    return {
        restrict: 'E',
        require: '^ngModel',
        scope: { value: '=ngModel', funcao: '='},
        templateUrl: 'views/templates/textarea/textareaGenerico.html',
        link: function (scope, element, attrs, model) {
            scope.mostrarSugestao = true;
            scope.label = attrs.label;
            scope.required = (attrs.required == null || attrs.required === undefined) ? false : true;
            scope.maxl = attrs.maxl;
            scope.nome = attrs.nome;
            scope.destino = attrs.destino;
        }
    }
});

camposDirectives.directive('funcaoPretendida', function () {
    return {
        restrict: 'E',
        require: ['^ngModel', '?carregarSugestaoAtividades','?buscarMedia'],
        scope: { value: '=ngModel'},
        templateUrl: 'views/templates/FuncaoPretendida.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.obrigatorio = (attrs.required === undefined)?'':'required';
            scope.maxl = attrs.maxl;
            scope.minl = attrs.minl;
            scope.nome = attrs.nome;
        }
    }
});




camposDirectives.directive('dropdownGenerico', function ($parse,$http) {
    return {
        restrict: 'EA',
        require: '^ngModel',
        controller: function ($scope) { },
        scope: { array: '=', value: '=ngModel', nivelChange: '&',initCombo: '&' },
        templateUrl: 'views/templates/select/DropDownGenerico.html',
        link: function (scope, element, attrs, model) {
            scope.optValue = attrs.optValue;
            scope.optDescription = attrs.optDescription;
            scope.label = attrs.label;
            scope.nome = attrs.nome;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.initCombo();

            scope.orderby = attrs.orderby;


            var _select = $(element.find('select'));
            _select.bind('change', function () {
                scope.$apply(function () {
                    scope.nivelChange();
                    
                });
            });
        }
    }
});

camposDirectives.directive('dropdownIdioma', function ($parse, $http) {
    return {
        restrict: 'EA',
        require: '^ngModel',
        controller: function ($scope) { },
        scope: { array: '=', value: '=ngModel', nivelChange: '&', initCombo: '&' },
        templateUrl: 'views/templates/select/DropDownIdioma.html',
        link: function (scope, element, attrs, model) {
            scope.optValue = attrs.optValue;
            scope.optDescription = attrs.optDescription;
            scope.label = attrs.label;
            scope.nome = attrs.nome;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.initCombo();

            var _select = $(element.find('select'));
            _select.bind('change', function () {
                scope.$apply(function () {
                    //scope.nivelChange();

                });
            });
        }
    }
});

camposDirectives.directive('anoInicio', function ($parse) {
    return {
        restrict: 'E',
        require: ['^ngModel', '?validarDatasExperienciaProfissional'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/AnoInicio.html',
        link: function (scope, element, attrs, model) {

            if (attrs.validarDatasExperienciaProfissional !== undefined && attrs.validarDatasExperienciaProfissional != "") {
                var data = attrs.validarDatasExperienciaProfissional;
                data = angular.fromJson(data);
                scope.form = data.form;
                scope.anoInicio = data.anoInicio;
            }

            scope.label = attrs.label;
            scope.required = (attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.nome = attrs.nome;
            
        }
    }
});

camposDirectives.directive('anoSaida', function ($parse) {
    return {
        restrict: 'E',
        require: '^ngModel',
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/AnoSaida.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (attrs.required == null || attrs.required === undefined) ? '' : 'required';

            scope.maxl = attrs.maxl;
            scope.nome = attrs.nome;
            console.info('required=>', scope.required);

            //element.on('focusout', function () {
            //    model.$setViewValue($(element.find('input')).val());
            //});
        }
    }
});

camposDirectives.directive('anoConclusao', function ($parse) {
    return {
        restrict: 'E',
        require: '^ngModel',
        scope: { value: '=ngModel', anoValidar: '&' },
        templateUrl: 'views/templates/AnoConclusao.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.nome = attrs.nome;
            scope.mask = attrs.mask;
            scope.obrigatorio = attrs.obrigatorio;

            var _input = $(element.find('input'));
            _input.bind('focusout', function () {
                scope.$apply(function () {
                    scope.retorno = ValidarAno(scope.value);
                });
            });
        }
    }
});

camposDirectives.directive('cursoFormacao', function ($parse) {
    return {
        restrict: 'E',
        require: ['^ngModel', '?validarCampoVazio'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/CursoFormacao.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.minl = attrs.minl;
            scope.nome = attrs.nome;
            scope.obrigatorio = attrs.obrigatorio;

            //var _input = $(element.find('input'));
            //_input.bind('blur', function () {
            //    scope.$apply(function () {
            //        model.$setValidity('validCurso', $(element.find('input')).val().length > 4);
            //    });
            //});
        }
    }
});

camposDirectives.directive('cursoComplementar', function ($parse) {
    return {
        restrict: 'E',
        require: ['^ngModel', '?validarCampoVazio'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/CursoComplementar.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.minl = attrs.minl;
            scope.nome = attrs.nome;
            scope.obrigatorio = attrs.obrigatorio;
        }
    }
});

camposDirectives.directive('instituicaoFormacao', function ($parse) {
    return {
        restrict: 'E',
        require: ['^ngModel', '?validarCampoVazio'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/InstituicaoFormacao.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.minl = attrs.minl;
            scope.nome = attrs.nome;
            scope.obrigatorio = attrs.obrigatorio;

            //var _input = $(element.find('input'));
            //_input.bind('blur', function () {
            //    scope.$apply(function () {
            //        model.$setValidity('validCurso', $(element.find('input')).val().length > 4);
            //    });
            //});
        }
    }
});

camposDirectives.directive('instituicaoCursoComplementar', function ($parse) {
    return {
        restrict: 'E',
        require: ['^ngModel', '?validarCampoVazio'],
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/InstituicaoCursoComplementar.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.minl = attrs.minl;
            scope.nome = attrs.nome;
            scope.obrigatorio = attrs.obrigatorio;
        }
    }
});

camposDirectives.directive('cargaHoraria', function ($parse) {
    return {
        restrict: 'E',
        require: '^ngModel',
        scope: { value: '=ngModel', anoValidar: '&' },
        templateUrl: 'views/templates/CargaHoraria.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.maxl = attrs.maxl;
            scope.nome = attrs.nome;
            scope.mask = attrs.mask;
            scope.obrigatorio = attrs.obrigatorio;

            //var _input = $(element.find('input'));
            //_input.bind('focusout', function () {
            //    scope.$apply(function () {
            //        scope.retorno = ValidarAno(scope.value);
            //    });
            //});
        }
    }
});

camposDirectives.directive('textareaObservacao', function ($parse) {
    return {
        restrict: 'E',
        require: '^ngModel',
        scope: { value: '=ngModel'},
        templateUrl: 'views/templates/textarea/textareaObservacao.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (attrs.required == null || attrs.required === undefined) ? false : true;
            scope.maxl = attrs.maxl;
            scope.nome = attrs.nome;
        }
    }
});

camposDirectives.directive('checkboxGenerico', function ($parse) {
    return {
        restrict: 'E',
        require: '^ngModel',
        scope: { value: '=ngModel' },
        templateUrl: 'views/templates/checkbox/CheckBoxGenerico.html',
        link: function (scope, element, attrs, model) {
            scope.label = attrs.label;
            scope.required = (!attrs.required == null || attrs.required === undefined) ? '' : 'required';
            scope.nome = attrs.nome;
            scope.valor = attrs.valor;
        }
    }
});