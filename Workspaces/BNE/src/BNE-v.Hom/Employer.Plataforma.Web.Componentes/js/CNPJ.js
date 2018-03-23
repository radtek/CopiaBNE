function AplicarMascaraCNPJ(controle)
{
    var valor = Trim(controle.value);    
    if (valor == '')
        return;
        
    if(valor.match("\\d{14}"))
    {       
        valor = valor.replace(/(\d{2})(\d)/,"$1.$2"); 
        valor = valor.replace(/(\d{3})(\d)/,"$1.$2"); 
        valor = valor.replace(/(\d{3})(\d)/,"$1/$2"); 
        valor = valor.replace(/(\d{4})(\d)/,"$1-$2");
    }
   controle.value = valor;
}
function ValidarCNPJ(sender, args)
{
    if(Trim(args.Value) == "")
        args.IsValid = true;
    else
    {
        var res = CNPJ.Validar(args.Value);
        args.IsValid = (res.error == null && res.value);
    }
}
