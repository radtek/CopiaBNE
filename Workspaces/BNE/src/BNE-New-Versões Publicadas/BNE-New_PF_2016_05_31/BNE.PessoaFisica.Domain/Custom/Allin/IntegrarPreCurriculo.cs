using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Custom.Allin
{
    public class IntegrarPreCurriculo
    {
        private readonly PreCurriculo _preCurriculo;
        private readonly Global.Domain.Cidade _cidade;
        private readonly Parametro _parametro;
        private readonly ILogger _logger;

      public IntegrarPreCurriculo(PreCurriculo objPreCurriculo, Parametro parametro, ILogger logger, Global.Domain.Cidade cidade)
        {
            _preCurriculo = objPreCurriculo;
            _cidade = cidade;
            _parametro = parametro;
            _logger = logger;
        }

        public string EnviarDadosAllin(int idPreCurriculo)
        {
            try
            {
                string ticket = "";


                var objPreCurriuclo = _preCurriculo.Get(idPreCurriculo);
               

                if (BNE.Core.Common.Utils.ValidarEmail(objPreCurriuclo.Email))
                {
                    Object[] objCampos = MontarCampos(objPreCurriuclo);

                    ticket = FazerLoginAllin();

                    if (ticket != "")
                        EnviarPreCurriculoAllin(ticket, objCampos[0].ToString(), objCampos[1].ToString());

                    return "Sucesso";
                }
                return "Falha";
            }
            catch (Exception ex)
            {

                return "Falha";
            }
           
        }

        #region MontarCampos
        private Object[] MontarCampos(Model.PreCurriculo objPreCurriculo)
        {
            StringBuilder sbCampos = new StringBuilder();
            StringBuilder sbValores = new StringBuilder();
            Object[] retorno = new Object[2];

            var cidade = _cidade.GetById(Convert.ToInt32(objPreCurriculo.IdCidade));

            sbCampos.Append("nm_email;");
            sbValores.AppendFormat("{0};", objPreCurriculo.Email);

            sbCampos.Append("nome_completo;");
            sbValores.AppendFormat("{0};", objPreCurriculo.Nome);

            sbCampos.Append("Funcao;");
            sbValores.AppendFormat("{0};", objPreCurriculo.DescricaoFuncao);

            sbCampos.Append("cidade;");
            sbValores.AppendFormat("{0};", cidade.Nome);

            sbCampos.Append("uf;");
            sbValores.AppendFormat("{0};", cidade.Estado.UF);

            sbCampos.Append("Ultimo_Salario;");
            sbValores.AppendFormat("{0};", objPreCurriculo.PretensaoSalarial);

            sbCampos.Append("dt_cadastro");
            sbValores.AppendFormat("{0}", objPreCurriculo.DataCadastro.ToString("yyyy-MM-dd"));

            retorno[0] = sbCampos.ToString();
            retorno[1] = sbValores.ToString();

            return retorno;
        }
        #endregion

        #region EnviarAllin
        private bool EnviarPreCurriculoAllin(string ticket, string sbCampos, string sbValores)
        {
            string retorno = "";

            try
            {
                Object[] dados = new Object[3];

                dados[0] = _parametro.RecuperarValor(Model.Enumeradores.Parametro.AllinPreCadastroNomeLista);
                dados[1] = sbCampos;
                dados[2] = sbValores.Replace("'", "");
                wsInserirEmailAllin.wsInserirEmailBaseService teste = new wsInserirEmailAllin.wsInserirEmailBaseService();

                using (wsInserirEmailAllin.wsInserirEmailBaseService ws = new wsInserirEmailAllin.wsInserirEmailBaseService())
                {
                   retorno = ws.inserirEmailBase(ticket, dados);
                }

                if (retorno == "Email inserido na base!")
                    return true;
                else
                {
                    _logger.Error(new Exception(retorno), string.Format("Erro ao inserir email na lista Pre Cadastro no Allin => {0}", dados));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("não inseriu Pre Cadastro no Allin {0}", sbValores));
                return false;
            }
        }

        #endregion

        #region FazerLoginAllin
        private string FazerLoginAllin()
        {
            string ticket = "";
            string loginAllin = _parametro.RecuperarValor(Model.Enumeradores.Parametro.AllinWebServiceLogin);
            string senhaAllin = _parametro.RecuperarValor(Model.Enumeradores.Parametro.AllinWebServiceSenha);

            try
            {
                
                using (wsLoginAllin.wsLoginService ws = new wsLoginAllin.wsLoginService())
                {
                    ticket = ws.getTicket(loginAllin, senhaAllin);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao efetuar login no Allin");
            }

            return ticket;
        }
        #endregion
    }
}
