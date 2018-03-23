<%@ Page Language="c#"
    CodeBehind="enquete.aspx.cs"
    AutoEventWireup="false"
    Inherits="SistMars.pessoa.frmEnquete" %>

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
    <meta content="http://schemas.microsoft.com/intellisense/ie5"
        name="vs_targetSchema">
    <link href="../include/SistMars.css"
        type="text/css"
        rel="stylesheet">
</head>
<body ms_positioning="GridLayout"
    background="../images/fundos.jpg">
    <form id="frmEnquete"
    method="post"
    runat="server">
    <table cellspacing="1"
        cellpadding="1"
        width="600"
        border="0"
        align="center">
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
                <img src="../images/avaliacao.gif">
            </td>
            <td align="right">
                <img src="../images/direitos_autorais.gif"
                    border="0">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table cellspacing="1"
                    cellpadding="1"
                    width="100%"
                    border="0">
                    <tr>
                        <td class="texto10 + justificar">
                            &nbsp;&nbsp;&nbsp;&nbsp;Até
                            que ponto
                            o resultado
                            pode desvendar
                            seu jeito
                            de ser e agir?
                            Esta ferramenta
                            poderia auxiliar
                            as empresas
                            a conhecer
                            melhor seus
                            funcionários?
                            São estes
                            questionamentos
                            que estamos
                            buscando responder
                            quando pedimos
                            para que você
                            opine sobre
                            o SistMars.<br>
                            &nbsp;&nbsp;&nbsp;&nbsp;Lembre-se
                            de que o resultado
                            do teste reflete
                            o seu perfil
                            atual, portanto
                            não se rotule
                            e nem considere
                            como definitiva
                            essa identificação.<br>
                            &nbsp;&nbsp;&nbsp;&nbsp;Reflita
                            sobre o resultado
                            apresentado
                            e, por favor,
                            responda as
                            questões abaixo.
                            Sua opinião
                            é necessaria
                            para a validação
                            do nosso sistema.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitulo">
                            O que você
                            achou do seu
                            resultado
                            do Sistmars
                            Test?
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList
                                ID="optCorresponde"
                                runat="server"
                                CssClass="texto10">
                                <asp:ListItem
                                    Value="0">N&#227;o corresponde ao meu perfil;</asp:ListItem>
                                <asp:ListItem
                                    Value="1">Corresponde razoavelmente ao meu perfil;</asp:ListItem>
                                <asp:ListItem
                                    Value="2">Corresponde ao meu perfil;</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1"
                                runat="server"
                                CssClass="texto8"
                                ErrorMessage="&amp;nbsp;&amp;nbsp;Por favor, selecione uma das respostas acima"
                                Display="Dynamic"
                                ControlToValidate="optCorresponde"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitulo">
                            Sugestões:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox
                                ID="txtObservacao"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="5"
                                Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:ImageButton
                                ID="imgContinuar"
                                runat="server"
                                ImageUrl="../images/continuar.gif">
                            </asp:ImageButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
