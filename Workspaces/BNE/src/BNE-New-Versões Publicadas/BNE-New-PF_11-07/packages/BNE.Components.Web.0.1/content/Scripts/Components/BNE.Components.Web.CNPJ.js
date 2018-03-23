bne.components.web.CNPJ = function (control, required) {
    control = 'input:text[id*=' + control + ']';
    mask: function mask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        if (valor.match("\\d{14}")) {
            valor = valor.replace(/(\d{2})(\d)/, "$1.$2");
            valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
            valor = valor.replace(/(\d{3})(\d)/, "$1/$2");
            valor = valor.replace(/(\d{4})(\d)/, "$1-$2");
        }
        control.val(valor);
    };
    unmask: function unmask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        valor = valor.replace('/', '');
        valor = valor.replace('-', '');
        valor = valor.replace(/\./g, '');

        control.val(valor);
    }
    validateFormat: function validateFormat(control) {
        $.validator.addMethod("validateFormatCNPJ", function (value, element) {
            var numeros, digitos, soma, i, resultado, pos, tamanho, digitosIguais, cnpj = value.replace(/\D+/g, '');

            digitosIguais = 1;
            if (cnpj.length !== 14) {
                return false;
            }

            for (i = 0; i < cnpj.length - 1; i++) {
                if (cnpj.charAt(i) !== cnpj.charAt(i + 1)) {
                    digitosIguais = 0;
                    break;
                }
            }
            if (!digitosIguais) {
                tamanho = cnpj.length - 2;
                numeros = cnpj.substring(0, tamanho);
                digitos = cnpj.substring(tamanho);
                soma = 0;
                pos = tamanho - 7;
                for (i = tamanho; i >= 1; i--) {
                    soma += numeros.charAt(tamanho - i) * pos--;
                    if (pos < 2) {
                        pos = 9;
                    }
                }
                resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
                if (resultado.toString() !== digitos.charAt(0)) {
                    return false;
                }

                tamanho = tamanho + 1;
                numeros = cnpj.substring(0, tamanho);
                soma = 0;
                pos = tamanho - 7;
                for (i = tamanho; i >= 1; i--) {
                    soma += numeros.charAt(tamanho - i) * pos--;
                    if (pos < 2)
                        pos = 9;
                }
                resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
                if (resultado.toString() !== digitos.charAt(1)) {
                    return this.optional(element) || false;
                }
                else {
                    return this.optional(element) || true;
                }
            }
            else {
                return this.optional(element) || false;
            }
        }, "* Formato inválido de CNPJ");

        $(control).rules("add", { validateFormatCNPJ: true });
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
        bne.components.web.util.focusNext($(control), 14);
    });

}