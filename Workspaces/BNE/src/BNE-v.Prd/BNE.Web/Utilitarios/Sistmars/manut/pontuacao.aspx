<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Page language="c#" Codebehind="pontuacao.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut.pontuacao" %>
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
		<form id="pontuacao" method="post" runat="server">
			<img src="../images/pixel.gif" onload=txtBaixaMin.focus();>
			<uc1:seguranca id=Seguranca1 runat="server"></uc1:seguranca>
			<uc1:menumanut id=MenuManut1 runat="server"></uc1:menumanut>
			<TABLE cellSpacing=1 cellPadding=1 width=270 border=0 class="texto10" align=center>
				<TR>
					<TD class=tdTitulo colSpan=2 valign=middle><B>Manutenção de Pontuação</B></TD></TR>
				<TR>
					<TD colSpan=2>&nbsp;</TD></TR>
				<TR>
					<TD width="50%">Pontuação Baixa:</TD>
					<TD align=right>
						<asp:TextBox id=txtBaixaMin runat="server" Width="50px" MaxLength="2"></asp:TextBox>
						a <asp:TextBox id=txtBaixaMax runat="server" Width="50px"></asp:TextBox></TD></TR>
				<TR>
					<TD>Pontuação Média:</TD>
					<TD align=right><asp:TextBox id=txtMediaMin runat="server" Width="50px" MaxLength="2"></asp:TextBox>
					a <asp:TextBox id=txtMediaMax runat="server" Width="50px"></asp:TextBox></TD></TR>
				<TR>
					<TD>Pontuação Alta:</TD>
					<TD align=right><asp:TextBox id=txtAltaMin runat="server" Width="50px" MaxLength="2"></asp:TextBox>
					a <asp:TextBox id=txtAltaMax runat="server" Width="50px"></asp:TextBox></TD></TR>
				<TR>
					<TD colSpan=2>&nbsp;</TD></TR>
				<TR align=right>
					<TD colSpan=2><asp:Button id=cmdGravar runat="server" Text="Gravar" Width="100px"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
