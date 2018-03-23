//-- Data: 24/02/2011 14:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class OrigemFilialFuncao // Tabela: TAB_Origem_Filial_Funcao
	{
		#region Atributos
		private int _idOrigemFilialFuncao;
		private OrigemFilial _origemFilial;
		private Funcao _funcao;
		private string _descricaoFuncao;
		private DateTime? _dataCadastro;
		private DateTime? _dataAlteracao;
		private bool? _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdOrigemFilialFuncao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdOrigemFilialFuncao
		{
			get
			{
				return this._idOrigemFilialFuncao;
			}
		}
		#endregion 

		#region OrigemFilial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public OrigemFilial OrigemFilial
		{
			get
			{
				return this._origemFilial;
			}
			set
			{
				this._origemFilial = value;
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

		#region DescricaoFuncao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoFuncao
		{
			get
			{
				return this._descricaoFuncao;
			}
			set
			{
				this._descricaoFuncao = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagInativo
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

		#endregion

		#region Construtores
		public OrigemFilialFuncao()
		{
		}
		public OrigemFilialFuncao(int idOrigemFilialFuncao)
		{
			this._idOrigemFilialFuncao = idOrigemFilialFuncao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Origem_Filial_Funcao (Idf_Origem_Filial, Idf_Funcao, Des_Funcao, Dta_Cadastro, Dta_Alteracao, Flg_Inativo) VALUES (@Idf_Origem_Filial, @Idf_Funcao, @Des_Funcao, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo);SET @Idf_Origem_Filial_Funcao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Origem_Filial_Funcao SET Idf_Origem_Filial = @Idf_Origem_Filial, Idf_Funcao = @Idf_Funcao, Des_Funcao = @Des_Funcao, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo WHERE Idf_Origem_Filial_Funcao = @Idf_Origem_Filial_Funcao";
		private const string SPDELETE = "DELETE FROM TAB_Origem_Filial_Funcao WHERE Idf_Origem_Filial_Funcao = @Idf_Origem_Filial_Funcao";
		private const string SPSELECTID = "SELECT * FROM TAB_Origem_Filial_Funcao WHERE Idf_Origem_Filial_Funcao = @Idf_Origem_Filial_Funcao";
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
			parms.Add(new SqlParameter("@Idf_Origem_Filial_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Origem_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idOrigemFilialFuncao;
			parms[1].Value = this._origemFilial.IdOrigemFilial;
			parms[2].Value = this._funcao.IdFuncao;

			if (!String.IsNullOrEmpty(this._descricaoFuncao))
				parms[3].Value = this._descricaoFuncao;
			else
				parms[3].Value = DBNull.Value;


			if (this._flagInativo.HasValue)
				parms[6].Value = this._flagInativo;
			else
				parms[6].Value = DBNull.Value;


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
			this._dataAlteracao = DateTime.Now;
			parms[5].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de OrigemFilialFuncao no banco de dados.
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
						this._idOrigemFilialFuncao = Convert.ToInt32(cmd.Parameters["@Idf_Origem_Filial_Funcao"].Value);
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
		/// Método utilizado para inserir uma instância de OrigemFilialFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idOrigemFilialFuncao = Convert.ToInt32(cmd.Parameters["@Idf_Origem_Filial_Funcao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de OrigemFilialFuncao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de OrigemFilialFuncao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de OrigemFilialFuncao no banco de dados.
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
		/// Método utilizado para salvar uma instância de OrigemFilialFuncao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de OrigemFilialFuncao no banco de dados.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemFilialFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilialFuncao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de OrigemFilialFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemFilialFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilialFuncao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de OrigemFilialFuncao no banco de dados.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idOrigemFilialFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Origem_Filial_Funcao where Idf_Origem_Filial_Funcao in (";

			for (int i = 0; i < idOrigemFilialFuncao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idOrigemFilialFuncao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemFilialFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilialFuncao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemFilialFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilialFuncao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ori.Idf_Origem_Filial_Funcao, Ori.Idf_Origem_Filial, Ori.Idf_Funcao, Ori.Des_Funcao, Ori.Dta_Cadastro, Ori.Dta_Alteracao, Ori.Flg_Inativo FROM TAB_Origem_Filial_Funcao Ori";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemFilialFuncao a partir do banco de dados.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Chave do registro.</param>
		/// <returns>Instância de OrigemFilialFuncao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemFilialFuncao LoadObject(int idOrigemFilialFuncao)
		{
			using (IDataReader dr = LoadDataReader(idOrigemFilialFuncao))
			{
				OrigemFilialFuncao objOrigemFilialFuncao = new OrigemFilialFuncao();
				if (SetInstance(dr, objOrigemFilialFuncao))
					return objOrigemFilialFuncao;
			}
			throw (new RecordNotFoundException(typeof(OrigemFilialFuncao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemFilialFuncao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemFilialFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de OrigemFilialFuncao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemFilialFuncao LoadObject(int idOrigemFilialFuncao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idOrigemFilialFuncao, trans))
			{
				OrigemFilialFuncao objOrigemFilialFuncao = new OrigemFilialFuncao();
				if (SetInstance(dr, objOrigemFilialFuncao))
					return objOrigemFilialFuncao;
			}
			throw (new RecordNotFoundException(typeof(OrigemFilialFuncao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemFilialFuncao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemFilialFuncao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemFilialFuncao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemFilialFuncao, trans))
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
		/// <param name="objOrigemFilialFuncao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, OrigemFilialFuncao objOrigemFilialFuncao)
		{
			try
			{
				if (dr.Read())
				{
					objOrigemFilialFuncao._idOrigemFilialFuncao = Convert.ToInt32(dr["Idf_Origem_Filial_Funcao"]);
					objOrigemFilialFuncao._origemFilial = new OrigemFilial(Convert.ToInt32(dr["Idf_Origem_Filial"]));
					objOrigemFilialFuncao._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					if (dr["Des_Funcao"] != DBNull.Value)
						objOrigemFilialFuncao._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objOrigemFilialFuncao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Dta_Alteracao"] != DBNull.Value)
						objOrigemFilialFuncao._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					if (dr["Flg_Inativo"] != DBNull.Value)
						objOrigemFilialFuncao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objOrigemFilialFuncao._persisted = true;
					objOrigemFilialFuncao._modified = false;

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