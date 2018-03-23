var autocomplete = {
    configure: function (control, url) {
        $('[id$=' + control + ']').on('focus', function () {
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {

                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    $(this).data('xhr', $.ajax({
                        url: url,
                        dataType: "json",
                        data: {
                            limit: 10,
                            query: request.term
                        },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item };
                            }));
                        }
                    }));
                },
                cache: false,
                select: function (event, ui) {
                    this.value = ui.item.label;
                    return false;
                },
                delay: 0,
                minLength: 0,
                focus: function (event, ui) {
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
    funcao: function (control) {
        autocomplete.configure(control, '/ajax/Funcao');
    },
    cidade: function (control) {
        autocomplete.configure(control, '/ajax/Cidade');
    }
};

/*
var modal = {
    abrirModal: function (control) {
        //alert('');
        //$.modal.close(); // must call this to have SimpleModal
        //alert($('#' + control.id)[0].id);
        $(control).modal({
            onClose: function (dialog) {
                dialog.data.fadeOut('fast', function () {
                    dialog.container.slideUp('fast', function () {
                        dialog.overlay.fadeOut('fast', function () {
                            $.modal.close(); // must call this to have SimpleModal
                            $('#' + control.id).remove();
                        });
                    });
                });
            }
        });
    }
};
*/

/*
var modal = {
    abrirModal: function (control) {
        //alert('fancybox');
        $('#' + control).magnificPopup({
            type: 'ajax'
        });
    }
};
*/

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