using System;
using System.Web.Script.Services;
using System.Web.Services;
using BNE.BLL;
using BNE.Common.Session;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.WebServices
{
    /// <summary>
    /// Summary description for wsUtils
    /// </summary>
    [ScriptService]
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wsUtils : System.Web.Services.WebService
    {

        #region MontarURLAtendimento
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true), ScriptMethod]
        public string MontarURLAtendimento()
        {
            var idPessoaFisicaLogada = new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString());
            var idFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());

            string retorno = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.AloWebBneVIP);

            if (idFilial.HasValue)
                retorno = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.AloWebBneCIA);

            //Verificando se existe pessoa física logada
            if (idPessoaFisicaLogada.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisicaLogada.Value);
                retorno += String.Format("&nome={0}", objPessoaFisica.NomeCompleto);
                retorno += String.Format("&email={0}", objPessoaFisica.EmailPessoa);

                String mensagemPadrao = String.Empty;
                mensagemPadrao += (String.Format("CPF:{0}", objPessoaFisica.NumeroCPF));
                mensagemPadrao += (String.Format(" NASCIMENTO:{0}", objPessoaFisica.DataNascimento.ToString("dd/MM/yyyy")));

                if (idFilial.HasValue && idFilial.Value > 0)
                {
                    Filial objFilial = Filial.LoadObject(idFilial.Value);
                    mensagemPadrao += (String.Format(" EMPRESA:{0}", objFilial.NomeFantasia));
                    mensagemPadrao += (String.Format(" CNPJ:{0}", objFilial.NumeroCNPJ));

                    PlanoAdquirido objPlanoAdquirido;
                    PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(objFilial, new PlanoSituacao((int)BLL.Enumeradores.PlanoSituacao.Liberado), out objPlanoAdquirido);

                    if (objPlanoAdquirido == null)
                    {
                        mensagemPadrao += " PLANO:SEM PLANO ATIVO";
                        PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(objFilial, new PlanoSituacao((int)BLL.Enumeradores.PlanoSituacao.AguardandoLiberacao), out objPlanoAdquirido);
                        if (objPlanoAdquirido != null)
                        {
                            objPlanoAdquirido.Plano.CompleteObject();
                            mensagemPadrao += String.Format("({0} - *Aguardando Liberação*", objPlanoAdquirido.Plano.DescricaoPlano);
                        }
                    }
                    else
                    {
                        objPlanoAdquirido.Plano.CompleteObject();
                        mensagemPadrao += String.Format(" PLANO:{0}", objPlanoAdquirido.Plano.DescricaoPlano);
                        mensagemPadrao += String.Format(" FIM DO PLANO:{0}", objPlanoAdquirido.DataFimPlano.ToString("dd/MM/yyyy"));
                    }
                }
                else
                {
                    PlanoAdquirido objPlanoAdquirido;
                    PlanoAdquirido.CarregarPlanoVigentePessoaFisicaPorSituacao(idPessoaFisicaLogada.Value, new PlanoSituacao((int)BLL.Enumeradores.PlanoSituacao.Liberado), out objPlanoAdquirido);

                    if (objPlanoAdquirido == null)
                    {
                        mensagemPadrao += " PLANO:SEM PLANO ATIVO";
                        PlanoAdquirido.CarregarPlanoVigentePessoaFisicaPorSituacao(idPessoaFisicaLogada.Value, new PlanoSituacao((int)BLL.Enumeradores.PlanoSituacao.AguardandoLiberacao), out objPlanoAdquirido);
                        if (objPlanoAdquirido != null)
                        {
                            objPlanoAdquirido.Plano.CompleteObject();
                            mensagemPadrao += String.Format("({0} - *Aguardando Liberação*", objPlanoAdquirido.Plano.DescricaoPlano);
                        }
                    }
                    else
                    {
                        objPlanoAdquirido.Plano.CompleteObject();
                        mensagemPadrao += String.Format(" PLANO: {0}", objPlanoAdquirido.Plano.DescricaoPlano);
                        mensagemPadrao += String.Format(" FIM DO PLANO: {0}", objPlanoAdquirido.DataFimPlano.ToString("dd/MM/yyyy"));
                    }
                }

                retorno += String.Format("&msg={0}", mensagemPadrao);
            }

            return retorno;
        }
        #endregion
    }
}
