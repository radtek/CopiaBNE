bne.components.web.data = function (ctrl, required, controlType, validarDataRange) {
    var control = 'input:text[id=' + ctrl + ']';
    retornarAno: function retornarAno(ano) {
        if (ano != null) {
            if (ano.toString().length < 4) {
                if (ano >= 30 && ano <= 99)
                    ano = "1900".substring(0, 4 - ano.length) + ano;
                else
                    ano = "2000".substring(0, 4 - ano.length) + ano;
            }
            else
                ano = ano.substring(0, 4);
        }
        return ano;
    }
    mask: function mask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        if (valor.match(/^(\d{6})$/)) {
            valor = valor.replace(/(\d{2})(\d{2})(\d{2})/, "$1/$2/" + retornarAno(valor.replace(/(\d{2})(\d{2})(\d{2})/, "$3")));
        }
        else if (valor.match(/^(\d{8})$/)) {
            valor = valor.replace(/(\d{2})(\d{2})(\d{4})/, "$1/$2/$3");
        }

        control.val(valor);
    };
    unmask: function unmask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        valor = valor.replace(/\//g, '');

        control.val(valor);
    }
    validateFormat: function validateFormat(control, controlType) {
        $.validator.addMethod("validateFormatData", function (value, element) {

            value = value.replace(/\//g, '');

            var valido = true;

            var dia = value.substring(0, 2);
            var mes = value.substring(2, 4);
            var ano = value.substring(4, 8);

            if (value.match(/^(\d{6})$/)) {
                ano = retornarAno(value.replace(/(\d{2})(\d{2})(\d{2})/, "$3"));
            }

            var novaData = new Date(ano, (mes - 1), dia);
            var dataMinima = new Date(1900, 0, 1);

            var mesmoDia = parseInt(dia, 10) === parseInt(novaData.getDate());
            var mesmoMes = parseInt(mes, 10) === parseInt(novaData.getMonth()) + 1;
            var mesmoAno = parseInt(ano) === parseInt(novaData.getFullYear());
            
            if (!((mesmoDia) && (mesmoMes) && (mesmoAno)) || novaData < dataMinima)
                valido = false;

            return this.optional(element) || valido;
        }, "* Formato inválido de data");
        $(control).rules("add", { validateFormatData: true });
    }
    validateRequired: function validateRequired(control, required) {
        $(control).rules("add", { required: required, messages: { required: 'Obrigatório' } });
    }
    validarDataMaiorqueHoje: function validarDataMaiorqueHoje(control) {
        var diasLimiteDataSaida = 60;
        $.validator.addMethod("validarDataMaiorqueHoje", function (value, element) {

            value = value.replace(/\//g, '');

            var dia = value.substring(0, 2);
            var mes = value.substring(2, 4);
            var ano = value.substring(4, 8);

            var dataEntrada = new Date(ano, mes - 1, dia);
            var hoje = new Date();
            var hojeMaisDiasLimiteSaida = new Date(hoje.setDate(hoje.getDate() + diasLimiteDataSaida));

            if (dataEntrada > hojeMaisDiasLimiteSaida) {
                return false;
            } else {
                return true;
            }
        }, "* Data não pode ser maior que " + diasLimiteDataSaida + " dias");
        $(control).rules("add", { validarDataMaiorqueHoje: true });
    };
    validarIdadeMinima: function validarIdadeMinima(control) {
        $.validator.addMethod("validarIdadeMinima", function (value, element) {
            value = value.replace(/\//g, '');

            var dia = value.substring(0, 2);
            var mes = value.substring(2, 4);
            var ano = value.substring(4, 8);

            var dataNascimento = new Date(ano, mes - 1, dia);
            var hoje = new Date();
            var idadeMinima = new Date(hoje.getFullYear() - 13, hoje.getMonth() - 1, hoje.getDay());

            if (dataNascimento > idadeMinima)
                return false;
            else
                return true;

        }, "* Data de nascimento inválida.");
        $(control).rules("add", { validarIdadeMinima: true });
    };
    validarDataSaidaEntrada: function validarDataSaidaEntrada(control, validarDataRange) {
        $.validator.addMethod("validarDataSaidaEntrada", function (value, element) {
            var dataEntrada = $('#txtDataEntrada').val().replace(/\//g, '');
            var dataSaida = value.replace(/\//g, '');

            var diaE = dataEntrada.substring(0, 2);
            var mesE = dataEntrada.substring(2, 4);
            var anoE = dataEntrada.substring(4, 8);

            var diaS = dataSaida.substring(0, 2);
            var mesS = dataSaida.substring(2, 4);
            var anoS = dataSaida.substring(4, 8);

            dataEntrada = new Date(anoE, mesE - 1, diaE);
            dataSaida = new Date(anoS, mesS - 1, diaS);

            if (dataEntrada > dataSaida) {
                return false;
            } else {
                return true;
            }
        }, "* Data de saída não pode ser menor que a data de entrada.");
        $(control).rules("add", { validarDataSaidaEntrada: validarDataRange });
    };
    $(document.body).on('focus', control, function () {
        unmask($(this));
    });
    $(document.body).on('blur', control, function () {
        mask($(this));
    });

    if (controlType === 'dataEntrada' || controlType === 'dataSaida')
        validarDataMaiorqueHoje(control);

    if (controlType === 'dataSaida')
        validarDataSaidaEntrada(control, validarDataRange);

    if (controlType === 'dataNascimento')
        validarIdadeMinima(control);

    validateRequired(control, required != null && required);
    validateFormat(control, controlType);

    $(document.body).on('keyup', control, function () {
        bne.components.web.util.focusNext(control, 8);
    });

}

bne.components.web.data.type = {
    dataEntrada: 'dataEntrada', dataSaida: 'dataSaida', dataNascimento: 'dataNascimento'
}