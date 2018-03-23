using BNE.BLL.Custom.Email;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class CobrancaBoleto // Tabela: GLO_Cobranca_Boleto
    {
        #region GerarBoletoNovo
        public static List<BoletoNet.Boleto> GerarBoletosNovo(PlanoAdquirido objPlanoAdquirido, int idUsuarioFilialPerfilLogadoEmpresa, SqlTransaction trans = null)
        {
            if (objPlanoAdquirido != null)
            {
                var parcelas = PlanoParcela.RecuperarParcelasPlanoAdquirido(objPlanoAdquirido);
                var listaPagamento = new List<Pagamento>();

                foreach (var objPlanoParcela in parcelas)
                {
                    var listaPagamentoPorParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela).Where(p =>
                        p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario
                        && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto
                        && p.FlagInativo == false)
                        .OrderByDescending(p => p.IdPagamento)
                        .ToList();
                    listaPagamento.Add(listaPagamentoPorParcela.First()); // adiciona o último pagamento

                    if (listaPagamentoPorParcela.Count > 1)//Apenas para parcela com mais de um pagamento aberto
                    {
                        listaPagamentoPorParcela.Remove(listaPagamentoPorParcela.First()); //Remove da lista de pagamentos em aberto
                        Pagamento.CancelarOutrosPagamentosEmAbertoDePlanoParcela(listaPagamentoPorParcela, trans); // Cancela outros pagamentos
                    }
                }
                return CriarBoleto(listaPagamento);
            }
            return null;
        }
        #endregion

        #region GerarBoletoPagamentoAdicionalNovo
        public static List<BoletoNet.Boleto> GerarBoletoPagamentoAdicionalNovo(decimal pagamentoAdicionalValorTotal, int pagamentoAdicionalQuantidade, int idFilial, int idUsuarioFilialPerfilLogadoEmpresa, ref int pagamentoIdentificadorPagamento)
        {
            var objFilial = new Filial(idFilial);

            PlanoAdquirido objPlanoAdquirido;
            PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

            if (objPlanoAdquirido != null)
            {
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfilLogadoEmpresa);

                BLL.Pagamento objPagamento;
                AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, pagamentoAdicionalValorTotal, pagamentoAdicionalQuantidade, objUsuarioFilialPerfil, Enumeradores.TipoPagamento.BoletoBancario, DateTime.Now, DateTime.Today, out objPagamento);

                pagamentoIdentificadorPagamento = objPagamento.IdPagamento;
                var listaPagamento = new List<Pagamento> { objPagamento };

                return CriarBoleto(listaPagamento);
            }
            return null;
        }
        #endregion

        #region CriarBoleto
        private static List<BoletoNet.Boleto> CriarBoleto(List<Pagamento> pagamentos)
        {
            using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<BoletoNet.Boleto> boletos = new List<BoletoNet.Boleto>();

                        foreach (var objPagamento in pagamentos)
                            boletos.Add(ProcessarBoletoNovo(objPagamento, trans));
                        trans.Commit();

                        return boletos;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region ProcessarNovoBoleto
        public static BoletoNet.Boleto ProcessarBoletoNovo(BLL.Pagamento objPagamento, SqlTransaction trans)
        {
            List<Enumeradores.Parametro> parametros = new List<Enumeradores.Parametro>();
            parametros.Add(Enumeradores.Parametro.CobreBemXCodigoAgencia);
            parametros.Add(Enumeradores.Parametro.CobreBemXNumeroContaCorrente);
            parametros.Add(Enumeradores.Parametro.CobreBemXCodigoCedente);
            parametros.Add(Enumeradores.Parametro.CobreBemXCodBanco);

            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            String codigoAgencia = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoAgencia];
            String[] conta = valoresParametros[Enumeradores.Parametro.CobreBemXNumeroContaCorrente].Split('-');
            String codigoCedente = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoCedente];
            String codigoBanco = valoresParametros[Enumeradores.Parametro.CobreBemXCodBanco];

            String nossoNumero = objPagamento.IdPagamento.ToString();
            String codigoBarras = String.Empty;
            String email = String.Empty;

            var cedente = new BoletoNet.Cedente("82.344.425/0001-82", "Bne - Banco Nacional de Empregos Ltda", codigoAgencia, "", conta[0], conta[1]);
            cedente.Codigo = codigoCedente;

            var boleto = new BoletoNet.Boleto(objPagamento.DataVencimento.Value, objPagamento.ValorPagamento, "CNR", nossoNumero, cedente);
            boleto.Banco = new BoletoNet.Banco(Convert.ToInt32(codigoBanco));

            //DADOS DO SACADO
            #region Informações do boleto de Endereço e Sacado

            if (objPagamento.Filial != null)
            {
                objPagamento.Filial.CompleteObject(trans);
                boleto.Sacado = new BoletoNet.Sacado(objPagamento.Filial.NumeroCNPJ.Value.ToString().PadLeft(11, '0'), objPagamento.Filial.RazaoSocial);
                if (objPagamento.Filial.Endereco != null)
                {
                    objPagamento.Filial.Endereco.CompleteObject(trans);

                    boleto.Sacado.Endereco = new BoletoNet.Endereco();

                    if (objPagamento.Filial.Endereco.Cidade.CompleteObject())
                        boleto.Sacado.Endereco.Cidade = objPagamento.Filial.Endereco.Cidade.NomeCidade;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoLogradouro))
                        boleto.Sacado.Endereco.Logradouro = objPagamento.Filial.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroEndereco))
                        boleto.Sacado.Endereco.Numero = objPagamento.Filial.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoComplemento))
                        boleto.Sacado.Endereco.Complemento = objPagamento.Filial.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroCEP))
                        boleto.Sacado.Endereco.CEP = objPagamento.Filial.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoBairro))
                        boleto.Sacado.Endereco.Bairro = objPagamento.Filial.Endereco.DescricaoBairro;
                }
            }
            else
            {
                objPagamento.UsuarioFilialPerfil.CompleteObject(trans);
                objPagamento.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                boleto.Sacado = new BoletoNet.Sacado(objPagamento.UsuarioFilialPerfil.PessoaFisica.CPF.ToString(), objPagamento.UsuarioFilialPerfil.PessoaFisica.NomePessoa);

                if (objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco != null)
                {
                    objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.CompleteObject(trans);

                    boleto.Sacado.Endereco = new BoletoNet.Endereco();
                    if (objPagamento.Filial.Endereco.Cidade.CompleteObject())
                        boleto.Sacado.Endereco.Cidade = objPagamento.Filial.Endereco.Cidade.NomeCidade;

                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro))
                        boleto.Sacado.Endereco.Logradouro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco))
                        boleto.Sacado.Endereco.Numero = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento))
                        boleto.Sacado.Endereco.Complemento = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP))
                        boleto.Sacado.Endereco.CEP = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro))
                        boleto.Sacado.Endereco.Bairro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro;
                }
            }
            #endregion Informações do boleto de Endereço e Sacado

            BoletoNet.Instrucao_HSBC instrucao = new BoletoNet.Instrucao_HSBC();
            instrucao.Descricao = "Todas as informações deste boleto são de exclusiva responsabilidade do cedente. Um dia útil após a confirmação do pagamento, você terá acesso ao serviço do BNE.";
            boleto.Instrucoes.Add(instrucao);

            boleto.Valida();

            objPagamento.DescricaoIdentificador = boleto.NossoNumero;
            objPagamento.DescricaoDescricao = boleto.CodigoBarra.Codigo;
            objPagamento.Save(trans);

            return boleto;
        }
        #endregion

        #region GerarLayoutBoleto
        public static string GerarLayoutBoleto(List<BoletoNet.Boleto> boletos)
        {
            try
            {
                BoletoNet.BoletoBancario boletoBancario;
                var html = new StringBuilder();
                foreach (var boleto in boletos)
                {
                    boletoBancario = new BoletoNet.BoletoBancario();
                    boletoBancario.CodigoBanco = (short)boleto.Banco.Codigo;
                    boletoBancario.Boleto = boleto;
                    html.Append(boletoBancario.MontaHtmlEmbedded());
                    html.Append("</br></br></br></br></br></br></br></br></br></br>");
                }
                return html.ToString();

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw ex;
            }

        }
        #endregion      
    }
}