function BarraRolagemModoEdicaoUsuario(){
    window.scroll(0, 0);
}

$(document).ready(function () {
    autocomplete.funcao("txtFuncaoExercida");
});


function checkResponsavel(radio, id) {
    var listRadio = document.querySelectorAll("input[type=radio]");
    for (var i = 0; i < listRadio.length; i++) {
        if (listRadio[i].value == "rbResponsavel") {
            listRadio[i].checked = false;
        }
    }
    document.getElementById("cphRodape_hfIdResponsavel").value = id;
    radio.checked = true;
}