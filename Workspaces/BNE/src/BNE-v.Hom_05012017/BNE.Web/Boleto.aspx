<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Boleto.aspx.cs" Inherits="BNE.Web.Boleto" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="boletonet" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>  
<boletonet:BoletoBancario ID="boletoBancario" runat="server" CodigoBanco="399" ></boletonet:BoletoBancario>
</body>
</html>
