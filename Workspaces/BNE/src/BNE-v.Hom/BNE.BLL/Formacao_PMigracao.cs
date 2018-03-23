using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class Formacao
    {
        #region Propriedades

        #region DataCadastro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataCadastro
        {
            get
            {
                return this._dataCadastro;
            }
            set
            {
                this._dataCadastro = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAlteracao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime? DataAlteracao
        {
            get
            {
                return this._dataAlteracao;
            }
            set
            {
                this._dataAlteracao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Mapeamento Novo -> Velho
        public void SalvarMigracao(SqlTransaction trans)
        {
            if (!this._persisted)
                this.InsertMigracao(trans);
            else
                this.UpdateMigracao(trans);
        }

        #region InsertMigracao
        /// <summary>
        /// Método utilizado para inserir uma instância de Formacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Fabiano Charan</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idFormacao = Convert.ToInt32(cmd.Parameters["@Idf_Formacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Formacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Fabiano Charan</remarks>
        private void UpdateMigracao(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParametersMigracao(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }
        #endregion

        #region SetParametersMigracao
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {
            parms[0].Value = this._idFormacao;
            parms[1].Value = this._pessoaFisica.IdPessoaFisica;
            parms[2].Value = this._escolaridade.IdEscolaridade;

            if (this._curso != null)
                parms[3].Value = this._curso.IdCurso;
            else
                parms[3].Value = DBNull.Value;


            if (this._fonte != null)
                parms[4].Value = this._fonte.IdFonte;
            else
                parms[4].Value = DBNull.Value;


            if (this._anoConclusao.HasValue)
                parms[5].Value = this._anoConclusao;
            else
                parms[5].Value = DBNull.Value;


            if (this._quantidadeCargaHoraria.HasValue)
                parms[6].Value = this._quantidadeCargaHoraria;
            else
                parms[6].Value = DBNull.Value;


            if (this._numeroPeriodo.HasValue)
                parms[7].Value = this._numeroPeriodo;
            else
                parms[7].Value = DBNull.Value;


            if (this._situacaoFormacao != null)
                parms[8].Value = this._situacaoFormacao.IdSituacaoFormacao;
            else
                parms[8].Value = DBNull.Value;

            parms[11].Value = this._flagInativo;
            parms[12].Value = this._flagNacional;

            if (!String.IsNullOrEmpty(this._descricaoEndereco))
                parms[13].Value = this._descricaoEndereco;
            else
                parms[13].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoCurso))
                parms[14].Value = this._descricaoCurso;
            else
                parms[14].Value = DBNull.Value;


            if (this._cidade != null)
                parms[15].Value = this._cidade.IdCidade;
            else
                parms[15].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoFonte))
                parms[16].Value = this._descricaoFonte;
            else
                parms[16].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }

            parms[9].Value = this._dataAlteracao;
            parms[10].Value = this._dataCadastro;
        }
        #endregion

        #endregion
    }
}
