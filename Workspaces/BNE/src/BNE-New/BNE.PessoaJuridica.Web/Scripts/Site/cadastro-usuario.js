$(document).ready(function () {
    Configurar();
});

function Configurar() {
    $("#CadastroUsuarioForm").validate();

    bne.components.web.textbox('txtNomeResponsavel', true, bne.components.web.textbox.type.nome);
    bne.components.web.CPF('txtCPFResponsavel', true);
    bne.components.web.data('txtDataNascimentoResponsavel', true);
    bne.components.web.autocomplete.funcao('txtFuncaoResponsavel', true, false);
    bne.components.web.telefone('txtNumeroCelularResponsavel', true, bne.components.web.telefone.type.celular);
    bne.components.web.textbox('txtEmailResponsavel', true, bne.components.web.textbox.type.email);
    bne.components.web.telefone('txtNumeroComercialResponsavel', true, bne.components.web.telefone.type.fixo);
    bne.components.web.radiobutton('option-f', true);

    componentHandler.upgradeDom();
}

var onSuccess = function (result) {
    if (result.url) {
        window.location.href = result.url;
    }
}