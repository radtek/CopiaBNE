using System;
using System.ServiceProcess;
using BNE.BLL;
using BNE.EL;
using Quartz;
using Quartz.Impl;
using System.IO;
using System.Data;
using PagarMe;
using System.Data.SqlClient;
using System.Configuration;

namespace BNE.Services
{
    partial class VerificaPagamentoBoleto : ServiceBase, IJob
    {
        private static readonly string DefaultApiKeyPagarMe = ConfigurationManager.AppSettings["DefaultApiKeyPagarMe"];
        private static readonly string DefaultEncryptionKeyPagarMe = ConfigurationManager.AppSettings["DefaultEncryptionKeyPagarMe"];
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public VerificaPagamentoBoleto()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            int countLiberados = 0;
            try
            {
                GravaLogText("Iniciou o VerificaPagamentoBoleto em " + DateTime.Now);

                PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
                PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;
         
                DataTable dtBoletos = BoletoBancario.ConsultarPagamentoBoletosPendente();
                int IdUsuarioSistema = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada));

                foreach (DataRow item in dtBoletos.Rows)
                {
                    try
                    {
                        Transaction objTran = PagarMeService.GetDefaultService().Transactions.Find(item["Des_Identificador"].ToString());
                        switch (objTran.Status)
                        {
                            case TransactionStatus.None:
                                break;
                            case TransactionStatus.Processing:
                                break;
                            case TransactionStatus.Refused:
                                break;
                            case TransactionStatus.Authorized:
                                break;
                            case TransactionStatus.WaitingPayment:
                                break;
                            case TransactionStatus.Paid:
                                #region [Liberar Pagamento]
                                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                                {
                                    conn.Open();
                                    using (SqlTransaction trans = conn.BeginTransaction())
                                    {

                                        try
                                        {
                                            var objBoleto = BoletoBancario.LoadObject(Convert.ToInt32(item["Idf_Boleto_Bancario"]));
                                            objBoleto.Pagamento.CompleteObject();
                                            //no site do pargar me aparece no horario certo mas a api retorna com duas horas a mais.
                                            objBoleto.Liquidar(trans, objTran.DateUpdated.Value.AddHours(-2), IdUsuarioSistema, objBoleto.Pagamento.ValorPagamento);

                                            if (objBoleto.Pagamento.PlanoParcela != null)
                                            {
                                                var parcelaFinal =
                                                PlanoParcela.CarregarUltimaParcelaPorPlanoAdquirido(
                                                    objBoleto.Pagamento.PlanoParcela.PlanoAdquirido
                                                        .IdPlanoAdquirido, trans);


                                                if (parcelaFinal != null && objBoleto.Pagamento.PlanoParcela.PlanoAdquirido.Plano != null
                                                    && parcelaFinal.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)BLL.Enumeradores.PlanoParcelaSituacao.Pago
                                                    && objBoleto.Pagamento.PlanoParcela.PlanoAdquirido.Plano.FlagRecorrente
                                                    && objBoleto.Pagamento.PlanoParcela.PlanoAdquirido.Plano.FlagBoletoRecorrente
                                                    )
                                                {
                                                    objBoleto.GerarNovoPagamento(objBoleto, trans);
                                                }


                                            }

                                            countLiberados++;
                                            trans.Commit();
                                        }
                                        catch (Exception ex)
                                        {
                                            EL.GerenciadorException.GravarExcecao(ex, $"erro ao liberar plano ao boleto {item["Idf_Boleto_Bancario"].ToString()}");
                                            trans.Rollback();
                                        }

                                    }
                                    conn.Close();
                                }

                                #endregion

                                break;
                            case TransactionStatus.PendingRefund:
                                break;
                            case TransactionStatus.Refunded:
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

             
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }

            GravaLogText("Finalizou o VerificaPagamentoBoleto em " + DateTime.Now + " LIberados: " + countLiberados);
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();

           
            var job = JobBuilder.Create<VerificaPagamentoBoleto>()
                .Build();

            ITrigger trigger;

            trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(30)
                        .OnEveryDay()
                  )
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

      

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
        private static void GravaLogText(string mensagem)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + mensagem);
            }
        }

    }
}