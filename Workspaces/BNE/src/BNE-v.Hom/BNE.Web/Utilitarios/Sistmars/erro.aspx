<%@ Page language="c#" Codebehind="erro.aspx.cs" AutoEventWireup="false" Inherits="SistMars.erro" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>.: SistMars :.</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" href="include/SistMars.css" type="text/css">
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout" MARGINWIDTH="0" MARGINHEIGHT="0" BACKGROUND="images/fundos.jpg">
		<br>
		<form id="frmErro" runat="server">
			<table cellSpacing="5" cellPadding="5" width="400" align="center" border="1">
				<tr>
					<td class=tdTitulo>
						<div align="center"><b><font size="2">Mensagens do&nbsp;Sistema</font></b></div>
					</td>
				</tr>
				<tr>
					<td>
						<P><asp:textbox id="txtErro" runat="server" ReadOnly="True" TextMode="MultiLine" Height="104px" Width="100%" ForeColor="Black"></asp:textbox></P>
					</td>
				</tr>
				<tr>
					<td align="right">
						<asp:Button id="cmdVoltar" runat="server" Text="Voltar" style="CURSOR:hand"></asp:Button>
						<input type="button" id="cmdFechar" runat="server" onclick="javascript:window.close();" value="Fechar" style="CURSOR:hand">
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
