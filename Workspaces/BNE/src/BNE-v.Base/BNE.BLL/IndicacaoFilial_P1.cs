//-- Data: 14/02/2011 09:16
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class IndicacaoFilial // Tabela: BNE_Indicacao_Filial
	{
		#region Atributos
		private int _idIndicacaoFilial;
		private string _nomeEmpresa;
		private Cidade _cidade;
		private bool _flagInativo;
		private DateTime? _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdIndicacaoFilial
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdIndicacaoFilial
		{
			get
			{
				return this._idIndicacaoFilial;
			}
		}
		#endregion 

		#region NomeEmpresa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomeEmpresa
		{
			get
			{
				return this._nomeEmpresa;
			}
			set
			{
				this._nomeEmpresa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public IndicacaoFilial()
		{
		}
		public IndicacaoFilial(int idIndicacaoFilial)
		{
			this._idIndicacaoFilial = idIndicacaoFilial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Indicacao_Filial (Nme_Empresa, Idf_Cidade, Flg_Inativo, Dta_Cadastro) VALUES (@Nme_Empresa, @Idf_Cidade, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Indicacao_Filial = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Indicacao_Filial SET Nme_Empresa = @Nme_Empresa, Idf_Cidade = @Idf_Cidade, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Indicacao_Filial = @Idf_Indicacao_Filial";
		private const string SPDELETE = "DELETE FROM BNE_Indicacao_Filial WHERE Idf_Indicacao_Filial = @Idf_Indicacao_Filial";
		private const string SPSELECTID = "SELECT * FROM BNE_Indicacao_Filial WHERE Idf_Indicacao_Filial = @Idf_Indicacao_Filial";
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
			parms.Add(new SqlParameter("@Idf_Indicacao_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Empresa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idIndicacaoFilial;
			parms[1].Value = this._nomeEmpresa;
			parms[2].Value = this._cidade.IdCidade;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de IndicacaoFilial no banco de dados.
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
						this._idIndicacaoFilial = Convert.ToInt32(cmd.Parameters["@Idf_Indicacao_Filial"].Value);
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
		/// Método utilizado para inserir uma instância de IndicacaoFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idIndicacaoFilial = Convert.ToInt32(cmd.Parameters["@Idf_Indicacao_Filial"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de IndicacaoFilial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de IndicacaoFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de IndicacaoFilial no banco de dados.
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
		/// Método utilizado para salvar uma instância de IndicacaoFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de IndicacaoFilial no banco de dados.
		/// </summary>
		/// <param name="idIndicacaoFilial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idIndicacaoFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Indicacao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIndicacaoFilial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de IndicacaoFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIndicacaoFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idIndicacaoFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Indicacao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIndicacaoFilial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de IndicacaoFilial no banco de dados.
		/// </summary>
		/// <param name="idIndicacaoFilial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idIndicacaoFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Indicacao_Filial where Idf_Indicacao_Filial in (";

			for (int i = 0; i < idIndicacaoFilial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idIndicacaoFilial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idIndicacaoFilial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idIndicacaoFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Indicacao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIndicacaoFilial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIndicacaoFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idIndicacaoFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Indicacao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIndicacaoFilial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ind.Idf_Indicacao_Filial, Ind.Nme_Empresa, Ind.Idf_Cidade, Ind.Flg_Inativo, Ind.Dta_Cadastro FROM BNE_Indicacao_Filial Ind";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de IndicacaoFilial a partir do banco de dados.
		/// </summary>
		/// <param name="idIndicacaoFilial">Chave do registro.</param>
		/// <returns>Instância de IndicacaoFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static IndicacaoFilial LoadObject(int idIndicacaoFilial)
		{
			using (IDataReader dr = LoadDataReader(idIndicacaoFilial))
			{
				IndicacaoFilial objIndicacaoFilial = new IndicacaoFilial();
				if (SetInstance(dr, objIndicacaoFilial))
					return objIndicacaoFilial;
			}
			throw (new RecordNotFoundException(typeof(IndicacaoFilial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de IndicacaoFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIndicacaoFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de IndicacaoFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static IndicacaoFilial LoadObject(int idIndicacaoFilial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idIndicacaoFilial, trans))
			{
				IndicacaoFilial objIndicacaoFilial = new IndicacaoFilial();
				if (SetInstance(dr, objIndicacaoFilial))
					return objIndicacaoFilial;
			}
			throw (new RecordNotFoundException(typeof(IndicacaoFilial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de IndicacaoFilial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idIndicacaoFilial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de IndicacaoFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idIndicacaoFilial, trans))
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
		/// <param name="objIndicacaoFilial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, IndicacaoFilial objIndicacaoFilial)
		{
			try
			{
				if (dr.Read())
				{
					objIndicacaoFilial._idIndicacaoFilial = Convert.ToInt32(dr["Idf_Indicacao_Filial"]);
					objIndicacaoFilial._nomeEmpresa = Convert.ToString(dr["Nme_Empresa"]);
					objIndicacaoFilial._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					objIndicacaoFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objIndicacaoFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objIndicacaoFilial._persisted = true;
					objIndicacaoFilial._modified = false;

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