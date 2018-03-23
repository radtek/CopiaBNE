function cvRazaoSocial_Validate(sender, args) {
    var res = ManutencaoRhOffice.ValidarRazaoSocial(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvCandidatoNaoLodago_Validate(sender, args) {
    var value = employer.util.findControl('ddlCandidatoNaoLogado')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;

}

function cvCandidatoLogado_Validate(sender, args) {
    var value = employer.util.findControl('ddlCandidatoLogado')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

function cvUsuarioMaster_Validate(sender, args) {
    var value = employer.util.findControl('ddlUsuarioMaster')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

function cvUsuario_Validate(sender, args) {
    var value = employer.util.findControl('ddlUsuario')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

function txtRazaoSocial_Changed(args) {
    var numCnpj = ManutencaoRhOffice.RecuperarCNPJPorRazaoSocial(args.value);
    employer.controles.setValorControle('txtCNPJ', numCnpj.value);
}

