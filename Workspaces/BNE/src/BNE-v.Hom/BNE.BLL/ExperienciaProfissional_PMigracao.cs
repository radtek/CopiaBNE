using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class ExperienciaProfissional
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
        /// Método utilizado para inserir uma instância de Experiencial Profissional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Fabiano Charan</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idExperienciaProfissional = Convert.ToInt32(cmd.Parameters["@Idf_Experiencia_Profissional"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Experiencial Profissional no banco de dados, dentro de uma transação.
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

            parms[0].Value = this._idExperienciaProfissional;

            if (this._areaBNE != null)
                parms[1].Value = this._areaBNE.IdAreaBNE;
            else
                parms[1].Value = DBNull.Value;

            parms[2].Value = this._pessoaFisica.IdPessoaFisica;
            parms[3].Value = this._razaoSocial;

            if (!String.IsNullOrEmpty(this._descricaoFuncaoExercida))
                parms[4].Value = this._descricaoFuncaoExercida;
            else
                parms[4].Value = DBNull.Value;


            if (this._funcao != null)
                parms[5].Value = this._funcao.IdFuncao;
            else
                parms[5].Value = DBNull.Value;

            parms[6].Value = this._dataAdmissao;

            if (this._dataDemissao.HasValue)
                parms[7].Value = this._dataDemissao;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoAtividade))
                parms[8].Value = this._descricaoAtividade;
            else
                parms[8].Value = DBNull.Value;

            parms[9].Value = this._flagInativo;

            if (this._flagImportado.HasValue)
                parms[11].Value = this._flagImportado;
            else
                parms[11].Value = DBNull.Value;

            if (this._vlrSalario.HasValue)
                parms[12].Value = this._vlrSalario;
            else
                parms[12].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(this._descricaoNavegador))
                parms[13].Value = this._descricaoNavegador;
            else
                parms[13].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[10].Value = this._dataCadastro;
        }
        #endregion

        #endregion
    }
}
