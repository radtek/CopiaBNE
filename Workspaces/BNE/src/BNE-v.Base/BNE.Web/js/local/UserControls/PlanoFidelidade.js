function cvFuncaoExercida_Validate(sender, args) {
    var res = PlanoFidelidade.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function txtCriarPlanoAdicionalQuantidadeSMS_ValorAlteradoClient(sender, args) {
    var vlrQuantidade = employer.controles.recuperarValorControle('txtCriarPlanoAdicionalQuantidadeSMS');
    var vlrSMS = employer.controles.recuperarValorControle('txtCriarPlanoAdicionalValorSMS').replace('.', '').replace(',', '.');
    
    var vlrTotal = vlrQuantidade * vlrSMS;
    if (vlrTotal != '')
        employer.controles.setValorControle('txtCriarPlanoAdicionalValorTotal', vlrTotal.toFixed(2).replace('.', ','));
}

function txtCriarPlanoAdicionalValorSMS_ValorAlteradoClient(sender, args) {
    var vlrQuantidade = employer.controles.recuperarValorControle('txtCriarPlanoAdicionalQuantidadeSMS');
    var vlrSMS = employer.controles.recuperarValorControle('txtCriarPlanoAdicionalValorSMS').replace('.', '').replace(',', '.');
    
    var vlrTotal = vlrQuantidade * vlrSMS;
    if (vlrTotal != '')
        employer.controles.setValorControle('txtCriarPlanoAdicionalValorTotal', vlrTotal.toFixed(2).replace('.', ','));
}

function txtCriarPlanoAdicionalValorTotal_ValorAlteradoClient(sender, args) {
    var vlrQuantidade = employer.controles.recuperarValorControle('txtCriarPlanoAdicionalQuantidadeSMS');
    var vlrTotal = employer.controles.recuperarValorControle('txtCriarPlanoAdicionalValorTotal').replace('.', '').replace(',', '.');

    var vlrSMS = vlrTotal / vlrQuantidade;
    if (vlrSMS != '')
        employer.controles.setValorControle('txtCriarPlanoAdicionalValorSMS', vlrSMS.toFixed(4).replace('.', ','));
}
