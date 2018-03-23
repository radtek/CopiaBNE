<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBreadCrumbs.ascx.cs" Inherits="BNE.Web.UserControls.ucBreadCrumbs" %>
<link href="../css/local/UserControls/Breadcrumbs.css" rel="stylesheet" />
<nav id="navBreadcrumbForms" runat="server" class="menu_breadcrumb" clientidmode="Static">
    <ol id="breadcrumbList" runat="server" class="breadcrumb" itemscope itemtype="http://schema.org/BreadcrumbList">
        <asp:Literal runat="server" ID="links"></asp:Literal>
        <asp:Literal runat="server" ID="nmePagina"></asp:Literal>
    </ol>
</nav>