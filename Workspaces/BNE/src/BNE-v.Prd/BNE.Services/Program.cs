using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using System.ServiceProcess;

namespace BNE.Services
{
    internal static class Program
    {

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {


            EventLogWriter log = new EventLogWriter(Settings.Default.LogName, "BNE.Services");

            ServiceBase[] servicesToRun = null;

            if (args.Length > 0)
            {
                log.LogEvent("Inicializando " + args[0], System.Diagnostics.EventLogEntryType.Information, Event.AjusteExecucao);

                switch (args[0])
                {
                    case "BNE.Services.AtualizarCurriculo":
                        servicesToRun = new ServiceBase[] { new AtualizarCurriculo() };
                        break;
                    case "BNE.Services.AtualizarPlano":
                        servicesToRun = new ServiceBase[] { new AtualizarPlano() };
                        break;
                    case "BNE.Services.ArquivarVaga":
                        servicesToRun = new ServiceBase[] { new ArquivarVaga() };
                        break;
                    case "BNE.Services.EnvioSMSSemanal":
                        servicesToRun = new ServiceBase[] { new EnvioSMSSemanal() };
                        break;
                    case "BNE.Services.AtualizarEmpresa":
                        servicesToRun = new ServiceBase[] { new AtualizarEmpresa() };
                        break;
                    case "BNE.Services.EnviarCurriculo":
                        servicesToRun = new ServiceBase[] { new EnviarCurriculo() };
                        break;
                    case "BNE.Services.AtualizaSitemap":
                        servicesToRun = new ServiceBase[] { new AtualizaSitemap() };
                        break;
                    case "BNE.Services.InativarCurriculo":
                        servicesToRun = new ServiceBase[] { new InativarCurriculo() };
                        break;
                    case "BNE.Services.BuscaCoordenada":
                        servicesToRun = new ServiceBase[] { new BuscaCoordenada() };
                        break;
                    case "BNE.Services.IntegrarVagas":
                        servicesToRun = new ServiceBase[] { new IntegrarVagas() };
                        break;
                    case "BNE.Services.IntegracaoWebfopag":
                        servicesToRun = new ServiceBase[] { new IntegracaoWebfopag() };
                        break;
                    case "BNE.Services.DestravaSMSPlanoEmployer":
                        servicesToRun = new ServiceBase[] { new DestravaSMSPlanoEmployer() };
                        break;
                    case "BNE.Services.ControleParcelas":
                        servicesToRun = new ServiceBase[] { new ControleParcelas() };
                        break;
                    case "BNE.Services.EnviarEmailAlertaExperienciaProfissional":
                        servicesToRun = new ServiceBase[] { new EnviarEmailAlertaExperienciaProfissional() };
                        break;
                    case "BNE.Services.EnviarEmailAlertaMensalFilial":
                        servicesToRun = new ServiceBase[] { new EnviarEmailAlertaMensalFilial() };
                        break;
                    case "BNE.Services.EmailFiliais":
                        servicesToRun = new ServiceBase[] { new EmailFiliais() };
                        break;
                    case "BNE.Services.EnvioSMSEmpresas":
                        servicesToRun = new ServiceBase[] { new EnvioSMSEmpresas() };
                        break;
                    case "BNE.Services.ControleFinanceiro":
                        servicesToRun = new ServiceBase[] { new ControleFinanceiro() };
                        break;
                    case "BNE.Services.EnvioSMSEmailEmpresasCvsNaoVistos":
                        servicesToRun = new ServiceBase[] { new EnvioSMSEmailEmpresasCvsNaoVistos() };
                        break;
                    case "BNE.Services.SondaBancoDoBrasil":
                        servicesToRun = new ServiceBase[] { new SondaBancoDoBrasil() };
                        break;
                    case "BNE.Services.DebitoOnlineBradesco":
                        servicesToRun = new ServiceBase[] { new DebitoOnlineBradesco() };
                        break;
                    case "BNE.Services.EnviarEmailConfirmacaoCandidatura":
                        servicesToRun = new ServiceBase[] { new EnviarEmailConfirmacaoCandidatura() };
                        break;
                    case "BNE.Services.EnviarEmailOportunidade":
                        servicesToRun = new ServiceBase[] { new EnviarEmailOportunidade() };
                        break;
                    case "BNE.Services.EnvioEmailParaSMSNaoRecebida":
                        servicesToRun = new ServiceBase[] { new EnvioEmailParaSMSNaoRecebida() };
                        break;
                    case "BNE.Services.QuantidadeInsuficienteRetornoPesquisaCurriculo":
                        servicesToRun = new ServiceBase[] { new QuantidadeInsuficienteRetornoPesquisaCurriculo() };
                        break;
                    case "BNE.Services.EnvioBoletoAntesDeVencerPlano":
                        servicesToRun = new ServiceBase[] { new EnvioBoletoAntesDeVencerPlano() };
                        break;
                    case "BNE.Services.CIACarrinhoAbandonado":
                        servicesToRun = new ServiceBase[] { new CIACarrinhoAbandonado() };
                        break;
                    //case "BNE.Services.jornalPopup":
                    //    servicesToRun = new ServiceBase[] { new jornalPopup() };
                    //    break;
                    case "BNE.Services.BoletosRecorrentesAVencer":
                        servicesToRun = new ServiceBase[] { new BoletosRecorrentesAVencer() };
                        break;
                    case "BNE.Services.ControleNotasAntecipadas":
                        servicesToRun = new ServiceBase[] { new ControleNotasAntecipadas() };
                        break;
                    case "BNE.Services.InscritosSTC":
                        servicesToRun = new ServiceBase[] { new InscritosSTC() };
                        break;
                    //case "BNE.Services.NotificarAtualizacaoPesquisaPoucosCVS":
                    //    servicesToRun = new ServiceBase[] { new NotificarAtualizacaoPesquisaPoucosCVS() };
                    //    break;
                    case "BNE.Services.NotificacaoPesquisaCurriculoAtendimento":
                        servicesToRun = new ServiceBase[] { new NotificacaoPesquisaCurriculoAtendimento() };
                        break;
                    case "BNE.Services.AlertaTentativaCompra":
                        servicesToRun = new ServiceBase[] { new AlertaTentativaCompra() };
                        break;
                    case "BNE.Services.AlertaPoucosCvsParaEmpresa":
                        servicesToRun = new ServiceBase[] { new AlertaPoucosCvsParaEmpresa() };
                        break;
                    case "BNE.Services.EnviarNotaAntecipada":
                        servicesToRun = new ServiceBase[] { new EnviarNotaAntecipada() };
                        break;
                    case "BNE.Services.VerificaPagamentoBoleto":
                        servicesToRun = new ServiceBase[] { new VerificaPagamentoBoleto() };
                        break;
                    case "BNE.Services.EnviaEmailEtapasCandidatura":
                        servicesToRun = new ServiceBase[] { new EnviaEmailEtapasCandidatura() };
                        break;
                    case "BNE.Services.EmpresaBuscaCVPerfil":
                        servicesToRun = new ServiceBase[] { new EmpresaBuscaCVPerfil() };
                        break;
                }
            }

            if (servicesToRun == null)
            {
                log.LogEvent("Nenhum serviço detectado", System.Diagnostics.EventLogEntryType.Information, Event.AjusteExecucao);
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}