employer.controle.mvc = {
    /// <summary>
    /// Retorna a versão da Framework.
    /// </summary>
    VERSION: function () {
        return 1.0;
    },
    // MMDDYYYY
    VERSION_DATE: function () {
        /// <summary>
        /// Retorna a data de alteração da versão da Framework.
        /// </summary>    
        return new Date("25-04-2012");
    },

    //Exemplo
    //$(function () {
    //    employer.controle.mvc.aplicarBehaviorListaSugestao("txtTesteLista",
    //        [{ key: 1, value: " Masculino" }, { key: 2, value: "Feminino"}],
    //        employer.controle.mvc.listaTipo.Numero);
    //});
    aplicarBehaviorListaSugestao: function (control, lista, tipoSugestao, callbackChange, cssPnlOculto, csslblDescricao) {
        var campo = employer.util.findControl(control);

        var componente = new employer.controle.mvc.lista(campo, lista, tipoSugestao, callbackChange, cssPnlOculto, csslblDescricao);
    },

    aplicarBehaviorCNPJ: function (control) {
        employer.util.findControl(control).on('blur', function () {
            employer.controle.mvc.cnpj.aplicarMascaraCNPJ(this);
        });
        employer.util.findControl(control).on('focus', function () {
            employer.controle.removerMascaraControle(this, '\\d{2}.\\d{3}.\\d{3}/\\d{4}-\\d{2}');
        });
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event, 14);
        });
        //employer.util.findControl(control).keyup(function () {
        //    employer.controle.moveToNextElement(this, event, 14);
        //});
    },
    aplicarBehaviorCPF: function (control) {
        employer.util.findControl(control).on('blur', function () {
            employer.controle.mvc.cpf.aplicarMascaraCPF(this);
        });
        employer.util.findControl(control).on('focus', function () {
            employer.controle.removerMascaraControle(this, '\\d{3}.\\d{3}.\\d{3}-\\d{2}');
        });
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event, 11);
        });
        //employer.util.findControl(control).keyup(function () {
        //    employer.controle.moveToNextElement(this, event, 11);
        //});
    },
    aplicarBehaviorData: function (control, callbackBlur) {
        var dataAtual = '';

        $.ajax({
            url: '/Base/RecuperarDataAtual',
            dataType: "json",
            success: function(data) {
                dataAtual = data;
            }
        });

        employer.util.findControl(control).on('blur', function () {
            employer.controle.mvc.data.aplicarMascaraData(this, dataAtual);
            if (callbackBlur != undefined) {
                callbackBlur(this);
            }
        });
        employer.util.findControl(control).on('focus', function () {
            employer.controle.removerMascaraControle(this, '(^(0[1-9]|[12][0-9]|3[01])[/.](0[1-9]|1[012])[/.](\\d{4})$)|(\\d{8})');
        });
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event, dataAtual);
        });
        //employer.util.findControl(control).keyup(function () {
        //    employer.controle.moveToNextElement(this, event, employer.controle.recuperarTamanhoData(this));
        //});
    },
    aplicarBehaviorTelefone: function (control) {
        employer.util.findControl(control).on('blur', function () {
            employer.controle.mvc.telefone.aplicarMascaraFone(this);
        });
        employer.util.findControl(control).on('focus', function () {
            employer.controle.removerMascaraControle(this, '(\\d{4}-\\d{4})|(\\d{5}-\\d{4})|(\\d{4}-\\d{3}-\\d{4})');
        });
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event, 9);
        });
        //employer.util.findControl(control).keyup(function () {
        //    employer.controle.moveToNextElement(this, event, 8);
        //});
    },
    aplicarBehaviorTelefoneDD: function (control) {
        employer.util.findControl(control).on('blur', function () {
            employer.controle.mvc.telefone.aplicarMascaraDD(this);
        });
        employer.util.findControl(control).on('focus', function () {
            employer.controle.removerMascaraControle(this, '\\d{1,3}');
        });
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event);
        });
        //employer.util.findControl(control).keyup(function () {
        //    employer.controle.moveToNextElement(this, event, 2);
        //});
    },
    aplicarBehaviorCEP: function (control) {
        employer.util.findControl(control).on('blur', function () {
            employer.controle.mvc.cep.aplicarMascaraCEP(this);
        });
        employer.util.findControl(control).on('focus', function () {
            employer.controle.removerMascaraControle(this, '\\d{5}-\\d{3}');
        });
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event, 8);
        });
    },
    aplicarBehaviorNumerico: function (control) {
        employer.util.findControl(control).on('keypress', function (event) {
            return employer.controle.apenasNumeros(this, event);
        });
    }
};


