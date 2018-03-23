<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NoticiasEditar.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.NoticiasEditar" %>
<div class="painel_configuracao_conteudo">
    <h3 class="titulo_configuracao_content">
        <asp:Literal ID="litModoTela" runat="server"></asp:Literal></h3>
    <%--Linha Titulo ----%>
    <div class="linha titulo_noticia">
        <div class="container_campo editar">
            <asp:Label ID="lblConfirmacaoCadastroCurriculo" runat="server" Text="Título da Notícia"
                CssClass="label_principal  titulo" />
            <componente:AlfaNumerico ID="txtTitulo" runat="server" CssClass="titulo_input"
                ValidationGroup="SalvarNoticia" />
        </div>
    </div>
    <%--FIM Titulo --%>

        <telerik:RadEditor CssClass="radeditor_boas_vindas_candidato noticias_editar" Height="150px" ID="reDescricao"
            runat="server" SkinID="RadEditorControlesBasicos" Width="618px">
        </telerik:RadEditor>

    <div class="form_menor">
        <%--Linha Data de Publicação --%>
        <div class="linha">
            <asp:Label ID="lblDataPublicacao" runat="server" Text="Data de Publicação" CssClass="label_principal texto_normal" />
            <div class="container_campo texto_normal">
                <componente:Data ID="txtDataPublicacao" runat="server" 
                    ValidationGroup="SalvarNoticia" />
            </div>
        </div>
        <%--FIM Data de Publicação --%>
        <%--Linha Em Exibição--%>
        <div class="linha">
            <asp:Label ID="lblEmexibicao" runat="server" Text="Em Exibição" CssClass="label_principal texto_normal " />
            <div class="container_campo texto_normal">
                <span>
                    <asp:RadioButton ID="rdbSim" runat="server" Text="Sim" GroupName="Exibicao" /></span>
                <span>
                    <asp:RadioButton ID="rdbNao" runat="server" Text="Não" GroupName="Exibicao" /></span>
            </div>
        </div>
        <%--FIM Em Exibição --%>
    </div>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="true" ValidationGroup="SalvarNoticia" OnClick="btnSalvar_Click" />
    </asp:Panel>
</div>
