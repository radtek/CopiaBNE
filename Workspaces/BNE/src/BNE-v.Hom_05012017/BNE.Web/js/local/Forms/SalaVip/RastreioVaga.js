function cvCidade_Validate(sender, args) {
    var res = RastreioVaga.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncao_Validate(sender, args) {
    var res = RastreioVaga.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}