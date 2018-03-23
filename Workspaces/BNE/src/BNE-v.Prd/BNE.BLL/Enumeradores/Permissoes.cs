namespace BNE.BLL.Enumeradores
{
    public sealed class Permissoes
    {

        #region CategoriaPermissao.PesquisaAvancadaCurriculo
        [CategoriaPermissaoAttribute(CategoriaPermissao.PesquisaAvancadaCurriculo)]
        public enum PesquisaAvancadaCurriculo
        {
            AcessarTelaPesquisaAvancadaCurriculo = 1
        }
        #endregion

        #region CategoriaPermissao.EnviarMensagem
        [CategoriaPermissaoAttribute(CategoriaPermissao.EnviarMensagem)]
        public enum EnviarMensagem
        {
            AcessarTelaEnviarMensagem = 2
        }
        #endregion

        #region CategoriaPermissao.EnviarCurriculo
        [CategoriaPermissaoAttribute(CategoriaPermissao.EnviarCurriculo)]
        public enum EnviarCurriculo
        {
            AcessarTelaEnviarCurriculo = 3
        }
        #endregion

        #region CategoriaPermissao.SolicitacaoR1
        [CategoriaPermissaoAttribute(CategoriaPermissao.SolicitacaoR1)]
        public enum SolicitacaoR1
        {
            AcessarTelaEnviarSolicitacaoR1 = 4
        }
        #endregion

        #region CategoriaPermissao.ManterVaga
        [CategoriaPermissaoAttribute(CategoriaPermissao.ManterVaga)]
        public enum ManterVaga
        {
            AcessarTelaAnunciarVaga = 5,
            PublicarVaga = 33
        }
        #endregion

        #region CategoriaPermissao.CompraPlanoEmpresa
        [CategoriaPermissaoAttribute(CategoriaPermissao.CompraPlanoEmpresa)]
        public enum CompraPlanoEmpresa
        {
            AcessarTelaCompraPlanoEmpresa = 6
        }
        #endregion

        #region CategoriaPermissao.CompraPlanoVIP
        [CategoriaPermissaoAttribute(CategoriaPermissao.CompraPlanoVIP)]
        public enum CompraPlanoVIP
        {
            AcessarTelaCompraPlanoVIP = 7
        }
        #endregion

        #region CategoriaPermissao.SalaVIP
        [CategoriaPermissaoAttribute(CategoriaPermissao.SalaVIP)]
        public enum SalaVIP
        {
            AcessarTelaSalaVIP = 8
        }
        #endregion

        #region CategoriaPermissao.SalaSelecionadora
        [CategoriaPermissaoAttribute(CategoriaPermissao.SalaSelecionadora)]
        public enum SalaSelecionadora
        {
            AcessarTelaSalaSelecionadora = 9
        }
        #endregion

        #region CategoriaPermissao.TelaCursos
        [CategoriaPermissaoAttribute(CategoriaPermissao.TelaCursos)]
        public enum TelaCursos
        {
            AcessarTelaCursos = 10
        }
        #endregion

        #region CategoriaPermissao.TelaCadastroEmpresaUsuario
        [CategoriaPermissaoAttribute(CategoriaPermissao.TelaCadastroEmpresaUsuario)]
        public enum TelaCadastroEmpresaUsuario
        {
            AcessarTelaCadastroUsuários = 11
        }
        #endregion

        #region CategoriaPermissao.TelaSalaSelecionadorPlanoIlimitado
        [CategoriaPermissaoAttribute(CategoriaPermissao.TelaSalaSelecionadorPlanoIlimitado)]
        public enum TelaSalaSelecionadorPlanoIlimitado
        {
            AcessarTelaSalaSelecionadorPlanoIlimitado = 12
        }
        #endregion

        #region CategoriaPermissao.TelaSalaAdministrador
        [CategoriaPermissaoAttribute(CategoriaPermissao.Administrador)]
        public enum Administrador
        {
            AcessarTelaSalaAdministrador = 13,
            AcessarTelaSalaAdministradorFinanceiro = 14,
            AcessarTelaSalaAdministradorEmpresas = 15,
            AcessarTelaSalaAdministradorConfiguracoes = 16,
            AcessarTelaSalaAdministradorVagas = 17,
            AcessarTelaSalaAdministradorPublicacaoCV = 18,
            AcessarTelaSalaAdministradorAuditoriaCV = 19,
            AcessarTelaSalaAdministradorEdicaoCV = 21,
            VisualizarInformacoesCadastroEmpresa = 22,
            FinanceiroSalvarPlano = 23,
            FinanceiroEditarPlano = 24,
            FinanceiroVisualizarParcelas = 25,
            FinanceiroSalvarParcela = 26,
            FinanceiroVisualizarInformacoes = 27,
            FinanceiroCancelarPlano = 28,
            FinanceiroLiberarPlano = 29,
            FinanceiroLiberarCliente = 30,
            FinanceiroAtualizarPlano = 31,
            AlterarSituacaoFilial = 32,
            FinanceiroEstornoPagamento = 34,
            DesbloqueioPlanoEmpresa = 35,
            BloqueioPlanoEmpresa = 36
        }
        #endregion

    }
}