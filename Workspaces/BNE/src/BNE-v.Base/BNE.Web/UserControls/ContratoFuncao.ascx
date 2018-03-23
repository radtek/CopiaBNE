<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContratoFuncao.ascx.cs" Inherits="BNE.Web.UserControls.ContratoFuncao" %>
<%--JS--%>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/TipoContratoFuncao.js" type="text/javascript" />
<script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js" type="text/javascript"></script>
<%--CSS--%>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/TipoContratoFuncao.css" type="text/css" rel="stylesheet" />
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
<div class="linha">
    <asp:Label ID="lblTipoContrato" CssClass="label_principal" Text="Tipo de Contrato" AssociatedControlID="chblContrato" runat="server"></asp:Label>
    <div class="container_campo">
        <asp:CheckBoxList ID="chblContrato" RepeatDirection="Horizontal" RepeatColumns="3" runat="server" />
    </div>
</div>
<%-- Linha alerta do Checked Box Estágio --%>
<div id="div_AnuncioVagasEstagio" class="linha lineAdjust">
    <div class="alert fade in">
        <!-- <button type="button" class="close" data-dismiss="alert">×</button>-->
        Informe abaixo <strong>a função</strong>  que o estagiário irá<strong> desempenhar</strong>.
                            <p><em><strong>- Exemplo: </strong>Auxiliar Administrativo.</em></p>
    </div>
</div>
<%-- FIM: alerta do Checked Box Estágio --%>
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
                <asp:TextBox ID="txtFuncaoAnunciarVaga" runat="server" OnTextChanged="txtFuncaoAnunciarVaga_TextChanged" AutoPostBack="true"></asp:TextBox>
                <AjaxToolkit:AutoCompleteExtender ID="aceFuncao" runat="server" TargetControlID="txtFuncaoAnunciarVaga"
                    UseContextKey="True" ServiceMethod="ListarFuncoes">
                </AjaxToolkit:AutoCompleteExtender>
            </div>
            <div style="float: left;">
                <div id="box_tipo_contrato" class="box_ajuda_tipo_contrato">
                    <div class="inner_box_tipo_contrato">
                        <asp:Image ID="imgBoxContrato" CssClass="img_box_tipo_contrato" runat="server" />
                        <%--<div class="seta_apontador_esq">
            </div>
            <div class="box_conteudo sugestao">
                <label class="label_principal" id="cphConteudo_lblSugestaoTarefasAnunciarVaga">Novidade</label>
                <div class="container_campo">
                    <asp:TextBox ID="TextBox1" CssClass="textbox_padrao sugestao_tarefas"
                        TextMode="MultiLine" ReadOnly="true" runat="server" Text="Agora você deve definir uma função para o Estagiário ou Aprendiz."></asp:TextBox>
                </div>
            </div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
