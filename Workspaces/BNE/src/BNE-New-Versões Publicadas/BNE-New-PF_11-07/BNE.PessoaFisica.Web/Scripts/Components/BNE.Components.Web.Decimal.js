//Formatar e validar um campo Decimal em R$
bne.components.web.decimal = function (control, required, controlType, validarSalarioMinimo) {
    control = 'input:text[id*=' + control + ']';
    removemask: function removemask(control){
        value = control.val()
        if (value === '')
            return '';

        value = value.replace('/', '');
        value = value.replace('-', '');
        value = value.replace('(', '');
        value = value.replace(')', '');
        value = value.replace(' ', '');
        value = value.replace(/\./g, '');

        //if (/[,00]/.test(value))
        //{
        //    value = value.replace(',00', '');
        //}

        value = value.replace(/\,00/g, '');

        control.val(value);
    }
    mask: function mask(control) {
        var valor = bne.components.web.util.trim(control.val());

        if (valor === '')
            return '';

        valor = valor.replace(/\ /g, '');

        somenteNumeros(control);

        valor = control.val();

        if (valor.match(/[^0-9]*/g)) {
            if (valor.length >= 8) {

                valor1 = valor.replace(/^\$?([0-9]{1,3}([0-9]{3})*[0-9]{3}|[0-9]+)([0-9][0-9])?$/, "$1");
                valor2 = valor.replace(/^\$?([0-9]{1,3}([0-9]{3})*[0-9]{3}|[0-9]+)([0-9][0-9])?$/, "$3")
                valor = valor1.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") + ',' + valor2;
            }
            else {
                valor = valor.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
                valor = valor + ',00';
            }
        }

        control.val(valor);
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
    validarPretensaoMaiorqueSalarioMinimo = function validarPretensaoMaiorqueSalarioMinimo (control) {

        $.validator.addMethod("validarPretensaoMaiorqueSalarioMinimo", function (value, element) {

            var validado = false;
            var valordoMinimo = (Math.round(parseFloat('880')*100)/100);
            var pretensaoSalarial = (Math.round(parseFloat(value.replace('.', '')) * 100) / 100);

            //if (parseFloat(pretensaoSalarial) >= parseFloat(valordoMinimo)) {
            if (pretensaoSalarial >= valordoMinimo) {
                validado = true;
            }
            return validado;
        }, "* Sálario não pode ser menor que o sálario mínimo de R$880,00");

        $(control).rules("add", { validarPretensaoMaiorqueSalarioMinimo: true });
    }
    validateRequired: function validateRequired(control) {
        $(control).rules("add", { required: true, messages: { required: 'Obrigatório' } });
    }
    $(document.body).on('blur', control, function () {
        mask($(this));
    });
    $(document.body).on('focus', control, function () {
        removemask($(this));
    });
    $(document.body).on('keyup', control, function () {
        somenteNumeros($(this));
    });
    if (required != null && required) {
        validateRequired(control);
    }

    if (validarSalarioMinimo)
        validarPretensaoMaiorqueSalarioMinimo(control);
}