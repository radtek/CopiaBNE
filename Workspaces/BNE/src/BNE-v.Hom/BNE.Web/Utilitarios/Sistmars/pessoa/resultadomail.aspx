<%@ Page Language="c#"
    CodeBehind="resultadomail.aspx.cs"
    AutoEventWireup="false"
    Inherits="SistMars.pessoa.resultadomail" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>.: SistMars
        :.</title>
    <meta content="Microsoft Visual Studio 7.0"
        name="GENERATOR">
    <meta content="C#"
        name="CODE_LANGUAGE">
    <meta content="JavaScript"
        name="vs_defaultClientScript">
    <link href="http://www.bne.com.br/Utilitarios/SistMars/include/SistMars.css"
        type="text/css"
        rel="stylesheet">

    <script src="http://www.bne.com.br/Utilitarios/SistMars/include/funcoes.js"
        language="javascript"></script>

</head>
<body background="http://www.bne.com.br/Utilitarios/SistMars/images/fundos.jpg"
    ms_positioning="GridLayout">
    <table height="1008"
        cellspacing="0"
        cellpadding="0"
        width="1006"
        border="0"
        ms_2d_layout="TRUE">
        <tr>
            <td width="0"
                height="0">
            </td>
            <td width="10"
                height="0">
            </td>
            <td width="996"
                height="0">
            </td>
        </tr>
        <tr valign="top">
            <td width="0"
                height="15">
            </td>
            <td colspan="2"
                rowspan="2">
                <form id="frmResultadomail"
                method="post"
                runat="server">
                <table height="989"
                    cellspacing="0"
                    cellpadding="0"
                    width="708"
                    border="0"
                    ms_2d_layout="TRUE">
                    <tr valign="top">
                        <td width="10"
                            height="15">
                        </td>
                        <td width="698">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td height="974">
                        </td>
                        <td>
                            <table height="989"
                                cellspacing="0"
                                cellpadding="0"
                                width="708"
                                border="0"
                                ms_2d_layout="TRUE">
                                <table height="973"
                                    cellspacing="0"
                                    cellpadding="0"
                                    width="697"
                                    border="0"
                                    ms_2d_layout="TRUE">
                                    <tbody>
                                        <tr valign="top">
                                            <td width="36"
                                                height="973">
                                            </td>
                                            <td width="661">
                                                <table height="973"
                                                    cellspacing="0"
                                                    cellpadding="0"
                                                    width="697"
                                                    border="0"
                                                    ms_2d_layout="TRUE">
                                                </table>
                                                <table width="650"
                                                    align="center"
                                                    border="0"
                                                    height="957">
                                                    <tbody>
                                                        <tr>
                                                            <td align="center">
                                                                <img src="http://www.bne.com.br/Utilitarios/SistMars/images/logosistopo.gif"
                                                                    border="0"><br>
                                                                <br>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table cellspacing="1"
                                                                    cellpadding="0"
                                                                    width="100%"
                                                                    border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img src="http://www.bne.com.br/Utilitarios/SistMars/images/delineamento.gif">
                                                                            <br>
                                                                            <img src="http://www.bne.com.br/Utilitarios/SistMars/images/avaliacao.gif">
                                                                        </td>
                                                                        <td align="right">
                                                                            <img src="http://www.bne.com.br/Utilitarios/SistMars/images/direitos_autorais.gif"
                                                                                border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <img src="http://www.bne.com.br/Utilitarios/SistMars/images/identficapessoal.gif">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
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
                                                            <td class="justificar">
                                                                <img src="http://www.bne.com.br/Utilitarios/SistMars/images/caracteristicaspersonalidade.gif"><br>
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
                                                            runat="server">
                                                            <td class="justificar">
                                                                <p>
                                                                    <img src="http://www.bne.com.br/Utilitarios/SistMars/images/espelhopersonalidade.gif"><br>
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
                                                            <td class="justificar">
                                                                <p>
                                                                    <img src="http://www.bne.com.br/Utilitarios/SistMars/images/seuestagioatual.gif"><br>
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
                                                            <td class="justificar">
                                                                <p>
                                                                    <img src="http://www.bne.com.br/Utilitarios/SistMars/images/recomendacoespessoais.gif">&nbsp;
                                                                    <br>
                                                                    <br>
                                                                    <asp:Label
                                                                        ID="lblPessoal"
                                                                        Font-Size="10pt"
                                                                        Font-Names="verdana"
                                                                        runat="server"></asp:Label></p>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:ImageButton
                                                                    ID="imgContinuar"
                                                                    runat="server"
                                                                    ImageUrl="http://www.bne.com.br/Utilitarios/SistMars/images/continuar.gif">
                                                                </asp:ImageButton>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </table>
                        </td>
                    </tr>
                </table>
                </form>
            </td>
        </tr>
    </table>
</body>
</html>
