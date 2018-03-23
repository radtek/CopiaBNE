using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class VagaCandidato
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
            this._idVagaCandidato = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Candidato"].Value);
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

            parms[0].Value = this._idVagaCandidato;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[2].Value = this._vaga.IdVaga;
            parms[4].Value = this._flagInativo;
            parms[5].Value = this._statusCurriculoVaga.IdStatusCurriculoVaga;

            if (this._flagAutoCandidatura.HasValue)
                parms[6].Value = this._flagAutoCandidatura;
            else
                parms[6].Value = DBNull.Value;

            if (this._statusCandidatura != null)
                parms[7].Value = this._statusCandidatura.IdStatusCandidatura;
            else
                parms[7].Value = (int)Enumeradores.StatusCandidatura.Candidatado;

            if (this._origemCandidatura != null)
                parms[8].Value = this._origemCandidatura.IdOrigemCandidatura;
            else
                parms[8].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[3].Value = this._dataCadastro;
            parms[9].Value = DateTime.Now;
        }
        #endregion

        #endregion
    }
}
