<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="AlertaVagasFormulario2.aspx.cs" Inherits="BNE.Web.AlertaVagasFormulario2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link rel="Stylesheet" type="text/css" href="/css/local/AlertaVagasFormulario2.css" />
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/local/AlertaVagasFormulario2.js" ></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
<div class="painel_padrao">
       <h1><span class="destaque">Alerta de Vagas</span></h1>
            <div class="container_alertaVaga">
            <div class="controlador_detalhes">
            <span class="label_tituloAlerta">Alerta de Vaga</span>
            </div>
                  <%--Background Businessman Image--%>
                  <div class="container_avatar">
                    <div class="body1"></div><div class="body2"></div>
                  <%--Balão aparece com seus respectivos textos só qdo os campos forem focados--%>
                    <div class="balao"></div>
                  </div>
                  <%--FIM: Background Businessman Image--%>
                   <%--Textos do Balão--%>
                   <%--Obs*** Este texto aparece com o balão qdo o foco for salário***--%>
                   <div class="textoSalario">
                        <span class="chamadaStrong">Hummm!!!</span><br/>
                        Só quero receber<br/>
                        <b>Alerta de Vagas</b> com<br/>
                        salário acima de...
                        
                  </div>
                   <%--Obs***Este texto aparece com o balão qdo o foco for não quero trabalhar em tal empresa***--%>
                        <div class="textoNaoQueroEmpresa">
                        Digite o nome de <br/><b>até 3 empresas</b> que<br/>
                        você não quer receber<br/>
                        <b>Alerta de Vagas</b>!
                        </div>
                  <%--FIM:Textos do Balão--%>
            
            
            <div class="controlador_detalhes2">
            <div class="label_textoParagrafo">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec 
            dignissim in massa dignissim ultrices. Pellentesque quis accumsan
            erat. Suspendisse varius vitae nulla et euismod.
            </div>
            </div>
            <%-- Linha Faixa Salarial --%>
            <div class="linha">
                <asp:Label ID="lblSalario" CssClass="label_principal" Text="Acima de" AssociatedControlID="txtSalarioDe"
                    runat="server"></asp:Label>
                <div class="container_campo">
                    <asp:UpdatePanel ID="upFaixaSalarial" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <componente:ValorDecimal ID="txtSalarioDe" Obrigatorio="True" runat="server" CasasDecimais="2"
                                ValidationGroup="SimularR1" />                                                       
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <%--FIM: Linha Faixa Salarial--%>
            
            <%--Linha Empresas que não quero trabalhar --%>
            <div class="linha">
                <div class="label_empresasNaoQueroTrabalhar">
                Não quero trabalhar nas empresas com esse nome:
                </div> 
            <%-- Apenas 3 empresas serão aceitas, não mais do que isto--%>   
                <div class="container_campo">
                     <asp:TextBox ID="TextBox1" runat="server" MaxLength="50" Columns="50" 
                     CssClass="balaoEmpresas textbox_padrao detalheLarguraCampoFieldEmpresa"></asp:TextBox>
                </div>
                <div class="container_campo">
                     <asp:TextBox ID="TextBox2" runat="server" MaxLength="50" Columns="50" 
                     CssClass="balaoEmpresas textbox_padrao detalheLarguraCampoFieldEmpresa"></asp:TextBox>
                </div>
                <div class="container_campo margem_bottom">
                     <asp:TextBox ID="TextBox3" runat="server" MaxLength="50" Columns="50" 
                     CssClass="balaoEmpresas textbox_padrao detalheLarguraCampoFieldEmpresa"></asp:TextBox>
                </div>
            </div>
            <%-- FIM: Linha Empresas que não quero trabalhar --%>
            <%-- Linha Grid Empresas Nao Quero Trabalhar --%>
                <div class="linha">
                <div class="GridEmpresasNaoQueroTrabalhar">
                Empresa Atrás dos Montes Uivantes Ltda
                </div>
                <div class="container_btnDeletarEmpresaGrid">
                <asp:HyperLink ID="HyperLink2" runat="server" Text="" CssClass="btnDeletarEmpresaGrid"></asp:HyperLink>
                </div>
                </div>                      
            <%-- FIM: Grid Empresas Nao QueroTrabalhar --%>
            </div>
            
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
