//-- Data: 04/04/2013 15:26
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MotivoRescisao // Tabela: plataforma.TAB_Motivo_Rescisao
	{
		#region Atributos
		private int _idMotivoRescisao;
		private string _descricaoMotivoRescisao;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _siglaCausaAfastamento;
		private string _siglaCodigoAfastamento;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMotivoRescisao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdMotivoRescisao
		{
			get
			{
				return this._idMotivoRescisao;
			}
			set
			{
				this._idMotivoRescisao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMotivoRescisao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMotivoRescisao
		{
			get
			{
				return this._descricaoMotivoRescisao;
			}
			set
			{
				this._descricaoMotivoRescisao = value;
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

		#region SiglaCausaAfastamento
		/// <summary>
		/// Tamanho do campo: 150.
		/// Campo opcional.
		/// </summary>
		public string SiglaCausaAfastamento
		{
			get
			{
				return this._siglaCausaAfastamento;
			}
			set
			{
				this._siglaCausaAfastamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SiglaCodigoAfastamento
		/// <summary>
		/// Tamanho do campo: 4.
		/// Campo opcional.
		/// </summary>
		public string SiglaCodigoAfastamento
		{
			get
			{
				return this._siglaCodigoAfastamento;
			}
			set
			{
				this._siglaCodigoAfastamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public MotivoRescisao()
		{
		}
		public MotivoRescisao(int idMotivoRescisao)
		{
			this._idMotivoRescisao = idMotivoRescisao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Motivo_Rescisao (Idf_Motivo_Rescisao, Des_Motivo_Rescisao, Flg_Inativo, Dta_Cadastro, Sig_Causa_Afastamento, Sig_Codigo_Afastamento) VALUES (@Idf_Motivo_Rescisao, @Des_Motivo_Rescisao, @Flg_Inativo, @Dta_Cadastro, @Sig_Causa_Afastamento, @Sig_Codigo_Afastamento);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Motivo_Rescisao SET Des_Motivo_Rescisao = @Des_Motivo_Rescisao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Sig_Causa_Afastamento = @Sig_Causa_Afastamento, Sig_Codigo_Afastamento = @Sig_Codigo_Afastamento WHERE Idf_Motivo_Rescisao = @Idf_Motivo_Rescisao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Motivo_Rescisao WHERE Idf_Motivo_Rescisao = @Idf_Motivo_Rescisao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Motivo_Rescisao WITH(NOLOCK) WHERE Idf_Motivo_Rescisao = @Idf_Motivo_Rescisao";
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
			parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Motivo_Rescisao", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Sig_Causa_Afastamento", SqlDbType.VarChar, 150));
			parms.Add(new SqlParameter("@Sig_Codigo_Afastamento", SqlDbType.VarChar, 4));
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
			parms[0].Value = this._idMotivoRescisao;
			parms[1].Value = this._descricaoMotivoRescisao;
			parms[2].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._siglaCausaAfastamento))
				parms[4].Value = this._siglaCausaAfastamento;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._siglaCodigoAfastamento))
				parms[5].Value = this._siglaCodigoAfastamento;
			else
				parms[5].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de MotivoRescisao no banco de dados.
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
		/// Método utilizado para inserir uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MotivoRescisao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de MotivoRescisao no banco de dados.
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
		/// Método utilizado para salvar uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de MotivoRescisao no banco de dados.
		/// </summary>
		/// <param name="idMotivoRescisao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMotivoRescisao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoRescisao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoRescisao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMotivoRescisao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoRescisao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MotivoRescisao no banco de dados.
		/// </summary>
		/// <param name="idMotivoRescisao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idMotivoRescisao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Motivo_Rescisao where Idf_Motivo_Rescisao in (";

			for (int i = 0; i < idMotivoRescisao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMotivoRescisao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMotivoRescisao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMotivoRescisao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoRescisao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoRescisao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMotivoRescisao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoRescisao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Mot.Idf_Motivo_Rescisao, Mot.Des_Motivo_Rescisao, Mot.Flg_Inativo, Mot.Dta_Cadastro, Mot.Sig_Causa_Afastamento, Mot.Sig_Codigo_Afastamento FROM plataforma.TAB_Motivo_Rescisao Mot";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoRescisao a partir do banco de dados.
		/// </summary>
		/// <param name="idMotivoRescisao">Chave do registro.</param>
		/// <returns>Instância de MotivoRescisao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MotivoRescisao LoadObject(int idMotivoRescisao)
		{
			using (IDataReader dr = LoadDataReader(idMotivoRescisao))
			{
				MotivoRescisao objMotivoRescisao = new MotivoRescisao();
				if (SetInstance(dr, objMotivoRescisao))
					return objMotivoRescisao;
			}
			throw (new RecordNotFoundException(typeof(MotivoRescisao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoRescisao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoRescisao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MotivoRescisao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MotivoRescisao LoadObject(int idMotivoRescisao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMotivoRescisao, trans))
			{
				MotivoRescisao objMotivoRescisao = new MotivoRescisao();
				if (SetInstance(dr, objMotivoRescisao))
					return objMotivoRescisao;
			}
			throw (new RecordNotFoundException(typeof(MotivoRescisao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoRescisao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoRescisao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoRescisao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoRescisao, trans))
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
		/// <param name="objMotivoRescisao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, MotivoRescisao objMotivoRescisao)
		{
			try
			{
				if (dr.Read())
				{
					objMotivoRescisao._idMotivoRescisao = Convert.ToInt32(dr["Idf_Motivo_Rescisao"]);
					objMotivoRescisao._descricaoMotivoRescisao = Convert.ToString(dr["Des_Motivo_Rescisao"]);
					objMotivoRescisao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objMotivoRescisao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Sig_Causa_Afastamento"] != DBNull.Value)
						objMotivoRescisao._siglaCausaAfastamento = Convert.ToString(dr["Sig_Causa_Afastamento"]);
					if (dr["Sig_Codigo_Afastamento"] != DBNull.Value)
						objMotivoRescisao._siglaCodigoAfastamento = Convert.ToString(dr["Sig_Codigo_Afastamento"]);

					objMotivoRescisao._persisted = true;
					objMotivoRescisao._modified = false;

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