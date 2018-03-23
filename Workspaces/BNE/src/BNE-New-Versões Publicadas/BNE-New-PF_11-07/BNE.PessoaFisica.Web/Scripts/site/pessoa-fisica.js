function ConfigurarEtapa1() {

    $('#Etapa1Form').validate();

    bne.components.web.textbox('txtNome', true, bne.components.web.textbox.type.nome);
    bne.components.web.autocomplete.email('txtEmail', false, true);
    bne.components.web.autocomplete.email('txtConfirmarEmail', false, true);
    bne.components.web.autocomplete.cidade('txtCidade', true, false);
    bne.components.web.autocomplete.funcao('txtFuncao', true, false);
    bne.components.web.radiobutton('option-m', true);

    bne.components.web.decimal('txtPretensaoSalarial', true, bne.components.web.textbox.type.numero, true);
    bne.components.web.telefone('txtCelular', true, bne.components.web.telefone.type.celular);
    bne.components.web.textbox('txtTempoExperienciaAnos', false, bne.components.web.textbox.type.numero);
    bne.components.web.textbox('txtTempoExperienciaMeses', false, bne.components.web.textbox.type.meses);

    bne.components.web.CPF('txtCPF', true);
    bne.components.web.data('txtDataNascimento', true);

    $('#ddlDeficiencia').hide();

    $("input[name='def']").click(function () {
        var temDeficiencia = $(this).val();

        if(temDeficiencia == 's')
        {
            $('#ddlDeficiencia').show();
        }
        else {
            $('#ddlDeficiencia').hide();
        }
    });

    componentHandler.upgradeDom();

}         