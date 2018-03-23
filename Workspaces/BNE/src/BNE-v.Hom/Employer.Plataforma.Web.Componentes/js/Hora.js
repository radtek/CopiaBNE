function AplicarMascaraHora(controle) {
    var valor = Trim(controle.value); 
    if (valor == '')
        return;
        
    if(valor.match(/(^([01][0-9]|2[0-3])[:]([0-5][0-9])$)|(^\d{1,4}$)/))
    { 
        valor = valor.replace(':', '');
        switch(valor.length)
        {
            case 1:                
                valor = '0' + valor + "00"; 
                break;
            case 2:                
                valor += "00"; 
                break;
            case 3:                
                valor = '0' + valor; 
                break;
        }
            
        valor = valor.replace(/(\d{2})(\d{2})/,"$1:$2"); 
    }
    controle.value = valor;
}

function RemoverMascaraHora(controle)
{
    var valor = Trim(controle.value);
    if(valor.match(/(^([01][0-9]|2[0-3])[:]([0-5][0-9])$)|(^\d{1,4}$)/)) 
        controle.value = valor.replace(':', '');
    else
        controle.value = valor;
        
   controle.select();
}

function Validar(sender, args) {
    var controle = sender.parentNode.parentNode;
    AplicarMascaraHora($get(controle.id + "_txtValor"));
    ValidarHora(sender, args);
}

function ValidarHora(sender, args) {
    var controle = sender.parentNode.parentNode;
    var valor = Trim($get(controle.id + "_txtValor").value);

    var msgErro, msgErroSummary;
    if (valor == "") {
        sender.firstChild.nodeValue = controle.getAttribute("MensagemErroObrigatorio");
        msgErro = controle.getAttribute("MensagemErroObrigatorio");
        msgErroSummary = controle.getAttribute("MensagemErroObrigatorioSummary");
        args.IsValid = ((controle.getAttribute("Obrigatorio") != true) || (controle.getAttribute("Obrigatorio") != "1"));
    }
    else {
        sender.firstChild.nodeValue = controle.getAttribute("MensagemErroFormato");
        msgErro = controle.getAttribute("MensagemErroFormato");
        msgErroSummary = controle.getAttribute("MensagemErroFormatoSummary");
        if (valor.match(/\d{2}:\d{2}/)) {
            var res = Hora.Validar(valor);
            var retorno = (res.error == null && res.value)

            args.IsValid = retorno;
            if(retorno){
                var hr = valor.substring(0, 2);
                var mn = valor.substring(3,5);

                sender.firstChild.nodeValue = controle.getAttribute("MensagemErroIntervalo");
                msgErro = controle.getAttribute("MensagemErroIntervalo");
                msgErroSummary = controle.getAttribute("MensagemErroIntervaloSummary");

                if($get(controle.id + "_hfHoraMaxima").value != ""){
                    var hrMax = $get(controle.id + "_hfHoraMaxima").value.substring(0, 2);
                    var mnMax = $get(controle.id + "_hfHoraMaxima").value.substring(3, 5);
                    if((hr > hrMax) || (hr == hrMax && mn > mnMax)){
                        args.IsValid = false;    
                    }
                }
                if($get(controle.id + "_hfHoraMinima").value != ""){
                    var hrMin = $get(controle.id + "_hfHoraMinima").value.substring(0, 2);
                    var mnMin = $get(controle.id + "_hfHoraMinima").value.substring(3, 5);
                    if((hr < hrMin) || (hr == hrMin && mn < mnMin)){
                        args.IsValid = false;
                    }
                }
            }
        } else {
            args.IsValid = false;
        }
    }
    try {
        var div = document.getElementById(controle.id + "_pnlValidador");
        var validador = $get(controle.id + "_cvValor");
        validador.innerHTML = msgErro;
        validador.errormessage = msgErroSummary;
        if (args.IsValid) {
            div.style.display = "none";
            validador.style.visibility = "hidden"
        }
        else {
            validador.style.visibility = "visible"
            div.style.display = "block";
        }
    } catch (e) {

    }
}