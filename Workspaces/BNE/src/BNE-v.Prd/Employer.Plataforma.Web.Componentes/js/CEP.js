
function AplicarMascaraCEP(controle)
{
    var valor = Trim(controle.value);       
    if (valor == '')
        return;
 
    if(valor.match(/^(\d{5})(\d{3})$/))
        valor = valor.replace(/(\d{5})(\d{3})/,"$1-$2");

    controle.value = valor;
}