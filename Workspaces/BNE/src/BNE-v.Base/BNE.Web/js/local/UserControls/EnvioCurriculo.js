function cvEmailEnviarPara_Validate(sender, args) {

    var emails = employer.controles.recuperarValor('txtEnvioPara');
    var mensagem = EnvioCurriculo.ValidarEmail(emails);

    if (mensagem.error == null)
    {
        if (!mensagem.value) 
        {
            args.IsValid = true;
        }
        else 
        {
            args.IsValid = false;
            employer.controles.setAttr(sender.id, 'innerText', mensagem.value);
        }
    }

}