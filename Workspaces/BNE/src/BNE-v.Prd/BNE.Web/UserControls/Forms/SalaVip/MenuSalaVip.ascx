<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="MenuSalaVip.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaVip.MenuSalaVip" %>
<%@ Register TagPrefix="uc1" TagName="ModalVendaRSM" Src="~/UserControls/Modais/ModalVendaRSM.ascx" %>
<script type="text/javascript">
    $(document).ready(function () {
        setInterval(PosicionarVIP, 50);
    });

    function PosicionarVIP() {
        $(".promocao_vip").each(
            function () {
                $(this).css("position", "absolute");
                $(this).css("top", $(this).closest(".fundo_btn_ss_g").offset().top - 12);
                $(this).css("left", $(this).closest(".fundo_btn_ss_g").offset().left + $(this).closest(".fundo_btn_ss_g").outerWidth() - $(this).outerWidth() + 7);
                $(this).css("border", "0px");
            }
        );
    }

</script>
<%--Menu Tela VIP --%>
<div class="menu_sv "> 
     <%-- Meu Currículo --%>
     <div class="menu_sv_block">
        <asp:HyperLink runat="server" ID="hlMeuCurriculo">
            <div><i class="fa fa-file-text-o  fa-5x"></i></div>
            <div class="menu_sv_block-title">Meu Currículo</div>
            <div>
                Atualize as informações do seu CV
            </div>            
        </asp:HyperLink>
    </div>
    <%-- Pesquisa de Vagas (Vagas no Perfil) --%>
    <div class="menu_sv_block">
        <asp:LinkButton ID="btnPesquisarVagas" runat="server" CausesValidation="false" ToolTip="Pesquisa de Vagas"
            OnClick="btnPesquisarVagas_Click">
            <div>
                <i class="fa fa-search fa-5x"></i>
            </div>
            <div class="menu_sv_block-title">Pesquisa de Vagas</div>
            <div>
                <asp:Label ID="lblPesquisaVagas" runat="server"></asp:Label>
            </div>
        </asp:LinkButton>        
    </div>
     <%-- Quem me viu --%>
    <div class="menu_sv_block">
        <asp:LinkButton ID="btnQuemMeViu" runat="server" CausesValidation="false" ToolTip="Quem me Viu"
            OnClick="btnQuemMeViu_Click">
            <div><i class="fa fa-crosshairs fa-5x"></i></div>
            <div class="menu_sv_block-title">Quem me Viu</div>
            <div>
                <strong><asp:Label ID="lblQuantidadeQuemMeViu" runat="server"></asp:Label> </strong>
                <asp:Label ID="lblQuemMeViu" runat="server"></asp:Label>
            </div>            
        </asp:LinkButton>        
    </div>
     <%-- Já Enviei --%>
     <div class="menu_sv_block">
         <asp:HyperLink runat="server" ID="hlJaEnviei">
            <div><i class="fa fa-check fa-5x"></i></div>
            <div class="menu_sv_block-title">Já Enviei</div>
            <div>
                Confira as vagas para as quais já enviou seu CV
            </div>            
         </asp:HyperLink> 
    </div>
     <%-- Escolher Empresa --%>
    <div class="menu_sv_block">
        <asp:LinkButton ID="btnEscolherEmpresa" runat="server" CausesValidation="false" ToolTip="Escolher Empresa"
            OnClick="btnEscolherEmpresa_Click">            
            <div><i class="fa fa-building-o fa-5x"></i></div>
            <div class="menu_sv_block-title">Escolher Empresa</div>
            <div>
                <asp:Label ID="lblQuantidadeEmpresasCadastradas" runat="server"></asp:Label>
                <asp:Label ID="lblEmpresasCadastradas" runat="server"></asp:Label>
            </div>
        </asp:LinkButton>        
    </div>
     <%-- Relatório salarial --%>
    <div class="menu_sv_block">
        <asp:LinkButton ID="btnRSM" runat="server" CausesValidation="false" ToolTip="Relatório Salarial de Mercado"
            OnClick="btnRSM_Click">
            <div><i class="fa fa-line-chart fa-5x"></i></div>
            <div class="menu_sv_block-title">Relatório Salarial de Mercado</div>
        </asp:LinkButton>
    </div>
     <%-- Alerta de Vagas --%>
    <div class="menu_sv_block">
        <asp:LinkButton ID="btnAlertaVagas" runat="server" CausesValidation="false" ToolTip="Alerta de Vagas"
            OnClick="btnAlertaVagas_Click">
           <div>
                <span class="fa-stack fa-lg fa-2x">
                    <i class="fa fa-comment-o fa-flip-horizontal fa-stack-2x"></i>
                    <i class="fa fa-exclamation fa-stack-1x"></i>
                </span>
            </div>
            <div class="menu_sv_block-title">Alerta de Vagas</div>
            <div >
                Receba novas vagas diretamente no seu Email
            </div>
        </asp:LinkButton>
    </div>
     <%-- Meu Plano --%>
    <div class="menu_sv_block">
        <asp:LinkButton ID="btnMeuPlano" runat="server" CausesValidation="false" ToolTip="Mensagens"
            OnClick="btnMeuPlano_Click" CssClass="container_promocao_vip">       
            <p class="overflow novo " runat="server"
                id="imgVip" visible="False">
                <span class="img_faixaNovidade">Promoção</span>
            </p>                    
            <div><i class="fa fa-check fa-5x"></i></div>
            <div class="menu_sv_block-title">Meu Plano</div>
            <div>
                <asp:Panel ID="pnlPlanoVIP" runat="server">
                    <div>
                        <asp:Label ID="lblMeuPlano1" Text="Você possui o " runat="server"></asp:Label>
                        <asp:Label ID="lblPlano" runat="server"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblMeuPlano2" runat="server"></asp:Label>
                        <asp:Label ID="lblMeuPlano3" runat="server"></asp:Label>             
                    </div>
                </asp:Panel>
                <asp:Literal ID="litNaoVIP" runat="server"></asp:Literal>
            </div>
        </asp:LinkButton>
    </div>
</div>
<asp:UpdatePanel runat="server" ID="upRSM" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:ModalVendaRSM ID="ModalVendaRSM" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRSM" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
