using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace BNE.Services
{
    partial class EnvioBoletoAntesDeVencerPlano : BaseService
    {
        #region Construtores
        public EnvioBoletoAntesDeVencerPlano()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnvioBoletoAntesDeVencerPlanoHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnvioBoletoAntesDeVencerPlanoDelay;
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvio);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #endregion

        #region Métodos

        #region IniciarEnvio
        public void IniciarEnvio()
        {
            try
            {
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.InicioExecucao);

                    #region [Plano Experimental]
                    var listaExperimental = PlanoAdquiridoDetalhes.CarregarPlanoAntesDoVencimento(Convert.ToInt32(
                        Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCIAExperimente200)));

                    if (listaExperimental.Rows.Count > 0)
                        EnviarBoletoParaCompraPlanoMensal200(listaExperimental);
                    #endregion

                    #region [Plano Mensal 200]
                    var listaMensal200 = PlanoAdquiridoDetalhes.CarregarPlanoAntesDoVencimento(Convert.ToInt32(
                        Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCIAMensal200)));

                    if (listaMensal200.Rows.Count > 0)
                        EnviarBoletoParaCompraPlanoMensal200(listaMensal200);
                    #endregion

                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);

                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
        #endregion

        #region EnviarBoleto
        private void EnviarBoletoParaCompraPlanoMensal200(DataTable lista)
        {

            string assunto;
            var template = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.RenovacaoPlanoMensal200, out assunto);
            var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);

            foreach (DataRow line in lista.Rows)
            {
                //using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                //{
                //    conn.Open();
                //    using (SqlTransaction trans = conn.BeginTransaction())
                //    {
                        try
                        {
                            var mensagem = template.Replace("{PrimeiroNome}", BLL.Custom.Helper.RetornarPrimeiroNome(line["Nme_Res_Plano_Adquirido"].ToString()));
                            PlanoAdquirido objPlanoAdquirido = null;
                            Plano objPlano = Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCIAMensal200)));
                            Pagamento objPagamento = null;

                            //Esta vencendo o plano experimental - cria o plano recorrente mensal 200
                            if (line["Idf_Plano"].ToString().Equals(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCIAExperimente200).Trim()))
                            {
                                #region [Criar plano mensal 200]
                                UsuarioFilialPerfil objUsuarioFilialPefil = new UsuarioFilialPerfil(Convert.ToInt32(line["Idf_Usuario_Filial_Perfil"]));
                                UsuarioFilial objUsuarioFilial = new UsuarioFilial(Convert.ToInt32(line["Idf_Usuario_Filial"]));
                                objUsuarioFilial.CompleteObject();
                                objUsuarioFilialPefil.CompleteObject();
                                Filial objFilial = new Filial(Convert.ToInt32(line["Idf_Filial"]));
                                objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPJ(objUsuarioFilialPefil,
                                objFilial, objUsuarioFilial, objPlano, objPlano.QuantidadePrazoBoletoMaxima.Value);

                                #region [Criar Pagamento e Boleto]
                                TipoPagamento objTipoPagamento = new TipoPagamento((int)BLL.Enumeradores.TipoPagamento.BoletoBancario);

                                var PagamentoSituacao = new PagamentoSituacao((int)BLL.Enumeradores.PagamentoSituacao.EmAberto);

                                #region [Criar Parcela]
                                var objPlanoParcela = new PlanoParcela();
                                objPlanoParcela.PlanoAdquirido = objPlanoAdquirido;
                                objPlanoParcela.ValorParcela = objPlanoAdquirido.ValorBase;
                                objPlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)BLL.Enumeradores.PlanoParcelaSituacao.EmAberto);
                                objPlanoParcela.FlagInativo = false;
                                objPlanoParcela.QuantidadeSMSTotal = objPlanoAdquirido.QuantidadeSMS / (objPlano.QuantidadeParcela == 0 ? 1 : objPlano.QuantidadeParcela);
                                objPlanoParcela.Save();
                                #endregion

                                #region [Criar Pagamento]
                                objPagamento = new Pagamento();

                                objPagamento.DataEmissao = DateTime.Now;
                                if (objPlano.QuantidadePrazoBoletoMaxima.HasValue)
                                    objPagamento.DataVencimento = DateTime.Now.AddDays(objPlano.QuantidadePrazoBoletoMaxima.Value);
                                objPagamento.PlanoParcela = objPlanoParcela;
                                objPagamento.TipoPagamento = objTipoPagamento;
                                objPagamento.PagamentoSituacao = new PagamentoSituacao((int)BLL.Enumeradores.PagamentoSituacao.EmAberto);
                                objPagamento.UsuarioFilialPerfil = objPlanoParcela.PlanoAdquirido.UsuarioFilialPerfil;
                                objPagamento.Filial = objPlanoParcela.PlanoAdquirido.Filial;
                                objPagamento.FlagAvulso = false;
                                objPagamento.FlagInativo = false;
                                objPagamento.ValorPagamento = objPlanoParcela.ValorParcela;
                                objPagamento.Save();

                                #endregion

                                #endregion

                                #endregion
                            }
                            else//Ja possui plano mensal 200
                            {
                                objPlanoAdquirido = PlanoAdquirido.LoadObject(Convert.ToInt32(line["Idf_Plano_Adquirido"]));
                                var objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(objPlanoAdquirido,null);
                                objPagamento = Pagamento.CriarPagamentoBoletoRecorrencia(objPlanoParcela, objPlanoAdquirido, DateTime.Now.AddDays(objPlano.QuantidadePrazoBoletoMaxima.Value), objPlano.ValorBase);
                            }

                            #region [Criar Boleto]
                            string htmlBoleto = String.Empty;
                            List<BoletoNet.Boleto> boletos = null;
                            List<Pagamento> listPagamento = new List<Pagamento>();
                            listPagamento.Add(objPagamento);
                            boletos = BoletoBancario.CriarBoletos(listPagamento);
                            byte[] pdf = BLL.Custom.PDF.GerarPdfAPartirdoHtml(BoletoBancario.GerarLayoutBoletoHTML(boletos));
                            #endregion

                            //trans.Commit();
                            EmailSenderFactory
                           .Create(TipoEnviadorEmail.Fila)
                           .Enviar(null, null, new UsuarioFilialPerfil(Convert.ToInt32(line["Idf_Usuario_Filial_Perfil"])),
                               assunto, mensagem, BLL.Enumeradores.CartaEmail.RenovacaoPlanoMensal200, emailRemetente, line["Eml_Envio_Boleto"].ToString(), "Boleto_-_BNE_-_Banco_Nacional_de_Empregos.pdf", pdf);

                        }
                        catch (Exception ex)
                        {
                            //trans.Rollback();
                            EL.GerenciadorException.GravarExcecao(ex,
                                String.Format("Erro - no robo EnvioBoletoAntesDeVencerPlano na geração do boleto para o {0} e-mail {1}",
                                line["Nme_Res_Plano_Adquirido"].ToString(), line["Eml_Envio_Boleto"].ToString()));
                        }
                //    }
                //}
            }
             
             
        }
        #endregion
              
        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                var horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(
                    string.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar),
                    EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Envio Boleto Antes de Vencer o Plano - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Envio Boleto Antes de Vencer o Plano  - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion
    }
}
