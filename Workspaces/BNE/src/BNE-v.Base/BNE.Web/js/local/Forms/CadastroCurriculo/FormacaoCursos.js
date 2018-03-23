function cvCidadeComplementar_Validate(sender, args) {
    var res = FormacaoCursos.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvCidade_Validate(sender, args) {
    var res = FormacaoCursos.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvTituloCursoComplementar_Validate(sender, args) {
    args.IsValid = true;
}

function cvTituloCurso_Validate(sender, args) {
    args.IsValid = true;
}

function ddlSituacao_SelectedIndexChanged(args) {
    ValidatorValidate(employer.util.findControl('cvSituacao')[0]);
}

function cvSituacao_Validate(sender, args) {
    var value = employer.util.findControl('ddlSituacao')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}