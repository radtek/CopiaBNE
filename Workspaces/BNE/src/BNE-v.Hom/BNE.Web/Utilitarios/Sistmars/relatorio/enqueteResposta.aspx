<%@ Page language="c#" Codebehind="enqueteResposta.aspx.cs" AutoEventWireup="false" Inherits="SistMars.relatorio.enqueteResposta" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>.: SistMars | Relatórios :.</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/SistMars.css" type="text/css" rel=stylesheet>
  </HEAD>
	<body MS_POSITIONING="GridLayout" topmargin=2>
		<form id="enqueteResposta" method="post" runat="server">
			<script src="../include/funcoes.js"></script>
			<table width="100%">
				<TR>
					<TD class=tdTitulo valign=middle> Responder enquete</TD></TR>
				<TR>
					<TD align=right>
						<TABLE class=texto10b cellSpacing=1 cellPadding=1 width="100%" border=0>
							<TR>
								<TD width="50%">Nome: <asp:label id=lblNome runat="server" CssClass="texto10"></asp:label></TD>
								<TD width="50%">Avaliação corresponde: <asp:image id=imgAvaliacao runat="server" BorderWidth="0px" ImageUrl="../images/avaliacao1.jpg"></asp:image></TD></TR>
							<TR>
								<TD>E-Mail: <asp:label id=lblEmail runat="server" CssClass="texto10"></asp:label></TD>
								<TD>Telefone: <asp:label id=lblTelefone runat="server" CssClass="texto10"></asp:label></TD></TR>
							<TR>
								<TD>Comentários da pessoa:</TD>
								<TD>  Cadastro da enquete: <asp:label id=lblDataEnquete runat="server" CssClass="texto10"></asp:label></TD></TR>
							<TR>
								<TD colSpan=2>
									<asp:textbox id=txtObservacao runat="server" Width="100%" ReadOnly="True" Rows="5" TextMode="MultiLine"></asp:textbox>
								</TD>
							</TR>
							<tr><td height=10 colspan=2></td></tr>
							<tr>
								<td colspan=2 class=texto8>
									<asp:Label id=lblExplicacao runat="server" CssClass="texto8">Para responder essa pessoa, digite o texto abaixo e clique em "Enviar resposta":</asp:Label>
									<asp:label id=lblRespondida runat="server" CssClass="texto8" ForeColor="Red" Visible="False">(essa enquete já foi respondida anteriormente)</asp:label>
									<asp:textbox id=txtResposta runat="server" Width="100%" TextMode="MultiLine" Rows="5" ></asp:textbox>
								</td>
							</tr>
							<tr>
								<td colspan=2 align=right>
									<asp:button id=cmdResponder runat="server" Width="110px" CssClass="texto10" Text="Enviar resposta"></asp:button>
									<input type=button name=cmdFechar value=Fechar onclick="window.close();" style="WIDTH:110px">
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
			<script language=javascript>
				enqueteResposta.txtResposta.focus();
			</script>
		</form>
	</body>
</HTML>
