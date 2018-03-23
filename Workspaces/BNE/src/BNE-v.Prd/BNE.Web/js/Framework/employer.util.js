// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.util-pkd.js!


/// <reference path="employer.js" />

employer.util = {
    /// <summary>
    /// Recupera o controle desejado.
    /// </summary>
    findControl: function(control) {
        return $('[id$=' + control + ']');
    },
    /// <summary>
    /// Recupera o ClientID de um componente ASP.NET.
    /// </summary>
    findClientID: function(control) {
        return eventcontrol.util.findControl(control).attr("id");
    },
    /// <summary>
    /// Recupera o ClientID de um servercontrol.
    /// </summary>
    findClientIDControle: function(control) {
        return employer.util.findClientID(control + "_txtValor");
    },
    acf: false,
    isArray: function(e) {
        /// <summary>
        /// Verifica se o elemento é um array.
        /// </summary>
        return (typeof (e.length) == "undefined") ? false : true;
    },
    fixBugModal: function() {
        var ovf = $get('conteudo').style.overflow;
        if (!ovf || ovf == "" || ovf == "auto") {
            $get('conteudo').style.overflow = "visible";
        } else {
            $get('conteudo').style.overflow = "auto";
        }
    },
    getSelectionSize: function(e) {
        /// <summary>
        /// Retorna o tamanho da seleção do texto em um element DOM.
        /// </summary>
        if (document.selection) {
            var textRange = document.selection.createRange();
            return textRange.text.length;
        } else if (typeof e.selectionStart == 'number') {
            return e.selectionEnd - e.selectionStart;
        }
    },
    validPwd: function(source, args) {
        /// <summary>
        /// Valida string para ter no mínimo 4 letras e 4 numeros.
        /// </summary>
        var txt = args.Value;
        var c_num = 0;
        var c_str = 0;
        for (var i = 0; i < txt.length; i++) {
            if (isNaN(+(txt.charAt(i)))) {
                if (txt.charCodeAt(i) >= 65 && txt.charCodeAt(i) <= 90) {
                    c_str++;
                } else if (txt.charCodeAt(i) >= 97 && txt.charCodeAt(i) <= 122) {
                    c_str++;
                }
            } else if (typeof +(txt.charAt(i)) == "number") {
                c_num++;
            }
        }
        if (c_num == 4 && c_str == 4) {
            args.IsValid = true;
        } else {
            args.IsValid = false;
        }
    }
};