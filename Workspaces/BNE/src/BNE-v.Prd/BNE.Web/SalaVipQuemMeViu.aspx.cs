using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipQuemMeViu : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
            ucQuemMeViu.EventVerDadosEmpresa += ucQuemMeViu_VerDadosEmpresa;
            ucQuemMeViu.EventVerVagasEmpresa += ucQuemMeViu_VerVagasEmpresa;
            ucVerDadosEmpresa.Candidatar += ucVerDadosEmpresa_Candidatar;
        }
        #endregion

        #region ucVerDadosEmpresa_Candidatar
        void ucVerDadosEmpresa_Candidatar()
        {
            try
            {
                Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);

                if (objCurriculo.FlagVIP)
                {
                    IntencaoFilial objIntencaoFilial;
                    if (!IntencaoFilial.CarregarPorFilialCurriculo(objCurriculo.IdCurriculo, ucVerDadosEmpresa.IdFilial, out objIntencaoFilial))
                    {
                        objIntencaoFilial = new IntencaoFilial
                        {
                            Curriculo = objCurriculo,
                            Filial = new Filial(ucVerDadosEmpresa.IdFilial)
                        };
                    }
                    objIntencaoFilial.FlagInativo = false;
                    objIntencaoFilial.Save();
                    try
                    {
                      
                        int? idOrigemFilial = OrigemFilial.RecuperarIdOrigemPorFilial(new Filial(ucVerDadosEmpresa.IdFilial));
                        if (idOrigemFilial.HasValue && !CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(idOrigemFilial.Value)))//Empresa tem STC.
                        {
                            CurriculoOrigem objCurriculoOrigem = new CurriculoOrigem();
                            objCurriculoOrigem.Curriculo = objCurriculo;
                            objCurriculoOrigem.Origem = new Origem(idOrigemFilial.Value);
                            objCurriculoOrigem.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, String.Format("Erro ao gravar origem do curriculo, na tela do quem me viu id filial {0} idcv {1}", ucVerDadosEmpresa.IdFilial, base.IdCurriculo.Value));
                    }
                    ucVerDadosEmpresa.FecharModal();
                    ucModalConfirmacao.PreencherCampos("Confirmação de Envio", "Notificação enviada com sucesso!", "Havendo interesse a própria empresa fará contato com você, sem intermediários.", false);
                    ucModalConfirmacao.MostrarModal();
                }
                else
                    Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.EscolherEmpresa));
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucQuemMeViu_VerDadosEmpresa
        void ucQuemMeViu_VerDadosEmpresa(int idFilial, string nomeEmpresa)
        {
            Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
            if (objCurriculo.FlagVIP)
            {
                ucVerDadosEmpresa.IdFilial = idFilial;
                ucVerDadosEmpresa.MostrarModal();
            }
            else
                Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.AcessoDadosEmpresas));
        }
        #endregion

        #region ucQuemMeViu_VerVagasEmpresa
        void ucQuemMeViu_VerVagasEmpresa(int idFilial, string nomeEmpresa)
        {
            Redirect(Helper.RecuperarURLVagasEmpresa(nomeEmpresa, idFilial));
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "QuemMeViu");
            AjustarTituloTela("Quem me Viu?");
            ucQuemMeViu.Inicializar();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o usuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoCandidato.Value, Enumeradores.CategoriaPermissao.SalaVIP);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.SalaVIP.AcessarTelaSalaVIP))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
        }
        #endregion

        #endregion
    }
}