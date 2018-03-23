using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;

namespace BNE.Services
{
    internal partial class EnviarEmailConfirmacaoCandidatura : BaseService
    {
        #region Construtores
        public EnviarEmailConfirmacaoCandidatura()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.EnviarEmailConfirmacaoCandidaturaHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnviarEmailConfirmacaoCandidaturaDelay;
        #endregion

        #region Eventos
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvio);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
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

                    try
                    {
                        EventLogWriter.LogEvent(
                            string.Format(
                                "Iniciou agora o envio da carta para Candidato com as candidaturas do dia {0}.",
                                DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);

                        //Pegar candidaturas do dia
                        var dtCandidaturasDia = VagaCandidato.CandidaturasDoDia();

                        if (dtCandidaturasDia != null || dtCandidaturasDia.Rows.Count > 0)
                        {
                            EnviarEmailConfirmacao(dtCandidaturasDia);
                        }
                    }
                    catch (Exception exEnvio)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(exEnvio, out message);
                        message = string.Format("{0} - {1}", id, message);
                        EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }

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

        public static bool EnviarEmailConfirmacao(DataTable dtCandidaturasDia)
        {
            var emailsEnviados = true;
            var templateEmail = "";

            //pegar apenas os candidatos
            var dtTodos = dtCandidaturasDia.Copy();
            var dtCandidatos = RemoveDuplicateRows(dtCandidaturasDia, "Idf_Curriculo");


            foreach (DataRow row in dtCandidatos.Rows)
            {
                templateEmail = "";

                //carregar o CV para o enfileiramento da mensagem de e-mail.
                PessoaFisica objPessoaFisica;
                Curriculo objCurriculo;
                objPessoaFisica = PessoaFisica.LoadObject(Convert.ToInt32(row[10]));

                UsuarioFilialPerfil objPerfilDestinatario = null;
                UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objPerfilDestinatario);

                Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

                //pegar carta para o Email
                var objcarta =
                    CartaEmail.LoadObject(Convert.ToInt32(BLL.Enumeradores.CartaEmail.EnviarEmailConfirmacaoCandidatura));
                var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
                var carta = "";

                string listaVagas,
                    salaVip,
                    vip,
                    cadastroCurriculo,
                    quemMeViu,
                    pesquisaVagas,
                    loginCandidato,
                    cadastroExperiencias;
                Curriculo.RetornarHashLogarCurriculo(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, out listaVagas,
                    out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato,
                    out cadastroExperiencias);

                var results = from myRow in dtTodos.AsEnumerable()
                    where myRow.Field<int>("Idf_Curriculo") == int.Parse(row[0].ToString())
                    select myRow;

                //montar a carta com as candidaturas do candidato
                foreach (var dr in results)
                {
                    templateEmail += MontarCorpoCarta();

                    #region Protocolo
                    var objVaga = Vaga.LoadObject(Convert.ToInt32(dr["Idf_Vaga"]));
                    //Funcao estagio
                    var funcao = VagaCurso.ListaCursoParaCartaEmail(objVaga.DescricaoFuncao, objVaga.IdVaga);

                    VagaCandidato objVagaCandidato;
                    if (VagaCandidato.CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo,
                        out objVagaCandidato))
                    {
                        var protocolo = string.Format("{0}{1}{2}", objCurriculo.IdCurriculo, objVaga.CodigoVaga,
                            objVagaCandidato.DataCadastro.ToString("yyyyMMdd"));
                        templateEmail = templateEmail.Replace("{Protocolo}", protocolo);
                    }
                    #endregion

                    templateEmail = templateEmail.Replace("{funcao}", funcao);
                    templateEmail = templateEmail.Replace("{salario}",
                        TratarSalarioVaga(dr["Vlr_Salario_De"].ToString(), dr["Vlr_Salario_Para"].ToString()));
                    templateEmail = templateEmail.Replace("{cidade}", dr["Nme_Cidade"].ToString());
                    templateEmail = templateEmail.Replace("{atribuicoes}", dr["Des_Atribuicoes"].ToString());
                }

                carta = objcarta.ValorCartaEmail.Replace("{vagas_candidatadas}", templateEmail);

                carta = carta.Replace("{nome}", objPessoaFisica.PrimeiroNome);

                carta = carta.Replace("{login_candidato}", loginCandidato);
                carta = carta.Replace("{quem_me_viu}", quemMeViu);
                carta = carta.Replace("{lista_vagas}", listaVagas);


                var objMensagem = new MensagemSistema();

                objMensagem.DescricaoMensagemSistema = carta;

                if (Validacao.ValidarEmail(objPessoaFisica.EmailPessoa))
                {
                    //Enviar E-mail para o candidato
                    MensagemCS.SalvarEmail(objCurriculo, null, objPerfilDestinatario, null, objcarta.DescricaoAssunto,
                        objMensagem.DescricaoMensagemSistema, BLL.Enumeradores.CartaEmail.EnviarEmailConfirmacaoCandidatura, emailRemetente, objPessoaFisica.EmailPessoa, null, null,
                        null);
                }
            }

            return emailsEnviados;
        }

        #region MontarCorpoCarta
        public static string MontarCorpoCarta()
        {
            var sb = new StringBuilder();

            sb.Append(CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.EnviarEmailConfirmacaoCandidaturaVAGA));

            return sb.ToString();
        }
        #endregion

        #region TratarSalario
        public static string TratarSalarioVaga(string de, string ate)
        {
            if (de == "" && ate == "")
                return "Salário: a combinar";

            if (de != null && ate != null)
            {
                return string.Format("Salário: {0} a {1}", Convert.ToDouble(de).ToString("C").Replace("$", "R$"),
                    Convert.ToDouble(ate).ToString("C").Replace("$", "R$"));
            }

            if (de != null)
            {
                return string.Format("Salário: {0}", Convert.ToDouble(de).ToString("C").Replace("$", "R$"));
            }
            if (ate != null)
            {
                return string.Format("Salário: {0}", Convert.ToDouble(ate).ToString("C").Replace("$", "R$"));
            }


            return "";
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
                Thread.Sleep((int) tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #region RemoveDuplicateRows
        public static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            var hTable = new Hashtable();
            var duplicateList = new ArrayList();

            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            return dTable;
        }
        #endregion

        #endregion
    }
}