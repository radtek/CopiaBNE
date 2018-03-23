using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;

namespace BNE.Web.Services.Integracao
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "App" in code, svc and config file together.
    public class App : IApp
    {

        #region Implementation of IApp

        #region DadosCurriculoJSON
        public Stream DadosCurriculoJSON(string numeroCNPJ, string senhaUsuario, string nomeCliente, int codigoCurriculo)
        {
            var retornoIntegrador = BLL.IntegradorCurriculo.RecuperarDadosCurriculo(numeroCNPJ, senhaUsuario, nomeCliente, codigoCurriculo);

            if (string.IsNullOrWhiteSpace(retornoIntegrador.Erro))
                return Helper.RetornarStream(retornoIntegrador.Curriculo, Helper.TipoRetorno.JSON);

            return Helper.RetornarStream(retornoIntegrador.Erro, Helper.TipoRetorno.Text);
        }
        #endregion

        #region DadosCurriculoXML
        public Stream DadosCurriculoXML(string numeroCNPJ, string senhaUsuario, string nomeCliente, int codigoCurriculo)
        {
            var retornoIntegrador = BLL.IntegradorCurriculo.RecuperarDadosCurriculo(numeroCNPJ, senhaUsuario, nomeCliente, codigoCurriculo);

            if (string.IsNullOrWhiteSpace(retornoIntegrador.Erro))
                return Helper.RetornarStream(retornoIntegrador.Curriculo, Helper.TipoRetorno.XML);

            return Helper.RetornarStream(retornoIntegrador.Erro, Helper.TipoRetorno.Text);
        }
        #endregion

        #region AtualizarNumNF
        public DTO.OutAtualizaNumNF AtualizaNF(DTO.InAtualizaNumNF atualizaNF)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            DTO.OutAtualizaNumNF outAtualiza = new DTO.OutAtualizaNumNF();

            try
            {
                Pagamento objPagamento;

                if (Pagamento.CarregarPagamentoPorNossoNumeroBoleto(atualizaNF.desIdentificador, out objPagamento))
                {
                    objPagamento.SalvarNotaFiscal(atualizaNF.numeroNF, atualizaNF.linkNF);

                    outAtualiza.retorno = true;
                }
                else
                {
                    outAtualiza.retorno = false;
                }

            }
            catch (Exception ex)
            {

                EL.GerenciadorException.GravarExcecao(ex);
                outAtualiza.retorno = false;
            }

            return outAtualiza;

        }
        #endregion

        #region IntegrarVagaSINE
        public bool IntegrarVagaSINE(BLL.DTO.wsIntegracao.InVaga oVaga)
        {
            try
            {
                Integrador oIntegrador = Integrador.CarregaIntegradorSINE();    //Vagas SINE
                Origem oOrigem = Origem.LoadObject(2);                          //Vagas SINE

                BLL.Custom.Vaga.IntegracaoVaga oIntegracaoVaga = new BLL.Custom.Vaga.IntegracaoVaga();

                VagaIntegracao oVagaIntegracao = oIntegracaoVaga.ConverteVagaToVagaIntegracao(oIntegrador, oVaga);

                BNE.BLL.Custom.Vaga.EnfileiraVaga.EnfileiraByVagaIntegracao(oVagaIntegracao, oOrigem, oIntegrador);

                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "appBNE - Ws Integracao - IntegrarVagaSINE");
                return false;
            }
        }
        #endregion IntegrarVagaSINE

        #region InativarVagaOrigemSINE
        /// <summary>
        /// Inativa as vagas do BNE que já foram inativadas no SINE
        /// </summary>
        public bool InativarVagaOrigemSINE(string idVagaSine, string oportunidade)
        {
            try
            {
                Integrador oIntegrador = Integrador.CarregaIntegradorSINE(); //Vagas SINE

                BNE.BLL.Custom.Vaga.EnfileiraVaga.EnfileiraParaInativar(idVagaSine, oIntegrador, oportunidade);

                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "appBNE - Ws Integracao - InativarVagaOrigemSINE");
                return false;
            }
        }
        #endregion IntegrarVagaSINE

        #region BloquearCurriculo
        /// <summary>
        /// Coloca o curriculo no bronquinha.
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="motivo"></param>
        public Boolean BloquearCurriculo(decimal cpf, string motivo)
        {
            try
            {
                Curriculo oCurriculo;
                if (Curriculo.CarregarPorCpf(cpf, out oCurriculo))
                {
                    oCurriculo.PessoaFisica.CompleteObject();
                    Curriculo.BloquearCurriculo(oCurriculo.IdCurriculo, String.Format("Bloqueado no Sine. - {0}", motivo));
                }
                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "appBNE - Ws Integracao - Inativar Curriculo - via sine");
                return false;
            }
        }
        #endregion

        #endregion

    }
}
