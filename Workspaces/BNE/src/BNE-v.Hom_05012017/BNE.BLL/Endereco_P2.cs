//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class Endereco : ICloneable // Tabela: TAB_Endereco
    {

        #region Propriedades

        #region DataCadastro
        /// <summary>
        /// Campo obrigat�rio.
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
        [Display(Name = "IgnoreData")]
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

        #region Consultas

        private const string Spselectporpessoafisica = @"
        SELECT  * 
        FROM    TAB_Endereco E WITH(NOLOCK) 
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";

        private const string Spselectporfilial = @"
        SELECT  * 
        FROM    TAB_Endereco E WITH(NOLOCK) 
                INNER JOIN TAB_Filial F WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
        WHERE   Idf_Filial = @Idf_Filial";

        #endregion

        #region M�todos

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region CarregarPorPessoaFisica
        /// <summary>
        /// M�todo respons�vel por carregar uma instancia de endere�o atrav�s da pessoa f�sica.
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa F�sica</param>
        /// <param name="objEndereco">Paramero out</param>
        /// <returns>objEndereco</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisica(PessoaFisica objPessoaFisica, out Endereco objEndereco)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporpessoafisica, parms))
            {
                objEndereco = new Endereco();
                if (SetInstance(dr, objEndereco))
                {
                    if (!dr.IsClosed)
                        dr.Close();

                    return true;
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            return false;
        }
        #endregion

        #region CarregarPorFilial
        /// <summary>
        /// M�todo respons�vel por carregar uma instancia de endere�o atrav�s da filial.
        /// </summary>
        /// <param name="objFilial">Filial</param>
        /// <param name="objEndereco">Paramero out</param>
        /// <returns>objEndereco</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorFilial(Filial objFilial, out Endereco objEndereco)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporfilial, parms))
            {
                objEndereco = new Endereco();
                if (SetInstance(dr, objEndereco))
                {
                    if (!dr.IsClosed)
                        dr.Close();

                    return true;
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            return false;
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
        /// M�todo utilizado para inserir uma inst�ncia de Filial no banco de dados, dentro de uma transa��o.
        /// </summary>
        /// <param name="trans">Transa��o existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idEndereco = Convert.ToInt32(cmd.Parameters["@Idf_Endereco"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// M�todo utilizado para atualizar uma inst�ncia de Filial no banco de dados, dentro de uma transa��o.
        /// </summary>
        /// <param name="trans">Transa��o existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
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
        /// M�todo auxiliar que recebe e preenche a lista de par�metros passada de acordo com os valores da inst�ncia.
        /// </summary>
        /// <param name="parms">Lista de par�metros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {
            parms[0].Value = this._idEndereco;

            if (!String.IsNullOrEmpty(this._numeroCEP))
                parms[1].Value = this._numeroCEP;
            else
                parms[1].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoLogradouro))
                parms[2].Value = this._descricaoLogradouro;
            else
                parms[2].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroEndereco))
                parms[3].Value = this._numeroEndereco;
            else
                parms[3].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoComplemento))
                parms[4].Value = this._descricaoComplemento;
            else
                parms[4].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoBairro))
                parms[5].Value = this._descricaoBairro;
            else
                parms[5].Value = DBNull.Value;

            parms[6].Value = this._cidade.IdCidade;
            parms[7].Value = this._flagInativo;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[8].Value = this._dataCadastro;
            parms[9].Value = this._dataAlteracao;
        }
        #endregion

        #endregion

    }
}