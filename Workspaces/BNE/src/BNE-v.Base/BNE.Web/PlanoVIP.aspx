<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="PlanoVIP.aspx.cs" Inherits="BNE.Web.PlanoVIP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PlanoVIP.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        function destacaValor() {
            $('.preco_desconto_mensal,.preco_desconto_tri').hide().fadeIn('slow');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;</div>
                <asp:UpdatePanel runat="server" ID="upCodigoCredito">
                    <ContentTemplate>
                       
                        <div style="text-align:center">
                            <asp:Panel runat="server" ID="panelPlanos">
                                <asp:Panel ID="panelPlanoMensal" runat="server" CssClass="box_painel">
                                    <div class="box_plano">
                                        <asp:Label runat="server" ID="lblPlanoMensal" AssociatedControlID="rbPlanoMensal"
                                            CssClass="mensal">
                                            <div class="preco_sem_desconto_mensal">
                                                <asp:Literal runat="server" ID="litPrecoSemDescontoMensal"></asp:Literal>
                                            </div>
                                            <div class="preco_desconto_mensal">
                                                <asp:Literal runat="server" ID="litPrecoDescontoMensal"></asp:Literal>
                                            </div>
                                        </asp:Label>
                                        <asp:RadioButton runat="server" ID="rbPlanoMensal" GroupName="Plano" Checked="false" CssClass="radiobutton_padrao" Text="Escolher mensal">
                                        </asp:RadioButton>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panelOu" runat="server" CssClass="box_painel_ou">
                                    <div class="label_ou">
                                        OU
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panelPlanoTri" runat="server" CssClass="box_painel">
                                    <div class="box_plano">
                                        <asp:Label runat="server" ID="lblPlanoTri" AssociatedControlID="rbPlanoTri" CssClass="trimestral">
                                            <div class="preco_sem_desconto_tri">
                                                <asp:Literal runat="server" ID="litPrecoSemDescontoTri"></asp:Literal>
                                            </div>
                                            <div class="preco_desconto_tri">
                                                <asp:Literal runat="server" ID="litPrecoDescontoTri"></asp:Literal>
                                            </div>
                                        </asp:Label>
                                        <asp:RadioButton runat="server" ID="rbPlanoTri" GroupName="Plano" Checked="false" CssClass="radiobutton_padrao" Text="Escolher trimestral">
                                        </asp:RadioButton>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="panelPlanoPromocional" runat="server" CssClass="box_painel promo" Visible="false">
                                <asp:Image runat="server" ID="imgCodigoPromocional" ImageUrl="~/img/CodigoPromocional/9.jpg" AlternateText="Promoção BNE"/>
                                <asp:RadioButton runat="server" ID="rbPlanoPromocional" GroupName="Plano" Checked="false" CssClass="radiobutton_padrao" Text="Plano Promocional" Enabled="false">
                                </asp:RadioButton>
                            </asp:Panel>
                        </div>
                        <div class="containerIsenrirCupom">
                            <div class="groupAll">
                                <asp:Label ID="lblCodigoCredito" AssociatedControlID="txtCodigoCredito" Text="Código promocional:" 
                                runat="server">
                            
                                <asp:TextBox ID="txtCodigoCredito" runat="server" Columns="40" MaxLength="200" 
                                    CssClass="textbox_padrao complementaTextbox" AutoPostBack="true" ontextchanged="txtCodigoCredito_TextChanged"></asp:TextBox>
                          &nbsp;<asp:Button ID="btnValidarCodigoCredito" runat="server" CssClass="buttonStyle" Text="Aplicar" 
                                    onclick="btnValidarCodigoCredito_Click" />
                                </asp:Label>
                            </div>
                        </div>
                        <script type="text/javascript">
                            Sys.Application.add_load(destacaValor);
                        </script>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="btn_continuar_cinza">
                    <asp:UpdatePanel runat="server" ID="upBtnContinuar">
                        <ContentTemplate>
                            <asp:ImageButton ID="btnContinuar" runat="server" CausesValidation="false" OnClick="btnContinuar_Click"
                                ImageUrl="/img/pacotes_bne/btn_continuar_cinza.png" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
