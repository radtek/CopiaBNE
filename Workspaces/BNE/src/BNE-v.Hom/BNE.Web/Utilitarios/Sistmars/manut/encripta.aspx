<%@ Page language="c#" Codebehind="encripta.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut.encripta" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>.: SistMars | Manut :.</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../include/SistMars.css" type="text/css">
  </HEAD>
	<body MS_POSITIONING="GridLayout" background="../images/fundos.jpg">
		<form id="encripta" method="post" runat="server">
			<TABLE BORDER="0" CELLSPACING="1" CELLPADDING="1" align=center>
				<TR>
					<TD valign=middle class=tdTitulo>Encriptador de Senha</TD>
				</TR>
				<TR><TD>&nbsp;</TD></TR>
				<TR>
					<TD nowrap="nowrap">
						<asp:TextBox id=TextBox1 runat="server" Width="250px"></asp:TextBox>&nbsp;
						<asp:Button id=cmdEncriptar runat="server" Text="Encriptar" Width="87px"></asp:Button>
					</TD>
				</TR>
				<TR>
					<TD>
						<asp:TextBox id=TextBox2 runat="server" Width="350px"></asp:TextBox>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
