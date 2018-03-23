//Depende do emplyer.key employer.event

employer.controle = {
    /// <summary>
    /// Remove uma regex de um componente.
    /// </summary>
    removerMascaraControle: function (controle, regex) {
        if (regex)
            controle.value = employer.controle.removerMascaraValor(controle.value, regex);
        controle.select();
    },
    /// <summary>
    /// Remove mascara de valores.
    /// </summary>
    removerMascaraValor: function (valor, regex) {
        if (valor.match(regex))
            return valor.replace(/[\. -\/]/g, '');
        else
            return valor;
    },
    /// <summary>
    /// Retorna somente número ou backspace/delete/return.
    /// </summary>
    apenasNumeros: function (control, event, size) {
        var tecla = employer.event.getKey(event);

        if (employer.key.isCtrlV(event)) {
            return true;
        }

        //verifica se é uma tecla especial
        if (employer.key.isSpecial(tecla[0])) return true;

        var selecionado = false;
        var userSelection;
        if (control.value.length == size && employer.controle.textoNaoSelecionado(control)) { return false; }
        // 48='0', 57='9'
        if (employer.key.isSpecial(tecla[0])) return true; // firefox bug backspace        
        if (tecla[1] >= 48 && tecla[1] <= 57) {
            return true;
        } else {
            return false;
        }
    },
    /// <summary>
    /// Verifica se usuário não selecionou parte do text ou textarea. X-browser compliance.
    /// </summary>
    textoNaoSelecionado: function (control) {
        var value = false;
        if (document.selection) {
            //IE support
            control.focus();
            sel = document.selection.createRange();
            if (sel.text == "") { value = true; }
        } else if (typeof control.selectionStart == 'number') {
            // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
            //MOZILLA/NETSCAPE support
            var startPos = control.selectionStart;
            var endPos = control.selectionEnd;
            if (startPos == endPos) { value = true; }
        }
        return value;
    },
    /// <summary>
    /// Força foco para o proximo campo caso ultrapasse o tamanho do campo.
    /// </summary>
    moveToNextElement: function (control, event, size) {
        var tecla = employer.event.getKey(event);
        // alterado validação para todas as teclas especiais
        if (control.value.length >= size && !employer.key.isSpecial(tecla[0]) && employer.controle.textoNaoSelecionado(control)) {
            // caso seja o único elemento ativo da pagina: tira o foco dele (para executar comportamentos do onblur) e retorna o foco ao elemento
            control.blur();
            //control.focus();
            // caso preencheu o tamanho segue para prox elemento
            //window.setTimeout(function () { 
            //    employer.form.next(control);
            //}, 200);
        } else {
            return true;
        }
    },
    /// <summary>
    /// Retorna tamanho da campo data.
    /// </summary>
    recuperarTamanhoData: function (control) {
        if (control.value.indexOf("/") >= 0) {
            return 10;
        } else {
            return 8;
        }
    }
};