<%@ Page language="c#" Codebehind="analises.aspx.cs" AutoEventWireup="false" Inherits="SistMars.relatorio.analises" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>.: SistMars | Relatórios :.</title>
		<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
		<meta content=C# name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name=vs_targetSchema>
		<LINK href="../include/SistMars.css" type="text/css" rel=stylesheet>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id=analises method=post runat="server">
		<IMG src="../images/pixel.gif" onload=txtPesquisa.focus();>
		<uc1:seguranca id=Seguranca1 runat="server"></uc1:seguranca>
		<uc1:menumanut id=MenuManut1 runat="server"></uc1:menumanut>
			<TABLE cellSpacing=1 cellPadding=1 width=780 border=0 align=center>
				<TR>
					<TD class=tdTitulo valign=middle colSpan=2>Resultados de Análises</TD></TR>
				<TR>
					<TD colSpan=2>&nbsp;</TD></TR>
				<TR>
					<TD width="30%"><asp:radiobuttonlist id=optPesquisa runat="server" RepeatDirection="Horizontal" CssClass="texto10">
							<asp:ListItem Value="Nome" Selected="True">Nome</asp:ListItem>
							<asp:ListItem Value="CPF">CPF</asp:ListItem>
							<asp:ListItem Value="Email">Email</asp:ListItem>
						</asp:radiobuttonlist></TD>
					<TD width="70%"><asp:textbox id=txtPesquisa runat="server" MaxLength="100"></asp:textbox>&nbsp;<asp:button id=cmdPesquisar runat="server" Text="Pesquisar"></asp:button>&nbsp;<asp:requiredfieldvalidator id=rfvPesquisa runat="server" CssClass="texto8" ErrorMessage="Informe a pesquisa" ControlToValidate="txtPesquisa"></asp:requiredfieldvalidator></TD></TR>
				<TR></TR>
				<TR>
					<TD colSpan=2>&nbsp;</TD></TR>
				<TR>
					<TD valign=middle colSpan=2>
						<P>
							<asp:DataGrid id=dtgPesquisa runat="server" AutoGenerateColumns="False" Width="770px" Visible="False" BorderStyle="None" CellPadding="1" CellSpacing="2" BorderWidth="0px">
								<ItemStyle CssClass="texto10">
								</ItemStyle>

								<HeaderStyle CssClass="tdTitulo">
								</HeaderStyle>

								<Columns>
									<asp:BoundColumn DataField="PES_Nome" HeaderText="Nome"></asp:BoundColumn>
									<asp:BoundColumn DataField="PES_DataNascimento" HeaderText="Data Nasc."></asp:BoundColumn>
									<asp:BoundColumn DataField="PES_Email" HeaderText="E-Mail"></asp:BoundColumn>
									<asp:BoundColumn DataField="PES_Telefone" HeaderText="Telefone"></asp:BoundColumn>
									<asp:BoundColumn DataField="PES_Cidade" HeaderText="Cidade/UF"></asp:BoundColumn>
									<asp:BoundColumn DataField="ANA_VerAnalise" HeaderText="Ver">
										<ItemStyle HorizontalAlign="Center">
										</ItemStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:DataGrid><asp:Label id=lblNenhum runat="server" CssClass="texto10b" Visible="False">Nenhum registro encontrado.</asp:Label></P></TD></TR></TABLE></form>
	</body>
</HTML>
