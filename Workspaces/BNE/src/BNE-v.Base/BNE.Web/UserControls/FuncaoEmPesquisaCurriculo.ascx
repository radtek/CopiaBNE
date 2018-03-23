<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FuncaoEmPesquisaCurriculo.ascx.cs" Inherits="BNE.Web.UserControls.FuncaoEmPesquisaCurriculo" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/TipoContratoFuncao.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PesquisaCurriculoFuncao.js" type="text/javascript" />
<script type="text/javascript">
    function pageLoad() {
        if (typeof bindEvents == 'function')
            bindEvents();

        var textBox = $("input:text[name*='txtFuncaoAnunciarVaga']");
        if (textBox == null || $(textBox).length == 0)
            return;

        var handler = function () {
            if (textBox.val().toString().trim() == "") {
                var emptyValidator = $("#<%=rfFuncao.ClientID%>");
                emptyValidator.show();
                textBox.unbind('.primeiraValidacao');
            }
        };

        textBox.bind('focusout.primeiraValidacao', handler);

    }
</script>

<div class="linha_funcao" id="container_checkbox">
    <asp:CheckBox runat="server" CssClass="checkbox_quero_estagiarios" ID="chbEstagiario" Text="" />
    <div class="container_campo">
        <asp:Label ID="lblAvisoEstagio" AssociatedControlID="txtFuncaoAnunciarVaga"
            runat="server" Text="Quero Contratar Estagiários" CssClass="label_quero_estagiarios"></asp:Label>
    </div>
    <asp:CheckBox runat="server" CssClass="checkbox_quero_estagiarios" ID="chbAprendiz" Text="" Visible="False"/>
    <div class="container_campo">
        <asp:Label ID="lblAvisoAprendiz" AssociatedControlID="txtFuncaoAnunciarVaga"
            runat="server" Text="Quero Contratar Aprendiz" CssClass="label_quero_estagiarios" Visible="False"></asp:Label>
    </div>
</div>

<div class="linha_funcao">
    <asp:Label ID="lblFuncao" CssClass="label_principal" AssociatedControlID="txtFuncaoAnunciarVaga"
        runat="server" Text="Função"></asp:Label>

    <div class="container_campo">
        <div>
            <asp:RequiredFieldValidator ID="rfFuncao" runat="server"
                ControlToValidate="txtFuncaoAnunciarVaga" EnableClientScript="True"></asp:RequiredFieldValidator>

            <%-- Mensagens de error estão no java script--%>
            <asp:CustomValidator ID="cvFuncaoAnunciarVaga" runat="server" OnServerValidate="cvFuncaoAnunciarVaga_OnServerValidate"
                ControlToValidate="txtFuncaoAnunciarVaga" EnableClientScript="True">
            </asp:CustomValidator>
        </div>

        <div>
            <div style="float: left;">
                <asp:TextBox ID="txtFuncaoAnunciarVaga" runat="server" OnTextChanged="txtFuncaoAnunciarVaga_TextChanged"></asp:TextBox>
                <AjaxToolkit:AutoCompleteExtender ID="aceFuncao" runat="server" TargetControlID="txtFuncaoAnunciarVaga"
                    UseContextKey="True" ServiceMethod="ListarFuncoes">
                </AjaxToolkit:AutoCompleteExtender>
            </div>
            <div style="float: left;">
                <div id="box_tipo_contrato" class="box_ajuda_tipo_contrato">
                    <div class="inner_box_estagiario">
                        <asp:Image ID="imgBoxContrato" CssClass="img_box_estagiario" runat="server" ImageUrl="~\img\aviso_empresa_estagiario_pesquisa.png" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
