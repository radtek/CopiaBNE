using System;
using System.Data;

namespace BNE.BLL.Notificacao
{
    public partial class ProcessamentoJornalVagas // Tabela: alerta.LOG_Processamento_Jornal_Vagas
    {
        #region Métodos

        #region SetInstance
        /// <summary>
        ///     Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as
        ///     colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objProcessamentoJornalVagas">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, ProcessamentoJornalVagas objProcessamentoJornalVagas, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objProcessamentoJornalVagas._idProcessamentoJornalVagas = Convert.ToInt32(dr["Idf_Processamento_Jornal_Vagas"]);
                    objProcessamentoJornalVagas._codigoVagas = Convert.ToString(dr["Cod_Vagas"]);
                    objProcessamentoJornalVagas._codigoCurriculos = Convert.ToString(dr["Cod_Curriculos"]);
                    objProcessamentoJornalVagas._flgInvisivel = Convert.ToBoolean(dr["Flg_Invisivel"]);
                    objProcessamentoJornalVagas._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Dta_Inicio_Processamento"] != DBNull.Value)
                        objProcessamentoJornalVagas._dataInicioProcessamento = Convert.ToDateTime(dr["Dta_Inicio_Processamento"]);
                    if (dr["Dta_Fim_Processamento"] != DBNull.Value)
                        objProcessamentoJornalVagas._dataFimProcessamento = Convert.ToDateTime(dr["Dta_Fim_Processamento"]);

                    objProcessamentoJornalVagas._persisted = true;
                    objProcessamentoJornalVagas._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dispose)
                    dr.Dispose();
            }
        }
        #endregion

        #endregion

        #region Atributos
        private int _idProcessamentoJornalVagas;
        private string _codigoVagas;
        private string _codigoCurriculos;
        private bool _flgInvisivel;
        private DateTime _dataCadastro;
        private DateTime? _dataInicioProcessamento;
        private DateTime? _dataFimProcessamento;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdProcessamentoJornalVagas
        /// <summary>
        ///     Campo obrigatório.
        ///     Campo auto-numerado.
        /// </summary>
        public int IdProcessamentoJornalVagas
        {
            get { return _idProcessamentoJornalVagas; }
        }
        #endregion

        #region CodigoVagas
        /// <summary>
        ///     Tamanho do campo: -1.
        ///     Campo obrigatório.
        /// </summary>
        public string CodigoVagas
        {
            get { return _codigoVagas; }
            set
            {
                _codigoVagas = value;
                _modified = true;
            }
        }
        #endregion

        #region CodigoCurriculos
        /// <summary>
        ///     Tamanho do campo: -1.
        ///     Campo obrigatório.
        /// </summary>
        public string CodigoCurriculos
        {
            get { return _codigoCurriculos; }
            set
            {
                _codigoCurriculos = value;
                _modified = true;
            }
        }
        #endregion

        #region         public bool FlagInvisivel
        /// <summary>
        ///     Boolean
        ///     Campo obrigatório.
        /// </summary>
        public bool FlagInvisivel
        {
            get { return _flgInvisivel; }
            set
            {
                _flgInvisivel = value;
                _modified = true;
            }
        }
        #endregion

        #region DataCadastro
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        public DateTime DataCadastro
        {
            get { return _dataCadastro; }
        }
        #endregion

        #region DataInicioProcessamento
        /// <summary>
        ///     Campo opcional.
        /// </summary>
        public DateTime? DataInicioProcessamento
        {
            get { return _dataInicioProcessamento; }
            set
            {
                _dataInicioProcessamento = value;
                _modified = true;
            }
        }
        #endregion

        #region DataFimProcessamento
        /// <summary>
        ///     Campo opcional.
        /// </summary>
        public DateTime? DataFimProcessamento
        {
            get { return _dataFimProcessamento; }
            set
            {
                _dataFimProcessamento = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public ProcessamentoJornalVagas()
        {
        }

        public ProcessamentoJornalVagas(int idProcessamentoJornalVagas)
        {
            _idProcessamentoJornalVagas = idProcessamentoJornalVagas;
            _persisted = true;
        }
        #endregion
    }
}