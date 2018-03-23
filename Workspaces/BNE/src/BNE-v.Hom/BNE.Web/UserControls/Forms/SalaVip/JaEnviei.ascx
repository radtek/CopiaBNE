<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JaEnviei.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaVip.JaEnviei" %>
<div class="painel_padrao_sala_vip">
    <p>
        Confira as <span class="texto_vaga" >VAGAS</span> que você já se candidatou..</p>
    <asp:UpdatePanel ID="upGvJaEnviei" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvJaEnviei" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvJaEnviei_PageIndexChanged"
                OnItemCommand="gvJaEnviei_ItemCommand" >
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Filial,Idf_Vaga,Img_Empresa_Confidencial_Visible ">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server" Text='<%# Eval("Dta_Cadastro","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hora" ItemStyle-CssClass="hora">
                            <ItemTemplate>
                                <asp:Label ID="lblHora" runat="server" Text='<%# Eval("Dta_Cadastro","{0:HH:mm}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Função" ItemStyle-CssClass="funcao">
                            <ItemTemplate>
                                <span>
                                    <asp:Literal ID="litCVIndicado" Visible='<%# Convert.ToBoolean(Eval("Flg_Auto_Candidatura")) %>' runat="server"><i class="fa fa-thumbs-up" data-toggle="tooltip" title="CV indicado."></i></asp:Literal>
                                </span>
                                <asp:LinkButton  ID="btiVerVaga" runat="server" CommandName="VerVaga" ToolTip="Ver Vaga" Enabled='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>'
                                    AlternateText="Ver Vaga"><%# Eval("Idf_Deficiencia") != DBNull.Value && Convert.ToInt32(Eval("Idf_Deficiencia")) >0  ? "<i class='fa fa-wheelchair fa-1x ico_pcd'></i>"+  Eval("Des_Funcao") : Eval("Des_Funcao")  %></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Salário" ItemStyle-CssClass="salario">
                            <ItemTemplate>
                                <asp:Label ID="lblSalario" runat="server" Text='<%#  Eval("Vlr_Salario").ToString() != "A combinar" ? Eval("Vlr_Salario") : Eval("MediaSalarial") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="nome_empresa">
                              <ItemTemplate>
                                <asp:LinkButton Visible='<%# Convert.ToBoolean(Eval("Flg_VIP")) %>' ID="btiVerEmpresa" runat="server" ToolTip="Ver Empresa" CommandName="VerEmpresa" AlternateText="Ver Empresa"><%# Eval("Raz_Social") %></asp:LinkButton>
                                <asp:ImageButton ID="imgEmpresaBloqueadaAcessoVIP" Visible='<%# !Convert.ToBoolean(Eval("Flg_VIP")) %>'
                                    runat="server" ImageUrl="~/img/img_nome_empresa_borrado.png" ToolTip="Ver Empresa" CommandName="VerEmpresa" />
                                <div runat="server" Visible='<%# Convert.ToBoolean(Eval("Img_Empresa_Confidencial_Visible")) && Convert.ToBoolean(Eval("Flg_VIP")) %>' class="fa fa-lock fa3"></div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Vagas {2} a {3} de {5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="painel_padrao_sala_vip">
    <p>
         Estas são as <span class="texto_oportunidade">OPORTUNIDADES</span> que você enviou currículo.. <a class="tooltipB balao"><span>Vagas com o processo seletivo já encerrado,
porém a empresa deseja receber mais
currículos para vagas futuras.</span>&nbsp;?&nbsp; </a></p>
   
    <asp:UpdatePanel ID="upGvOportunidade" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvOportunidade" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvOportunidade_PageIndexChanged"
                OnItemCommand="gvJaEnviei_ItemCommand" >
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Filial,Idf_Vaga,Img_Empresa_Confidencial_Visible ">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server" Text='<%# Eval("Dta_Cadastro","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hora" ItemStyle-CssClass="hora">
                            <ItemTemplate>
                                <asp:Label ID="lblHora" runat="server" Text='<%# Eval("Dta_Cadastro","{0:HH:mm}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Função" ItemStyle-CssClass="funcao">
                            <ItemTemplate>
                                <span>
                                    <asp:Literal ID="litCVIndicado" Visible='<%# Convert.ToBoolean(Eval("Flg_Auto_Candidatura")) %>' runat="server"><i class="fa fa-thumbs-up" data-toggle="tooltip" title="CV indicado."></i></asp:Literal>
                                </span>
                                <asp:LinkButton  ID="btiVerVaga" runat="server" CommandName="VerVaga" ToolTip="Ver Vaga" Enabled='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>'
                                     AlternateText="Ver Vaga"><%# Eval("Idf_Deficiencia") != DBNull.Value && Convert.ToInt32(Eval("Idf_Deficiencia")) >0  ? "<i class='fa fa-wheelchair fa-1x ico_pcd'></i>"+  Eval("Des_Funcao") : Eval("Des_Funcao") %></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Salário" ItemStyle-CssClass="salario">
                            <ItemTemplate>
                                <asp:Label ID="lblSalario" runat="server" Text='<%#  Eval("Vlr_Salario").ToString() != "A combinar" ? Eval("Vlr_Salario") : Eval("MediaSalarial") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Segmento da Empresa" ItemStyle-CssClass="nome_empresa">
                              <ItemTemplate>
                                <asp:Label ID="lblArea" runat="server" Text='<%# Eval("Des_Area_BNE") %>' ></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Vagas {2} a {3} de {5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%-- Botões --%>
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
        OnClick="btnVoltar_Click" />
</asp:Panel>
<%-- Fim Botões --%>
