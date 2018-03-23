function cvCidade_Validate(sender, args) {
    var res = SolicitacaoR1.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}