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
<div class="menu_sv">
    <%-- Coluna 01 do Menu da Tela Selecionadora --%>
    <div class="col01">
        <asp:LinkButton ID="btnPesquisarVagas" runat="server" CausesValidation="false" ToolTip="Pesquisa de Vagas"
            OnClick="btnPesquisarVagas_Click">
            <div class="btn_col">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <p class="block-ico">
                            <i class="fa fa-search fa-5x"></i>
                        </p>
                        <div class="titulo_texto_btn menor">
                            <span class="tit_btn_ss">Pesquisa de Vagas</span> <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>
                                        <asp:Label ID="lblPesquisaVagas" runat="server"></asp:Label>
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="btnRSM" runat="server" CausesValidation="false" ToolTip="Relatório Salarial de Mercado"
            OnClick="btnRSM_Click">
            <div class="btn_col bottom">
                <div class="fundo_btn_ss">
                                            <p class="overflow novo"><span class="img_faixaNovidade">Novo</span></p>

                    <div class="sombra_btn_ss rsm">
                       
                       <p class="block-ico"><i class="fa fa-line-chart fa-5x"></i></p>
                        <div class="titulo_texto_btn menor rsm">
                            <span class="tit_btn_ss">Relatório Salarial de Mercado</span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
    </div>
    <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
    <%-- Coluna 02 do Menu da Tela Selecionadora --%>
    <div class="col02">
        <asp:LinkButton ID="btnQuemMeViu" runat="server" CausesValidation="false" ToolTip="Quem me Viu"
            OnClick="btnQuemMeViu_Click">
            <div class="btn_col">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">

                        <p class="block-ico"><i class="fa fa-crosshairs fa-5x"></i></p>
                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">Quem me Viu</span> <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li><span class="negrito_btn">
                                        <asp:Label ID="lblQuantidadeQuemMeViu" runat="server"></asp:Label></span>
                                        <asp:Label ID="lblQuemMeViu" runat="server"></asp:Label>
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="btnAlertaVagas" runat="server" CausesValidation="false" ToolTip="Alerta de Vagas"
            OnClick="btnAlertaVagas_Click">
            <div class="btn_col bottom" title="Alerta de Vagas">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <p class="block-ico">
                            <span class="fa-stack fa-lg fa-2x">
                                <i class="fa fa-comment-o fa-flip-horizontal fa-stack-2x"></i>
                                <i class="fa fa-exclamation fa-stack-1x"></i>
                            </span>
                        </p>
                        <div class="titulo_texto_btn menor alerta">
                            <span class="tit_btn_ss">Alerta de Vagas</span> <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>
                                        <span>
                                            Receba novas vagas<br />diretamente no seu Email
                                        </span>
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
    </div>
    <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
    <%-- Coluna 03 do Menu da Tela Selecionadora --%>
    <div class="col03">
        <asp:LinkButton ID="btnEscolherEmpresa" runat="server" CausesValidation="false" ToolTip="Escolher Empresa"
            OnClick="btnEscolherEmpresa_Click">
            <div class="btn_col" title="Escolher Empresa">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <p class="block-ico"><i class="fa fa-building-o fa-5x"></i></p>

                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">Escolher Empresa</span> <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li><span class="negrito_btn">
                                        <asp:Label ID="lblQuantidadeEmpresasCadastradas" runat="server"></asp:Label></span>
                                        <asp:Label ID="lblEmpresasCadastradas" runat="server"></asp:Label>
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="btnMeuPlano" runat="server" CausesValidation="false" ToolTip="Mensagens"
            OnClick="btnMeuPlano_Click" CssClass="container_promocao_vip">
            <div class="btn_col bottom" title="Meu Plano">
                <div class="fundo_btn_ss">

                    <p class="overflow novo " runat="server"
                        id="imgVip" visible="False">
                        <span class="img_faixaNovidade">Promoção</span>
                    </p>

                    <div class="sombra_btn_ss">
                        <p class="block-ico"><i class="fa fa-check fa-5x"></i></p>

                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">Meu Plano</span> <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <asp:Panel ID="pnlPlanoVIP" runat="server">
                                        <li>
                                            <asp:Label ID="lblMeuPlano1" Text="Você possui o " runat="server"></asp:Label>
                                            <span class="negrito_btn">
                                                <asp:Label ID="lblPlano" runat="server"></asp:Label>
                                            </span></li>
                                        <li>
                                            <asp:Label ID="lblMeuPlano2" runat="server"></asp:Label>
                                            <span class="negrito_btn">
                                                <asp:Label ID="lblMeuPlano3" runat="server"></asp:Label>
                                            </span></li>
                                    </asp:Panel>
                                    <asp:Literal ID="litNaoVIP" runat="server"></asp:Literal>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
    </div>
    <%-- Fim da Coluna 03 do Menu da Tela Selecionadora --%>
</div>
<asp:UpdatePanel runat="server" ID="upRSM" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:ModalVendaRSM ID="ModalVendaRSM" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRSM" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
