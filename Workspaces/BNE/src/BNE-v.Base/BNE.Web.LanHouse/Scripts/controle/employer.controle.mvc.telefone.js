employer.controle.mvc.telefone = {
    aplicarMascaraDD: function (control) {
        var valor = employer.util.trim(control.value);
        if (valor == '')
            return;
        if (valor.match("\\d{1,3}")) {
            valor = employer.controle.mvc.telefone.removerZeroEsquerda(valor);
            valor = valor.substring(0, 2);
        }
        control.value = valor;
    },
    aplicarMascaraFone: function (control) {
        var valor = employer.util.trim(control.value);
        if (valor == '') { return; }
        if (valor.match(/^(\d{8})$/)) {
            valor = valor.replace(/(\d{4})(\d{4})/, "$1-$2");
        }
        else if (valor.match(/^(\d{9})$/)) {
            valor = valor.replace(/(\d{5})(\d{4})/, "$1-$2");
        }
        else if (valor.match(/^(\d{11})$/) && valor.substring(0, 1) == "0") {
            valor = valor.replace(/(\d{4})(\d{3})(\d{4})/, "$1-$2-$3");
        }
        control.value = valor;
    },
    removerZeroEsquerda: function (valor) {
        for (i = 0; i < valor.length; i++) {
            if (valor.charAt(i) != '0')
                break;
        }

        return valor.substring(i);
    }



};