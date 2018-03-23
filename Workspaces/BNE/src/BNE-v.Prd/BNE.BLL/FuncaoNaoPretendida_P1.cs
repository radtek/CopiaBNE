//-- Data: 26/08/2016 14:51
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class FuncaoNaoPretendida // Tabela: BNE_Funcao_Nao_Pretendida
	{
		#region Atributos
		private int _idFuncaoNaoPretendida;
		private Curriculo _curriculo;
		private Funcao _funcao;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFuncaoNaoPretendida
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFuncaoNaoPretendida
		{
			get
			{
				return this._idFuncaoNaoPretendida;
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

		#region Funcao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
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
		public FuncaoNaoPretendida()
		{
		}
		public FuncaoNaoPretendida(int idFuncaoNaoPretendida)
		{
			this._idFuncaoNaoPretendida = idFuncaoNaoPretendida;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Funcao_Nao_Pretendida (Idf_Curriculo, Idf_Funcao, Dta_Cadastro) VALUES (@Idf_Curriculo, @Idf_Funcao, @Dta_Cadastro);SET @Idf_Funcao_Nao_Pretendida = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Funcao_Nao_Pretendida SET Idf_Curriculo = @Idf_Curriculo, Idf_Funcao = @Idf_Funcao, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Funcao_Nao_Pretendida = @Idf_Funcao_Nao_Pretendida";
		private const string SPDELETE = "DELETE FROM BNE_Funcao_Nao_Pretendida WHERE Idf_Funcao_Nao_Pretendida = @Idf_Funcao_Nao_Pretendida";
		private const string SPSELECTID = "SELECT * FROM BNE_Funcao_Nao_Pretendida WITH(NOLOCK) WHERE Idf_Funcao_Nao_Pretendida = @Idf_Funcao_Nao_Pretendida";
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
			parms.Add(new SqlParameter("@Idf_Funcao_Nao_Pretendida", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idFuncaoNaoPretendida;
			parms[1].Value = this._curriculo.IdCurriculo;
			parms[2].Value = this._funcao.IdFuncao;

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
		/// Método utilizado para inserir uma instância de FuncaoNaoPretendida no banco de dados.
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
						this._idFuncaoNaoPretendida = Convert.ToInt32(cmd.Parameters["@Idf_Funcao_Nao_Pretendida"].Value);
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
		/// Método utilizado para inserir uma instância de FuncaoNaoPretendida no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idFuncaoNaoPretendida = Convert.ToInt32(cmd.Parameters["@Idf_Funcao_Nao_Pretendida"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de FuncaoNaoPretendida no banco de dados.
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
		/// Método utilizado para atualizar uma instância de FuncaoNaoPretendida no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de FuncaoNaoPretendida no banco de dados.
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
		/// Método utilizado para salvar uma instância de FuncaoNaoPretendida no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de FuncaoNaoPretendida no banco de dados.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idFuncaoNaoPretendida)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Nao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoNaoPretendida;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de FuncaoNaoPretendida no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idFuncaoNaoPretendida, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Nao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoNaoPretendida;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de FuncaoNaoPretendida no banco de dados.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idFuncaoNaoPretendida)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Funcao_Nao_Pretendida where Idf_Funcao_Nao_Pretendida in (";

			for (int i = 0; i < idFuncaoNaoPretendida.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFuncaoNaoPretendida[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idFuncaoNaoPretendida)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Nao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoNaoPretendida;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idFuncaoNaoPretendida, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Nao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoNaoPretendida;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fun.Idf_Funcao_Nao_Pretendida, Fun.Idf_Curriculo, Fun.Idf_Funcao, Fun.Dta_Cadastro FROM BNE_Funcao_Nao_Pretendida Fun";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoNaoPretendida a partir do banco de dados.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Chave do registro.</param>
		/// <returns>Instância de FuncaoNaoPretendida.</returns>
		/// <remarks>Mailson</remarks>
		public static FuncaoNaoPretendida LoadObject(int idFuncaoNaoPretendida)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoNaoPretendida))
			{
				FuncaoNaoPretendida objFuncaoNaoPretendida = new FuncaoNaoPretendida();
				if (SetInstance(dr, objFuncaoNaoPretendida))
					return objFuncaoNaoPretendida;
			}
			throw (new RecordNotFoundException(typeof(FuncaoNaoPretendida)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoNaoPretendida a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoNaoPretendida">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de FuncaoNaoPretendida.</returns>
		/// <remarks>Mailson</remarks>
		public static FuncaoNaoPretendida LoadObject(int idFuncaoNaoPretendida, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoNaoPretendida, trans))
			{
				FuncaoNaoPretendida objFuncaoNaoPretendida = new FuncaoNaoPretendida();
				if (SetInstance(dr, objFuncaoNaoPretendida))
					return objFuncaoNaoPretendida;
			}
			throw (new RecordNotFoundException(typeof(FuncaoNaoPretendida)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoNaoPretendida a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoNaoPretendida))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoNaoPretendida a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoNaoPretendida, trans))
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
		/// <param name="objFuncaoNaoPretendida">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, FuncaoNaoPretendida objFuncaoNaoPretendida, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objFuncaoNaoPretendida._idFuncaoNaoPretendida = Convert.ToInt32(dr["Idf_Funcao_Nao_Pretendida"]);
					objFuncaoNaoPretendida._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objFuncaoNaoPretendida._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					objFuncaoNaoPretendida._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objFuncaoNaoPretendida._persisted = true;
					objFuncaoNaoPretendida._modified = false;

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