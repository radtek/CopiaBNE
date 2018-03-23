using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Data.Entity;
using System.Data.Common;

namespace LanHouse.Business
{
    public class PlanoAdquirido
    {
        #region CarregarPorID
        public static BNE_Plano_Adquirido CarregarPorID(int idPlanoAdquirido)
        {
            using (var entity = new LanEntities())
            {
                var retortno = (from pa in entity.BNE_Plano_Adquirido
                                    where pa.Idf_Plano_Adquirido == idPlanoAdquirido
                                    select pa).FirstOrDefault();
                return retortno;
            }

        }
        #endregion

        #region CriarPlanoAdquiridoPF
        public static BNE_Plano_Adquirido CriarPlanoAdquiridoPF(TAB_Usuario_Filial_Perfil objUsuarioFilialPerfil, int? idFilial, BNE_Usuario_Filial objUsuarioFilial, BNE_Plano objPlano, int? quantidadePrazoBoleto = null)
        {
            using (var entity = new LanEntities())
            {
                try
                {
                    //Inserir o Plano Adquirido
                    BNE_Plano_Adquirido objPlanoAdquirido = new BNE_Plano_Adquirido()
                    {
                        Idf_Filial = idFilial,
                        Idf_Plano = objPlano.Idf_Plano,
                        Idf_Usuario_Filial_Perfil = objUsuarioFilialPerfil.Idf_Usuario_Filial_Perfil,
                        Idf_Plano_Situacao = (int)Enumeradores.PlanoSituacao.AguardandoLiberacao,
                        Dta_Cadastro = DateTime.Now,
                        Dta_Inicio_Plano = DateTime.Now,
                        Dta_Fim_Plano = DateTime.Now.AddDays(objPlano.Qtd_Dias_Validade),
                        Qtd_SMS = objPlano.Qtd_SMS,
                        Vlr_Base = objPlano.Vlr_Base,
                        Qtd_Prazo_Boleto = quantidadePrazoBoleto.HasValue ? quantidadePrazoBoleto : 0,
                        Flg_Boleto_Registrado = objPlano.Flg_Boleto_Registrado
                    };
                    entity.BNE_Plano_Adquirido.Add(objPlanoAdquirido);
                    entity.SaveChanges();

                    //PlanoQuantidade.InserirPlanoQuantidade(objPlano, objPlanoAdquirido);
                    //PlanoAdquiridoDetalhes.InserirPlanoAdquiridoDetalhes(objPlano, objPlanoAdquirido, objUsuarioFilial, objUsuarioFilialPerfil);

                    return objPlanoAdquirido;
                }
                catch
                {
                    throw;
                }
            }
            
        }
        #endregion

        #region CriarParcelas
        /// <summary>
        /// Cria as parcelas para o plano adquirido
        /// </summary>
        /// <param name="objTipoPagamento"></param>
        /// <param name="prazoBoleto"></param>
        /// <returns></returns>
        /// 
        public static bool CriarParcelas(BNE_Plano_Adquirido objPlanoAdquirido, BNE_Plano objPlano, int idTipoPagamento, BNE_Codigo_Desconto objCodigoDesconto, int? prazoBoleto)
        {
            try 
            {
                using (var context = new LanEntities())
                {
                    using (var db = context.Database.BeginTransaction())
                    {
                        try
                        {
                            CalcularParcelas(objPlanoAdquirido, objPlano, DateTime.Now.AddDays(prazoBoleto ?? 0), DateTime.Now, idTipoPagamento, objCodigoDesconto, context);
                            context.SaveChanges();
                            db.Commit();
                            return true;
                        }
                        catch
                        {
                            db.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region CalcularParcelas
        public static List<BNE_Pagamento> CalcularParcelas(BNE_Plano_Adquirido objPlanoAdquirido, BNE_Plano objPlano, DateTime dtVencimento, DateTime dtEnvioBoleto, int idTipoPagamento, BNE_Codigo_Desconto objCodigoDesconto, LanEntities context)
        {
            var pagamentos = new List<BNE_Pagamento>();

            for (int i = 0; i < objPlano.Qtd_Parcela; i++)
                pagamentos.Add(PlanoParcela.CriarNovaParcela(objPlanoAdquirido, objPlano, 0, dtVencimento.AddMonths(i), dtEnvioBoleto.AddMonths(i), idTipoPagamento, objCodigoDesconto, context));

            return pagamentos;
        }
        #endregion

        #region CriarPagamento
        public static BNE_Pagamento CriarPagamento(BNE_Plano_Parcela objPlanoParcela, BNE_Plano objPlano, TAB_Filial objFilial, TAB_Usuario_Filial_Perfil objUsuarioFilialPerfil, int idTipoPagamento, decimal? valorPagamento = null, int? idCodigoDesconto = null)
        {
            using (var entity = new LanEntities())
            {
                var objPagamentoNovo = new BNE_Pagamento
                {
                    Idf_Tipo_Pagamento = idTipoPagamento,
                    Idf_Plano_Parcela = objPlanoParcela.Idf_Plano_Parcela,
                    Dta_Emissao = DateTime.Now,
                    Dta_Vencimento = DateTime.Today,
                    Idf_Usuario_Filial_Perfil = objUsuarioFilialPerfil.Idf_Usuario_Filial_Perfil,
                    Vlr_Pagamento = valorPagamento.HasValue ? valorPagamento.Value : objPlanoParcela.BNE_Plano_Adquirido.Vlr_Base,
                    Idf_Pagamento_Situacao = (int)Enumeradores.PagamentoSituacao.EmAberto,
                    Idf_Filial = objFilial.Idf_Filial
                };

                if (idCodigoDesconto.HasValue)
                    objPagamentoNovo.Idf_Codigo_Desconto = idCodigoDesconto.Value;

                //Caso o tipo do pagamento seja débito, define o número de dias do vencimento baseado no parâmetro.
                //Necessário pois alguns bancos pedem um limite mínimo de dias para o envio da requisição de débito
                Int32 diasMinimoParaVencimento = Convert.ToInt32(new Business.Parametro().GetById((int)Enumeradores.Parametro.PagamentoDebitoDiasMinimosVencimentoParaEnvio).Vlr_Parametro);
                if (idTipoPagamento == (int)Enumeradores.TipoPagamento.DebitoRecorrente)
                    objPagamentoNovo.Dta_Vencimento = DateTime.Now.AddDays(diasMinimoParaVencimento);

                entity.BNE_Pagamento.Add(objPagamentoNovo);
                entity.SaveChanges();

                return objPagamentoNovo;
            }

        }
        #endregion

        #region AtualizarPagamento
        /// <summary>
        /// Metodo responsável por atualizar o pagamento
        /// </summary>
        /// <param name="objPagamento">Object</param>
        /// <param name="objTipoPagamento">Object</param>
        /// <param name="objPlanoAdquirido">Plano adquirido que está sendo manipulado pelo processo de pagamento </param>
        /// <param name="objPlano">O plano escolhido pelo usuário para efetuar a compra </param>
        /// <returns>Object</returns>
        public static BNE_Pagamento AtualizarPagamento(BNE_Pagamento objPagamento, int idTipoPagamento, BNE_Plano_Adquirido objPlanoAdquirido, BNE_Plano objPlano)
        {
            using (var entity = new LanEntities())
            {
                objPagamento = (from pagamento in entity.BNE_Pagamento
                                    where pagamento.Idf_Pagamento == objPagamento.Idf_Pagamento
                                    select pagamento).FirstOrDefault();

                objPagamento.Idf_Tipo_Pagamento = idTipoPagamento;
                objPagamento.Vlr_Pagamento = objPlanoAdquirido.Vlr_Base;
                objPagamento.Dta_Emissao = DateTime.Now;
                objPagamento.Dta_Vencimento = DateTime.Today;
                objPagamento.Idf_Pagamento_Situacao = (int)Enumeradores.PagamentoSituacao.EmAberto;

                entity.SaveChanges();
                return objPagamento;
            }
        }
        #endregion

        #region Liberar
        /// <summary>
        /// Efeuta a liberação do plano adquirido
        /// </summary>
        /// <param name="ajustarDatas">Ajusta a data do início do plano para que o VIP/CIA não perca dias de utilização</param>
        /// <param name="trans">SQL Transaction</param>
        /// <returns>Se o plano foi liberado, retorna true. Caso o VIP/CIA já tenha um plano liberado, marca o plano para liberação futura, retornando false.</returns>
        public static bool Liberar(BNE_Plano_Adquirido objPlanoAdquirido, bool ajustarDatas, LanEntities context)
        {
            //Verifica se o plano é plano para liberação futura. 
            //Utilizado para planos de renovação
            //Se existe um plano liberado, e a data de início do plano é maior que a data e o plano adquirido não for para plano adicional
            if (objPlanoAdquirido.BNE_Plano != null)
            {
                BNE_Plano_Adquirido objPlanoAdquiridoLiberado;
                if (ParaPessoaFisica(objPlanoAdquirido))
                {
                    BNE_Curriculo objCurriculo;
                    Curriculo.CarregarPorIdPessoaFisica(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Idf_Pessoa_Fisica, out objCurriculo, context);
                    
                    if (CarregarPlanoAdquiridoPorSituacao(objCurriculo, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado, context)
                        && objPlanoAdquiridoLiberado.Idf_Plano_Adquirido != objPlanoAdquirido.Idf_Plano_Adquirido)
                    {
                        objPlanoAdquirido.Idf_Plano_Situacao = (int)Enumeradores.PlanoSituacao.LiberacaoFutura;
                    }
                    else
                    {
                        objPlanoAdquirido.Idf_Plano_Situacao = (int)Enumeradores.PlanoSituacao.Liberado;
                    }
                }
                else
                {
                    if (CarregarPlanoAdquiridoPorSituacao(objPlanoAdquirido.TAB_Filial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado, context)
                        && objPlanoAdquiridoLiberado.Idf_Plano_Adquirido != objPlanoAdquirido.Idf_Plano_Adquirido)
                    {
                        objPlanoAdquirido.Idf_Plano_Situacao = (int)Enumeradores.PlanoSituacao.LiberacaoFutura;
                    }
                    else
                    {
                        objPlanoAdquirido.Idf_Plano_Situacao = (int)Enumeradores.PlanoSituacao.Liberado;
                    }
                }
            }

            //TODO: chamada ao assincrono, implementar
            //if (objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado)
            //{
            //    if (Plano.IdPlano == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.VendaPlanoCIA_PlanoSmsEmailVagaIdentificador)))
            //    {
            //        int idVaga;
            //        string codVaga;
            //        if (PlanoAdquiridoDetalhes.RecuperarIdVagaPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out idVaga, out codVaga) && idVaga > 0)
            //        {
            //            var parametros = new ParametroExecucaoCollection 
            //                    {
            //                        {"idVaga","Vaga",idVaga.ToString(CultureInfo.InvariantCulture), codVaga ?? string.Empty}
            //                    };

            //            ProcessoAssincrono.IniciarAtividade(
            //                BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil,
            //                PluginsCompatibilidade.CarregarPorMetadata("EnvioCandidatoVagaPerfil", "PluginSaidaEmailSMSTanque"),
            //                parametros,
            //                null,
            //                null,
            //                null,
            //                null,
            //                DateTime.Now);
            //        }
            //    }

            //    if (ajustarDatas)
            //    {
            //        if (objPlanoAdquirido.DataInicioPlano < DateTime.Now &&
            //            objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.AguardandoLiberacao))
            //            objPlanoAdquirido.DataInicioPlano = DateTime.Now;

            //        // Atribui a data de vencimento a quantidade de dias referente a soma da data de inicio e a cadastrada no plano.
            //        objPlanoAdquirido.DataFimPlano = objPlanoAdquirido.DataInicioPlano.AddDays(objPlanoAdquirido.Plano.QuantidadeDiasValidade);
            //    }
            //}

            context.SaveChanges();

            if (objPlanoAdquirido.Idf_Plano_Situacao == (int)Enumeradores.PlanoSituacao.Liberado)
                return true;

            return false;
        }
        #endregion

        #region ParaPessoaFisica
        /// <summary>
        /// O plano adquirido é para pessoa fisica?
        /// </summary>
        /// <returns>true se foi plano para pessoa fisica, false se nao</returns>
        public static bool ParaPessoaFisica(BNE_Plano_Adquirido objPlanoAdquirido)
        {
            return objPlanoAdquirido.BNE_Plano.Idf_Plano_Tipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica);
        }
        #endregion

        #region ParaPessoaJuridica
        /// <summary>
        /// O plano adquirido é para pessoa fisica?
        /// </summary>
        /// <returns>true se foi plano para pessoa fisica, false se nao</returns>
        public static bool ParaPessoaJuridica(BNE_Plano_Adquirido objPlanoAdquirido)
        {
            return objPlanoAdquirido.BNE_Plano.Idf_Plano_Tipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica);
        }
        #endregion

        #region CarregarPlanoAdquiridoPorSituacao
        /// <summary>
        /// Método responsável por carregar uma instancia de plano candidato
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool CarregarPlanoAdquiridoPorSituacao(BNE_Curriculo objCurriculo, int idfPlanoSituacao, out BNE_Plano_Adquirido objPlanoAdquirido, LanEntities context)
        {
            objPlanoAdquirido = (from planoAdquirido in context.BNE_Plano_Adquirido
                                 join ufp in context.TAB_Usuario_Filial_Perfil on planoAdquirido.Idf_Usuario_Filial_Perfil equals ufp.Idf_Usuario_Filial_Perfil
                                 join curriculo in context.BNE_Curriculo on ufp.Idf_Pessoa_Fisica equals curriculo.Idf_Pessoa_Fisica
                                        where planoAdquirido.Idf_Plano_Situacao == idfPlanoSituacao
                                        && curriculo.Idf_Curriculo == objCurriculo.Idf_Curriculo
                                        && ufp.Idf_Filial == null
                                        select planoAdquirido).FirstOrDefault();


            return objPlanoAdquirido != null;

        }
        public static bool CarregarPlanoAdquiridoPorSituacao(TAB_Filial objFilial, int idfPlanoSituacao, out BNE_Plano_Adquirido objPlanoAdquirido, LanEntities context)
        {
            objPlanoAdquirido = (from planoAdquirido in context.BNE_Plano_Adquirido
                                    where planoAdquirido.Idf_Plano_Situacao == idfPlanoSituacao
                                    && planoAdquirido.Idf_Filial == objFilial.Idf_Filial
                                    select planoAdquirido).FirstOrDefault();

            return objPlanoAdquirido != null;

        }
        #endregion

        #region CarregarPlanoAdquiridoPorSituacao
        /// <summary>
        /// Método responsável por carregar uma instancia de plano candidato
        /// </summary>
        /// <returns>Boolean</returns>
        public static List<BNE_Plano_Adquirido> CarregarPlanosAdquiridoPorSituacaoECV(int idCurriculo, int idfPlanoSituacao, LanEntities context)
        {
            List<BNE_Plano_Adquirido> planosAdquiridos = (from planoAdquirido in context.BNE_Plano_Adquirido
                                                            join ufp in context.TAB_Usuario_Filial_Perfil on planoAdquirido.Idf_Usuario_Filial_Perfil equals ufp.Idf_Usuario_Filial_Perfil
                                                            join cv in context.BNE_Curriculo on ufp.Idf_Pessoa_Fisica equals cv.Idf_Curriculo
                                                            where planoAdquirido.Idf_Plano_Situacao == idfPlanoSituacao
                                                            && cv.Idf_Curriculo == idCurriculo
                                                            && ufp.Idf_Filial == null
                                                                select planoAdquirido).ToList();
                                                          

            return planosAdquiridos;
        }
        public static List<BNE_Plano_Adquirido> CarregarPlanosAdquiridoPorSituacaoEFilial(int idFilial, int idfPlanoSituacao, LanEntities context)
        {
                List<BNE_Plano_Adquirido> planosAdquiridos = (from planoAdquirido in context.BNE_Plano_Adquirido
                                                             where planoAdquirido.Idf_Filial == idFilial 
                                                             && planoAdquirido.Idf_Plano_Situacao == idfPlanoSituacao
                                                             select planoAdquirido).ToList();

                return planosAdquiridos;
        }
        #endregion

        #region CancelarPlanoAdquirido
        public static bool CancelarPlanoAdquirido(BNE_Plano_Adquirido objPlanoAdquirido, string nomeProcessoPaiParaSalvarNoCRM, bool derrubarVIP, LanEntities context, int? idCurriculo = null)
        {
            //Cancelar Plano Adquirido
            objPlanoAdquirido.Idf_Plano_Situacao = 3;

            //Cancelar Plano Parcela
            List<BNE_Plano_Parcela> planoParcela = (from pp in context.BNE_Plano_Parcela
                                                    join pagamento in context.BNE_Pagamento on pp.Idf_Plano_Parcela equals pagamento.Idf_Plano_Parcela
                                                    where pp.Idf_Plano_Adquirido == objPlanoAdquirido.Idf_Plano_Adquirido
                                                    && (pagamento.Des_Identificador == null && pagamento.Idf_Pagamento_Situacao != 2
                                                    || (pagamento.Des_Identificador != null && pagamento.Idf_Pagamento_Situacao != 1 && pagamento.Idf_Pagamento_Situacao != 2))
                                                    select pp).ToList();

            foreach (var el in planoParcela)
                el.Idf_Plano_Parcela_Situacao = 3;


            //Cancelar Pagamento
            List<BNE_Pagamento> oPagamento = (from pagamento in context.BNE_Pagamento
                                              join pp in context.BNE_Plano_Parcela on pagamento.Idf_Plano_Parcela equals pp.Idf_Plano_Parcela
                                                    where pp.Idf_Plano_Adquirido == objPlanoAdquirido.Idf_Plano_Adquirido
                                                    && (pagamento.Des_Identificador == null && pagamento.Idf_Pagamento_Situacao != 2
                                                        || (pagamento.Des_Identificador != null && pagamento.Idf_Pagamento_Situacao != 1 && pagamento.Idf_Pagamento_Situacao != 2))
                                                    select pagamento).ToList();

            foreach (var el in oPagamento) {
                el.Idf_Pagamento_Situacao = 3;
                el.Flg_Inativo = true;
            }


            //Cancelar Plano Adicional
            List<BNE_Adicional_Plano> planoAdc = (from planoAdicional in context.BNE_Adicional_Plano
                                                where planoAdicional.Idf_Plano_Adquirido == objPlanoAdquirido.Idf_Plano_Adquirido
                                                select planoAdicional).ToList();

            foreach (var el in planoAdc)
                el.Idf_Adicional_Plano_Situacao = 4;

            //Cancelar Pagamento Plano Adicional
            List<BNE_Pagamento> pagamentoAdc = (from pagamento in context.BNE_Pagamento
                                                join planoAdicional in context.BNE_Adicional_Plano on pagamento.Idf_Adicional_Plano equals planoAdicional.Idf_Adicional_Plano
                                                    where planoAdicional.Idf_Plano_Adquirido == objPlanoAdquirido.Idf_Plano_Adquirido
                                                    select pagamento).ToList();
            foreach (var el in pagamentoAdc)
            {
                el.Idf_Pagamento_Situacao = 3;
                el.Flg_Inativo = true;
            }


            //Cancelar Plano Quantidade
            List<BNE_Plano_Quantidade> planoQtde = (from pq in context.BNE_Plano_Quantidade
                                                    where pq.Idf_Plano_Adquirido == objPlanoAdquirido.Idf_Plano_Adquirido
                                                    select pq).ToList();

            foreach (var el in planoQtde)
                el.Flg_Inativo = true;


            if (derrubarVIP) //A SP derruba o FLG_Vip se o Identificador do Currículo for passado.
            {
                if (idCurriculo != null)
                {
                    BNE_Curriculo oCv = (from cv in context.BNE_Curriculo
                                         where cv.Idf_Curriculo == idCurriculo
                                         select cv).FirstOrDefault();

                    oCv.Flg_VIP = false;
                }
            }
            context.SaveChanges();


            var descricaoCRM = string.Concat("Plano adquirido ", objPlanoAdquirido.Idf_Plano_Adquirido, " cancelado!");
            if (idCurriculo.HasValue)
            {
                CurriculoObservacao.SalvarCRM(descricaoCRM, idCurriculo.Value, nomeProcessoPaiParaSalvarNoCRM, context);
            }
            else
            {
                FilialObservacao.SalvarCRM(descricaoCRM, objPlanoAdquirido.Idf_Filial.Value, nomeProcessoPaiParaSalvarNoCRM, context);
            }

            return true;
        }
        #endregion
    }
}
