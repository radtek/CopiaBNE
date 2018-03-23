
$(document).ready(function () {
   // $("#txtValorDesconto").mask("99.99%");
    ChangeDes();
});

function ChangeDes() {
    document.getElementById("lblValorComDesconto").innerHTML = document.getElementById("lblValorPlano").innerHTML;
    document.getElementById("hdfValorComDesconto").value = document.getElementById("lblValorPlano").innerHTML;
        
    //if (document.getElementById("rbFormaDesconto_0").checked) {
    //    $("#txtValorDesconto").mask("99.99%");
    //}
    //else {
    //    $("#txtValorDesconto").mask("R$ 99.99");
    //}

    calcDesconto();
}


    function calcDesconto() {
    var valorDesconto;
    var valorFinal;
    var valorDesconMax;
    $("#cvValorDesconto").hide();
    if (document.getElementById("rbFormaDesconto_0").checked){
         //calcula porcentagem/
       
        valorDesconto = document.getElementById("txtValorDesconto").value.replace("%", "").replace("R$", "")  * parseFloat(document.getElementById("lblValorPlano").innerHTML) / 100;

         valorFinal = parseFloat(document.getElementById("lblValorPlano").innerHTML) - valorDesconto;

          valorDesconMax = parseFloat(document.getElementById("lblValorPlano").innerHTML) * document.getElementById("percentDesconto").value / 100;
          document.getElementById("lblValorComDesconto").innerHTML = valorFinal.toFixed(2);
          document.getElementById("hdfValorComDesconto").value = valorFinal.toFixed(2).toString().replace(".", ",");
          if (valorDesconto > valorDesconMax) {
              exibirAvisoValor();
          }
          else {
              ocultarAviso();
          }

        }
    else {//calcula preço fixo
       
        valorFinal = parseFloat(document.getElementById("lblValorPlano").innerHTML) - document.getElementById("txtValorDesconto").value.replace("%", "").replace("R$", "")  ;
        valorDesconMax = parseFloat(document.getElementById("lblValorPlano").innerHTML) - (parseFloat(document.getElementById("lblValorPlano").innerHTML) * document.getElementById("percentDesconto").value / 100);
        document.getElementById("lblValorComDesconto").innerHTML = valorFinal.toFixed(2);
        document.getElementById("hdfValorComDesconto").value = valorFinal.toFixed(2).toString().replace(".",",");
        if (valorFinal < valorDesconMax) {
                exibirAvisoValor();
        }
        else {
            ocultarAviso();
        }
    }
    
}

function exibirAvisoValor() {
    document.getElementById("spAviso").innerHTML = "Valor Máximo de desconto é de " + document.getElementById("percentDesconto").value + "%";
    document.getElementById("lnkDesconto").disabled = true;
}

function ocultarAviso() {
    document.getElementById("spAviso").innerHTML = "";
    document.getElementById("lnkDesconto").disabled = false;
}

function valDesconto() {

    if (parseFloat(document.getElementById("hdfValorComDesconto").value) > -1 && parseFloat(document.getElementById("hdfValorComDesconto").value) < parseFloat(document.getElementById("lblValorPlano").innerHTML)){
        return true;
    }

    return false;       

}


function selectdItem() {
    var chkBox = document.getElementById("cblMotivoCancelar")
    var itens = chkBox.getElementsByTagName('input');

    for (var i = 0; i < itens.length; i++) {
        if (itens[i].checked && itens[i].value == 7) {//outros
            document.getElementsByClassName("togglevisibility")[0].style.display = "block"
        }
        else if (!itens[i].checked && itens[i].value == 7) {//outros
            document.getElementsByClassName("togglevisibility")[0].style.display = "none"
        }
    }
}


function valCalcDesconto(sender, args) {
    var isValid = false;
    if (parseFloat(document.getElementById("hdfValorComDesconto").value) > -1 && parseFloat(document.getElementById("hdfValorComDesconto").value) < parseFloat(document.getElementById("lblValorPlano").innerHTML)) {
        isValid = true;
        return;
    }
    args.IsValid = isValid;
}

function ValCheckBoxList(sender, args) {
    var checkBoxList = document.getElementById("cblMotivoCancelar");
    var checkboxes = checkBoxList.getElementsByTagName("input");
    var isValid = false;
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            isValid = true;
            break;
        }
    }
    args.IsValid = isValid;
}