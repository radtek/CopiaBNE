$(document).on("click", '.btn_add_user', function () {
    $('#list-user').slideToggle('fast');
    return false;
});

$(document).on("click", ".button-remove-user", function () {
    var txtNome = $(this).closest(".user").find('[id^=txtNomeResponsavelAdicional]');
    txtNome.val('');

    var txtEmail = $(this).closest(".user").find('[id^=txtEmailResponsavelAdicional]');
    txtEmail.val('');

    $(this).closest(".user").hide();

    return false;
});

function ConfigurarPasso1() {
    $("#CadastroEmpresaForm").validate();

    bne.components.web.CNPJ('txtCNPJ', true);
    bne.components.web.textbox('txtSite', false, bne.components.web.textbox.type.site, 150);
    bne.components.web.telefone('txtNumeroComercial', true, bne.components.web.telefone.type.fixo);
    bne.components.web.textbox('txtQuantidadeFuncionario', true, bne.components.web.textbox.type.number, 3);

    /*
    $('#txtCNPJ').val('78.575.149/0006-09');
    $('#txtSite').val('www.site.com.br');
    $('#txtNumeroComercial').val('(41) 3055-6000');
    $('#txtQuantidadeFuncionario').val('60');
    */

    componentHandler.upgradeDom();
}

function ConfigurarCaptcha() {
    ConfigurarPasso1();
    bne.components.web.textbox('txtCaptchaReceitaFederal', true);
}

function QuantidadeUsuarioAdicionado() {
    return $('.box').children('.user').length;
}

function ConfigurarPasso2() {
    $('.content-add-user').hide();

    $('.btn_add_user').click(function () {
        $('#list-user').slideToggle('fast');

        if (QuantidadeUsuarioAdicionado() === 0) {
            AdicionarUsuario();
        }
        return false;
    });

    if (QuantidadeUsuarioAdicionado() > 0) {
        $('#list-user').show();
    }

    $('.btn_add_new_user').click(function () {
        AdicionarUsuario();
        return false;
    });

    componentHandler.upgradeDom();

    $('.box').children('.user').each(function () {
        var txtNome = $(this).find('[id^=txtNomeResponsavelAdicional]');
        var txtEmail = $(this).find('[id^=txtEmailResponsavelAdicional]');

        if (txtNome.val() === '' && txtEmail.val() === '') {
            $(this).hide();
        }
    });

    bne.components.web.textbox('txtNomeResponsavel', true, bne.components.web.textbox.type.nome, 200);
    bne.components.web.CPF('txtCPFResponsavel', true);
    bne.components.web.data('txtDataNascimentoResponsavel', true);
    bne.components.web.autocomplete.funcao('txtFuncaoResponsavel', true, false);
    bne.components.web.telefone('txtNumeroCelularResponsavel', true, bne.components.web.telefone.type.celular);
    bne.components.web.textbox('txtEmailResponsavel', true, bne.components.web.textbox.type.email);
    bne.components.web.telefone('txtNumeroComercialResponsavel', true, bne.components.web.telefone.type.fixo);
    bne.components.web.radiobutton('option-f', true);

    bne.components.web.textbox('txtNomeResponsavelAdicional', true, bne.components.web.textbox.type.nome);
    bne.components.web.textbox('txtEmailResponsavelAdicional', true, bne.components.web.textbox.type.email);
}

function LimparMDL(component) {
    $(component).attr('data-upgraded', '');
    $(component).attr('class', $(component).attr('class').replace(/is-upgraded/g, ''));
}

function AdicionarUsuario() {
    var divUsuario = $('.box').children('.user-template:first').clone();

    divUsuario.children().each(function () {
        LimparMDL(this);
        $(this).children().each(function () {
            LimparMDL(this);
            $(this).children().each(function () {
                LimparMDL(this);
            });
        });
    });

    divUsuario.find('label.error').each(function () {
        $(this).remove();
    });

    divUsuario.removeClass('user-template');
    divUsuario.addClass('user');

    var idNome = "txtNomeResponsavelAdicional" + QuantidadeUsuarioAdicionado();
    var nameNome = "UsuariosAdicionais[" + QuantidadeUsuarioAdicionado() + "].Nome";
    var txtNome = divUsuario.find('#txtNomeResponsavelAdicional');

    txtNome.attr("id", idNome);
    txtNome.attr("name", nameNome);
    txtNome.val('');

    var idEmail = "txtEmailResponsavelAdicional" + QuantidadeUsuarioAdicionado();
    var nameEmail = "UsuariosAdicionais[" + QuantidadeUsuarioAdicionado() + "].Email";
    var txtEmail = divUsuario.find('#txtEmailResponsavelAdicional');

    txtEmail.attr("id", idEmail);
    txtEmail.attr("name", nameEmail);
    txtEmail.val('');

    $('.box').append(divUsuario).show();

    componentHandler.upgradeDom();

    return false;
}

var onSuccess = function (result) {
    if (result.url) {
        window.location.href = result.url;
    }
}

function ConfigurarConfirmacao() {
    componentHandler.upgradeDom();
}