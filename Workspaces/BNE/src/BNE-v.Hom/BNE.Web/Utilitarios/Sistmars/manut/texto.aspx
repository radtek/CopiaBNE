<%@ Page language="c#" Codebehind="texto.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut.frmTexto" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>.: SistMars | Manut :.</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../include/SistMars.css" type="text/css">
  </HEAD>
	<body MS_POSITIONING="GridLayout" background="../images/fundos.jpg">
		<form id="frmTexto" method="post" runat="server">
			<img src="../images/pixel.gif" onload="txtDiaMes.focus();">
			<uc1:seguranca id="Seguranca1" runat="server"></uc1:seguranca>
			<uc1:menuManut id="MenuManut1" runat="server"></uc1:menuManut>
			<TABLE cellSpacing="1" cellPadding="1" border="0" class="texto10b" align=center>
				<TR>
					<TD class=tdTitulo valign="middle" colspan="2">Manutenção de Textos</TD>
				</TR>
				<TR>
					<TD colspan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD>Dia&nbsp;e mês:
						<asp:TextBox id="txtDiaMes" runat="server" Width="50px" MaxLength="4" AutoPostBack="True"></asp:TextBox>&nbsp;
						<asp:Button id="cmdBuscar" runat="server" Text="Buscar"></asp:Button><font size="1" style="FONT-WEIGHT: normal">&nbsp;<asp:RegularExpressionValidator id="revDiaMes" runat="server" ErrorMessage="Insira o dia e o mês no formato DDMM" ValidationExpression="\d\d\d\d" Display="Dynamic" ControlToValidate="txtDiaMes"></asp:RegularExpressionValidator><asp:RequiredFieldValidator id="rfvDiaMes" runat="server" ErrorMessage="Informar o dia e o mês" Display="Dynamic" ControlToValidate="txtDiaMes"></asp:RequiredFieldValidator></font>
					</TD>
					<TD align="right">
						<asp:Button id="cmdGravar1" runat="server" Width="100px" Text="Gravar"></asp:Button>
					</TD>
				</TR>
				<TR>
					<TD colspan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class=tdTitulo colspan="2">Pessoal</TD>
				</TR>
				<TR>
					<TD colspan="2"><asp:TextBox id="txtPessoal" runat="server" Rows="6" TextMode="MultiLine" Columns="70">-| n&#227;o dispon&#237;vel |-</asp:TextBox></TD>
				</TR>
				<TR>
					<TD colspan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class=tdTitulo colspan="2">Personalidade</TD>
				</TR>
				<TR>
					<TD colspan="2"><asp:TextBox id="txtPersonalidade" runat="server" Rows="20" TextMode="MultiLine" Columns="70">-| n&#227;o dispon&#237;vel |-</asp:TextBox></TD>
				</TR>
				<TR>
					<TD colspan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD align="right" colspan="2"><asp:Button id="cmdGravar2" runat="server" Width="100px" Text="Gravar"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
