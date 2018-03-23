employer.event = {
    /// <summary>
    /// Retorna o código da tecla digitada.
    /// k = não faz diferença entre maiúscula e minúscula = NON CHAR.
    /// c = faz diferença entre maiúscula e minúscula (tabela ascii) = CHAR
    /// </summary>
    getKey: function (evt) {
        var k, c;
        if (window.event) {
            k = (window.event.keyCode) ? window.event.keyCode : null; // RETORNA NON CHAR
            //c = (window.event.charCode) ? window.event.charCode : null; // RETORNA CHAR
            if (window.event.type == 'keypress') {
                c = k;
                k = 0;
            }
        } else {
            k = (evt.keyCode) ? evt.keyCode : null; // RETORNA NON CHAR
            c = (evt.charCode) ? evt.charCode : null; // RETORNA CHAR
        }
        return [k, c];
    }
};