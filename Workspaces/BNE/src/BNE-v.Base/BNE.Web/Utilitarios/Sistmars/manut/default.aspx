<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut._default" %>
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
	<body MS_POSITIONING="GridLayout" onload="frmLogin.txtUsuario.focus();" background="../images/fundos.jpg">
		<form id="frmLogin" method="post" runat="server">
			<TABLE align=center cellSpacing="1" cellPadding="1" border="0" class=texto10>
				<TR>
					<TD class=tdTitulo valign=middle colspan=2><b>Manutenção do SistMars</b></TD>
				</TR>
				<TR>
					<TD colspan=2>&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan=2>
						Bem-vindo ao Manut do SistMars.<BR>
						Favor informar usuário e senha para acesso.
					</TD>
				</TR>
				<TR>
					<TD colspan=2>&nbsp;</TD>
				</TR>
				<TR>
					<TD><b>Usuário:</b></TD>
					<TD><asp:TextBox id=txtUsuario runat="server" Width="120px" MaxLength="20"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD><b>Senha:</b></TD>
					<TD><asp:TextBox id=txtSenha runat="server" Width="120px" MaxLength="20" TextMode="Password"></asp:TextBox>&nbsp;<asp:Button id=cmdOK runat="server" Width="80px" Text="OK"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
