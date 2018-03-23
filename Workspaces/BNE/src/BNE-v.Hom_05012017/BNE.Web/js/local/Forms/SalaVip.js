
function txtAtividadeExercida_KeyUp() {
    contarCaracteres(document.getElementById('cphConteudo_ucDados_txtAtividadeExercida_txtValor').value, 'GraficoQualidade1');
}

//Acerta a altura das linhas de resultado das tabs (executa a cada 1 segundo)
setInterval(
    function()
    {
        $(".conteudo_uctabs").each(
            function()
            {
                //Se a coluna da esquerda for mais alta que a da direita, define para a
                //da direita a altura da esquerda
                if( $(this).children().eq(0).height() > $(this).children().eq(1).height() )
                {
                    $(this).children().eq(1).height( $(this).children().eq(0).height() );
                }
                
                //Se a coluna da direita for mais alta que a da esquerda, define para a
                //da esquerda a altura da direita
                if( $(this).children().eq(1).height() > $(this).children().eq(0).height() )
                {
                    $(this).children().eq(0).height( $(this).children().eq(1).height() );
                }
            }
        );
    }
, 1000);

function cvFuncao_Validate(sender, args) {
    var res = SalaVip.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function AbrirPopup(pagina, h, w) {
    var iTop, iLeft, features

    iTop = (window.screen.availHeight - h) / 2
    iLeft = (window.screen.availWidth - w) / 2

    features = 'top=' + iTop + ',left=' + iLeft + ',height=' + h + ',width=' + w + ',scrollbars=yes';

    novaJanela = window.open(pagina, 'pop3', features);
    novaJanela.focus();
}


function ImprimirMeuCurriculo(divImprimir) 
{
    var wrapperDiv = employer.util.findControl(divImprimir)[0];
    var printIframe = document.createElement("IFRAME");
    document.body.appendChild(printIframe);
    var printDocument = printIframe.contentWindow.document;
    printDocument.designMode = "on";
    printDocument.open();
    printDocument.write("<html><head></head><body>" + wrapperDiv.innerHTML + "</body></html>");
    printIframe.style.position = "absolute";
    printIframe.style.top = "-1000px";
    printIframe.style.left = "-1000px";

    if (document.all) {
        printDocument.execCommand("Print", null, false);
    }
    else {
        printDocument.contentWindow.print();
    }
}


$(document).ready(
    function () {
        //Ajustando o background
        $("body").addClass("bg_fundo_empresa");
    }

       
);

function selectdItem() {
    var chkBox = document.getElementById("cphConteudo_cblMotivoCancelar")
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
function OcultarBtnCancelar() {
    $('#cphConteudo_btnCancelarAssinatura').hide();
    trackEvent('vip-meu-plano', 'Click', 'CancelarPlanoRecorrenteIniciou');
}

function closeModel() {
    document.getElementById("closeModal").click()
}



function ValCheckBoxList(sender, args) {
    var checkBoxList = document.getElementById("cphConteudo_cblMotivoCancelar");
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
/*
//Accordion
function btiConviteEntrevista_OnClick() {
showMenu('#MenuConviteEntrevista');
}

function btiQuemMeViu_OnClick() {
showMenu('#MenuQuemMeViu');
}

function btiEnviarCurriculo_OnClick() {
showMenu('#MenuEnviarCurriculo');
}

function btiJaEnviei_OnClick() {
showMenu('#MenuJaEnviei');
}

function btiMensagensRecebidas_OnClick() {
showMenu('#MenuMensagensRecebidas');
}

function showMenu(menuAtual) {
/// <summary>
/// Mostra os menu selecionado e esconde os outros menus caso estejam abertos.
/// </summary>     
if (menuAtual != undefined && typeof (menuAtual) != undefined && menuAtual != '') {
$('#MenuConviteEntrevista').slideUp();
$('#MenuQuemMeViu').slideUp();
$('#MenuEnviarCurriculo').slideUp();
$('#MenuJaEnviei').slideUp();
$('#MenuMensagensRecebidas').slideUp();

if ($(menuAtual).css("display") === "none") {
$(menuAtual).slideDown();
}
else {
$(menuAtual).slideUp();
//$('.atalho_teclado').fadeOut();
}
}
}
//FIM: Accordion
*/