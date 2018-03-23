// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.key-pkd.js!


/// <reference path="employer.js" />

// IE reflete charcode somente no keypress

// MOZILLA Firefox

// KeyEvent = http://developer.mozilla.org/en/DOM/Event/UIEvent/KeyEvent
// event.keyCode = http://developer.mozilla.org/en/DOM/event.keyCode
// event.charCode = http://developer.mozilla.org/en/DOM/event.charCode

// event.charCode = Returns the Unicode value of a non-character key in a keypress event (tab esc bkspc del ins hom end F*)
// event.keyCode = Returns the Unicode value of a character key pressed during a keypress event

// KEYPRESS NÃO DEMONSTRA = capslok shift ctrl alt printscreen scrolllok numlok

employer.key = {
    isSpecial: function(code) {
        if (employer.key.isCommand(code)) {
            return true;
        } else if (employer.key.isArrow(code)) {
            return true;
        } else if (employer.key.isFunction(code)) {
            return true;
        } else {
            return false;
        }
    },
    isEnter: function(code) {
        if (code == employer.key._commands.ENTER) {
            return true;
        } else {
            return false;
        }
    },
    isSpace: function(code) {
        if (code == employer.key._space.SPACE) {
            return true;
        } else {
            return false;
        }
    },
    isArrow: function(code) {
        var is = false;
        var list = employer.key._arrows;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isNumber: function(code) {
        var is = false;
        var list = employer.key._numbers;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isUpperCaseLetter: function(code) {
        var is = false;
        var list = employer.key._upperLetters;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isLowerCaseLetter: function(code) {
        var is = false;
        var list = employer.key._lowerLetters;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isUpperAccentedCharacter: function(code) {
        var is = false;
        var list = employer.key._upperAccented;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isLowerAccentedCharacter: function(code) {
        var is = false;
        var list = employer.key._lowerAccented;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isCommand: function(code) {
        var is = false;
        var list = employer.key._commands;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isModifier: function(code) {
        var is = false;
        var list = employer.key._modifier;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isFunction: function(code) {
        var is = false;
        var list = employer.key._functions;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isCtrlV: function(evt) {
        /// <summary>
        /// Valida se é um atalho de um PASTE CONTENT CTRL+V.
        /// </summary>        
        if (window.event) {
            if (window.event.ctrlKey) {
                return true;
            } else {
                return false;
            }
        } else {
            if (evt.ctrlKey) {
                return true;
            } else {
                return false;
            }
        }
    },
    hasModifierKeyPressed: function(e) {
        /// <summary>
        /// Verifica se alguma tecla modificadora está sendo pressionada.
        /// </summary>        
        // verifica se é um evento do jqery
        if (e.originalEvent != null) { e = e.originalEvent; }
        var modifier = null;
        if (e.altKey == true) {
            modifier = 'ALT';
        } else if (e.altLeft == true) {
            modifier = 'ALT';
        } else if (e.ctrlKey == true) {
            modifier = 'CTRL';
        } else if (e.ctrlLeft == true) {
            modifier = 'CTRL';
        } else if (e.shiftKey == true) {
            modifier = 'SHIFT';
        } else if (e.shiftLeft == true) {
            modifier = 'SHIFT';
        }
        return modifier;
    },
    shortcuts: function(evt) {
        /// <summary>
        /// Atalhos da aplicação.
        /// </summary>        
        var type = evt.type;
        var k = employer.event.getKey(evt)[0];
        var mod = employer.key.hasModifierKeyPressed(evt);
        // IE não possui charcode e só diferencia Maisculo de Minúsculo no keypress
        if (type == 'keydown' || type == 'keyup') {
            if (k == 113) {
                // 113 = F2
                var btnF2 = $get('ctl00_btnAtalhoF2');
                if (btnF2 != null) {
                    employer.event.cancel(evt);
                    btnF2.click();
                }
            } else if (k == 119) {
                // 119 = F8
                var btnF8 = $get('ctl00_btnAtalhoF8');
                if (btnF8 != null) {
                    employer.event.cancel(evt);
                    btnF8.click();
                }
            } else if (k == 120) {
                // 120 = F9
                var tempF9 = $get('ctl00_btnAtalhoF9');
                if (tempF9 != null) {
                    employer.event.cancel(evt);
                    employer.form.util.focusBtnPesquisa(evt);
                }
            } else if (k == 123) {
                // 123 = F12
                var tempF12 = $get('ctl00_hfIDBtnF12');
                if (tempF12 != null) {
                    if (tempF12.value != '') {
                        var btnF12 = $get(tempF12.value);
                        if (btnF12 != null) {
                            employer.event.cancel(evt);
                            btnF12.click();
                        }
                    }
                }
            }
            if (mod === 'ALT') {
                if (k == 68) {
                    // ALT + D (68)
                    var tempAltD = $get('ctl00_hfIDBtnAltD');
                    if (tempAltD != null) {
                        if (tempAltD.value != '') {
                            var btnAltD = $get(tempAltD.value);
                            if (btnAltD != null) {
                                var confirm = window.confirm("Tem certeza que deseja excluir?");
                                if (confirm) {
                                    btnAltD.click();
                                    employer.event.cancel(evt);
                                }
                            }
                        }
                    }
                } else if (k == 83) {
                    // ALT + S (83)
                    if (employer.form.util.searchInvalidValidator('Salvar')) {
                        var tempAltS = $get('ctl00_hfIDBtnAltS');
                        if (typeof tempAltS == 'object') {
                            if (tempAltS.value != '') {
                                var btnAltS = $get(tempAltS.value);
                                if (typeof btnAltS == 'object') {
                                    btnAltS.click();
                                    employer.event.cancel(evt);
                                }
                            }
                        }
                    }
                }
            }
        }
    },
    disableKeys: function(evt) {
        var k, t, ty, ro;
        if (window.event) { // eg. IE
            k = window.event.keyCode;
            t = window.event.srcElement.nodeName;
            e = window.event.srcElement;
            id = window.event.srcElement.id;
            ty = window.event.srcElement.type;
            ro = window.event.srcElement.getAttribute("readonly");
        } else if (evt.which) { // eg. Firefox
            k = evt.which;
            t = evt.target.nodeName;
            e = evt.target;
            id = evt.target.id;
            ty = evt.target.type;
            ro = evt.target.getAttribute("readonly");
        }
        if (k == 8 && (!(t == 'INPUT' && (ty == 'text' || ty == 'password')) && t !== 'TEXTAREA') || ro == true) {
            return false;
        } else if (k == 13 && (t !== 'TEXTAREA' && ty !== 'submit') && (id != 'ctl00_txtFiltroPesquisa_txtValor' && id != 'ctl00_cphConteudo_txtFiltroPesquisa_txtValor' && id != 'ctl00_cphConteudo_ucEmpresas_tbxFiltroBusca_text' && id != 'ctl00_cphConteudo_ucEdicaoCV_tbxFiltroBusca_text')) {
            return false;
        } else if (k == 13 && id == 'ctl00_txtFiltroPesquisa_txtValor') {
            $get('ctl00_btiPesquisar').click();
        } else if (k == 13 && id == 'ctl00_cphConteudo_txtFiltroPesquisa_txtValor') {
            $get('ctl00_cphConteudo_btiPesquisar').click();
        }
    },
    disableCapsLock: function(evt) {
        /// <summary>
        /// Gieyson Stelmak alterado no BNE para possibilitar que seja travado o CAPS LOOK
        /// Adicionado variável s e validação no fim do método
        /// Foi desabilitada esta função. Ela é chamada na geral.js
        /// </summary>
        var k, t, ty, ro, s;
        if (window.event) { // eg. IE
            k = window.event.keyCode;
            t = window.event.srcElement.nodeName;
            e = window.event.srcElement;
            id = window.event.srcElement.id;
            ty = window.event.srcElement.type;
            ro = window.event.srcElement.getAttribute("readonly");
            s = window.event.shiftKey;
        } else if (evt.which) { // eg. Firefox
            k = evt.which;
            t = evt.target.nodeName;
            e = evt.target;
            id = evt.target.id;
            ty = evt.target.type;
            ro = evt.target.getAttribute("readonly");
            s = evt.shiftKey;
        }
        if (k >= 65 && k <= 90 && !s && e.parentNode.className != 'lista_sugestoes') {
            setMensagemAviso('Por favor, não digite informações apenas em MAIÚSCULAS! Desligue a tecla Caps Lock.', 'error');
            return false;
        }
    },
    invertToCase: function(evt) {
        /// <summary>
        /// Sobrescreve  o evento KEYPRESS invertendo o charCase(upper/lower).
        /// </summary>        
        if (evt.type == 'keypress') {
            var key = employer.event.getKey(evt)[1];
            var evtMaker = function(newKey) {
                // firefox e outros que usam o Gecko 
                if (evt.which) {
                    var newEvt = document.createEvent("KeyboardEvent");
                    newEvt.initKeyEvent(evt.type, true, true, document.defaultView,
                             evt.ctrlKey, evt.altKey, evt.shiftKey,
                             evt.metaKey, 0, newKey);
                    evt.preventDefault();
                    evt.target.dispatchEvent(newEvt);
                    // IE 
                } else {
                    evt.keyCode = newKey;
                }
            };
            if ((key >= 97 && key <= 122) || (key >= 224 && key <= 255)) {
                // converte de acordo com o valor decimal da tecla na tabela ascii    
                evtMaker(key - 32);
            } else if ((key >= 65 && key <= 90) || (key >= 192 && key <= 223)) {
                // converte de acordo com o valor decimal da tecla na tabela ascii    
                evtMaker(key + 32);
            }
        }
    }
};

employer.key._numbers = {
    ZERO: 48,
    ONE: 49,
    TWO: 50,
    THREE: 51,
    FOUR: 52,
    FIVE: 53,
    SIX: 54,
    SEVEN: 55,
    EIGHT: 56,
    NINE: 57
};

employer.key._upperLetters = {
    A: 65,
    B: 66,
    C: 67,
    D: 68,
    E: 69,
    F: 70,
    G: 71,
    H: 72,
    I: 73,
    J: 74,
    K: 75,
    L: 76,
    M: 77,
    N: 78,
    O: 79,
    P: 80,
    Q: 81,
    R: 82,
    S: 83,
    T: 84,
    U: 85,
    V: 86,
    W: 87,
    X: 88,
    Y: 89,
    Z: 90
};

employer.key._lowerLetters = {
    a: 97,
    b: 98,
    c: 99,
    d: 100,
    e: 101,
    f: 102,
    g: 103,
    h: 104,
    i: 105,
    j: 106,
    k: 107,
    l: 108,
    m: 109,
    n: 110,
    o: 111,
    p: 112,
    q: 113,
    r: 114,
    s: 115,
    t: 116,
    u: 117,
    v: 118,
    w: 119,
    x: 120,
    y: 121,
    z: 122
};

employer.key._upperAccented = {
    A_GRAVE: 192,
    A_ACUTE: 193,
    A_CIRCUMFLEX: 194,
    A_TILDE: 195,
    A_DIAERESIS: 196,
    A_ring_above: 197,
    AE: 198,
    C_CEDILLA: 199,
    E_GRAVE: 200,
    E_ACUTE: 201,
    E_CIRCUMFLEX: 202,
    E_DIAERESIS: 203,
    I_GRAVE: 204,
    I_ACUTE: 205,
    I_ACUTE: 206,
    I_DIAERESIS: 207,
    ETH: 208,
    N_tild: 209,
    O_GRAVE: 210,
    O_ACUTE: 211,
    O_CIRCUMFLEX: 212,
    O_TILDE: 213,
    O_DIAERESIS: 214,
    multiplication: 215,
    O_STROKE: 216,
    U_GRAVE: 217,
    U_ACUTE: 218,
    U_CIRCUMFLEX: 219,
    U_DIAERESIS: 220,
    Y_ACUTE: 221,
    THORN: 222,
    SHARP_s: 223
};

employer.key._lowerAccented = {
    a_GRAVE: 192,
    a_ACUTE: 193,
    a_CIRCUMFLEX: 194,
    a_TILDE: 195,
    a_DIAERESIS: 196,
    a_ring_above: 197,
    ae: 198,
    c_CEDILLA: 199,
    e_GRAVE: 200,
    e_ACUTE: 201,
    e_CIRCUMFLEX: 202,
    e_DIAERESIS: 203,
    i_GRAVE: 204,
    i_ACUTE: 205,
    i_CIRCIMFLEX: 206,
    i_DIAERESIS: 207,
    eth: 208,
    n_tild: 209,
    o_GRAVE: 210,
    o_ACUTE: 211,
    o_CIRCUMFLEX: 212,
    o_TILDE: 213,
    o_DIAERESIS: 214,
    multiplication: 215,
    o_STROKE: 216,
    u_GRAVE: 217,
    u_ACUTE: 218,
    u_CIRCUMFLEX: 219,
    u_DIAERESIS: 220,
    y_ACUTE: 221,
    thron: 222,
    y_DIAERESIS: 223
};

employer.key._space = {
    SPACE: 23
};

employer.key._modifiers = {
    SHIFT: 16,
    CTRL: 17,
    ALT: 18
};

employer.key._commands = {
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
    SELECT_KEY: 93
};

employer.key._arrows = {
    LEFT_ARROW: 37,
    UP_ARROW: 38,
    RIGHT_ARROW: 39,
    DOWN_ARROW: 40
};

employer.key._functions = {
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
};