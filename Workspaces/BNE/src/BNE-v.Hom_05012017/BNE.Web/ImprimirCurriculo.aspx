﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimirCurriculo.aspx.cs"
    Inherits="BNE.Web.ImprimirCurriculo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body { background-image: none; background-color: white; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal runat="server" ID="litCurriculo"></asp:Literal>
    </div>
    </form>
</body>
</html>
