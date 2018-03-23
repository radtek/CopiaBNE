<%@ Page language="c#" Codebehind="enquetes.aspx.cs" AutoEventWireup="false" Inherits="SistMars.relatorio.enquetes" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>.: SistMars | Relatórios :.</title>
		<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
		<meta content=C# name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name=vs_targetSchema><LINK href="../include/SistMars.css" type="text/css" rel=stylesheet>
  </HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id=enquetes method=post runat="server"><uc1:seguranca id=Seguranca1 runat="server"></uc1:seguranca><uc1:menumanut id=MenuManut1 runat="server"></uc1:menumanut>
			<script src="../include/funcoes.js"></script>
			<script language=javascript>
				function responder(codigoEnquete)
				{
					//para quando recarregar a pagina, voltar na pagina certa
					var pageIndex = enquetes.txtPageIndex.value;
					if(pageIndex=='')
						pageIndex = 0;

					//cria link que sera aberto.. tem que passar codigo da enquete para responder;
					//data inicial, data final e pagina do datagrid para refazer a pesquisa depois.
					var link = "enqueteResposta.aspx?ENQ_CodigoEnquete=" + codigoEnquete.toString();
					link += "&dataInicial=" + enquetes.txtDataInicial.value
					link += "&dataFinal=" + enquetes.txtDataFinal.value
					link += "&pageIndex=" + pageIndex.toString();

					//abre a pagina
					AbrirPopup(link, 330, 550);
				}
			</script>
			<input type=hidden id=txtPageIndex name=txtPageIndex runat="server">
			<TABLE cellSpacing=1 cellPadding=1 width=750 align=center border=0>
				<TR>
					<TD class=tdTitulo align="center">Relatório de
						Enquetes</TD></TR>
				<TR>
					<TD>&nbsp;</TD></TR>
				<TR>
					<TD class=texto10>Data inicial: <asp:textbox id=txtDataInicial runat="server" Width="100px" MaxLength="10"></asp:textbox>&nbsp;Data
						final: <asp:textbox id=txtDataFinal runat="server" Width="100px" MaxLength="10"></asp:textbox>&nbsp;<asp:button id=cmdPesquisar runat="server" Width="110px" CssClass="texto10" Text="Pesquisar"></asp:button>&nbsp;<asp:comparevalidator id=cvDataInicial runat="server" CssClass="texto8" ErrorMessage="Data inicial inválida!" Operator="DataTypeCheck" Type="Date" ControlToValidate="txtDataInicial" Display="Dynamic"></asp:comparevalidator>&nbsp;<asp:comparevalidator id=cvDataFinal runat="server" CssClass="texto8" ErrorMessage="Data final inválida!" Operator="DataTypeCheck" Type="Date" ControlToValidate="txtDataFinal" Display="Dynamic"></asp:comparevalidator>
					</TD></TR>
				<TR>
					<TD>&nbsp;</TD></TR>
				<TR>
					<TD>
						<TABLE id=Table1 cellSpacing=1 cellPadding=1 width=300 border=0>
							<TR>
								<TD class=texto10b width=170>Total de respostas:</TD>
								<TD><asp:label id=lblTotal runat="server" CssClass="texto10">0</asp:label></TD>
								<TD></TD></TR>
							<TR>
								<TD class=texto10b>Positivas:</TD>
								<TD class=texto10><asp:label id=lblPositivas runat="server" CssClass="texto10">0</asp:label></TD>
								<TD class=texto10>(<asp:label id=lblPositivasPerc runat="server" CssClass="texto10">0</asp:label>&nbsp;%)</TD></TR>
							<TR>
								<TD class=texto10b>Parciais:</TD>
								<TD class=texto10><asp:label id=lblParciais runat="server" CssClass="texto10">0</asp:label></TD>
								<TD class=texto10>(<asp:label id=lblParciaisPerc runat="server" CssClass="texto10">0</asp:label>&nbsp;%)</TD></TR>
							<TR>
								<TD class=texto10b>Negativas:</TD>
								<TD class=texto10><asp:label id=lblNegativas runat="server" CssClass="texto10">0</asp:label></TD>
								<TD class=texto10>(<asp:label id=lblNegativasPerc runat="server" CssClass="texto10">0</asp:label>&nbsp;%)</TD></TR></TABLE></TD></TR>
				<TR>
					<TD>
						<asp:DataGrid id=dtgDetalhes runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" PageSize="20">
							<ItemStyle CssClass="texto10">
							</ItemStyle>

							<HeaderStyle CssClass="tdTitulo">
							</HeaderStyle>

							<Columns>
								<asp:BoundColumn Visible="False" DataField="ENQ_CodigoEnquete" HeaderText="ENQ_CodigoEnquete"></asp:BoundColumn>
								<asp:BoundColumn DataField="PES_Nome" HeaderText="Nome">
									<HeaderStyle Width="40%">
									</HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ENQ_DataCadastro" HeaderText="Data Enq.">
									<HeaderStyle HorizontalAlign="Center" Width="15%">
									</HeaderStyle>

									<ItemStyle HorizontalAlign="Center">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ENQ_Corresponde" HeaderText="Avalia&#231;&#227;o">
									<HeaderStyle HorizontalAlign="Center" Width="15%">
									</HeaderStyle>

									<ItemStyle HorizontalAlign="Center">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ENQ_Observacao" HeaderText="Obs.">
									<HeaderStyle HorizontalAlign="Center" Width="15%">
									</HeaderStyle>

									<ItemStyle HorizontalAlign="Center">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RES_Resposta" HeaderText="Resposta">
									<HeaderStyle HorizontalAlign="Center" Width="15%">
									</HeaderStyle>

									<ItemStyle HorizontalAlign="Center">
									</ItemStyle>
								</asp:BoundColumn>
							</Columns>

							<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" PageButtonCount="20" CssClass="tdTitulo" Mode="NumericPages">
							</PagerStyle>
						</asp:DataGrid></TD></TR></TABLE>
			<IMG src="../images/pixel.gif" onload=txtDataInicial.focus();> </form>
	</body>
</HTML>
