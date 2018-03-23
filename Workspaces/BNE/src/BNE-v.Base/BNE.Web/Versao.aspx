<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Versao.aspx.cs" Inherits="BNE.Web.Versao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:Label ID="lblVersao" runat="server"></asp:Label>
        <asp:Label ID="lblErro" runat="server"></asp:Label>
        <Employer:ControlCNPJReceitaFederal runat="server" ID="txtCNPJ" ValidarReceitaFederal="False"
            Obrigatorio="true" ValidationGroup="SalvarDadosEmpresa" cssclasstextbox="textbox_padrao"
            TextBoxCssClass="textbox_padrao" />
        <Employer:ControlCNPJReceitaFederal runat="server" ID="ControlCNPJReceitaFederal1"
            ValidarReceitaFederal="True" Obrigatorio="true" ValidationGroup="SalvarDadosEmpresa"
            cssclasstextbox="textbox_padrao" TextBoxCssClass="textbox_padrao" />
    </div>
    </form>
</body>
</html>
