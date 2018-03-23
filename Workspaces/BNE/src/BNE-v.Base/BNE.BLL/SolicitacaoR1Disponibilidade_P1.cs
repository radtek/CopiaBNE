//-- Data: 25/03/2015 13:55
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SolicitacaoR1Disponibilidade // Tabela: BNE_Solicitacao_R1_Disponibilidade
	{
		#region Atributos
		private int _idSolicitacaoR1Disponibilidade;
		private Disponibilidade _disponibilidade;
		private SolicitacaoR1 _solicitacaoR1;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSolicitacaoR1Disponibilidade
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdSolicitacaoR1Disponibilidade
		{
			get
			{
				return this._idSolicitacaoR1Disponibilidade;
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

		#region SolicitacaoR1
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public SolicitacaoR1 SolicitacaoR1
		{
			get
			{
				return this._solicitacaoR1;
			}
			set
			{
				this._solicitacaoR1 = value;
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

		#endregion

		#region Construtores
		public SolicitacaoR1Disponibilidade()
		{
		}
		public SolicitacaoR1Disponibilidade(int idSolicitacaoR1Disponibilidade)
		{
			this._idSolicitacaoR1Disponibilidade = idSolicitacaoR1Disponibilidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Solicitacao_R1_Disponibilidade (Idf_Disponibilidade, Idf_Solicitacao_R1, Dta_Cadastro) VALUES (@Idf_Disponibilidade, @Idf_Solicitacao_R1, @Dta_Cadastro);SET @Idf_Solicitacao_R1_Disponibilidade = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Solicitacao_R1_Disponibilidade SET Idf_Disponibilidade = @Idf_Disponibilidade, Idf_Solicitacao_R1 = @Idf_Solicitacao_R1, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Solicitacao_R1_Disponibilidade = @Idf_Solicitacao_R1_Disponibilidade";
		private const string SPDELETE = "DELETE FROM BNE_Solicitacao_R1_Disponibilidade WHERE Idf_Solicitacao_R1_Disponibilidade = @Idf_Solicitacao_R1_Disponibilidade";
		private const string SPSELECTID = "SELECT * FROM BNE_Solicitacao_R1_Disponibilidade WITH(NOLOCK) WHERE Idf_Solicitacao_R1_Disponibilidade = @Idf_Solicitacao_R1_Disponibilidade";
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
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1_Disponibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Disponibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idSolicitacaoR1Disponibilidade;
			parms[1].Value = this._disponibilidade.IdDisponibilidade;
			parms[2].Value = this._solicitacaoR1.IdSolicitacaoR1;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de SolicitacaoR1Disponibilidade no banco de dados.
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
						this._idSolicitacaoR1Disponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Solicitacao_R1_Disponibilidade"].Value);
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
		/// Método utilizado para inserir uma instância de SolicitacaoR1Disponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idSolicitacaoR1Disponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Solicitacao_R1_Disponibilidade"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de SolicitacaoR1Disponibilidade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de SolicitacaoR1Disponibilidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de SolicitacaoR1Disponibilidade no banco de dados.
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
		/// Método utilizado para salvar uma instância de SolicitacaoR1Disponibilidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de SolicitacaoR1Disponibilidade no banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSolicitacaoR1Disponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1Disponibilidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SolicitacaoR1Disponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSolicitacaoR1Disponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1Disponibilidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SolicitacaoR1Disponibilidade no banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idSolicitacaoR1Disponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Solicitacao_R1_Disponibilidade where Idf_Solicitacao_R1_Disponibilidade in (";

			for (int i = 0; i < idSolicitacaoR1Disponibilidade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idSolicitacaoR1Disponibilidade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSolicitacaoR1Disponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1Disponibilidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSolicitacaoR1Disponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1Disponibilidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sol.Idf_Solicitacao_R1_Disponibilidade, Sol.Idf_Disponibilidade, Sol.Idf_Solicitacao_R1, Sol.Dta_Cadastro FROM BNE_Solicitacao_R1_Disponibilidade Sol";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SolicitacaoR1Disponibilidade a partir do banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Chave do registro.</param>
		/// <returns>Instância de SolicitacaoR1Disponibilidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SolicitacaoR1Disponibilidade LoadObject(int idSolicitacaoR1Disponibilidade)
		{
			using (IDataReader dr = LoadDataReader(idSolicitacaoR1Disponibilidade))
			{
				SolicitacaoR1Disponibilidade objSolicitacaoR1Disponibilidade = new SolicitacaoR1Disponibilidade();
				if (SetInstance(dr, objSolicitacaoR1Disponibilidade))
					return objSolicitacaoR1Disponibilidade;
			}
			throw (new RecordNotFoundException(typeof(SolicitacaoR1Disponibilidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SolicitacaoR1Disponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSolicitacaoR1Disponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SolicitacaoR1Disponibilidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SolicitacaoR1Disponibilidade LoadObject(int idSolicitacaoR1Disponibilidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSolicitacaoR1Disponibilidade, trans))
			{
				SolicitacaoR1Disponibilidade objSolicitacaoR1Disponibilidade = new SolicitacaoR1Disponibilidade();
				if (SetInstance(dr, objSolicitacaoR1Disponibilidade))
					return objSolicitacaoR1Disponibilidade;
			}
			throw (new RecordNotFoundException(typeof(SolicitacaoR1Disponibilidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de SolicitacaoR1Disponibilidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idSolicitacaoR1Disponibilidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de SolicitacaoR1Disponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idSolicitacaoR1Disponibilidade, trans))
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
		/// <param name="objSolicitacaoR1Disponibilidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, SolicitacaoR1Disponibilidade objSolicitacaoR1Disponibilidade)
		{
			try
			{
				if (dr.Read())
				{
					objSolicitacaoR1Disponibilidade._idSolicitacaoR1Disponibilidade = Convert.ToInt32(dr["Idf_Solicitacao_R1_Disponibilidade"]);
					objSolicitacaoR1Disponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));
					objSolicitacaoR1Disponibilidade._solicitacaoR1 = new SolicitacaoR1(Convert.ToInt32(dr["Idf_Solicitacao_R1"]));
					objSolicitacaoR1Disponibilidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objSolicitacaoR1Disponibilidade._persisted = true;
					objSolicitacaoR1Disponibilidade._modified = false;

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