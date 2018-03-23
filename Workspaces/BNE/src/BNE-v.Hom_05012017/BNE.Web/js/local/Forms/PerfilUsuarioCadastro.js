var AutoCompleteExtenders = {
    perfilModelo: 'ctl00_cphPrincipal_acPerfilModelo'
}

//Responsável por validar se o sindicato informado é válido.
function cvPerfilModelo_clientValidate(sender, args) {

    if (args == null || args.Value.lenght == 0) {
        args.IsValid = false;

        return;
    }

    //Valida o sindicato
    var res = PerfilUsuarioCadastro.ValidarPerfil(args.Value)
    args.IsValid = (res.error == null && res.value);
}

//Valor alterado do textbox de perfil modelo. 
//Responsável por configurar dados que dependem do perfil modelo.
function txtPerfil_ValorAlterado(args) {

    //Se o sindicato for válido deve-se executar as regras do campo.
    var res = PerfilUsuarioCadastro.AjustarPerfil(args.value)

    if (res != null && res.error == null && res.value != null && res.value != "") {
        employer.controles.setValor('hfIdPerfilModelo', res.value);
    }
    else {
        employer.controles.setValor('hfIdPerfilModelo', "");
    }
}