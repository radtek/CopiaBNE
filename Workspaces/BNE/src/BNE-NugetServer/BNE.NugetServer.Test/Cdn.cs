using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BNE.Cdn;

namespace BNE.NugetServer.Test
{
    [TestClass]
    public class CdnTest
    {
        
        [TestMethod]
        public void TestaConfig()
        {
            CdnConfigSection config = CdnManager.config;
        }

        [TestMethod]
        public void AplicaCDN()
        {
            String resultado = CdnManager.SetCdn(htmlToTest);
        }

        private string htmlToTest = @"<html xmlns=""http://www.w3.org/1999/xhtml"" lang=""pt-br""><head id=""ctl00_Head1""><title>
	Vagas de Emprego Grátis - Busca de Vagas | SINE
</title><meta name=""google-site-verification"" content=""eHnE5TOUSJrpndyPH2njwhO0bPrrRKS-GoJ5NntpQVM""><meta http-equiv=""content-type"" content=""text/html; charset=UTF-8""><meta name=""distribution"" content=""Global""><meta name=""rating"" content=""General""><meta name=""language"" content=""pt-br""><meta name=""content-language"" content=""pt-br""><meta http-equiv=""Page-Enter"" content=""blendTrans(Duration=0.2)""><meta http-equiv=""Page-Exit"" content=""blendTrans(Duration=0.2)""><meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><meta property=""og:image"" content=""http://www.sine.com.br/img/perfil_face.png""><meta name=""viewport"" content=""width=device-width, initial-scale=1""><link rel=""icon"" href=""img/favicon.png?01122010142824""><link href=""/css/bootstrap.css"" rel=""stylesheet""><link href=""/css/stylesheet.css"" rel=""stylesheet""><link href=""/css/injected.css"" rel=""stylesheet""><link href=""css/global/font-awesome.css"" rel=""stylesheet"">
    
    <script src=""https://apis.google.com/_/scs/apps-static/_/js/k=oz.gapi.en.suYhVw2l3BQ.O/m=auth/exm=plusone/rt=j/sv=1/d=1/ed=1/am=IQ/rs=AGLTcCPHBV0CiDsBeUQFHqQSgFrKg5UHBA/t=zcms/cb=gapi.loaded_1"" async=""""></script><script src=""https://apis.google.com/_/scs/apps-static/_/js/k=oz.gapi.en.suYhVw2l3BQ.O/m=plusone/rt=j/sv=1/d=1/ed=1/am=IQ/rs=AGLTcCPHBV0CiDsBeUQFHqQSgFrKg5UHBA/t=zcms/cb=gapi.loaded_0"" async=""""></script><script type=""text/javascript"" async="""" src=""//u.heatmap.it/conf/www.sine.com.br.js""></script><script type=""text/javascript"" async="""" src=""https://apis.google.com/js/plusone.js"" gapi_processed=""true""></script><script type=""text/javascript"" src=""//navdmp.com/req?v=8&amp;id=13903205489|2&amp;acc=33524&amp;tit=Vagas%20de%20Emprego%20Gr%E1tis%20-%20Busca%20de%20Vagas%20%7C%20SINE&amp;utm=110469127.1427226639.292.10.utmcsr%3Dplugin%7Cutmccn%3Dplugin_wordpress%7Cutmcmd%3Dimagem_topo"" async=""""></script><script id=""facebook-jssdk"" async="""" src=""//connect.facebook.net/pt_BR/all.js#xfbml=1""></script><script type=""text/javascript"" async="""" src=""http://www.google-analytics.com/ga.js""></script><script type=""text/javascript"" async="""" src=""//u.heatmap.it/log.js""></script><script type=""text/javascript"" src=""//code.jquery.com/jquery-1.10.2.js""></script>
    <script type=""text/javascript"" src=""//code.jquery.com/ui/1.11.0/jquery-ui.js""></script>
    
    <script type=""text/javascript"">
        (function () {
            var hm = document.createElement('script'); hm.type = 'text/javascript'; hm.async = true;
            hm.src = ('++u-heatmap-it+log-js').replace(/[+]/g, '/').replace(/-/g, '.');
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(hm, s);
        })();
    </script>



    <script type=""text/javascript"">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-19439452-1']);
        _gaq.push(['_trackPageview']);
        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

        /*
        FUNCOES PARA CONTROLE DA MODAL DE DESTAQUE VAGA
        */
        function ShowModalDestaque() {
            $('.img_destaque').html($('.img_destaque').html().replace('<!--', '').replace('-->', ''));
            $find('ModalDestaqueVaga').show();
        }
        function HideModalDestaque() {
            $find('ModalDestaqueVaga').hide();
        }
    </script>
<link rel=""canonical"" href=""http://www.sine.com.br/""><meta name=""description"" content=""Encontre vagas de empregos no seu classificado online de ofertas de trabalho, cadastre seu currículo ou anuncie suas vagas de empregos.""><meta name=""keywords"" content=""Jornal de Vagas, Jornal de Empregos, Classificado de Empregos, Classificado de Vagas, Vagas de Empregos, Oferta de Trabalho, Anuncie Vagas para Empregos, Oportunidade para Trabalho""></head>
<body cz-shortcut-listen=""true"" style=""padding-top: 120px;"">
    <div class=""container"">
        <div id=""fb-root"">
        </div>
        <form name=""aspnetForm"" method=""post"" action="""" id=""aspnetForm"" autocomplete=""off"">
<div>
<input type=""hidden"" name=""ctl00_tsmPrincipal_HiddenField"" id=""ctl00_tsmPrincipal_HiddenField"" value="""">
<input type=""hidden"" name=""__EVENTTARGET"" id=""__EVENTTARGET"" value="""">
<input type=""hidden"" name=""__EVENTARGUMENT"" id=""__EVENTARGUMENT"" value="""">
<input type=""hidden"" name=""__VIEWSTATE"" id=""__VIEWSTATE"" value=""/wEPDwUKLTcxMDgxMTQxM2RkF0m4X0KrH3VS4yQy8TzIBGm0i9ICphyis4YmeIqBCWE="">
</div>

<script type=""text/javascript"">
//<![CDATA[
var theForm = document.forms['aspnetForm'];
function __doPostBack(eventTarget, eventArgument) {
    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
        theForm.__EVENTTARGET.value = eventTarget;
        theForm.__EVENTARGUMENT.value = eventArgument;
        theForm.submit();
    }
}
//]]>
</script>


<script src=""/WebResource.axd?d=l_LoTc3SomOnFguKcE_F6Yfc7xzNgz7ZRz28Ncyk1mvleH3yWnbMIeSE4M0wZEcJC8hWyGIBfNtw_bMoOq6lgJ3tV_4CQaC5MQPTQar2TUk1&amp;t=635375167334701695"" type=""text/javascript""></script>


<script src=""/ScriptResource.axd?d=iC75yj5JdYPnutTz0CYlzQ4lMfgpFLorpiDKJpj4RbOHRS1kWIIjaX7o89HzPCcKvciOjMx3Kik1_cMfmE4H3pCJV5C9WS_pXn12UOXlH9yqUsAl011eYEQzKAPyUMUfXSr1XmPwGETf7NbBNdIEo3X7lAXbTTGl45FozCWY8e01&amp;t=ffffffffb53e74b8"" type=""text/javascript""></script>
<script src=""/ScriptResource.axd?d=6VQhobb3Q3efeexihbT_hUKKBOOx12TSRgHw_iUHoXadRRGV1Ggq3gf3SczK-3KTMXhMEufwZgjJu4PDzTNJpg-eKy_AmX-hti5h8Icf4ccKsg085dKfkJZRyRs-V2pJyXIaplKSLqcWmoDCsUf77uMc_v4Kyrvf4k9rb629Fc6BF3TjFTIp_JVRi4G12xyB0&amp;t=ffffffffb53e74b8"" type=""text/javascript""></script>
<div>

	<input type=""hidden"" name=""__VIEWSTATEGENERATOR"" id=""__VIEWSTATEGENERATOR"" value=""CA0B0334"">
</div>
            
            <div class=""navbar navbar-default navbar-fixed-top"" role=""navigation"">
                <div class=""container-fluid"">
                    <div class=""navbar-header col-md-2"">
                        <button type=""button"" class=""navbar-toggle"" data-toggle=""collapse"" data-target="".navbar-collapse""><span class=""sr-only"">Toggle navigation</span> <span class=""icon-bar""></span><span class=""icon-bar""></span><span class=""icon-bar""></span></button>
                        <a class=""navbar-brand"" href=""/"">Sine</a>
                    </div>
                    <div class=""col-md-10"">
                        <div class=""navbar-collapse collapse"">
                            <div class=""row col-md-12"">
                                <ul class=""nav navbar-nav col-md-7"">
                                    <li class=""dropdown""><a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown""><strong>Sou Trabalhador</strong> <span class=""caret""></span></a>
                                        <ul class=""dropdown-menu"" role=""menu"">
                                            <li><a href=""/cadastrar-curriculo"">Cadastrar Currículo</a></li>
                                            <li><a href=""/cadastrar-curriculo"" onclick=""return AbrirLogin('/cadastrar-curriculo', 'seu cadastro.');"">Alterar Currículo</a></li>
                                            <li>
                                                <a href=""/alerta-de-vagas"" onclick=""return AbrirLogin('/alerta-de-vagas', 'os seus ALERTAS DE VAGAS!');"">Alerta de Vaga</a>
                                            </li>
                                            <li class=""divider""></li>
                                            <li><a href=""/busca-de-vagas"">Buscar Vaga</a></li>
                                        </ul>
                                    </li>
                                    <li class=""dropdown""><a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown""><strong>Sou Empregador</strong> <span class=""caret""></span></a>
                                        <ul class=""dropdown-menu"" role=""menu"">
                                            <li><a href=""/anunciar-vagas-empregos"">Anunciar Vaga</a></li>
                                            <li><a href=""/sine-convoca"">Convocar Candidatos</a></li>
                                            <li><a href=""/administrar-vagas "" onclick=""return AbrirLogin('/administrar-vagas', 'a área administrativa.', 'pj');"">Minhas Vagas Anunciadas</a></li>
                                            <li class=""divider""></li>
                                            <li>
                                                
                                                <a href=""/destaque-vaga"">Destacar Minhas Vagas</a>
                                            </li>
                                        </ul>
                                    </li>
                                    <li class=""dropdown""><a href=""/pesquisa-salarial""><strong>Pesquisa Salarial</strong> </a>
                                    </li>
                                </ul>
                                <div class=""redes-sociais hidden-xs col-md-3"">
                                    <div class=""fb-like"" data-href=""https://www.facebook.com/SINE.BR"" data-width=""450"" data-layout=""button_count"" data-show-faces=""false"" data-send=""false"">
                                    </div>
                                    <!-- Place this tag where you want the +1 button to ren/der. -->
                                    <div id=""___plusone_0"" style=""text-indent: 0px; margin: 0px; padding: 0px; border-style: none; float: none; line-height: normal; font-size: 1px; vertical-align: baseline; display: inline-block; width: 90px; height: 20px; background: transparent;""><iframe frameborder=""0"" hspace=""0"" marginheight=""0"" marginwidth=""0"" scrolling=""no"" style=""position: static; top: 0px; width: 90px; margin: 0px; border-style: none; left: 0px; visibility: visible; height: 20px;"" tabindex=""0"" vspace=""0"" width=""100%"" id=""I0_1427316928377"" name=""I0_1427316928377"" src=""https://apis.google.com/u/0/se/0/_/+1/fastbutton?usegapi=1&amp;size=medium&amp;hl=pt-BR&amp;origin=http%3A%2F%2Fwww.sine.com.br&amp;url=http%3A%2F%2Fwww.sine.com.br%2F&amp;gsrc=3p&amp;ic=1&amp;jsh=m%3B%2F_%2Fscs%2Fapps-static%2F_%2Fjs%2Fk%3Doz.gapi.en.suYhVw2l3BQ.O%2Fm%3D__features__%2Fam%3DIQ%2Frt%3Dj%2Fd%3D1%2Ft%3Dzcms%2Frs%3DAGLTcCPHBV0CiDsBeUQFHqQSgFrKg5UHBA#_methods=onPlusOne%2C_ready%2C_close%2C_open%2C_resizeMe%2C_renderstart%2Concircled%2Cdrefresh%2Cerefresh%2Conload&amp;id=I0_1427316928377&amp;parent=http%3A%2F%2Fwww.sine.com.br&amp;pfname=&amp;rpctoken=41553092"" data-gapiattached=""true"" title=""+1""></iframe></div>
                                </div>
                                <div id=""divLogin"" class=""col-md-2"" style=""display: none;"">
                                    <a href=""#"" class=""btn btn-primary pull-right"" onclick=""return AbrirLogin('PagAtual', 'as VAGAS!');"">Entrar</a>
                                </div>
                                <div id=""divInfoLogin"" style="""" class=""login-info nav navbar-nav col-md-2"">Olá, <b id=""nomeLogin"">Francisco</b>| <a href=""/usuario/logout?tk=65468934"">Sair</a></div>
                            </div>
                        </div>
                        <div class=""row col-md-12"">
                            <div class=""form-group col-md-5"">
                                
                                <input name=""ctl00$txtFuncaoTopo"" type=""text"" id=""txtFuncaoTopo"" class=""form-control ui-autocomplete-input"" placeholder=""Função"" autocomplete=""off"">
                            </div>
                            <div class=""form-group col-md-5"">
                                
                                <input name=""ctl00$txtCidadeTopo"" type=""text"" id=""txtCidadeTopo"" class=""form-control ui-autocomplete-input"" placeholder=""Cidade"" autocomplete=""off"">
                            </div>
                            <div class=""form-group col-md-2"">
                                <input type=""button"" class=""btn btn-default"" onclick=""return submitPesquisa();"" value=""Buscar Vagas"">
                            </div>
                        </div>
                    </div>
                    <!--/.nav-collapse -->
                </div>
                <!--/.container-fluid -->
            </div>
            
            <script type=""text/javascript"">
//<![CDATA[
Sys.WebForms.PageRequestManager._initialize('ctl00$tsmPrincipal', 'aspnetForm', [], [], [], 90, 'ctl00');
//]]>
</script>

            
    
    <h1 id=""titulo"">230.630 Vagas de Emprego</h1>
    
    <div class=""row lista-links-home"">
        
        <div id=""ctl00_cphConteudo_divLinks1"" class=""col-md-3"">
            <h2>Vagas por cidade</h2>
            <ul class=""list-group"">
                
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-rio-de-janeiro-rj"" title=""Vagas em Rio de Janeiro / RJ"">
                                <h3>Rio de Janeiro / RJ  (208.239)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-campinas-sp"" title=""Vagas em Campinas / SP"">
                                <h3>Campinas / SP  (76.122)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-sao-paulo-sp"" title=""Vagas em São Paulo / SP"">
                                <h3>São Paulo / SP  (63.143)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-belo-horizonte-mg"" title=""Vagas em Belo Horizonte / MG"">
                                <h3>Belo Horizonte / MG  (59.877)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-fortaleza-ce"" title=""Vagas em Fortaleza / CE"">
                                <h3>Fortaleza / CE  (58.161)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-maringa-pr"" title=""Vagas em Maringá / PR"">
                                <h3>Maringá / PR  (54.879)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-porto-alegre-rs"" title=""Vagas em Porto Alegre / RS"">
                                <h3>Porto Alegre / RS  (36.618)</h3>
                            </a></li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos-em-curitiba-pr"" title=""Vagas em Curitiba / PR"">
                                <h3>Curitiba / PR  (28.822)</h3>
                            </a></li>
                    
                <li class=""text-right""><a href=""/busca-de-vagas"">Busca de vagas por estado</a></li>
            </ul>
        </div>
        <div id=""ctl00_cphConteudo_divLinks2"" class=""col-md-3"">
            <h2>Vagas por função</h2>
            <input type=""hidden"" name=""ctl00$cphConteudo$hdnIdCidade"" id=""ctl00_cphConteudo_hdnIdCidade"" value=""0"">
            <input type=""hidden"" name=""ctl00$cphConteudo$hdnIdFuncao"" id=""ctl00_cphConteudo_hdnIdFuncao"" value=""0"">
            <ul class=""list-group"">
                
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/vendedor"" title=""Vagas para Vendedor"">
                                <h3>Vendedor  (39.322)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/estagiario"" title=""Vagas para Estagiário"">
                                <h3>Estagiário  (29.621)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/auxiliar-de-servicos-gerais"" title=""Vagas para Auxiliar de Serviços Gerais"">
                                <h3>Auxiliar de Serviços Gerais  (21.079)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/atendente"" title=""Vagas para Atendente"">
                                <h3>Atendente  (16.740)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/auxiliar-administrativo"" title=""Vagas para Auxiliar Administrativo"">
                                <h3>Auxiliar Administrativo  (16.614)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/recepcionista"" title=""Vagas para Recepcionista"">
                                <h3>Recepcionista  (15.312)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/vendedor-externo"" title=""Vagas para Vendedor Externo"">
                                <h3>Vendedor Externo  (13.102)</h3>
                            </a>
                        </li>
                    
                        <li class=""list-group-item"">
                            <a href=""/vagas-empregos/auxiliar-de-cozinha"" title=""Vagas para Auxiliar de Cozinha"">
                                <h3>Auxiliar de Cozinha  (11.821)</h3>
                            </a>
                        </li>
                    
                <li class=""text-right""><a href=""/busca-de-vagas-area"">Busca de vaga por área</a></li>
            </ul>
        </div>
        <div id=""pnlAnuncio"" class=""col-md-6"">
            <div class=""row"">
                
                <a href=""#myModalVideo"" title=""Ver vídeo!"" role=""button"" data-toggle=""modal"">
                    <img alt=""Anuncie no SINE"" class=""img-responsive col-md-12"" width=""560px"" height=""284px"" src=""/img/video/imgVejaVideo.jpg"">
                </a>
            </div>
        </div>
    </div>
    <div class=""row jobs"">
        
                <div id=""ctl00_cphConteudo_rptVagas_ctl00_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl00_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl00_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1752377"" href=""/vagas-empregos-em-petropolis-rj/vendedor-externo/1752377"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor Externo
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl00_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           VENDEDORA EXTERNA COM EXPERIENCIA NA AREA DE PROPAGANDA E MARKETING,COMUNICATIVA,PERSISTENTENTE,QUE GOSTE DE TRABALHAR COM PUBLICO E TENHA PACIENCIA E...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl01_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl01_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl01_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1751900"" href=""/vagas-empregos-em-petropolis-rj/vendedor-interno/1751900"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor Interno
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl01_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Nexvr Comercio De Aparelhos Telefonicos Ltda</span>
                       </p>
                       <p itemprop=""description"">
                           VENDEDOR INTERNO PETRÓPOLIS Essa vaga é exclusiva para pessoas com experiência em VENDAS INTERNAS, ou seja, Loja, Shopping, Quiosque. PRODUTO: NEXTEL ...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl02_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl02_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl02_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1751773"" href=""/vagas-empregos-em-petropolis-rj/vendedor-de-servicos/1751773"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor de Serviços
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl02_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Fgssistemas</span>
                       </p>
                       <p itemprop=""description"">
                           Precisamos de vendedor externo, dinâmico e goste de trabalhar com vendas. Oferecemos piso do comercio, vale transporte, comissão bem atrativa paga na ...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl03_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl03_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl03_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1750470"" href=""/vagas-empregos-em-petropolis-rj/representante-comercial/1750470"">
                           <h4 itemprop=""title"">
                               Vaga de Representante Comercial
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl03_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Softconn Tecnologia &amp; Sistemas</span>
                       </p>
                       <p itemprop=""description"">
                           Breve descrição empresa:       Indústria de desenvolvimento de softwares Modulares, Customizados e parametrizados para Gestão Empresarial Integrada, o...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl04_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl04_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl04_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1743959"" href=""/vagas-empregos-em-petropolis-rj/operador-de-caixa/1743959"">
                           <h4 itemprop=""title"">
                               Vaga de Operador de Caixa
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl04_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Real Park Estacionamentos Lta</span>
                       </p>
                       <p itemprop=""description"">
                           ESTACIONAMENTO CONTRATA OPERADOR DE CAIXA MASCULINO/FEMININO PARA ALTO DA SERRA ALMOÇO NO LOCAL + LANCHE CONTRATAÇÃO IMEDIATA HORÁRIO NOTURNO COM OU S...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl05_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl05_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl05_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1742631"" href=""/vagas-empregos-em-petropolis-rj/pintor/1742631"">
                           <h4 itemprop=""title"">
                               Vaga de Pintor
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl05_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           EMPRESA CONTRATA URGENTE PINTOR PARA TRABALHAR EM PETROPOLIS. SALARIO + BENEFICIOS</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl06_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl06_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl06_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1742627"" href=""/vagas-empregos-em-petropolis-rj/servente-de-obras/1742627"">
                           <h4 itemprop=""title"">
                               Vaga de Servente de Obras
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl06_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           EMPRESA CONTRATA URGENTE SERVENTE PARA TRABALHAR EM PETROPOLIS. SALARIO + BENEFICIOS.</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl07_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl07_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl07_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1742537"" href=""/vagas-empregos-em-petropolis-rj/operador-de-acabamento/1742537"">
                           <h4 itemprop=""title"">
                               <div id=""ctl00_cphConteudo_rptVagas_ctl07_UcVaga_divPcd"" class=""fa fa-wheelchair fa-1x ico_pcd""></div>Vaga de Operador de Acabamento
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl07_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Ambev</span>
                       </p>
                       <p itemprop=""description"">
                           Operar os equipamentos sob sua responsabilidade, através da aplicação dos conhecimentos adquiridos no treinamento básico e padrões complementares, res...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl08_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl08_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl08_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1746928"" href=""/vagas-empregos-em-petropolis-rj/operador-de-bate-estacas/1746928"">
                           <h4 itemprop=""title"">
                               Vaga de Operador de Bate-Estacas
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl08_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Ambev.Com.Br</span>
                       </p>
                       <p itemprop=""description"">
                           Operador de packaging – pcd
requisitos: ensino médio completo
atividades a serem desenvolvidas: operar os equipamentos sob sua responsabilidade, atrav...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl09_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl09_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl09_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1742105"" href=""/vagas-empregos-em-petropolis-rj/pedreiro/1742105"">
                           <h4 itemprop=""title"">
                               Vaga de Pedreiro
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl09_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           EMPRESA CONTRATA PEDREIROS DE ACABAMENTO PARA TRABALHAR EM PETRÓPOLIS. INTERESSADOS ENTRAR EM CONTATO.</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl10_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl10_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl10_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1742104"" href=""/vagas-empregos-em-petropolis-rj/vendedor-externo/1742104"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor Externo
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl10_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Fgs</span>
                       </p>
                       <p itemprop=""description"">
                           Precisamos de vendedor(a) externo que possua experiência em vendas técnicas, que seja bem comunicativa, organizada e que goste de trabalhar com vendas...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl11_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl11_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl11_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1742098"" href=""/vagas-empregos-em-petropolis-rj/vendedor/1742098"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl11_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           CONTRATA -SE VENDEDORA PARA ATUAR EM ESCOLA DE CURSOS DA CIDADE, EMPRESA DE 20 ANOS NO MERCADO. BENEFÍCIOS SALÁRIO + COMISSÃO + BÔNUS POR META ATINGID...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl12_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl12_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl12_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1738981"" href=""/vagas-empregos-em-petropolis-rj/vendedor/1738981"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl12_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Hotmart</span>
                       </p>
                       <p itemprop=""description"">
                           Função : O trabalhador irá trabalhar com vendas pela internet para a empresa, para mais informações acesse o site para preencher a ficha de inscrição,...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl13_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl13_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl13_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1733823"" href=""/vagas-empregos-em-petropolis-rj/nutricionista/1733823"">
                           <h4 itemprop=""title"">
                               Vaga de Nutricionista
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl13_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Omega Alimentação E Serviços Especializados Ltda</span>
                       </p>
                       <p itemprop=""description"">
                           Experiência: Administração de Cozinha Industrial, ter formação em Nutrição. Atividades relativas à : Administração e controle operacional em serviços ...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl14_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl14_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl14_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1735493"" href=""/vagas-empregos-em-petropolis-rj/entregador/1735493"">
                           <h4 itemprop=""title"">
                               Vaga de Entregador
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl14_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Futurestep.Com,</span>
                       </p>
                       <p itemprop=""description"">
                           Atividades:
conferência de pedidos por pontos de entrega;
organização das caixas no veículo;
entrega dos pedidos nos clientes;
entrega das notas fisca...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl15_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl15_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl15_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1730774"" href=""/vagas-empregos-em-petropolis-rj/entregador/1730774"">
                           <h4 itemprop=""title"">
                               Vaga de Entregador
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl15_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Futurestep.Com</span>
                       </p>
                       <p itemprop=""description"">
                           Entregador
atividades:
conferência de pedidos por pontos de entrega;
organização das caixas no veículo;
entrega dos pedidos nos clientes;
entrega das ...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl16_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl16_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl16_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1723628"" href=""/vagas-empregos-em-petropolis-rj/cozinheiro/1723628"">
                           <h4 itemprop=""title"">
                               Vaga de Cozinheiro
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl16_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Churrascaria Majórica</span>
                       </p>
                       <p itemprop=""description"">
                           Precisa-se de cozinheiro com experiência em restaurante a la carte.</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl17_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl17_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl17_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1724120"" href=""/vagas-empregos-em-petropolis-rj/auxiliar-de-servicos-gerais/1724120"">
                           <h4 itemprop=""title"">
                               Vaga de Auxiliar de Serviços Gerais
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl17_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Confidencial</span>
                       </p>
                       <p itemprop=""description"">
                           Empresa de grande porte no segmento de asseio e conservação contrata:
auxiliar de serviços gerais
salario: R$ 900,00
benefício: vt + vr
local: petrópo...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl18_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl18_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl18_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1717578"" href=""/vagas-empregos-em-petropolis-rj/vendedor-externo/1717578"">
                           <h4 itemprop=""title"">
                               Vaga de Vendedor Externo
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl18_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Confidencial</span>
                       </p>
                       <p itemprop=""description"">
                           Vendedor externo

com no mínimo 6 meses de experiência com vendas externa;
conhecimento do pacote office e internet;
imprescindível possuir carro....</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl19_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl19_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl19_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1710092"" href=""/vagas-empregos-em-petropolis-rj/consultor-de-vendas/1710092"">
                           <h4 itemprop=""title"">
                               Vaga de Consultor de Vendas
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl19_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Eurodata Educação Para O Mercado De Trabalho</span>
                       </p>
                       <p itemprop=""description"">
                           VAGA/ CONSULTOR DE VENDAS ( SEXO FEMININO)  PARA ATUAR INTERNAMENTE NA CONSULTORIA DE NOVOS CLIENTES E FINALIZAÇÃO DA VENDA. EMPRESA NO RAMO DE CURSOS...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl20_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl20_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl20_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1708798"" href=""/vagas-empregos-em-petropolis-rj/baba/1708798"">
                           <h4 itemprop=""title"">
                               Vaga de Babá
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl20_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Particular</span>
                       </p>
                       <p itemprop=""description"">
                           Procura-se babá com experiencia e com flexibilidade de horario. Assinamos carteira. Interessadas enviar curriculo por email ou entrar em contato pelo ...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl21_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl21_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl21_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1702481"" href=""/vagas-empregos-em-petropolis-rj/manicure/1702481"">
                           <h4 itemprop=""title"">
                               Vaga de Manicure
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl21_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Studio Hans Petrópolis</span>
                       </p>
                       <p itemprop=""description"">
                           Salão de beleza na rua 16 de março, admite manicures qualificadas e experientes. ótimo ambiente e estrutura super moderna!  Venha fazer parte da nossa...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl22_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl22_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl22_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1702450"" href=""/vagas-empregos-em-petropolis-rj/representante-de-vendas/1702450"">
                           <h4 itemprop=""title"">
                               Vaga de Representante de Vendas
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl22_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           Empresa que fabrica e comercializa produtos de beleza, busca Representante de Vendas para Petrópolis e Região.  É necessário residir em Petrópolis e t...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl23_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl23_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl23_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1683175"" href=""/vagas-empregos-em-petropolis-rj/manicure/1683175"">
                           <h4 itemprop=""title"">
                               Vaga de Manicure
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl23_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Esmalteria</span>
                       </p>
                       <p itemprop=""description"">
                           Manicure e Pedicure para trabalhar em Esmalteria (abrirá em breve) salário compatível com mercado + carteira assinada + hora extra. Jornada de trabalh...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl24_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl24_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl24_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1721159"" href=""/vagas-empregos-em-petropolis-rj/armador-de-ferros/1721159"">
                           <h4 itemprop=""title"">
                               Vaga de Armador de Ferros
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl24_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Isomast.Com.Br</span>
                       </p>
                       <p itemprop=""description"">
                           empresa no ramo de impermeabilização e obra civil
Cargo:: armador
Número de vagas: 2
descrição do Cargo: / responsabilidades:
ensino fundamental c...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl25_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl25_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl25_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1680669"" href=""/vagas-empregos-em-petropolis-rj/telemarketing/1680669"">
                           <h4 itemprop=""title"">
                               Vaga de Telemarketing
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl25_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           Contrata-se estagiária para escola de cursos de 20 anos de mercado, para o cargo de TELEMARKETING  Requisitos: Esta cursado Ensino Médio, ter boa fluê...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl26_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl26_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl26_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1678061"" href=""/vagas-empregos-em-petropolis-rj/motoboy/1678061"">
                           <h4 itemprop=""title"">
                               Vaga de Motoboy
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl26_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Petrolog Express</span>
                       </p>
                       <p itemprop=""description"">
                           Preciso de motoboy habilitado com moto própria para trabalhar a noite em pizzaria</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl27_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl27_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl27_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1670414"" href=""/vagas-empregos-em-petropolis-rj/operador-de-caminhao/1670414"">
                           <h4 itemprop=""title"">
                               Vaga de Operador de Caminhão
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl27_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Britanite</span>
                       </p>
                       <p itemprop=""description"">
                           Motorista para operar caminhão de produtos explosivos.  Necessário CNH D e curso do MOPP.</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl28_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl28_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl28_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1683889"" href=""/vagas-empregos-em-petropolis-rj/supervisor-de-merchandising/1683889"">
                           <h4 itemprop=""title"">
                               Vaga de Supervisor de Merchandising
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl28_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Confidencial</span>
                       </p>
                       <p itemprop=""description"">
                           Oferecemos:
R$ fixo + variável
assistência médica, assistência odontológica, celular corporativo, vale-refeição, km, reembolso de estacionamento e ped...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl29_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl29_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl29_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1721645"" href=""/vagas-empregos-em-petropolis-rj/promotor-de-vendas/1721645"">
                           <h4 itemprop=""title"">
                               Vaga de Promotor de Vendas
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl29_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Unilider.Com.Br</span>
                       </p>
                       <p itemprop=""description"">
                           unilider distribuidora
Cargo:: promotor de vendas - petropolis
Número de vagas: 3
descrição do Cargo: / responsabilidades:
promover as vendas dent...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl30_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl30_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl30_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1669926"" href=""/vagas-empregos-em-petropolis-rj/auxiliar-de-estoque/1669926"">
                           <h4 itemprop=""title"">
                               Vaga de Auxiliar de Estoque
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl30_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Union - Citroen</span>
                       </p>
                       <p itemprop=""description"">
                           AUXILIAR NO ESTOQUE DE PEÇAS.   A EMPRESA OFERECE: SALÁRIO + REFEIÇÃO +  VALE TRANSPORTE SE NECESSÁRIO + PLANO DE SAÚDE APÓS A EXPERIÊNCIA.</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl31_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl31_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl31_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1669924"" href=""/vagas-empregos-em-petropolis-rj/assistente-de-garantia-da-qualidade/1669924"">
                           <h4 itemprop=""title"">
                               Vaga de Assistente de Garantia da Qualidade
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl31_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Union - Citroen</span>
                       </p>
                       <p itemprop=""description"">
                           FAZER GARANTIA DE PEÇAS E SERVIÇOS DE ACORDO COM O CARGO.    A EMPRESA OFERECE: SALÁRIO + REFEIÇÃO + VALE TRANSPORTE SE NECESSÁRIO + PLANO DE SAÚDE AP...</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl32_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl32_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl32_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1669922"" href=""/vagas-empregos-em-petropolis-rj/ajudante-de-mecanico/1669922"">
                           <h4 itemprop=""title"">
                               Vaga de Ajudante de Mecânico
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl32_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Union - Citroen</span>
                       </p>
                       <p itemprop=""description"">
                           AUXILIAR O MECÂNICO NA OFICINA.  A EMPRESA OFERECE: SALÁRIO + INSALUBRIDADE + REFEIÇÃO + VALE TRANSPORTE SE NECESSÁRIO + PLANO DE SAÚDE APÓS A EXPERIÊ...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl33_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl33_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl33_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1669234"" href=""/vagas-empregos-em-petropolis-rj/ajudante-geral/1669234"">
                           <h4 itemprop=""title"">
                               Vaga de Ajudante Geral
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl33_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Engeprime Engenharia Ltda</span>
                       </p>
                       <p itemprop=""description"">
                           RESIDE EM PETRÓPOLIS 2º GRAU COMPLETO EXPERIENCIA DE 1 ANO</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl34_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl34_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl34_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1669232"" href=""/vagas-empregos-em-petropolis-rj/eletricista/1669232"">
                           <h4 itemprop=""title"">
                               Vaga de Eletricista
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl34_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Engeprime Engenharia Ltda</span>
                       </p>
                       <p itemprop=""description"">
                           RESIDE EM PETRÓPOLIS 2º COMPLETO EXPERIENCIA</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl35_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl35_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl35_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1665286"" href=""/vagas-empregos-em-petropolis-rj/auxiliar-de-cozinha/1665286"">
                           <h4 itemprop=""title"">
                               Vaga de Auxiliar de Cozinha
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl35_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Crepe Delícia</span>
                       </p>
                       <p itemprop=""description"">
                           turno noite, 16 as 00:20, com 4 folgas semanais. masculino ou feminino responsável, com noções de higiene ;não precisa experiência, damos treinamento.</p>
                   
	</div>
               
</div>
               
            
                
           </div>
           <div class=""row jobs"">
               <div id=""ctl00_cphConteudo_rptVagas_ctl36_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl36_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl36_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1659739"" href=""/vagas-empregos-em-petropolis-rj/professor-de-ingles/1659739"">
                           <h4 itemprop=""title"">
                               Vaga de Professor de Inglês
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl36_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           Ministrar aulas, aplicar testes e acompanhar os alunos. Ensino Médio ou Superior. Benefícios: Curso de idiomas, Vale-transporte Regime de contratação:...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl37_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl37_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl37_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1659737"" href=""/vagas-empregos-em-petropolis-rj/coordenador-pedagogico/1659737"">
                           <h4 itemprop=""title"">
                               Vaga de Coordenador Pedagógico
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl37_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           Atuar como coordenador pedagógico em curso de idiomas. Supervisionar atividades pedagógicas e aulas dos professores, eventos, festas, entre outras ati...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl38_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl38_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl38_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1659735"" href=""/vagas-empregos-em-petropolis-rj/divulgador/1659735"">
                           <h4 itemprop=""title"">
                               Vaga de Divulgador
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl38_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">(Confidencial)</span>
                       </p>
                       <p itemprop=""description"">
                           Atuar na prospecção e fidelização de clientes, realizar visitas externas em busca de parcerias com escolas de ensino regular, universidades, empresas,...</p>
                   
	</div>
               
</div>
               
            
                <div id=""ctl00_cphConteudo_rptVagas_ctl39_UcVaga_pnlVaga"" class=""vaga col-md-3 item"" itemscope="""" itemtype=""http://schema.org/JobPosting"">
	
                   <div id=""ctl00_cphConteudo_rptVagas_ctl39_UcVaga_pnlBlocoVaga"" class=""item-block col-md-12"">
		
                       
                       <a id=""ctl00_cphConteudo_rptVagas_ctl39_UcVaga_hlVaga"" class=""lnkVaga"" id-vaga=""1662672"" href=""/vagas-empregos-em-petropolis-rj/auxiliar-de-cobranca/1662672"">
                           <h4 itemprop=""title"">
                               Vaga de Auxiliar de Cobrança
                               <span> em Petrópolis/RJ
                               </span>
                           </h4>
                       </a>
                       <p itemprop=""jobLocation"" itemscope="""" itemtype=""http://schema.org/Place"">
                           <span class=""glyphicon glyphicon-map-marker""></span>
                           <span itemprop=""address"" itemscope="""" itemtype=""http://schema.org/Postaladdress"">
                               <span itemprop=""addressLocality"">
                                   Petrópolis/RJ
                               </span>
                           </span>
                       </p>
                       <p id=""ctl00_cphConteudo_rptVagas_ctl39_UcVaga_hiringOrganization"" itemprop=""hiringOrganization"" itemtype=""http://schema.org/Organization"">
                           <span class=""glyphicon glyphicon-briefcase""></span>
                           <span itemprop=""empresa"">Escola De Campeoes Ensino E Desenvolvimento Profissional Ltda. - Epp (via <a class=""link_via"" target=""_blank"" href=""http://www.bne.com.br/vaga/AuxiliardeCobrança/Petrópolis/Rio de Janeiro-RJ/895573"">BNE</a>)</span>
                       </p>
                       <p itemprop=""description"">
                           Auxiliar e dar suporte às rotinas da área de cobrança, controle de títulos a receber, envio de títulos a banco, cálculo de juros. Efetuar remessas ban...</p>
                   
	</div>
               
</div>
               
            
    </div>
    <div class=""row col-md-12"" id=""progress_vagas_loading"" style=""display: none;"">
        <img src=""/img/ajax-loader.gif"" alt=""Carregando informações!"" style=""border-width: 0px;"" height=""32"" width=""32"">
    </div>
    <div id=""pnlBtnCarregarMaisVagas"" class=""row col-md-12 text-center"">
        <input type=""button"" value=""Carregar mais vagas"" class=""btn btn-primary"" onclick=""btnCarregarMaisVagasClicked = true; resultadoVazio = false; carregar($('#hfIdFuncaoTopo').val(), $('#hfIdCidadeTopo').val());"">
    </div>
    <div class=""col-md-12 text-center alert alert-warning"" id=""msg-nao-ha-mais-vagas"" style=""display: none;"">
        Não há mais vagas para sua pesquisa!!!
    </div>

   
    
    <script type=""text/javascript"">
        var isLoading = false;
        $(document).ready(function () {
            $(window).scroll(function () {
                if (!isLoading) {
                    scrollPercentage = 100 * $(this).scrollTop() / ($('.container').height() - $(this).height());

                    if (scrollPercentage >= 80 && !isLoading) {
                        isLoading = true;
                        carregar($(""#hfIdFuncaoTopo"").val(), $(""#hfIdCidadeTopo"").val());
                    }
                }

                //Condição criada para telas muito grandes
                if ($(""body"").outerHeight(true) < $(window).height()) {
                    carregar($(""#hfIdFuncaoTopo"").val(), $(""#hfIdCidadeTopo"").val(), 2);
                }
            });

            Vaga_setClickEvent();
        });

     

        function doValidate() {
            if (Page_ClientValidate() == true) {
                (Solicitar() == true)

            }
        }
    </script>
    
    
     <div id=""myModalVideo"" class=""modal fade"">
                <div class=""modal-dialog"">
                    <div class=""modal-content"">
                        <div class=""modal-header"">
                            <p><button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true"">x</button></p>
                            <h3>Como funciona!</h3>
                        </div>
                        <div class=""modal-body"">
                            <p><iframe width=""100%"" height=""315"" src=""https://www.youtube.com/embed/v4V5XuUXI6Q"" frameborder=""0"" allowfullscreen=""""></iframe></p>
                           
                        </div>
                    </div>
                </div>
            </div>
    

            <hr>
            <div id=""ctl00_pnlRodapeCandidato"" class=""footer row"">
	
                <div class=""col-md-4"">
                    <b>Últimas Vagas Visualizadas</b>
                    <ul class=""list-unstyled"">
                        
                                <li><a href=""/vagas-empregos-em-rio-de-janeiro-rj"" title=""Vaga de emprego em Rio de Janeiro / RJ"">Vaga em Rio de Janeiro / RJ</a></li>
                            
                                <li><a href=""/vagas-empregos-em-campinas-sp"" title=""Vaga de emprego em Campinas / SP"">Vaga em Campinas / SP</a></li>
                            
                                <li><a href=""/vagas-empregos-em-sao-paulo-sp"" title=""Vaga de emprego em São Paulo / SP"">Vaga em São Paulo / SP</a></li>
                            
                                <li><a href=""/vagas-empregos-em-belo-horizonte-mg"" title=""Vaga de emprego em Belo Horizonte / MG"">Vaga em Belo Horizonte / MG</a></li>
                            
                                <li><a href=""/vagas-empregos-em-fortaleza-ce"" title=""Vaga de emprego em Fortaleza / CE"">Vaga em Fortaleza / CE</a></li>
                            
                                <li><a href=""/vagas-empregos-em-maringa-pr"" title=""Vaga de emprego em Maringá / PR"">Vaga em Maringá / PR</a></li>
                            
                                <li><a href=""/vagas-empregos-em-porto-alegre-rs"" title=""Vaga de emprego em Porto Alegre / RS"">Vaga em Porto Alegre / RS</a></li>
                            
                    </ul>
                </div>
                <div class=""col-md-4"">
                    <b>Vagas mais concorridas</b>
                    <ul class=""list-unstyled"">
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp"" title=""Vagas de Empregos em São Paulo / SP"">Vagas de Empregos em São Paulo / SP</a></li>
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp/auxiliar-administrativo"" title=""Vaga de Auxiliar Administrativo em São Paulo / SP"">Vaga de Auxiliar Administrativo em São Paulo / SP</a></li>
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp/assistente-administrativo"" title=""Vaga de Assistente Administrativo em São Paulo / SP"">Vaga de Assistente Administrativo em São Paulo / SP</a></li>
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp/vendedor"" title=""Vaga de Vendedor em São Paulo / SP"">Vaga de Vendedor em São Paulo / SP</a></li>
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp/estagiario"" title=""Vaga de Estagiário em São Paulo / SP"">Vaga de Estagiário em São Paulo / SP</a></li>
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp/recepcionista"" title=""Vaga de Recepcionista em São Paulo / SP"">Vaga de Recepcionista em São Paulo / SP</a></li>
                        <li><a href=""http://www.sine.com.br/vagas-empregos-em-sao-paulo-sp/ajudante-de-producao"" title=""Vaga de Ajudante de Produção em São Paulo / SP"">Vaga de Ajudante de Produção em São Paulo / SP</a></li>
                    </ul>
                </div>
                <div class=""col-md-4"">
                    <b>Vagas mais buscadas</b>
                    <ul class=""list-unstyled"">
                        
                                <li><a href=""/vagas-empregos-em-belo-horizonte-mg/auxiliar-administrativo"" title=""Vagas de emprego para Auxiliar Administrativo em Belo Horizonte / MG"">Vagas para Auxiliar Administrativo em Belo Horizonte / MG</a></li>
                            
                                <li><a href=""/vagas-empregos-em-belo-horizonte-mg/recepcionista"" title=""Vagas de emprego para Recepcionista em Belo Horizonte / MG"">Vagas para Recepcionista em Belo Horizonte / MG</a></li>
                            
                                <li><a href=""/vagas-empregos-em-rio-de-janeiro-rj/recepcionista"" title=""Vagas de emprego para Recepcionista em Rio de Janeiro / RJ"">Vagas para Recepcionista em Rio de Janeiro / RJ</a></li>
                            
                                <li><a href=""/vagas-empregos-em-rio-de-janeiro-rj/auxiliar-administrativo"" title=""Vagas de emprego para Auxiliar Administrativo em Rio de Janeiro / RJ"">Vagas para Auxiliar Administrativo em Rio de Janeiro / RJ</a></li>
                            
                                <li><a href=""/vagas-empregos-em-rio-de-janeiro-rj/assistente-administrativo"" title=""Vagas de emprego para Assistente Administrativo em Rio de Janeiro / RJ"">Vagas para Assistente Administrativo em Rio de Janeiro / RJ</a></li>
                            
                                <li><a href=""/vagas-empregos-em-belo-horizonte-mg/assistente-administrativo"" title=""Vagas de emprego para Assistente Administrativo em Belo Horizonte / MG"">Vagas para Assistente Administrativo em Belo Horizonte / MG</a></li>
                            
                                <li><a href=""/vagas-empregos-em-brasilia-df/auxiliar-administrativo"" title=""Vagas de emprego para Auxiliar Administrativo em Brasília / DF"">Vagas para Auxiliar Administrativo em Brasília / DF</a></li>
                            
                    </ul>
                </div>
            
</div>
            
            
            <div id=""ctl00_upCarregando"" style=""display:none;"" role=""status"" aria-hidden=""true"">
	
                    <div class=""progress_background"">
                        &nbsp;
                    </div>
                    <div class=""progress_img_container"" id=""progress_img_container"">
                        <div class=""img_container"" id=""img_container"">
                            <img id=""ctl00_Image1"" src=""/img/ajax-loader.gif"" alt=""Carregando informações!"" height=""32px"" width=""32px"" style=""border-width: 0px;"">
                            <span id=""ctl00_Label1"" class=""carregando"">carregando...</span>
                        </div>
                    </div>
                
</div>
            
            <script type=""text/javascript"">
                if (typeof window.pageLoadSpecific == 'function') { Sys.Application.add_load(pageLoadSpecific); }
            </script>
        

<script type=""text/javascript"">
//<![CDATA[
Sys.Application.add_init(function() {
    $create(Sys.UI._UpdateProgress, {""associatedUpdatePanelId"":null,""displayAfter"":10,""dynamicLayout"":true}, null, null, $get(""ctl00_upCarregando""));
});
//]]>
</script>
</form>
        <form class=""row col-md-12"" id=""frmPesquisa"" method=""post"" action=""/"">
            <input name=""ctl00$hfTxtFuncaoTopo"" type=""hidden"" id=""hfTxtFuncaoTopo"">
            <input name=""ctl00$hfIdFuncaoTopo"" type=""hidden"" id=""hfIdFuncaoTopo"">
            <input name=""ctl00$hfTxtCidadeTopo"" type=""hidden"" id=""hfTxtCidadeTopo"">
            <input name=""ctl00$hfIdCidadeTopo"" type=""hidden"" id=""hfIdCidadeTopo"">
        </form>

        <form method=""post"" action=""/login?tk=65468934"">
            <div id=""mdlCadastro"" class=""modal fade bv-form"" novalidate=""novalidate"">
                <div class=""modal-dialog"">
                    <div class=""modal-content"">
                        <div class=""modal-header"">
                            <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">×</span><span class=""sr-only"">Fechar</span></button>
                            <h4 class=""modal-title"">SEJA BEM-VINDO!</h4>
                        </div>
                        <div class=""modal-body row col-md-12"">
                            <div class=""form-group col-md-12"">
                                <div class=""alert alert-danger"" role=""alert"" style=""display: none;"">
                                    <strong>Oh snap!</strong> Change a few things up and try submitting again.
                                </div>
                                <p>Preencha os dados para ter livre acesso às vagas!</p>
                            </div>
                            <input type=""hidden"" name=""url_redirecionamento_cadastro"" id=""url_redirecionamento_cadastro"">
                            <div class=""form-group col-md-6 has-feedback"">
                                <label>Email:</label>
                                <input name=""email"" type=""text"" class=""form-control"" placeholder=""e-mail"" data-bv-field=""email""><i class=""form-control-feedback"" data-bv-icon-for=""email"" style=""display: none;""></i>
                            <small class=""help-block"" data-bv-validator=""notEmpty"" data-bv-for=""email"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">O endereço de e-mail não pode estar vazio</small><small class=""help-block"" data-bv-validator=""emailAddress"" data-bv-for=""email"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">O e-mail não é válido</small></div>
                            <div class=""form-group col-md-6 has-feedback"">
                                <label>Data de Nascimento:</label>
                                <input type=""text"" name=""nascimento"" class=""form-control"" maxlength=""10"" autocomplete=""off"" data-bv-field=""nascimento""><i class=""form-control-feedback"" data-bv-icon-for=""nascimento"" style=""display: none;""></i>
                            <small class=""help-block"" data-bv-validator=""notEmpty"" data-bv-for=""nascimento"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">A data de nascimento deve ser preenchida</small><small class=""help-block"" data-bv-validator=""birthday"" data-bv-for=""nascimento"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">Data de Nascimento inválida</small><small class=""help-block"" data-bv-validator=""stringLength"" data-bv-for=""nascimento"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">Please enter a value with valid length</small></div>
                            <div class=""form-group col-md-12 has-feedback"">
                                <label>Nome:</label>
                                <input type=""text"" name=""nome"" class=""form-control"" placeholder=""Nome Completo"" data-bv-field=""nome""><i class=""form-control-feedback"" data-bv-icon-for=""nome"" style=""display: none;""></i>
                            <small class=""help-block"" data-bv-validator=""notEmpty"" data-bv-for=""nome"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">O nome deve ser preenchido</small><small class=""help-block"" data-bv-validator=""NomeCompleto"" data-bv-for=""nome"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">Preencha seu nome completo, sem abreviações</small></div>
                        </div>
                        <div class=""modal-footer"">
                            <button type=""submit"" class=""btn btn-primary large"" onclick=""return submitCadastro();"">CONTINUAR</button>

                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
        </form>

        
        <form method=""get"" action=""/usuario/login"" id=""fmLogin"" name=""fmLogin"">
            <div id=""mdlLogin"" class=""modal fade bv-form"" novalidate=""novalidate"">

                <div class=""modal-dialog"">
                    <div class=""modal-content"">
                        <div class=""modal-header"">
                            <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">×</span><span class=""sr-only"">Fechar</span></button>
                            <h4 class=""modal-title"">SEJA BEM-VINDO!</h4>
                        </div>
                        <div class=""modal-body row col-md-12"">
                            <div class=""form-group col-md-12"">
                                <div class=""alert alert-danger"" role=""alert"" style=""display: none;"">
                                    <strong>Oh snap!</strong> Change a few things up and try submitting again.
                                </div>
                                <p>
                                    Preencha os dados para acessar
                                    <span id=""ctl00_Label2"" name=""lblModalLogin""></span>
                                </p>
                            </div>
                            <div class=""row col-md-12"">
                                <input type=""hidden"" name=""url"" class=""form-control"">
                                <input type=""hidden"" name=""tk"" class=""form-control"" value=""65468934"">
                                <div class=""form-group col-md-6 has-feedback"">
                                    <label>CPF:</label>
                                    <input type=""text"" name=""cpf"" class=""form-control"" maxlength=""14"" onkeyup=""proxCampo('cpf','nascimento')"" placeholder=""CPF"" autocomplete=""off"" data-bv-field=""cpf""><i class=""form-control-feedback"" data-bv-icon-for=""cpf"" style=""display: none;""></i>
                                <small class=""help-block"" data-bv-validator=""notEmpty"" data-bv-for=""cpf"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">O CPF não pode estar vazio</small><small class=""help-block"" data-bv-validator=""cpf"" data-bv-for=""cpf"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">O CPF não é válido</small><small class=""help-block"" data-bv-validator=""stringLength"" data-bv-for=""cpf"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">Please enter a value with valid length</small></div>
                                <div class=""form-group col-md-6 has-feedback"">
                                    <label>Data de Nascimento:</label>
                                    <input type=""text"" name=""nascimento"" class=""form-control"" onkeyup=""proxCampo('nascimento','nome')"" maxlength=""10"" placeholder=""Data de Nascimento"" autocomplete=""off"" data-bv-field=""nascimento""><i class=""form-control-feedback"" data-bv-icon-for=""nascimento"" style=""display: none;""></i>
                                <small class=""help-block"" data-bv-validator=""notEmpty"" data-bv-for=""nascimento"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">A data de nascimento deve ser preenchida</small><small class=""help-block"" data-bv-validator=""birthday"" data-bv-for=""nascimento"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">Data de Nascimento inválida</small><small class=""help-block"" data-bv-validator=""stringLength"" data-bv-for=""nascimento"" data-bv-result=""NOT_VALIDATED"" style=""display: none;"">Please enter a value with valid length</small></div>
                                <div class=""form-group col-md-12"">
                                    <label>Nome:</label>
                                    <input type=""text"" name=""nome"" class=""form-control"" placeholder=""Nome Completo"">
                                </div>
                                <input type=""hidden"" name=""hdCadastro"" id=""hdCadastro"">
                            </div>

                        </div>
                        <div class=""modal-footer"">
                            <button type=""submit"" disabled="""" class=""btn btn-primary large"" onclick=""return submitLogin();"">CONTINUAR</button>
                            <button type=""button"" style=""display: none;"" name=""btnContinuar"" class=""btn btn-primary large"">CONTINUAR</button>
                            <div class=""row col-md-12"">
                                <div class=""form-group col-md-6"">
                                </div>
                                <div class=""form-group col-md-6"">
                                    <br>
                                    <a name=""hrefCadastro"" id=""hrefCadastro"" style=""text-decoration: underline;"">Cadastre-se gratuitamente.</a>
                                </div>
                            </div>
                        </div>

                    </div>

                    <!-- /.modal-content -->
                </div>
            </div>
        </form>
        

        <div id=""mdlDescatarVagas"" class=""modal fade"">
            <div class=""modal-dialog"">
                <div class=""modal-content"">
                    <div class=""modal-header"">
                        <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">×</span><span class=""sr-only"">Fechar</span></button>
                    </div>
                    <div class=""modal-body row"">
                        <a href=""/administrar-vagas"" rel=""nofollow"">
                            <!-- <img alt=""Destacar minhas vagas"" src=""/img/destacarvagas/telaMagica.jpg"" class=""img-responsive col-md-12"" /> -->
                        </a>
                    </div>
                </div>
            </div>
        </div>

<script type=""text/javascript"">    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.async = ""true"";
        js.src = ""//connect.facebook.net/pt_BR/all.js#xfbml=1"";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
</script>

<script type=""text/javascript"" src=""/js/bootstrap.js""></script>
<script type=""text/javascript"" src=""/js/ie-emulation-modes-warning.js""></script>
<script type=""text/javascript"" src=""/js/global/geral.js""></script>
<script type=""text/javascript"" src=""/js/jquery.plugins/bootstrapValidator.js""></script>
<script type=""text/javascript"" src=""/js/jquery.plugins/bootstrapValidator.custom.js""></script>
<script type=""text/javascript"" src=""/js/jquery.plugins/jquery.mask.js""></script>
<script type=""text/javascript"" src=""/js/jquery.plugins/jquery.bootstrap-autohidingnavbar.js""></script>
<script id=""navegg"" type=""text/javascript"" src=""//tag.navdmp.com/tm33524.js""></script>

<script type=""text/javascript"">
    window.___gcfg = { lang: 'pt-BR' };

    (function () {
        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
        po.src = 'https://apis.google.com/js/plusone.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
    })();

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

    var rotaFuncao = ""/vagas-empregos/_funcao_"";
    var rotaCidade = ""/vagas-empregos-em-_cidade_-_sigla_"";
    var rotaFuncaoCidade = ""/vagas-empregos-em-_cidade_-_sigla_/_funcao_"";
    function submitPesquisa() {
        //$(""#frmPesquisa"").action = 
        var funcao = encodeURIComponent(removerAcentos($(""#txtFuncaoTopo"").val()).toLowerCase().replace(/ /g, ""-""));
        var cidade = $(""#txtCidadeTopo"").val();
        var sigla = cidade.substring(cidade.length - 2, cidade.length).toLowerCase();
        cidade = cidade.substring(0, cidade.length - 5);
        cidade = encodeURIComponent(removerAcentos(cidade.toLowerCase().replace(/ /g, ""-"")));
        $(""#hfTxtFuncaoTopo"").val($(""#txtFuncaoTopo"").val());
        $(""#hfTxtCidadeTopo"").val($(""#txtCidadeTopo"").val());

        //Colocando zero como argumento para desconsiderar o cookie
        if ($(""#hfIdFuncaoTopo"").val() == """") { $(""#hfIdFuncaoTopo"").val(""0""); }
        if ($(""#hfIdCidadeTopo"").val() == """") { $(""#hfIdCidadeTopo"").val(""0""); }

        //Mantendo os parametros de querystring
        var queryParams = """";
        if (window.location.href.indexOf('?') > 0) {
            queryParams = window.location.href.slice(window.location.href.indexOf('?'));
        }

        if ($(""#hfIdFuncaoTopo"").val() == ""0"" && funcao != """") {
            trackEvent('PesquisaVaga', 'FuncaoInvalida', funcao);
        }

        if ($(""#hfIdFuncaoTopo"").val() != ""0"" && $(""#hfIdCidadeTopo"").val() != ""0"") {
            window.location = window.location.protocol + ""//"" + window.location.host + ""/vagas-empregos-em-"" + cidade + ""-"" + sigla + ""/"" + funcao;
            return false;
        }
        if ($(""#hfIdFuncaoTopo"").val() != ""0"") {
            window.location = window.location.protocol + ""//"" + window.location.host + ""/vagas-empregos/"" + funcao;
            return false;
        }
        if ($(""#hfIdCidadeTopo"").val() != ""0"") {
            window.location = window.location.protocol + ""//"" + window.location.host + ""/vagas-empregos-em-"" + cidade + ""-"" + sigla;
            return false;
        }


        if (parseInt($(""#hfIdFuncaoTopo"").val()) > 0 && parseInt($(""#hfIdCidadeTopo"").val()) > 0) {
            $(""#frmPesquisa"").get(0).setAttribute('action', rotaFuncaoCidade.replace(""_funcao_"", funcao).replace(""_cidade_"", cidade).replace(""_sigla_"", sigla));
            $(""#frmPesquisa"").submit();
            return true;
        }
        if (parseInt($(""#hfIdFuncaoTopo"").val()) > 0) {
            $(""#frmPesquisa"").get(0).setAttribute('action', rotaFuncao.replace(""_funcao_"", funcao));
            $(""#frmPesquisa"").submit();
            return true;
        }
        if (parseInt($(""#hfIdCidadeTopo"").val()) > 0) {
            $(""#frmPesquisa"").get(0).setAttribute('action', rotaCidade.replace(""_cidade_"", cidade).replace(""_sigla_"", sigla));
            $(""#frmPesquisa"").submit();
            return true;
        }
        $(""#frmPesquisa"").action = ""/"";
        $(""#frmPesquisa"").submit();
    }

    var EventName = 'Concluiu_Novo';
    function submitCadastro() {
        if ($('#mdlCadastro .alert-danger').is("":visible"")) return false;
        $('#mdlCadastro').data('bootstrapValidator').validate(); $('#mdlCadastro').data('bootstrapValidator').disableSubmitButtons(false);
        trackEvent('Acesso', 'PopUp', 'Concluiu');
        return $('#mdlCadastro').data('bootstrapValidator').isValid();
    }

    $(function () {
        //Escondendo menu
        $("".navbar-fixed-top"").autoHidingNavbar({ showOnBottom: false });
        $(""body"").css(""padding-top"", $("".navbar"").outerHeight() + ""px"");
        $(window).resize(function () {
            $(""body"").css(""padding-top"", $("".navbar"").outerHeight() + ""px"");
        });

        var user = readCookieItem(""SINE_INFO"", ""user"");
        var preUser = readCookieItem(""SINE_INFO"", ""preUser"");
        var name = readCookieItem(""SINE_INFO"", ""userName"");

        if (name == """" && (user != """" || preUser != """")) {
            var idDePreUsuario = ""true"";
            var id = preUser;
            if (preUser == """") {
                idDePreUsuario = ""false"";
                id = user;
            }

            $.ajax({
                type: ""POST"",
                url: ""/ajax.aspx/RecuperarPrimeiroNomeUsuario"",
                data: '{ id: ""' + id + '"", preUsuario: ' + idDePreUsuario + '}',
                contentType: ""application/json; charset=utf-8"",
                dataType: ""json"",
                success: function (data) {
                    retorno = jQuery.parseJSON(data.d);
                    if (retorno.nome !== """" && retorno.nome != null) {
                        $(""#divInfoLogin"").show();
                        $(""#nomeLogin"").text(retorno.nome);
                        writeCookieItem(""SINE_INFO"", ""userName"", retorno.nome, 365);
                    }
                },
            });
        }

        if (name == """") {
            $(""#divInfoLogin"").hide();
            $(""#divLogin"").show();
        } else {
            $(""#divInfoLogin"").show();
            $(""#divLogin"").hide();

            if (navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0)
                $(""#nomeLogin"").text(decodeURIComponent(escape(name)));//Internet Explorer
            else
                $(""#nomeLogin"").text(name);
        }

        var cacheFuncoes = {};
        var cacheCidades = {};

        $(""#txtFuncaoTopo"").autocomplete({
            minLength: 2,
            delay: 100,
            open: function (event, ui) {
                var ul = $("".ui-autocomplete"");
                ul.outerWidth($(""#txtFuncaoTopo"").outerWidth());
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $(""#hfIdFuncaoTopo"").val(0);
                } else {
                    $(""#hfIdFuncaoTopo"").val(ui.item.id);
                }
            },
            source: function (request, response) {
                var term = request.term;
                if (term in cacheFuncoes) {
                    response(cacheFuncoes[term]);
                    return;
                }

                $.ajax({
                    type: ""POST"",
                    url: ""/ajax.aspx/ListarFuncoesAC"",
                    data: '{ prefixText: ""' + term + '""}',
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",
                    success: function (data) {
                        cacheFuncoes[term] = jQuery.parseJSON(data.d);
                        for (var i in cacheFuncoes[term]) {
                            cacheFuncoes[term][i].value = cacheFuncoes[term][i].descricao;
                        }
                        response(cacheFuncoes[term]);
                    },
                });
            }
        }).autocomplete(""instance"")._renderItem = function (ul, item) {
            return $(""<li>"")
                  .text(item.descricao)
                  .appendTo(ul);
        };

        $(""#txtCidadeTopo"").autocomplete({
            minLength: 2,
            delay: 100,
            open: function (event, ui) {
                var ul = $("".ui-autocomplete"");
                ul.outerWidth($(""#txtCidadeTopo"").outerWidth());
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $(""#hfIdCidadeTopo"").val(0);
                } else {
                    $(""#hfIdCidadeTopo"").val(ui.item.id);
                }
            },
            source: function (request, response) {
                var term = request.term;
                if (term in cacheCidades) {
                    response(cacheCidades[term]);
                    return;
                }

                $.ajax({
                    type: ""POST"",
                    url: ""/ajax.aspx/ListarCidadesAC"",
                    data: '{ prefixText: ""' + term + '""}',
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",
                    success: function (data) {
                        cacheCidades[term] = jQuery.parseJSON(data.d);
                        for (var i in cacheCidades[term]) {
                            cacheCidades[term][i].value = cacheCidades[term][i].descricao;
                        }
                        response(cacheCidades[term]);
                    },
                });
            }
        }).autocomplete(""instance"")._renderItem = function (ul, item) {
            return $(""<li>"")
                  .text(item.descricao)
                  .appendTo(ul);
        };

        //Mascara Dados de contato
        $('#mdlCadastro').on('show.bs.modal', function (e) {
            trackEvent('Acesso', 'PopUp', 'Abriu');
        })

        $('#mdlCadastro [name=nascimento]').mask('00/00/0000');

        $('#mdlCadastro [name=nascimento]').change(ValidarEmailDataNascimento);
        $('#mdlCadastro [name=email]').change(ValidarEmailDataNascimento);

        //Mascara Dados de Login
        $('#mdlLogin [name=nascimento]').mask('00/00/0000');
        $('#mdlLogin [name=cpf]').mask('000.000.000-00');

        $('#mdlLogin [name=nascimento]').change(ValidarCPFDataNascimento);
        $('#mdlLogin [name=cpf]').change(ValidarCPFDataNascimento);
        $('#mdlLogin [name=nome]').change(ValidarCPFDataNascimento);

        //Validação Dados de Contato
        $('#mdlCadastro').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            live: ""submitted"",
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                email: {
                    validators: {
                        notEmpty: {
                            message: 'O endereço de e-mail não pode estar vazio'
                        },
                        emailAddress: {
                            message: 'O e-mail não é válido'
                        }
                    }
                },
                nascimento: {
                    validators: {
                        notEmpty: {
                            message: 'A data de nascimento deve ser preenchida'
                        },
                        birthday: {
                            format: 'DD/MM/YYYY',
                            message: 'Data de Nascimento inválida'
                        }
                    }
                },
                nome: {
                    validators: {
                        notEmpty: {
                            message: 'O nome deve ser preenchido'
                        },
                        NomeCompleto: {
                            message: 'Preencha seu nome completo, sem abreviações'
                        }
                    }
                }
            }
        }).on('status.field.bv', function (e, data) {
            // $(e.target)  --> The field element
            // data.bv      --> The BootstrapValidator instance
            // data.field   --> The field name
            // data.element --> The field element

            data.bv.disableSubmitButtons(false);
        });

        //Validação Dados de Login
        $('#mdlLogin').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            live: ""submitted"",
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                cpf: {
                    validators: {
                        notEmpty: {
                            message: 'O CPF não pode estar vazio'
                        },
                        cpf: {
                            message: 'O CPF não é válido'
                        }
                    }
                },
                nascimento: {
                    validators: {
                        notEmpty: {
                            message: 'A data de nascimento deve ser preenchida'
                        },
                        birthday: {
                            format: 'DD/MM/YYYY',
                            message: 'Data de Nascimento inválida'
                        }
                    }
                }
            }
        }).on('status.field.bv', function (e, data) {
            // $(e.target)  --> The field element
            // data.bv      --> The BootstrapValidator instance
            // data.field   --> The field name
            // data.element --> The field element

            //data.bv.disableSubmitButtons(false);
        });
    });




</script>

</div><ul class=""ui-autocomplete ui-front ui-menu ui-widget ui-widget-content"" id=""ui-id-1"" tabindex=""0"" style=""display: none;""></ul><span role=""status"" aria-live=""assertive"" aria-relevant=""additions"" class=""ui-helper-hidden-accessible""></span><ul class=""ui-autocomplete ui-front ui-menu ui-widget ui-widget-content"" id=""ui-id-2"" tabindex=""0"" style=""display: none;""></ul><span role=""status"" aria-live=""assertive"" aria-relevant=""additions"" class=""ui-helper-hidden-accessible""></span><iframe name=""oauth2relay411407308"" id=""oauth2relay411407308"" src=""https://accounts.google.com/o/oauth2/postmessageRelay?parent=http%3A%2F%2Fwww.sine.com.br#rpctoken=715017247&amp;forcesecure=1"" tabindex=""-1"" style=""width: 1px; height: 1px; position: absolute; top: -100px;""></iframe></body></html>";
    }
}
