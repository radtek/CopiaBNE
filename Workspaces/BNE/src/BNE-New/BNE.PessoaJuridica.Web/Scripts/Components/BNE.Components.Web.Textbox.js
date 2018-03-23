bne.components.web.textbox = function (control, required, controlType, maxLength) {
    control = 'input:text[id=' + control + ']';
    validateFormat: function validateFormat(controle, controlType) {
        $.validator.addMethod("validateFormatTextbox", function (value, element, validation) {
            var validationExpression = '';

            switch (validation) {
                case 'site':
                    validationExpression = '(((http|https|ftp)\:\/\/|(www))(((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])|([a-zA-Z0-9_\\-\\.])+\\.(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|uk|me|ind|eng|adv)))((:[a-zA-Z0-9]*)?\/?([a-zA-Z0-9\-\._\?\,\&apos;\/\+&amp;amp;%\$#\=~])*)';
                    break;
                case 'email':
                    validationExpression = '^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$';
                    break;
                case 'nome':
                    validationExpression = '^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç\']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç\']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç\']{1,})*$';
                    break;
                case 'number':
                    validationExpression = '^[0-9]*$';
                    break;
            }

            var valor = value;
            if (valor === '')
                return this.optional(element) || false;

            if (valor.match(validationExpression)) {
                return this.optional(element) || true;
            } else {
                return this.optional(element) || false;
            }
        }, "* Formato inválido.");

        switch (controlType) {
            case bne.components.web.textbox.type.site:
                $(controle).rules("add", { validateFormatTextbox: 'site' });
                break;
            case bne.components.web.textbox.type.email:
                $(controle).rules("add", { validateFormatTextbox: 'email' });
                break;
            case bne.components.web.textbox.type.nome:
                $(controle).rules("add", { validateFormatTextbox: 'nome' });
                break;
            case bne.components.web.textbox.type.number:
                $(controle).rules("add", { validateFormatTextbox: 'number' });
                break;
        }

    }
    validateRequired: function validateRequired(controle, required) {
        $(controle).rules("add", { required: required, messages: { required: 'Obrigatório' } });
    }

    validateRequired(control, required != null && required);
    validateFormat(control, controlType);

    if (maxLength != null && maxLength > 0) {
        $(document.body).on('keyup', control, function () {
            bne.components.web.util.focusNext(control, maxLength);
        });
    }
}

bne.components.web.textbox.type = {
    site: 'site', email: 'email', nome: 'nome', normal: 'normal', number: 'number'
}