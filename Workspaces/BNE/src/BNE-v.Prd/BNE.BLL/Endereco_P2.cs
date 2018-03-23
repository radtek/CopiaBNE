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
                INNER JOIN plataforma.TAB_Cidade cid WITH(NOLOCK) ON E.Idf_Cidade = cid.Idf_Cidade
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";

        private const string Spselectporfilial = @"
        SELECT  * 
        FROM    TAB_Endereco E WITH(NOLOCK) 
                INNER JOIN TAB_Filial F WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
        WHERE   Idf_Filial = @Idf_Filial";

        #endregion

        #region Métodos

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region CarregarPorPessoaFisica
        /// <summary>
        /// Método responsável por carregar uma instancia de endereço através da pessoa física.
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa Física</param>
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
                var objCidade = new Cidade();
                if (dr.Read() && 
                    SetInstance_NotDispose(dr, objEndereco) && 
                    Cidade.SetInstance_NotDispose(dr, objCidade))
                {
                    objEndereco.Cidade = objCidade;

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
        /// Método responsável por carregar uma instancia de endereço através da filial.
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

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEndereco">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NotDispose(IDataReader dr, Endereco objEndereco)
        {
            try
            {
                objEndereco._idEndereco = Convert.ToInt32(dr["Idf_Endereco"]);
                if (dr["Num_CEP"] != DBNull.Value)
                    objEndereco._numeroCEP = Convert.ToString(dr["Num_CEP"]);
                if (dr["Des_Logradouro"] != DBNull.Value)
                    objEndereco._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
                if (dr["Num_Endereco"] != DBNull.Value)
                    objEndereco._numeroEndereco = Convert.ToString(dr["Num_Endereco"]);
                if (dr["Des_Complemento"] != DBNull.Value)
                    objEndereco._descricaoComplemento = Convert.ToString(dr["Des_Complemento"]);
                if (dr["Des_Bairro"] != DBNull.Value)
                    objEndereco._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
                objEndereco._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                objEndereco._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objEndereco._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                if (dr["Dta_Alteracao"] != DBNull.Value)
                    objEndereco._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

                objEndereco._persisted = true;
                objEndereco._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
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
        /// Método utilizado para atualizar uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
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
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
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

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idEndereco = value;
            }
            get { return this._idEndereco; }
        }
        #endregion
    }
}