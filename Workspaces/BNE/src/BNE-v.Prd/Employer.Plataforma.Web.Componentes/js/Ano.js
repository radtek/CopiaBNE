function AplicarMascaraAno(controle)
{
    var valor = Trim(controle.value);       
    if (valor == '')
        return;
 
    if(valor.match(/^(\d{1,4})$/))
    {
        if(parseInt(valor) > 30 && parseInt(valor) < 99)
            valor = "1900".substring(0, 4 - valor.length) + valor;
        else
            valor = "2000".substring(0, 4 - valor.length) + valor;
    }

    controle.value = valor;
}