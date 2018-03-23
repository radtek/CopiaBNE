function AplicarMascaraAlfaNumerico(controle, tipo, tam)
{
    var valor = Trim(controle.value); 
    if (valor == '') {
        controle.value = '';
        return;
    }

    switch(tipo) 
    {
        case 1: //letras
        case 2: //letras maiúsculas    
            if (valor.match("^[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç]{1," + tam + "}$")) {
                if (valor.length > tam)
                    valor = valor.substrig(0, tam);

                valor = valor.toUpperCase();                
            }
            break;        
        case 3: //letras minusculas
            if(valor.match("^[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç]{1," + tam + "}$"))
            {
                if(valor.length > tam)
                    valor = valor.substrig(0, tam);

                valor = valor.toLowerCase();             
            }
            break;           
        case 4: //números
            if(valor.match("^\\d{1," + tam + "}$"))
            {
                if(valor.length > tam)
                    valor = valor.substrig(0,tam);
            }
            break;                   
        case 5: //alfa-numérico maiúsculas          
        case 6: //alfa-numérico minusculas
            if(valor.length > tam)
                valor = valor.substrig(0,tam);
            
            if(tipo ==5)
                valor = valor.toUpperCase();
            else if(tipo == 6)                    
                valor = valor.toLowerCase();                
            break;           
        default: //alfa-numérico            
            break; 
    }
    controle.value = valor;
}

function ApenasTeclasAlfaNumerico(controle, event, tam, tipo) {
    /// <summary>
    /// Valida tecla digitada, conforme parametro TIPO.
    /// 1 2 3 letras
    /// 4 numeros
    /// 5 6 alfanumérico
    /// </summary>
    var charkey = employer.event.getKey(event)[1];
    var keycode = employer.event.getKey(event)[0];
    var tecla;
    try {
        //isCtrlV ?
        if (employer.key.isCtrlV(event)) {
            return true;
        }
    }
    catch (e) { }    
    if (window && window.event && window.event.type != 'keypress') {
        tecla = keycode;
    } else {
        tecla = charkey;
    }
    // Se for enter retorna true
    if (tecla == 13) {
        return true;
    }
    var maxlength = (+controle.getAttribute('MaxLength'));
    if (maxlength == 0) {
        maxlength = (+controle.getAttribute("rows"))*(+controle.getAttribute("cols"));
    }
    if (employer.key.isSpecial(keycode)) return true; // firefox bug backspace
    if (maxlength != 0 && controle.value.length >= maxlength && employer.util.getSelectionSize(controle) == 0) { return false };
    switch (tipo) {
        case 1: //letras
            // 65 = 'A' 90 = 'Z' || 97 = 'a' 122 = 'z' || charcode >= 192 && charcode <= 223 || charcode >= 224 && charcode <= 255
            if ( !( (tecla >= 65 && tecla <= 90) || (tecla >= 97 && tecla <= 122)
             || (tecla >= 192 && tecla <= 223) || (tecla >= 224 && tecla <= 255) || tecla == 32 )) {
                 return false;
            }
            break;
        case 2: //letras maiúsculas 
            if (employer.key.isLowerCaseLetter(tecla) || employer.key.isLowerAccentedCharacter(tecla)) {      
//            if (tecla >= 97 && tecla <= 122) {
                employer.key.invertToCase(event);                
                charkey = employer.event.getKey(event)[1];
                keycode = employer.event.getKey(event)[0];
                if (window && window.event && window.event.type != 'keypress') {
                    tecla = keycode;
                } else {
                    tecla = charkey;
                }
            }
            if ( !( (tecla >= 65 && tecla <= 90) || (tecla >= 192 && tecla <= 223) || tecla == 32 )) {
                 return false;
            }
            break;           
        case 3: //letras minusculas
            if (employer.key.isUpperCaseLetter(tecla) || employer.key.isUpperAccentedCharacter(tecla)) {      
//            if (tecla >= 65 && tecla <= 90) {
                employer.key.invertToCase(event);
                charkey = employer.event.getKey(event)[1];
                keycode = employer.event.getKey(event)[0];
                if (window && window.event && window.event.type != 'keypress') {
                    tecla = keycode;
                } else {
                    tecla = charkey;
                }
            }
            if ( !( (tecla >= 97 && tecla <= 122) || (tecla >= 224 && tecla <= 255) || tecla == 32 )) {
                 return false;
            }
            break;         
        case 4: //números
            //48 = '0', 57 = '9'
            if( !( (tecla >= 48) && (tecla <= 57) )) {
                return false;
            }
            break;
        case 5: //alfa-numérico maiúsculas                      
            //48 = '0', 57 = '9' || 65 = 'A', 90 = 'Z'
            if (employer.key.isLowerCaseLetter(tecla) || employer.key.isLowerAccentedCharacter(tecla)) {      
//            if (tecla >= 97 && tecla <= 122) {
                employer.key.invertToCase(event);
                charkey = employer.event.getKey(event)[1];
                keycode = employer.event.getKey(event)[0];
                if (window && window.event && window.event.type != 'keypress') {
                    tecla = keycode;
                } else {
                    tecla = charkey;
                }
            }
            if ( !( (tecla >= 65 && tecla <= 90) || (tecla >= 192 && tecla <= 223) || (tecla >= 48 && tecla <= 57) || tecla == 32 )) {
                 return false;
            }
            break;           
        case 6: //alfa-numérico minusculas
            //48 = '0', 57 = '9' || 97 = 'a', 122 = 'z'
            if (employer.key.isUpperCaseLetter(tecla) || employer.key.isUpperAccentedCharacter(tecla)) {      
//            if (tecla >= 65 && tecla <= 90) {
                employer.key.invertToCase(event);
                charkey = employer.event.getKey(event)[1];
                keycode = employer.event.getKey(event)[0];
                if (window && window.event && window.event.type != 'keypress') {
                    tecla = keycode;
                } else {
                    tecla = charkey;
                }
            }
            if ( !( (tecla >= 97 && tecla <= 122) || (tecla >= 192 && tecla <= 223) || (tecla >= 48 && tecla <= 57) || tecla == 32 )) {
                 return false;
            }
            break;
        default : //aceita tudo (numeros letras e caracteres especiais @$...)         
            if ( !( tecla >= 32) ) {
                 return false;
            }
            break;
    }
    return true;
}