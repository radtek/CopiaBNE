var itemCount = 0;
var cnpj;
var filial;
var qtdeCandidaturas = 1;
var spanSelecionado;

var actions = {
    autocompletefuncao: 'ajax/autocompletefuncao',
    autocompletecidade: 'ajax/autocompletecidade',
    autocompletefonte: 'ajax/autocompletefonte',
    buscarfilial: 'ajax/f',
    logofilial: 'home/logofilial/',
    signout: 'ajax/signout',
    logado: 'ajax/logado',
    candidaturas: 'ajax/qc',
    carta: 'ajax/ca',
    interesse: 'ajax/seinteressar',
    segundatela: 'ajax/segundatela',
    terceiratela: 'ajax/terceiratela',
    quartatela: 'ajax/quartatela',
    quintatela: 'ajax/quintatela',
    sextatela: 'ajax/sextatela',
    signinc: 'ajax/signinc',
    dadospessoa: 'ajax/dp',
    fotopessoa: 'home/fotopessoa',
    vipOk: 'ajax/vipok',
    vip: 'ajax/vip',
    dadosfacebook: 'ajax/fb',
    curriculo: 'ajax/cv',
    validaCelular: 'ajax/vc',
};

$(function () {

    (function gaq() {
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        var uri = document.baseURI || document.URL;

        var baseAddress = uri.replace("http://", "").replace("https://", "");
        if (baseAddress.indexOf("www.bne.com.br") == 0 || baseAddress.indexOf("bne.com.br") == 0) {
            ga('create', 'UA-1937941-6', 'auto');
            ga('send', 'pageview');
        }
        else {
            ga('create', 'UA-1937941-9', 'auto');
            ga('send', 'pageview');
        }
    })();

    // acumula anti-forgery token nas requisicoes ajax
    cacheToken();

    (function inicializar() {
        $(window).scroll(function () {
            if (itemCount > 0 && (($(window).scrollTop() + $(window).height() > 0.90 * $(document).height()))) {
                loadMoreEmpresas(itemCount);
            }
        });

        employer.controle.mvc.aplicarBehaviorCPF('txtCpf');
        employer.controle.mvc.aplicarBehaviorTelefoneDD('txtDDD');
        employer.controle.mvc.aplicarBehaviorTelefone('txtNumCelular');
        employer.controle.mvc.aplicarBehaviorTelefoneDD('txtDDDRecado');
        employer.controle.mvc.aplicarBehaviorTelefone('txtTelefoneRecado');

        $("#txtSalario,#txtUltimoSalario").maskMoney({ decimal: ',', thousands: '', showSymbol: false });

        employer.controle.mvc.aplicarBehaviorCEP('txtCep');
        employer.controle.mvc.aplicarBehaviorData('txtDataNasc');
        employer.controle.mvc.aplicarBehaviorData('txtDataAdmissao1');
        employer.controle.mvc.aplicarBehaviorData('txtDataAdmissao2');
        employer.controle.mvc.aplicarBehaviorData('txtDataAdmissao3');
        employer.controle.mvc.aplicarBehaviorData('txtDataDemissao1');
        employer.controle.mvc.aplicarBehaviorData('txtDataDemissao2');
        employer.controle.mvc.aplicarBehaviorData('txtDataDemissao3');

        var anoEscolaridade = $("#txtAnoConclusaoEscolaridade");
        anoEscolaridade.inputmask({ mask: "9999" });

        var periodoEscolaridade = $("#txtEscolaridadePeriodo");
        periodoEscolaridade.inputmask({ mask: "99" });

        $("#divMenuLogado").hide();

        habilitarMenu();

        $("#accordionProfissional").accordion({ autoHeight: false }).children().removeClass("ui-widget-content").end().removeClass("ui-widget");

        $("#txtEscolaridadeCidade").autocomplete({
            source: function (request, response) {
                $.post(actions.autocompletecidade, request, response);
            },
            minLength: 3,
            change: function (e, u) {
                if (u && u.item)
                    $("#hidEscolaridadeCidade").val(u.item.id);
            },
            error: function (x, s, e) { mostrarErro(x.statusText); }
        });

        $("#txtEscolaridadeInstituicaoEnsino").autocomplete({
            source: function (request, response) {
                $.post(actions.autocompletefonte, request, response);
            },
            minLength: 2,
            change: function (e, u) {
                if (u && u.item)
                    $("#hidFonte").val(u.item.id);
            },
            error: function (x, s, e) { mostrarErro(x.statusText); }
        });

        $("#txtFuncaoExercida1").autocomplete({
            source: function (request, response) {
                $.post(actions.autocompletefuncao, request, response);
            },
            minLength: 1,
            error: function (x, s, e) { mostrarErro(x.statusText); }
        });

        $("#txtFuncaoExercida2").autocomplete({
            source: function (request, response) {
                $.post(actions.autocompletefuncao, request, response);
            },
            minLength: 1,
            error: function (x, s, e) { mostrarErro(x.statusText); }
        });

        $("#txtFuncaoExercida3").autocomplete({
            source: function (request, response) {
                $.post(actions.autocompletefuncao, request, response);
            },
            minLength: 1,
            error: function (x, s, e) { mostrarErro(x.statusText); }
        });

        $("#txtCargo").autocomplete({
            source: function (request, response) {
                $.post(actions.autocompletefuncao, request, response);
            },
            minLength: 1,
            change: function (e, u) {
                if (u && u.item)
                    $("#hidCargo").val(u.item.id);
            },
            error: function (x, s, e) { mostrarErro(x.statusText); }
        });

        $(window).on("unload", function () {
            cacheToken();
            deslogar();
        });

        $(document.body).on('mouseenter', '.destaque-hover, .destaque-small-hover, .normal-hover', function (e) {
            $(this).css('background-color', 'rgba(68, 68, 68, 0.592157)');
        });
        $(document.body).on('mouseleave', '.destaque-hover, .destaque-small-hover, .normal-hover', function (e) {
            $(this).css('background-color', 'transparent');
        });

        modal.abrirModal('youtube');

    })();

    loadMoreEmpresas(0);
});

function OnBeforeSend() {
    //$('#loading').show();
    $('#loading').css('display', 'block');
}

function OnComplete() {
    //$('#loading').hide();
    $('#loading').css('display', 'none');
}

function keypressSearchFormHandler(e) {
    if (e.which == 13) {
        e.preventDefault();
        $('#Submit').click();
    }
}

$('#searchForm').keypress(keypressSearchFormHandler);

function cacheToken(token) {
    if (token)
        $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val(token);

    $.ajaxSetup({
        data: {
            __RequestVerificationToken: $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val()
        },
        dataType: "json",
        cache: false
    });
}

function deslogar() {
    var r = false;

    $.ajax({
        type: "post",
        cache: false,
        url: actions.signout,
        success: function (d, s, x) {
            if (d) {
                r = true;

                if (d.t) {
                    cacheToken(d.t);
                    limparCampos();
                    clean();
                    loadMoreEmpresas(0);
                }
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    return r;
}


function logado() {
    var r = false;

    $.ajax({
        type: "post",
        cache: false,
        url: actions.logado,
        success: function (d, s, x) {
            r = d;
        },
        error: function (x, s, e) { mostrarErro(x.statusText); r = null; },
        async: false
    });

    return r;
}

function loadMoreEmpresas(itemIndex) {
    OnBeforeSend();

    $.ajax({
        type: "POST",
        url: actions.buscarfilial,
        data: {
            "t": $("#txtTermoBusca").val(),
            "i": itemIndex,
            "c": 35
        },
        success:
            function (d, s, x) {
                var i = 0;
                $.each(d, function (i, e) {
                    var check;
                    var js;

                    if (e.candidatado) {
                        check = "<img class='ja' src='/Content/img/checked.png' />";
                        js = "modalCandidatou(this)";
                    } else {
                        check = "";
                        if (e.p) {
                            js = "if (marcarImagem(this)) terceiraTela()";
                        } else {
                            js = "if (marcarImagem(this)) segundaTela()";
                        }
                    }

                    var ul;
                    var id;
                    var ulcssclass;
                    var containercssclass = e.destaqueCss;

                    if (e.candidatado) {
                        containercssclass += ' candidatou';
                    }

                    if (e.destaqueCss === 'destaque') { //Identifica em qual lista deve ser adicionado
                        id = 'ulDestaque';
                        ulcssclass = '';
                    } else {
                        id = 'ulDestaqueSmall';
                        ulcssclass = 'ul-destaque-small';
                    }

                    var empresa = "<span class='" + containercssclass + "' onclick='" + js + "' cnpj='" + e.cnpj + "' filial='" + e.filial + "'>" + check + "<span class='" + e.destaqueCss + "-hover'></span><span class='imagem'><img class='logoFilial' src=" + actions.logofilial + e.cnpj + " alt='" + e.nomeFantasia + "' title='" + e.nomeFantasia + "' /></span><p class='nome' title='" + e.nomeFantasia + "'>" + e.nomeFantasia + "</p><p class='resenha'>" + e.apresentacao + "</p>";

                    if (e.o !== '') {
                        empresa += "<p class='representante'>* Esta empresa contrata através da " + e.o + "</p>";
                    }

                    empresa += "</span>";

                    ul = $('#' + id);
                    if (ul.length === 0) {
                        ul = $('<ul/>', { id: id }).addClass(ulcssclass);
                        $(ul).appendTo("#divEmpresas");
                    }
                    $('<li>' + empresa + '</li>').appendTo(ul);

                });

                if ($("#txtTermoBusca").val() !== '' && itemIndex === 0) {
                    var voltar = "<span class='voltar-busca' onclick='primeiraTela();'><a class='botao-voltar-busca'>VOLTAR</a></span>";
                    var ul = $('<ul/>');
                    $(ul).appendTo("#divEmpresas");
                    $('<li>' + voltar + '</li>').appendTo(ul);
                }
                itemCount += 20;
            },
        async: false
    })
    .fail(function (x, s, e) { mostrarErro(x.statusText); });

    OnComplete();
}

function habilitarMenu() {
    var menuHabilitado = logado();

    if (menuHabilitado) {
        if (!$("#imgPessoa")[0]) {

            //var n = nome();
            var n = curriculo();
            var div = $("#divFotoLogado").hide();

            $("<span class='destaque-logado'>" + n + "</span>").appendTo(div);

            /************ Não apagar /************/
            //var url = actions.fotopessoa + '/' + cpf();
            //$("<div id='imgPessoa' class='imagem-logado' style='background-image: url(" + url + ")'></div>").appendTo(div);
            /************ Não apagar /************/
            $("<div id='imgPessoa' class='imagem-logado' style='display: none'></div>").appendTo(div);

            $("#divMenuLogado").show();
            div.show();
        }
    } else {
        $("#divFotoLogado").hide("fast").html("");
        $("#divMenuLogado").hide("fast");
    }

    if (menuHabilitado !== null)
        setTimeout(habilitarMenu, 5000);
}

function mostrarErro(msg) {
    console.log('mostrarErro: ' + msg);

    $(".erro").remove();

    var erro = "<div class='erro'><a class='fechar-erro' title='Fechar' onclick='fecharErro();'>X</a> " + msg + "</div>";

    var element = modal.ultimaModalAberta();

    if (element !== '') {
        $('#' + element).find(".topo").after(erro);

        $('.erro').fadeIn();
    }
}

function fecharErro() {
    $('.erro').fadeOut();
    $(".erro").remove();
}

function clean() {
    console.log('clean');
    itemCount = 0;
    $("#divEmpresas").html('');
}

function limparCampos() {
    $("#txtNomeCompleto").val('');
    $("#txtDataNasc").val('');
    $('#rbSexoMasc').prop('checked', false);
    $('#rbSexoFem').prop('checked', false);
    $("#txtDDD").val('');
    $("#txtNumCelular").val('');
    $("#txtCpf").val('');
    $("#txtEmail").val('');
    $("#txtCargo").val('');
    $("#txtSalario").val('');
    $("#hidEscolaridade").val('');
    $("#txtEscolaridadeInstituicaoEnsino").val('');
    $("#hidFonte").val(-1);
    $("#txtEscolaridadeNomeCurso").val('');
    $("#txtEscolaridadeCidade").val('');
    $("#hidEscolaridadeCidade").val('');
    $("#txtEscolaridadePeriodo").val('');
    $("#selEscolaridadeSituacao").val(-2);
    $("#txtAnoConclusaoEscolaridade").val('');
    $("#txtNomeEmpresa1").val('');
    $("#selAreaEmpresa1").val('');
    $("#txtDataAdmissao1").val('');
    $("#txtDataDemissao1").val('');
    $("#txtFuncaoExercida1").val('');
    $("#txtAtribuicoes1").val('');
    $("#txtUltimoSalario").val('');
    $("#txtNomeEmpresa2").val('');
    $("#selAreaEmpresa2").val('');
    $("#txtDataAdmissao2").val('');
    $("#txtDataDemissao2").val('');
    $("#txtFuncaoExercida2").val('');
    $("#txtAtribuicoes2").val('');
    $("#txtNomeEmpresa3").val('');
    $("#selAreaEmpresa3").val('');
    $("#txtDataAdmissao3").val('');
    $("#txtDataDemissao3").val('');
    $("#txtFuncaoExercida3").val('');
    $("#txtAtribuicoes3").val('');
    $("#selEstadoCivil").val('');
    $("#txtCep").val('');
    $("#txtDDDRecado").val('');
    $("#txtTelefoneRecado").val('');
    $("#txtFalarCom").val('');
    $('#validacao-celular').hide();
    $("#txtCodigoValidacaoCelular").val('');
    fecharErro();
}

function modalCandidatou(span) {
    spanSelecionado = $(span);
    $(".logo-empresa").html("").append(spanSelecionado.find(".imagem > img:first").clone());
    modal.abrirModal('divJaCandidatou');
}

/************ Não apagar /************/
//function cpf() {
//    var r;

//    $.ajax({
//        type: "post",
//        cache: false,
//        url: actions.dadospessoa,
//        data: {
//            dado: "cpf"
//        },
//        success: function (d, s, x) {
//            if (d && d.cpf) {
//                r = d.cpf;
//            }
//        },
//        error: function (x, s, e) { mostrarErro(x.statusText); },
//        async: false
//    });

//    return r;
//}

//function nome() {
//    var r;

//    $.ajax({
//        type: "post",
//        cache: false,
//        url: actions.dadospessoa,
//        data: {
//            dado: "nome"
//        },
//        success: function (d, s, x) {
//            if (d && d.nome) {
//                r = d.nome;
//            }
//        },
//        error: function (x, s, e) { mostrarErro(x.statusText); },
//        async: false
//    });

//    return r;
//}

/************ Não apagar /************/


function curriculo() {
    var r;

    $.ajax({
        type: "post",
        cache: false,
        url: actions.curriculo,
        success: function (d, s, x) {
            if (d) {
                $('.funcao-candidato').html(d.f);
                $('.idade-candidato').html(d.i);
                $(".nome-candidato").html(d.nc);
                $(".data").html(d.dc);
                $('#cadastre-curriculo').attr('href', d.link);
                r = d.n;
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    return r;
}

function qc() {
    var qtde = 0;

    $.ajax({
        type: "post",
        cache: false,
        url: actions.candidaturas,
        success: function (d, s, x) {
            qtde = +d;
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    return qtde;
}

function vip() {
    var usuarioVip = false;

    $.ajax({
        type: "post",
        cache: false,
        url: actions.vip,
        success: function (d, s, x) {
            usuarioVip = d;
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    return usuarioVip;
}

function mostrarCartaApresentacao() {
    var r = true;

    $.ajax({
        type: "post",
        url: actions.carta,
        data: {
            c: cnpj
        },
        dataType: "json",
        success: function (d, s, x) {
            $("#cartaApresentacao").html(d.c);
            $("#nomeEmpresa").html(d.n);
            $(".nome-empresa").html(d.n);
        },
        error: function (x, s, e) {
            mostrarErro(x.statusText);
            r = false;
        },
        async: false
    });

    return r;
}
function seInteressar() {
    var r = false;

    $.ajax({
        type: "post",
        url: actions.interesse,
        data: {
            c: cnpj,
            f: filial
        },
        dataType: "json",
        success: function (d, s, x) {
            r = d;
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    return r;
}

function primeiraTela() {
    modal.fecharModais();
    $("#txtTermoBusca").val('');
    clean();
    loadMoreEmpresas(0);
}

function segundaTela() {
    if (qc() > 3) {
        vipTela();
        return;
    }

    if (mostrarCartaApresentacao()) {
        modal.abrirModal('div2');
    }
}

function terceiraTela() {
    modal.fecharModal('div2');

    var cand = qc();

    if (cand != 0) {

        if (cand === 1) {
            quintaTela();
        } else if (cand === 2) {
            sextaTela();
        }

        if (cand >= 3) {
            if (vip()) {
                quartaTela();
                return;
            } else {
                vipTela();
                return;
            }

        }

        return;
    }

    modal.abrirModal('div3');
}

function quartaTela() {

    modal.fecharModais();

    $.ajax({
        type: "post",
        url: actions.quartatela,
        dataType: "json",
        success: function (d, s, x) {
            if (d.t) {
                $("#codigo").html("").append(d.c);
                modal.abrirModal('div4');
                ticaSpanSelecionado();
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });
}

function quintaTela() {
    modal.abrirModal('div5');
    $("#accordionProfissional").accordion("refresh");
}

function sextaTela() {
    modal.abrirModal('div6');
}

function vipTela() {
    modal.abrirModal('divVip');
}

function SelecionarUsuario(cpf, data, nome) {
    $("#txtCpf").val(cpf);
    $("#txtDataNasc").val(data);
    $("#txtNomeCompleto").val(nome);
    terceiraTela();
}

function vipOk() {
    var r = false;

    $.ajax({
        type: "post",
        url: actions.vipOk,
        data: {
            c: $("#txtCodigoDesconto").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d.error) {
                mostrarErro(d.error);
                r = false;
            }
            else {
                r = true;
            }
        },
        error: function (x, s, e) {
            mostrarErro(x.statusText);
        },
        async: false
    });

    return r;
}

function ticaSpanSelecionado() {
    if (spanSelecionado[0]) {
        spanSelecionado.attr("onclick", "").bind("click", function () {
            modalCandidatou(this);
        });
        spanSelecionado.append("<img class='ja' src='/Content/img/checked.png' />");
        spanSelecionado.addClass("candidatou");
    }
}

function marcarImagem(span) {
    var c = $(span).attr("cnpj");
    var f = $(span).attr("filial");
    spanSelecionado = $(span);
    cnpj = c;
    filial = f;

    $("#nomeempresa").html("").append(spanSelecionado.find(".nome").clone());
    $(".nome-empresa").html("").append(spanSelecionado.find(".nome").clone());
    $(".logo-empresa").html("").append(spanSelecionado.find(".imagem > img:first").clone());

    if (seInteressar()) {
        return true;
    } else {
        cnpj = null;
        filial = null;

        return false;
    }
}

function formatarNomeCompleto(i) {
    var nomeCompleto = $(i || "#txtNomeCompleto").val();

    if (nomeCompleto === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (/\d+/i.test(nomeCompleto) || !/\s/.test(nomeCompleto)) {
        mostrarErro("<b>Digite um nome completo válido</b>, com nome e sobrenome, por favor");

        if (typeof (i) === 'undefined') {
            $(i || "#txtNomeCompleto").focus();
        }

        return false;
    }

    return true;
}

function formatarSexo() {
    if (!$('#rbSexoMasc').is(":checked") && !$('#rbSexoFem').is(":checked")) {
        mostrarErro("<b>Escolha um sexo</b>, masculino ou feminino, por favor");
        return false;
    }

    return true;
}

function formatarDDD(i) {
    var ddd = +$(i || "#txtDDD").val();

    if (ddd === 0 && typeof (i) !== 'undefined') {
        return false;
    }

    if (!ddd || ddd < 11 || ddd > 99) {
        mostrarErro("<b>DDD inválido</b>, favor corrigir");

        if (typeof (i) === 'undefined') {
            $(i || "#txtDDD").focus();
        }

        return false;
    }

    return true;
}

function formatarNumCelular(i) {
    var numCelular = $(i || "#txtNumCelular").val();

    if (numCelular === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (!/\d{4,5}-\d{4}$/.test(numCelular) && !/\d{8,9}$/.test(numCelular)) {
        mostrarErro("<p><b>Número de celular inválido.</b></p>");

        if (typeof (i) === 'undefined') {
            $(i || "#txtNumCelular").focus();
        }

        return false;
    }

    validaCelular();

    if (!logado() && formatarDDD()) {
        if (!seInteressar()) {
            primeiraTela();
        }
    }

    return true;
}

function validaCelular() {
    var r = false;

    $.ajax({
        type: "post",
        url: actions.validaCelular,
        data: {
            ddd: $("#txtDDD").val(),
            numero: $("#txtNumCelular").val(),
            enviarNovamente: false
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d.message) {
                $('#validacao-celular').show();
                mostrarErro(d.message);

                r = true;
            }
        },
        error: function (x, s, e) {
            mostrarErro(x.statusText);
        },
        async: false
    });

    return r;
}

function validaCelularEnviarOutroCodigo() {
    var r = false;

    $.ajax({
        type: "post",
        url: actions.validaCelular,
        data: {
            ddd: $("#txtDDD").val(),
            numero: $("#txtNumCelular").val(),
            enviarNovamente: true
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d.message) {
                mostrarErro(d.message);

                r = true;
            }
        },
        error: function (x, s, e) {
            mostrarErro(x.statusText);
        },
        async: false
    });

    return r;
}

function validarSegundaTela() {
    var valido = formatarNomeCompleto() && formatarDDD() && formatarNumCelular() && formatarDataNasc() && formatarSexo();

    valido = valido && salvarSegundaTela();

    return valido;
}

function salvarSegundaTela() {
    var r = false;

    OnBeforeSend();

    $.ajax({
        type: "post",
        url: actions.segundatela,
        data: {
            n: $("#txtNomeCompleto").val(),
            dn: $("#txtDataNasc").val(),
            s: $("#rbSexoMasc:checked") ? 1 : 2,
            d: $("#txtDDD").val(),
            m: $("#txtNumCelular").val(),
            cv: $("#txtCodigoValidacaoCelular").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d) {
                r = true;
            }
        },
        error: function (x, s, e) {
            mostrarErro('Ocorreu um erro em seu acesso.');
        },
        async: false
    });

    OnComplete();

    return r;
}

function validarTerceiraTela() {
    var valido = formatarCpf() && formatarEmail() && formatarCargo() && formatarSalario();

    valido = valido && salvarTerceiraTela();

    return valido && logado();
}

function salvarTerceiraTela() {
    var r = false;

    OnBeforeSend();

    $.ajax({
        type: "post",
        url: actions.terceiratela,
        data: {
            c: $("#txtCpf").val(),
            e: $("#txtEmail").val(),
            g: $("#txtCargo").val(),
            s: $("#txtSalario").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d.t) {
                r = true;
                cacheToken(d.t);
                clean();
                loadMoreEmpresas(0);
                modal.fecharModal('div3');
            }
            else {
                if (d.error) {
                    mostrarErro(d.error);
                } else {
                    mostrarErro('Erro no login os dados não conferem!');
                }
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    OnComplete();

    return r;
}

function formatarCpf(i) {
    var cpf = $(i || "#txtCpf").val();

    if (cpf === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (!/^\d{3}\.\d{3}\.\d{3}-\d{2}$/.test(cpf) && !/^\d{11}$/.test(cpf)) {
        mostrarErro("<b>Número de CPF inválido</b>, favor corrigir");

        if (typeof (i) === 'undefined') {
            $(i || "#txtCpf").focus();
        }

        return false;
    }

    return true;
}

function formatarDataNasc(i) {
    var dataNasc = $(i || "#txtDataNasc").val();

    if (dataNasc === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (!/^\d{2}\/\d{2}\/\d{4}$/.test(dataNasc) && !/^\d{8}$/.test(dataNasc) && !/^\d{6}$/.test(dataNasc)) {
        mostrarErro("<b>Data de nascimento inválida</b>, favor corrigir");

        if (typeof (i) === 'undefined') {
            $(i || "#txtDataNasc").focus();
        }

        return false;
    }

    if (!logado()) {
        if (!seInteressar()) {
            primeiraTela();
        }
    }

    return true;
}

function logarCpf() {
    var r = false;

    $.ajax({
        type: "post",
        url: actions.signinc,
        data: {
            c: $("#txtCpf").val(),
            d: $("#txtDataNasc").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d) {
                r = true;
                if (d.t) {
                    cacheToken(d.t);
                    clean();
                    loadMoreEmpresas(0);
                }
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    return r;
}

function formatarEmail(i) {
    var email = $(i || "#txtEmail").val();

    if (typeof (i) !== 'undefined') {
        return false;
    }

    if (email === '') {
        return true;
    }


    if (!email || !/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/.test(email)) {
        mostrarErro("<b>E-mail inválido</b>, favor corrigir");

        if (typeof (i) === 'undefined') {
            $(i || "#txtEmail").focus();
        }

        return false;
    }

    return true;
}

function formatarCargo(i) {
    var cargo = $(i || "#txtCargo").val();

    if (cargo === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (!/^\D+$/.test(cargo)) {
        mostrarErro("<b>Cargo inválido</b>, favor corrigir");

        if (typeof (i) === 'undefined') {
            $(i || "#txtCargo").focus();
        }

        return false;
    }

    return true;
}

function formatarSalario(i) {
    var salario = $(i || "#txtSalario").val();

    if (salario === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (!/(\$?(:?\d+,?.?)+)/.test(salario)) { //&& !/^(\d\.)?\d{3},\d{2}$/.test(salario)
        mostrarErro("<b>Salário inválido</b>, favor corrigir");

        if (typeof (i) === 'undefined') {
            $(i || "#txtSalario").focus();
        }

        return false;
    }

    return true;
}

function formatarEscolaridade() {
    var escolaridade = +$("#selEscolaridade").val();
    var completa = $("#selEscolaridadeCompleta").val() == "Completo";
    var preencherHidden = function () {
        var e = $("#hidEscolaridade");
        return e.val.apply(e, arguments);
    };
    var incompletude = completa ? (escolaridade == 10 || escolaridade == 11 ? 2 : 1) : 0;
    var detalhe;

    switch (escolaridade) {
        case 6:
            detalhe = completa ? "#divEscolaridadeCompletaAnoConclusao" : "#divEscolaridadeIncompletaSituacao";
            break;
        case 8:
        case 10:
        case 11:
            detalhe = "#divEscolaridadeInstituicaoEnsino";
            detalhe += completa ? ",#divEscolaridadeCompletaAnoConclusao" : ",#divEscolaridadeIncompletaSituacao";

            if (!completa) {
                detalhe += ",#divEscolaridadePeriodo";
            }

            break;
        default:
            detalhe = null;
            break;
    }

    preencherHidden(escolaridade + incompletude);

    $('#divEscolaridadeCompletaAnoConclusao').fadeOut('fast');
    $('#divEscolaridadeIncompletaSituacao').fadeOut('fast');
    $('#divEscolaridadeInstituicaoEnsino').fadeOut('fast');
    $('#divEscolaridadePeriodo').fadeOut('fast');

    if (detalhe) {
        $(detalhe).fadeIn("fast");
        $("#txtEscolaridadeInstituicaoEnsino:visible").focus();
    }

    if (escolaridade <= 0) {
        mostrarErro("<b>Selecione uma escolaridade</b>, por favor");
        return false;
    }

    return true;
}

function formatarInstituicaoEnsino(i) {
    var instituicaoEnsino = $(i || "#txtEscolaridadeInstituicaoEnsino").val();

    if (!instituicaoEnsino) {
        mostrarErro("<b>Digite uma instituição de ensino válida</b>, por favor");
        $(i || "#txtEscolaridadeInstituicaoEnsino").focus();
        return false;
    }

    return true;
}

function formatarNomeCurso(i) {
    var nomeCurso = $(i || "#txtEscolaridadeNomeCurso").val();

    if (!nomeCurso) {
        mostrarErro("<b>Digite um nome de curso válido</b>, por favor");
        $(i || "#txtEscolaridadeNomeCurso").focus();
        return false;
    }

    return true;
}

function formatarEscolaridadeCidade(i) {
    var cidade = $(i || "#txtEscolaridadeCidade").val();
    var id = +$("#hidEscolaridadeCidade").val();

    if (!id && !cidade) {
        mostrarErro("<b>Selecione ou digite uma cidade válida</b>, por favor");
        $(i || "#txtEscolaridadeCidade").focus();
        return false;
    }

    return true;
}

function formatarEscolaridadeSituacao(i) {
    var situacao = +$(i || "#selEscolaridadeSituacao").val();

    if (!situacao || situacao < 0) {
        mostrarErro("<b>Selecione uma situação de escolaridade correta</b>, por favor");
        $(i || "#selEscolaridadeSituacao").focus();
        return false;
    }

    return true;
}

function formatarEscolaridadePeriodo(i) {
    return true;
}

function formatarEscolaridadeAnoConclusao(i) {
    var ano = +$(i || "#txtAnoConclusaoEscolaridade").val();

    if (!ano || ano < 1900 || ano > new Date().getFullYear()) {
        mostrarErro("<b>Digite um ano de conclusão válido</b>, por favor");
        $(i || "#txtAnoConclusaoEscolaridade").focus();
        return false;
    }

    return true;
}

function validarQuintaTela() {
    var r = formatarEscolaridade();

    if (r) {
        var escolaridade = +$("#hidEscolaridade").val();
        switch (escolaridade) {
            case 4:
            case 5:
                break;
            case 6:
                r = formatarEscolaridadeSituacao();
                break;
            case 7:
                r = formatarEscolaridadeAnoConclusao();
                break;
            case 8:
            case 10:
            case 11:
                r = formatarInstituicaoEnsino() && formatarNomeCurso() && formatarEscolaridadeCidade() && formatarEscolaridadePeriodo();
                r = r && formatarEscolaridadeSituacao();
                break;
            case 9:
            case 12:
            case 13:
                r = formatarInstituicaoEnsino() && formatarNomeCurso() && formatarEscolaridadeCidade() && formatarEscolaridadePeriodo();
                r = r && formatarEscolaridadeAnoConclusao();
                break;
            default:
                r = false;
                mostrarErro("<b>A escolaridade está incorreta</b>, favor corrigir");
                $("#selEscolaridade").focus();
                break;
        }
    }

    r = r && salvarQuintaTela();

    return r;
}

function salvarQuintaTela() {
    var r = false;

    OnBeforeSend();

    $.ajax({
        type: "post",
        url: actions.quintatela,
        data: {
            es: $("#hidEscolaridade").val(),
            i: $("#txtEscolaridadeInstituicaoEnsino").val(),
            f: $("#hidFonte").val(),
            nc: $("#txtEscolaridadeNomeCurso").val(),
            c: $("#txtEscolaridadeCidade").val(),
            ic: $("#hidEscolaridadeCidade").val(),
            p: $("#txtEscolaridadePeriodo").val(),
            s: $("#selEscolaridadeSituacao").val(),
            a: $("#txtAnoConclusaoEscolaridade").val(),
            ne1: $("#txtNomeEmpresa1").val(),
            ar1: $("#selAreaEmpresa1").val(),
            da1: $("#txtDataAdmissao1").val(),
            dd1: $("#txtDataDemissao1").val(),
            f1: $("#txtFuncaoExercida1").val(),
            at1: $("#txtAtribuicoes1").val(),
            us: $("#txtUltimoSalario").val(),
            ne2: $("#txtNomeEmpresa2").val(),
            ar2: $("#selAreaEmpresa2").val(),
            da2: $("#txtDataAdmissao2").val(),
            dd2: $("#txtDataDemissao2").val(),
            f2: $("#txtFuncaoExercida2").val(),
            at2: $("#txtAtribuicoes2").val(),
            ne3: $("#txtNomeEmpresa3").val(),
            ar3: $("#selAreaEmpresa3").val(),
            da3: $("#txtDataAdmissao3").val(),
            dd3: $("#txtDataDemissao3").val(),
            f3: $("#txtFuncaoExercida3").val(),
            at3: $("#txtAtribuicoes3").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            r = d;
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    OnComplete();

    return r;
}

function validarSextaTela() {
    var valido = formatarEstadoCivil() && formatarCep();

    valido = valido && salvarSextaTela();

    return valido;
}

function salvarSextaTela() {
    var r = false;

    OnBeforeSend();

    $.ajax({
        type: "post",
        url: actions.sextatela,
        data: {
            e: $("#selEstadoCivil").val(),
            c: $("#txtCep").val(),
            d: $("#txtDDDRecado").val(),
            t: $("#txtTelefoneRecado").val(),
            fc: $("#txtFalarCom").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            r = d;
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });

    OnComplete();

    return r;
}

function formatarEstadoCivil(i) {
    var estadoCivil = +$(i || "#selEstadoCivil").val();

    if (estadoCivil <= 0) {
        mostrarErro("Selecione um estado civil, por favor");
        return false;
    }

    return true;
}

function formatarCep(i) {
    var cep = $(i || "#txtCep").val();

    if (cep === '' && typeof (i) !== 'undefined') {
        return false;
    }

    if (!/^\d{5}-\d{3}$/.test(cep) && !/^\d{8}$/.test(cep)) {

        if (typeof (i) === 'undefined') {
            mostrarErro("CEP inválido, favor corrigir");
        }

        return false;
    }

    return true;
}

function PreencherCamposFacebook(response) {
    $("#txtNomeCompleto").val(response.name);
    $("#txtDataNasc").val($.datepicker.formatDate('dd/mm/yy', new Date(response.birthday)));
    if (response.gender === 'masculino') {
        $('#rbSexoMasc').attr('checked', 'checked');
    } else if (response.gender === 'feminino') {
        $('#rbSexoFem').attr('checked', 'checked');
    }
    $("#txtEmail").val(response.email);

    $.ajax({
        type: "post",
        url: actions.dadosfacebook,
        dataType: "json",
        success: function (d, s, x) {
            if (d) {
                $("#selEstadoCivil").val(d.ec);
                $("#txtCargo").val(d.cp);
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusText); },
        async: false
    });
}

function comprovante() {
    modal.fecharModal('div4');

    modal.abrirModal('divComprovante');

    setTimeout(function () { window.print(); }, 1000);
}