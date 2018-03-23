bne.components.web.textbox = function (control, required, controlType) {
    var currentControl = 'input:text[id*=' + control + ']';
    validateFormat: function validateFormat(controle, controlType) {

        $.validator.addMethod("validateFormatTextbox", function (value, element, validation) {

            var validationExpression = '';

            switch (validation) {
                case 'site':
                    validationExpression = '(((http|https|ftp)\:\/\/|(www))(((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])|([a-zA-Z0-9_\\-\\.])+\\.(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|uk|me|ind)))((:[a-zA-Z0-9]*)?\/?([a-zA-Z0-9\-\._\?\,\&apos;\/\+&amp;amp;%\$#\=~])*)';
                    break;
                case 'email':
                    validationExpression = '^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$';
                    break;
                case 'nome':
                    validationExpression = '^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç\']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç\']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç\']{1,})*$';
                    break;
                case 'numero':
                    validationExpression = '^[0-9]+$';
                    break;
            }

            var valor = value.replace(/\s+$/, '');
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
            case bne.components.web.textbox.type.numero:
                $(controle).rules("add", { validateFormatTextbox: 'numero' });
                break;
            case bne.components.web.textbox.type.meses:
                $(controle).rules("add", { range: [0, 11], messages: { range: jQuery.validator.format("Preencha o valor entre {0} e {1}.") } });
                break;
        }

    }
    validateRequired: function validateRequired(controle, required) {
        $(controle).rules("add", { required: required, messages: { required: 'Obrigatório' } });            
    }
    validateRequired(currentControl, required != null && required);
    validateFormat(currentControl, controlType);
    formatarAno: function formatarAno(control) {
        var ano = control.val();
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
        return control.val(ano);
    };
    somenteNumeros = function (control) {
        var expre = /[^0-9]/g;
        var ret;
        
        if (control.val().match(expre)) {
            ret = control.val().replace(expre, '')
        }

        if (ret == null)
            control.val(control.val())
        else
            control.val(ret);
    };
    $(document.body).on('keyup', currentControl, function () {
        if (controlType == 'numero' || controlType == 'ano')
            somenteNumeros($(this));
    });
    $(document.body).on('blur', currentControl, function () {
        if (controlType == 'ano')
            formatarAno($(this));
    });
}

bne.components.web.textbox.type = {
    site: 'site', email: 'email', nome: 'nome', normal: 'normal', numero: 'numero', ano:'ano', meses: 'meses'
}