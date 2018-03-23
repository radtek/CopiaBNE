function ccbEstado_ClientCheckItem() {
    employer.util.findControl('btnCarregarCidades')[0].click();
}

function cvFilial_Validate(sender, args) {
    var res = EmailParaFilial.ValidarFilial(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function txtFilial_OnChange(args) {
    var cvFilial = employer.util.findControl('cvFilial');
    ValidatorValidate(cvFilial[0]);
}