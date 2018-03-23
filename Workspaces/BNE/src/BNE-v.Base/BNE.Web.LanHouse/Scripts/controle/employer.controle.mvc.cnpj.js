employer.controle.mvc.cnpj = {
    aplicarMascaraCNPJ : function AplicarMascaraCNPJ(control) {
        var valor = employer.util.trim(control.value);    
        if (valor == '')
            return;
        
        if(valor.match("\\d{14}"))
        {       
            valor = valor.replace(/(\d{2})(\d)/,"$1.$2"); 
            valor = valor.replace(/(\d{3})(\d)/,"$1.$2"); 
            valor = valor.replace(/(\d{3})(\d)/,"$1/$2"); 
            valor = valor.replace(/(\d{4})(\d)/,"$1-$2");
        }
        control.value = valor;
    }
//   ,
//    validarCNPJ : function ValidarCNPJ(sender, args)
//    {
//        if(Trim(args.Value) == "")
//            args.IsValid = true;
//        else
//        {
//            var res = CNPJ.Validar(args.Value);
//            args.IsValid = (res.error == null && res.value);
//        }
//    }
};