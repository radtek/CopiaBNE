//Função responsável por aceitar somente numeros no textbox
function TravaTextBox() {
    tecla = event.keyCode;
    if (tecla >= 48 && tecla <= 57) {
        return true;
    }
    else {
        return false;
    }
}

var CartaoCredito = {
    digito1: '',
    digito2: '',
    digito3: '',
    digito4: ''
}

function DigitosCartaoCredito(parametros){
    CartaoCredito.digito1 = parametros.DigitosCartaoCredito1;
    CartaoCredito.digito2 = parametros.DigitosCartaoCredito2;
    CartaoCredito.digito3 = parametros.DigitosCartaoCredito3;
    CartaoCredito.digito4 = parametros.DigitosCartaoCredito4;
}

function ValidarTextBoxFormato(sender,args) {
   var txtCartao1 = employer.controles.recuperarValor(CartaoCredito.digito1);
   var txtCartao2 = employer.controles.recuperarValor(CartaoCredito.digito2);
   var txtCartao3 = employer.controles.recuperarValor(CartaoCredito.digito3);
   var txtCartao4 = employer.controles.recuperarValor(CartaoCredito.digito4);
       
    var valido = true;

    if (txtCartao1 != '' && txtCartao2 != '' && txtCartao3 != '' && txtCartao4 != '') {
        if (txtCartao1.length < 4 || txtCartao2.length < 4 || txtCartao3.length < 4 || txtCartao4.length < 4)
            valido = false;
    }
    args.IsValid = valido;   
}

function ValidarTextBox(sender, args) {
    var txtCartao1 = employer.controles.recuperarValor(CartaoCredito.digito1);
    var txtCartao2 = employer.controles.recuperarValor(CartaoCredito.digito2);
    var txtCartao3 = employer.controles.recuperarValor(CartaoCredito.digito3);
    var txtCartao4 = employer.controles.recuperarValor(CartaoCredito.digito4);
    var valido = true;
    if (txtCartao1 == '' || txtCartao2 == '' || txtCartao3 == '' || txtCartao4 == '')
        valido = false;

    args.IsValid = valido;
}