function cvFuncaoPretendida_Validate(sender, args) {
    var res = DadosPessoais.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function txtFuncaoPretendida_OnChange(args) {
    var mediaSalarial = DadosPessoais.PesquisarMediaSalarial(employer.controles.recuperarValor('txtFuncaoPretendida'));

    if (mediaSalarial.value != null && mediaSalarial.value != 0) {
        var valorMaximo = mediaSalarial.value * 1.5;
        var valorMinimo = mediaSalarial.value * 0.5;

        employer.controles.setAttr('txtPretensaoSalarial', 'ValorMaximo', valorMaximo);
        employer.controles.setAttr('txtPretensaoSalarial', 'valorMinimo', valorMinimo);
    }

    var valorPretensao = employer.controles.recuperarValorControle('txtPretensaoSalarial');
    DadosPessoais.SalvarCadastro(args.value, valorPretensao, '', '');
}

function txtPretensaoSalarial_ValorAlterado(args) {
    DadosPessoais.SalvarCadastro('', employer.util.findControl(args.NomeCampoValor)[0].value, '', '');
}

function txtTelefoneCelular_ValorAlterado(args) {
    DadosPessoais.SalvarCadastro('', '', employer.util.findControl(args.NomeCampoValor)[0].value, employer.util.findControl(args.NomeCampoValor)[0].parentElement.childNodes[1].value);
}

function txtTelefoneCelularDDD_ValorAlterado(args) {
    DadosPessoais.SalvarCadastro('', '', employer.util.findControl(args.NomeCampoValor)[0].parentElement.childNodes[2].value, employer.util.findControl(args.NomeCampoValor)[0].value);
}
