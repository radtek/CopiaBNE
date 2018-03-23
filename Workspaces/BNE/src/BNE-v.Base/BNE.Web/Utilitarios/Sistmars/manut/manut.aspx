<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Page language="c#" Codebehind="manut.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut.manut" %>
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
		<form id="manut" method="post" runat="server">
			<uc1:seguranca id="Seguranca1" runat="server"></uc1:seguranca>
			<uc1:menuManut id="MenuManut1" runat="server"></uc1:menuManut>
			<TABLE class="texto10" cellSpacing="1" cellPadding="1" align="center" border="0">
				<TR>
					<TD class=tdTitulo valign="middle" colSpan="2"><B>Manutenção do SistMars</B></TD>
				</TR>
				<TR>
					<TD colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="2">
						Bem-vindo ao Manut do SistMars.<BR>
						Escolha uma opção no menu acima para continuar.
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
