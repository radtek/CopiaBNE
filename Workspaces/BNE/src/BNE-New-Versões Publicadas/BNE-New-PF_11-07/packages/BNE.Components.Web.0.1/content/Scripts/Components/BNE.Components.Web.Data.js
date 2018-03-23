bne.components.web.data = function (control, required) {
    control = 'input:text[id*=' + control + ']';
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
    validateFormat: function validateFormat(control) {
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

            var mesmoDia = parseInt(dia, 10) === parseInt(novaData.getDate());
            var mesmoMes = parseInt(mes, 10) === parseInt(novaData.getMonth()) + 1;
            var mesmoAno = parseInt(ano) === parseInt(novaData.getFullYear());

            if (!((mesmoDia) && (mesmoMes) && (mesmoAno)))
                valido = false;

            return this.optional(element) || valido;
        }, "* Formato inválido de data");

        $(control).rules("add", { validateFormatData: true });
    }
    validateRequired: function validateRequired(control, required) {
        $(control).rules("add", { required: required, messages: { required: 'Obrigatório' } });
    }
    $(document.body).on('focus', control, function () {
        unmask($(this));
    });
    $(document.body).on('blur', control, function () {
        mask($(this));
    });
        
    validateRequired(control, required != null && required);
    validateFormat(control);

    $(document.body).on('keyup', control, function () {
        bne.components.web.util.focusNext(control, 8);
    });

}