//-- Data: 21/07/2015 12:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MotivoCampanhaFinalizada // Tabela: BNE_Motivo_Campanha_Finalizada
	{
		#region Atributos
		private int _idMotivoCampanhaFinalizada;
		private string _descricaoMotivoCampanhaFinalizada;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMotivoCampanhaFinalizada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdMotivoCampanhaFinalizada
		{
			get
			{
				return this._idMotivoCampanhaFinalizada;
			}
			set
			{
				this._idMotivoCampanhaFinalizada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMotivoCampanhaFinalizada
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMotivoCampanhaFinalizada
		{
			get
			{
				return this._descricaoMotivoCampanhaFinalizada;
			}
			set
			{
				this._descricaoMotivoCampanhaFinalizada = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public MotivoCampanhaFinalizada()
		{
		}
		public MotivoCampanhaFinalizada(int idMotivoCampanhaFinalizada)
		{
			this._idMotivoCampanhaFinalizada = idMotivoCampanhaFinalizada;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Motivo_Campanha_Finalizada (Idf_Motivo_Campanha_Finalizada, Des_Motivo_Campanha_Finalizada) VALUES (@Idf_Motivo_Campanha_Finalizada, @Des_Motivo_Campanha_Finalizada);";
		private const string SPUPDATE = "UPDATE BNE_Motivo_Campanha_Finalizada SET Des_Motivo_Campanha_Finalizada = @Des_Motivo_Campanha_Finalizada WHERE Idf_Motivo_Campanha_Finalizada = @Idf_Motivo_Campanha_Finalizada";
		private const string SPDELETE = "DELETE FROM BNE_Motivo_Campanha_Finalizada WHERE Idf_Motivo_Campanha_Finalizada = @Idf_Motivo_Campanha_Finalizada";
		private const string SPSELECTID = "SELECT * FROM BNE_Motivo_Campanha_Finalizada WITH(NOLOCK) WHERE Idf_Motivo_Campanha_Finalizada = @Idf_Motivo_Campanha_Finalizada";
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
			parms.Add(new SqlParameter("@Idf_Motivo_Campanha_Finalizada", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Motivo_Campanha_Finalizada", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idMotivoCampanhaFinalizada;
			parms[1].Value = this._descricaoMotivoCampanhaFinalizada;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de MotivoCampanhaFinalizada no banco de dados.
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
		/// Método utilizado para inserir uma instância de MotivoCampanhaFinalizada no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de MotivoCampanhaFinalizada no banco de dados.
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
		/// Método utilizado para atualizar uma instância de MotivoCampanhaFinalizada no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de MotivoCampanhaFinalizada no banco de dados.
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
		/// Método utilizado para salvar uma instância de MotivoCampanhaFinalizada no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de MotivoCampanhaFinalizada no banco de dados.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMotivoCampanhaFinalizada)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Campanha_Finalizada", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCampanhaFinalizada;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MotivoCampanhaFinalizada no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMotivoCampanhaFinalizada, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Campanha_Finalizada", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCampanhaFinalizada;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MotivoCampanhaFinalizada no banco de dados.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idMotivoCampanhaFinalizada)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Motivo_Campanha_Finalizada where Idf_Motivo_Campanha_Finalizada in (";

			for (int i = 0; i < idMotivoCampanhaFinalizada.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMotivoCampanhaFinalizada[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMotivoCampanhaFinalizada)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Campanha_Finalizada", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCampanhaFinalizada;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMotivoCampanhaFinalizada, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Campanha_Finalizada", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCampanhaFinalizada;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Mot.Idf_Motivo_Campanha_Finalizada, Mot.Des_Motivo_Campanha_Finalizada FROM BNE_Motivo_Campanha_Finalizada Mot";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoCampanhaFinalizada a partir do banco de dados.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Chave do registro.</param>
		/// <returns>Instância de MotivoCampanhaFinalizada.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MotivoCampanhaFinalizada LoadObject(int idMotivoCampanhaFinalizada)
		{
			using (IDataReader dr = LoadDataReader(idMotivoCampanhaFinalizada))
			{
				MotivoCampanhaFinalizada objMotivoCampanhaFinalizada = new MotivoCampanhaFinalizada();
				if (SetInstance(dr, objMotivoCampanhaFinalizada))
					return objMotivoCampanhaFinalizada;
			}
			throw (new RecordNotFoundException(typeof(MotivoCampanhaFinalizada)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoCampanhaFinalizada a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoCampanhaFinalizada">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MotivoCampanhaFinalizada.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MotivoCampanhaFinalizada LoadObject(int idMotivoCampanhaFinalizada, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMotivoCampanhaFinalizada, trans))
			{
				MotivoCampanhaFinalizada objMotivoCampanhaFinalizada = new MotivoCampanhaFinalizada();
				if (SetInstance(dr, objMotivoCampanhaFinalizada))
					return objMotivoCampanhaFinalizada;
			}
			throw (new RecordNotFoundException(typeof(MotivoCampanhaFinalizada)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoCampanhaFinalizada a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoCampanhaFinalizada))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoCampanhaFinalizada a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoCampanhaFinalizada, trans))
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
		/// <param name="objMotivoCampanhaFinalizada">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, MotivoCampanhaFinalizada objMotivoCampanhaFinalizada, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objMotivoCampanhaFinalizada._idMotivoCampanhaFinalizada = Convert.ToInt32(dr["Idf_Motivo_Campanha_Finalizada"]);
					objMotivoCampanhaFinalizada._descricaoMotivoCampanhaFinalizada = Convert.ToString(dr["Des_Motivo_Campanha_Finalizada"]);

					objMotivoCampanhaFinalizada._persisted = true;
					objMotivoCampanhaFinalizada._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}