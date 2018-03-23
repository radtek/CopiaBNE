<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimirProposta.aspx.cs" Inherits="BNE.Web.ImprimirProposta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblProposta" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>