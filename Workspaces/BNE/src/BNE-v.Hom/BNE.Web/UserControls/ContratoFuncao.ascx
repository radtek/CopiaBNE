<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContratoFuncao.ascx.cs" Inherits="BNE.Web.UserControls.ContratoFuncao" %>
<%--JS--%>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/TipoContratoFuncao.js" type="text/javascript" />
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
        autocomplete.funcao("txtFuncaoAnunciarVaga");
        if ($('input[name*=btnCurso').val() == null) {
            autocomplete.curso("txtCursoAnunciarVaga");
            $('#div_AnuncioVagasEstagio').hide();
        } else {
            autocomplete.curso("txtCursoAnunciarVaga", btnAdicionarCursoClick);
        }
    }
    $(document).ready(function () {
        carregarCursosFromHiddenField();
    });
</script>
<div class="linha">
    <asp:Label ID="lblTipoContrato" CssClass="label_principal" Text="Tipo de Contrato" AssociatedControlID="rblContrato" runat="server"></asp:Label>
    <div class="container_campo">
        <asp:RadioButtonList ID="rblContrato" RepeatDirection="Horizontal"
            AutoPostBack="true" RepeatColumns="3" runat="server" OnSelectedIndexChanged="rblContrato_SelectedIndexChanged" />
    </div>
</div>
<%-- Linha alerta do Checked Box Estágio --%>
<div id="div_AnuncioVagasEstagio" class="linha lineAdjust">
    <div class="alert fade in">
        <!-- <button type="button" class="close" data-dismiss="alert">×</button>-->
        Informe abaixo os <strong>cursos</strong> que o estagiário deve estar <strong>cursando</strong>.
    </div>
</div>
<asp:UpdatePanel ID="upPanelFuncaoCursos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- FIM: alerta do Checked Box Estágio --%>
        <div class="linha" runat="server" id="divFuncao">
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
                    </div>
                </div>
            </div>
        </div>
        <div class="linha" runat="server" id="divCursos" visible="false">
            <asp:Label ID="lblCurso" CssClass="label_principal" AssociatedControlID="txtCursoAnunciarVaga"
                runat="server" Text="Curso"></asp:Label>
            <div class="container_campo">
                <div>
                    <div style="float: left;">
                        <asp:TextBox ID="txtCursoAnunciarVaga" CssClass="textbox_padrao" runat="server" AutoPostBack="false"></asp:TextBox>
                        <asp:Button ID="btnCurso" CssClass="botao_padrao botao_curso" CausesValidation="false"
                            runat="server" Text="Adicionar Curso" OnClientClick="return btnAdicionarCursoClick();" />
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdfListaCursos" runat="server" Value="[]"></asp:HiddenField>
        <div class="linha_cursos" style="margin-left: 180px" runat="server" id="divCursosAceitos" visible="false">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
