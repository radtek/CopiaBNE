using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PlanoParcela
    {
        #region CriarNovaParcela
        public static BNE_Pagamento CriarNovaParcela(BNE_Plano_Adquirido objPlanoAdquirido, BNE_Plano objPlano, int? numeroDesconto, DateTime dtaVencimento, DateTime dtaEnvioBoleto, int idTipoPagamento, BNE_Codigo_Desconto objCodigoDesconto, LanEntities context)
        {
            var objPlanoParcela = new BNE_Plano_Parcela
            {
                Dta_Cadastro = DateTime.Now,
                Idf_Plano_Adquirido = objPlanoAdquirido.Idf_Plano_Adquirido,
                Vlr_Parcela = objPlanoAdquirido.Vlr_Base,
                //Se for cortesia gerar parcela com a situação pago
                Idf_Plano_Parcela_Situacao = objPlanoAdquirido.Vlr_Base > 0 ? (int)Enumeradores.PlanoParcelaSituacao.EmAberto : (int)Enumeradores.PlanoParcelaSituacao.Pago,
                Num_Desconto = numeroDesconto,
                Flg_Inativo = false,
                Qtd_SMS_Total = objPlanoAdquirido.Qtd_SMS / (objPlano.Qtd_Parcela == 0 ? 1 : objPlano.Qtd_Parcela)
            };

            context.BNE_Plano_Parcela.Add(objPlanoParcela);
            context.SaveChanges();

            objPlanoParcela.BNE_Plano_Adquirido = objPlanoAdquirido;
            objPlanoParcela.BNE_Plano_Adquirido.BNE_Plano = objPlano;

            return CriarPagamento(objPlanoParcela, dtaEnvioBoleto, dtaVencimento, idTipoPagamento, objCodigoDesconto, context);
        }
        #endregion

        #region CriarPagamento
        public static BNE_Pagamento CriarPagamento(BNE_Plano_Parcela objPlanoParcela, DateTime dataEmissaoBoleto, DateTime dataVencimentoBoleto, int idTipoPagamento, BNE_Codigo_Desconto objCodigoDesconto, LanEntities context)
        {
            try
            {
                var objPagamento = new BNE_Pagamento
                {
                    Dta_Emissao = dataEmissaoBoleto,
                    Dta_Vencimento = dataVencimentoBoleto,
                    Dta_Cadastro = DateTime.Now,
                    Idf_Plano_Parcela = objPlanoParcela.Idf_Plano_Parcela,
                    Idf_Tipo_Pagamento = idTipoPagamento,
                    //Se for cortesia gerar o pagamento com a situação pago
                    Idf_Pagamento_Situacao = objPlanoParcela.BNE_Plano_Adquirido.Vlr_Base > 0 ? (int)Enumeradores.PagamentoSituacao.EmAberto : (int)Enumeradores.PagamentoSituacao.Pago,
                    Idf_Usuario_Filial_Perfil = objPlanoParcela.BNE_Plano_Adquirido.Idf_Usuario_Filial_Perfil,
                    Idf_Filial = objPlanoParcela.BNE_Plano_Adquirido.Idf_Filial,
                    Flg_Avulso = false,
                    Flg_Inativo = false,
                    Idf_Codigo_Desconto = objCodigoDesconto.Idf_Codigo_Desconto
                };

                if (ParaPessoaJuridica(objPlanoParcela.BNE_Plano_Adquirido))
                {
                    bool flgISS;
                    string textoNF;
                    int idCidade;
                    decimal valorLiquido;

                    Filial.RecuperarInfoISS(objPlanoParcela.BNE_Plano_Adquirido.Idf_Filial.Value, out flgISS, out textoNF, out idCidade);

                    if (flgISS)
                        CalcularISS(idCidade, objPlanoParcela.BNE_Plano_Adquirido.Vlr_Base, out valorLiquido);
                    else
                        if (objPlanoParcela.Num_Desconto.HasValue)
                            valorLiquido = objPlanoParcela.Vlr_Parcela - (objPlanoParcela.Vlr_Parcela * objPlanoParcela.Num_Desconto.Value / 100);
                        else
                            valorLiquido = objPlanoParcela.Vlr_Parcela;

                    objPagamento.Vlr_Pagamento = valorLiquido;
                }
                else
                {
                    if (objPlanoParcela.Num_Desconto.HasValue)
                        objPagamento.Vlr_Pagamento = objPlanoParcela.Vlr_Parcela - (objPlanoParcela.Vlr_Parcela * objPlanoParcela.Num_Desconto.Value / 100);
                    else
                        objPagamento.Vlr_Pagamento = objPlanoParcela.Vlr_Parcela;
                }

                context.BNE_Pagamento.Add(objPagamento);
                context.SaveChanges();

                return objPagamento;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ParaPessoaJuridica
        public static bool ParaPessoaJuridica(BNE_Plano_Adquirido objPlanoAdquirido)
        {
            return objPlanoAdquirido.BNE_Plano.Idf_Plano_Tipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica);
        }
        #endregion

        #region CalcularISS
        public static void CalcularISS(int idCidade, decimal valorBruto, out decimal valorLiquido)
        {
            valorLiquido = 0;

            decimal taxaISS;
            decimal desconto;

            Cidade.RecuperarTaxaISS(idCidade, out taxaISS);

            desconto = (valorBruto * taxaISS) / 100;

            valorLiquido = valorBruto - desconto;
        }
        #endregion

        #region CarregaParcelaAtualEmAbertoPorPlanoAdquirido
        public static BNE_Plano_Parcela CarregaParcelaAtualEmAbertoPorPlanoAdquirido(int idPlanoAdquirido)
        {
            using (var entity = new LanEntities())
            {
                BNE_Plano_Parcela objPlanoParcela;

                objPlanoParcela = (from planoParcela in entity.BNE_Plano_Parcela
                                   where planoParcela.Idf_Plano_Adquirido == idPlanoAdquirido
                                   && planoParcela.Idf_Plano_Parcela_Situacao == 1
                                   && planoParcela.Flg_Inativo == false
                                   select planoParcela).OrderBy(x => x.Idf_Plano_Parcela).FirstOrDefault();

                return objPlanoParcela;
            }

        }
        #endregion

        #region CarregarPrimeiraParcelaPorPlanoAdquirido
        /// <summary>
        /// Carrega a primeira parcela de qualquer plano adquirido atraves do ID do plano Adquirido
        /// </summary>
        public static BNE_Plano_Parcela CarregarPrimeiraParcelaPorPlanoAdquirido(int idPlanoAdquirido)
        {
            using (var entity = new LanEntities())
            {
                BNE_Plano_Parcela objPlanoParcela;

                objPlanoParcela = (from planoParcela in entity.BNE_Plano_Parcela
                                   where planoParcela.Idf_Plano_Adquirido == idPlanoAdquirido
                                   select planoParcela).OrderBy(x => x.Idf_Plano_Parcela).FirstOrDefault();

                return objPlanoParcela;
            }
        }
        #endregion

        #region Liberar
        /// <summary>
        /// Liberar parcela
        /// </summary>
        /// <param name="objPagamento">objeto Pagamento referente ao pagamento da parcela</param>
        /// <param name="dtaPagamento">data do pagamento</param>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se tudo correu bem, false se não for plano para pessoa fisica nem para pessoa juridica</returns>
        public static bool Liberar(BNE_Plano_Parcela objPlanoParcela, BNE_Pagamento objPagamento, DateTime dtaPagamento, LanEntities context)
        {
            if (PlanoAdquirido.ParaPessoaFisica(objPlanoParcela.BNE_Plano_Adquirido))
            {
                BNE_Curriculo objCurriculo;
                Curriculo.LiberarVIP(objPlanoParcela.BNE_Plano_Adquirido, out objCurriculo, context);
                CancelarOutrosPagamentos(objPlanoParcela, objPagamento, context);
                CancelarOutrosPlanosAdquiridos(objPlanoParcela, objCurriculo, context);
            }
            else if (PlanoAdquirido.ParaPessoaJuridica(objPagamento.BNE_Plano_Parcela.BNE_Plano_Adquirido))
            {
                BNE_Usuario_Filial objUsuarioFilial;

                UsuarioFilial.LiberarCIA(objPagamento.BNE_Plano_Parcela.BNE_Plano_Adquirido, objPlanoParcela, out objUsuarioFilial, context);

                CancelarOutrosPlanosAdquiridos(objPlanoParcela, objPagamento.Idf_Filial.Value, context);
                RecarregarSMS(objPlanoParcela);    // libera SMSs da parcela
            }
            else
            {
                return false;
            }

            objPlanoParcela.Dta_Pagamento = dtaPagamento;
            objPlanoParcela.Idf_Plano_Parcela_Situacao = (int)Enumeradores.PlanoParcelaSituacao.Pago;
            context.SaveChanges();

            return true;
        }
        #endregion

        #region CancelarOutrosPagamentos
        /// <summary>
        /// Cancela pagamentos diferentes do pagamento informado
        /// </summary>
        /// <param name="objPagamento">Pagamento informado</param>
        /// <param name="trans">transação SQL Server</param>
        public static void CancelarOutrosPagamentos(BNE_Plano_Parcela objPlanoParcela, BNE_Pagamento objPagamento, LanEntities context)
        {
            //Cancela os outros pagamento relacionados a parcela atual que eventualmente estejam abertos
            var objListPagamentosEmAberto = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.Idf_Plano_Parcela, context);
            var pagamentos = objListPagamentosEmAberto
                .Where(p => p.Flg_Inativo == false
                    && p.Idf_Pagamento_Situacao == (int)Enumeradores.PagamentoSituacao.EmAberto
                    && p.Idf_Pagamento != objPagamento.Idf_Pagamento);

            foreach (var pagamentoEmAberto in pagamentos)
            {
                pagamentoEmAberto.Idf_Pagamento_Situacao = (int)Enumeradores.PagamentoSituacao.Cancelado;
                pagamentoEmAberto.Flg_Inativo = true;
                context.SaveChanges();
            }
        }

        #endregion

        #region CancelarOutrosPlanosAdquiridos
        /// <summary>
        /// Cancela planos adquiridos diferentes do informado
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="trans"></param>
        public static void CancelarOutrosPlanosAdquiridos(BNE_Plano_Parcela objPlanoParcela, BNE_Curriculo objCurriculo, LanEntities context)
        {
            var planosAguardandoLiberacao = PlanoAdquirido.CarregarPlanosAdquiridoPorSituacaoECV(objCurriculo.Idf_Curriculo, (int)Enumeradores.PlanoSituacao.AguardandoLiberacao, context)
                    .Where(p => p.Idf_Plano_Adquirido != objPlanoParcela.Idf_Plano_Adquirido);

            foreach (var objPlanoAdquirido in planosAguardandoLiberacao)
            {
                PlanoAdquirido.CancelarPlanoAdquirido(objPlanoAdquirido, "PlanoParcela > CancelarOutrosPlanosAdquiridos", false, context, objCurriculo.Idf_Curriculo);
            }
        }

        /// <summary>
        /// Cancela planos adquiridos diferentes do informado
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="trans"></param>
        public static void CancelarOutrosPlanosAdquiridos(BNE_Plano_Parcela objPlanoParcela, int idFilial, LanEntities context)
        {
            var planosAguardandoLiberacao = PlanoAdquirido.CarregarPlanosAdquiridoPorSituacaoEFilial(idFilial, (int)Enumeradores.PlanoSituacao.AguardandoLiberacao, context)
                    .Where(p => p.Idf_Plano_Adquirido != objPlanoParcela.Idf_Plano_Adquirido);

            foreach (var objPlanoAdquirido in planosAguardandoLiberacao)
            {
                PlanoAdquirido.CancelarPlanoAdquirido(objPlanoAdquirido, "PlanoParcela > CancelarOutrosPlanosAdquiridos", false, context);
            }
        }
        #endregion

        #region RecarregarSMS
        /// <summary>
        /// Recarrega SMS 
        /// </summary>
        public static void RecarregarSMS(BNE_Plano_Parcela objPlanoParcela)
        {
            int recarga;
            int qtdeJaLiberada = objPlanoParcela.Qtd_SMS_Liberada.HasValue ? objPlanoParcela.Qtd_SMS_Liberada.Value : 0;

            recarga = objPlanoParcela.Qtd_SMS_Total - qtdeJaLiberada;

            PlanoQuantidade.RecarregarSMS(objPlanoParcela.BNE_Plano_Adquirido, recarga);
        }
        #endregion

        #region PrimeiraParcela
        /// <summary>
        /// Verifica qual o número da parcela do Plano Adquirido.
        /// </summary>
        /// <returns>Número da parcela. Se não encontrado, retorna um número negativo.</returns>
        public static Int32 NumeroParcela(int idPlanoAdquirido, int idPlanoParcela, LanEntities context)
        {
            var numParcela = (from pa in context.BNE_Plano_Adquirido
                                  join pp in context.BNE_Plano_Parcela on pa.Idf_Plano_Adquirido equals pp.Idf_Plano_Adquirido
                                  where pp.Idf_Plano_Parcela_Situacao != 3
                                  && pa.Idf_Plano_Adquirido == idPlanoAdquirido
                                  && pp.Idf_Plano_Parcela == idPlanoParcela
                              select pp).ToList().GroupBy(x => new { x.Idf_Plano_Adquirido, x.Idf_Plano_Parcela});

            return numParcela.Count();
        }

        #endregion

    }
}
