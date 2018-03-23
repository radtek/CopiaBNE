<%@ Page Language="c#"
    CodeBehind="quemevc.aspx.cs"
    AutoEventWireup="false"
    Inherits="SistMars.pessoa.frmQuemevc" %>

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
    <link rel="stylesheet"
        href="../include/SistMars.css"
        type="text/css">
</head>
<body ms_positioning="GridLayout"
    background="../images/fundos.jpg">
    <form id="frmQuemevc"
    method="post"
    runat="server">
    <table width="650"
        align="center"
        border="0">
        <tr>
            <td valign="middle"
                colspan="2">
                <img src="../images/logosistopo.gif"
                    border="0"><br>
                <br>
            </td>
        </tr>
        <tr>
            <td>
                <img src="../images/avaliacaoatraves.gif">
            </td>
            <td align="right">
                <img src="../images/direitos_autorais.gif"
                    border="0">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br>
                <img src="../images/delineamento.gif">
            </td>
        </tr>
        <tr>
            <td colspan="2"
                class="texto10 + justificar">
                &nbsp;&nbsp;&nbsp;&nbsp;O
                SistMars produz
                um relatório
                a partir de
                seus dados
                e características
                pessoais.
                O resultado
                obtido com
                a aplicação
                da avaliação
                é um delineamento
                de sua individualidade
                e tem dupla
                finalidade.
                Por um lado
                lhe proporciona
                a oportunidade
                de descobrir
                quais seus
                pontos fortes
                e fracos;
                por outro,
                pode auxiliar
                diante de
                um processo
                seletivo.<br>
                &nbsp;&nbsp;&nbsp;&nbsp;
                Como toda
                avaliação,
                o SistMars
                necessita
                de validação
                humana e seu
                resultado
                pode ser legitimado
                pelo conhecimento
                de pessoas
                próximas a
                você.
            </td>
        </tr>
        <tr>
            <td height="10">
            </td>
        </tr>
        <tr>
            <td colspan="2"
                align="right">
                <asp:ImageButton
                    ID="imgContinuar"
                    runat="server"
                    ImageUrl="../images/continuar.gif">
                </asp:ImageButton>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
