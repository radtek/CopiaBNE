$(document).ready(function () {
    //Ajustando o background
    $("body").addClass("bg_fundo_empresa");
   
});

function OcultarBtnCancelar() {
    $('#cphConteudo_btnCancelarAssinatura').hide();
}

function closeModel() {
    document.getElementById("closeModal").click();
    $("#etapa02").hide();
    $("#etapa03").hide();
    $("#etapa04").hide();
    $("#btn-cancel-01").click(function () {
        if (document.getElementById("cphConteudo_hfTotalCv").value > 0) {
            $('#etapa02').show();
            $('#etapa01').hide();
        }
        else {
            $('#etapa03').show();
            $('#etapa01').hide();
        }
       
    });
    $("#btn-cancel-02").click(function () {
        $('#etapa03').show();
        $('#etapa02').hide();
    });
    $("#btn-cancel-03").click(function () {
        $('#etapa04').show();
        $('#etapa03').hide();
    });
    // Sub Opções 
    $(".motivo1-opcoes").hide();
    $('#motivo1').click(function () {
        if ($("#motivo1").is(':checked')) {
            document.getElementById("cphConteudo_rbCandidatoBNE").checked = true;
            $(".motivo1-opcoes").show();
        } else {
            document.getElementById("cphConteudo_rbCandidatoBNE").checked = false;
            document.getElementById("cphConteudo_rbIndicacao").checked = false;
            document.getElementById("cphConteudo_outro_site").checked = false;
            $(".motivo1-opcoes").hide();
        }
    });
    $(".motivo2-opcoes").hide();
    $('#cphConteudo_motivo2').click(function () {
        if ($("#cphConteudo_motivo2").is(':checked')) {
            $(".motivo2-opcoes").show();
        } else {
            $(".motivo2-opcoes").hide();
        }
    });
    $(".motivo3-opcoes").hide();
    $('#cphConteudo_motivo3').click(function () {
        if ($("#cphConteudo_motivo3").is(':checked')) {
            $(".motivo3-opcoes").show();
        } else {
            $(".motivo3-opcoes").hide();
        }
    });
    $(".motivo4-opcoes").hide();
    $('#cphConteudo_motivo4').click(function () {
        if ($("#cphConteudo_motivo4").is(':checked')) {
            $(".motivo4-opcoes").show();
        } else {
            $(".motivo4-opcoes").hide();
        }
    });
    $("#cphConteudo_qual").hide();
    $('#cphConteudo_outro_site').click(function () {
        if ($("#cphConteudo_outro_site").is(':checked')) {
            $("#cphConteudo_qual").show();
        } else {
            $("#cphConteudo_qual").hide();
        }
    });
    $("#cphConteudo_txtMotivoNaoConseguir").hide();
    $('#cphConteudo_ckNaoConsegui').click(function () {
        if ($('#cphConteudo_ckNaoConsegui').is(':checked')) {
            $('#cphConteudo_txtMotivoNaoConseguir').show();
        } else {
            $("#cphConteudo_txtMotivoNaoConseguir").hide();
        }
    })

    $('#cphConteudo_rbIndicacao').click(function () {
        if ($("#cphConteudo_rbIndicacao").is(':checked')) {
            $("#cphConteudo_qual").hide();
        }
    });
    $('#cphConteudo_rbCandidatoBNE').click(function () {
        if ($("#cphConteudo_rbCandidatoBNE").is(':checked')) {
            $("#cphConteudo_qual").hide();
        } 
    });
   

}

function Finalizar(){
    $('#etapa04').show();
    $('#etapa03').hide();
    $('#etapa02').hide();
    $('#etapa01').hide();
}

function Validar(){
    var checkList = document.getElementsByClassName("valCheck")
    for (var i = 0; i < checkList.length; i++) {
        if (checkList[i].checked) {
            $('#lblMensagem').hide();
            return true;
        }
    }
    $('#lblMensagem').show();
    return false;
}

function valCheck(ckb) {
    console.log(ckb);
    if (ckb.checked) {
        $('#lblMensagem').hide();
    }
       
}

function erroEncerrar() {
    $('#etapa03').show();
    $('#etapa04').hide();
    $('#etapa02').hide();
    $('#etapa01').hide();
    $('#lblMensagemErro').show();
    $(".motivo1-opcoes").hide();
    $(".motivo2-opcoes").hide();
    $(".motivo3-opcoes").hide();
    $("#cphConteudo_qual").hide();
}
