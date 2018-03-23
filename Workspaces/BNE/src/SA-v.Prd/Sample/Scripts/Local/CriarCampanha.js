

$(document).ready(function () {
    $('#summernote').summernote({
        lang: 'pt-BR',
        height: 500,
    });
    $('[id^=detail-]').hide();
    $('.toggle').click(function () {
        $input = $(this);
        $target = $('#' + $input.attr('data-toggle'));
        $target.slideToggle();
    });

    //document.getElementsByClassName("note-editor note-frame panel")[0].style.width = "70%"

});
   

    //$('#formEnviar')
    //    .bootstrapValidator({
    //        message: 'This value is not valid',
    //        feedbackIcons: {
    //            valid: 'glyphicon glyphicon-ok',
    //            invalid: 'glyphicon glyphicon-remove',
    //            validating: 'glyphicon glyphicon-refresh'
    //        },
    //        fields: {
    //            lblAsssuntoEmail: {
    //                message: 'Precisa do assunto do e-mail',
    //                validators: {
    //                    notEmpty: {
    //                        message: 'Precisa do assunto do e-mail'
    //                    },
    //                    stringLength: {
    //                        min: 1,
    //                        max: 100,
    //                        message: 'Máximo de 100 caracteres'
    //                    }

    //                }
    //            },
    //            lblCampanha: {
    //                message: 'Precisa Nome da campanha',
    //                validators: {
    //                    notEmpty: {
    //                        message: 'Precisa Nome da campanha'
    //                    },
    //                    stringLength: {
    //                        min: 1,
    //                        max: 100,
    //                        message: 'Máximo de 100 caracteres'
    //                    }

    //                }
    //            }
    //        }
    //    });



function Visualizar(cnpj) {
    $('#div_carregando').show();
    $.ajax({
        url: '/api/apiCampanha/?cnpj=' + cnpj,
        type: 'GET',
        dataType: 'json',
        success: function (resp2) {
            $('#div_carregando').hide();
            console.log(resp2);
            $("#hdfHtmlEsqueleto").val(resp2.Html_Padrao.replace('{Conteudo}', $('#summernote').summernote('code')));
            document.getElementById("divVisualizacao").innerHTML = resp2.Html_Padrao.replace('{Conteudo}', $('#summernote').summernote('code')).replace('{Raz_Social}', resp2.Raz_Social).replace('{Nome_Completo}', resp2.Nme_Usuario).replace('{Primeiro_Nome}', resp2.Nme_Usuario_Primeiro);
            $("#modalVisualizacao").modal('show');
        },
        error: function () {
            $('#div_carregando').hide();
            console.log("erro");
        }
    });
}

function valCampAssunto() {
    if ($("#lblAsssuntoEmail").val().length == 0) {
        $("#lblValAssunto").show();
    }
    else {
        $("#lblValAssunto").hide()
    }
}

function valCampCampanha() {
    if ($("#lblCampanha").val().length == 0) {
        $("#lblValCampanha").show();
    }
    else {
        $("#lblValCampanha").hide()
    }
}
function DispararCampanha(teste) {
    valCampCampanha();
    valCampAssunto();

    if ($("#lblAsssuntoEmail").val().length > 0 && $("#lblCampanha").val().length > 0) {
        $("#modalVisualizacao").modal('hide');
        $("#div_carregando").show();

        $.ajax({
            type: 'POST',
            url: '/api/apiCampanha/DispararCampanha',
            async: false,
            data: {
                html: $("#hdfHtmlEsqueleto").val(), assunto: $("#lblAsssuntoEmail").val(), campanha: $("#lblCampanha").val(), envioTeste: teste, listCnpj: $("#hdflistacnpj").val() },
            contentType: 'application/x-www-form-urlencoded',
            //dataType: 'json',
            success: function (data) {
                $('#div_carregando').hide();
                $("#modalResultado").modal('show');
            },
            error: function (erros) {
                console.log(erros);
                document.getElementById("lblErro").innerHTML =erros.responseText;
                $('#div_carregando').hide();
                $("#modalErro").modal('show');
            }
        });
    }
}
