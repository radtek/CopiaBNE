// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.menuKey-pkd.js!


//Teclas de atalhos - Menu Principal

//PARA REVISAR - feito sem jquery
//$(document).ready(function() {
//$('.box_arvore').each( function () {
//var submenuOpcoes = $(this).children().children();
//$(this).children().insert('<span class="atalho_teclado">' + (a+1) + '</span>');
//});

//Incluindo o geral - NÃO PODE APAGAR
/// <reference path="../geral.js" />

employer.menuKey = {
    shortcutsNav: function(evt) {
        /// <summary>
        /// Atalhos da aplicação. Deve ser colocado o id dos menus da tela MenuMaisOpcoes.
        /// </summary>        
        var type = evt.type;
        var k = employer.event.getKey(evt)[0];
        var mod = employer.key.hasModifierKeyPressed(evt);
        // IE não possui charcode e só diferencia Maisculo de Minúsculo no keypress
        if (type == 'keydown' || type == 'keyup') {
            if (typeof (mod) != undefined && mod != null) {
                var menu = employer.menuKey.getMenu(mod, k);
                employer.menuKey.showMenu(menu[0], menu[1]);
                evt.preventDefault();
                evt.stopPropagation();
            }
        }
    },
    showMenu: function(menuAtual, subMenusAtual) {
        /// <summary>
        /// Mostra os menu selecionado e esconde os outros menus caso estejam abertos.
        /// </summary>     
        if (menuAtual != undefined && typeof (menuAtual) != undefined && menuAtual != '') {

            $('.atalho_teclado').fadeOut();

            $('#MenuArvoreBeneficios').slideUp();
            $('#MenuArvoreConsultarGerarArquivos').slideUp();
            $('#MenuArvoreFechamento').slideUp();
            $('#MenuArvoreProcessosLote').slideUp();
            $('#MenuArvoreRegraCalculo').slideUp();
            $('#MenuArvoreTabelasConfiguracao').slideUp();
            $('#MenuArvoreCadastrosAuxiliares').slideUp();
            $('#MenuArvoreAdministracaoSistema').slideUp();
            $('#MenuArvoreManutencaoFuncoes').slideUp();
            $('#MenuArvoreContratoProfissional').slideUp();

            if ($(menuAtual).css("display") === "none") {

                $('.atalho_teclado').css('display', 'none');

                $(menuAtual).slideDown();

                if (subMenusAtual != undefined && typeof (subMenusAtual) != undefined && subMenusAtual != '') {
                    var k;
                    for (k = 0; k < subMenusAtual.length; k++) {
                        //Cria o span somente para as nove primeiras opcoes
                        if (k <= 8) {
                            //Criando o span para mostrar nos botões
                            var newtext = document.createTextNode((k + 1).toString());
                            span = document.createElement("span");
                            span.className = "atalho_teclado";
                            span.appendChild(newtext);
                            //Incluindo o span dentro da lista
                            $get(subMenusAtual[k] + '_li').appendChild(span);
                        }

                        //Criando o evento no keydown
                        var es = "$(document).bind(\"keydown\", \"" + (k + 1).toString() + "\", function() { window.location = $(\"#" + subMenusAtual[k] + "\").attr(\"href\"); })";
                        eval(es);
                    }
                    for (k; k < 20; k++) {
                        var es = "$(document).bind(\"keydown\", \"" + (k + 1).toString() + "\", function() { window.location = 'MenuMaisOpcoes.aspx'; })";
                        eval(es);
                    }
                }
                //Depois que os spans estão criados então é usado o fadeIn
                $('.atalho_teclado').fadeIn('slow');

                //Scroll automatico nos menus com mais botoes e que nao aparecem na pagina depois de abertos
                if (menuAtual === '#MenuArvoreCadastrosAuxiliares') {
                    $.scrollTo({ top: '400px' }, 600);
                } else if (menuAtual === '#MenuArvoreConsultarGerarArquivos') {
                    $.scrollTo({ top: '50px' }, 600);
                } else if (menuAtual === '#MenuArvoreBeneficios') {
                    $.scrollTo({ top: '50px' }, 600);
                } else if (menuAtual === '#MenuArvoreRegraCalculo') {
                    $.scrollTo({ top: '100px' }, 600);
                } else if (menuAtual === '#MenuArvoreTabelasConfiguracao') {
                    $.scrollTo({ top: '150px' }, 600);
                } else if (menuAtual === '#MenuArvoreAdministracaoSistema') {
                    $.scrollTo({ top: '200px' }, 600);
                } else if (menuAtual === '#MenuArvoreManutencaoFuncoes') {
                    $.scrollTo({ top: '400px' }, 600);
                } else if (menuAtual === '#MenuArvoreContratoProfissional') {
                    $.scrollTo({ top: '400px' }, 600);
                }
            }
            else {
                $(menuAtual).slideUp();
                $('.atalho_teclado').fadeOut();
            }
        }
    },
    getMenu: function(modifier, key) {
        /// <summary>
        /// Recupera o menu e submenu a partir das teclas pressionadas
        /// </summary>   
        var menu;
        var subMenus = [];
        if (modifier === 'CTRL') {
            if (key === 51 || key === 99) {
                // CTRL + 3 (51)
                menu = '#MenuArvoreBeneficios';
                subMenus.push("Ctrl_3_1");
                subMenus.push("Ctrl_3_2");
                subMenus.push("Ctrl_3_3");
                subMenus.push("Ctrl_3_4");
                subMenus.push("Ctrl_3_5");
                subMenus.push("Ctrl_3_6");
                subMenus.push("Ctrl_3_7");
            }
            if (key === 52 || key === 100) {
                // CTRL + 4 (52)
                menu = '#MenuArvoreConsultarGerarArquivos';
                subMenus.push("Ctrl_4_1");
                subMenus.push("Ctrl_4_2");
                subMenus.push("Ctrl_4_3");
                subMenus.push("Ctrl_4_4");
                subMenus.push("Ctrl_4_5");
                subMenus.push("Ctrl_4_6");
                subMenus.push("Ctrl_4_7");
                subMenus.push("Ctrl_4_8");
                subMenus.push("Ctrl_4_9");
                subMenus.push("Ctrl_4_10");
                subMenus.push("Ctrl_4_11");
                subMenus.push("Ctrl_4_12");
                subMenus.push("Ctrl_4_13");
                subMenus.push("Ctrl_4_14");
                subMenus.push("Ctrl_4_15");
            }
            else if (key === 54 || key === 102) {
                // CTRL + 6 (54)
                menu = '#MenuArvoreContratoProfissional';
                subMenus.push("Ctrl_6_1");
                subMenus.push("Ctrl_6_2");
            }
            else if (key === 56 || key === 104) {
                // CTRL + 8 (56)
                menu = '#MenuArvoreFechamento';
                subMenus.push("Ctrl_8_1");
                subMenus.push("Ctrl_8_2");
            }
            else if (key === 57 || key === 105) {
                // CTRL + 9 (57)
                menu = '#MenuArvoreProcessosLote';
                subMenus.push("Ctrl_9_1");
                subMenus.push("Ctrl_9_2");
            }
            else if (key === 78) {
                // CTRL + N (78)
                menu = '#MenuArvoreRegraCalculo';
                subMenus.push("Ctrl_N_1");
                subMenus.push("Ctrl_N_2");
                subMenus.push("Ctrl_N_3");
            }
            else if (key === 84) {
                // CTRL + T (84)
                menu = '#MenuArvoreTabelasConfiguracao';
                subMenus.push("Ctrl_T_1");
                subMenus.push("Ctrl_T_2");
                subMenus.push("Ctrl_T_3");
                subMenus.push("Ctrl_T_4");
            }
            else if (key === 88) {
                // CTRL + x (88)
                menu = '#MenuArvoreCadastrosAuxiliares';
                subMenus.push("Ctrl_X_1");
                subMenus.push("Ctrl_X_2");
                subMenus.push("Ctrl_X_3");
                subMenus.push("Ctrl_X_4");
                subMenus.push("Ctrl_X_5");
                subMenus.push("Ctrl_X_6");
                subMenus.push("Ctrl_X_7");
                subMenus.push("Ctrl_X_8");
                subMenus.push("Ctrl_X_9");
                subMenus.push("Ctrl_X_10");
                subMenus.push("Ctrl_X_11");
                subMenus.push("Ctrl_X_12");
            }
            else if (key === 65) {
                // CTRL + A (65)
                menu = '#MenuArvoreAdministracaoSistema';
                subMenus.push("Ctrl_A_1");
                subMenus.push("Ctrl_A_2");
                subMenus.push("Ctrl_A_3");
                subMenus.push("Ctrl_A_4");
            }
            else if (key === 85) {
                // CTRL + U (85)
                menu = '#MenuArvoreManutencaoFuncoes';
                subMenus.push("Ctrl_U_1");
                subMenus.push("Ctrl_U_2");
                subMenus.push("Ctrl_U_3");
                subMenus.push("Ctrl_U_4");
                subMenus.push("Ctrl_U_5");
            }
        }
        return [menu, subMenus];
    }
};

function LocalPageLoad() {
    openQueryString();
}

function openQueryString() {
    //Abre a arvore de links atraves da querystring no browser
    var querystring = window.location.search.substring(1);
    if (querystring != undefined && typeof (querystring) != undefined && querystring != '') {
        var idMenu = querystring.split('=');
        //Pegando o numero do menu e tranformando em charcode para retornar o menu o os submenus
        var menu = employer.menuKey.getMenu('CTRL', idMenu[1].charCodeAt(0));
        //Mostrando o submenu
        employer.menuKey.showMenu(menu[0], menu[1]);
    }
}

$(document).bind("keydown", employer.menuKey.shortcutsNav);

//Os links sem subMenu devem continuar com o bind para as teclas
//Menu - Principal [ Ctrl + 0 ]
$(document).bind("keydown", "Ctrl+0", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "MenuPrincipal.aspx";
    return false;
});

//Lançar Folha [ Ctrl + 1 ]
$(document).bind("keydown", "Ctrl+1", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "LancamentoFolhaFuncionario.aspx";
    return false;
});

//Holerite [ Ctrl + 2 ]
$(document).bind("keydown", "Ctrl+2", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "GerarHoleriteInicial.aspx";
    return false;
});

//Cadastrar Pessoa Física [ Ctrl + 5 ]
$(document).bind("keydown", "Ctrl+5", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "PessoaFisicaCadastroDados.aspx";
    return false;
});

//Menu - Mais Opções [ Ctrl + Q ]
$(document).bind("keydown", "Ctrl+Q", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "MenuMaisOpcoes.aspx";
    return false;
});

////Contratar Profissional [ Ctrl + 6 ]
//$(document).bind("keydown", "Ctrl+6", function(e) {
//    e.preventDefault();
//    e.stopPropagation();
//    window.location = "EmpregadoCadastro.aspx";
//    return false;
//});

//Gerar Arquivo de Pagamento [ Ctrl + 7 ]
$(document).bind("keydown", "Ctrl+7", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "GerarArquivoFolha.aspx";
    return false;
});

//Estatísticas [ Ctrl + E ]
$(document).bind("keydown", "Ctrl+E", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "AcessoNegado.aspx";
    return false;
});

//Cadastrar Pessoa Jurídica [ Ctrl + J ]
$(document).bind("keydown", "Ctrl+J", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "PessoaJuridicaCadastroDados.aspx";
    return false;
});

//Administração de Sistema [ Ctrl + S ]
$(document).bind("keydown", "Ctrl+S", function(e) {
    e.preventDefault();
    e.stopPropagation();
    window.location = "MenuMaisOpcoes.aspx";
    return false;
});

//Eventos dos botões
function btiBeneficios_OnClick() {
    var mod = 'CTRL';
    var k = '3';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiRegrasNegocios_OnClick() {
    var mod = 'CTRL';
    var k = 'N';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiCadastrosAuxiliares_OnClick() {
    var mod = 'CTRL';
    var k = 'X';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiConsultarGerarArquivos_OnClick() {
    var mod = 'CTRL';
    var k = '4';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiContratoProfissional_OnClick() {
    var mod = 'CTRL';
    var k = '6';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiFechamento_OnClick() {
    var mod = 'CTRL';
    var k = '8';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiProcessosLote_OnClick() {
    var mod = 'CTRL';
    var k = '9';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiTabelasConfiguracao_OnClick() {
    var mod = 'CTRL';
    var k = 'T';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiAdministracaoSistema_OnClick() {
    var mod = 'CTRL';
    var k = 'A';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}

function btiManutencaoFuncoes_OnClick() {
    var mod = 'CTRL';
    var k = 'U';
    var menu = employer.menuKey.getMenu(mod, k.charCodeAt(0));
    employer.menuKey.showMenu(menu[0], menu[1]);
    return false;
}