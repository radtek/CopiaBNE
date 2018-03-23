//-- Data: 08/06/2016 14:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class HistoricoCandidatura // Tabela: BNE_Historico_Candidatura
	{
		#region Atributos
		private int _idHistoricoCandidatura;
		private Vaga _vaga;
		private Curriculo _curriculo;
		private DateTime _dataCandidatura;
		private string _descricaoCandidatura;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdHistoricoCandidatura
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdHistoricoCandidatura
		{
			get
			{
				return this._idHistoricoCandidatura;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCandidatura
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataCandidatura
		{
			get
			{
				return this._dataCandidatura;
			}
			set
			{
				this._dataCandidatura = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCandidatura
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCandidatura
		{
			get
			{
				return this._descricaoCandidatura;
			}
			set
			{
				this._descricaoCandidatura = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public HistoricoCandidatura()
		{
		}
		public HistoricoCandidatura(int idHistoricoCandidatura)
		{
			this._idHistoricoCandidatura = idHistoricoCandidatura;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Historico_Candidatura (Idf_Vaga, Idf_Curriculo, Dta_Candidatura, Des_Candidatura) VALUES (@Idf_Vaga, @Idf_Curriculo, @Dta_Candidatura, @Des_Candidatura);SET @Idf_Historico_Candidatura = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Historico_Candidatura SET Idf_Vaga = @Idf_Vaga, Idf_Curriculo = @Idf_Curriculo, Dta_Candidatura = @Dta_Candidatura, Des_Candidatura = @Des_Candidatura WHERE Idf_Historico_Candidatura = @Idf_Historico_Candidatura";
		private const string SPDELETE = "DELETE FROM BNE_Historico_Candidatura WHERE Idf_Historico_Candidatura = @Idf_Historico_Candidatura";
		private const string SPSELECTID = "SELECT * FROM BNE_Historico_Candidatura WITH(NOLOCK) WHERE Idf_Historico_Candidatura = @Idf_Historico_Candidatura";
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
			parms.Add(new SqlParameter("@Idf_Historico_Candidatura", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Candidatura", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Candidatura", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idHistoricoCandidatura;
			parms[1].Value = this._vaga.IdVaga;
			parms[2].Value = this._curriculo.IdCurriculo;
			parms[3].Value = this._dataCandidatura;

			if (!String.IsNullOrEmpty(this._descricaoCandidatura))
				parms[4].Value = this._descricaoCandidatura;
			else
				parms[4].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de HistoricoCandidatura no banco de dados.
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
						this._idHistoricoCandidatura = Convert.ToInt32(cmd.Parameters["@Idf_Historico_Candidatura"].Value);
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
		/// Método utilizado para inserir uma instância de HistoricoCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idHistoricoCandidatura = Convert.ToInt32(cmd.Parameters["@Idf_Historico_Candidatura"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de HistoricoCandidatura no banco de dados.
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
		/// Método utilizado para atualizar uma instância de HistoricoCandidatura no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de HistoricoCandidatura no banco de dados.
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
		/// Método utilizado para salvar uma instância de HistoricoCandidatura no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de HistoricoCandidatura no banco de dados.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idHistoricoCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoCandidatura;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de HistoricoCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idHistoricoCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoCandidatura;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de HistoricoCandidatura no banco de dados.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idHistoricoCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Historico_Candidatura where Idf_Historico_Candidatura in (";

			for (int i = 0; i < idHistoricoCandidatura.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idHistoricoCandidatura[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idHistoricoCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoCandidatura;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idHistoricoCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoCandidatura;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, His.Idf_Historico_Candidatura, His.Idf_Vaga, His.Idf_Curriculo, His.Dta_Candidatura, His.Des_Candidatura FROM BNE_Historico_Candidatura His";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de HistoricoCandidatura a partir do banco de dados.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Chave do registro.</param>
		/// <returns>Instância de HistoricoCandidatura.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static HistoricoCandidatura LoadObject(int idHistoricoCandidatura)
		{
			using (IDataReader dr = LoadDataReader(idHistoricoCandidatura))
			{
				HistoricoCandidatura objHistoricoCandidatura = new HistoricoCandidatura();
				if (SetInstance(dr, objHistoricoCandidatura))
					return objHistoricoCandidatura;
			}
			throw (new RecordNotFoundException(typeof(HistoricoCandidatura)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de HistoricoCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idHistoricoCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de HistoricoCandidatura.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static HistoricoCandidatura LoadObject(int idHistoricoCandidatura, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idHistoricoCandidatura, trans))
			{
				HistoricoCandidatura objHistoricoCandidatura = new HistoricoCandidatura();
				if (SetInstance(dr, objHistoricoCandidatura))
					return objHistoricoCandidatura;
			}
			throw (new RecordNotFoundException(typeof(HistoricoCandidatura)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de HistoricoCandidatura a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idHistoricoCandidatura))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de HistoricoCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idHistoricoCandidatura, trans))
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
		/// <param name="objHistoricoCandidatura">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, HistoricoCandidatura objHistoricoCandidatura, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objHistoricoCandidatura._idHistoricoCandidatura = Convert.ToInt32(dr["Idf_Historico_Candidatura"]);
					objHistoricoCandidatura._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objHistoricoCandidatura._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objHistoricoCandidatura._dataCandidatura = Convert.ToDateTime(dr["Dta_Candidatura"]);
					if (dr["Des_Candidatura"] != DBNull.Value)
						objHistoricoCandidatura._descricaoCandidatura = Convert.ToString(dr["Des_Candidatura"]);

					objHistoricoCandidatura._persisted = true;
					objHistoricoCandidatura._modified = false;

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