<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FuncaoEmPesquisaCurriculo.ascx.cs" Inherits="BNE.Web.UserControls.FuncaoEmPesquisaCurriculo" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/TipoContratoFuncao.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PesquisaCurriculoFuncao.js" type="text/javascript" />
<script type="text/javascript">
    function pageLoad() {
        if (typeof bindEvents == 'function')
            bindEvents();

        var textBox = $("input:text[name*='txtFuncaoPretendida']");
        if (textBox == null || $(textBox).length == 0)
            return;

        var handler = function () {
            if (textBox.val().toString().trim() === "") {
                var emptyValidator = $("#<%=rfFuncao.ClientID%>");
                emptyValidator.show();
                textBox.unbind('.primeiraValidacao');
            }
        };

        textBox.bind('focusout.primeiraValidacao', handler);

        autocomplete.funcao("txtFuncaoPretendida");
    }
</script>
<div class="linha_funcao">
    <asp:Label ID="lblFuncao" CssClass="label_principal-set" AssociatedControlID="txtFuncaoPretendida" runat="server"><strong>Função</strong></asp:Label>
    <div class="clearfix"></div>
    <div class="container_campo">
        <div>
            <asp:RequiredFieldValidator ID="rfFuncao" runat="server"
                ControlToValidate="txtFuncaoPretendida" EnableClientScript="True"></asp:RequiredFieldValidator>

            <%-- Mensagens de error estão no java script--%>
            <asp:CustomValidator ID="cvFuncaoAnunciarVaga" runat="server" OnServerValidate="cvFuncaoAnunciarVaga_OnServerValidate"
                ControlToValidate="txtFuncaoPretendida" EnableClientScript="True">
            </asp:CustomValidator>
        </div>
        <div>
            <div style="float: left;">
                <asp:UpdatePanel ID="upTxtCidade" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CustomValidator ID="cvFuncaoPretendida" runat="server" ControlToValidate="txtFuncaoPretendida"
                            ErrorMessage="Função Inválida" ValidationGroup="ValidarFuncao"></asp:CustomValidator>
                        <asp:TextBox ID="txtFuncaoPretendida" runat="server" Columns="40"
                            CssClass="campo_obrigatorio dados_alert" placeholder="Digite a função"
                            CausesValidation="False" OnTextChanged="OnTextChanged"></asp:TextBox>
                        <button class="adicionar_alert">+</button>
                        <p class="inf_alertav">Para <strong>remover as funções desejadas</strong>, basta clicar no <strong>“X”</strong> ao lado do nome.</p>
                        <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida" runat="server" TargetControlID="txtFuncaoPretendida"
                            UseContextKey="True" ServiceMethod="ListarFuncoesComId">
                        </AjaxToolkit:AutoCompleteExtender>
                        <asp:Repeater ID="rptFuncoes" runat="server" OnItemCommand="repeater_ItemCommand">
                            <HeaderTemplate>
                                <ul id="gridFuncoes" class="GridItems">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li value="<%# DataBinder.Eval(Container.DataItem,"IdFuncao")%>" class="<%# DataBinder.Eval(Container.DataItem,"class")%>">
                                    <%# DataBinder.Eval(Container.DataItem, "DescricaoFuncao")%>
                                    <asp:LinkButton ID="lnkDeletarFuncao" runat="server" CommandName="deletarFuncao" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdFuncao") %>' BorderStyle="None">
                                                <img class="fechar_img" src="img/pacote_alertaVaga/btn_excluirnew.png" alt="" />
                                    </asp:LinkButton>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:HiddenField ID="FuncoesSel" runat="server" Value="" />
                        <%-- FIM: Grid Cidade --%>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div style="float: left;">
                <div id="box_tipo_contrato" class="box_ajuda_tipo_contrato">
                    <div class="inner_box_estagiario">
                        <%--<asp:Image ID="imgBoxContrato" CssClass="img_box_estagiario" runat="server" ImageUrl="~\img\aviso_empresa_estagiario_pesquisa.png" />--%>
                        <asp:Label runat="server" Visible="false"><small><i class="fa fa-reply fa-2x"></i>ESCOLHA A FUNÇÃO DO ESTAGIÁRIO</small></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="linha_funcao" id="container_checkbox">
    <asp:CheckBox runat="server" CssClass="checkbox_quero_estagiarios" ID="chbEstagiario" Text="" />
    <asp:Label ID="lblAvisoEstagio" AssociatedControlID="txtFuncaoPretendida" runat="server" Text="Quero Contratar Estagiários" CssClass="label_quero_estagiarios"></asp:Label>
    <asp:CheckBox runat="server" CssClass="checkbox_quero_estagiarios" ID="chbAprendiz" Text="" Visible="False" />
    <div class="container_campo">
        <asp:Label ID="lblAvisoAprendiz" AssociatedControlID="txtFuncaoPretendida"
            runat="server" Text="Quero Contratar Aprendiz" CssClass="label_quero_estagiarios" Visible="False"></asp:Label>
    </div>
</div>
