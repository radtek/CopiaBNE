function AplicarMascaraPIS(controle, idDescricao)
{
    var valor = Trim(controle.value);
    
    var res = PIS.RecuperarTipo(valor);   
    if (res.error == null && res.value != null)
        setElementText($get(idDescricao), res.value);
    else    
        setElementText($get(idDescricao), "");
    
    if (valor == '')
        return;
        
    if(valor.match("\\d{11}"))
    {       
       valor = valor.replace(/(\d{3})(\d)/,"$1.$2"); 
       valor = valor.replace(/(\d{5})(\d)/,"$1.$2");
       valor = valor.replace(/(\d{2})(\d{1})$/,"$1-$2");
    }        
    controle.value = valor;
}
function ValidarPIS(sender, args)
{
    if(Trim(args.Value) == "")
        args.IsValid = true;
    else
    {
        var res = PIS.Validar(args.Value);
        args.IsValid = (res.error == null && res.value);
    }
}