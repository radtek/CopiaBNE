<%@ Page Language="c#"
    CodeBehind="resultadoImpressao.aspx.cs"
    AutoEventWireup="false"
    Inherits="SistMars.pessoa.frmResultadoImpressao" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>.: SistMars
        :.</title>
    <meta name="GENERATOR"
        content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE"
        content="C#">
    <meta name="vs_defaultClientScript"
        content="JavaScript">
    <meta name="vs_targetSchema"
        content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../include/SistMars.css"
        type="text/css"
        rel="stylesheet">
</head>
<body ms_positioning="GridLayout"
    onload="window.print();">
    <form id="frmResultadoImpressao"
    method="post"
    runat="server">
    <table width="650"
        align="center"
        border="0">
        <tr>
            <td valign="top">
                <img src="../images/direitos_autorais.gif"
                    border="0">
            </td>
            <%--<TD align=right rowspan=3><IMG id="imgTeste" runat="server" src="../images/logosistopo.gif" border=0></TD>--%>
            <td align="right"
                rowspan="3">
                <asp:Image
                    ID="imgTeste"
                    runat="server"
                    ImageUrl="../images/logosistopo.gif" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td valign="bottom">
                <img src="../images/identficapessoal.gif">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table class="texto10"
                    cellspacing="1"
                    cellpadding="1"
                    width="100%"
                    border="0">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label
                                    ID="lblNome"
                                    Font-Size="10pt"
                                    Font-Names="Verdana"
                                    runat="server"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Data de
                                Nascimento:</strong>&nbsp;
                            <asp:Label
                                ID="lblDataNascimento"
                                Font-Size="10pt"
                                Font-Names="Verdana"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br>
            </td>
        </tr>
        <tr>
            <td class="justificar"
                colspan="2">
                <img src="../images/caracteristicaspersonalidade.gif"><br>
                <br>
                <asp:Label
                    ID="lblPersonalidade"
                    Font-Size="10pt"
                    Font-Names="Verdana"
                    runat="server"></asp:Label>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
        <tr id="trCaracteristica"
            runat="server"
            colspan="2">
            <td class="justificar"
                colspan="2">
                <p>
                    <img src="../images/espelhopersonalidade.gif"><br>
                    <br>
                    <asp:Label
                        ID="lblCaracteristica"
                        Font-Size="10pt"
                        Font-Names="verdana"
                        runat="server"></asp:Label></p>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
        <tr>
            <td class="justificar"
                colspan="2">
                <p>
                    <img src="../images/seuestagioatual.gif"><br>
                    <br>
                    <asp:Label
                        ID="lblEstagioAtual"
                        Font-Size="10pt"
                        Font-Names="verdana"
                        runat="server"></asp:Label></p>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
        <tr>
            <td class="justificar"
                colspan="2">
                <p>
                    <img src="../images/recomendacoespessoais.gif">&nbsp;
                    <br>
                    <br>
                    <asp:Label
                        ID="lblPessoal"
                        Font-Size="10pt"
                        Font-Names="verdana"
                        runat="server"></asp:Label></p>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
