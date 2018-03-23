using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL;
using BNE.Services.Pagamento.Properties;

namespace BNE.Services.Pagamento
{
    partial class EmissaoBoletoTerminoVip : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static readonly int _maximoThreads = Convert.ToInt32(ConfigurationManager.AppSettings["MaximoThreads"]);
        private static readonly string _horaMinutoExecucao = ConfigurationManager.AppSettings["HoraMinutoExecucao"];
        #endregion

        #region Construtores
        public EmissaoBoletoTerminoVip()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            ThreadStart objTs = new ThreadStart(IniciarEmissaoBoletoTerminoVip);
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

        #region IniciarEmissaoBoletoTerminoVip
        public void IniciarEmissaoBoletoTerminoVip()
        {
            try
            {
                bool executou = false;
                while (true)
                {
                    if ((DateTime.Now.Hour + ":" + DateTime.Now.Minute).Equals(_horaMinutoExecucao))
                    {
                        if (!executou)
                        {
                            executou = true;
                            try
                            {
                                List<PessoaFisica> listPessoaFisica = PlanoAdquirido.RecuperarItensEmissaoBoletoVIP(_maximoThreads);

                                if (listPessoaFisica.Count > 0)
                                {
                                    foreach (PessoaFisica objPessoaFisica in listPessoaFisica)
                                        SalvarEmissaoBoletoTerminoVip(objPessoaFisica);

                                    //    ThreadPool.QueueUserWorkItem(new WaitCallback(SalvarEmissaoBoletoTerminoVip), objPessoaFisica);
                                }
                            }
                            catch { }
                        }
                        else
                            System.Threading.Thread.Sleep(Settings.Default.DelayEmissaoBoletoTerminoVip);
                    }
                    else
                    {
                        executou = false;
                        System.Threading.Thread.Sleep(Settings.Default.DelayEmissaoBoletoTerminoVip);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region SalvarEmissaoBoletoTerminoVip
        private void SalvarEmissaoBoletoTerminoVip(Object objPF)
        {
            try
            {
                PessoaFisica objPessoaFisica = (PessoaFisica)objPF;
                String strDescricaoMensagem, strCodigoBoleto;
                PlanoAdquirido.SalvarNovoPlanoBoletoVIP(objPessoaFisica, out strDescricaoMensagem, out strCodigoBoleto);
            }
            catch { }
        }
        #endregion

        #endregion

    }
}
