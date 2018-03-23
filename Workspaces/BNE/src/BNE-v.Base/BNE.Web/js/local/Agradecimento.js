var IdAnterior;
var IdAtual;
var IdProximo;
var IdPadraoLi = 'li_carousel';
var ContadorGlobal = 0;

var IndexAtual = 0;

var Parametros = {
   // quantidadeAgradecimentos : ''
}

function InicializarRowId(parametros) {
   // Parametros.rowId = parametros.rowId;
   // Parametros.quantidadeAgradecimentos = parametros.quantidadeAgradecimentos;
}

function InicializarAgradecimentos()
{
    //Pega o id do maior agradecimento
    IdAtual = Agradecimentos.RetornarMaiorCodigoAgradecimento();

    //Criando uma UL e adicionando ao painel
    var panelInicio = document.getElementById('pnlAgradecimentosContainer');
    var ul = document.createElement('ul');
    ul.setAttribute('id', 'carousel_ul');
    panelInicio.appendChild(ul);

    //Recupera os Agradecimentos de acordo com o agradecimento desejado.
    var res = Agradecimentos.ListarAgradecimentos(IdAtual.value);  

    if (res.value && res.error == null) {
        var retorno = eval("(" + res.value + ")");

        //Ajustando os id's
        IdAnterior = retorno.IdAgradecimentoAnterior;
        IdAtual = retorno.IdAgradecimentoAtual;
        IdProximo = retorno.IdAgradecimentoProximo;

        //Criando os elementos da UL
        CreateLIAndAppendToUL(ul, retorno.IdAgradecimentoAnterior, retorno.MensagemAgradecimentoAnterior, retorno.UsuarioAgradecimentoAnterior, retorno.CidadeAgradecimentoAnterior);
        CreateLIAndAppendToUL(ul, retorno.IdAgradecimentoAtual, retorno.MensagemAgradecimentoAtual, retorno.UsuarioAgradecimentoAtual, retorno.CidadeAgradecimentoAtual);
        CreateLIAndAppendToUL(ul, retorno.IdAgradecimentoProximo, retorno.MensagemAgradecimentoProximo, retorno.UsuarioAgradecimentoProximo, retorno.CidadeAgradecimentoProximo);

        IndexAtual = 1;

        //Ajustando o agradecimento atual
        VisualizarAgradecimento(IdAnterior);
    }
}

function CreateLIAndAppendToUL(ul, idAgradecimento, descricaoAgradecimento, usuario, cidade) {
    var li = document.createElement('li');
    var textNode = CriarTextNodeAgradecimento(idAgradecimento, descricaoAgradecimento, usuario, cidade);
    li.appendChild(textNode);
    li.setAttribute('id', IdPadraoLi + '_' + idAgradecimento + '_' + ContadorGlobal++);
    li.setAttribute('class', 'balao_agradecimento');
    ul.appendChild(li);

}

function CreateLIAndInsertBeforeToUL(ul, idAgradecimento, descricaoAgradecimento, usuario, cidade) {
    var li = document.createElement('li');
    var textNode = CriarTextNodeAgradecimento(idAgradecimento, descricaoAgradecimento, usuario, cidade);
    li.appendChild(textNode);
    li.setAttribute('id', IdPadraoLi + '_' + idAgradecimento + '_' + ContadorGlobal++);
    li.setAttribute('class', 'balao_agradecimento');
    ul.insertBefore(li, ul.childNodes[0]);

    //Ajustando indice atual, pois sempre que inserir algum elemento antes de todos significa que estamos remontando o indice
    IndexAtual = 2;
}

function CriarTextNodeAgradecimento(idAgradecimento, descricaoAgradecimento, usuarioAgradecimento, cidadeAgradecimento) {
    //Ajustando o texto para não estourar a DIV
    if (descricaoAgradecimento.length > 150)
        descricaoAgradecimento = descricaoAgradecimento.substring(0, 150) + '...' + '<div class="icone_mais_agradecimento" onclick="VisualizarAgradecimento(' + idAgradecimento + ');" title="Ler Agradecimento"></div>';
    
    //Setando o texto
    var agradecimento = '<div class="balao_agradecimento" onclick="javascript:VisualizarAgradecimento(' + idAgradecimento + ');"><p class="texto">' + descricaoAgradecimento + '</p><div class="agradecimento_usuario"><i class="fa fa-user fa-2x boneco_agradecimento"></i><div class="assinatura_agradecimento">' + usuarioAgradecimento + '</div><div class="cidade_assinatura_agradecimento">' + cidadeAgradecimento + '</div></div></div>';
    
    //Criando uma DIV e adicionando o texto em seu HTML
    var div = document.createElement('div');
    div.className = "agradecimento_over";
    div.innerHTML = agradecimento;
    return div;
}

function SlideCarrousel(direcao)
{
    var ul = document.getElementById('carousel_ul');
    var bool = CriarTag(ul, direcao);
    Animar(direcao, bool);
    //CleanUL(ul, IdAtual);
}

function Animar(direcao, ajustaLeft) {
    var item_width = $('#carousel_ul li').outerWidth() + 0;
    var left_indent;
    
    if (direcao == 'Esquerda') {
        IndexAtual--;
        left_indent = parseInt($('#carousel_ul').css('left')) + item_width;
        
        //Ajuste, pois foi adicionado um LI que não existia
        if (ajustaLeft) {
            left_indent -= item_width;
            $('#carousel_ul').css('left', left_indent - item_width);
        }
    } else {
        IndexAtual++;
        left_indent = parseInt($('#carousel_ul').css('left')) - item_width;
    }

    $('#carousel_ul').animate({ top: '0px', left: left_indent }, 300);

    //Easter egg
    if (left_indent < -9000)
        $('#carousel_ul').css('width', (-left_indent + 1000) + 'px');
}

function CriarTag(ul, direcao) {
    var res;
    if (direcao == 'Esquerda') {
        res = Agradecimentos.ListarAgradecimentos(IdAnterior);
    }
    else {
        res = Agradecimentos.ListarAgradecimentos(IdProximo);
    }

    if (res.value && res.error == null) {
        var retorno = eval("(" + res.value + ")");

        IdAnterior = retorno.IdAgradecimentoAnterior;
        IdAtual = retorno.IdAgradecimentoAtual;
        IdProximo = retorno.IdAgradecimentoProximo;

        if (direcao == 'Esquerda') {
            if (!ExistsLIIndex(ul, retorno.IdAgradecimentoAtual, retorno.IdAgradecimentoAnterior, null)) {
                CreateLIAndInsertBeforeToUL(ul, retorno.IdAgradecimentoAnterior, retorno.MensagemAgradecimentoAnterior, retorno.UsuarioAgradecimentoAnterior, retorno.CidadeAgradecimentoAnterior);
                return true;
            }
        }
        else {
            if (!ExistsLIIndex(ul, retorno.IdAgradecimentoAtual, null, retorno.IdAgradecimentoProximo))
                CreateLIAndAppendToUL(ul, retorno.IdAgradecimentoProximo, retorno.MensagemAgradecimentoProximo, retorno.UsuarioAgradecimentoProximo, retorno.CidadeAgradecimentoProximo);
        }
    }
    return false;
}

function CleanUL(ul, idLI) {
    for (x = 0; x < ul.childNodes.length; x++) {
        if (IdPadraoLi + '_' + idLI == ul.childNodes[x].id) {
            for (y = 0; y < ul.childNodes.length; y++) {
                if (y < x - 3 || y > x + 3) {
                    if (typeof (ul.childNodes[y]) != 'undefined') {
                        ul.replaceChild(ul.childNodes[y]);
                    }
                }
            }
        }
    }
}

function ExistsLI(ul, idLIAtual, idLIAnterior, idLIProxima) {
    for (x = 0; x < ul.childNodes.length; x++) {
        if (ul.childNodes[x].id.indexOf((IdPadraoLi + '_' + idLIAtual + '_')) != -1) {
            if (idLIAnterior != null) {
                if (typeof (ul.childNodes[x - 1]) != 'undefined') {
                    if (ul.childNodes[x - 1].id.indexOf((IdPadraoLi + '_' + idLIAnterior+ '_')) != -1)
                    return true;
                }
            }
            else if (idLIProxima != null) {
                if (typeof (ul.childNodes[x + 1]) != 'undefined') {
                    if (ul.childNodes[x + 1].id.indexOf((IdPadraoLi + '_' + idLIProxima+ '_')) != -1)
                    return true;
                }
            }
        }
    }
    return false;
}

function ExistsLIIndex(ul, idLIAtual, idLIAnterior, idLIProxima) {
    if (idLIAnterior != null) {
        if (typeof (ul.childNodes[IndexAtual - 2]) != 'undefined') {
            if (ul.childNodes[IndexAtual - 2].id.indexOf((IdPadraoLi + '_' + idLIAnterior + '_')) != -1)
                return true;
        }
    }
    else if (idLIProxima != null) {
        if (typeof (ul.childNodes[IndexAtual + 2]) != 'undefined') {
            if (ul.childNodes[IndexAtual + 2].id.indexOf((IdPadraoLi + '_' + idLIProxima + '_')) != -1)
                return true;
        }
    }
    return false;
}

function RemoverElemento(ul, direcao) {
    if (+(ul.childNodes.length) > 5) {
        if (direcao == 'Esquerda')
            ul.removeChild(ul.childNodes[ul.childNodes.length-1])
        else
            ul.removeChild(ul.childNodes[0])
    }
}

function MeuAgradecimento() {
    var painelMeuAgradecimento = employer.util.findControl('divEnviarAgradecimentos');
    var painelVisualizarAgradecimento = employer.util.findControl('pnlVisualizarAgradecimento');

    painelMeuAgradecimento.css('display', 'block');
    painelVisualizarAgradecimento.css('display', 'none');
}

function VisualizarAgradecimento(idAgradecimento) {
    var painelMeuAgradecimento = employer.util.findControl('divEnviarAgradecimentos');
    var painelVisualizarAgradecimento = employer.util.findControl('pnlVisualizarAgradecimento');

    painelVisualizarAgradecimento.innerHTML = '';

    //Recupera os Agradecimentos de acordo com o agradecimento desejado.
    var res = Agradecimentos.ListarAgradecimentos(idAgradecimento);

    if (res.value && res.error == null) {
        var retorno = eval("(" + res.value + ")");

        var agradecimento = '<div class="ver_agradecimento"><div class="apostrofo_superior"><img src="/img/agradecimentos/apostrofo_abre.png" width: "30px" height="20px" alt="icone aspas"/></div><div class="agradecimento_usuario"><div class="texto"><p>' + retorno.MensagemAgradecimentoAtual + '</p></div></div><div class="apostrofo_inferior"><img src="/img/agradecimentos/apostrofo_fecha.png"  width: "30px" height="20px"  alt="icone aspas"/></div></div></div><div class="assinatura_agradecimento_rodape"><div class="agradecimento_usuario_rodape"><i class="fa fa-user fa-3x boneco_agradecimento"></i><div class="assinatura_agradecimento">' + retorno.UsuarioAgradecimentoAtual + '</div> <div class="cidade_assinatura_agradecimento"> ' + retorno.CidadeAgradecimentoAtual + '</div></div>';
        
        var div = document.createElement('div');
        div.innerHTML = agradecimento;

        //Limpandos todos os controles.
        painelVisualizarAgradecimento.children().remove();
        painelVisualizarAgradecimento[0].appendChild(div);
        //CreateLIAndAppendToUL(ul, retorno.IdAgradecimentoAtual, retorno.MensagemAgradecimentoAtual, retorno.UsuarioAgradecimentoAtual, retorno.CidadeAgradecimentoAtual);

        painelMeuAgradecimento.css('display', 'none');
        painelVisualizarAgradecimento.css('display', 'block');

        CentralizarDescricaoAgradecimentos();
    }

}

function cvCidade_Validate(sender, args) {
    var res = Agradecimentos.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function CentralizarDescricaoAgradecimentos() {
    paddingCentro = (($(".painel_agradecimento .texto").height() - $(".painel_agradecimento .texto p").height()) / 2);
    $(".painel_agradecimento .texto p").css("padding-bottom", paddingCentro);
    $(".painel_agradecimento .texto p").css("padding-top", paddingCentro);
}



        $("img[id$='imgEsquerda']").live("mouseover",
            function () {
                $(this).attr("src", "/img/agradecimentos/icon_seta_esq_over.png");
            }
        );

        $("img[id$='imgEsquerda']").live("mouseout",
            function () {
                $(this).attr("src", "/img/agradecimentos/icon_seta_esq.png");
            }
        );

            $("img[id$='imgDireita']").live("mouseover",
            function () {
                $(this).attr("src", "/img/agradecimentos/icon_seta_dir_over.png");
            }
        );

            $("img[id$='imgDireita']").live("mouseout",
            function () {
                $(this).attr("src", "/img/agradecimentos/icon_seta_dir.png");
            }
        );