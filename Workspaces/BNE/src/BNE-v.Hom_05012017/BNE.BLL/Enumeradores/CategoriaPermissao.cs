using System;
namespace BNE.BLL.Enumeradores
{

    #region CategoriaPermissao
    public enum CategoriaPermissao
    {
        PesquisaAvancadaCurriculo = 1,
        EnviarMensagem = 2,
        EnviarCurriculo = 3,
        SolicitacaoR1 = 4,
        ManterVaga = 5,
        CompraPlanoEmpresa = 6,
        CompraPlanoVIP = 7,
        SalaSelecionadora = 8,
        SalaVIP = 9,
        TelaCursos = 10,
        TelaCadastroEmpresaUsuario = 11,
        TelaSalaSelecionadorPlanoIlimitado = 12,
        Administrador = 13
    }
    #endregion

    #region CategoriaPermissaoAttribute
    public class CategoriaPermissaoAttribute : Attribute
    {
        public CategoriaPermissao Categoria { get; private set; }

        public CategoriaPermissaoAttribute(CategoriaPermissao categoriaPermissao)
        {
            this.Categoria = categoriaPermissao;
        }
    }
    #endregion

}
