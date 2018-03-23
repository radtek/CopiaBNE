<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaginaEmBranco.aspx.cs" Inherits="BNE.Web.PaginaEmBranco" %>

<%@ Register TagPrefix="chat" TagName="RenderizarChat" Src="~/UserControlsChat/RenderizarChat.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/jquery-1.8.3.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server"></asp:ScriptManager>
        <div>
            <chat:RenderizarChat runat="server" />
        </div>
    </form>
</body>
</html>
