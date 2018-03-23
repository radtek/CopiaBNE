AjaxClientDataBoundControlBase = function () {

}

AjaxClientDataBoundControlBase.Trim = function (valor) {
    /// <summary>
    /// Remove espaços a direita e a esquerda da string.
    /// </summary>
    return valor.replace(/^\s+|\s+$/g, '');
}

AjaxClientDataBoundControlBase.padRight = function (str, pad, count) {
    str = str.toString();
    while (str.length < count)
        str = str + pad;
    return str;
}
AjaxClientDataBoundControlBase.padLeft = function (str, pad, count) {
    str = str.toString();
    while (str.length < count)
        str = pad + str;
    return str;
}

AjaxClientDataBoundControlBase.IsNullOrEmpty = function (str) {
    return str == null || str == "";
}

AjaxClientDataBoundControlBase.Next = function (e) {
    if (e == $get(e.id) && e.blur) {
        //To do 
        //e.blur();
    }
}

AjaxClientDataBoundControlBase.NextFocul = function (ev, campo) {
    //Compatibilidade com a Framawork da Employer
    if (typeof moveToNextElement == "function" && campo.getAttribute("MaxLength")) {
        var maxLength = parseInt(campo.getAttribute("MaxLength"));
        moveToNextElement(campo, ev.rawEvent, maxLength);
    } else if (campo.getAttribute("MaxLength")) {
        var maxLength = parseInt(campo.getAttribute("MaxLength"));
        if (maxLength <= campo.value.length && !AjaxClientDataBoundControlBase.Key.isSpecial(ev.keyCode)) {
            AjaxClientDataBoundControlBase.Next(campo);
        }
    }
}

AjaxClientDataBoundControlBase.CancelEvent = function (evt) {
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

//AjaxClientDataBoundControlBase.prototype = {
//   
//}

AjaxClientDataBoundControlBase.Key = function () {

}

AjaxClientDataBoundControlBase.Key.Commands = {

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

AjaxClientDataBoundControlBase.Key.Functions = {
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

AjaxClientDataBoundControlBase.Key.GetKeyCode = function (evt) {
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


AjaxClientDataBoundControlBase.Key.isExistsList = function (list, value) {
    for (var property in list) {
        if (list[property] == value) { return true; }
    }
    return false;
}

AjaxClientDataBoundControlBase.Key.isCommand = function (code) {
    return this.isExistsList(AjaxClientDataBoundControlBase.Key.Commands, code);
}

AjaxClientDataBoundControlBase.Key.isFunctions = function (code) {
    return this.isExistsList(AjaxClientDataBoundControlBase.Key.Functions, code);
}


AjaxClientDataBoundControlBase.Key.isSpecial = function (code) {
    return AjaxClientDataBoundControlBase.Key.isCommand(code) || AjaxClientDataBoundControlBase.Key.isFunctions(code);
}