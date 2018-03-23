<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="Erros.aspx.cs"
    Inherits="BNE.Web.Erros.Erros" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1"
    runat="server">
    <telerik:RadScriptManager
        ID="rm"
        runat="server">
    </telerik:RadScriptManager>
    <asp:UpdatePanel
        ID="upBotoes"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button
                ID="btnLimparPasta"
                runat="server"
                Text="Limpar Arquivos"
                OnClick="btnLimparPasta_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel
        ID="upErros"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label
                ID="lblQuantidadeRegistros"
                runat="server"
                CssClass="label_principal"
                Text="(x) arquivos"></asp:Label>
            <telerik:RadGrid
                ID="gvErros"
                runat="server"
                OnDeleteCommand="gvErros_DeleteCommand">
                <MasterTableView
                    DataKeyNames="arquivo"
                    AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridBoundColumn
                            HeaderText="Data"
                            DataField="Data">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="Usuário"
                            DataField="Usuario">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="Código"
                            DataField="Codigo">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="Mensagem"
                            DataField="Mensagem">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="StackTrace"
                            DataField="StackTrace">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="InnerException"
                            DataField="InnerException">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="HelpLink"
                            DataField="HelpLink">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn
                            HeaderText="Source"
                            DataField="Source">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn
                            HeaderText="Excluir"
                            HeaderStyle-Width="10px">
                            <ItemTemplate>
                                <asp:ImageButton
                                    ID="btiExcluir"
                                    runat="server"
                                    AlternateText="Excluir"
                                    CausesValidation="False"
                                    CommandName="Delete"
                                    ImageUrl="~/img/icone_excluir_16x16.png" />
                            </ItemTemplate>
                            <HeaderStyle
                                CssClass="rgHeader centro coluna_icones" />
                            <ItemStyle
                                CssClass="espaco_icones centro" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
