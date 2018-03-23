<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ComprarSMS.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ComprarSMS" %>
<asp:Panel
    ID="pnlComprarSMS"
    Style="display: none"
    runat="server">
    <asp:Label
        ID="lblQuantidadeSMS"
        runat="server"></asp:Label>
</asp:Panel>
<asp:HiddenField
    ID="hfVariavel"
    runat="server" />
<AjaxToolkit:ModalPopupExtender
    ID="mpeComprarSMS"
    runat="server"
    PopupControlID="pnlComprarSMS"
    TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
