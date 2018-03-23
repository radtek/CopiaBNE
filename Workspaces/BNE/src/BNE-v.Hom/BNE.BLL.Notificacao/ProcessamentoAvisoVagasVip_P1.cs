using System;
using System.Data;

namespace BNE.BLL.Notificacao
{
    public partial class ProcessamentoAvisoVagasVip // Tabela: alerta.Log_Processamento_VIP
    {
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
        public ProcessamentoAvisoVagasVip()
        {
        }

        public ProcessamentoAvisoVagasVip(int idProcessamentoJornalVagas)
        {
            _idProcessamentoJornalVagas = idProcessamentoJornalVagas;
            _persisted = true;
        }
        #endregion
    }
}