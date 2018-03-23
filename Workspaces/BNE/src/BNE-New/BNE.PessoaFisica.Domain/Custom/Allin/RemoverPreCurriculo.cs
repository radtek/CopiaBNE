using BNE.Logger.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Custom.Allin
{
    public class RemoverPreCurriculo
    {
        private readonly PreCurriculo _preCurriculo;
        private readonly Global.Domain.Cidade _cidade;
        private readonly Parametro _parametro;
        private readonly ILogger _logger;

        public RemoverPreCurriculo(PreCurriculo objpreCurriculo, Parametro parametro, ILogger logger, Global.Domain.Cidade cidade)
        {
            _preCurriculo = objpreCurriculo;
            _cidade = cidade;
            _parametro = parametro;
            _logger = logger;
        }

        public string EnviarDadosAllin(int preCadastro)
        {
            string ticket = string.Empty;

            //carregar dados do CV
            var objPreCurriculo = _preCurriculo.Get(preCadastro);

            if(objPreCurriculo != null)
            {
              
                    if(BNE.Core.Common.Utils.ValidarEmail(objPreCurriculo.Email))
                    {
                      
                        ticket = FazerLoginAllin();

                        if(ticket != "")
                            EnviarCurriculoAllin(ticket, objPreCurriculo.Email);


                        return "Sucesso";
                    }
                }

            return "Falha";
        }

     
        #region EnviarAllin
        private bool EnviarCurriculoAllin(string ticket, string email)
        {
            string retorno ="";

            try
            {

                using (wsRemoverEmailAllin.wsRemoverEmailBaseService ws = new wsRemoverEmailAllin.wsRemoverEmailBaseService())
                {
                    retorno = ws.removerEmailBase(ticket, email, _parametro.RecuperarValor(Model.Enumeradores.Parametro.AllinPreCadastroNomeLista), "nm_email", email);
                }

                if (retorno == "Email removido da lista!")
                    return true;
                else
                {
                    _logger.Error(new Exception(retorno), string.Format("Erro ao remover email na lista Pre Cadastro no Allin => {0}", email));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("não remover Pre Cadastro no Allin {0}", email));
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
