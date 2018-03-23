//-- Data: 18/04/2016 14:25
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class StatusCandidatura // Tabela: BNE_Status_Candidatura
	{
		#region Atributos
		private Int16 _idStatusCandidatura;
		private string _descricaoStatusCandidatura;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdStatusCandidatura
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public Int16 IdStatusCandidatura
		{
			get
			{
				return this._idStatusCandidatura;
			}
		}
		#endregion 

		#region DescricaoStatusCandidatura
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoStatusCandidatura
		{
			get
			{
				return this._descricaoStatusCandidatura;
			}
			set
			{
				this._descricaoStatusCandidatura = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public StatusCandidatura()
		{
		}
		public StatusCandidatura(Int16 idStatusCandidatura)
		{
			this._idStatusCandidatura = idStatusCandidatura;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Status_Candidatura (Des_Status_Candidatura) VALUES (@Des_Status_Candidatura);SET @Idf_Status_Candidatura = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Status_Candidatura SET Des_Status_Candidatura = @Des_Status_Candidatura WHERE Idf_Status_Candidatura = @Idf_Status_Candidatura";
		private const string SPDELETE = "DELETE FROM BNE_Status_Candidatura WHERE Idf_Status_Candidatura = @Idf_Status_Candidatura";
		private const string SPSELECTID = "SELECT * FROM BNE_Status_Candidatura WITH(NOLOCK) WHERE Idf_Status_Candidatura = @Idf_Status_Candidatura";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Candidatura", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Status_Candidatura", SqlDbType.VarChar, 50));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idStatusCandidatura;
			parms[1].Value = this._descricaoStatusCandidatura;

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
		/// Método utilizado para inserir uma instância de StatusCandidatura no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idStatusCandidatura = Convert.ToInt16(cmd.Parameters["@Idf_Status_Candidatura"].Value);
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
		/// Método utilizado para inserir uma instância de StatusCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idStatusCandidatura = Convert.ToInt16(cmd.Parameters["@Idf_Status_Candidatura"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de StatusCandidatura no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de StatusCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de StatusCandidatura no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de StatusCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de StatusCandidatura no banco de dados.
		/// </summary>
		/// <param name="idStatusCandidatura">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(Int16 idStatusCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Candidatura", SqlDbType.Int, 2));

			parms[0].Value = idStatusCandidatura;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de StatusCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(Int16 idStatusCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Candidatura", SqlDbType.Int, 2));

			parms[0].Value = idStatusCandidatura;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de StatusCandidatura no banco de dados.
		/// </summary>
		/// <param name="idStatusCandidatura">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<Int16> idStatusCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Status_Candidatura where Idf_Status_Candidatura in (";

			for (int i = 0; i < idStatusCandidatura.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 2));
				parms[i].Value = idStatusCandidatura[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idStatusCandidatura">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(Int16 idStatusCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Candidatura", SqlDbType.Int, 2));

			parms[0].Value = idStatusCandidatura;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(Int16 idStatusCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Candidatura", SqlDbType.Int, 2));

			parms[0].Value = idStatusCandidatura;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sta.Idf_Status_Candidatura, Sta.Des_Status_Candidatura FROM BNE_Status_Candidatura Sta";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusCandidatura a partir do banco de dados.
		/// </summary>
		/// <param name="idStatusCandidatura">Chave do registro.</param>
		/// <returns>Instância de StatusCandidatura.</returns>
		/// <remarks>Mailson</remarks>
		public static StatusCandidatura LoadObject(Int16 idStatusCandidatura)
		{
			using (IDataReader dr = LoadDataReader(idStatusCandidatura))
			{
				StatusCandidatura objStatusCandidatura = new StatusCandidatura();
				if (SetInstance(dr, objStatusCandidatura))
					return objStatusCandidatura;
			}
			throw (new RecordNotFoundException(typeof(StatusCandidatura)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de StatusCandidatura.</returns>
		/// <remarks>Mailson</remarks>
		public static StatusCandidatura LoadObject(Int16 idStatusCandidatura, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idStatusCandidatura, trans))
			{
				StatusCandidatura objStatusCandidatura = new StatusCandidatura();
				if (SetInstance(dr, objStatusCandidatura))
					return objStatusCandidatura;
			}
			throw (new RecordNotFoundException(typeof(StatusCandidatura)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de StatusCandidatura a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idStatusCandidatura))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de StatusCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idStatusCandidatura, trans))
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
		/// <param name="objStatusCandidatura">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, StatusCandidatura objStatusCandidatura, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objStatusCandidatura._idStatusCandidatura = Convert.ToInt16(dr["Idf_Status_Candidatura"]);
					objStatusCandidatura._descricaoStatusCandidatura = Convert.ToString(dr["Des_Status_Candidatura"]);

					objStatusCandidatura._persisted = true;
					objStatusCandidatura._modified = false;

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