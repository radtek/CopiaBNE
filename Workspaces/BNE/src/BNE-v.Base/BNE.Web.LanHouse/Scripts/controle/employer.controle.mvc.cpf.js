employer.controle.mvc.cpf = {
    aplicarMascaraCPF: function (control) {
        var valor = employer.util.trim(control.value);    
        if (valor == '')
            return;

        if (valor.match(/^(\d{11})$/)) {
            valor = valor.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        }
        control.value = valor;
    }
};