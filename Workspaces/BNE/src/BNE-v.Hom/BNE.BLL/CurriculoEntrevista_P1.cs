//-- Data: 09/11/2010 17:15
//-- Autor: Bruno Flammarion Chervisnki Boscolo

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoEntrevista // Tabela: BNE_Curriculo_Entrevista
	{
		#region Atributos
		private int _idCurriculoEntrevista;
		private Vaga _vaga;
		private DateTime _dataCadastro;
		private int _idMensagemCS;
		private Curriculo _curriculo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoEntrevista
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurriculoEntrevista
		{
			get
			{
				return this._idCurriculoEntrevista;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo opcional.
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

		#region IdMensagemCS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdMensagemCS
		{
			get
			{
				return this._idMensagemCS;
			}
			set
			{
				this._idMensagemCS = value;
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

		#endregion

		#region Construtores
		public CurriculoEntrevista()
		{
		}
		public CurriculoEntrevista(int idCurriculoEntrevista)
		{
			this._idCurriculoEntrevista = idCurriculoEntrevista;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Entrevista (Idf_Vaga, Dta_Cadastro, Idf_Mensagem_CS, Idf_Curriculo) VALUES (@Idf_Vaga, @Dta_Cadastro, @Idf_Mensagem_CS, @Idf_Curriculo);SET @Idf_Curriculo_Entrevista = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Entrevista SET Idf_Vaga = @Idf_Vaga, Dta_Cadastro = @Dta_Cadastro, Idf_Mensagem_CS = @Idf_Mensagem_CS, Idf_Curriculo = @Idf_Curriculo WHERE Idf_Curriculo_Entrevista = @Idf_Curriculo_Entrevista";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Entrevista WHERE Idf_Curriculo_Entrevista = @Idf_Curriculo_Entrevista";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Entrevista WHERE Idf_Curriculo_Entrevista = @Idf_Curriculo_Entrevista";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Entrevista", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCurriculoEntrevista;

			if (this._vaga != null)
				parms[1].Value = this._vaga.IdVaga;
			else
				parms[1].Value = DBNull.Value;

			parms[3].Value = this._idMensagemCS;
			parms[4].Value = this._curriculo.IdCurriculo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoEntrevista no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
						this._idCurriculoEntrevista = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Entrevista"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoEntrevista no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoEntrevista = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Entrevista"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoEntrevista no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
		/// Método utilizado para atualizar uma instância de CurriculoEntrevista no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
		/// Método utilizado para salvar uma instância de CurriculoEntrevista no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de CurriculoEntrevista no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
		/// Método utilizado para excluir uma instância de CurriculoEntrevista no banco de dados.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Chave do registro.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static void Delete(int idCurriculoEntrevista)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Entrevista", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoEntrevista;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoEntrevista no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static void Delete(int idCurriculoEntrevista, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Entrevista", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoEntrevista;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoEntrevista no banco de dados.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static void Delete(List<int> idCurriculoEntrevista)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Entrevista where Idf_Curriculo_Entrevista in (";

			for (int i = 0; i < idCurriculoEntrevista.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculoEntrevista[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private static IDataReader LoadDataReader(int idCurriculoEntrevista)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Entrevista", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoEntrevista;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private static IDataReader LoadDataReader(int idCurriculoEntrevista, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Entrevista", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoEntrevista;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Entrevista, Cur.Idf_Vaga, Cur.Dta_Cadastro, Cur.Idf_Mensagem_CS, Cur.Idf_Curriculo FROM BNE_Curriculo_Entrevista Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoEntrevista a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Chave do registro.</param>
		/// <returns>Instância de CurriculoEntrevista.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static CurriculoEntrevista LoadObject(int idCurriculoEntrevista)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoEntrevista))
			{
				CurriculoEntrevista objCurriculoEntrevista = new CurriculoEntrevista();
				if (SetInstance(dr, objCurriculoEntrevista))
					return objCurriculoEntrevista;
			}
			throw (new RecordNotFoundException(typeof(CurriculoEntrevista)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoEntrevista a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoEntrevista">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoEntrevista.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static CurriculoEntrevista LoadObject(int idCurriculoEntrevista, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoEntrevista, trans))
			{
				CurriculoEntrevista objCurriculoEntrevista = new CurriculoEntrevista();
				if (SetInstance(dr, objCurriculoEntrevista))
					return objCurriculoEntrevista;
			}
			throw (new RecordNotFoundException(typeof(CurriculoEntrevista)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoEntrevista a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoEntrevista))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoEntrevista a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoEntrevista, trans))
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
		/// <param name="objCurriculoEntrevista">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoEntrevista objCurriculoEntrevista)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoEntrevista._idCurriculoEntrevista = Convert.ToInt32(dr["Idf_Curriculo_Entrevista"]);
					if (dr["Idf_Vaga"] != DBNull.Value)
						objCurriculoEntrevista._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objCurriculoEntrevista._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCurriculoEntrevista._idMensagemCS = Convert.ToInt32(dr["Idf_Mensagem_CS"]);
					objCurriculoEntrevista._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));

					objCurriculoEntrevista._persisted = true;
					objCurriculoEntrevista._modified = false;

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