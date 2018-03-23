
function AplicarMascaraPlacaVeiculo(controle)
{
    var valor = Trim(controle.value);       
    if (valor == '')
        return;
 
    if(valor.match( /^[A-Za-z]{3}-{0,1}\d{4}$/ ))
        valor = valor.toUpperCase().replace(/([A-Z]{3})(\d{4})/,"$1-$2");

    controle.value = valor;
}

function ApenasNumerosPlacaVeiculo(controle, event) {
    /// <summary>
    /// Retorna somente nros ou backspace/delete/return e impede ZERO (0) no primeiro digito.
    /// </summary>

    var tecla = component_getEventKey(event);
    /*
     8 - backspace
    37 - left arrow
    38 - up arrow
    39 - right arrow
    40 - down arrow
    46 - delete
    */
    if (tecla == 8 || (tecla >= 37 && tecla <= 40) || tecla == 46)
        return true;

    // força uppercase no event keypress
    //component_keyEvtToCase(event, 'upper')
    
    var pos = component_getSelectionStart(controle);
    var sizeSelected = component_getSizeTextSelected(controle);
    
    if (controle.value.length < 7 || (controle.value.length == 7 && sizeSelected > 0)) {
        if (pos < 3) {
            // letras
            //65 = 'A', 90 = 'Z', 97 = 'a', 122 = 'z'
            if ((tecla >= 65 && tecla <= 90) || (tecla >= 97 && tecla <= 122)) {
                return true;
            } else {
                return false;
            }  
        } else {
            // nros
            // 48='0', 57='9'  
            if ((tecla >= 48 && tecla <= 57)) {
                return true;
            } 
            else {
                return false;
            }
        }
    } else {
        return false;
    }
}