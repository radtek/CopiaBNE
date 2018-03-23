using System;

namespace BNE.BLL.Notificacao
{
    public partial class ProcessamentoJornalVagas // Tabela: alerta.LOG_Processamento_Jornal_Vagas
    {
        #region Atributos
        private int _idProcessamentoJornalVagas;
        private string _codigoVagas;
        private int _idfCurriculo;
        private bool _flgInvisivel;
        private DateTime _dataCadastro;
        private DateTime? _dataInicioProcessamento;
        private DateTime? _dataFimProcessamento;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

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

        #region IdfCurriculo
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        public int IdfCurriculo
        {
            get { return _idfCurriculo; }
            set
            {
                _idfCurriculo = value;
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