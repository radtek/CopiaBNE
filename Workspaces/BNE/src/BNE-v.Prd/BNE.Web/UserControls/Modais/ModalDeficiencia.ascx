<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalDeficiencia.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ModalDeficiencia" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalDeficiencia.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $("#cphConteudo_ucDeficiencia_cbAll").change(function (event) {
        checkAll();
    });

</script>
<asp:UpdatePanel ID="upDeficiencia" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hfModalDeficiencia" runat="server" />
        <asp:Panel ID="pnlDeficiencia" runat="server" CssClass="modal_confirmacao_registro deficiencia reduzida">
            <asp:UpdatePanel ID="upConteudo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdDeficiencias" runat="server" />
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo_modal_deficiencia"> Deficiências</asp:Label>
                    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="texto_deficiencia">

                        <asp:CheckBox ID="cbAll" runat="server" onchange="checkAll()" />
                            <asp:Label ID="lblTdodos" runat="server" Text="Todos"></asp:Label>
                            
                        <hr />
                     
                        <asp:Label ID="lblFisica" runat="server" Text="Física"></asp:Label>
                           <div class="Check_box">
                            <asp:CheckBoxList ID="cblFisica" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                        </div>
                        <hr />
                       
                            <asp:Label ID="Label1" runat="server" Text="Auditiva"></asp:Label>
                         <div class="Check_box">
                            <asp:CheckBoxList ID="cblAuditiva" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                        </div>
                        <hr />
                       
                            <asp:Label ID="Label2" runat="server" Text="Visual"></asp:Label>
                         <div class="Check_box">
                            <asp:CheckBoxList ID="cblVisual" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                        </div>

                        <hr />
                       
                            <asp:Label ID="Label3" runat="server" Text="Mental"></asp:Label>
                         <div class="Check_box">
                            <asp:CheckBoxList ID="cblMental" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                        </div>

                    <div class="rodape">
                        <asp:Button ID="lnkOK" runat="server" CssClass="btn_Confirma" Text="OK"></asp:Button>
                    </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <AjaxToolkit:ModalPopupExtender ID="mpeDeficiencia" runat="server" PopupControlID="pnlDeficiencia"
            TargetControlID="hfModalDeficiencia" CancelControlID="lnkOK" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btiFechar" EventName="Click" />
    </Triggers>



</asp:UpdatePanel>

