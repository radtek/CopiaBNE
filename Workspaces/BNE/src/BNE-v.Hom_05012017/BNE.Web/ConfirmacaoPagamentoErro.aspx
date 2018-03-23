<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="ConfirmacaoPagamentoErro.aspx.cs"
    Inherits="BNE.Web.ConfirmacaoPagamentoErro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <link href="css/local/ConfirmacaoPagamento2.css" rel="stylesheet" />
    <link href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet"/>
	<link href='http://fonts.googleapis.com/css?family=Roboto:400,400italic,300italic,500,500italic,700,700italic,900,300,100italic' rel='stylesheet' type='text/css'>
	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <br>
    <div class="bredcrumb" runat="server" id="divBreadCrumb">
		<span class="tit_bdc">Passos para ser VIP:</span>
		<span class="txt_bdc">1 - Escolha o Plano</span>
		<span class="txt_bdc">/</span>
		<span class="txt_bdc">2 - Formas de Pagamento</span>
		<span class="txt_bdc">/</span>
		<span class="txt_bdc"><strong><u>3 - Confirmação</u></strong></span>
		<span class="txt_bdc"><strong><u>-</u></strong></span>
		<span class="txt_bdc"><u>Ocorreu um erro no pagamento.</u></span>
	</div>
	<div class="tela_pag">
		<br><br><br>
		<i class="fa fa-meh-o"></i> 
		<p class="tela_pag">Pagamento não deu certo!</p>
		<h2 class="msgerror">Oops! </h2>
		<p class="tela_pag">Entre em contato com seu banco, <Br>ou fale conosco pelo <strong>0800 41 2400</strong></p>
		
		<a href="javascript:history.go(-1);" class="btn_vervagas">Voltar</a> <Br><Br>
		<a href="Default.aspx" class="btn_irhome">Ir para Home</a>  
	</div>


</asp:Content>