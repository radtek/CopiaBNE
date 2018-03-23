<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IdiomaEmPesquisaCurriculo.ascx.cs" Inherits="BNE.Web.UserControls.IdiomaEmPesquisaCurriculo" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/TipoContratoFuncao.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PesquisaCurriculoFuncao.js" type="text/javascript" />


    
   
    <asp:Label ID="lblFuncao" CssClass="label_principal-set" AssociatedControlID="rcbIdioma"  runat="server"><strong>Idioma</strong></asp:Label>
    <Componentes:BalaoSaibaMais ID="bsmChkIdioma" runat="server" CssClassLabel="balao_saiba_mais" ToolTipTitle="Idioma:" Text="Saiba mais" ToolTipText="Se você procura candidatos com idiomas. Selecione nesse campo uma ou mais opções e tenha apenas os candidatos com os idiomas escolhidos." ShowOnMouseover="true" />   
                                                        <div class="clearfix"></div>
      
            <div style="float: left;">
              
                        <asp:UpdatePanel ID="upDDL" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                            <telerik:RadComboBox id="rcbIdioma" runat="server"  ></telerik:RadComboBox>
                            <telerik:RadComboBox id="rcbNivel" runat="server" ></telerik:RadComboBox>
                                <asp:Button runat="server" ID="btnAdicionar" OnClick="btnAdicionar_Click" class="adicionar_alert_idioma" Text="+"></asp:Button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                       <asp:UpdatePanel ID="upIdiomas" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:Repeater ID="rptIdioma" runat="server" OnItemCommand="repeater_ItemCommand">
                            <HeaderTemplate>
                                <ul id="gridFuncoes" class="GridItems">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li value="<%# String.Format("{0}{1}",DataBinder.Eval(Container.DataItem, "idIdioma"), DataBinder.Eval(Container.DataItem,"idNivel"))  %>" >
                                    <%# String.Format("{0} {1}", DataBinder.Eval(Container.DataItem, "DescricaoIdioma"), (DataBinder.Eval(Container.DataItem, "DescricaoNivel").Equals("Selecione") ? "" : "Nivel " + DataBinder.Eval(Container.DataItem, "DescricaoNivel"))) %>
                                    <asp:LinkButton ID="lnkDeletarFuncao" runat="server" CommandName="deletarFuncao" CommandArgument='<%# String.Format("{0}{1}",DataBinder.Eval(Container.DataItem, "idIdioma"), DataBinder.Eval(Container.DataItem,"idNivel")) %>' BorderStyle="None">
                                                <img class="fechar_img" src="img/pacote_alertaVaga/btn_excluirnew.png" alt="" />
                                    </asp:LinkButton>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        
                        <%-- FIM: Grid Cidade --%>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
         
      
   



