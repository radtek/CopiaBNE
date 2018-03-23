<%@ Page language="c#" Codebehind="dadosPessoais.aspx.cs" AutoEventWireup="false" Inherits="SistMars.dadosPessoais" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>.: SistMars :.</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/SistMars.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body background="../images/fundos.jpg" onload="dadosPessoais.txtCPF.focus();" MS_POSITIONING="GridLayout">
		<form id="dadosPessoais" method="post" runat="server">
			<script src="../include/funcoes.js"></script>
			<TABLE width="650" align="center" border="0">
				<TR>
					<TD align="center" colSpan="2"><IMG src="../images/logosistopo.gif" border="0"><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD><IMG src="../images/avaliacao.gif">
					</TD>
					<TD align="right"><IMG src="../images/direitos_autorais.gif" border="0">
					</TD>
				</TR>
				<TR>
					<TD colSpan="2"><br>
						<IMG src="../images/delineamento.gif">
					</TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE width="100%" border="0">
							<TR>
								<TD>
									<TABLE cellSpacing="2" cellPadding="2" width="100%" bgColor="#dcf0ff" border="0">
										<TR vAlign="middle">
											<TD  nowrap="nowrap" colSpan="2"><FONT face="Verdana, Arial, Helvetica, sans-serif" color="#3d4088" size="2"><B><FONT color="#333399">Dados 
															Pessoais</FONT></B></FONT></TD>
											<TD  nowrap="nowrap"></TD>
											<TD  nowrap="nowrap"></TD>
										</TR>
										<TR>
											<TD  nowrap="nowrap" align="right"><FONT face="Verdana" size="2">CPF:</FONT></TD>
											<TD colSpan="3"><asp:textbox id="txtCPF" runat="server" Width="150px" MaxLength="11" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvCPF" runat="server" ErrorMessage="Favor informar o CPF" Font-Names="Verdana"
													Font-Size="8pt" ControlToValidate="txtCPF" Display="Dynamic">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revCPF" runat="server" ErrorMessage="CPF inválido (use somente números)" Font-Names="verdana"
													Font-Size="8pt" ControlToValidate="txtCPF" Display="Dynamic" ValidationExpression="\d\d\d\d\d\d\d\d\d\d\d">*</asp:regularexpressionvalidator>&nbsp;<asp:button id="cmdBuscar" runat="server" CssClass="tx1" Text="Buscar Dados" CausesValidation="False"></asp:button>&nbsp;<font face="verdana" size="1">(<u><span style="CURSOR: hand" onclick="AbrirPopup('dicaCPF.htm', 180, 300);">o 
															que é isso?</span></u>)</font>
											</TD>
										</TR>
										<TR vAlign="middle">
											<TD  nowrap="nowrap" align="right"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2">Nome:</FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2"><asp:textbox id="txtNome" runat="server" Width="250px" MaxLength="100" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvNome" runat="server" ErrorMessage="Favor informar o Nome" Font-Names="Verdana"
														Font-Size="8pt" ControlToValidate="txtNome" Display="Dynamic">*</asp:requiredfieldvalidator></FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2">Data de 
													nascimento:</FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2"><asp:textbox id="txtDataNascimento" onkeyup="dt_onkeyup(this);" runat="server" Width="80px" MaxLength="10"
														CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvDataNascimento" runat="server" ErrorMessage="Favor informar a Data de Nascimento"
														Font-Names="Verdana" Font-Size="8pt" ControlToValidate="txtDataNascimento" Display="Dynamic">*</asp:requiredfieldvalidator><asp:comparevalidator id="cvDataNascimento" runat="server" ErrorMessage="Data de Nascimento inválida (DD/MM/AAAA)"
														Font-Names="verdana" Font-Size="8pt" ControlToValidate="txtDataNascimento" Display="Dynamic" Operator="DataTypeCheck" Type="Date">*</asp:comparevalidator></FONT></TD>
										</TR>
										<TR vAlign="middle">
											<TD  nowrap="nowrap" align="right"><FONT face="Verdana" size="2">Cidade:</FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2"><asp:textbox id="txtCidade" runat="server" Width="150px" MaxLength="50" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvCidade" runat="server" ErrorMessage="Favor informar a Cidade" Font-Names="Verdana"
														Font-Size="8pt" ControlToValidate="txtCidade" Display="Dynamic">*</asp:requiredfieldvalidator></FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana" size="2">Estado:</FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana" size="2"></FONT><asp:dropdownlist id="cboEstado" runat="server" Width="50px" CssClass="caixa1">
													<asp:ListItem Value="-1">&#160;</asp:ListItem>
													<asp:ListItem Value="1">AC</asp:ListItem>
													<asp:ListItem Value="2">AL</asp:ListItem>
													<asp:ListItem Value="3">AM</asp:ListItem>
													<asp:ListItem Value="4">AP</asp:ListItem>
													<asp:ListItem Value="5">BA</asp:ListItem>
													<asp:ListItem Value="6">CE</asp:ListItem>
													<asp:ListItem Value="7">DF</asp:ListItem>
													<asp:ListItem Value="8">ES</asp:ListItem>
													<asp:ListItem Value="9">GO</asp:ListItem>
													<asp:ListItem Value="10">MA</asp:ListItem>
													<asp:ListItem Value="11">MG</asp:ListItem>
													<asp:ListItem Value="12">MS</asp:ListItem>
													<asp:ListItem Value="13">MT</asp:ListItem>
													<asp:ListItem Value="14">PA</asp:ListItem>
													<asp:ListItem Value="15">PB</asp:ListItem>
													<asp:ListItem Value="16">PE</asp:ListItem>
													<asp:ListItem Value="17">PI</asp:ListItem>
													<asp:ListItem Value="18">PR</asp:ListItem>
													<asp:ListItem Value="19">RJ</asp:ListItem>
													<asp:ListItem Value="20">RN</asp:ListItem>
													<asp:ListItem Value="21">RO</asp:ListItem>
													<asp:ListItem Value="22">RR</asp:ListItem>
													<asp:ListItem Value="23">RS</asp:ListItem>
													<asp:ListItem Value="24">SC</asp:ListItem>
													<asp:ListItem Value="25">SE</asp:ListItem>
													<asp:ListItem Value="26">SP</asp:ListItem>
													<asp:ListItem Value="27">TO</asp:ListItem>
												</asp:dropdownlist><asp:rangevalidator id="rvEstado" runat="server" ErrorMessage="Favor informar o Estado" Font-Names="verdana"
													Font-Size="8pt" ControlToValidate="cboEstado" Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="100">*</asp:rangevalidator></TD>
										</TR>
										<TR vAlign="middle">
											<TD  nowrap="nowrap" align="right"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2">CEP:</FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2"><asp:textbox id="txtCEP" runat="server" Width="100px" MaxLength="8" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvCEP" runat="server" ErrorMessage="Favor informar o CEP" Font-Names="Verdana"
														Font-Size="8pt" ControlToValidate="txtCEP" Display="Dynamic">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revCEP" runat="server" ErrorMessage="CEP inválido (use somente números)" Font-Names="verdana"
														Font-Size="8pt" ControlToValidate="txtCEP" Display="Dynamic" ValidationExpression="\d\d\d\d\d\d\d\d">*</asp:regularexpressionvalidator></FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana" size="2">Telefone:</FONT></TD>
											<TD  nowrap="nowrap"><asp:textbox id="txtTelefoneDDD" runat="server" Width="30px" MaxLength="2" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvDDD" runat="server" ErrorMessage="Favor informar o DDD" Font-Names="Verdana"
													Font-Size="8pt" ControlToValidate="txtTelefoneDDD" Display="Dynamic">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revDDD" runat="server" ErrorMessage="DDD inválido (use somente números)" Font-Names="verdana"
													Font-Size="8pt" ControlToValidate="txtTelefoneDDD" Display="Dynamic" ValidationExpression="\d\d">*</asp:regularexpressionvalidator><FONT face="Verdana" size="2">&nbsp;
												</FONT>
												<asp:textbox id="txtTelefone" runat="server" Width="100px" MaxLength="8" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvTelefone" runat="server" ErrorMessage="Favor informar o Telefone" Font-Names="Verdana"
													Font-Size="8pt" ControlToValidate="txtTelefone"   ValidationGroup="ValidarTelefone"  Display="Dynamic">*</asp:requiredfieldvalidator>
                                                <asp:regularexpressionvalidator id="revTelefone" 
                                                    runat="server" ErrorMessage="Telefone inválido (use somente números)"
													Font-Names="verdana" Font-Size="8pt" 
                                                    ControlToValidate="txtTelefone" 
                                                    Display="Dynamic" 
                                                    ValidationGroup="ValidarTelefone"
                                                    
                                                    ValidationExpression="[2-9]{1}\d{3}\-{0,1}\d{4}$">*</asp:regularexpressionvalidator></TD>
										</TR>
										<TR vAlign="middle">
											<TD  nowrap="nowrap" align="right"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2">E-mail:</FONT></TD>
											<TD  nowrap="nowrap"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="2"><asp:textbox id="txtEmail" runat="server" Width="250px" MaxLength="50" CssClass="caixa1"></asp:textbox><asp:requiredfieldvalidator id="rfvEmail" runat="server" ErrorMessage="Favor informar o E-mail" Font-Names="Verdana"
														Font-Size="8pt" ControlToValidate="txtEmail" Display="Dynamic">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revEmail" runat="server" ErrorMessage="E-mail inválido" Font-Names="verdana"
														Font-Size="8pt" ControlToValidate="txtEmail" Display="Dynamic" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:regularexpressionvalidator></FONT></TD>
											<TD  nowrap="nowrap"></TD>
											<TD  nowrap="nowrap"></TD>
										</TR>
									</TABLE>
									<asp:validationsummary id="ValidationSummary1" runat="server" Font-Names="Verdana" Font-Size="8pt" ShowMessageBox="True"
										ShowSummary="False" HeaderText="Os campos que marcados com * apresentam os problemas abaixo: "></asp:validationsummary></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD align="right" colSpan="2"><asp:imagebutton id="imgContinuar" runat="server" ImageUrl="../images/continuar.gif"></asp:imagebutton></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
