bne.components.web.CPF = function (control, required) {
    control = 'input:text[id*=' + control + ']';
    mask: function mask(control) {
        var valor = bne.components.web.util.trim(control.val());
        if (valor === '')
            return;

        if (valor.match("\\d{11}")) {
            valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
            valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
            valor = valor.replace(/(\d{3})(\d)/, "$1-$2");
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
        $.validator.addMethod("validateFormatCPF", function (value, element) {
            var digitosIguais, cpf = value.replace(/\D+/g, '');

            digitosIguais = 1;

            if (cpf.length !== 11) {
                return this.optional(element) || false;
            }
            var i;
            for (i = 0; i < cpf.length - 1; i++) {
                if (cpf.charAt(i) !== cpf.charAt(i + 1)) {
                    digitosIguais = 0;
                    break;
                }
            }

            if (!digitosIguais) {
                var add = 0;
                for (i = 0; i < 9; i++)
                    add += parseInt(cpf.charAt(i)) * (10 - i);

                var rev = 11 - (add % 11);
                if (rev === 10 || rev === 11)
                    rev = 0;

                if (rev !== parseInt(cpf.charAt(9)))
                    return this.optional(element) || false;

                add = 0;
                for (i = 0; i < 10; i++)
                    add += parseInt(cpf.charAt(i)) * (11 - i);
                rev = 11 - (add % 11);
                if (rev === 10 || rev === 11)
                    rev = 0;

                if (rev !== parseInt(cpf.charAt(10))) {
                    return this.optional(element) || false;
                }
                return this.optional(element) || true;
            }
            else {
                return this.optional(element) || false;
            }
        }, "* Formato inválido de CPF");

        $(control).rules("add", { validateFormatCPF: true });
    }
    validateRequired: function validateRequired(control) {
        $(control).rules("add", { required: true, messages: { required: 'Obrigatório' } });
    }
    $(document.body).on('focus', control, function () {
        unmask($(this));
    });
    $(document.body).on('blur', control, function () {
        mask($(this));
    });
    if (required != null && required) {
        validateRequired(control);
    }
    validateFormat(control);
}