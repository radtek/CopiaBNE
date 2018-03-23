<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloquearCandidato.ascx.cs" Inherits="BNE.Web.UserControls.Modais.BloquearCandidato" %>
<asp:Panel ID="pnlBloquearCandidato" CssClass="modal_conteudo candidato reduzida bloquear_candidato"
    Style="display: none; background-color: #ebeff2 !important; height: auto !important;" runat="server">
    <asp:UpdatePanel ID="upBloquearCandidato" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h2 class="titulo_modal_nova">
                <asp:Label ID="Label1" runat="server" Text="Bronquinha"></asp:Label>
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass=" modal_fechar" ID="btiFechar" ImageUrl="/img/modal_nova/btn_amarelo_fechar_modal.png"
                        runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel CssClass="coluna_esquerda bloqueio" ID="pnlColunaEsquerda" runat="server">
                <asp:Panel CssClass="painel_bronquinha" ID="pnlEsquerdaBloquearCandidato" runat="server">
                    <asp:Image CssClass="sucesso-icon" ID="imgLogo" ImageUrl="/img/icn_bronquinha.png" AlternateText="" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass=" painel_bloquear_candidato modal_corpo" ID="pnlBloquearCandidatoCentro" runat="server">
                <div class="linha">
                    <div class="label_principal">
                        <asp:Literal ID="litTextoNomeCandidato" runat="server"></asp:Literal>
                    </div>
                    <div class="container_campo">
                        <asp:Literal ID="lblNomeCandidato" runat="server"></asp:Literal>
                    </div>
                </div>

                <div class="linha">
                    <div class="label_principal">
                        <asp:Label runat="server" ID="lblInformeMotivo" Text="Informe abaixo o motivo:" CssClass="label_principal" />
                        <asp:Label runat="server" ID="lblAvisoBloqueio" Text="" CssClass="label_principal"></asp:Label>
                    </div>
                     <div class="container_campo">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <div class="label_principal">
                        <asp:RequiredFieldValidator runat="server" ID="rfMotivo" ControlToValidate="tbxMotivo" Text="Campo Obrigatório." ValidationGroup="Bloquear"></asp:RequiredFieldValidator>
                        <br />
                        <telerik:RadTextBox runat="server" ID="tbxMotivo" TextMode="MultiLine" Rows="3" ValidationGroup="Bloquear" MaxLength="512"
                            EmptyMessage="" CssClass="textbox_padrao bloquear_candidato" Width="537px">
                        </telerik:RadTextBox>
                    </div>
                </div>
                <asp:Panel ID="pnlBloqueioEmail" CssClass="pnl_bloquear_candidato_email"  runat="server" Visible="false">
                    <div class="pnl_bloquear_candidato_email_text">
                        Ao bloquear este currículo, o <b>BNE</b> não enviará mais mensagens via e-mail para o endereço:
                        <b>
                            <asp:Literal ID="litEmailCandidato" runat="server"></asp:Literal></b>
                    </div>
                </asp:Panel>
                <asp:Panel CssClass="painel_botoes" ID="pnlBloquearDesbloquear1" runat="server">
                    <div class="btnVoltar">
                        <asp:Button ID="btnBloquearCandidato" runat="server" Text="Bloquear" CausesValidation="true"
                            ValidationGroup="Bloquear" OnClick="btnBloquearCandidato_Click" />
                    </div>
                    <div class="btnVoltar">
                        <asp:Button ID="btnBloqueadoCandidato" runat="server" Text="Desbloquear" CausesValidation="true"
                            ValidationGroup="Bloquear" OnClick="btnDesbloquearCandidato_Click" />
                    </div>
                </asp:Panel>
                <!--INÍCIO Listar os motivos do Bronquinha-->
                <telerik:RadGrid ID="gvBronquinha" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao gridVerMotivosBronquinha" runat="server" Skin="Office2007" GridLines="None">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <MasterTableView DataKeyNames="Idf_Curriculo_Correcao">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Motivo" ItemStyle-CssClass="data">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescricao" runat="server" Text='<%# Eval("Des_Correcao") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="nome_empresa">
                                <ItemTemplate>
                                    <asp:Label ID="lblData" runat="server" Text='<%# Eval("Dta_Cadastro","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <!--FIM Listar os motivos do Bronquinha-->

            </asp:Panel>
            <asp:Panel CssClass=" painel_confirmar_bloqueio" ID="pnlPerguntaBloquear"
                runat="server">
                <div class="pergunta_bloquear modal_corpo">
                    <asp:Literal ID="litPerguntaBloquear" runat="server"></asp:Literal>
                </div>
                <asp:Panel CssClass="painel_botoes" ID="pnlBotaoBloquieoDesbloqueio" runat="server">

                    <asp:Button ID="btnSimBloquear" runat="server" Text="Sim" CausesValidation="true" CssClass="botao_padrao"
                        ValidationGroup="Bloquear" OnClick="btnSimBloquear_Click" />

                    <asp:Button ID="btnNaoBloquear" runat="server" Text="Não" CssClass="botao_padrao" CausesValidation="true"
                        ValidationGroup="Bloquear" OnClick="btnNaoBloquear_Click" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeBloquearCandidato" TargetControlID="hfVariavel" PopupControlID="pnlBloquearCandidato"
    runat="server">
</AjaxToolkit:ModalPopupExtender>
