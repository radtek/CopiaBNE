AjaxClientControlBase = function () {

}

AjaxClientControlBase.Trim = function(valor) {
    /// <summary>
    /// Remove espaços a direita e a esquerda da string.
    /// </summary>
    return valor.replace(/^\s+|\s+$/g, '');
}

AjaxClientControlBase.padRight = function(str, pad, count) {
    str = str.toString();
    while (str.length < count)
        str = str + pad;
    return str;
}
AjaxClientControlBase.padLeft = function (str, pad, count) {
    str = str.toString();
    while (str.length < count)
        str = pad + str;
    return str;
}

AjaxClientControlBase.Next = function (e) {
    if (e == $get(e.id) && e.blur) {
        //To do 
        //e.blur();
    }
}

AjaxClientControlBase.Trim = function (e) {
    return e.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}

AjaxClientControlBase.NextFocul = function (ev, campo) {
    //Compatibilidade com a Framawork da Employer
    if (typeof moveToNextElement == "function" && campo.getAttribute("MaxLength")) {
        var maxLength = parseInt(campo.getAttribute("MaxLength"));
        moveToNextElement(campo, ev.rawEvent, maxLength);
    } else if (campo.getAttribute("MaxLength")) {
        var maxLength = parseInt(campo.getAttribute("MaxLength"));
        if (maxLength <= campo.value.length && !AjaxClientControlBase.Key.isSpecial(ev.keyCode)) {
            AjaxClientControlBase.Next(campo);
        }
    }
}

AjaxClientControlBase.IsNullOrEmpty = function (str) {
    return str == null || str == "";
}

AjaxClientControlBase.EqualsIsNullOrEmpty = function (str1, str2) {
    return (AjaxClientControlBase.IsNullOrEmpty(str1) && AjaxClientControlBase.IsNullOrEmpty(str2)) || str1 == str2;
}

AjaxClientControlBase.CancelEvent = function (evt) {
    /// <summary>
    /// Cancel Event for any browser
    /// </summary>
    /// <param name="evt" type="Sys.UI.DomEvent">
    /// Event info
    /// </param>
    if (typeof (evt.returnValue) !== "undefined") {
        evt.returnValue = false;
    }
    if (typeof (evt.cancelBubble) !== "undefined") {
        evt.cancelBubble = true;
    }
    if (typeof (evt.preventDefault) !== "undefined") {
        evt.preventDefault();
    }
    if (typeof (evt.stopPropagation) !== "undefined") {
        evt.stopPropagation();
    }
    return false;
}

AjaxClientControlBase.GetSelectionStart = function (element) {
    /// <summary>
    /// Retorna a posição inicial de seleção do texto em um DOM element.
    /// </summary>
    if (document.selection) {
        var textRange = document.selection.createRange();
        var isCollapsed = textRange.compareEndPoints("StartToEnd", textRange) == 0;
        if (!isCollapsed) { textRange.collapse(true); }
        var bookmark = textRange.getBookmark();
        var ret = bookmark.charCodeAt(2) - 2;
        if (navigator.userAgent.toLowerCase().indexOf('nt 5') > -1) {
            ret--;
        }
        return ret;
    } else if (typeof element.selectionStart == 'number') {
        // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
        return element.selectionStart;
    }
}

AjaxClientControlBase.GetSizeTextSelected = function (element) {
    /// <summary>
    /// Retorna o tamanho da seleção do texto em um DOM element.
    /// </summary>
    if (document.selection) {
        var textRange = document.selection.createRange();
        return textRange.text.length;
    } else if (typeof element.selectionStart == 'number') {
        // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
        return element.selectionEnd - element.selectionStart;
    }
}

AjaxClientControlBase.RestringeNumeroUp = function (campo, minimo, maximo) {
    var v = campo.value.replace("-", "").replace(",", "").replace(".", "");
    if (v == "")
        return;

    if (isNaN(v))
        campo.value = minimo;
    else {
        if (v < minimo)
            campo.value = minimo;
        else if (v > maximo)
            campo.value = maximo;
    }
}

AjaxClientControlBase.Key = function () { }


AjaxClientControlBase.Key.Commands = {

    BACKSPACE: 8,
    TAB: 9,
    ENTER: 13,
    PAUSE_BREAK: 19,
    CAPS_LOCK: 20,
    ESCAPE: 27,
    PAGE_UP: 33,
    PAGE_DOWN: 34,
    END: 35,
    HOME: 36,
    INSERT_KEY: 45,
    DELETE_KEY: 46,
    LEFT_WINDOW_KEY: 91,
    RIGHT_WINDOW_KEY: 92,
    SELECT_KEY: 93,
    LEFT: 37,
    RIGTH: 39,
    UP: 38,
    DOWN: 38
}

AjaxClientControlBase.Key.Functions = {
    F1: 112,
    F2: 113,
    F3: 114,
    F4: 115,
    F5: 116,
    F6: 117,
    F7: 118,
    F8: 119,
    F9: 120,
    F10: 121,
    F11: 122,
    F12: 123
}

AjaxClientControlBase.Key.GetKeyCode = function(evt) {
    /// <summary>
    /// Get Keycode for any browser
    /// and convert keycode Safari to IE Code
    /// </summary>
    /// <param>
    /// Event info name="evt" type="Sys.UI.DomEvent" 
    /// </param>
    /// <Return>
    /// Keycode value 
    /// </Return>
    var scanCode = 0;
    if (evt.keyIdentifier) {
        if (evt.charCode == 63272) { //63272: 'KEY_DELETE', 46
            scanCode = 46;
        }
        else if (evt.charCode == 63302) { //63302: 'KEY_INSERT', 45
            scanCode = 45;
        }
        else if (evt.charCode == 63233) { //63233: 'KEY_ARROW_DOWN',40
            scanCode = 40;
        }
        else if (evt.charCode == 63235) { //63235: 'KEY_ARROW_RIGHT', 39
            scanCode = 39;
        }
        else if (evt.charCode == 63232) { //63232: 'KEY_ARROW_UP', 38
            scanCode = 38;
        }
        else if (evt.charCode == 63234) { //63234: 'KEY_ARROW_LEFT', 37
            scanCode = 37;
        }
        else if (evt.charCode == 63273) { //63273: 'KEY_HOME', 36
            scanCode = 36;
        }
        else if (evt.charCode == 63275) { //63275: 'KEY_END', 35
            scanCode = 35;
        }
        else if (evt.charCode == 63277) { //63277: 'KEY_PAGE_DOWN', 34
            scanCode = 34;
        }
        else if (evt.charCode == 63276) { //63276: 'KEY_PAGE_UP', 33
            scanCode = 33;
        }
        else if (evt.charCode == 3) { //3: 'KEY_ENTER', 13
            scanCode = 13;
        }
    }    
    if (scanCode == 0) {
        if (evt.charCode) {
            scanCode = evt.charCode;
        }
    }
    if (scanCode == 0) {
        scanCode = evt.keyCode;
    }
    return scanCode;
}




AjaxClientControlBase.Key.isExistsList = function (list, value) {
    for (var property in list) {
        if (list[property] == value) { return true; }
    }
    return false;
}

AjaxClientControlBase.Key.isCommand = function (code) {
    return this.isExistsList(AjaxClientControlBase.Key.Commands, code);
}

AjaxClientControlBase.Key.isFunctions = function (code) {
    return this.isExistsList(AjaxClientControlBase.Key.Functions, code);
}


AjaxClientControlBase.Key.isSpecial = function (code) {
    return AjaxClientControlBase.Key.isCommand(code) || AjaxClientControlBase.Key.isFunctions(code);
}

AjaxClientControlBase.SoNumero = function (e, clickButon) {
    var key = AjaxClientControlBase.Key.GetKeyCode(e);

    if (e.keyCode && AjaxClientControlBase.Key.isCommand(key)) {
        if (clickButon && e.keyCode == AjaxClientControlBase.Key.Commands.ENTER) {
            AjaxClientControlBase.CancelEvent(e);
            setTimeout("document.getElementById('" + clickButon.id + "').click()", 300);
            return false;
        }
        return true;
    }

    var caracter = String.fromCharCode(key);

    if (key == Sys.UI.Key.space || isNaN(caracter))
        AjaxClientControlBase.CancelEvent(e);
}

AjaxClientControlBase.SimulateClick = function (e, id)
{    
    if (e.keyCode == AjaxClientControlBase.Key.Commands.ENTER)
    { 
        AjaxClientControlBase.CancelEvent(e);
        document.getElementById(id).click(); 
    } 
}