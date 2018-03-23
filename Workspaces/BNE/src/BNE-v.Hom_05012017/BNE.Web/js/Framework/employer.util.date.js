// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.util.date-pkd.js!


/// <reference path="employer.js" />
/// <reference path="employer.util.js" />

employer.util.date = {
    isEarlier: function(arg1, arg2) {
        /// <summary>
        /// Verifica se a primeira data é anterior a segunda.
        /// </summary>
        var dt1, dt2;
        if (typeof arg1 == "string" && typeof arg2 == "string") {
            var splitter;
            for (var i = 0; i < arg1.length; i++) {
                if (arg1.charCodeAt(i) < 48 || arg1.charCodeAt(i) > 57) {
                    splitter = arg1.charAt(i);
                    break;
                }
            }
            if (splitter) {
                var t1 = arg1.split(splitter);
                dt1 = new Date(t1[2], t1[1] - 1, t1[0]);
                var t2 = arg2.split(splitter);
                dt2 = new Date(t2[2], t2[1] - 1, t2[0]);
            } else {
                return false;
            }
        } else {
            dt1 = arg1;
            dt2 = arg2;
        }

        if (dt1.getTime() < dt2.getTime()) {
            return true;
        } else {
            return false;
        }
    },
    isEqual: function(arg1, arg2) {
        /// <summary>
        /// Verifica se a primeira data é igual a segunda.
        /// </summary>
        var dt1, dt2;
        if (typeof arg1 == "string" && typeof arg2 == "string") {
            var splitter;
            for (var i = 0; i < arg1.length; i++) {
                if (arg1.charCodeAt(i) < 48 || arg1.charCodeAt(i) > 57) {
                    splitter = arg1.charAt(i);
                    break;
                }
            }
            var t1 = arg1.split(splitter);
            dt1 = new Date(t1[2], t1[1] - 1, t1[0]);
            var t2 = arg2.split(splitter);
            dt2 = new Date(t2[2], t2[1] - 1, t2[0]);
        } else {
            dt1 = arg1;
            dt2 = arg2;
        }

        if (dt1.getTime() == dt2.getTime()) {
            return true;
        } else {
            return false;
        }
    },


    /// <summary>
    /// Calcula a Diferença de Datas em Milisegundos.
    /// </summary>
    calcDiferencaMilisegundos: function(data1, data2) {
        return Math.abs(data1.getTime() - data2.getTime());
    },
    /// <summary>
    /// Calcula a Diferença de Datas em Dias.
    /// </summary>
    calcDiferencaDias: function(data1, data2) {
        var diferencaDias = employer.util.date.calcDiferencaMilisegundos(data1, data2) / 86400000;
        var diferencaDiasArredondado = Math.round(diferencaDias);

        return (diferencaDiasArredondado > diferencaDias) ? diferencaDiasArredondado - 1 : diferencaDiasArredondado;
    },
    /// <summary>
    /// Calcula a Diferença de Datas em Anos.
    /// </summary>
    calcDiferencaAnos: function(data1, data2) {
        var diferencaAnos = employer.util.date.calcDiferencaDias(data1, data2) / 365;
        var diferencaAnosArredondado = Math.round(diferencaAnos);

        return (diferencaAnosArredondado > diferencaAnos) ? diferencaAnosArredondado - 1 : diferencaAnosArredondado;
    }
};