//-- Data: 25/01/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RastreadorIdioma // Tabela: BNE_Rastreador_Idioma
	{
		#region Atributos
		private int _idRastreadorIdioma;
		private Rastreador _rastreador;
		private Idioma _idioma;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreadorIdioma
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreadorIdioma
		{
			get
			{
				return this._idRastreadorIdioma;
			}
		}
		#endregion 

		#region Rastreador
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Rastreador Rastreador
		{
			get
			{
				return this._rastreador;
			}
			set
			{
				this._rastreador = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Idioma
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Idioma Idioma
		{
			get
			{
				return this._idioma;
			}
			set
			{
				this._idioma = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RastreadorIdioma()
		{
		}
		public RastreadorIdioma(int idRastreadorIdioma)
		{
			this._idRastreadorIdioma = idRastreadorIdioma;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador_Idioma (Idf_Rastreador, Idf_Idioma) VALUES (@Idf_Rastreador, @Idf_Idioma);SET @Idf_Rastreador_Idioma = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador_Idioma SET Idf_Rastreador = @Idf_Rastreador, Idf_Idioma = @Idf_Idioma WHERE Idf_Rastreador_Idioma = @Idf_Rastreador_Idioma";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador_Idioma WHERE Idf_Rastreador_Idioma = @Idf_Rastreador_Idioma";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Idioma WHERE Idf_Rastreador_Idioma = @Idf_Rastreador_Idioma";
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
			parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Idioma", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRastreadorIdioma;
			parms[1].Value = this._rastreador.IdRastreador;
			parms[2].Value = this._idioma.IdIdioma;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RastreadorIdioma no banco de dados.
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
						this._idRastreadorIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Idioma"].Value);
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
		/// Método utilizado para inserir uma instância de RastreadorIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreadorIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Idioma"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RastreadorIdioma no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RastreadorIdioma no banco de dados, dentro de uma transação.
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

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de RastreadorIdioma no banco de dados.
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
		/// Método utilizado para salvar uma instância de RastreadorIdioma no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RastreadorIdioma no banco de dados.
		/// </summary>
		/// <param name="idRastreadorIdioma">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorIdioma;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RastreadorIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorIdioma, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorIdioma;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RastreadorIdioma no banco de dados.
		/// </summary>
		/// <param name="idRastreadorIdioma">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreadorIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador_Idioma where Idf_Rastreador_Idioma in (";

			for (int i = 0; i < idRastreadorIdioma.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreadorIdioma[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreadorIdioma">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorIdioma;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorIdioma, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorIdioma;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Idioma, Ras.Idf_Rastreador, Ras.Idf_Idioma FROM BNE_Rastreador_Idioma Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorIdioma a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreadorIdioma">Chave do registro.</param>
		/// <returns>Instância de RastreadorIdioma.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorIdioma LoadObject(int idRastreadorIdioma)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorIdioma))
			{
				RastreadorIdioma objRastreadorIdioma = new RastreadorIdioma();
				if (SetInstance(dr, objRastreadorIdioma))
					return objRastreadorIdioma;
			}
			throw (new RecordNotFoundException(typeof(RastreadorIdioma)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorIdioma a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RastreadorIdioma.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorIdioma LoadObject(int idRastreadorIdioma, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorIdioma, trans))
			{
				RastreadorIdioma objRastreadorIdioma = new RastreadorIdioma();
				if (SetInstance(dr, objRastreadorIdioma))
					return objRastreadorIdioma;
			}
			throw (new RecordNotFoundException(typeof(RastreadorIdioma)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorIdioma a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorIdioma))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorIdioma a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorIdioma, trans))
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
		/// <param name="objRastreadorIdioma">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RastreadorIdioma objRastreadorIdioma)
		{
			try
			{
				if (dr.Read())
				{
					objRastreadorIdioma._idRastreadorIdioma = Convert.ToInt32(dr["Idf_Rastreador_Idioma"]);
					objRastreadorIdioma._rastreador = new Rastreador(Convert.ToInt32(dr["Idf_Rastreador"]));
					objRastreadorIdioma._idioma = new Idioma(Convert.ToInt32(dr["Idf_Idioma"]));

					objRastreadorIdioma._persisted = true;
					objRastreadorIdioma._modified = false;

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