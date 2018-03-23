//-- Data: 08/06/2016 14:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class OrigemCandidatura // Tabela: BNE_Origem_Candidatura
	{
		#region Atributos
		private int _idOrigemCandidatura;
		private string _descricaoOrigem;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdOrigemCandidatura
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdOrigemCandidatura
		{
			get
			{
				return this._idOrigemCandidatura;
			}
		}
		#endregion 

		#region DescricaoOrigem
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoOrigem
		{
			get
			{
				return this._descricaoOrigem;
			}
			set
			{
				this._descricaoOrigem = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public OrigemCandidatura()
		{
		}
		public OrigemCandidatura(int idOrigemCandidatura)
		{
			this._idOrigemCandidatura = idOrigemCandidatura;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Origem_Candidatura (Des_Origem) VALUES (@Des_Origem);SET @Idf_Origem_Candidatura = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Origem_Candidatura SET Des_Origem = @Des_Origem WHERE Idf_Origem_Candidatura = @Idf_Origem_Candidatura";
		private const string SPDELETE = "DELETE FROM BNE_Origem_Candidatura WHERE Idf_Origem_Candidatura = @Idf_Origem_Candidatura";
		private const string SPSELECTID = "SELECT * FROM BNE_Origem_Candidatura WITH(NOLOCK) WHERE Idf_Origem_Candidatura = @Idf_Origem_Candidatura";
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
			parms.Add(new SqlParameter("@Idf_Origem_Candidatura", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Origem", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idOrigemCandidatura;

			if (!String.IsNullOrEmpty(this._descricaoOrigem))
				parms[1].Value = this._descricaoOrigem;
			else
				parms[1].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de OrigemCandidatura no banco de dados.
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
						this._idOrigemCandidatura = Convert.ToInt32(cmd.Parameters["@Idf_Origem_Candidatura"].Value);
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
		/// Método utilizado para inserir uma instância de OrigemCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idOrigemCandidatura = Convert.ToInt32(cmd.Parameters["@Idf_Origem_Candidatura"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de OrigemCandidatura no banco de dados.
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
		/// Método utilizado para atualizar uma instância de OrigemCandidatura no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de OrigemCandidatura no banco de dados.
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
		/// Método utilizado para salvar uma instância de OrigemCandidatura no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de OrigemCandidatura no banco de dados.
		/// </summary>
		/// <param name="idOrigemCandidatura">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCandidatura;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de OrigemCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCandidatura;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de OrigemCandidatura no banco de dados.
		/// </summary>
		/// <param name="idOrigemCandidatura">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idOrigemCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Origem_Candidatura where Idf_Origem_Candidatura in (";

			for (int i = 0; i < idOrigemCandidatura.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idOrigemCandidatura[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idOrigemCandidatura">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCandidatura;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCandidatura;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ori.Idf_Origem_Candidatura, Ori.Des_Origem FROM BNE_Origem_Candidatura Ori";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemCandidatura a partir do banco de dados.
		/// </summary>
		/// <param name="idOrigemCandidatura">Chave do registro.</param>
		/// <returns>Instância de OrigemCandidatura.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemCandidatura LoadObject(int idOrigemCandidatura)
		{
			using (IDataReader dr = LoadDataReader(idOrigemCandidatura))
			{
				OrigemCandidatura objOrigemCandidatura = new OrigemCandidatura();
				if (SetInstance(dr, objOrigemCandidatura))
					return objOrigemCandidatura;
			}
			throw (new RecordNotFoundException(typeof(OrigemCandidatura)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de OrigemCandidatura.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemCandidatura LoadObject(int idOrigemCandidatura, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idOrigemCandidatura, trans))
			{
				OrigemCandidatura objOrigemCandidatura = new OrigemCandidatura();
				if (SetInstance(dr, objOrigemCandidatura))
					return objOrigemCandidatura;
			}
			throw (new RecordNotFoundException(typeof(OrigemCandidatura)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemCandidatura a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemCandidatura))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemCandidatura, trans))
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
		/// <param name="objOrigemCandidatura">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, OrigemCandidatura objOrigemCandidatura, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objOrigemCandidatura._idOrigemCandidatura = Convert.ToInt32(dr["Idf_Origem_Candidatura"]);
					if (dr["Des_Origem"] != DBNull.Value)
						objOrigemCandidatura._descricaoOrigem = Convert.ToString(dr["Des_Origem"]);

					objOrigemCandidatura._persisted = true;
					objOrigemCandidatura._modified = false;

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