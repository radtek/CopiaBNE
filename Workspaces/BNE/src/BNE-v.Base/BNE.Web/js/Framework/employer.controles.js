// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Minify' para comprimir e atualizar o arquivo /min/employer.controles-min.js!


/// <reference path="employer.js" />

var Controle = {
    Interno: '_txtValor',
    Telerik: '_Input'
}

var ControlType = {
    Aspnet: 'aspnet',
    Component: 'component',
    Telerik: 'telerik'
}

employer.controles = {

    lastFocus: function () {
        /// <summary>
        /// Recupera o ID do controle com último foco.
        /// </summary> 
        return employer.util.findControl('hfUltimoFoco').val();
    },

    setFocus: function (control) {
        /// <summary>
        /// Define o foco para um campo.
        /// </summary>
        employer.util.findControl(control).focus();
    },

    setFocusControle: function (control) {
        /// <summary>
        /// Define o foco para um controle.
        /// </summary>
        employer.util.findControl(control + Controle.Interno).focus();
    },

    recuperarValor: function (control) {
        /// <summary>
        /// Recupera o valor de um controle.
        /// </summary> 
        return employer.util.findControl(control).val();
    },

    recuperarValorControle: function (control) {
        /// <summary>
        /// Recupera o valor de um servercontrol.
        /// </summary>
        return employer.controles.recuperarValor(control + Controle.Interno);
    },

    setValor: function (control, value) {
        /// <summary>
        /// Define o valor de um controle.
        /// </summary>
        employer.util.findControl(control).val(value);
    },

    setValorControle: function (control, value) {
        /// <summary>
        /// Define o valor de um servercontrol.
        /// </summary>
        employer.controles.setValor(control + Controle.Interno, value);
    },

    limparValor: function (control) {
        /// <summary>
        /// Limpa o valor de um campo input.
        /// </summary> 
        employer.util.findControl(control).val("");
    },

    limparValorControle: function (control) {
        /// <summary>
        /// Limpa o valor de um controle.
        /// </summary>
        employer.controles.limparValor(control + Controle.Interno);
    },

    enable: function (control, status) {
        /// <summary>
        /// Habilita/Desabilita um controle.
        /// </summary> 
        employer.util.findControl(control).attr("disabled", !status);
    },

    enableControle: function (control, status) {
        /// <summary>
        /// Habilita/Desabilita um servercontrol.
        /// </summary>
        employer.controles.enable(control, status);
        employer.controles.enable(control + Controle.Interno, status);
    },

    enableControleTlk: function (control, status) {
        /// <summary>
        /// Habilita/Desabilita um servercontrol.
        /// </summary>
        employer.controles.enable(control, status);
        employer.controles.enable(control + Controle.Telerik, status);
    },

    enableValidatorControleVal: function (control, validator, status, validar) {
        /// <summary>
        /// Habilita/Desabilita o validador de um Controle.
        /// </summary>
        employer.controles.enableValidatorVal(control + "_" + validator, status, validar);
    },

    enableValidatorControle: function (control, validator, status) {
        /// <summary>
        /// Habilita/Desabilita o validador de um Controle.
        /// </summary>
        employer.controles.enableValidator(control + "_" + validator, status);
    },

    enableValidatorVal: function (validator, status, validar) {
        /// <summary>
        /// Habilita/Desabilita um validador.
        /// </summary>
        var control = employer.util.findControl(validator)[0];
        control.enable = status;

        if (!validar)
            return;

        ValidatorEnable(control, status);
        ValidatorValidate(control);
    },

    enableValidator: function (validator, status) {
        /// <summary>
        /// Habilita/Desabilita um validador.
        /// </summary>
        var control = employer.util.findControl(validator)[0];
        control.enable = status;

        ValidatorEnable(control, status);
    },

    setAttr: function (control, attr, valor) {
        /// <summary>
        /// Define um atributo a um controle.
        /// </summary>
        employer.util.findControl(control).attr(attr, valor);
    },

    isEnable: function (control) {
        /// <summary>
        /// Verifica se um controle está Habilitado/Desabilitado.
        /// </summary>
        return !employer.util.findControl(control).attr("disabled");
    },

    isEnableControle: function (control) {
        /// <summary>
        /// Verifica se um servercontrol está Habilitado/Desabilitado.
        /// </summary>
        return employer.controles.isEnable(control + Controle.Interno);
    },

    enableComponenteTelefone: function (control, status, clearvalue) {
        /// <summary>
        /// Habilita/Desabilita/Limpa o componente de Telefone
        /// </summary>
        employer.controles.enable(control + "_txtDDD", status);
        employer.controles.enable(control + "_txtFone", status);

        if (!clearvalue)
            return;

        employer.controles.limparValor(control + "_txtDDD");
        employer.controles.limparValor(control + "_txtFone");
    },

    setValorComponenteTelefone: function (control, ddd, telefone) {
        /// <summary>
        /// Define os valores do controle de Telefone.
        /// </summary>
        employer.controles.setValor(control + "_txtDDD", ddd);
        employer.controles.setValor(control + "_txtFone", telefone);
    },

    enableControlesArr: function (controles, status) {
        /// <summary>
        /// Habilita/Desabilita uma lista de controles sob-demanda
        /// </summary>
        for (i = 0; i < controles.length; i++) {
            if (controles[i].tipo == ControlType.Aspnet)
                employer.controles.enable(controles[i].id, !status ? controles[i].enable : status);
            else if (controles[i].tipo == ControlType.Component)
                employer.controles.enableControle(controles[i].id, !status ? controles[i].enable : status);
            else if (controles[i].tipo == ControlType.Telerik)
                employer.controles.enableControleTlk(controles[i].id, !status ? controles[i].enable : status);
        }

        controles.length = 0; // Remove o Array da Memória
    },

    enableValidatorsControlesArr: function (controles, status, validar) {
        /// <summary>
        /// Habilita/Desabilita uma lista de validators de controles sob-demanda
        /// </summary>
        if (validar) {
            for (i = 0; i < controles.length; i++) {
                if (controles[i].tipo == ControlType.Aspnet)
                    employer.controles.enableValidatorVal(controles[i].id, !status ? controles[i].enable : status);
                else if (controles[i].tipo == ControlType.Component)
                    employer.controles.enableValidatorControleVal(controles[i].id, !status ? controles[i].enable : status);
            }
        }
        else {
            for (i = 0; i < controles.length; i++) {
                if (controles[i].tipo == ControlType.Aspnet)
                    employer.controles.enableValidator(controles[i].id, !status ? controles[i].enable : status);
                else if (controles[i].tipo == ControlType.Component)
                    employer.controles.enableValidatorControle(controles[i].id, !status ? controles[i].enable : status);
            }
        }

        controles.length = 0; // Remove o Array da Memória
    }
};