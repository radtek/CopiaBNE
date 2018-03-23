
function AplicarMascaraCEI(controle)
{        
    var valor = Trim(controle.value);    
    if (valor == '')
        return;
        
    if(valor.match("\\d{12}"))
    {       
       valor = valor.replace(/(\d{2})(\d)/,"$1.$2"); 
       valor = valor.replace(/(\d{3})(\d)/,"$1.$2");
       valor = valor.replace(/(\d{5})(\d)/,"$1/$2");
    }        
    controle.value = valor;
}
function ValidarCEI(sender, args)
{
    if(Trim(args.Value) == "")
        args.IsValid = true;
    else
    {
        var res = CEI.Validar(args.Value);
        args.IsValid = (res.error == null && res.value);
    }
}