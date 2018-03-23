employer.controle.mvc.cep = {
    aplicarMascaraCEP: function (control) {
        var valor = employer.util.trim(control.value);

        if (valor == '')
            return;

        if (valor.match(/^(\d{5})(\d{3})$/))
            valor = valor.replace(/(\d{5})(\d{3})/, "$1-$2");

        control.value = valor;
    }
};