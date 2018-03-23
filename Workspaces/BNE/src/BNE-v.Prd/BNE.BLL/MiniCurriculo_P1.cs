//-- Data: 13/05/2010 16:05
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MiniCurriculo // Tabela: BNE_Mini_Curriculo
	{
		#region Atributos
		private int _idMiniCurriculo;
		private PessoaFisica _pessoaFisica;
		private string _descricaoMiniCurriculo;
		private DateTime _dataCadastro;
		private PessoaFisicaTemp _pessoaFisicaTemp;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMiniCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMiniCurriculo
		{
			get
			{
				return this._idMiniCurriculo;
			}
		}
		#endregion 

		#region PessoaFisica
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public PessoaFisica PessoaFisica
		{
			get
			{
				return this._pessoaFisica;
			}
			set
			{
				this._pessoaFisica = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMiniCurriculo
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMiniCurriculo
		{
			get
			{
				return this._descricaoMiniCurriculo;
			}
			set
			{
				this._descricaoMiniCurriculo = value;
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

		#region PessoaFisicaTemp
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public PessoaFisicaTemp PessoaFisicaTemp
		{
			get
			{
				return this._pessoaFisicaTemp;
			}
			set
			{
				this._pessoaFisicaTemp = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public MiniCurriculo()
		{
		}
		public MiniCurriculo(int idMiniCurriculo)
		{
			this._idMiniCurriculo = idMiniCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Mini_Curriculo (Idf_Pessoa_Fisica, Des_Mini_Curriculo, Dta_Cadastro, Idf_Pessoa_Fisica_Temp) VALUES (@Idf_Pessoa_Fisica, @Des_Mini_Curriculo, @Dta_Cadastro, @Idf_Pessoa_Fisica_Temp);SET @Idf_Mini_Curriculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Mini_Curriculo SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Des_Mini_Curriculo = @Des_Mini_Curriculo, Dta_Cadastro = @Dta_Cadastro, Idf_Pessoa_Fisica_Temp = @Idf_Pessoa_Fisica_Temp WHERE Idf_Mini_Curriculo = @Idf_Mini_Curriculo";
		private const string SPDELETE = "DELETE FROM BNE_Mini_Curriculo WHERE Idf_Mini_Curriculo = @Idf_Mini_Curriculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Mini_Curriculo WHERE Idf_Mini_Curriculo = @Idf_Mini_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Mini_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mini_Curriculo", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Temp", SqlDbType.Int, 4));
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
			parms[0].Value = this._idMiniCurriculo;

			if (this._pessoaFisica != null)
				parms[1].Value = this._pessoaFisica.IdPessoaFisica;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._descricaoMiniCurriculo;

			if (this._pessoaFisicaTemp != null)
				parms[4].Value = this._pessoaFisicaTemp.IdPessoaFisicaTemp;
			else
				parms[4].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de MiniCurriculo no banco de dados.
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
						this._idMiniCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Mini_Curriculo"].Value);
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
		/// Método utilizado para inserir uma instância de MiniCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMiniCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Mini_Curriculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MiniCurriculo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de MiniCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de MiniCurriculo no banco de dados.
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
		/// Método utilizado para salvar uma instância de MiniCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de MiniCurriculo no banco de dados.
		/// </summary>
		/// <param name="idMiniCurriculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMiniCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mini_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idMiniCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MiniCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMiniCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMiniCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mini_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idMiniCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MiniCurriculo no banco de dados.
		/// </summary>
		/// <param name="idMiniCurriculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idMiniCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Mini_Curriculo where Idf_Mini_Curriculo in (";

			for (int i = 0; i < idMiniCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMiniCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMiniCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMiniCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mini_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idMiniCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMiniCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMiniCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mini_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idMiniCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Min.Idf_Mini_Curriculo, Min.Idf_Pessoa_Fisica, Min.Des_Mini_Curriculo, Min.Dta_Cadastro, Min.Idf_Pessoa_Fisica_Temp FROM BNE_Mini_Curriculo Min";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MiniCurriculo a partir do banco de dados.
		/// </summary>
		/// <param name="idMiniCurriculo">Chave do registro.</param>
		/// <returns>Instância de MiniCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MiniCurriculo LoadObject(int idMiniCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idMiniCurriculo))
			{
				MiniCurriculo objMiniCurriculo = new MiniCurriculo();
				if (SetInstance(dr, objMiniCurriculo))
					return objMiniCurriculo;
			}
			throw (new RecordNotFoundException(typeof(MiniCurriculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MiniCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMiniCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MiniCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MiniCurriculo LoadObject(int idMiniCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMiniCurriculo, trans))
			{
				MiniCurriculo objMiniCurriculo = new MiniCurriculo();
				if (SetInstance(dr, objMiniCurriculo))
					return objMiniCurriculo;
			}
			throw (new RecordNotFoundException(typeof(MiniCurriculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MiniCurriculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMiniCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MiniCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMiniCurriculo, trans))
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
		/// <param name="objMiniCurriculo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, MiniCurriculo objMiniCurriculo)
		{
			try
			{
				if (dr.Read())
				{
					objMiniCurriculo._idMiniCurriculo = Convert.ToInt32(dr["Idf_Mini_Curriculo"]);
					if (dr["Idf_Pessoa_Fisica"] != DBNull.Value)
						objMiniCurriculo._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					objMiniCurriculo._descricaoMiniCurriculo = Convert.ToString(dr["Des_Mini_Curriculo"]);
					objMiniCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Pessoa_Fisica_Temp"] != DBNull.Value)
						objMiniCurriculo._pessoaFisicaTemp = new PessoaFisicaTemp(Convert.ToInt32(dr["Idf_Pessoa_Fisica_Temp"]));

					objMiniCurriculo._persisted = true;
					objMiniCurriculo._modified = false;

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