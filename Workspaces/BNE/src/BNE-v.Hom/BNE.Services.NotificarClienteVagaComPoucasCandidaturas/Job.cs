using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Text;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using log4net;
using Quartz;

namespace BNE.Services.NotificarClienteVagaComPoucasCandidaturas
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Job()
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;
        }

        private int Dias => Convert.ToInt16(ConfigurationManager.AppSettings["Dias"]);
        private int QuantidadeMinimaCurriculos => Convert.ToInt16(ConfigurationManager.AppSettings["QuantidadeMinimaCurriculos"]);

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug("Job started now " + DateTime.Now);

            try
            {
                var vagas = Vaga.RecuperarVagasParaComPoucasCandidaturas(Dias, QuantidadeMinimaCurriculos);
                var sbvagas = new StringBuilder();

                foreach (var vaga in vagas)
                {
                    _logger.Debug($"Enviando carta para a vaga {vaga.IdVaga}");
                    sbvagas.Append($@"<tr><td><p style='font-size:.8em; color:#757575;line-height:150%;'>A empresa {vaga.RazaoSocial} divulgou a vaga {vaga.CodigoVaga} - {vaga.Funcao} mas obteve apenas {vaga.QuantidadeInscritos} candidaturas de no mínimo {QuantidadeMinimaCurriculos}</p></td></tr>");

                    if (vaga.QuantidadeBancoCurriculo > QuantidadeMinimaCurriculos)
                    {
                        var cartaEmail = CartaEmail.GetById((int) BLL.Enumeradores.CartaEmail.NotificarClienteVagaComPoucasCandidaturas).Montar(new
                        {
                            nome = vaga.Nome,
                            funcao = vaga.Funcao,
                            quantidadeinscritos = vaga.QuantidadeInscritos,
                            quantidadebancocurriculos = vaga.QuantidadeBancoCurriculo,
                            vercurriculos = vaga.LinkVerCurriculos
                        });

                        EmailSenderFactory
                            .Create(TipoEnviadorEmail.Fila)
                            .Enviar(cartaEmail.Assunto, cartaEmail.Conteudo, BLL.Enumeradores.CartaEmail.NotificarClienteVagaComPoucasCandidaturas, Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens), vaga.Email);
                    }
                }

                var cartaEmailInterno = CartaEmail.GetById((int) BLL.Enumeradores.CartaEmail.NotificarInternamenteVagaComPoucasCandidaturas).Montar(new
                {
                    vagas = sbvagas.ToString()
                });

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(
                        cartaEmailInterno.Assunto,
                        cartaEmailInterno.Conteudo,
                        BLL.Enumeradores.CartaEmail.NotificarClienteVagaComPoucasCandidaturas,
                        Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens),
                        Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailDestinatarioVagaPoucasCandidaturas));
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao executar", ex);
            }
            _logger.Debug("Job ended...");
        }
    }
}