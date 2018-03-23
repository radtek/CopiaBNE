<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlackFriday.aspx.cs" Inherits="BNE.Web.Promocoes.BlackFriday.BlackFriday" %>

<!DOCTYPE html>
<html lang="en">

<head>
   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <meta name="description" content="The Page Description">
    <style type="text/css">
        @-ms-viewport {
            width: device-width;
        }
    </style>
    <title>BNE - Black Friday</title>
    <link rel="stylesheet" href="Promocoes/BlackFriday/css/layers.min.css" media="screen">
    <link rel="stylesheet" href="Promocoes/BlackFriday/css/font-awesome.min.css" media="screen">
    <link rel="stylesheet" href="Promocoes/BlackFriday/style.css" media="screen">
    <link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Open+Sans:400italic,700italic,400,700' rel='stylesheet' type='text/css'>
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
		<![endif]-->
    <link rel="icon" href="Promocoes/BlackFriday/favicon.ico">
    <link rel="apple-touch-icon" href="img/apple-touch-icon.png">
    <link rel="apple-touch-icon" sizes="76x76" href="Promocoes/BlackFriday/img/apple-touch-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="120x120" href="Promocoes/BlackFriday/img/apple-touch-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="152x152" href="Promocoes/BlackFriday/img/apple-touch-icon-152x152.png">
</head>

<body class="page">
  
    <div id="fb-root"></div>
    <script>
    
        (function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/pt_BR/sdk.js#xfbml=1&version=v2.5";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>

       <header role="banner" class="transparent light">
        <div class="row">
            <div class="nav-inner row-content buffer-left buffer-right even clear-after">
                <div id="brand"> 
                    <h1 class="reset">Bne - Banco Nacional de Empregos</h1>
                </div><!-- brand -->
                <a id="menu-toggle" href="#"><i class="fa fa-bars fa-lg"></i></a>
                <nav>
                    <ul class="reset" role="navigation">
                        <li class="menu-item"><a href="http://www.bne.com.br/?utm_source=Hotsite-BF&utm_medium=hotsite&utm_campaign=BlackFriday">Ir para o bne.com.br</a></li>
                    </ul>
                </nav>
            </div>
            <!-- row-content -->
        </div>
        <!-- row -->
    </header>
    <form runat="server">
        <asp:UpdatePanel ID="upPagina" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <main role="main">
            <asp:ScriptManager runat="server" ID="sc"></asp:ScriptManager>
			<div id="intro-wrap">
				<div id="intro" class="preload" data-autoplay="5000" data-navigation="true" data-pagination="true" data-transition="fade">	
					<div class="intro-item" style="background-color: #000;">		
						<div class="intro-mockup-wrapper">		
							<div class="caption-mockup caption-right column six last-special">
								<h2>Somos o 1º Site de Empregos a entrar no Black Friday</h2>
								<p>Vamos enviar os melhores descontos para você conseguir seu novo emprego :)</p>
									
								<a class="button white transparent" href="#main">Cadastre-se Agora</a>
							</div><!-- caption -->
							<div class="intro-mockup intro-left column six">
								<img src="Promocoes/BlackFriday/img/blackfriday.png" alt="">
							</div><!-- intro-mockup -->							
						</div><!-- intro-mockup-wrapper -->
					</div>					
				</div><!-- intro -->
			</div><!-- intro-wrap -->

			<div id="main">
            <section class="row section call-to-action" style="background-color:#000">
					<div class="row-content buffer even">
							<div id="timer" class="timer">
  								<div id="days" class="column three"></div>
  								<div id="hours" class="column three"></div>
 								<div id="minutes" class="column three"></div>
  								<div id="seconds" class="column three last"></div>
							</div>
					</div>
				</section> 
			<section class="row section" style="background-color: #000;">
					<div class="row-content buffer even clear-after" >
						<div class="section-title "><h3>Faça o seu pré-cadastro agora</h3></div>	
						<p class="centertxt">Saiba em primeira mão as oportunidades do Black Friday do BNE <br>e receba as melhores vagas do Brasil no seu e-mail: </p>
						<div class="column two"> </div>
                        <div class="column eight">
                            
						
                           <div id="contact-form" class="contact-section" >
                                <asp:RequiredFieldValidator runat="server" ID="rfvNome" ControlToValidate="txtNome" ErrorMessage="Campo Obrigatório." ValidationGroup="vlgCadastro">
                                </asp:RequiredFieldValidator>

								<span class="pre-input"><i class="icon icon-user"></i></span>
								<asp:TextBox CssClass="name plain buffer" id="txtNome" runat="server" placeholder="Nome Completo" />

                               <asp:RegularExpressionValidator ID="regexEmail"
                                                runat="server"
                                                ErrorMessage="E-mail Inválido."
                                                ControlToValidate="txtEmail"
                                                ValidationGroup="vlgCadastro"
                                                ValidationExpression="(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)">
                                            </asp:RegularExpressionValidator>

                                <asp:RequiredFieldValidator ID="rfvEmail"
                                                runat="server"
                                                ErrorMessage="Campo Obrigatório."
                                                ControlToValidate="txtEmail"
                                                ValidationGroup="vlgCadastro" />
								<span class="pre-input"><i class="icon icon-email"></i></span>
								<asp:TextBox ID="txtEmail" runat="server" CssClass="email plain buffer" placeholder="Email" />
                               <asp:RequiredFieldValidator runat="server" ID="rfvCheck" ErrorMessage="Campo Obrigatório." ValidationGroup="vlgCadastro"
                                        ControlToValidate="rbTipo"></asp:RequiredFieldValidator>
                               <div class="column twelve centertxt">
                                   <asp:RadioButtonList ID="rbTipo" CssClass="column eight teste"  RepeatDirection="Horizontal" runat="server" >
                                       <asp:ListItem Text="<span></span> Sou Candidato" Value="0"></asp:ListItem>
                                       <asp:ListItem Text="<span></span> Sou Empresa" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                   
                                </div>
                                <div class="column twelve centertxt">
                                <asp:LinkButton id="lnkCadastrar" ValidationGroup="vlgCadastro" class="plain button mint " runat="server" OnClick="lnkCadastrar_Click" >Cadastrar</asp:LinkButton> 
                                </div>
                                
                               </div>
							
                              
							<div id="success"></div>
						</div>
                        <div class="column two last"> </div>
					</div>
				</section>	
				
			<section class="row section call-to-action" style="background-color:#000">
					<div class="row-content buffer even ">
							<div class="widget">
								<div class="fb-page" data-href="https://www.facebook.com/BancoNacionalDeEmpregos" data-width="658" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true" data-show-posts="false"><div class="fb-xfbml-parse-ignore"><blockquote cite="https://www.facebook.com/BancoNacionalDeEmpregos"><a href="https://www.facebook.com/BancoNacionalDeEmpregos">Banco Nacional de Empregos</a></blockquote></div></div>
							</div>	
					</div>
				</section>				

			</div><!-- id-main -->
		</main>
                <!-- main -->


                <script src="Promocoes/BlackFriday/js/sweet-alert.js"></script>
                <link href="Promocoes/BlackFriday/css/sweet-alert.css" rel="stylesheet" />
                <script src="https://code.jquery.com/jquery.js"></script>
                <script src="Promocoes/BlackFriday/js/plugins.js"></script>
                <script src="Promocoes/BlackFriday/js/beetle.js"></script>
                <script src="Promocoes/BlackFriday/js/timer.js"></script>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>


</body>

</html>

