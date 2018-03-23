function ValidarEnderecoSTC(sender, args) {
    var res = SiteTrabalheConoscoCriacao.ValidarEnderecoSTC(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function AjustarTema() {
    switch ($("*[id$='hfTemplate']").val()) {
        case "Amarelo": $("*[id$='btnEscolhaTemaAmarelo']").click(); break;
        case "Azul": $("*[id$='btnEscolhaTemaAzul']").click(); break;
        case "Cinza": $("*[id$='btnEscolhaTemaCinza']").click(); break;
        case "Laranja": $("*[id$='btnEscolhaTemaLaranja']").click(); break;
        case "Verde": $("*[id$='btnEscolhaTemaVerde']").click(); break;
        case "Vermelho": $("*[id$='btnEscolhaTemaVermelho']").click(); break;
        default: $("*[id$='btnEscolhaTemaAmarelo']").click(); break;
    }
}

function MarcarTemaSelecionado(elemento) {
    $(".botoes_escolha_tema input").removeClass("tema_selecionado");
    $("#" + elemento).addClass("tema_selecionado");
}

function pageLoad() {
    //Preload de imagens
    var imgPreloadResultadoAmarelo = new Image();
    var imgPreloadResultadoAzul = new Image();
    var imgPreloadResultadoCinza = new Image();
    var imgPreloadResultadoLaranja = new Image();
    var imgPreloadResultadoVerde = new Image();
    var imgPreloadResultadoVermelho = new Image();

    imgPreloadResultadoAmarelo.src = "/img/img_tema_resultado_amarelo.jpg";
    imgPreloadResultadoAzul = "/img/img_tema_resultado_azul.jpg";
    imgPreloadResultadoCinza = "/img/img_tema_resultado_cinza.jpg";
    imgPreloadResultadoLaranja = "/img/img_tema_resultado_laranja.jpg";
    imgPreloadResultadoVerde = "/img/img_tema_resultado_verde.jpg";
    imgPreloadResultadoVermelho = "/img/img_tema_resultado_vermelho.jpg";

    //Ajustando o background
    $("body").addClass("bg_fundo_empresa");

    AjustarTema();
}

//Seleciona o tema Amarelo
$("*[id$='btnEscolhaTemaAmarelo']").live('click',
            function () {
                $("*[id$='imgMiniaturaResultado']").attr("src", "/img/img_tema_resultado_amarelo.jpg");
                MarcarTemaSelecionado($(this).attr("id"));
                $("*[id$='hfTemplate']").val("Amarelo");
            }
        );

//Seleciona o tema Azul
$("*[id$='btnEscolhaTemaAzul']").live('click',
            function () {
                $("*[id$='imgMiniaturaResultado']").attr("src", "/img/img_tema_resultado_azul.jpg");
                MarcarTemaSelecionado($(this).attr("id"));
                $("*[id$='hfTemplate']").val("Azul");
            }
        );

//Seleciona o tema Cinza
$("*[id$='btnEscolhaTemaCinza']").live('click',
            function () {
                $("*[id$='imgMiniaturaResultado']").attr("src", "/img/img_tema_resultado_cinza.jpg");
                MarcarTemaSelecionado($(this).attr("id"));
                $("*[id$='hfTemplate']").val("Cinza");
            }
        );

//Seleciona o tema Laranja
$("*[id$='btnEscolhaTemaLaranja']").live('click',
            function () {
                $("*[id$='imgMiniaturaResultado']").attr("src", "/img/img_tema_resultado_laranja.jpg");
                MarcarTemaSelecionado($(this).attr("id"));
                $("*[id$='hfTemplate']").val("Laranja");
            }
        );

//Seleciona o tema Verde
$("*[id$='btnEscolhaTemaVerde']").live('click',
            function () {
                $("*[id$='imgMiniaturaResultado']").attr("src", "/img/img_tema_resultado_verde.jpg");
                MarcarTemaSelecionado($(this).attr("id"));
                $("*[id$='hfTemplate']").val("Verde");
            }
        );

//Seleciona o tema Vermelho
$("*[id$='btnEscolhaTemaVermelho']").live('click',
            function () {
                $("*[id$='imgMiniaturaResultado']").attr("src", "/img/img_tema_resultado_vermelho.jpg");
                MarcarTemaSelecionado($(this).attr("id"));
                $("*[id$='hfTemplate']").val("Vermelho");
            }
        );