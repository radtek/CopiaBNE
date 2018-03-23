<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipCursos.aspx.cs" Inherits="BNE.Web.SalaVipCursos" %>

<%@ Register Src="UserControls/ucCurso.ascx" TagName="ucCurso" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipCursos.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;
        </div>
        <div class="cursos">
            <asp:Panel runat="server" ID="pnlCursosEspecificos" CssClass="container_cursos">
                <h2>
                    <asp:Literal runat="server" ID="litCursos"></asp:Literal>
                </h2>
                <asp:Repeater runat="server" ID="rptCursosEspecificos" OnItemCommand="rptCursos_ItemCommand">
                    <ItemTemplate>
                        <uc1:ucCurso ID="ucCurso" runat="server" ImageURLMiniatura='<%# Eval("Des_Caminho_Imagem_Miniatura")%>'
                            TituloCurso='<%# Eval("Des_Titulo_Curso")%>' CargaHoraria='<%# Eval("Qtd_Carga_Horaria")%>'
                            Valor='<%# Eval("Vlr_Curso")%>' MostrarParcela='<%# !String.IsNullOrEmpty(Eval("Qtd_Parcela").ToString()) %>'
                            QuantidadeParcela='<%# Eval("Qtd_Parcela")%>' ValorParcela='<%# Eval("Vlr_Curso_Parcela")%>'
                            IdCurso='<%# Eval("Idf_Curso_Parceiro_Tecla") %>' />
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlOutrosCursos" Visible="False" CssClass="container_cursos">
                <h2>
                    Outros cursos</h2>
                <asp:Repeater runat="server" ID="rptOutrosCursos" OnItemCommand="rptCursos_ItemCommand">
                    <ItemTemplate>
                        <uc1:ucCurso ID="ucCurso" runat="server" ImageURLMiniatura='<%# Eval("Des_Caminho_Imagem_Miniatura")%>'
                            TituloCurso='<%# Eval("Des_Titulo_Curso")%>' CargaHoraria='<%# Eval("Qtd_Carga_Horaria")%>'
                            Valor='<%# Eval("Vlr_Curso")%>' MostrarParcela='<%# !String.IsNullOrEmpty(Eval("Qtd_Parcela").ToString()) %>'
                            QuantidadeParcela='<%# Eval("Qtd_Parcela")%>' ValorParcela='<%# Eval("Vlr_Curso_Parcela")%>'
                            IdCurso='<%# Eval("Idf_Curso_Parceiro_Tecla") %>' />
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
