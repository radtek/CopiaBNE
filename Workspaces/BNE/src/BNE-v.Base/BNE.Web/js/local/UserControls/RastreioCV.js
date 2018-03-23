function cvCidade_Validate(sender, args) {
    var res = SalaSelecionadorCadastroRastreadorCV.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncao_Validate(sender, args) {
    var res = SalaSelecionadorCadastroRastreadorCV.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvRamo_Validate(sender, args) {
    var res = SalaSelecionadorCadastroRastreadorCV.ValidarAreaBNE(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvInstEnsino_Validate(sender, args) {
    var res = SalaSelecionadorCadastroRastreadorCV.ValidarInstEnsino(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function AjustarCampos() {
    AjustarCidade(employer.controles.recuperarValor('txtCidade'));
}

function txtCidade_onChange(sender) {
    AjustarCidade(sender.value);
}

function AjustarCidade(valor) {
    employer.controles.setAttr('txtBairro_txtValor', 'disabled', valor == '');
    employer.controles.setAttr('txtBairro', 'disabled', valor == '');
    employer.controles.setAttr('btiMapaBairro', 'disabled', valor == '');
    employer.util.findControl('lblAvisoCidadeBairro').css("display", valor != '' ? 'none' : '');


}

