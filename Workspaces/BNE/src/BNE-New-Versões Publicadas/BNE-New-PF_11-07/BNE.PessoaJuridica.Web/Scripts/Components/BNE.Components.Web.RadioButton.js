bne.components.web.radiobutton = function (control, required) {
    control = 'input[id="' + control + '"]';
    validateRequired: function validateRequired(control, required) {
        $.validator.addMethod("validateRequiredRadioButton", function (value, control) {
            var name = $(control).attr('name');
            var retorno = false;

            $('input[name="' + name + '"]:radio').each(function () {
                if ($(this).is(":checked"))
                    retorno = true;
            });

            return retorno;
        }, "Obrigatório");

        if (required) {
            $(control).rules("add", { validateRequiredRadioButton: true });
        } else {
            $(control).rules("add", { validateRequiredRadioButton: false });

        }
    }
    validateRequired(control, required != null && required);
}

bne.components.web.radiobutton2 = function (label, required) {
    label = 'label[for="' + label + '"]';
    var checkboxid = $(label).attr('for');
    var checkbox = 'input[id="' + checkboxid + '"]';

    validateRequired: function validateRequired(control, required) {
        $.validator.addMethod("validateRequiredRadioButton", function (value, control) {
            var name = $(control).attr('name');
            var retorno = false;

            $('input[name="' + name + '"]:radio').each(function () {
                if ($(this).is(":checked"))
                    retorno = true;
            });

            return retorno;
        }, "Obrigatório");

        if (required) {
            $(control).rules("add", { validateRequiredRadioButton: true });
        } else {
            $(control).rules("add", { validateRequiredRadioButton: false });

        }
    }

    validateRequired(checkbox, required != null && required);
    
    $(document.body).on('click', label, function () {
        var checkbox = $('input[id="' + checkboxid + '"]');
        var checkboxname = $(checkbox).attr('name');

        //Zerando todos os rb
        $('input[name="' + checkboxname + '"]:radio').each(function () {
            var id = $(this).attr('id');

            $('label[for="' + id + '"]').removeClass('is-checked');
            $(this).removeAttr('checked');
        });

        $(this).addClass('is-checked');
        checkbox.attr('checked', 'checked');
    });
}

