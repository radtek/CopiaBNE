<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="MenuSalaAdministrador.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.MenuSalaAdministrador" %>
<div class="menu_ss">
    <%-- Coluna 01 do Menu da Tela Selecionadora --%>
    <div class="col01">
        <asp:LinkButton ID="lnkEditarCurriculo" runat="server" CausesValidation="false" ToolTip="Currículos"
            OnClick="lnkEditarCurriculo_Click">
            <div class="btn_col">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <i class="fa fa-file-o fa-5x pull-left"></i>
                        <div class="titulo_texto_btn menor">
                            <span class="tit_btn_ss">Currículos</span> <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li id="liVagasAnunciadas" runat="server">Alteração de dados</li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="lnkFinanceiro" runat="server" CausesValidation="false" ToolTip="Financeiro"
            OnClick="lnkFinanceiro_Click">
            <div class="btn_col bottom">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <i class="fa fa-usd fa-5x pull-left"></i>
                        <div class="titulo_texto_btn menor">
                            <span class="tit_btn_ss">
                                Financeiro
                            </span>                            
                            <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>
                                        Planos, Liberações e Pagamentos
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
    </div>
    <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
    <%-- Coluna 02 do Menu da Tela Selecionadora --%>
    <div class="col02">
        <asp:LinkButton ID="lnkEditarEmpresas" runat="server" CausesValidation="false" ToolTip="Empresas"
            OnClick="lnkEditarEmpresas_Click">
            <div class="btn_col">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <i class="fa fa-building-o fa-5x pull-left"></i>
                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">
                               Empresas</span>
                            <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>Auditoria e Edição
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="lnkConfiguracoes" runat="server" CausesValidation="false" ToolTip="Configurações"
            OnClick="lnkConfiguracoes_Click">
            <div class="btn_col bottom"
                title="Configurações">
                <div class="fundo_btn_ss">
                    <div class="sombra_btn_ss">
                        <i class="fa fa-cogs fa-5x pull-left"></i>
                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">
                                Configurações</span>
                            <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>
                                       E-mail, Cartas, Valores e Permissões
                                    </li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
    </div>
    <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
    <%-- Coluna 03 do Menu da Tela Selecionadora --%>
    <div class="col03">
        <asp:LinkButton ID="lnkEditarVagas" runat="server" CausesValidation="false" ToolTip="Vagas"
            OnClick="lnkEditarVagas_Click">
            <div class="btn_col"
                title="Vagas">
                <div class="fundo_btn_ss_g">
                    <div class="sombra_btn_ss_g">
                        <i class="fa fa-bullhorn fa-5x pull-left"></i>
                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">
                                Vagas</span>
                            <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>Auditoria e Edição</li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="lnkRelatorios" runat="server" CausesValidation="false" ToolTip="Relatórios"
            OnClick="lnkRelatorios_Click">
            <div class="btn_col bottom"
                title="Relatórios">
                <div class="fundo_btn_ss_g">
                    <div class="sombra_btn_ss_g">
                        <i class="fa fa-pie-chart fa-5x pull-left"></i>
                        <div class="titulo_texto_btn">
                            <span class="tit_btn_ss">
                                Relatórios</span>
                            <span class="texto_btn_ss">
                                <ul class="item_btn">
                                    <li>Analíticos, Sintéticos e Cubos</li>
                                </ul>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:LinkButton>
    </div>
    <%-- Fim da Coluna 03 do Menu da Tela Selecionadora --%>
</div>
