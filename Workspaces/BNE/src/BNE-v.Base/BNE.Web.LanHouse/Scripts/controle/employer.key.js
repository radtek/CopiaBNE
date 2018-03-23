employer.key = {
    isSpecial: function (code) {
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
    isCommand: function (code) {
        var is = false;
        var list = employer.key._commands;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isArrow: function (code) {
        var is = false;
        var list = employer.key._arrows;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isFunction: function (code) {
        var is = false;
        var list = employer.key._functions;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isUpperCaseLetter: function (code) {
        var is = false;
        var list = employer.key._upperLetters;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    isLowerCaseLetter: function (code) {
        var is = false;
        var list = employer.key._lowerLetters;
        for (var property in list) {
            if (list[property] == code) { return true; }
        }
        return is;
    },
    /// <summary>
    /// Valida se é um atalho de um PASTE CONTENT CTRL+V.
    /// </summary>      
    isCtrlV: function (evt) {
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
    }
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