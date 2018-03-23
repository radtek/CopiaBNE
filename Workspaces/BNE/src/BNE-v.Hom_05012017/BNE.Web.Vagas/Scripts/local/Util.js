var autocomplete = {
    configure: function (control, url, data, selectCallback, type) {
        $('input:text[name*=' + control + ']').on('focus', function () {
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {

                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    var query = request.term;
                    if (type === autocomplete.type.bairro) {
                        query = autocomplete.extractLast(request.term);
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
                    if (type === autocomplete.type.bairro) {
                        var terms = autocomplete.split(this.value);
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
                    if (type === autocomplete.type.bairro) {
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
    type: {
        cidade: 'Cidade', bairro: 'Bairro', funcao: 'Funcao'
    },
    funcao: function (control, callback) {
        var data = { limit: autocomplete.limit.LimiteCompleteFuncao };
        autocomplete.configure(control, autocomplete.URL.URLCompleteFuncao, data, callback, autocomplete.type.funcao);
    },
    cidade: function (control, callback) {
        var data = { limit: autocomplete.limit.LimiteCompleteCidade };
        autocomplete.configure(control, autocomplete.URL.URLCompleteCidade, data, callback, autocomplete.type.cidade);
    },
    bairro: function (control, cidade, estado, callback) {
        var data = { limit: autocomplete.limit.LimiteCompleteBairro, cidade: cidade, estado: estado };
        autocomplete.configure(control, autocomplete.URL.URLCompleteBairro, data, callback, autocomplete.type.bairro);
    },
    split: function (val) {
        return val.split(/,\s*/);
    },
    extractLast: function (term) {
        return autocomplete.split(term).pop();
    },
    URL: {
        URLCompleteCidade: '',
        URLCompleteFuncao: '',
        URLCompleteBairro: ''
    },
    limit: {
        LimiteCompleteCidade: '',
        LimiteCompleteFuncao: '',
        LimiteCompleteBairro: '',
    }
};

var modal = {
    abrirModal: function (control) {

        var id = control + $.now();
        $('#' + control).attr('id', id);

        //$('.modal:not(#' + control.id + ')').bPopup().close();

        if (latestOpenedModal !== '' && latestOpenedModal !== id) {
            if ($('#' + latestOpenedModal).length > 0) //Verifica se o elemento existe
            {
                $('#' + latestOpenedModal).bPopup().close();
                $('#' + latestOpenedModal).remove();
            }
        }

        latestOpenedModal = id;

        $('#' + id).bPopup({
            modalClose: false, modalColor: '#cde1e8', zIndex: 2, easing: 'swing', onClose: function () {
                $(this).remove();
            }
        });
    }
};

var latestOpenedModal = '';

//Conta quantos acessos houveram em uma determinada página
function trackEvent(category, action, label) {
    try {
        ga('send', 'event', category, action, label);
    } catch (err) { }
}