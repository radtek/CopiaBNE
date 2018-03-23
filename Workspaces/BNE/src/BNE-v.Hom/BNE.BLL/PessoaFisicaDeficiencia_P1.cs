using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PessoaFisicaDeficiencia
    {
  	    #region Atributos
        private int _idfPessoaFisicaDeficiencia;
		private PessoaFisica _pessoaFisica;
		private Deficiencia _deficiencia;
        private DeficienciaDetalhe _deficienciaDetalhe;
        private DateTime _dtaCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

        #region IdfPessoaFisicaDeficiencia
        public int IdfPessoaFisicaDeficiencia
        {
            get
            {
                return this._idfPessoaFisicaDeficiencia;
            }
          
        }
        #endregion

		#region PessoaFisica
		/// <summary>
		/// 
		/// Campo obrigatório.
		/// </summary>
		public PessoaFisica PessoaFisica
		{
			get
			{
				return this._pessoaFisica;
			}
			set
			{
				this._pessoaFisica = value;
				this._modified = true;
			}
		}
		#endregion 

        #region Deficiencia
        /// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Deficiencia Deficiencia
		{
			get
			{
				return this._deficiencia;
			}
			set
			{
				this._deficiencia = value;
				this._modified = true;
			}
		}
		#endregion 
		
        #region DeficienciaDetalhe
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DeficienciaDetalhe DeficienciaDetalhe
        {
            get
            {
                return this._deficienciaDetalhe;
            }
            set
            {
                this._deficienciaDetalhe = value;
                this._modified = true;
            }
        }
        #endregion 

        #region DtaCadastro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DtaCadastro
        {
            get
            {
                return this._dtaCadastro;
            }
            set
            {
                this._dtaCadastro = value;
                this._modified = true;
            }
        }
        #endregion 
        
		#endregion

		#region Construtores
		public PessoaFisicaDeficiencia()
		{
		}
        //public PessoaFisicaDeficiencia(int idDeficiencia)
        //{
        //    this._idDeficiencia = idDeficiencia;
        //    this._persisted = true;
        //}
		#endregion

		#region Consultas
        private const string SPINSERT = "insert into bne.bne_pessoa_fisica_deficiencia (idf_pessoa_fisica, idf_deficiencia, idf_Deficiencia_Detalhe) values(@idf_Pessoa_Fisica, @idf_Deficiencia, @idf_Deficiencia_Detalhe);";
        private const string SPDELETE = "DELETE FROM BNE.BNE_Pessoa_Fisica_Deficiencia WHERE Idf_Pessoa_Fisica_Deficiencia = @Idf_Pessoa_Fisica_Deficiencia";
        private const string SPSELECTID = "Select * from bne_Pessoa_Fisica_Deficiencia WHERE Idf_Pessoa_Fisica_Deficiencia = @Idf_Pessoa_Fisica_Deficiencia";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_Pessoa_Fisica_Deficiencia", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@idf_Deficiencia_Detalhe", SqlDbType.Int, 4));

			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
            parms[0].Value = this._idfPessoaFisicaDeficiencia;
			parms[1].Value = this._pessoaFisica.IdPessoaFisica;
			parms[2].Value = this._deficiencia.IdDeficiencia;
            parms[3].Value = this._deficienciaDetalhe.IdfDeficienciaDetalhe;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Deficiencia no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert()
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);

			using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
						cmd.Parameters.Clear();
						this._persisted = true;
						this._modified = false;
						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}
		/// <summary>
		/// Método utilizado para inserir uma instância de Deficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de Deficiencia no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Deficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save(SqlTransaction trans)
		{
			if (!this._persisted)
				this.Insert(trans);
			
		}
		#endregion

		#region Delete
		/// <summary>
		/// Método utilizado para excluir uma instância de Deficiencia no banco de dados.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPessoaFisicaDeficiente)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@idf_Pessoa_Fisica_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisicaDeficiente;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Deficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPessoaFisicaDeficiente, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_Pessoa_Fisica_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisicaDeficiente;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Deficiencia no banco de dados.
		/// </summary>
		/// <param name="idDeficiencia">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idDeficienciaDetalhe, int idPessoaFisica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Pessoa_Fisica_Deficiencia where idf_pessoa_fisica = " + idPessoaFisica + " and Idf_Deficiencia_Detalhe in (";

            for (int i = 0; i < idDeficienciaDetalhe.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idDeficienciaDetalhe[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPessoaFisicaDeficiencia">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        private static IDataReader LoadDataReader(int idPessoaFisicaDeficiencia)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisicaDeficiencia;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPessoaFisicaDeficiencia">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPessoaFisicaDeficiencia, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisicaDeficiencia;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        }
      
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de AcaoPublicacao a partir do banco de dados.
        /// </summary>
        /// <param name="idAcaoPublicacao">Chave do registro.</param>
        /// <returns>Instância de AcaoPublicacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisicaDeficiencia LoadObject(int idPessoaFisicaDeficiencia)
        {
            using (IDataReader dr = LoadDataReader(idPessoaFisicaDeficiencia))
            {
                PessoaFisicaDeficiencia objPessoaFisicaDeficiencia = new PessoaFisicaDeficiencia();
                if (SetInstance(dr, objPessoaFisicaDeficiencia))
                    return objPessoaFisicaDeficiencia;
            }
            throw (new RecordNotFoundException(typeof(PessoaFisicaDeficiencia)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de AcaoPublicacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAcaoPublicacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de AcaoPublicacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisicaDeficiencia LoadObject(int idAcaoPublicacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idAcaoPublicacao, trans))
            {
                PessoaFisicaDeficiencia objPessoaFisicaDeficiencia = new PessoaFisicaDeficiencia();
                if (SetInstance(dr, objPessoaFisicaDeficiencia))
                    return objPessoaFisicaDeficiencia;
            }
            throw (new RecordNotFoundException(typeof(PessoaFisicaDeficiencia)));
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPessoaFisicaDeficiencia">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PessoaFisicaDeficiencia objPessoaFisicaDeficiencia)
        {
            try
            {
                if (dr.Read())
                {
                    objPessoaFisicaDeficiencia._idfPessoaFisicaDeficiencia = Convert.ToInt32(dr["Idf_Pessoa_Fisica_Deficiencia"]);
                    objPessoaFisicaDeficiencia.Deficiencia = new Deficiencia(Convert.ToInt32(dr["idf_Deficiencia"]));
                    objPessoaFisicaDeficiencia.DeficienciaDetalhe = new DeficienciaDetalhe(Convert.ToInt32(dr["Idf_Deficiencia_Detalhe"]));
                    objPessoaFisicaDeficiencia._dtaCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objPessoaFisicaDeficiencia._persisted = true;
                    objPessoaFisicaDeficiencia._modified = false;

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
                dr.Dispose();
            }
        }
        #endregion

		#endregion
	}
}