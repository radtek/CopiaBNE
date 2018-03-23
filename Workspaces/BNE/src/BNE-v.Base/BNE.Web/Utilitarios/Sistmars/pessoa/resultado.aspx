<%@ Page language="c#" Codebehind="resultado.aspx.cs" AutoEventWireup="false" Inherits="SistMars.pessoa.resultado" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>.: SistMars :.</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<LINK href="../include/SistMars.css" type="text/css" rel="stylesheet">
			<script language="javascript" src="../include/funcoes.js"></script>
	</HEAD>
	<body background="../images/fundos.jpg" MS_POSITIONING="GridLayout">
		<TABLE height="1056" cellSpacing="0" cellPadding="0" width="652" border="0" ms_2d_layout="TRUE">
			<TBODY>
				<TR vAlign="top">
					<TD width="0" height="15"></TD>
					<TD colSpan="2" rowSpan="2">
						<form id="frmResultado" method="post" runat="server">
							<TABLE height="973" cellSpacing="0" cellPadding="0" width="652" border="0" ms_2d_layout="TRUE">
								<TR vAlign="top">
									<TD width="1" height="15"></TD>
									<TD width="651"></TD>
								</TR>
								<TR vAlign="top">
									<TD height="958"></TD>
									<TD>
										<table height="957" width="650" align="center" border="0">
											<TBODY>
												<tr>
													<td align="center"><IMG src="../images/logosistopo.gif" border="0"><br>
														<br>
													</td>
												</tr>
												<tr>
													<td>
														<table cellSpacing="1" cellPadding="0" width="100%" border="0">
															<TR>
																<TD><IMG src="../images/delineamento.gif">
																	<BR>
																	<IMG src="../images/avaliacao.gif">
																</TD>
																<TD align="right"><IMG src="../images/direitos_autorais.gif" border="0">
																</TD>
															</TR>
														</table>
														<br>
													</td>
												</tr>
												<tr>
													<td class="texto8"><A onclick="AbrirPopup('resultadoImpressao.aspx', 500, 700);" href="#"><IMG src="../images/print.gif" border="0">
															Versão para impressão </A>
													</td>
												</tr>
												<TR>
													<TD class="texto8">&nbsp;
														<asp:textbox id="txtCodAnalise" runat="server" Height="14px" Visible="False"></asp:textbox></TD>
												</TR>
												<TR>
													<TD class="texto8">Enviar por E-mail</TD>
												</TR>
												<TR>
													<TD class="texto8"><asp:textbox id="txtEmail"  runat="server"></asp:textbox><asp:button id="btnEnviarEmail" OnClientClick="" runat="server" Text="Enviar"></asp:button></TD>
												</TR>
												<tr>
													<td>&nbsp;</td>
												</tr>
												<TR>
													<TD><IMG src="../images/identficapessoal.gif">
													</TD>
												</TR>
												<TR>
													<TD>
														<TABLE class="texto10" cellSpacing="1" cellPadding="1" width="100%" border="0">
															<TR>
																<TD><STRONG><asp:label id="lblNome" runat="server" Font-Names="Verdana" Font-Size="10pt"></asp:label></STRONG></TD>
															</TR>
															<TR>
																<TD height="10"></TD>
															</TR>
															<TR>
																<TD><STRONG>Data de Nascimento:</STRONG>&nbsp;
																	<asp:label id="lblDataNascimento" runat="server" Font-Names="Verdana" Font-Size="10pt"></asp:label></TD>
															</TR>
														</TABLE>
														<br>
													</TD>
												</TR>
												<TR>
													<TD class="justificar"><IMG src="../images/caracteristicaspersonalidade.gif"><BR>
														<BR>
														<asp:label id="lblPersonalidade" runat="server" Font-Names="Verdana" Font-Size="10pt"></asp:label>
														<P>&nbsp;</P>
													</TD>
												</TR>
												<TR id="trCaracteristica" runat="server">
													<TD class="justificar">
														<P><IMG src="../images/espelhopersonalidade.gif"><BR>
															<BR>
															<asp:label id="lblCaracteristica" runat="server" Font-Names="verdana" Font-Size="10pt"></asp:label></P>
														<P>&nbsp;</P>
													</TD>
												</TR>
												<TR>
													<TD class="justificar">
														<P><IMG src="../images/seuestagioatual.gif"><BR>
															<BR>
															<asp:label id="lblEstagioAtual" runat="server" Font-Names="verdana" Font-Size="10pt"></asp:label></P>
														<P>&nbsp;</P>
													</TD>
												</TR>
												<TR>
													<TD class="justificar">
														<P><IMG src="../images/recomendacoespessoais.gif">&nbsp;
															<BR>
															<BR>
															<asp:label id="lblPessoal" runat="server" Font-Names="verdana" Font-Size="10pt"></asp:label></P>
													</TD>
												</TR>
												<TR>
													<TD align="right"><asp:imagebutton id="imgContinuar" runat="server" ImageUrl="../images/continuar.gif"></asp:imagebutton></TD>
												</TR>
											</TBODY></table>
									</TD>
								</TR>
							</TABLE>
						</form>
					</TD>
				</TR>
				<TR vAlign="top">
					<TD width="0" height="1041"></TD>
					<TD>
					
		</TD></TR></TBODY></TABLE>
	</body>
</HTML>
