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
    public class Pagamento
    {
        #region CarregarPorId
        public static BNE_Pagamento CarregarPorId(int IdPagamento)
        {
            using (var entity = new LanEntities())
            {
                return (from pagamento in entity.BNE_Pagamento
                        where pagamento.Idf_Pagamento == IdPagamento
                        select pagamento).FirstOrDefault();
            }
        }
        public static void AtualizarDesIdentificador(int IdPagamento, string Des_Identificador, LanEntities context)
        {
            var objPagamento = (from pagamento in context.BNE_Pagamento
                    where pagamento.Idf_Pagamento == IdPagamento
                    select pagamento).FirstOrDefault();

            objPagamento.Des_Identificador = "Liberado por cupom de desconto";

            context.SaveChanges();
        }
        #endregion

        #region ConcederDescontoIntegral
        /// <summary>
        /// Concede e libera desconto integral ao usuario
        /// </summary>
        /// <param name="idUsuarioFilialPerfil">id do objeto UsuarioFilialPerfil</param>
        /// <param name="idPlano">id do objeto Plano</param>
        /// <param name="idCodigoDesconto">id do objeto CodigoDesconto</param>
        /// <param name="erro">mensagem de erro</param>
        /// <returns>true se a liberação ocorreu corretamente, false se houve erro, sendo que o parâmetro erro contém a mensagem relacionada</returns>
        public static bool ConcederDescontoIntegral(BNE_Curriculo objCurriculo, TAB_Usuario_Filial_Perfil objUsuarioFilialPerfil, BNE_Plano objPlano, BNE_Codigo_Desconto objCodigoDesconto, out string erro)
        {
            erro = null;

            BNE_Pagamento objPagamento = null;

            if (objPlano.Idf_Plano_Tipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
            {
                BNE_Plano_Adquirido objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPF(objUsuarioFilialPerfil, null, null, objPlano);

                bool parcelarCriadas = PlanoAdquirido.CriarParcelas(objPlanoAdquirido, objPlano, (int)Enumeradores.TipoPagamento.Parceiro, objCodigoDesconto, null);

                if (!parcelarCriadas)
                { 
                    erro = "Erro na criação das parcelas";
                    return false;
                }
                  
                BNE_Plano_Parcela objPlanoParcela;
                //Se o valor base do plano for zerado, parcelas já estão pagas
                if (objPlano.Vlr_Base > 0)
                    objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido.Idf_Plano_Adquirido);
                else
                    objPlanoParcela = PlanoParcela.CarregarPrimeiraParcelaPorPlanoAdquirido(objPlanoAdquirido.Idf_Plano_Adquirido);

                // cria ou recupera o pagamento para o plano adquirido
                List<BNE_Pagamento> objListPagamentosPorParcela = CarregaPagamentosPorPlanoParcela(objPlanoParcela.Idf_Plano_Parcela);

                if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                {
                    objPagamento = objListPagamentosPorParcela
                        .FirstOrDefault(p =>
                            (p.Idf_Tipo_Pagamento == (int)Enumeradores.TipoPagamento.Parceiro)
                            && p.Flg_Inativo == false);

                    // Atualiza pagamentos selecionados com o tipo == null, caso não exista entao cria um novo.
                    if (objPagamento == null)
                        objPagamento =
                            PlanoAdquirido.CriarPagamento(objPlanoParcela, objPlano, null, objUsuarioFilialPerfil,
                            (int)Enumeradores.TipoPagamento.Parceiro);

                    PlanoAdquirido.AtualizarPagamento(objPagamento,
                        (int)Enumeradores.TipoPagamento.Parceiro,
                        objPlanoAdquirido,
                        objPlano);


                    using (var context = new LanEntities())
                    {
                        using (var dbTrans = context.Database.BeginTransaction())
                        {
                            try
                            {
                                // liberando por cupom de desconto
                                Pagamento.AtualizarDesIdentificador(objPagamento.Idf_Pagamento, "Liberado por cupom de desconto", context);
                                ConcederDesconto(objCodigoDesconto.Idf_Codigo_Desconto, objPagamento.Idf_Pagamento, context);
                                Liberar(objPagamento.Idf_Pagamento, DateTime.Now, context);
                                context.SaveChanges();
                                dbTrans.Commit();
                                return true;
                            }
                            catch(Exception ex)
                            {
                                erro = ex.Message;
                                dbTrans.Rollback();
                                throw;
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    erro = "Não há registro de pagamento associado ao perfil do usuário";
                }
            }
            else
            {
                erro = "O plano selecionado não é para pessoa física";
            }

            return false;
        }
        #endregion

        #region CarregaPagamentosPorPlanoParcela
        /// <summary>
        /// Carrega todos os pagamentos em aberto referente "objPlanoParcela" passados como parâmetros
        /// </summary>
        /// <param name="objPlanoParcela"></param>
        /// <returns></returns>
        public static List<BNE_Pagamento> CarregaPagamentosPorPlanoParcela(int idPlanoParcela, LanEntities context)
        {
            List<BNE_Pagamento> lstPagamentos;

            lstPagamentos = (from pagamento in context.BNE_Pagamento
                                where pagamento.Idf_Plano_Parcela == idPlanoParcela
                                select pagamento).ToList();

            return lstPagamentos;
        }
        public static List<BNE_Pagamento> CarregaPagamentosPorPlanoParcela(int idPlanoParcela)
        {
            using (var entity = new LanEntities())
            {
                List<BNE_Pagamento> lstPagamentos;

                lstPagamentos = (from pagamento in entity.BNE_Pagamento
                                 where pagamento.Idf_Plano_Parcela == idPlanoParcela
                                 select pagamento).ToList();

                return lstPagamentos;
            }
        }
        #endregion

        #region ConcederDesconto
        /// <summary>
        /// Concede o desconto ao objeto Pagamento,
        /// chamar logo antes do procedimento que autoriza o pagamento do objeto Pagamento
        /// </summary>
        /// <returns>true se a concessão ocorreu corretamente, false se o objeto cupom de desconto ja tiver sido usado ou se o objeto Pagamento ja tiver um codigo de desconto</returns>
        public static bool ConcederDesconto(int idCodigoDesconto, int idPagamento, LanEntities context)
        {
            var objPagamento = (from pagamento in context.BNE_Pagamento
                                where pagamento.Idf_Pagamento == idPagamento
                                select pagamento).FirstOrDefault();

            var objCodigoDesconto = (from codigoDesconto in context.BNE_Codigo_Desconto
                                         where codigoDesconto.Idf_Codigo_Desconto==idCodigoDesconto
                                         select codigoDesconto).FirstOrDefault();

            context.Entry(objCodigoDesconto).Reference(b => b.BNE_Tipo_Codigo_Desconto).Load();

            decimal valor = objPagamento.Vlr_Pagamento;

            if (!CodigoDesconto.JaUtilizado(objCodigoDesconto))
            {
                CodigoDesconto.CalcularDesconto(ref valor, objCodigoDesconto);

                objPagamento.Vlr_Pagamento = valor;
                objPagamento.Idf_Codigo_Desconto = objCodigoDesconto.Idf_Codigo_Desconto;
                context.SaveChanges();
                return true;
            }

            return false;
        }
        #endregion

        #region Liberar
        /// <summary>
        /// Libera o pagamento
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static bool Liberar(int idPagamento, DateTime dataPagamento, LanEntities context)
        {
            try
            {
                var query = (from pagamento in context.BNE_Pagamento
                             where pagamento.Idf_Pagamento == idPagamento
                                select pagamento);
                var objPagamento = query.Single();

                //Load objetos internos
                context.Entry(objPagamento).Reference(e => e.BNE_Codigo_Desconto).Load();
                context.Entry(objPagamento).Reference(e => e.BNE_Plano_Parcela).Load();
                context.Entry(objPagamento.BNE_Plano_Parcela).Reference(e => e.BNE_Plano_Adquirido).Load();
                context.Entry(objPagamento.BNE_Plano_Parcela.BNE_Plano_Adquirido).Reference(e => e.BNE_Plano).Load();

                //Caso seja um pagamento de adicional de SMS, soma a quantidade de SMS ao plano quantidade vigente.
                if (objPagamento.Idf_Adicional_Plano != null)
                {
                    PlanoAdicional.LiberarPlanoAdicional(objPagamento.Idf_Adicional_Plano.Value, context); //Alterando a situação do plano adiconal para PAGO
                }
                else
                {
                    PlanoParcela.Liberar(objPagamento.BNE_Plano_Parcela, objPagamento, dataPagamento, context);
                    PlanoAdquirido.Liberar(objPagamento.BNE_Plano_Parcela.BNE_Plano_Adquirido, true, context);
                }

                // usa codigo de credito, caso haja
                if (objPagamento.Idf_Codigo_Desconto != null)
                    CodigoDesconto.Utilizar(objPagamento.BNE_Codigo_Desconto, context);

                objPagamento.Idf_Pagamento_Situacao = (int)Enumeradores.PagamentoSituacao.Pago;
                context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
