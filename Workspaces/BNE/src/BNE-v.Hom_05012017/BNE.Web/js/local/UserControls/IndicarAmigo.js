function cvEmailIndiqueBNE_Validate(sender, args) {
    var email = employer.controles.recuperarValor('txtEmailIndiqueBNE');
    var res = IndicarAmigo.ValidarEmail(email);
    args.IsValid = (res.error == null && res.value);
}

function cvEmailAmigoIndiqueBNE_Validate(sender, args) {
    var email = employer.controles.recuperarValor('txtEmailAmigoIndiqueBNE');
    var res = IndicarAmigo.ValidarEmail(email);
    args.IsValid = (res.error == null && res.value);
}

