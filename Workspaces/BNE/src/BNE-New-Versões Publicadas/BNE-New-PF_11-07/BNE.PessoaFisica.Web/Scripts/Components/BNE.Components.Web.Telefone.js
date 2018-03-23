bne.components.web.telefone = function (ctrl, required, controlType) {
    var dddNoveDigitos = [11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 32, 33, 34, 35, 37, 38, 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99];

    var control = 'input:text[id=' + ctrl + ']';
    removemask: function removemask(value) {
        if (value === '')
            return '';

        value = value.replace('/', '');
        value = value.replace('-', '');
        value = value.replace('(', '');
        value = value.replace(')', '');
        value = value.replace(' ', '');
        value = value.replace(/\./g, '');

        return value;
    }
    mask: function mask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        valor = valor.replace(/\ /g, '');

        if (valor.match(/^(\d{10})$/)) {
            valor = valor.replace(/(\d{2})(\d{4})(\d{4})/, "($1) $2-$3");
        }
        else if (valor.match(/^(\d{11})$/)) {
            valor = valor.replace(/(\d{2})(\d{5})(\d{4})/, "($1) $2-$3");
        }
        else if (valor.match(/^(\d{11})$/) && valor.substring(0, 1) === "0") {
            valor = valor.replace(/(\d{4})(\d{3})(\d{4})/, "$1-$2-$3");
        }
        control.val(valor);
    }
    unmask: function unmask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        valor = removemask(valor);

        control.val(valor);
    }
    expressionValidation: function expressionValidation(ddd, controlType) {
        ddd = parseInt(ddd);

        if (controlType === bne.components.web.telefone.type.celular) {
            if (dddNoveDigitos.indexOf(ddd) >= 0) {
                return bne.components.web.telefone.type.celularNoveDigitos.regex;
            }
            else {
                return bne.components.web.telefone.type.celular.regex;
            }
        }
        else if (controlType === bne.components.web.telefone.type.fixocelular) {
            if (dddNoveDigitos.indexOf(ddd) >= 0) {
                return bne.components.web.telefone.type.fixocelularNoveDigitos.regex;
            }
            else {
                return bne.components.web.telefone.type.fixocelular.regex;
            }
        }
        else if (controlType === bne.components.web.telefone.type.fixo) {
            return bne.components.web.telefone.type.fixo.regex;
        }
        else if (controlType === bne.components.web.telefone.type.outros) {
            return bne.components.web.telefone.type.fixo.regex;
        }
        return '';
    }
    validateFormat: function validateFormat(control) {
        $.validator.addMethod("validateFormatTelefone", function (value, element, controlType) {
            value = removemask(value);

            var ddd = +value.substring(0, 2);
            var fone = value.substring(2, 11);

            if (isNaN(ddd) || isNaN(fone)) {
                return this.optional(element) || false;
            }

            var expression = expressionValidation(ddd, controlType);

            if (fone.match(expression)) {
                return this.optional(element) || true;
            } else {
                return this.optional(element) || false;
            }
        }, "* Formato inválido de telefone");

        $(control).rules("add", { validateFormatTelefone: controlType });
    }
    validateRequired: function validateRequired(control) {
        $(control).rules("add", { required: true, messages: { required: 'Obrigatório' } });
    }
    removerZeroEsquerda: function removerZeroEsquerda(valor) {
        var i;
        for (i = 0; i < valor.length; i++) {
            if (valor.charAt(i) !== '0')
                break;
        }

        return valor.substring(i);
    }
    $(document.body).on('focus', control, function () {
        unmask($(this));
        $(this).attr('placeholder', '(XX) XXXX-XXXX');
    });
    $(document.body).on('blur', control, function () {
        mask($(this));
        $(this).attr('placeholder', '');
    });
    if (required != null && required) {
        validateRequired(control);
    }
    validateFormat(control);

    $(document.body).on('input', control, function () {
        var maxLength = 10;
        var value = removemask($(control).val());
        var ddd = +value.substring(0, 2);

        if (controlType === bne.components.web.telefone.type.celular) {
            if (dddNoveDigitos.indexOf(ddd) >= 0) {
                maxLength = 11;
            }
        }
        else if (controlType === bne.components.web.telefone.type.fixocelular) {
            if (dddNoveDigitos.indexOf(ddd) >= 0) {
                maxLength = 11;
            }
        }

        bne.components.web.util.focusNext(control, maxLength);
    });


}
bne.components.web.telefone.type = {
    outros: {
        index: 0,
        name: "Outros",
        regex: "^[2-9]{1}\d{3}\-{0,1}\d{4}$|^[0][1-9]\d{2}\-\d{3}\-\d{4}$|^[0][1-9]\d{2}\d{7}$",
        size: 11
    },
    fixo: {
        index: 1,
        name: "Fixo",
        regex: "^[2-5]{1}\\d{3}\\-{0,1}\\d{4}$",
        size: 8
    },
    celular: {
        index: 2,
        name: "Celular",
        regex: "^[6-9]{1}\\d{3}\\-{0,1}\\d{4}$",
        size: 8
    },
    celularNoveDigitos: {
        index: 2,
        name: "Celular",
        regex: "^[9]\\d{4}\\-{0,1}\\d{4}|[7]{1}\\d{3}\\-{0,1}\\d{4}$",

        size: 9
    },
    fixocelular: {
        index: 3,
        name: "FixoCelular",
        regex: "^[2-5]{1}\\d{3}\\-{0,1}\\d{4}|[6-9]{1}\\d{3}\\-{0,1}\\d{4}$"
    },
    fixocelularNoveDigitos: {
        index: 3,
        name: "FixoCelular",
        regex: "^[2-5]{1}\\d{3}\\-{0,1}\\d{4}|[9]\\d{4}\\-{0,1}\\d{4}|[7]{1}\\d{3}\\-{0,1}\\d{4}$"
    }
}