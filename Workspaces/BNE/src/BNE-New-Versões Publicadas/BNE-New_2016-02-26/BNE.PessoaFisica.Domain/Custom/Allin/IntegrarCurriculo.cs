using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.ExceptionLog.Interface;

namespace BNE.PessoaFisica.Domain.Custom.Allin
{
    public class IntegrarCurriculo
    {
        private readonly Curriculo _curriculo;
        private readonly PessoaFisica _pessoaFisica;
        private readonly Email _email;
        private readonly Global.Domain.Cidade _cidade;
        private readonly Parametro _parametro;
        private readonly ILogger _logger;

        public IntegrarCurriculo(Curriculo curriculo, PessoaFisica pessoaFisica, Email email, Parametro parametro, ILogger logger, Global.Domain.Cidade cidade)
        {
            _curriculo = curriculo;
            _pessoaFisica = pessoaFisica;
            _email = email;
            _cidade = cidade;
            _parametro = parametro;
            _logger = logger;
        }

        public string EnviarDadosAllin(int idCurriculo)
        {
            string ticket = "";

            //carregar dados do CV
            var objCurriculo = _curriculo.GetById(idCurriculo);

            if(objCurriculo != null)
            {
                var objPessoa = _pessoaFisica.GetById(objCurriculo.PessoaFisica.Id);
                var objPessoaEmail = _email.GetByIdPessoaFisica(objPessoa.Id);

                if(objPessoaEmail != null)
                {
                    if(BNE.Core.Common.Utils.ValidarEmail(objPessoaEmail.Endereco))
                    {
                        Object[] objCampos = MontarCampos(objCurriculo, objPessoa, objPessoaEmail);

                        ticket = FazerLoginAllin();

                        if(ticket != "")
                            EnviarCurriculoAllin(ticket, objCampos[0].ToString(), objCampos[1].ToString());


                        return "Sucesso";
                    }
                }
            }


            return "Falha";
        }

        #region MontarCampos
        private Object[] MontarCampos(Model.Curriculo curriculo, Model.PessoaFisica pessoaFisica, Model.Email email)
        {
            StringBuilder sbCampos = new StringBuilder();
            StringBuilder sbValores = new StringBuilder();
            Object[] retorno = new Object[2];


            sbCampos.Append("nm_email");
            sbValores.AppendFormat("{0};", email.Endereco);

            sbCampos.Append("id_cadastro;");
            sbValores.AppendFormat("{0};", curriculo.Id);

            sbCampos.Append("Tipo_Curriculo;");
            sbValores.AppendFormat("{0};", curriculo.TipoCurriculo.Descricao);

            sbCampos.Append("cpf;");
            sbValores.AppendFormat("{0};", pessoaFisica.CPF);

            sbCampos.Append("nome;");
            sbValores.AppendFormat("{0};", pessoaFisica.Nome);

            sbCampos.Append("Observacao_Curriculo;");
            sbValores.AppendFormat("{0};", curriculo.Observacao != null ? curriculo.Observacao.ToString().Substring(0, curriculo.Observacao.ToString().IndexOf(" ")) : "");

            sbCampos.Append("dt_nascimento;");
            sbValores.AppendFormat("{0};", pessoaFisica.DataNascimento.ToString("dd/MM/yyyy"));

            sbCampos.Append("dt_cadastro;");
            sbValores.AppendFormat("{0};", pessoaFisica.DataCadastro.ToString("dd/MM/yyyy"));

            if (pessoaFisica.DataAlteracao != null)
            {
                sbCampos.Append("dt_atualizacao;");
                sbValores.AppendFormat("{0};", curriculo.DataAtualizacao != null ? curriculo.DataAtualizacao.Value.ToString("dd/MM/yyyy") : "");

                sbCampos.Append("Dt_Modificacao;");
                sbValores.AppendFormat("{0};", curriculo.DataModificacao != null ? curriculo.DataModificacao.Value.ToString("dd/MM/yyyy") : "");
            }

            sbCampos.Append("Flag_Vip;");
            sbValores.AppendFormat("{0};", (curriculo.FlgVIP == true ? "1" : "0"));

            //sbCampos.Append("Dt_Fim_Vip;");
            //sbValores.AppendFormat("{0};", "");

            if (pessoaFisica.Cidade != null)
            {
                sbCampos.Append("cidade;");
                sbValores.AppendFormat("{0};", pessoaFisica.Cidade.Nome);

                sbCampos.Append("uf;");
                sbValores.AppendFormat("{0};", pessoaFisica.Cidade.Estado.Nome);
            }

            if(pessoaFisica.Sexo != null)
            {
                sbCampos.Append("sexo;");
                sbValores.AppendFormat("{0};", pessoaFisica.Sexo != null? pessoaFisica.Sexo.ToString() : "");
            }

            //sbCampos.Append("Experiencia_Dta_Demissao;");
            //sbValores.AppendFormat("{0};", "");

            sbCampos.Append("Qtd_Qm_Me_Viu");
            sbValores.AppendFormat("{0}", "0");

            retorno[0] = sbCampos.ToString();
            retorno[1] = sbValores.ToString();

            return retorno;
        }
        #endregion

        #region EnviarAllin
        private bool EnviarCurriculoAllin(string ticket, string sbCampos, string sbValores)
        {
            string retorno ="";

            try
            {
                Object[] dados = new Object[3];

                dados[0] = _parametro.RecuperarValor(Model.Enumeradores.Parametro.AllinCandidatoNomeLista);
                dados[1] = sbCampos;
                dados[2] = sbValores.Replace("'", "");

                using (wsInserirEmailAllin.wsInserirEmailBaseService ws = new wsInserirEmailAllin.wsInserirEmailBaseService())
                {
                   retorno = ws.inserirEmailBase(ticket, dados);
                }

                if (retorno == "Email inserido na base!")
                    return true;
                else
                {
                    _logger.Error(new Exception(retorno), string.Format("Erro ao inserir email no Allin => {0}", dados));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("não inseriu CV no Allin {0}", sbValores));
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
