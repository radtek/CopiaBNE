function cvFuncaoPesquisaSalarial_Validate(sender, args) {
    var res = PesquisaSalarial.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvCidadePesquisaSalarial_Validate(sender, args) {
    var res = PesquisaSalarial.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

