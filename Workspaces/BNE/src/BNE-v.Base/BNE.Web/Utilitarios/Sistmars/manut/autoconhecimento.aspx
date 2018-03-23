<%@ Page language="c#" Codebehind="autoconhecimento.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut.frmAutoConhecimento" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>.: SistMars | Manut :.</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK href="../include/SistMars.css" type="text/css" rel=stylesheet>
  </HEAD>
  <body MS_POSITIONING="GridLayout" background="../images/fundos.jpg">
	<form id="frmAutoConhecimento" method="post" runat="server">
		<uc1:seguranca id=Seguranca1 runat="server"></uc1:seguranca>
		<uc1:menumanut id=MenuManut1 runat="server"></uc1:menumanut>
		<TABLE cellSpacing=1 cellPadding=1 border=0 align=center>
			<TR>
				<TD nowrap="nowrap" class=tdTitulo align="center">Manutenção de Textos de Auto-Conhecimento</TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD nowrap="nowrap" class=texto10>Faixa de pontuação: <asp:DropDownList id=cboPontuacao runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD><asp:TextBox id=txtTexto runat="server" Width="500px" TextMode="MultiLine" Rows="10" Enabled="False">-| n&#227;o dispon&#237;vel |-</asp:TextBox></TD>
			</TR>
			<TR>
				<TD align=right><asp:Button id=cmdGravar runat="server" Text="Gravar" Width="100px"></asp:Button></TD>
			</TR>
		</TABLE>
	</form>
  </body>
</HTML>
