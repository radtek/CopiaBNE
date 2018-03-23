//var URLCompleteCidade = '';
//var LimiteCompleteCidade = '';
//var URLCompleteFuncao = '';
//var LimiteCompleteFuncao = '';
//var URLCompleteBairro = '';
//var LimiteCompleteBairro = '';

//function InicializarAutoCompletes(parametros) {
//    if (typeof (parametros) != 'undefined' && parametros != 'undefined') {
//        autocomplete.URL.URLCompleteCidade = parametros.URLCompleteCidade;
//        autocomplete.limit.LimiteCompleteCidade = parametros.LimiteCompleteCidade;
//        autocomplete.URL.URLCompleteFuncao = parametros.URLCompleteFuncao;
//        autocomplete.limit.LimiteCompleteFuncao = parametros.LimiteCompleteFuncao;
//        autocomplete.URL.URLCompleteBairro = parametros.URLCompleteBairro;
//        autocomplete.limit.LimiteCompleteBairro = parametros.LimiteCompleteBairro;
//    }
//}

var VariveisPesquisa = {
    tipoBusca: '',
    expandido: '',
    mostrarPesquisaVaga: '',
    mostrarPesquisaCurriculo: ''
};

function InicializarAbasBusca(parametros) {
    if (typeof (parametros) != 'undefined' && parametros != 'undefined') {
        VariveisPesquisa.tipoBusca = parametros.tipoBusca;
        VariveisPesquisa.expandido = parametros.expandido.toLowerCase();
    }

    if (VariveisPesquisa.tipoBusca == 'V') {
        VariveisPesquisa.mostrarPesquisaVaga = VariveisPesquisa.expandido == 'true' ? true : false;
        VariveisPesquisa.mostrarPesquisaCurriculo = false;
    }
    else if (VariveisPesquisa.tipoBusca == 'C') {
        VariveisPesquisa.mostrarPesquisaCurriculo = VariveisPesquisa.expandido == 'true' ? true : false;
        VariveisPesquisa.mostrarPesquisaVaga = false;
    }

    AjustarAbasBusca();
}

function AjustarParametrosAbasBusca(strTipoBusca) {
    if (strTipoBusca === 'V') {
        if (VariveisPesquisa.tipoBusca === strTipoBusca) {
            VariveisPesquisa.mostrarPesquisaVaga = !VariveisPesquisa.mostrarPesquisaVaga;
            $("#buscaavancada").hide();
            $("#buscaavancadacv").show();
            $("#descPalavraChaveTopo").show();
        }
        else {
            VariveisPesquisa.mostrarPesquisaVaga = true;
            VariveisPesquisa.mostrarPesquisaCurriculo = false;
            $("#buscaavancada").show();
            $("#buscaavancadacv").hide();
            $("#descPalavraChaveTopo").hide();

        }
    }
    else if (strTipoBusca === 'C') {
        if (VariveisPesquisa.tipoBusca == strTipoBusca) {
            VariveisPesquisa.mostrarPesquisaCurriculo = !VariveisPesquisa.mostrarPesquisaCurriculo;

            $("#buscaavancada").show();
            $("#buscaavancadacv").hide();
            $("#descPalavraChaveTopo").hide();
        }
        else {
            VariveisPesquisa.mostrarPesquisaCurriculo = true;
            VariveisPesquisa.mostrarPesquisaVaga = false;
            $("#buscaavancada").hide();
            $("#buscaavancadacv").show();
            $("#descPalavraChaveTopo").show();
        }
    }
    VariveisPesquisa.tipoBusca = strTipoBusca;
    AjustarAbasBusca();
}

function AbrirPopup(url) {
    open(url);
}

//Ajusta a busca do topo
function AjustarAbasBusca() {


    var navBreadcrumbForms_Height = 0;
    var pnlMenuSecaoCandidato_Height = 0;
    var pnlMenuSecaoEmpresa_Height = 0;

    if ($("#navBreadcrumbForms").length)
    {
        if ($('#navBreadcrumbForms').css('display') != 'none')
            navBreadcrumbForms_Height = $('#navBreadcrumbForms').height();
    }

    if ($("#pnlMenuSecaoCandidato").length)
    {
        if ($('#pnlMenuSecaoCandidato').css('display') != 'none')
            pnlMenuSecaoCandidato_Height = $('#pnlMenuSecaoCandidato').height();
    }

    if ($("#pnlMenuSecaoEmpresa").length)
    {
        if ($('#pnlMenuSecaoEmpresa').css('display') != 'none')
            pnlMenuSecaoEmpresa_Height = $('#pnlMenuSecaoEmpresa').height();
    }


    
    var pnlBusca = employer.util.findControl('pnlBusca');
    var txtFuncaoMaster = employer.util.findControl('txtFuncaoMaster');
    var txtFuncaoPre = employer.util.findControl('txtFuncaoPre');
    var txtCidadeMaster = employer.util.findControl('txtCidadeMaster');
    var txtCidadePre = employer.util.findControl('txtCidadePre');
    var txtPalavraChaveMaster = employer.util.findControl('txtPalavraChaveMaster');
    var bsmPalavraChaveMasterLinkSaibaMais = employer.util.findControl('bsmPalavraChaveMasterLinkSaibaMais');
    var btnBuscaCurriculo = employer.util.findControl('btiPesquisarCurriculo');
    var btnBuscaVaga = employer.util.findControl('btiPesquisarVaga');

    $('#changeUser').addClass('empresa');
    $('#changeUser').removeClass('candidato');

    $(".descricaoTopo").show();

    if (VariveisPesquisa.tipoBusca === 'V') { //Recuperando a ultima pesquisa utilizada
        $('#changeUser').removeClass('empresa');
        $('#changeUser').addClass('candidato');
    }
    else if (VariveisPesquisa.tipoBusca === 'C') {
        $('#changeUser').removeClass('candidato');
        $('#changeUser').addClass('empresa');
    }

    //Opção de esconder Pesquisa Curriculo desabilitado
    if (VariveisPesquisa.mostrarPesquisaVaga) {
        //Removendo classes pertencentes a busca antiga
        pnlBusca.removeClass("painel_filtro_curriculo_escondido");
        pnlBusca.removeClass("painel_filtro_curriculo");

        $("#buscaavancada").show();
        $("#buscaavancadacv").hide();
        $("#descPalavraChaveTopo").hide();

        //Ajustando o painel e os botoes
        txtFuncaoMaster.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        txtFuncaoMaster.attr({ onkeypress: "BuscarTeclaEnter(event)" });
        txtFuncaoPre.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        txtFuncaoPre.attr({ onkeypress: "BuscarTeclaEnter(event)" });
        txtCidadeMaster.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        txtCidadeMaster.attr({ onkeypress: "BuscarTeclaEnter(event)" });
        txtCidadePre.attr({ onkeyDown: "BuscarTeclaEnter(event)" });
        txtCidadePre.attr({ onKeyPress: "BuscarTeclaEnter(event)" });
        txtPalavraChaveMaster.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        btnBuscaCurriculo.css("display", "none");
        btnBuscaVaga.css("display", "");

        pnlBusca.removeClass("painel_filtro_vaga_escondido");

        pnlBusca.addClass("painel_filtro_vaga");

        //Limpa o valor do campo Palavra-chave
        $("*[id$='txtPalavraChaveMaster']").val("");

        //employer.controles.setAttr('imgBuscarVagas', 'src', 'img/aba_buscar_vagas_seta_cima.gif');
        //if ($("*[id$='imgBuscarVagas']").length > 0) {
        //$("*[id$='imgBuscarVagas']").attr("src", ($("*[id$='imgBuscarVagas']").attr("src")).replace("_baixa", "_cima"));
        //}

        //Ajustando espacamento do conteudo
        $('#conteudo').children('.interna').removeClass('internaSlim');
        $('.menu_secao').removeClass('menu_secaoSlim');
        $('.TituloTela').removeClass('TituloTelaSlim');
        $('.menu_breadcrumb').removeClass('menu_breadcrumbSlim');


        $('#upTopo').css('height', 200 + navBreadcrumbForms_Height + pnlMenuSecaoCandidato_Height + pnlMenuSecaoEmpresa_Height);
        if ($("#navBreadcrumbForms").length)
        {
            if ($('#navBreadcrumbForms').css('display') == 'none')
                $("#pnlMenuSecaoEmpresa").css('padding-top', '100px');
        }

        txtFuncaoMaster.css('display', 'block');
        txtFuncaoPre.css('display', 'block');
        txtCidadeMaster.css('display', 'block');
        txtCidadePre.css('display', 'block');
        txtPalavraChaveMaster.css('display', 'none');
        bsmPalavraChaveMasterLinkSaibaMais.css('display', 'none');


    } else if (VariveisPesquisa.mostrarPesquisaCurriculo) {
        $("#buscaavancada").hide();
        $("#buscaavancadacv").show();
        $("#descPalavraChaveTopo").show();
        //Removendo classes pertencentes a busca antiga
        pnlBusca.removeClass("painel_filtro_vaga");
        pnlBusca.removeClass("painel_filtro_vaga_escondido");

        //Ajustando o painel e os botoes
        txtFuncaoMaster.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        txtCidadeMaster.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        txtPalavraChaveMaster.attr({ onKeyDown: "BuscarTeclaEnter(event)" });
        btnBuscaCurriculo.css("display", "");
        btnBuscaVaga.css("display", "none");

        pnlBusca.removeClass("painel_filtro_curriculo_escondido");
        pnlBusca.addClass("painel_filtro_curriculo");

        //employer.controles.setAttr('imgBuscarCurriculos', 'src', 'img/aba_buscar_curriculos_seta_cima.png');
        //$("*[id$='imgBuscarCurriculos']").attr("src", ($("*[id$='imgBuscarCurriculos']").attr("src")).replace("_baixa", "_cima"));

        //Ajustando espacamento do conteudo
        $('#conteudo').children('.interna').removeClass('internaSlim');
        $('.menu_secao').removeClass('menu_secaoSlim');
        $('.TituloTela').removeClass('TituloTelaSlim');
        $('.menu_breadcrumb').removeClass('menu_breadcrumbSlim');

        $('#upTopo').css('height', 200 + navBreadcrumbForms_Height + pnlMenuSecaoCandidato_Height + pnlMenuSecaoEmpresa_Height);
        if ($("#navBreadcrumbForms").length)
        {
            if ($('#navBreadcrumbForms').css('display') == 'none')
                $("#pnlMenuSecaoEmpresa").css('padding-top', '100px');
        }

        txtFuncaoMaster.css('display', 'block');
        txtCidadeMaster.css('display', 'block');
        txtPalavraChaveMaster.css('display', 'block');
        bsmPalavraChaveMasterLinkSaibaMais.css('display', 'block');

    } else {
        //Esconder tudo
        pnlBusca.removeClass('painel_filtro_vaga');
        pnlBusca.removeClass('painel_filtro_curriculo');

        btnBuscaCurriculo.css('display', 'none');
        btnBuscaVaga.css('display', 'none');
        $("#buscaavancada").hide();
        $("#buscaavancadacv").hide();
        $(".descricaoTopo").hide();

        if (VariveisPesquisa.tipoBusca === 'V') { //Recuperando a ultima pesquisa utilizada
            pnlBusca.addClass("painel_filtro_vaga_escondido");
        }
        else if (VariveisPesquisa.tipoBusca === 'C') {
            pnlBusca.addClass("painel_filtro_curriculo_escondido");
        }

        txtFuncaoMaster.css('display', 'none');
        txtCidadeMaster.css('display', 'none');
        txtPalavraChaveMaster.css('display', 'none');
        bsmPalavraChaveMasterLinkSaibaMais.css('display', 'none');

        $('#conteudo').children('.interna').addClass('internaSlim');
        $('.menu_secao').addClass('menu_secaoSlim');
        $('.TituloTela').addClass('TituloTelaSlim');
        $('.menu_breadcrumb').addClass('menu_breadcrumbSlim');

        $('#upTopo').css('height', 150 + navBreadcrumbForms_Height + pnlMenuSecaoCandidato_Height + pnlMenuSecaoEmpresa_Height);
        if ($("#navBreadcrumbForms").length)
        {
            if ($('#navBreadcrumbForms').css('display') == 'none')
                $("#pnlMenuSecaoEmpresa").css('padding-top', '40px');
        }
    }
}

//Função que executa a busca ao teclar ENTER
function BuscarTeclaEnter(e) {
    var code = e.keyCode || e.charCode || e.which || null;

    if (
        //Se os suggest (autocomplete) está fechado
    ($("ul[id*='aceFuncaoMaster']").css("display") == "none" || $("ul[id*='aceFuncaoMaster']").css("display") == "" || $("ul[id*='aceFuncaoMaster']").css("display") == "undefined")
    && ($("ul[id*='aceCidade']").css("display") == "none" || $("ul[id*='aceCidade']").css("display") == "" || $("ul[id*='aceCidade']").css("display") == "undefined")
    &&
        //Se o botão pressionado é o ENTER
    code == 13
    ) {
        //Dispara o botão de busca de vagas se o painel de vagas estiver ativo
        if ($("*[id$='pnlBusca']").hasClass("painel_filtro_vaga")) {
            document.getElementById('btiPesquisarVaga').click();
        }
            //Dispara o botão de busca de currículos se o painel de currículos estiver ativo
        else if ($("*[id$='pnlBusca']").hasClass("painel_filtro_curriculo")) {
            document.getElementById('btiPesquisarCurriculo').click();
        }
        e.preventDefault();

    }
}

function cvCidadePrincipal_Validate(sender, args) {
    var res = Principal.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
    employer.util.findControl('divCidadeInexistente').css('display', args.IsValid ? 'none' : 'block');
}

function cvFuncaoPrincipal_Validate(sender, args) {
    var res = Principal.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
    employer.util.findControl('divFuncaoInexistente').css('display', args.IsValid ? 'none' : 'block');
}

function CidadeSelecionadaMasterPage(sender, args) {
}

function FuncaoOnChange() {
    var valor = employer.controles.recuperarValor('txtFuncaoMaster');
    if (valor == "")
        employer.util.findControl('divFuncaoInexistente').css('display', 'none');
    employer.util.findControl('divFuncaoInexistente').css('display', 'none');
}

function CidadeOnChange(sender) {
    var valor = employer.controles.recuperarValor('txtCidadeMaster');
    if (valor == "")
        employer.util.findControl('divCidadeInexistente').css('display', 'none');
    employer.util.findControl('divCidadeInexistente').css('display', 'none');
}

$(document).ready(function () {
    //Define o height do update progress
    $("div[id$='progress_img_container']").height($(document).height());

    //Ajustando o Carregando...
    var vTop = ($(window).height() / 2) - $("div[id='img_container']").height() / 2; // +$(top.window).scrollTop();
    //Ajustando a altura
    $("div[id='img_container']").css({ "margin-top": vTop + "px" });

    $(window).scroll(function () {
        //Ajusta o tamanho da div do Carregando
        $("div[id$='progress_img_container']").height($(document).height());
    });

    autocomplete.cidade("txtCidadeMaster");
    autocomplete.cidade("txtCidadePre");
    autocomplete.funcao("txtFuncaoMaster");
    autocomplete.funcao("txtFuncaoPre");
});

function bntFecharModal_ClientClick() {
    FecharModal();
}

function FecharModal() {
    $find("ctl00_mpe").hide();
    $("#modal").remove();
}

function ValidarNome(source, args) {
    var w, z, y, x;
    var isValid = true;
    for (x = 0; x < args.Value.length; x++) {
        z = args.Value.substring(x, x + 1);
        if ((x >= 2 && z == y && z == w)) {
            isValid = false;
        }
        else {
            y = w;
            w = z;
            z = '-';
        }
    }

    if (!args.Value.match("^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,})*$"))
        isValid = false;

    args.IsValid = isValid;
}

function ValidarFuncao(sender, args) {
    var res = Principal.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function ValidarCidade(sender, args) {
    var res = Principal.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

var BNEFacebook = {};

BNEFacebook.EfetuarLogin = function () {
    FB.login(
        BNEFacebook.LoginFacebookCallback,
        { scope: 'email,user_about_me,user_birthday,user_location,user_work_history,user_education_history,user_relationships,user_likes,read_friendlists' }
    );
};

BNEFacebook.LoginFacebookCallback = function (response) {
    if (response.authResponse) {
        var objectMe;
        var objectFriends;
        var objectPicture;

        //Recuperando os dados do usuário
        FB.api('/me?locale=pt_BR', function (responseMe) {
            objectMe = responseMe;

            //Recuperando a lista de amigos
            FB.api('/me/friends', function (responseFriends) {
                objectFriends = responseFriends;

                //Recuperando a foto
                FB.api('/me/picture?width=180&height=180', function (responsePicture) {
                    objectPicture = responsePicture;

                    //Depois de pegar todas as informações necessárias, valida se o usuário existe.
                    var userExist = Login.ValidarFacebook(objectMe.id);
                    if (userExist.value) {
                        var botao = employer.util.findControl('btnEntrarFacebook');
                        __doPostBack(botao[0].id, 'facebook');
                        //window.location.reload();
                    } else {
                        var jsonMe = JSON.stringify(objectMe);
                        var jsonFriends = JSON.stringify(objectFriends);
                        var jsonMePicture = JSON.stringify(objectPicture);
                        Login.ArmazenarDadosFacebook(jsonMe, jsonFriends, jsonMePicture);
                        window.location = "/CadastroCurriculoMini.aspx";
                    }
                });
            });
        });
    }
};

var windw = this;
$.fn.followTo = function (pos) {
    var $this = this,
        $window = $(windw);

    $window.scroll(function (e) {
        if ($window.scrollTop() > pos) {
            $("#prices .block-bts").css({ position: 'static' });
        } else {
            $("#prices .block-bts").css({
                position: 'fixed', bottom: '0px', left: '0px'
            });
        }
    });
};

$('#f').followTo(770);


function removerAcentos(newStringComAcento) {
    var string = newStringComAcento;
    var mapaAcentosHex = {
        a: /[\xE0-\xE6]/g,
        e: /[\xE8-\xEB]/g,
        i: /[\xEC-\xEF]/g,
        o: /[\xF2-\xF6]/g,
        u: /[\xF9-\xFC]/g,
        c: /\xE7/g,
        n: /\xF1/g
    };

    for (var letra in mapaAcentosHex) {
        var expressaoRegular = mapaAcentosHex[letra];
        string = string.replace(expressaoRegular, letra);
    }

    return string;
}

function atualizarUrlPesquisaCurriculo() {
    if (employer.util.findControl('txtFuncaoMaster').val().trim() == "" && employer.util.findControl('txtCidadeMaster').val().trim() == ""){
        window.location = window.location.protocol + "//" + window.location.host + "/lista-de-curriculos";
        return false;
    }

    var funcaoValida = false;
    var cidadeValida = false;
    var funcao;
    var cidade;
    var siglaEstado;

    if (employer.util.findControl('txtFuncaoMaster').val().trim() != "") {
        var res = Principal.ValidarFuncao(employer.util.findControl('txtFuncaoMaster').val());        
        if (res.error == null && res.value) {
            funcaoValida = true;
            funcao = encodeURIComponent(removerAcentos(employer.util.findControl('txtFuncaoMaster').val().trim()).toLowerCase().replace(/ /g, "-").replace('#', '(csharp)'));
        }
    }
    if (employer.util.findControl('txtFuncaoPre').val().trim() != "") {
        var res = Principal.ValidarFuncao(employer.util.findControl('txtFuncaoPre').val());
        if (res.error == null && res.value) {
            funcaoValida = true;
            funcao = encodeURIComponent(removerAcentos(employer.util.findControl('txtFuncaoPre').val().trim()).toLowerCase().replace(/ /g, "-").replace('#', '(csharp)'));
        }
    }
    if (employer.util.findControl('txtCidadeMaster').val().trim() != "") {
        var cidadeEstado = Principal.CarregarCidade(employer.util.findControl('txtCidadeMaster').val());
        if (cidadeEstado.error == null && cidadeEstado.value) {
            cidadeValida = true;
            var arrayCidade = cidadeEstado.value.split('/');
            cidade = encodeURIComponent(removerAcentos(jQuery.trim(arrayCidade[0].toLowerCase().replace(/ /g, "-"))));
            siglaEstado = jQuery.trim(arrayCidade[1].substring(0, 2)).toLowerCase();
        }
    }

    if (employer.util.findControl('txtCidadePre').val().trim() != "") {
        var cidadeEstado = Principal.CarregarCidade(employer.util.findControl('txtCidadePre').val());
        if (cidadeEstado.error == null && cidadeEstado.value) {
            cidadeValida = true;
            var arrayCidade = cidadeEstado.value.split('/');
            cidade = encodeURIComponent(removerAcentos(jQuery.trim(arrayCidade[0].toLowerCase().replace(/ /g, "-"))));
            siglaEstado = jQuery.trim(arrayCidade[1].substring(0, 2)).toLowerCase();
        }
    }

    if (funcaoValida && cidadeValida) {
        window.location = window.location.protocol + "//" + window.location.host + "/curriculos-de-" + funcao + "-em-" + cidade + "-" + siglaEstado;
        return false;
    }
    if (funcaoValida){
        window.location = window.location.protocol + "//" + window.location.host + "/curriculos-de-" + funcao;
        return false;
    }
    if (cidadeValida) {
        window.location = window.location.protocol + "//" + window.location.host + "/curriculos-em-" + cidade + "-" + siglaEstado;
        return false;
    }
}