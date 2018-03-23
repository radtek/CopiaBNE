bne.components.web.data = function (control, required) {
    control = 'input:text[id=' + control + ']';
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
            var datapassado = new Date(Date.parse(mes+"/"+ dia+"/"+ano));
            if (datapassado == "Invalid Date") {
                valido = false;
            }

            var novaData = new Date(ano, (mes - 1), dia);
            var dataMinima = new Date(1900, 0, 1);

            var mesmoDia = parseInt(dia, 10) === parseInt(novaData.getDate());
            var mesmoMes = parseInt(mes, 10) === parseInt(novaData.getMonth()) + 1;
            var mesmoAno = parseInt(ano) === parseInt(novaData.getFullYear());

            if (!((mesmoDia) && (mesmoMes) && (mesmoAno))) {
                valido = false;
            }
            return this.optional(element) || valido;
        }, "* Formato inválido de data");

        $.validator.addMethod("validateFormatDataRange", function (value, element) {
            value = value.replace(/\//g, '');

            var valido = true;

            var dia = value.substring(0, 2);
            var mes = value.substring(2, 4);
            var ano = value.substring(4, 8);

            var datapassado = new Date(Date.parse(mes+"/"+ dia+"/"+ano));
            var ageDifMs = Date.now() - datapassado.getTime();
            var ageDate = new Date(ageDifMs);
            var idade = Math.abs(ageDate.getFullYear() - 1970);
            if (idade > 100 || idade < 14) {
                valido = false;
            }

            return this.optional(element) || valido;
        }, "* A data deve estar entre 14 e 100 anos");

        $(control).rules("add", { validateFormatData: true });
        $(control).rules("add", { validateFormatDataRange: true });

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