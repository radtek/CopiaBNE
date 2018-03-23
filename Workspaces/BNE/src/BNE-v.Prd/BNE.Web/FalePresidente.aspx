<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="FalePresidente.aspx.cs"
    Inherits="BNE.Web.FalePresidente" %>

<%@ Register
    Src="UserControls/ucFalePresidente.ascx"
    TagName="FalePresidente"
    TagPrefix="uc3" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <uc3:FalePresidente
        ID="ucFalePresidente"
        runat="server" />
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
