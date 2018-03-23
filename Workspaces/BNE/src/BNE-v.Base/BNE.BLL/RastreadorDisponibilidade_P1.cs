//-- Data: 25/01/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RastreadorDisponibilidade // Tabela: BNE_Rastreador_Disponibilidade
	{
		#region Atributos
		private int _idRastreadorDisponibilidade;
		private Rastreador _rastreador;
		private Disponibilidade _disponibilidade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreadorDisponibilidade
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreadorDisponibilidade
		{
			get
			{
				return this._idRastreadorDisponibilidade;
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

		#region Disponibilidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Disponibilidade Disponibilidade
		{
			get
			{
				return this._disponibilidade;
			}
			set
			{
				this._disponibilidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RastreadorDisponibilidade()
		{
		}
		public RastreadorDisponibilidade(int idRastreadorDisponibilidade)
		{
			this._idRastreadorDisponibilidade = idRastreadorDisponibilidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador_Disponibilidade (Idf_Rastreador, Idf_Disponibilidade) VALUES (@Idf_Rastreador, @Idf_Disponibilidade);SET @Idf_Rastreador_Disponibilidade = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador_Disponibilidade SET Idf_Rastreador = @Idf_Rastreador, Idf_Disponibilidade = @Idf_Disponibilidade WHERE Idf_Rastreador_Disponibilidade = @Idf_Rastreador_Disponibilidade";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador_Disponibilidade WHERE Idf_Rastreador_Disponibilidade = @Idf_Rastreador_Disponibilidade";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Disponibilidade WHERE Idf_Rastreador_Disponibilidade = @Idf_Rastreador_Disponibilidade";
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
			parms.Add(new SqlParameter("@Idf_Rastreador_Disponibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Disponibilidade", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRastreadorDisponibilidade;
			parms[1].Value = this._rastreador.IdRastreador;
			parms[2].Value = this._disponibilidade.IdDisponibilidade;

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
		/// Método utilizado para inserir uma instância de RastreadorDisponibilidade no banco de dados.
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
						this._idRastreadorDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Disponibilidade"].Value);
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
		/// Método utilizado para inserir uma instância de RastreadorDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreadorDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Disponibilidade"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RastreadorDisponibilidade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RastreadorDisponibilidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RastreadorDisponibilidade no banco de dados.
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
		/// Método utilizado para salvar uma instância de RastreadorDisponibilidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RastreadorDisponibilidade no banco de dados.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorDisponibilidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RastreadorDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorDisponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorDisponibilidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RastreadorDisponibilidade no banco de dados.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreadorDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador_Disponibilidade where Idf_Rastreador_Disponibilidade in (";

			for (int i = 0; i < idRastreadorDisponibilidade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreadorDisponibilidade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorDisponibilidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorDisponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorDisponibilidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Disponibilidade, Ras.Idf_Rastreador, Ras.Idf_Disponibilidade FROM BNE_Rastreador_Disponibilidade Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorDisponibilidade a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Chave do registro.</param>
		/// <returns>Instância de RastreadorDisponibilidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorDisponibilidade LoadObject(int idRastreadorDisponibilidade)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorDisponibilidade))
			{
				RastreadorDisponibilidade objRastreadorDisponibilidade = new RastreadorDisponibilidade();
				if (SetInstance(dr, objRastreadorDisponibilidade))
					return objRastreadorDisponibilidade;
			}
			throw (new RecordNotFoundException(typeof(RastreadorDisponibilidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorDisponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RastreadorDisponibilidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorDisponibilidade LoadObject(int idRastreadorDisponibilidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorDisponibilidade, trans))
			{
				RastreadorDisponibilidade objRastreadorDisponibilidade = new RastreadorDisponibilidade();
				if (SetInstance(dr, objRastreadorDisponibilidade))
					return objRastreadorDisponibilidade;
			}
			throw (new RecordNotFoundException(typeof(RastreadorDisponibilidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorDisponibilidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorDisponibilidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorDisponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorDisponibilidade, trans))
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
		/// <param name="objRastreadorDisponibilidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RastreadorDisponibilidade objRastreadorDisponibilidade)
		{
			try
			{
				if (dr.Read())
				{
					objRastreadorDisponibilidade._idRastreadorDisponibilidade = Convert.ToInt32(dr["Idf_Rastreador_Disponibilidade"]);
					objRastreadorDisponibilidade._rastreador = new Rastreador(Convert.ToInt32(dr["Idf_Rastreador"]));
					objRastreadorDisponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));

					objRastreadorDisponibilidade._persisted = true;
					objRastreadorDisponibilidade._modified = false;

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