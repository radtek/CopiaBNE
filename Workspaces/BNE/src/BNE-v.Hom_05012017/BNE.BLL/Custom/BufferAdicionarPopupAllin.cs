using BNE.EL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace BNE.BLL.Custom
{
    public class BufferAdicionarPopupAllin{

     private static IList<int> Buffer = new  List<int>();
        private static Thread _objThread;
        private static bool _atualizaBuffer = Convert.ToBoolean(Convert.ToInt16(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarCVnoAllin)));
        private static readonly int TamanhoBuffer = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamenteTamanhoBuffer));
        private static readonly int Timeout = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarCVnoAllinaCada));
        private static string _allinLogin =  Parametro.RecuperaValorParametro(Enumeradores.Parametro.AllinWebServiceLogin);
        private static string _allinSenha = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AllinWebServiceSenha);
        private static string _allinPopupNomeLista = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AllinPopupNomeLista);
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
                    DataTable dtCurriculo = PreCadastro.CarregarCadastroAllin(item);

                    foreach (DataRow objCurriculo in dtCurriculo.Rows)
                    {
                        if (BLL.Custom.Validacao.ValidarEmail(objCurriculo[0].ToString()))
                        {
                            var campos = new StringBuilder();
                            var valores = new StringBuilder();
                            
                            campos.Append("nm_email;");
                            valores.AppendFormat("{0};", objCurriculo[0]);

                            campos.Append("nome_completo;");
                            valores.AppendFormat("{0};", objCurriculo[1]);

                            campos.Append("Funcao;");
                            valores.AppendFormat("{0};", objCurriculo[2]);

                            campos.Append("cidade;");
                            valores.AppendFormat("{0};", objCurriculo[3]);

                            campos.Append("uf");
                            valores.AppendFormat("{0}", objCurriculo[4]);
                           //if (ticket == "" || ticket == null || ticket == "Ticket nao confere!")
                            
                             ticket = FazerLoginAllin();

                            if (ticket != "")
                            {
                                InserirEmailAllin(ticket, campos.ToString(), valores.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msgErro;
                    GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("não montou os valores do BneLead para Allin {0}", item));
                }
            }
        }
        #endregion

        #region InserirEmailAllin
        /// <summary>
        /// Inserir/Atualizar email no Allin
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        public static bool InserirEmailAllin(string ticket, string campos, string valores)
        {
            try
            {
                string retorno = "";
                Object[] dados = new Object[3];

                dados[0] = _allinPopupNomeLista;
                dados[1] = campos;
                dados[2] = valores.Replace("'","");

                wsAllinInserirEmail.wsInserirEmailBaseService ws = new wsAllinInserirEmail.wsInserirEmailBaseService();
                retorno = ws.inserirEmailBase(ticket, dados);
                 

                if (retorno == "Email inserido na base!")
                    return true;
                else
                {
                    string msgErro;
                    GerenciadorException.GravarExcecao(new Exception(retorno), out msgErro, string.Format("não Inseriu Email no BneLead Allin {0}", valores));
                    return false;
                }
            }
            catch (Exception ex)
            {
                string msgErro;
                GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("Erro ao tentar inserir email na lista BneLead  {0}", valores));
                return false;
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
            string ticket = "";

            try
            {
                wsAllinLogin.wsLoginService ws = new wsAllinLogin.wsLoginService();
                ticket = ws.getTicket(_allinLogin, _allinSenha);
            }
            catch (Exception ex)
            {
                string msgErro;
                GerenciadorException.GravarExcecao(ex,out msgErro, "Erro ao efetuar login no Allin");
            }

            return ticket;
        }
        #endregion

        #region Add
        public static void Add(int idPreCadastro)
        {
            try
            {
                lock (Buffer)
                {
                    Inicializar();

                    if (!Buffer.Contains(idPreCadastro))
                        Buffer.Add(idPreCadastro);
                }
            }
            catch (Exception ex)
            {
                string msgErro;
                GerenciadorException.GravarExcecao(ex, out msgErro, "Adicionar email na lista BneLead na base do Allin");
            }
        }
        #endregion
        
    }
}
