<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="CampanhaRecrutamento.aspx.cs" Inherits="BNE.Web.CampanhaRecrutamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphExperimentos" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CampanhaRecrutamento.css" type="text/css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            autocomplete.funcao('txtFuncao');
            autocomplete.cidade('txtCidade');
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="content_dspc">
        <div class="section_banner">
        </div>
        <div class="section_dados">
            <h3 class="tit_camp">Divulgue sua vaga com uma
                <br>
                <strong>campanha bem sucedida!</strong></h3>
            <p class="sub_camp">
                Defina <strong>quantos currículos deseja receber</strong> e de qual forma
                <br>
                gostaria que os <strong>candidatos retornassem o contato</strong>:
            </p>
            <div class="aside_form">
                <div class="linha">
                    <div class="coluna-esquerda">
                        <asp:Label runat="server" AssociatedControlID="rcbQuantidadeCurriculos" Text="Currículos"></asp:Label>
                        <Componentes:BalaoSaibaMais ID="bsmCurriculo" runat="server" ToolTipText="Selecione o número de retornos que deseja receber nesta Campanha." />
                        <telerik:RadComboBox ID="rcbQuantidadeCurriculos" CssClass="custom" runat="server"></telerik:RadComboBox>
                    </div>
                    <div class="coluna-direita">
                        <asp:Label runat="server" AssociatedControlID="rcbTipoRetorno" Text="Retorno"></asp:Label>
                        <Componentes:BalaoSaibaMais ID="bsmRetorno" runat="server" ToolTipText="<strong>Escolha a opção:</strong> <br><br><strong>1.</strong> Receber ligação- para os candidatos interessados ligarem para você.<br><br> <strong>2.</strong> Inscrição na vaga - para os candidatos interessados realizarem a inscrição na sua vaga." />
                        <telerik:RadComboBox ID="rcbTipoRetorno" CssClass="custom" runat="server"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label runat="server" AssociatedControlID="txtFuncao" Text="Função"></asp:Label>
                    <Componentes:BalaoSaibaMais ID="bsmFuncao" runat="server" ToolTipText="Digite a função do candidato que devem receber sua campanha." />
                    <div class="container-campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFuncao" ValidationGroup="Campanha" runat="server"
                                ControlToValidate="txtFuncao"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvFuncao" runat="server" ErrorMessage="Função Inválida."
                                ClientValidationFunction="autocomplete.validacao.funcao" ControlToValidate="txtFuncao"
                                ValidationGroup="Campanha"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtFuncao" runat="server" Columns="40" CssClass="textbox_padrao funcao"></asp:TextBox>
                    </div>
                </div>
                <div class="linha">
                    <div class="coluna-esquerda">
                        <asp:Label runat="server" AssociatedControlID="txtSalario" Text="Salário"></asp:Label>
                        <Componentes:BalaoSaibaMais ID="bsmSalario" runat="server" ToolTipText="Digite o salário ofertado na sua vaga, a partir dele sua campanha será divulgada para candidatos próximos desta pretensão salarial." />
                        <componente:ValorDecimal ID="txtSalario" Obrigatorio="true" CssClassTextBox="textbox_padrao salario" runat="server" CasasDecimais="2" ValidationGroup="Campanha" />
                    </div>
                    <div class="coluna-direita">
                        <asp:Label runat="server" AssociatedControlID="txtCidade" Text="Cidade/Estado"></asp:Label>
                        <Componentes:BalaoSaibaMais ID="bsmCidade" runat="server" ToolTipText="Digite a cidade dos candidatos que devem receber sua campanha." />
                        <div class="container-campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfCidade" ValidationGroup="Campanha" runat="server"
                                    ControlToValidate="txtCidade"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inválida."
                                    ClientValidationFunction="autocomplete.validacao.cidade" ControlToValidate="txtCidade"
                                    ValidationGroup="Campanha"></asp:CustomValidator>
                            </div>
                            <asp:TextBox ID="txtCidade" runat="server" CssClass="textbox_padrao cidade"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <asp:LinkButton ID="btnDispararCampanha" runat="server" class="disparar_btn" OnClick="btnDispararCampanha_Click" ValidationGroup="Campanha" Text="Disparar Campanha!" />
            <asp:Panel runat="server" ID="pnlSaldo" Visible="False">
                <p class="qnt_camp">
                    Você ainda tem <strong class="quant_txt">
                        <asp:Literal ID="litSaldo" runat="server"></asp:Literal></strong> campanhas gratuitas!
                </p>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
    <%--<Employer:DynamicScript runat="server" Src="/js/local/CampanhaRecrutamento.js" type="text/javascript" />--%>
</asp:Content>
