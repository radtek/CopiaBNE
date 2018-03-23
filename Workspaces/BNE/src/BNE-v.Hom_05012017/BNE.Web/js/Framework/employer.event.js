// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.event-pkd.js!


/// <reference path="employer.js" />

employer.event = {
    cancel: function(evt) {
        /// <summary>
        /// Cancela o evento.
        /// </summary>
        if (window.event) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        } else {
            evt.preventDefault();
            evt.stopPropagation();
        }
        if (evt.originalEvent) {
            evt.preventDefault();
            evt.stopPropagation();
        }
    },
    getTarget: function(evt) {
        /// <summary>
        /// Retorna o elemento que iniciou o evento.
        /// </summary>
        if (window.event) {
            return window.event.srcElement;
        } else if (evt.srcElement) {
            return evt.srcElement;
        } else {
            return (evt.target.nodeType == 3) ? evt.target.parentNode : evt.target;
        }
    },
    getEvent: function(evt) {
        /// <summary>
        /// Retorna o evento disparador. X-browser compliance.
        /// </summary>
        return (window.event) ? window.event : evt;
    },
    getKey: function(evt) {
        /// <summary>
        /// Retorna o código da tecla digitada.
        /// k = não faz diferença entre maiúscula e minúscula = NON CHAR.
        /// c = faz diferença entre maiúscula e minúscula (tabela ascii) = CHAR
        /// </summary>
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