// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.util.number-pkd.js!


/// <reference path="employer.js" />
/// <reference path="employer.util.js" />

employer.util.number = {
    mask: function(val, mask) {
        /// <summary>
        /// Insere uma mascara em um numero.
        /// 'R$ ###.###.###.##0,00'
        /// '#-###-###-##.##000'
        /// 12
        /// 12.3
        /// 12.33
        /// 12.33555678
        /// </summary>
        var prefix;
        var rad;
        var sufix;
        var negative = false;
        if (val < 0) {
            negative = true;
        }
        if (typeof val == 'number') {
            val = String(val);
            val = val.replace(/\./g, ',');
            prefix = mask.substring(0, mask.indexOf('#'));
            sufix = mask.substring(mask.lastIndexOf('#') + 1, mask.length);
            mask = mask.substring(mask.indexOf('#'), mask.length);
            //mask = mask.substring(mask.indexOf('#'), mask.lastIndexOf('#')+1);
            // normaliza o value caso haja 
            if (sufix) {
                if (sufix.indexOf(',') > -1) {
                    ts = sufix.split(',');
                    tv = val.split(',');
                    // ambos possuem decimal
                    if (ts.length == tv.length) {
                        // decimal do sufixo maior q decimal do valor
                        if (tv[1].length < ts[1].length) {
                            for (var i = 0; i < ts[1].length; i++) {
                                if (tv[1].charAt(i) == '') {
                                    tv[1] += ts[i].charAt(i);
                                }
                            }
                            val = tv[0] + "," + tv[1];
                            // remove caracteres qndo o decimal do valor é maior q o decimal do sufixo
                        } else if (tv[1].length > ts[1].length) {
                            val = val.substring(0, val.length - (tv[1].length - ts[1].length));
                        }
                        // adiciona o decima do sufixo ao valor
                    } else if (tv.length < ts.length) {
                        val += "," + ts[1];
                    }
                }
            }
            var im = 0;
            if (val.length - sufix.length > 0) {
                for (var j = val.length - sufix.length - 1; j > -1; j--) {
                    if (val.charAt(j) != '') {
                        if (mask.charAt(mask.length - sufix.length - 1 - im) != '#') {
                            val = val.substring(0, j + 1) + mask.charAt(mask.length - sufix.length - 1 - im) + val.substring(j + 1, val.length);
                        }
                    }
                    im++;
                }
            }
            if (typeof val == 'string') {
                if (negative) {
                    return "( " + prefix + val + " )";
                } else {
                    return prefix + val;
                }
            } else {
                return null;
            }
        } else {
            return null;
        }
    }
};