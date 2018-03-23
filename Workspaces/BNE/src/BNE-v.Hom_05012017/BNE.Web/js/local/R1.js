$(document).ready(function () {

    $('#btnSolicitarAgora').on('click', function (e) {
        NavegarPasso1(e);
    });

    $('#btnPasso1Continuar').click(function (e) {
        if (typeof (Page_ClientValidate) == 'function') {
            if (Page_ClientValidate("Passo1")) {
                NavegarPasso2(e);
            }
        }
        return false;
    });

    $('#btnPasso2Continuar').click(function (e) {
        if (typeof (Page_ClientValidate) == 'function') {
            if (Page_ClientValidate("Passo2")) {
                NavegarPasso3(e);
            }
        }
        return false;
    });

    $('#btnPasso2Voltar').click(function (e) {
        NavegarPasso1(e);
        e.preventDefault();
    });

    $('#btnPasso3Voltar').click(function (e) {
        NavegarPasso2(e);
        e.preventDefault();
    });

    $('#cphConteudo_txtFuncao').on('blur', function () {
        var res = R1Produto.RecuperarJobFuncao($(this).val());
        if (res.error == null) {
            $('#cphConteudo_txtAtribuicoes').text(res.value);
        }
    });

    $('#cphConteudo_txtTelefone_txtDDD').attr('placeholder', 'DDD');
    $('#cphConteudo_txtTelefone_txtFone').attr('placeholder', 'TELEFONE');
});

function NavegarPasso1(e) {
    $("[name='btn'][value='two']").prop('checked', 'checked');
    e.preventDefault();
    //$('#cphConteudo_txtFuncao').focus();
}

function NavegarPasso2(e) {
    $("[name='btn'][value='three']").prop('checked', 'checked');
    e.preventDefault();
}

function NavegarPasso3(e) {
    $("[name='btn'][value='four']").prop('checked', 'checked');
    e.preventDefault();
}

function AbrirModal() {
    $('#myModal').modal('show');
}

function FecharModal() {
    $('#myModal').modal('hide');
}