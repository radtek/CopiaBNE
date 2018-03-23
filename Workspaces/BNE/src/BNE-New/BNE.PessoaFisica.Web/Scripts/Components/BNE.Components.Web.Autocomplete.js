bne.components.web.autocomplete = {
    configure: function (control, url, data, selectCallback, type) {
        $(document.body).on('focus', control, function () {
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {

                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    var query = request.term;
                    if (type === bne.components.web.autocomplete.type.bairro) {
                        query = bne.components.web.autocomplete.extractLast(request.term);
                        data.cidade = data.cidade;
                        data.funcao = data.funcao;
                    }
                    data.query = query;

                    $(this).data('xhr',
                        $.ajax({
                            url: url,
                            dataType: "json",
                            data: data,
                            success: function (data) {
                                response($.map(data, function (item) {
                                    return { label: item };
                                }));
                            }
                        }));
                },
                cache: false,
                select: function (event, ui) {
                    if (type === bne.components.web.autocomplete.type.bairro) {
                        var terms = bne.components.web.autocomplete.split(this.value);
                        // remove the current input
                        terms.pop();
                        // add the selected item
                        terms.push(ui.item.value);
                        // add placeholder to get the comma-and-space at the end
                        terms.push("");
                        this.value = terms.join(", ");
                        return false;
                    } else {
                        this.value = ui.item.label;
                    }
                    if (selectCallback != null) {
                        selectCallback();
                    }
                    return false;
                },
                delay: 0,
                minLength: 0,
                focus: function (event, ui) {
                    if (type === bne.components.web.autocomplete.type.bairro) {
                        return false;
                    }
                    var menu = $(this).data("ui-autocomplete").menu.element;
                    menu.find("li").removeClass("ui-state-hover");
                    menu.find("li:has(a.ui-state-focus)").addClass("ui-state-hover");
                },
                close: function (event, ui) {
                    var menu = $(this).data("ui-autocomplete").menu.element;
                    menu.find("li").removeClass("ui-state-hover");
                }
            });
        });
    },
    validateRequired: function validateRequired(control) {
        $(control).rules("add", { required: true, messages: { required: 'Campo Obrigatório' } });
    },
    funcao: function (control, required, validateInput, callback) {
        control = 'input:text[id*=' + control + ']';

        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteFuncao };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteFuncao, data, callback, bne.components.web.autocomplete.type.funcao);
        if (required != null && required) {
            bne.components.web.autocomplete.validateRequired(control);
        }
        if (validateInput != null && validateInput) {
            bne.components.web.autocomplete.validacao.funcao(control);
        }
    },
    cidade: function (control, required, validateInput, callback) {
        control = 'input:text[id*=' + control + ']';

        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteCidade };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteCidade, data, callback, bne.components.web.autocomplete.type.cidade);
        if (required != null && required) {
            bne.components.web.autocomplete.validateRequired(control);
        }
        if (validateInput != null && validateInput) {
            bne.components.web.autocomplete.validacao.cidade(control);
        }
    },
    bairro: function (control, cidade, estado, callback) {
        control = 'input:text[id*=' + control + ']';

        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteBairro, cidade: cidade, estado: estado };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteBairro, data, callback, bne.components.web.autocomplete.type.bairro);
    },
    email: function (control, required, validateInput, callback) {
        control = 'input:text[id*=' + control + ']';
        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteEmail };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteEmail, data, callback, bne.components.web.autocomplete.type.email);

        if (required != null && required) {
            bne.components.web.autocomplete.validateRequired(control);
        }
        if (validateInput != null && validateInput) {
            bne.components.web.autocomplete.validacao.email(control);
        }
    },
    curso: function (control, required, validateInput, callback) {
        control = 'input:text[id*=' + control + ']';
        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteCurso };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteCurso, data, callback, bne.components.web.autocomplete.type.curso);

        if (required != null && required) {
            bne.components.web.autocomplete.validateRequired(control);
        }
        if (validateInput != null && validateInput) {
            //bne.components.web.autocomplete.validacao.email(control);
        }
    },
    instituicaoEnsino: function (control, required, validateInput, callback) {
        control = 'input:text[id*=' + control + ']';
        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteInstituicaoEnsino };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteInstituicaoEnsino, data, callback, bne.components.web.autocomplete.type.instituicaoEnsino);

        if (required != null && required) {
            bne.components.web.autocomplete.validateRequired(control);
        }
        if (validateInput != null && validateInput) {
            //bne.components.web.autocomplete.validacao.email(control);
        }
    },
    ramoAtividade: function (control, required, validateInput, callback) {
        control = 'input:text[id*=' + control + ']';
        var data = { limit: bne.components.web.autocomplete.limit.LimiteCompleteRamoAtividade };
        bne.components.web.autocomplete.configure(control, bne.components.web.autocomplete.URL.URLCompleteRamoAtividade, data, callback, bne.components.web.autocomplete.type.ramoAtividade);

        if (required != null && required) {
            bne.components.web.autocomplete.validateRequired(control);
        }
        if (validateInput != null && validateInput) {
            //bne.components.web.autocomplete.validacao.email(control);
        }
    },
    split: function (val) {
        return val.split(/,\s*/);
    },
    extractLast: function (term) {
        return bne.components.web.autocomplete.split(term).pop();
    },
    type: {
        cidade: 'Cidade', bairro: 'Bairro', funcao: 'Funcao', email: 'Email', curso: 'Curso', instituicaoEnsino: 'InstituicaoEnsino', ramoAtividade: 'ramoAtividade'
    },
    URL: {
        URLCompleteCidade: '',
        URLCompleteFuncao: '',
        URLCompleteBairro: '',
        //URLCompleteEmail: 'http://gatewayapiteste.bne.com.br/bne/pessoafisica/public/RankingEmail?nome=',
        URLCompleteEmail: 'http://api.pessoafisica.bne.com.br/v1/RankingEmail?query=',
        URLCompleteCurso: 'http://api.pessoafisica.bne.com.br/v1/curso/GetCursos?query=',
        URLCompleteInstituicaoEnsino: 'http://api.pessoafisica.bne.com.br/v1/instituicao/GetInstituicaoEnsinos?query=',
        URLCompleteRamoAtividade: 'http://api.pessoafisica.bne.com.br/v1/ramoatividade/Get?query='

    },
    limit: {
        LimiteCompleteCidade: '',
        LimiteCompleteFuncao: '',
        LimiteCompleteBairro: '',
        LimiteCompleteEmail: '10',
        LimiteCompleteCurso: '10',
        LimiteCompleteInstituicaoEnsino: '10',
        LimiteCompleteRamoAtividade: '10',
    },
    validacao: {
        cidade: function (sender, args) {
            $.validator.addMethod("validateFormatCidade", function (value, element) {
                var valido = false;
                var data = { query: value, limit: 1 };
                $.ajax({
                    url: bne.components.web.autocomplete.URL.URLCompleteCidade,
                    dataType: "json",
                    data: data,
                    async: false,
                    success: function (result) {
                        if (result != null) {
                            //args.IsValid = result.contains(data.query);
                            valido = result.indexOf(data.query) > -1;
                        }
                    }
                });
                return this.optional(element) || valido;
            }, "* Cidade Inválida");

            $(control).rules("add", { validateFormatCidade: true });
        },
        funcao: function (control) {
            $.validator.addMethod("validateFormatFuncao", function (value, element) {
                var valido = false;
                var data = { query: value, limit: 1 };
                $.ajax({
                    url: bne.components.web.autocomplete.URL.URLCompleteFuncao,
                    dataType: "json",
                    data: data,
                    async: false,
                    success: function (result) {
                        if (result != null) {
                            //valido = result.contains(data.query);
                            valido = result.indexOf(data.query) > -1;
                        }
                    }
                });
                return this.optional(element) || valido;
            }, "* Função Inválida");

            $(control).rules("add", { validateFormatFuncao: true });
        },
        email: function (control) {
            $.validator.addMethod("validateFormatEmail", function (email, element) {
                if (email == null || email == '')
                    return false;

                return (/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/.test(email))
            }, "Email inválido!");

            $(control).rules("add", { validateFormatEmail: true });
        }
    }
};