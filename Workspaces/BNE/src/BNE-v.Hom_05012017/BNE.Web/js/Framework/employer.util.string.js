// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.util.string-pkd.js!


/// <reference path="employer.js" />
/// <reference path="employer.util.js" />

employer.util.string = {
    toNumber: function(string) {
        /// <summary>
        /// Remove letras e caracteres, remove '.', troca ',' por '.' para casas decimais.
        /// </summary>
        var i = 0;
        var negativo = false;
        while (string.charAt(i) == ' ') {
            i++;
        }
        if (string.charAt(i) == '-') {
            negativo = true;
        }
        var temp = string.replace(/\./g, '');
        temp = temp.replace(/,/g, '.');
        temp = temp.replace(/[^0-9.]/g, '');
        if (negativo) {
            return -(+temp);
        } else {
            return (+temp);
        }
    },
    trim: function(str) {
        /// <summary>
        /// Remove espaços brancos a direita e a esquerda.
        /// </summary>
        return str.replace(/^\s+|\s+$/g, '');
    },
    shortTrim: function(str) {
        /// <summary>
        /// Remove espaços brancos a direita e a esquerda usando regex, implementação otimizada
        /// para strings curtas.
        /// </summary>
        return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
    },
    longTrim: function(str) {
        /// <summary>
        /// Remove espaços brancos a direita e a esquerda, usando uma implementação hibrida
        /// de regex e com procura normal.
        /// </summary>
        var str = str.replace(/^\s\s*/, ''), ws = /\s/, i = str.length;
        while (ws.test(str.charAt(--i)));
        return str.slice(0, i + 1);
    },
    replace: function(p, t) {
        /// <summary>
        /// Substitui uma lista de valores em um contexto informado.
        /// pattern (p) é uma string que será procurada no contexto. ex: "%%"
        /// contexto (t). ex: "Meu pai se chama %%."
        /// a lista de parametros ou string com parametro é dinâmica, qualquer quantidade é válida (array ou string).
        /// </summary>
        var _p; // regex
        var _pg; // regex
        var _tt = t; // temp text
        var _ocur = []; // array de inicios das patterns, ocorrência no context 
        var _val = []; // valores a serem substituidos
        if (typeof p == "string") {
            _p = new RegExp(p);
            _pg = new RegExp(p, "g");
        }
        if (t.match(_p)) {
            // parse da lista de argumentos, tranforma tudo em um array de valores (_val)
            for (var i = 2; i < arguments.length; i++) {
                if (i == 2) {
                    if (typeof arguments[2] == "string") {
                        _val.push(arguments[2]);
                    } else {
                        _val = arguments[2];
                    }
                } else {
                    if (typeof arguments[i] == "object" && employer.util.isArray(arguments[i])) {
                        for (var j = 0; j < arguments[i].length; j++) {
                            _val.push(arguments[i][j]);
                        }
                    } else {
                        _val.push(arguments[i]);
                    }
                }
            }
            // acha tds as ocorrências
            var _tmp_s = 0;
            while (t.indexOf(p, _tmp_s) > -1) {
                _ocur.push(t.indexOf(p, _tmp_s));
                _tmp_s = t.indexOf(p, _tmp_s) + p.length;
            }
            // verifica se se as ocorrências são na mesma quantidade dos valores informados.
            if (_ocur.length != _val.length) {
                return "Lista de valores informados (" + _val.length + ") diferente da quantidade de ocorrência (" + _ocur.length + ") no contexto informado.";
            }
            for (var k = 0; k < _val.length; k++) {
                _tt = _tt.replace(_p, _val[k]);
            }
            return _tt;
        }
    }
};