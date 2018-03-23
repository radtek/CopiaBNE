<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
     CodeBehind="ReimprimirBoleto.aspx.cs" Inherits="BNE.Web.ReimprimirBoleto" %>
<%@ Register TagName="ucConfirmacao" Src="~/UserControls/Modais/ModalConfirmacao.ascx" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
<Employer:DynamicHtmlLink runat="server" href="css/local/ReimprimirBoleto.css" type="text/css" rel="stylesheet" />
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">

    <div class="noPrint" style="width:883px; margin:0 auto;">
		<!-- Utilizar somente a Section abaixo - - - - - - - - - - - - - - - - - - - -->
		<section id="reimprimir_boleto">
			<div id="reimprimir_boleto_icon">
				<svg style="width:112px;height:112px" viewBox="0 0 24 24">
	                <path fill="#ffffff" d="M6,2H18V8H18V8L14,12L18,16V16H18V22H6V16H6V16L10,12L6,8V8H6V2M16,16.5L12,12.5L8,16.5V20H16V16.5M12,11.5L16,7.5V4H8V7.5L12,11.5M10,6H14V6.75L12,8.75L10,6.75V6Z" />
	            </svg>
			</div>
            <asp:HiddenField runat="server" ID="hdfBoleto" />
          
			<h1>Falta pouco!</h1>
       		<h4 >
                Seu plano será liberado assim que identificarmos o pagamento.
            </h4>
            <div id="reimprimir_boleto_options">
				 	<a class="btn btn-success" OnClick="window.print();" >Imprimir boleto</a>
				 <asp:Button runat="server" ID="btnEnviarPorEmail" OnClick="btnEnviarPorEmail_Click" class="btn btn-success" Text="Enviar boleto por email" />
                 <asp:Button runat="server" ID="btnVoltar" OnClick="btnVoltar_Click" class="btn btn-default" Text="Voltar" />

            </div>
        </section>

	</div>
    <br />
    <br />
    <br />
      <div class="printableBoleto" >
                <asp:Image runat="server" ID="imgBoleto" />
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc:ucConfirmacao runat="server" ID="ucConfirmacao" />
</asp:Content>
