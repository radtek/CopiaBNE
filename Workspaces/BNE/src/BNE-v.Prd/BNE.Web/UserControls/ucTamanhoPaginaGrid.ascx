<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ucTamanhoPaginaGrid.ascx.cs"
    Inherits="BNE.Web.UserControls.ucTamanhoPaginaGrid" %>
<asp:Panel
    ID="Panel1"
    runat="server">
    <asp:Label
        ID="lblTextoParte1"
        runat="server"
        Text="Exibir"></asp:Label>
    &nbsp;<asp:DropDownList
        ID="ddlTamanhoPagina"
        runat="server"
        AutoPostBack="True"
        OnSelectedIndexChanged="ddlTamanhoPagina_SelectedIndexChanged">
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>200</asp:ListItem>
    </asp:DropDownList>
    &nbsp;<asp:Label
        ID="lblTextoParte2"
        runat="server"
        Text="Por Página"></asp:Label>
</asp:Panel>
