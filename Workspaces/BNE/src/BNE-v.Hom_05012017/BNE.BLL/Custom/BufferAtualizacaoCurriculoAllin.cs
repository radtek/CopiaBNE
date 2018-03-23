using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace BNE.BLL.Custom
{
    public class BufferAtualizacaoCurriculoAllin
    {
        private static IList<int> Buffer = new List<int>();
        private static Thread _objThread;
        private static bool _atualizaBuffer = Convert.ToBoolean(Convert.ToInt16(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarCVnoAllin)));
        private static readonly int TamanhoBuffer = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamenteTamanhoBuffer));
        private static readonly int Timeout = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarCVnoAllinaCada));
        private static string _allinLogin = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AllinWebServiceLogin);
        private static string _allinSenha = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AllinWebServiceSenha);
        private static string _allinCandidatoNomeLista = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AllinCandidatoNomeLista);
        public static string ticket = "";

        private static void Inicializar()
        {
            if (_objThread == null)
            {
                _objThread = new Thread(TimeOutLog);
                _objThread.Start();
            }
        }

        private static void TimeOutLog()
        {
            while (_atualizaBuffer)
            {
                Thread.Sleep(Timeout);

                GravaBuffer();
            }
        }

        #region GravaBuffer
        private static void GravaBuffer()
        {
            lock (Buffer)
            {
                try
                {
                    DispararThread(Buffer);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region DispararThread
        private static void DispararThread(IList<int> Buffer)
        {
            if (Buffer.Count > 0)
            {
                new Thread(() =>
                {
                    var threadObject = Buffer.ToList<int>();
                    Thread.CurrentThread.IsBackground = true;

                    Buffer.Clear();
                    AtualizarAllin(threadObject);
                }).Start();
            }
        }
        #endregion

        #region AtualizarAllin
        private static void AtualizarAllin(IList<int> objBuffer)
        {
            foreach (var item in objBuffer)
            {
                try
                {
                    DataTable dtCurriculo = Curriculo.CarregarCurriculoparaWebServiceAllin(item);

                    foreach (DataRow objCurriculo in dtCurriculo.Rows)
                    {
                        if (BLL.Custom.Validacao.ValidarEmail(objCurriculo[0].ToString()))
                        {
                            var campos = new StringBuilder();
                            var valores = new StringBuilder();

                            campos.Append("nm_email;");
                            valores.AppendFormat("{0};", objCurriculo[0]);

                            campos.Append("id_cadastro;");
                            valores.AppendFormat("{0};", objCurriculo[1]);

                            campos.Append("Tipo_Curriculo;");
                            valores.AppendFormat("{0};", objCurriculo[2]);

                            campos.Append("cpf;");
                            valores.AppendFormat("{0};", objCurriculo[3]);

                            campos.Append("nome;");
                            valores.AppendFormat("{0};", objCurriculo[4]);

                            if (objCurriculo[4].ToString().IndexOf(" ", StringComparison.Ordinal) > -1)
                            {
                                campos.Append("Observacao_Curriculo;");
                                valores.AppendFormat("{0};", objCurriculo[4].ToString().Substring(0, objCurriculo[4].ToString().IndexOf(" ", StringComparison.Ordinal)));
                            }

                            campos.Append("dt_nascimento;");
                            valores.AppendFormat("{0};", Convert.ToDateTime(objCurriculo[5]).ToString("dd/MM/yyyy"));

                            campos.Append("dt_cadastro;");
                            valores.AppendFormat("{0};", Convert.ToDateTime(objCurriculo[6]).ToString("dd/MM/yyyy"));

                            campos.Append("dt_atualizacao;");
                            valores.AppendFormat("{0};", Convert.ToDateTime(objCurriculo[7]).ToString("dd/MM/yyyy"));

                            if (objCurriculo[8].ToString() != "")
                            {
                                campos.Append("Dt_Modificacao;");
                                valores.AppendFormat("{0};", Convert.ToDateTime(objCurriculo[8]).ToString("dd/MM/yyyy"));
                            }

                            campos.Append("Flag_Vip;");
                            valores.AppendFormat("{0};", ((bool)objCurriculo[9] ? "1" : "0"));

                            if (objCurriculo[10].ToString() != "")
                            {
                                campos.Append("Dt_Fim_Vip;");
                                valores.AppendFormat("{0};", Convert.ToDateTime(objCurriculo[10]).ToString("dd/MM/yyyy"));
                            }
                            campos.Append("cidade;");
                            valores.AppendFormat("{0};", objCurriculo[11]);

                            campos.Append("uf;");
                            valores.AppendFormat("{0};", objCurriculo[12]);

                            campos.Append("sexo;");
                            valores.AppendFormat("{0};", objCurriculo[13]);

                            if (objCurriculo[14].ToString() != "")
                            {
                                campos.Append("Experiencia_Dta_Demissao;");
                                valores.AppendFormat("{0};", Convert.ToDateTime(objCurriculo[14]).ToString("dd/MM/yyyy"));
                            }

                            campos.Append("Qtd_Qm_Me_Viu");
                            valores.AppendFormat("{0}", objCurriculo[15]);

                            ticket = FazerLoginAllin();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msgErro;
                    GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("não montou os valores do cv para Allin {0}", item));
                }
            }
        }
        #endregion

        #region FazerLoginAllin
        /// <summary>
        /// Efetuar Login no Allin para pegar o token
        /// </summary>
        /// <returns></returns>
        public static string FazerLoginAllin()
        {
            string t = "";

            try
            {
                wsAllinLogin.wsLoginService ws = new wsAllinLogin.wsLoginService();
                t = ws.getTicket(_allinLogin, _allinSenha);
            }
            catch (Exception ex)
            {
                string msgErro;
                GerenciadorException.GravarExcecao(ex, out msgErro, "Erro ao efetuar login no Allin");
            }

            return t;
        }
        #endregion

        #region Add
        public static void Add(int idCurriculo)
        {
            try
            {
                lock (Buffer)
                {
                    Inicializar();

                    if (!Buffer.Contains(idCurriculo))
                        Buffer.Add(idCurriculo);
                }
            }
            catch (Exception ex)
            {
                string msgErro;
                GerenciadorException.GravarExcecao(ex, out msgErro, "Atualizar CV na base do Allin");
            }
        }
        #endregion

    }
}
