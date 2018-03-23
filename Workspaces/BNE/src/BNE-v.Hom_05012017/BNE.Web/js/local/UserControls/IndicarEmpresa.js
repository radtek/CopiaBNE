function cvCidade_Validate(sender, args) {
    var res = IndicarEmpresa.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

$(document).ready(function () {
    autocomplete.cidade("txtCidade");
});
