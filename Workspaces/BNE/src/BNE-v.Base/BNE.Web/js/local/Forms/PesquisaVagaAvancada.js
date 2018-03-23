function cvCidade_Validate(sender, args) {
    var res = PesquisaVagaAvancada.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncao_Validate(sender, args) {
    var res = PesquisaVagaAvancada.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function txtCidade_onBlur(sender) {
    var res = PesquisaVagaAvancada.RecuperarCidade(sender.value);

    if (res.error == null && res.value)
        employer.controles.setValor(sender.id, res.value);
}

function txtSalarioDe_ValorAlterado(args) {
    var valor = employer.controles.recuperarValor(args.NomeCampoValor);
    if (valor == '')
        return;
    employer.controles.setAttr('txtSalarioAte', 'ValorMinimo', valor);
}