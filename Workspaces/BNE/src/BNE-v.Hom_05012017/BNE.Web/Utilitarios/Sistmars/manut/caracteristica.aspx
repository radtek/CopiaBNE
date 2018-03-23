<%@ Page language="c#" Codebehind="caracteristica.aspx.cs" AutoEventWireup="false" Inherits="SistMars.manut.caracteristica" %>
<%@ Register TagPrefix="uc1" TagName="seguranca" Src="../include/seguranca.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuManut" Src="../include/menuManut.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>.: SistMars | Manut :.</title>
		<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
		<meta content=C# name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name=vs_targetSchema>
		<LINK href="../include/SistMars.css" type="text/css" rel=stylesheet>
  </HEAD>
	<body MS_POSITIONING="GridLayout" background="../images/fundos.jpg">
		<form id=frmCaracteristica method=post runat="server">
			<IMG src="../images/pixel.gif" onload=txtColunaLinha.focus();>
			<uc1:seguranca id=Seguranca1 runat="server"></uc1:seguranca>
			<uc1:menumanut id=MenuManut1 runat="server"></uc1:menumanut>
			<TABLE cellSpacing=1 cellPadding=1 border=0 align=center>
				<TR>
					<TD class=tdTitulo valign=middle><B>Manutenção de Características</B></TD></TR>
				<TR>
					<TD>&nbsp;</TD></TR>
				<TR>
					<TD valign=middle>
						<TABLE class=texto8 borderColor=#2a186f cellSpacing=0 cellPadding=1 width=720 align=center border=1>
							<TR class=titulo1 valign=middle>
								<TD>&nbsp;</TD>
								<TD>A</TD>
								<TD>B</TD>
								<TD>C</TD>
								<TD>D</TD>
								<TD>E</TD>
								<TD>F</TD>
								<TD>G</TD>
								<TD>H</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>1</TD>
								<TD width="12%">Intuitivo</TD>
								<TD width="12%">Empreendedor</TD>
								<TD width="12%">Explosivo</TD>
								<TD width="12%">Curioso</TD>
								<TD width="12%">Dinâmico</TD>
								<TD width="12%">Íntegro</TD>
								<TD width="12%">Respeitador</TD>
								<TD width="12%">Idealista</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>2</TD>
								<TD>Persuasivo</TD>
								<TD>Presente</TD>
								<TD>Vencedor</TD>
								<TD>Versátil</TD>
								<TD>Convincente</TD>
								<TD>Resolvido</TD>
								<TD>Sereno</TD>
								<TD>Feliz</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>3</TD>
								<TD>Firme</TD>
								<TD>Moderado</TD>
								<TD>Julgador</TD>
								<TD>Agradável</TD>
								<TD>Líder</TD>
								<TD>Planejador</TD>
								<TD>Inconveniente</TD>
								<TD>Impulsivo</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>4</TD>
								<TD>Teimoso</TD>
								<TD>Guerreiro</TD>
								<TD>Vibrante</TD>
								<TD>Imprevisível</TD>
								<TD>Leal</TD>
								<TD>Egoísta</TD>
								<TD>Unificador</TD>
								<TD>Animado</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>5</TD>
								<TD>Sério</TD>
								<TD>Legislador</TD>
								<TD>Controlador</TD>
								<TD>Protetor</TD>
								<TD>Aventureiro</TD>
								<TD>Profissional</TD>
								<TD>Solitário</TD>
								<TD>Comprometido</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>6</TD>
								<TD>Resistente</TD>
								<TD>Motivado</TD>
								<TD>Emotivo</TD>
								<TD>Defensor</TD>
								<TD>Distraído</TD>
								<TD>Criativo</TD>
								<TD>Sensível</TD>
								<TD>Sonhador</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>7</TD>
								<TD>Econômico</TD>
								<TD>Observador</TD>
								<TD>Prudente</TD>
								<TD>Verdadeiro</TD>
								<TD>Arrebatador</TD>
								<TD>Prático</TD>
								<TD>Paciente</TD>
								<TD>Incorrigível</TD></TR>
							<TR bgColor=#ffffff>
								<TD class=titulo1 valign=middle>8</TD>
								<TD>Autoritário</TD>
								<TD>Ambicioso</TD>
								<TD>Indomável</TD>
								<TD>Exigente</TD>
								<TD>Envolvido</TD>
								<TD>Sofisticado</TD>
								<TD>Superior</TD>
								<TD>Deprimido</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>&nbsp;</TD></TR>
				<TR>
					<TD class=texto10>
						<B>Linha/coluna:</B>
						<asp:textbox id=txtColunaLinha runat="server" Width="35px" MaxLength="2" AutoPostBack="True"></asp:textbox>&nbsp;
						<asp:button id=cmdBuscar runat="server" Text="Buscar"></asp:button>&nbsp;
						<asp:regularexpressionvalidator id=revColunaLinha runat="server" Font-Size="8pt" Font-Names="Verdana" ValidationExpression="(A|B|C|D|E|F|G|H|a|b|c|d|e|f|g|h)(1|2|3|4|5|6|7|8)" ControlToValidate="txtColunaLinha" Display="Dynamic" ErrorMessage="Digite uma coluna/linha válida (ex: A1)"></asp:regularexpressionvalidator>
						<asp:requiredfieldvalidator id=rfvColunaLinha runat="server" Font-Size="8pt" Font-Names="Verdana" ControlToValidate="txtColunaLinha" Display="Dynamic" ErrorMessage="Digite a coluna e a linha (ex: A1)"></asp:requiredfieldvalidator>
					</TD>
				</TR>
				<TR>
					<TD>&nbsp;</TD>
				</TR>
				<TR>
					<TD class=texto10b>
						Característica: <asp:textbox id=txtDescricao runat="server" Enabled="False">-| n&#227;o dispon&#237;vel |-</asp:textbox>
					</TD>
				</TR>
				<TR class=texto10b>
					<TD align=right>
						<asp:textbox id=txtTexto runat="server" Width="720px" TextMode="MultiLine" Rows="10">-| n&#227;o dispon&#237;vel |-</asp:textbox><BR>
						<asp:Button id="cmdGravar" runat="server" Text="Gravar" Width="100px"></asp:Button>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
