//-- Data: 19/03/2013 11:02
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ParametroFilial // Tabela: TAB_Parametro_Filial
	{
		#region Atributos
		private int _idParametro;
		private int _idFilial;
		private DateTime _dataCadastro;
		private string _valorParametro;
		private bool _flagInativo;
        private int _flagEstag;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdParametro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdParametro
		{
			get
			{
				return this._idParametro;
			}
			set
			{
				this._idParametro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdFilial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdFilial
		{
			get
			{
				return this._idFilial;
			}
			set
			{
				this._idFilial = value;
				this._modified = true;
			}
		}
		#endregion 

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
		}
		#endregion 

		#region ValorParametro
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorParametro
		{
			get
			{
				return this._valorParametro;
			}
			set
			{
				this._valorParametro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
				this._modified = true;
			}
		}
		#endregion 


		#endregion

		#region Construtores
		public ParametroFilial()
		{
		}
		public ParametroFilial(int idParametro, int idFilial)
		{
			this._idParametro = idParametro;
			this._idFilial = idFilial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Parametro_Filial (Idf_Parametro, Idf_Filial, Dta_Cadastro, Vlr_Parametro, Flg_Inativo) VALUES (@Idf_Parametro, @Idf_Filial, @Dta_Cadastro, @Vlr_Parametro, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE TAB_Parametro_Filial SET Dta_Cadastro = @Dta_Cadastro, Vlr_Parametro = @Vlr_Parametro, Flg_Inativo = @Flg_Inativo WHERE Idf_Parametro = @Idf_Parametro AND Idf_Filial = @Idf_Filial";
		private const string SPDELETE = "DELETE FROM TAB_Parametro_Filial WHERE Idf_Parametro = @Idf_Parametro AND Idf_Filial = @Idf_Filial";
		private const string SPSELECTID = "SELECT * FROM TAB_Parametro_Filial WITH(NOLOCK) WHERE Idf_Parametro = @Idf_Parametro AND Idf_Filial = @Idf_Filial";
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
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Vlr_Parametro", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idParametro;
			parms[1].Value = this._idFilial;
			parms[3].Value = this._valorParametro;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de ParametroFilial no banco de dados.
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
		/// Método utilizado para inserir uma instância de ParametroFilial no banco de dados, dentro de uma transação.
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

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de ParametroFilial no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Update()
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de ParametroFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Update(SqlTransaction trans)
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		#endregion

        #region exist
        public static bool IsParamentro(int idParametro, int idFilial)
        {
            string EXISTPARAMETER = "SELECT COUNT(*) AS QUANT FROM TAB_Parametro_Filial WITH(NOLOCK) WHERE Idf_Parametro = @Idf_Parametro AND Idf_Filial = @Idf_Filial";

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idParametro;
            parms[1].Value = idFilial;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, EXISTPARAMETER, parms))
            {
                try
                {
                    if (dr.Read())
                    {
                        if (Convert.ToInt32(dr["QUANT"]) > 0)
                            return true;
                        else
                            return false;
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
        }
        #endregion
        #region Save
        /// <summary>
		/// Método utilizado para salvar uma instância de ParametroFilial no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de ParametroFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save(SqlTransaction trans)
		{
			if (!this._persisted)
				this.Insert(trans);
			else
				this.Update(trans);
		}
		#endregion

		#region Delete
		/// <summary>
		/// Método utilizado para excluir uma instância de ParametroFilial no banco de dados.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idFilial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idParametro, int idFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idFilial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ParametroFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idParametro, int idFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idFilial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idFilial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idParametro, int idFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idFilial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idParametro, int idFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idFilial;

			return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar uma consulta paginada do banco de dados.
		/// </summary>
		/// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
		/// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
		/// <param name="paginaCorrente">Número da página que será exibida.</param>
		/// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
		/// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
			int fim = paginaCorrente * tamanhoPagina;

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Par.Idf_Parametro, Par.Idf_Filial, Par.Dta_Cadastro, Par.Vlr_Parametro, Par.Flg_Inativo FROM TAB_Parametro_Filial Par";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ParametroFilial a partir do banco de dados.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idFilial">Chave do registro.</param>
		/// <returns>Instância de ParametroFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ParametroFilial LoadObject(int idParametro, int idFilial)
		{
			using (IDataReader dr = LoadDataReader(idParametro, idFilial))
			{
				ParametroFilial objParametroFilial = new ParametroFilial();
				if (SetInstance(dr, objParametroFilial))
					return objParametroFilial;
			}
			throw (new RecordNotFoundException(typeof(ParametroFilial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ParametroFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ParametroFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ParametroFilial LoadObject(int idParametro, int idFilial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idParametro, idFilial, trans))
			{
				ParametroFilial objParametroFilial = new ParametroFilial();
				if (SetInstance(dr, objParametroFilial))
					return objParametroFilial;
			}
			throw (new RecordNotFoundException(typeof(ParametroFilial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ParametroFilial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idParametro, this._idFilial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ParametroFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idParametro, this._idFilial, trans))
			{
				return SetInstance(dr, this);
			}
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objParametroFilial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, ParametroFilial objParametroFilial)
		{
			try
			{
				if (dr.Read())
				{
					objParametroFilial._idParametro = Convert.ToInt32(dr["Idf_Parametro"]);
					objParametroFilial._idFilial = Convert.ToInt32(dr["Idf_Filial"]);
					objParametroFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objParametroFilial._valorParametro = Convert.ToString(dr["Vlr_Parametro"]);
					objParametroFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objParametroFilial._persisted = true;
					objParametroFilial._modified = false;

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