using BNE.BLL.Custom;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL.Integracoes.ArquivosBanco
{
    public class ArquivoDebitoHSBC
    {
        #region Consultas
        private const String SELECT_PROXIMA_NUMERACAO = "SELECT COUNT(Idf_Arquivo) + 1 FROM BNE.BNE_Arquivo WHERE Idf_Tipo_Arquivo = @IdTipoArquivo;";
        #endregion

        /// <summary>
        /// Gera uma string com o conteúdo do arquivo a ser enviado ao HSBC através do home bank.
        /// O Arquivo deve conter as transações ainda não enviadas e as transações já registradas com planos cancelados
        /// </summary>
        /// <returns>Conteúdo do Arquivo</returns>
        public static string GerarArquivo()
        {
            //Recuperar débitos a enviar do HSBC
            List<Transacao> lstTranscoesHSBC = Transacao.CarregarTransacoesDebitoRecorrenteNaoRemetidas(399);

            if (lstTranscoesHSBC.Count <= 0)
            {
                return null;
            }

            StringBuilder conteudo = new StringBuilder();
            conteudo.AppendLine(GerarHeader());

            foreach (Transacao objTransacao in lstTranscoesHSBC)
            {
                conteudo.AppendLine(GerarRegistro(objTransacao));
            }

            conteudo.AppendLine(GerarFooter(lstTranscoesHSBC.Count, lstTranscoesHSBC.Sum(t => t.ValorDocumento)));

            return conteudo.ToString();
        }

        private static string GerarHeader()
        {
            String header = "A"; //Código do Registro
            header += "1"; //Código de Remessa
            
            // Codigo Convênio
            String temp = Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXCodigoAgencia)), 4);
            temp += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXNumeroContaCorrente).Replace("-", "")), 7);
            header += Helper.NumericoParaTamanhoExato(Convert.ToInt32(temp), 11) + Helper.StringParaTamanhoExato("", 9);

            //Nome do cliente credor
            header += Helper.StringParaTamanhoExato("Banco Nacional de Empregos", 20);
            //Código do Banco
            header += "399";
            //Nome do Banco
            header += Helper.StringParaTamanhoExato("HSBC", 20);
            //Data de geração do arquivo
            header += DateTime.Now.ToString("yyyyMMdd");
            //Número Sequencial do Arquivo
            header += Helper.NumericoParaTamanhoExato(ObterProximaSequencia(), 6);
            //Versão do Layout
            header += "04";
            //Identificação do serviço
            header += Helper.StringParaTamanhoExato("DEBITO AUTOMATICO", 17);
            //Brancos
            header += Helper.StringParaTamanhoExato("", 52);

            return header;
        }

        private static string GerarRegistro(Transacao objTransacao, bool inclusao = true)
        {

            if (objTransacao.PlanoAdquirido.ValorBase == null)
            {
                objTransacao.PlanoAdquirido.CompleteObject();
            }

            String registro = "E"; //Código do Registro
            //Identificacao do cliente consumidor junto ao cliente credor
            registro += Helper.NumericoParaTamanhoExato(objTransacao.PlanoAdquirido.IdPlanoAdquirido, 20);
            //Brancos
            registro += Helper.StringParaTamanhoExato("", 5);
            //Agencia do banco para debito
            registro += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoAgenciaDebito, 4);
            //Conta Corrente
            registro += "399";
            registro += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoAgenciaDebito, 4);
            registro += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoContaCorrenteDebito, 7);
            //Data do Vencimento
            registro += DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            //Valor do Debito
            registro += Helper.MonetarioParaTamanhoExato(objTransacao.ValorDocumento, 15);
            //Código da Moead
            registro += "03";
            //Uso do cliente credor
            registro += Helper.StringParaTamanhoExato("", 31);
            //CPF/CNPJ - Obrigatório quando validar
            registro += Helper.NumericoParaTamanhoExato(objTransacao.NumeroCPFTitularContaCorrenteDebito.HasValue ? objTransacao.NumeroCPFTitularContaCorrenteDebito.Value : objTransacao.NumeroCNPJTitularContaCorrenteDebito.Value, 14);
            //Valor Base de Cálculo do IOF - Não enviar
            registro += Helper.NumericoParaTamanhoExato(0, 15);
            //Tipo do Débito - enviar 19 - Compromissos Diversos
            registro += "19";
            //Alíquota IPF = Não Enviar
            registro += Helper.NumericoParaTamanhoExato(0, 7);
            //Brancos
            registro += Helper.StringParaTamanhoExato("", 11);
            //Código do movimento
            if (inclusao)
            {
                registro += "0"; //Inclusão
            }
            else
            {
                registro += "1"; //Cancelamento
            }

            return registro;
        }

        private static string GerarFooter(int totalRegistros, decimal vlrTotalDebitos)
        {
            //Código do Registro
            String footer = "Z"; 
            //Total de registros do arquivo
            footer += Helper.NumericoParaTamanhoExato(totalRegistros, 6); 
            //Valor Total dos registros do arquivo
            footer += Helper.MonetarioParaTamanhoExato(vlrTotalDebitos, 17);
            //Valor Total de IOF recolhido
            footer += Helper.MonetarioParaTamanhoExato(0, 17);
            //Brancos
            footer += Helper.StringParaTamanhoExato("", 109);

            return footer;
        }

        private static int ObterProximaSequencia()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("IdTipoArquivo", (int)Enumeradores.TipoArquivo.RemessaDebitoHSBC));

            return (int)DataAccessLayer.ExecuteScalar(System.Data.CommandType.Text, SELECT_PROXIMA_NUMERACAO, parms);
        }
    }
}
