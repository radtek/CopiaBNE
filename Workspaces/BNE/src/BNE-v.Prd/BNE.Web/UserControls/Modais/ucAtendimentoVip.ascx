<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAtendimentoVip.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ucAtendimentoVip" %>

<asp:Panel ID="pnlAtendimentoVip" CssClass="modal_conteudo candidato reduzida bloquear_candidato"
    Style="display: none; background-color: #ebeff2 !important; height: auto !important;" runat="server">
  
    <script src="../../js/jquery.maskedinput.min.js"></script>
    <asp:UpdatePanel ID="upAtendimentoVip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <script src="../../js/local/Modais/ucAtendimentoVip.js"></script>
            <asp:HiddenField runat="server" ID="hdfPlanoAd" />
            <asp:HiddenField runat="server" ID="hdfCurriculo" />
            <h2 class="titulo_modal_nova">
                <asp:Label ID="lblTitulo" style="color: #fff;" runat="server" ></asp:Label>
            </h2>

            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass=" modal_fechar" ID="btiFechar" ImageUrl="/img/modal_nova/btn_amarelo_fechar_modal.png"
                        runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel CssClass=" painel_bloquear_candidato modal_corpo" ID="pnlvipCandidatoCentro" runat="server">
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
                        <asp:Literal ID="Literal6" runat="server" Text="Valor Atual:"></asp:Literal>
                    </div>
                    <div class="container_campo">
                        <asp:Label runat="server" ID="lblValorPlano" ClientIDMode="Static"></asp:Label>

                    </div>
                </div>

                <div class="linha">
                    <div class="label_principal">
                        <asp:Literal ID="Literal2" runat="server" Text="Historico dos últimos Pagamentos"></asp:Literal>
                    </div>
                    <div class="container_campo">
                    </div>
                </div>
                <div class="linha">
                    <div class="label_principal">
                        <telerik:RadGrid ID="gvPagamentos" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao gridVerMotivosBronquinha" runat="server" Skin="Office2007" GridLines="None">
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                            <AlternatingItemStyle CssClass="alt_row" />
                            <MasterTableView DataKeyNames="vlr_parcela">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Data do Pagamento" ItemStyle-CssClass="data">
                                        <ItemTemplate>
                                            <asp:Label ID="lblData" runat="server" Text='<%# Eval("dta_pagamento","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="nome_empresa">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescricao" runat="server" Text='<%# Eval("vlr_parcela") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>



                <div class="linha">
                    <div class="label_principal">
                        <asp:Literal ID="Literal7" runat="server" Text="Fornecer Desconto"></asp:Literal>
                    </div>
                    <div class="container_campo">
                        <asp:RadioButtonList runat="server" ID="rbFormaDesconto" onclick="ChangeDes();" ClientIDMode="Static" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Porcentagem" Selected="True" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Fixo" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div class="linha">
                    <div class="label_principal">
                    </div>
                    <div class="container_campo">
                        <span id="spAviso" style="color:red;"></span>

                    </div>
                </div>

                <div class="linha">
                    <div class="label_principal">
                        <asp:Literal ID="Literal1" runat="server" Text="Valor do Desconto:"></asp:Literal>
                    </div>
                    <div class="container_campo">
                         <asp:CustomValidator ID="cvValorDesconto" ValidationGroup="DarDesconto" runat="server" ClientIDMode="Static" ErrorMessage="coloque o desconto"
                                    ClientValidationFunction="valCalcDesconto"></asp:CustomValidator><br />
                        <asp:TextBox runat="server" ID="txtValorDesconto" onkeyup="calcDesconto();" AutoPostBack="false" ClientIDMode="Static"></asp:TextBox>

                    </div>
                </div>

                <div class="linha">
                    <div class="label_principal">
                        <asp:Literal ID="Literal8" runat="server" Text="Preço Final:"></asp:Literal>
                    </div>
                    <div class="container_campo">
                        R$ 
                        <asp:Label ID="lblValorComDesconto" runat="server" ClientIDMode="Static"></asp:Label>
                        <asp:HiddenField runat="server" ID="hdfValorComDesconto" ClientIDMode="Static" />
                    </div>
                </div>
                <asp:HiddenField runat="server" ID="percentDesconto" ClientIDMode="Static" />


                <div class="linha">
                    <div class="label_principal">
                        <asp:Literal ID="Literal4" runat="server" Text="Motivo Cancelamento:"></asp:Literal>
                    </div>
                    <div class="container_campo">
                        <asp:Literal runat="server" ID="litMotivoCancelamento"></asp:Literal>
                    </div>
                </div>
                <asp:Panel runat="server" Visible="false" ID="pnlVipCancelado">
                    <div class="linha">
                        <div class="label_principal">
                            <asp:Literal ID="Literal3" runat="server" Text="Recorrência Cancelada:"></asp:Literal>
                        </div>
                        <div class="container_campo">
                            <asp:Literal ID="lblDataCancelamento" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div class="linha">
                        <div class="label_principal">
                            <asp:Literal ID="Literal5" runat="server" Text="Cancelado por:"></asp:Literal>
                        </div>
                        <div class="container_campo">
                            <asp:Literal ID="litQuemCancelou" runat="server"></asp:Literal>
                        </div>
                    </div>
                </asp:Panel>

                <div class="linha">
                     <div class="label_principal">
                            <asp:Literal ID="Literal9" runat="server" Text=""></asp:Literal>
                        </div>
                    <div class="container_campo">

                         <asp:CustomValidator ID="cvMotivo" ValidationGroup="CancelarPlano" runat="server" ErrorMessage="Selecione pelo menos um dos motivos"
                                    ClientValidationFunction="ValCheckBoxList"></asp:CustomValidator>
                                <asp:CheckBoxList runat="server" ID="cblMotivoCancelar" onclick="selectdItem();" ClientIDMode="Static" RepeatDirection="Vertical" TextAlign="Right">
                                </asp:CheckBoxList>

                         <div class="togglevisibility" style="display: none;">
                                <asp:TextBox ID="txtOutro" TextMode="MultiLine" CssClass="" runat="server" placeholder="Quais? (opcional)"></asp:TextBox>
                            </div>
                    </div>
                </div>


                <asp:Panel runat="server" ID="pnlDesconto" Visible="false">
                    <div class="linha">
                        <div class="label_principal">
                            <asp:Literal ID="litDescontoConcedido" runat="server"></asp:Literal>
                        </div>
                </asp:Panel>
                <div class="painel_botoes">
                    <asp:Button runat="server" ID="lnkDesconto" CssClass="btn btn-secondary" ValidationGroup="DarDesconto" Text="Desconto Plano" ClientIDMode="Static"  OnClick="linkDesconto_Click"></asp:Button>
                    <asp:Button runat="server" ID="lnkCancelarPlano" CssClass="btn btn-info" OnClick="lnkCancelar_Click" ValidationGroup="CancelarPlano" ClientIDMode="Static" Text="Cancelar Plano" />
                </div>

            </asp:Panel>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel2" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeAtendimentoVip" TargetControlID="hfVariavel2" PopupControlID="pnlAtendimentoVip"
    runat="server">
</AjaxToolkit:ModalPopupExtender>

<style type="text/css">
    input[type="radio"], input[type="checkbox"]{
    float:left !important;
}

</style>