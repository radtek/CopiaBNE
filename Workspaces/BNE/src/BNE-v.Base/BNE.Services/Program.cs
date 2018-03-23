using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace BNE.Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
                { 
                    new AtualizarCurriculo(),
                    new AtualizarPlano(),
                    new ArquivarVaga(),
                    new EnvioSMSSemanal(),
                    new AtualizarEmpresa(),
                    new EnviarCurriculo(),
                    new EnvioEmailMailing(),
                    new AtualizaSitemap(),
                    new InativarCurriculo(),
                    //new BuscaCoordenada(),
                    new IntegrarVagas(),
                    new IntegracaoWebfopag(),
                    new DestravaSMSPlanoEmployer(),
                    new OperadoraCelular(),
                    new ControleParcelas(),
                    new EmailsInvalidos(),
                    new AllInEmailSincronizacaoLista(),
                    new EnviarEmailAlertaExperienciaProfissional(),
                    new EnviarEmailAlertaMensalFilial(),
                    new EmailFiliais(),
                    new EnvioSMSEmpresas(),
                    new AllinEmailQuemMeViu(),
                    new ControleFinanceiro(),                    
                    new EnvioSMSEmailEmpresasCvsNaoVistos(),
                    new SondaBancoDoBrasil()
                };

            ServiceBase.Run(servicesToRun);
            
            /*var service = new AllInEmailSincronizacaoLista();

            Type service1Type = service.GetType();

            MethodInfo onStart = service1Type.GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
            onStart.Invoke(service, new object[] { null });

            MethodInfo onStop = service1Type.GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);
            onStop.Invoke(service, null);

            //new AllInEmailSincronizacaoLista().DoEmpresa(true);*/            
        }
    }
}
