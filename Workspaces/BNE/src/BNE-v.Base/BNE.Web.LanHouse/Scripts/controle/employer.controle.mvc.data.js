employer.controle.mvc.data = {
    aplicarMascaraData: function (control, dataAtual) {
        var valor = employer.util.trim(control.value);
        if (valor == '')
            return;

        control.value = employer.controle.mvc.data.definirMascaraData(valor, dataAtual);
    },
    definirMascaraData: function (data, dataAtual) {
        if (data.match("\\d{1,2}(/)*(\\d{1,2})*(/)*(\\d{1,})*")) {
            arr = dataAtual.split('/');
            dia = arr[0];
            mes = arr[1];
            ano = arr[2];

            arr = data.split('/');
            if (arr.length == 1) {
                if (arr[0].length != 0) {
                    if (arr[0].length <= 2)
                        dia = arr[0];
                    else {
                        if (arr[0].length <= 4) {
                            dia = arr[0].substring(0, 2);
                            mes = arr[0].substring(2);
                        }
                        else {
                            dia = arr[0].substring(0, 2);
                            mes = arr[0].substring(2, 4);
                            ano = arr[0].substring(4);
                        }
                    }
                }
            }
            else {
                if (arr.length == 2) {
                    if (arr[0].length != 0)
                        dia = arr[0];
                    if (arr[1].length != 0)
                        mes = arr[1];
                }
                else {
                    if (arr[0].length != 0)
                        dia = arr[0];
                    if (arr[1].length != 0)
                        mes = arr[1];
                    if (arr[2].length != 0)
                        ano = arr[2];
                }
            }

            if (dia.length < 2)
                dia = '0' + dia;
            else
                dia = dia.substring(0, 2);

            if (mes.length < 2)
                mes = '0' + mes;
            else
                mes = mes.substring(0, 2);

            if (ano.length < 4) {
                if (ano > 30 && ano < 99)
                    ano = "1900".substring(0, 4 - ano.length) + ano;
                else
                    ano = "2000".substring(0, 4 - ano.length) + ano;
            }
            else
                ano = ano.substring(0, 4);

            data = dia + '/' + mes + '/' + ano;
        }
        return data;
    }
};