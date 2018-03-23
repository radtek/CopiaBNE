var LanDirectives = angular.module('LanDirectives', []);

LanDirectives.directive('getName', ['$http', function ($http) {
    return function (scope, element, attrs) {
        element.bind("blur", function (e) {
            var cpf = $('#CPF').val();
            var dataNascimento = $('#DataNascimento').val();

            if (cpf != "" && dataNascimento != "") {
                $http.get(urlAPI + 'Curriculo/GetCandidatoNome?cpf=' + scope.loginData.userName + '&dataNascimento=' + scope.loginData.password)
                .success(function (data) {
                    scope.loginData.nome = data;
                })
                .error(function (data) {
                });
            }
        });
    }
}]);

LanDirectives.directive('validar9Digitos', ['$rootScope', function ($rootScope) {
    //Define os DDD ques tem celulares com 9 dígitos
    var dddNoveDigitos = ['11', '12', '13', '14', '15', '16', '17', '18', '19', '21', '22', '24', '27', '28', '31', '32', '33', '34', '35', '37', '38', '41', '42', '43', '44', '45', '46', '47', '48', '49', '51', '53', '54', '55', '61', '62', '63', '64', '65', '66', '67', '68', '69', '71', '73', '74', '75', '77', '79', '81', '89', '91', '92', '93', '94', '95', '96', '97', '98', '99'];
    var Celular = $('#Celular').val();

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    if ($('#Celular').val() != "") {
                        var isDDD9Digitos = dddNoveDigitos.indexOf($('#DDDCelular').val()) > -1;
                        var celularValido = ValidarCelular(ctrl.$modelValue);
                        $rootScope.celularTem9Digitos = $('#Celular').val().length == 9;
                        $rootScope.is9Digitos = isDDD9Digitos;

                        if (isDDD9Digitos) {
                            if ($rootScope.celularTem9Digitos && celularValido)
                                ctrl.$setValidity('Celular', true);
                            else
                                ctrl.$setValidity('Celular', false);
                        }
                        else
                            ctrl.$setValidity('Celular', true);
                    } else {
                        ctrl.$setValidity('Celular', true);
                    }
                });
            });
        }
    }
}])

LanDirectives.directive('onlyNum', function () {
    return function (scope, element, attrs) {

        var keyCode = [8, 9, 37, 39, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 110];
        element.bind("keydown", function (event) {
            if ($.inArray(event.which, keyCode) == -1) {
                scope.$apply(function () {
                    scope.$eval(attrs.onlyNum);
                    event.preventDefault();
                });
                event.preventDefault();
            }

        });

        element.bind("keypress", function (event) {
            if ($(element).val().length >= 2) {
                return false;
            }
        });

        //element.bind("keyup", function (e) {
        //    e.preventDefault();
        //    var expre = /[^0-9]/g;
        //    // REMOVE OS CARACTERES DA EXPRESSAO ACIMA
        //    if ($(this).val().match(expre))
        //        $(this).val($(this).val().replace(expre, ''));
        //});
    };
});

LanDirectives.directive('sonumeros', function () {
    return function (scope, element, attrs) {
        element.bind("keyup blur focus", function (e) {
            e.preventDefault();
            var expre = /[^0-9]/g;
            ///^d{3}.d{3}.d{3}-d{2}$/
            // REMOVE OS CARACTERES DA EXPRESSAO ACIMA
            if ($(this).val().match(expre))
                $(this).val($(this).val().replace(expre, ''));
        });
    }
});

LanDirectives.directive('autocompleteCidade', ['$parse', 'localStorageService', function ($parse, localStorageService) {
    return function (scope, element, attrs, controller) {
        var ngModel;
        if (attrs.ngModel2)
            ngModel = $parse(attrs.ngModel2);
        else
            ngModel = $parse(attrs.ngModel);

        $(function () {
            $(element).autocomplete({
                source: function (request, response) {
                    var data = {};
                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    $(this).data('xhr', $.ajax({
                        url: urlAPI + 'AutoComplete/GetCidade',
                        dataType: "json",
                        data: {
                            limit: 10,
                            query: request.term
                        },
                        beforeSend: function (xhr) {
                            var authData = localStorageService.get('authorizationData');
                            xhr.setRequestHeader("Authorization", "bearer " + authData.token);
                        },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.text }
                            }))
                        }
                    }));
                },
                selectFirst: true,
                autoFocus: true,
                cache: false,
                select: function (event, ui) {
                    this.value = ui.item.label;
                    scope.$apply(function (scope) {
                        // Change binded variable
                        ngModel.assign(scope, ui.item.label);
                    });
                    return true;
                },
                //delay: 100,
                delay: 0,
                minLength: 1,
                open: function (event, ui) { disableFuncao = true; },
                close: function (event, ui) { disableFuncao = false; $(this).focus(); }
            });
        });
    }
}]);

LanDirectives.directive('autocompleteFuncao', ['$parse', 'localStorageService', function ($parse, localStorageService) {
    return function (scope, element, attrs, controller) {

        var ngModel;
        if (attrs.ngModel2)
            ngModel = $parse(attrs.ngModel2);
        else
            ngModel = $parse(attrs.ngModel);

        $(function () {
            $(element).autocomplete({

                source: function (request, response) {
                    var data = {};
                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    $(this).data('xhr', $.ajax({
                        url: urlAPI + 'AutoCompleteFuncao/GetFuncao',
                        dataType: "json",
                        data: {
                            limit: 20,
                            query: request.term
                        },
                        beforeSend: function (xhr) {
                            var authData = localStorageService.get('authorizationData');
                            xhr.setRequestHeader("Authorization", "bearer " + authData.token);
                        },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.text }
                            }))
                        }
                    }));
                },
                selectFirst: true,
                autoFocus: true,
                cache: false,
                select: function (event, ui) {
                    this.value = ui.item.label;
                    scope.$apply(function (scope) {
                        // Change binded variable
                        ngModel.assign(scope, ui.item.label);
                    });
                    return true;
                },
                //delay: 100,
                delay: 0,
                minLength: 1
                //open: function (event, ui) { disableFuncao = true; },
                //close: function (event, ui) { disableFuncao = false; $(this).focus(); }
            });
        });
    }
}]);

LanDirectives.directive('autocompleteEmail', ['$parse', 'localStorageService', function ($parse, localStorageService) {
    return function (scope, element, attrs, controller) {
        var ngModel;
        if (attrs.ngModel2)
            ngModel = $parse(attrs.ngModel2);
        else
            ngModel = $parse(attrs.ngModel);

        $(function () {
            $(element).autocomplete({
                source: function (request, response) {
                    var data = {};
                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    $(this).data('xhr', $.ajax({
                        url: urlAPI + "AutoCompleteEmail/GetSugestaoEmail",
                        dataType: "json",
                        data: {
                            limit: 15,
                            query: request.term
                        },
                        beforeSend: function (xhr) {
                            var authData = localStorageService.get('authorizationData');
                            xhr.setRequestHeader("Authorization", "bearer " + authData.token);
                        },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.text }
                            }))
                        }
                    }));
                },
                selectFirst: true,
                autoFocus: true,
                cache: false,
                select: function (event, ui) {
                    this.value = ui.item.label;
                    scope.$apply(function (scope) {
                        // Change binded variable
                        ngModel.assign(scope, ui.item.label);
                    });
                    return true;
                },
                //delay: 100,
                delay: 0,
                minLength: 1,
                //open: function (event, ui) { disableFuncao = true; },
                //close: function (event, ui) { disableFuncao = false; $(this).focus(); }
            });
        });
    }
}]);

LanDirectives.directive('autocompleteCurso', ['$parse', 'localStorageService', function ($parse, localStorageService) {
    return function (scope, element, attrs, controller) {
        var ngModel;
        if (attrs.ngModel2)
            ngModel = $parse(attrs.ngModel2);
        else
            ngModel = $parse(attrs.ngModel);
        $(function () {
            $(element).autocomplete({

                source: function (request, response) {
                    var data = {};
                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    $(this).data('xhr', $.ajax({
                        url: urlAPI + 'Curso/',
                        dataType: "json",
                        data: {
                            limit: 20,
                            query: request.term
                        },
                        beforeSend: function (xhr) {
                            var authData = localStorageService.get('authorizationData');
                            xhr.setRequestHeader("Authorization", "bearer " + authData.token);
                        },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.text }
                            }))
                        }
                    }));
                },
                selectFirst: true,
                autoFocus: true,
                cache: false,
                select: function (event, ui) {
                    this.value = ui.item.label;
                    scope.$apply(function (scope) {
                        // Change binded variable
                        ngModel.assign(scope, ui.item.label);
                        //scope.validacao_cidade(ui.item.label, scope, controller, cidade_sucesso, cidade_erro);
                    });
                    return true;
                },
                //delay: 100,
                delay: 0,
                minLength: 1,
                //open: function (event, ui) { disableFuncao = true; },
                //close: function (event, ui) { disableFuncao = false; $(this).focus(); }
            });
        });
    }
}]);

LanDirectives.directive('autocompleteInstituicao', ['$parse', 'localStorageService', function ($parse, localStorageService) {
    return function (scope, element, attrs, controller) {
        var ngModel;
        if (attrs.ngModel2)
            ngModel = $parse(attrs.ngModel2);
        else
            ngModel = $parse(attrs.ngModel);
        $(function () {
            $(element).autocomplete({

                source: function (request, response) {
                    var data = {};
                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    $(this).data('xhr', $.ajax({
                        url: urlAPI + 'Fonte/',
                        dataType: "json",
                        data: {
                            limit: 20,
                            query: request.term
                        },
                        beforeSend: function (xhr) {
                            var authData = localStorageService.get('authorizationData');
                            xhr.setRequestHeader("Authorization", "bearer " + authData.token);
                        },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.text }
                            }))
                        }
                    }));
                },
                selectFirst: true,
                autoFocus: true,
                cache: false,
                select: function (event, ui) {
                    this.value = ui.item.label;
                    scope.$apply(function (scope) {
                        // Change binded variable
                        ngModel.assign(scope, ui.item.label);
                    });
                    return true;
                },
                //delay: 100,
                delay: 0,
                minLength: 1
                //open: function (event, ui) { disableFuncao = true; },
                //close: function (event, ui) { disableFuncao = false; $(this).focus(); }
            });
        });
    }
}]);

LanDirectives.directive('inputCurrency', function ($filter, $locale) {
    return {
        terminal: true,
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel) {

            if (!ngModel)
                return; // do nothing if no ng-model

            // get the number format
            var formats = $locale.NUMBER_FORMATS;

            // construct the currency prefix
            var outer = angular.element('<div />').addClass('input-group');
            var span = angular.element('<span />').addClass('input-group-addon');
            $(span).html(formats.CURRENCY_SYM).appendTo(outer);

            // insert it on the page and add the input to it
            $(outer).insertBefore(element);
            $(element).appendTo(outer).addClass('numeric');

            // fix up the incoming number to make sure it will parse into a number correctly
            var fixNumber = function (number) {
                if (number) {
                    if (typeof number !== 'number') {

                        number = number.replace('.', '');
                        number = number.replace(',', '');
                        number = parseFloat(number);
                        number = number / 100;
                    }
                }
                return number;
            }

            // function to do the rounding
            var roundMe = function (number) {
                number = fixNumber(number);
                if (number) {
                    return $filter('number')(number, 2);
                } else
                    return $filter('number')(0, 2);
            }

            // Listen for change events to enable binding
            element.bind('blur', function () {

                element.val(roundMe(ngModel.$modelValue));
            });

            // push a formatter so the model knows how to render
            ngModel.$formatters.push(function (value) {
                if (value) {
                    return roundMe(value);
                }
                else {
                    return $filter('number')(0, 2)
                }
            });

            // push a parser to remove any special rendering and make sure the inputted number is rounded
            ngModel.$parsers.push(function (value) {
                if (value) {
                    var val_el = roundMe(value);
                    element.val(val_el);
                    return fixNumber(val_el);
                }
            });
        }
    };
});

LanDirectives.directive('numberValidator', [function () {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, ctrl) {
            ctrl.$parsers.unshift(maior_que_zero);
            ctrl.$formatters.unshift(maior_que_zero);

            function maior_que_zero(viewValue) {
                _Value = viewValue || '0';
                _Value = _Value.replace('.', '');
                _Value = _Value.replace(',', '');
                if (parseFloat(_Value) === 0) {
                    ctrl.$setValidity('greaterzero', false);
                }
                else {
                    ctrl.$setValidity('greaterzero', true);
                }
                return viewValue;
            }

        }
    }
}]);

LanDirectives.directive('validDate', function ($filter, $window, $parse, $timeout) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            var moment = $window.moment;
            var getter, setter;

            //Declaring the getter and setter
            getter = $parse(attrs.ngModel);
            setter = getter.assign;

            //Set the initial value to the View and the Mode       

            ngModel.$formatters.unshift(function (modelValue) {

                if (!modelValue) return "";

                if (modelValue.length < 8) return "";

                var anoOK = FormatarAno(modelValue.substr(modelValue.length - 4).substr(2, 2));

                var retVal = $filter('date')(modelValue, "DD/MM/YYYY");
                setter(scope, retVal);

                retVal = modelValue.substr(0, 2) + '/' + modelValue.substr(3, 2) + '/' + anoOK;

                return retVal;
            });

            // If the ngModel directive is used, then set the initial value and keep it in sync
            if (ngModel) {
                element.bind('blur', function (event) {

                    var dt = element.val();

                    dt = dt.trim();

                    if (dt.length == 8) {
                        dt = dt.substring(0, 2) + '/' + dt.substring(2, 4) + '/' + dt.substring(4, 8);
                    } else {
                        if (dt.length == 6) {
                            var dia = dt.substring(0, 2);
                            var mes = dt.substring(2, 4);
                            var ano = dt.substring(4, 6);

                            ano = parseInt(ano);

                            FormatarAno(ano);

                            dt = dia + '/' + mes + '/' + ano;
                        }
                    }

                    var date = moment($filter('date')(dt, "dd/MM/yyyy"), "DD/MM/YYYY");
                    // if the date entered is a valid date then save the value
                    if (date && moment(dt, "DD/MM/YYYY").isValid()) {
                        element.css('background', 'white');
                        element[0].value = $filter('date')(date.toDate(), "dd/MM/yyyy");

                        var newValue = element.val();
                        scope.$apply(function () {
                            setter(scope, newValue);
                            if (CalcularIdade(newValue) > 13)
                                scope.Nascimentovalido = true;
                            else
                                scope.Nascimentovalido = false;
                        });
                    } else { //show an error and clear the value

                        element.css('background', 'pink');
                        element[0].value = "";
                        scope.$apply(function () {
                            setter(scope, '');
                            scope.Nascimentovalido = true;
                        });
                    }
                });

                element.bind('focus', function () {
                    scope.$apply(function () {
                        var dataLimpa = $(element).val().replace('/', '').replace('/', '');
                        element[0].value = dataLimpa;
                    });
                });
            }
        }
    };
});

LanDirectives.directive('cpfValidator', function ($filter, $window, $parse, $timeout) {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, ctrl) {
            var getter, setter;

            //Declaring the getter and setter
            getter = $parse(attrs.ngModel);
            setter = getter.assign;

            ctrl.$formatters.unshift(function (modelValue) {

                if (!modelValue) return "";

                var retVal = modelValue;
                setter(scope, retVal);

                retVal = modelValue.substr(0, 3) + '.' + modelValue.substr(3, 3) + '.' + modelValue.substr(6, 3) + '-' + modelValue.substr(9, 2);
                //retVal = modelValue;

                return retVal;
            });

            ele.bind('blur', function () {
                scope.$apply(function () {
                    var retorno = ValidaCPF($(ele).val());

                    if (retorno) {
                        var cpfFormatado = formatarCPF(ctrl, $(ele).val())
                        ctrl.$modelValue = cpfFormatado;
                        ctrl.$viewValue = cpfFormatado;
                        ele[0].value = cpfFormatado;
                    }
                    ctrl.$setValidity('cpf', retorno);
                    ctrl.$invalid = !retorno;
                });
            });

            ele.bind('focus', function () {
                scope.$apply(function () {
                    var cpfLimpo = LimparCPF(ctrl, $(ele).val());
                    //ctrl.$modelValue = cpfLimpo;
                    //ctrl.$viewValue = cpfLimpo;
                    ele[0].value = cpfLimpo;
                });
            });
        }
    }
});

LanDirectives.directive('validarNomeCompleto', [function ($parse) {

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('nome', ValidarNome(ctrl.$modelValue));
                });
            });
        }
    }
}]);

LanDirectives.directive('validarEmail', [function ($parse) {

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    var retorno = validarEmail(ctrl.$modelValue);
                    //if (ctrl.$isEmpty(ctrl.$modelValue)) {
                    //    ctrl.$setValidity('', false);
                    //    ctrl.$invalid = false;
                    //}
                    //else {
                    ctrl.$setValidity('email', retorno);
                    //ctrl.$invalid = !retorno;
                    //}
                });
            });
        }
    }
}]);

LanDirectives.directive('validarCelular', [function ($parse) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('valid_celular', ValidarCelular(ctrl.$modelValue));
                });
            });
        }
    }
}]);

LanDirectives.directive('validarTelefone', [function ($parse) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('valid_telefone', ValidarTelefone(ctrl.$modelValue));
                });
            });
        }
    }
}]);

LanDirectives.directive('validarCidade', ['$http', function ($http) {

    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, ctrl) {

            ele.bind('blur', function () {
                scope.$apply(function () {
                    if (ctrl.$modelValue.length > 3) {
                        $http.post(urlAPI + '/AutoComplete/ValidaCidade?cidade=' + ctrl.$modelValue).success(function (res) {
                            ctrl.$setValidity('valid_cidade', res.valid);
                        }).error(function () {
                            ctrl.$setValidity('valid_cidade', res.valid);
                        });
                    } else {
                        ctrl.$setValidity('valid_cidade', true);
                    }

                });
            });
        }
    }
}]);

LanDirectives.directive('anoDg', ['$http', function ($parse) {

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    var anoOK = FormatarAno(ctrl.$modelValue);
                    element[0].value = anoOK;
                });
            });
        }
    }
}]);

LanDirectives.directive('validarPretensao', [function ($parse, $scope) {

    $('#avisoMediaSalario').hide();

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('pretensao', ValidarPretensaoMaiorqueSalarioMinimo(ctrl.$modelValue, sessionStorage.getItem('salarioMinimo')));
                });
            });
        }
    }

}]);

LanDirectives.directive('validarAno', [function ($scope) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('ano', ValidarAno($(element).val()));
                });
            });
        }
    }
}]);

LanDirectives.directive('validarDatasExperienciaProfissional', [function ($scope) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('AnoInicio', ValidarDataDemissao($('#MesInicio').val(), $('#AnoInicio').val(), $('#MesSaida').val(), $('#AnoSaida').val()));
                });
            });
        }
    }
}]);

LanDirectives.directive('validarDatasExperienciaProfissionalPe', [function ($scope) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    var retorno = ValidarDataDemissao($('#MesInicioPE').val(), $('#AnoInicioPE').val(), $('#MesSaidaPE').val(), $('#AnoSaidaPE').val());
                    ctrl.$setValidity('valid_dtaExperienciape', retorno);
                });
            });
        }
    }
}]);

LanDirectives.directive('validarDatasExperienciaProfissionalTres', [function ($scope) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {

            element.bind('blur', function () {
                scope.$apply(function () {
                    ctrl.$setValidity('valid_dtaExperiencia3', ValidarDataDemissao($('#MesInicio3').val(), $('#AnoInicio3').val(), $('#MesSaida3').val(), $('#AnoSaida3').val()));
                });
            });
        }
    }
}]);

LanDirectives.directive('scrollOnClick', function () {
    return {
        restrict: 'A',
        link: function (scope, $elm) {
            $elm.on('click', function () {
                $("body").animate({ scrollTop: 0 }, "slow");
            });
        }
    }
});

LanDirectives.directive('scrollUltimoEmprego', function () {
    return {
        restrict: 'A',
        link: function (scope, $elm) {
            $elm.on('click', function () {
                $("body").animate({ scrollTop: 0 }, "slow");
            });
        }
    }
});

LanDirectives.directive('scrollPenultimoEmprego', function () {
    return {
        restrict: 'A',
        link: function (scope, $elm) {
            $elm.on('click', function () {
                $("body").animate({ scrollTop: 480 }, "slow");
            });
        }
    }
});

LanDirectives.directive('scroll3Emprego', function () {
    return {
        restrict: 'A',
        link: function (scope, $elm) {
            $elm.on('click', function () {
                $("body").animate({ scrollTop: 980 }, "slow");
            });
        }
    }
});

//buscar o valor do salário para a função e estado.
LanDirectives.directive('buscarMedia', ['$http', '$rootScope', function ($http, $rootScope) {

    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, ctrl) {

            ele.bind('blur', function () {
                scope.$apply(function () {
                    var cidade = $('#Cidade').val();
                    var funcao = $('#Funcao').val();

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
}]);

LanDirectives.directive('filters', []).
    filter('truncate', function () {
        return function (text, length, end) {
            if (isNaN(length))
                length = 10;

            if (end === undefined)
                end = "...";

            if (text.length <= length || text.length - end.length <= length) {
                return text;
            }
            else {
                return String(text).substring(0, length - end.length) + end;
            }

        };
    });

LanDirectives.directive('acaoEditar', ['$http', function ($http) {
    return {
        link: function (scope, elemento, attrs, ctrl) {
            elemento.on('mouseover', function () {
                scope.$apply(function () {
                    elemento.addClass('EdicaoInformacoes');
                    elemento.after('<span id="talkbubble"><i class="glyphicon glyphicon-pencil"></i></span>');
                });
            });
            elemento.on('mouseout', function () {
                scope.$apply(function () {
                    elemento.removeClass('EdicaoInformacoes');
                    $('#talkbubble').remove();
                });
            });
        }
    }
}]);
