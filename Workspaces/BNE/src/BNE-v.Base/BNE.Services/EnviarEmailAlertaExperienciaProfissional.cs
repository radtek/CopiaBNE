using BNE.BLL;
using BNE.Services.Code;
using BNE.Services.Properties;
using System;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using BNE.BLL.Custom;
using System.Collections.Generic;
using System.Collections;

namespace BNE.Services
{
    partial class EnviarEmailAlertaExperienciaProfissional : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.EnviarEmailAlertaExperienciaProfissionalHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnviarEmailAlertaExperienciaProfissionalDelayMinutos;
        private const string EventSourceName = "EnviarEmailAlertaExperienciaProfissional";
        private static Estatistica _estatisticas = null;

        #endregion

        #region Construtores
        public EnviarEmailAlertaExperienciaProfissional()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvioEmailAlerta);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #region Metodos

        #region IniciarEnvioEmailAlerta
        public void IniciarEnvioEmailAlerta()
        {
            try
            {
                AjustarThread(DateTime.Now, true);
      
                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
                    try
                    {
                        EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                        try
                        {
                            //ajustar data da consulta
                            string DtaInicial = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                            string DtaFim = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                            string DtaInicial20Dias = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd");
                            string DtaFim20Dias = DateTime.Now.AddDays(-19).ToString("yyyy-MM-dd");

                            //parametros de estatisticas
                            _estatisticas  = Estatistica.RecuperarEstatistica();

                            DataTable dtCurriculosSemExperiencia = ExperienciaProfissional.ListarCurriculosSemExperienciaProfissional(2,DtaInicial,DtaFim);
                            DataTable dtCurriculosSemExperiencia20Dias = ExperienciaProfissional.ListarCurriculosSemExperienciaProfissional(20,DtaInicial20Dias,DtaFim20Dias);

                            DataTable dtCurriculosComExperienciaFraca = ExperienciaProfissional.ListarCurriculosComExperienciaProfissionalFraca(2, DtaInicial,DtaFim);
                            DataTable dtCurriculosComExperienciaFraca20Dias = ExperienciaProfissional.ListarCurriculosComExperienciaProfissionalFraca(20,DtaInicial20Dias,DtaFim20Dias);

                            //enviar e-mail e SMS de alerta
                            //Enviar alerta para curriculos cadastrados ha 2 dias e sem experiência Profissional
                            if (dtCurriculosSemExperiencia.Rows.Count > 0)
                                EnviarAlertaCurriculoSemExperiencia(dtCurriculosSemExperiencia);

                            //Enviar alerta para curriculos cadastrados ha 2 dias e com experiência profissional fraca ou regular.
                            if (dtCurriculosComExperienciaFraca.Rows.Count > 0)
                                EnviarAlertCurriculosComExperienciaProfissionalFraca(dtCurriculosComExperienciaFraca);

                            //APÓS 20 DAIS CHECAR E ENVIAR NOVAMENTE
                            //Enviar alerta para curriculos cadastrados ha 20 dias e sem experiência Profissional
                            if (dtCurriculosSemExperiencia20Dias.Rows.Count > 0)
                                EnviarAlertaCurriculoSemExperiencia(dtCurriculosSemExperiencia20Dias);

                            //Enviar alerta para curriculos cadastrados ha 20 dias e com experiência profissional fraca ou regular.
                            if (dtCurriculosComExperienciaFraca20Dias.Rows.Count > 0)
                                EnviarAlertCurriculosComExperienciaProfissionalFraca(dtCurriculosComExperienciaFraca20Dias);

                        }
                        catch (Exception exEnvio)
                        {
                            string message;
                            var id = EL.GerenciadorException.GravarExcecao(exEnvio, out message);
                            message = string.Format("{0} - {1}", id, message);
                            EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                        message = string.Format("{0} - {1}", id, message);
                           
                        EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                    }
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);
                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #region EnviarAlertaCurriculoSemExperiencia
        /// <summary>
        /// Método responsável por enviar SMS para os curriculos sem Experiencia Profissional
        /// </summary>
        /// <param name="numeroDias">indica o número de dias que o curriculo foi cadastrado</param>
        /// <returns>Envia alerta de sms para o candidato</returns>
        public static bool EnviarAlertaCurriculoSemExperiencia(DataTable dtCurriculosSemExperiencia)
        {
            bool emailsEnviados = true;
            string templateSMS = "";
            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaSMS = new List<BLL.DTO.PessoaFisicaEnvioSMSTanque>();
            string idUFPRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EnvioAlertaSMSExperienciaProfissional);

            try
            {
                foreach (DataRow dr in dtCurriculosSemExperiencia.Rows)
                {
                    PessoaFisica objPessoaFisica;
                    Curriculo objCurriculo;
                    objPessoaFisica = PessoaFisica.LoadObject(Convert.ToInt32(dr[0]));

                    UsuarioFilialPerfil objPerfilDestinatario = null;
                    UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objPerfilDestinatario);

                    Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

                    //Enviar Email
                    //pegar carta para o Email
                    CartaEmail objcarta = CartaEmail.LoadObject(Convert.ToInt32(BNE.BLL.Enumeradores.CartaEmail.AlertaEmailExperienciaProfissional));
                    string emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
                    var carta = CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.AlertaEmailExperienciaProfissional);

                    string listaVagas, salaVip, vip, cadastroCurriculo, quemMeViu, pesquisaVagas, loginCandidato, cadastroExperiencias;
                    BNE.BLL.Curriculo.RetornarHashLogarCurriculo(objCurriculo.IdCurriculo, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);

                    carta = carta.Replace("{Lista_Vagas}", listaVagas);
                    carta = carta.Replace("{Sala_Vip}", salaVip);
                    carta = carta.Replace("{vip}", vip);
                    carta = carta.Replace("{Quem_Me_Viu}", quemMeViu);
                    carta = carta.Replace("{Cadastro_Curriculo}", cadastroCurriculo);
                    carta = carta.Replace("{Pesquisa_Vagas}", pesquisaVagas);
                    carta = carta.Replace("{login_candidato}", salaVip);

                    carta = carta.Replace("{Quantidade_empresas}", _estatisticas.QuantidadeEmpresa.ToString());
                    carta = carta.Replace("{Quantidade_vagas}", _estatisticas.QuantidadeVaga.ToString());

                    carta = carta.Replace("{nomeCandidato}", objPessoaFisica.NomeCompleto);
                    carta = carta.Replace("{login_candidato}", vip);
                    
                    carta = carta.Replace("{cadastro_experiencias}", cadastroExperiencias);
                    carta = carta.Replace("{cadastro_curriculo}", cadastroCurriculo);

                    MensagemSistema objMensagem = new MensagemSistema();

                    objMensagem.DescricaoMensagemSistema = carta;

                    if (BLL.Custom.Validacao.ValidarEmail(objPessoaFisica.EmailPessoa))
                    {
                        //Enviar E-mail para o candidato
                        MensagemCS.SalvarEmail(objCurriculo, null, objPerfilDestinatario, null, objcarta.DescricaoAssunto, objMensagem.DescricaoMensagemSistema, emailRemetente, objPessoaFisica.EmailPessoa, null, null, null);
                    }

                    //EnviarSMS
                    if (!string.IsNullOrEmpty(dr[3].ToString()) && !string.IsNullOrEmpty(dr[4].ToString()))
                    {
                        //pegar carta para o SMS
                        templateSMS = CartaSMS.RecuperaValorConteudo(BNE.BLL.Enumeradores.CartaSMS.AlertaExperienciaProfissional2dias);

                        //Enviar SMS para o candidato
                        StringBuilder smsMensagem = new StringBuilder();
                        smsMensagem.AppendFormat("{0}, {1} ", dr[1], templateSMS);

                        //popular lista SMS
                        var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                        {
                            dddCelular = dr[3].ToString(),
                            numeroCelular = dr[4].ToString(),
                            nomePessoa = dr[1].ToString(),
                            mensagem = smsMensagem.ToString(),
                            idDestinatario = objCurriculo.IdCurriculo
                        };

                        listaSMS.Add(objUsuarioEnvioSMS);
                        //fim do popular lista SMS
                    }
                }

                //enviar SMS pelo tanque
                BNE.BLL.Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS, true);

            }
            catch (Exception ex)
            {
                string message;
                EL.GerenciadorException.GravarExcecao(ex, out message);
                emailsEnviados = false;
            }


            return emailsEnviados;
        }
        #endregion

        #region EnviarAlertCurriculosComExperienciaProfissionalFraca
        /// <summary>
        /// Método responsável por enviar alerta de e-mail e sms para o candidato
        /// </summary>
        /// <param name="dtCurriculosSemExperiencia"></param>
        /// <returns></returns>
        public static bool EnviarAlertCurriculosComExperienciaProfissionalFraca(DataTable dtCurriculosSemExperiencia)
        {
            //IList<PessoaFisica> listaPessoas = new List<PessoaFisica>();
            DataTable dtCunsultaOriginal = dtCurriculosSemExperiencia.Copy();
            DataTable dtTemp = RemoveDuplicateRows(dtCurriculosSemExperiencia, "Idf_Pessoa_Fisica");
            bool emailsEnviados = true;
            string templateSMS = "";
            string templateEmail = "";
            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaSMS = new List<BLL.DTO.PessoaFisicaEnvioSMSTanque>();
            string idUFPRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EnvioAlertaSMSExperienciaProfissional);

            try
            {
                foreach (DataRow linhaPessoa in dtTemp.Rows)
                {
                    templateEmail = "";

                    var results = from myRow in dtCunsultaOriginal.AsEnumerable()
                                  where myRow.Field<int>("Idf_Pessoa_Fisica") == int.Parse(linhaPessoa[0].ToString())
                                  select myRow;

                    //Inserir informações das empresas e atividades na carta
                    foreach (DataRow dr in results)
                    {
                        templateEmail += MontarCorpoCarta(dr);
                    }

                    //gerar alertas, email e SMS
                    PessoaFisica objPessoaFisica;
                    Curriculo objCurriculo;
                    objPessoaFisica = PessoaFisica.LoadObject(Convert.ToInt32(linhaPessoa[0]));

                    UsuarioFilialPerfil objPerfilDestinatario = null;
                    UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objPerfilDestinatario);

                    Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

                    //Enviar Email
                    //pegar carta para o Email
                    CartaEmail objcarta = CartaEmail.LoadObject(Convert.ToInt32(BNE.BLL.Enumeradores.CartaEmail.AlertaEmailExperienciaProfissionalFraca));
                    string emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
                    var carta = CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.AlertaEmailExperienciaProfissionalFraca);

                    string listaVagas, salaVip, vip, cadastroCurriculo, quemMeViu, pesquisaVagas, loginCandidato, cadastroExperiencias;
                    BNE.BLL.Curriculo.RetornarHashLogarCurriculo(objCurriculo.IdCurriculo, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);

                    carta = carta.Replace("{Lista_Vagas}", listaVagas);
                    carta = carta.Replace("{Sala_Vip}", salaVip);
                    carta = carta.Replace("{vip}", vip);
                    carta = carta.Replace("{Quem_Me_Viu}", quemMeViu);
                    carta = carta.Replace("{Cadastro_Curriculo}", cadastroCurriculo);
                    carta = carta.Replace("{Pesquisa_Vagas}", pesquisaVagas);
                    carta = carta.Replace("{login_candidato}", salaVip);

                    carta = carta.Replace("{Quantidade_empresas}", _estatisticas.QuantidadeEmpresa.ToString());
                    carta = carta.Replace("{Quantidade_vagas}", _estatisticas.QuantidadeVaga.ToString());

                    carta = carta.Replace("{nomeCandidato}", objPessoaFisica.NomeCompleto);
                    carta = carta.Replace("{login_candidato}", vip);

                    carta = carta.Replace("{cadastro_experiencias}", cadastroExperiencias);

                    carta = carta.Replace("{dadosEmpresas}", templateEmail);

                    MensagemSistema objMensagem = new MensagemSistema();

                    objMensagem.DescricaoMensagemSistema = carta;

                    if (BLL.Custom.Validacao.ValidarEmail(objPessoaFisica.EmailPessoa))
                    {
                        //Enviar E-mail para o candidato
                        MensagemCS.SalvarEmail(objCurriculo, null, objPerfilDestinatario, null, objcarta.DescricaoAssunto, objMensagem.DescricaoMensagemSistema, emailRemetente, objPessoaFisica.EmailPessoa, null, null, null);
                    }

                    //Enviar SMS para o candidato
                    if (!string.IsNullOrEmpty(linhaPessoa[3].ToString()) && !string.IsNullOrEmpty(linhaPessoa[4].ToString()))
                    {
                        templateSMS = CartaSMS.RecuperaValorConteudo(BNE.BLL.Enumeradores.CartaSMS.AlertaExperienciaProfissionalFraca2dias);

                        StringBuilder smsMensagem = new StringBuilder();
                        smsMensagem.AppendFormat("{0}, {1} ", linhaPessoa[2], templateSMS);

                        //popular lista SMS
                        var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                        {
                            dddCelular = linhaPessoa[3].ToString(),
                            numeroCelular = linhaPessoa[4].ToString(),
                            nomePessoa = linhaPessoa[2].ToString(),
                            mensagem = smsMensagem.ToString(),
                            idDestinatario = objCurriculo.IdCurriculo
                        };

                        listaSMS.Add(objUsuarioEnvioSMS);
                        //fim do popular lista SMS
                    }
                }

                //enviar SMS pelo tanque
                BNE.BLL.Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS, true);
            }
            catch (Exception ex)
            {
                string message;
                EL.GerenciadorException.GravarExcecao(ex, out message);
                emailsEnviados = false;
            }
            
            return emailsEnviados;
        }
        #endregion

        #region MontarCorpoCarta
        public static string MontarCorpoCarta(DataRow row)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<p style=\"margin-left: 1.27cm; margin-bottom: 0.35cm; line-height: 115%\">");
            sb.AppendFormat("<b>{0} atividades realizadas: {1}</b>", row[5].ToString(), row[6].ToString());
            sb.Append("</p>");    
           
            return sb.ToString();
        }
        #endregion

        #region RemoveDuplicateRows
        public static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

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