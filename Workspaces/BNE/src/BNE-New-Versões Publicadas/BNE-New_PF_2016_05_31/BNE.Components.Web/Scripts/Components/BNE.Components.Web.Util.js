bne.components.web.util = {
    trim: function trim(valor) {
        return valor.replace(/^\s+|\s+$/g, '');
    },
    focusNext: function focusNext(control, maxLength) {
        var value = $(control).val();

        if (value.length >= maxLength) {
            $(control).blur();
            var inputs = $(control).closest('form').find(':input');

            inputs.eq(inputs.index($(control)) + 1).focus();
        }
    }
};