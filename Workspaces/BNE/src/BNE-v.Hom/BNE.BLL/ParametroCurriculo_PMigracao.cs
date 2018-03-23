using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class ParametroCurriculo
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
        /// Campo opcional.
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
        /// Método utilizado para inserir uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Fabiano Charan</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._valorParametro = cmd.Parameters["@Vlr_Parametro"].Value.ToString();
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Filial no banco de dados, dentro de uma transação.
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
        /// <remarks>Fabiano Charan</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {

            parms[0].Value = this._parametro.IdParametro;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[3].Value = this._valorParametro;
            parms[4].Value = this._flagInativo;
            parms[2].Value = this._dataCadastro;
            parms[5].Value = DateTime.Now.Date;

        }
        #endregion

        #endregion
    }
}
