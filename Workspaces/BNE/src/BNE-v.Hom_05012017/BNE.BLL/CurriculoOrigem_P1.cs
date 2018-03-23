//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoOrigem // Tabela: BNE_Curriculo_Origem
	{
		#region Atributos
		private int _idCurriculoOrigem;
		private Origem _origem;
		private Curriculo _curriculo;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private string _descricaoIP;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoOrigem
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurriculoOrigem
		{
			get
			{
				return this._idCurriculoOrigem;
			}
		}
		#endregion 

		#region Origem
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Origem Origem
		{
			get
			{
				return this._origem;
			}
			set
			{
				this._origem = value;
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

		#region DescricaoIP
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string DescricaoIP
		{
			get
			{
				return this._descricaoIP;
			}
			set
			{
				this._descricaoIP = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CurriculoOrigem()
		{
		}
		public CurriculoOrigem(int idCurriculoOrigem)
		{
			this._idCurriculoOrigem = idCurriculoOrigem;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Origem (Idf_Origem, Idf_Curriculo, Dta_Cadastro, Dta_Alteracao, Des_IP) VALUES (@Idf_Origem, @Idf_Curriculo, @Dta_Cadastro, @Dta_Alteracao, @Des_IP);SET @Idf_Curriculo_Origem = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Origem SET Idf_Origem = @Idf_Origem, Idf_Curriculo = @Idf_Curriculo, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Des_IP = @Des_IP WHERE Idf_Curriculo_Origem = @Idf_Curriculo_Origem";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Origem WHERE Idf_Curriculo_Origem = @Idf_Curriculo_Origem";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Origem WHERE Idf_Curriculo_Origem = @Idf_Curriculo_Origem";
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
			parms.Add(new SqlParameter("@Idf_Curriculo_Origem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.VarChar, 15));
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
			parms[0].Value = this._idCurriculoOrigem;
			parms[1].Value = this._origem.IdOrigem;
			parms[2].Value = this._curriculo.IdCurriculo;

			if (!String.IsNullOrEmpty(this._descricaoIP))
				parms[5].Value = this._descricaoIP;
			else
				parms[5].Value = DBNull.Value;


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
			this._dataAlteracao = DateTime.Now;
			parms[4].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoOrigem no banco de dados.
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
						this._idCurriculoOrigem = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Origem"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoOrigem no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoOrigem = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Origem"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoOrigem no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CurriculoOrigem no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CurriculoOrigem no banco de dados.
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
		/// Método utilizado para salvar uma instância de CurriculoOrigem no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CurriculoOrigem no banco de dados.
		/// </summary>
		/// <param name="idCurriculoOrigem">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurriculoOrigem)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Origem", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoOrigem;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoOrigem no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoOrigem">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurriculoOrigem, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Origem", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoOrigem;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoOrigem no banco de dados.
		/// </summary>
		/// <param name="idCurriculoOrigem">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCurriculoOrigem)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Origem where Idf_Curriculo_Origem in (";

			for (int i = 0; i < idCurriculoOrigem.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculoOrigem[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoOrigem">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurriculoOrigem)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Origem", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoOrigem;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoOrigem">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurriculoOrigem, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Origem", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoOrigem;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Origem, Cur.Idf_Origem, Cur.Idf_Curriculo, Cur.Dta_Cadastro, Cur.Dta_Alteracao, Cur.Des_IP FROM BNE_Curriculo_Origem Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoOrigem a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoOrigem">Chave do registro.</param>
		/// <returns>Instância de CurriculoOrigem.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoOrigem LoadObject(int idCurriculoOrigem)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoOrigem))
			{
				CurriculoOrigem objCurriculoOrigem = new CurriculoOrigem();
				if (SetInstance(dr, objCurriculoOrigem))
					return objCurriculoOrigem;
			}
			throw (new RecordNotFoundException(typeof(CurriculoOrigem)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoOrigem a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoOrigem">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoOrigem.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoOrigem LoadObject(int idCurriculoOrigem, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoOrigem, trans))
			{
				CurriculoOrigem objCurriculoOrigem = new CurriculoOrigem();
				if (SetInstance(dr, objCurriculoOrigem))
					return objCurriculoOrigem;
			}
			throw (new RecordNotFoundException(typeof(CurriculoOrigem)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoOrigem a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoOrigem))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoOrigem a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoOrigem, trans))
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
		/// <param name="objCurriculoOrigem">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoOrigem objCurriculoOrigem)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoOrigem._idCurriculoOrigem = Convert.ToInt32(dr["Idf_Curriculo_Origem"]);
					objCurriculoOrigem._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));
					objCurriculoOrigem._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objCurriculoOrigem._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCurriculoOrigem._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					if (dr["Des_IP"] != DBNull.Value)
						objCurriculoOrigem._descricaoIP = Convert.ToString(dr["Des_IP"]);

					objCurriculoOrigem._persisted = true;
					objCurriculoOrigem._modified = false;

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