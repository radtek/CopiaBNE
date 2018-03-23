//-- Data: 15/05/2013 13:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ParceiroTecla // Tabela: BNE_Parceiro_Tecla
	{
		#region Atributos
		private int _idParceiroTecla;
		private string _nomeParceiroTecla;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private string _descricaoURLCadastro;
		private string _descricaoURLAutenticacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdParceiroTecla
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdParceiroTecla
		{
			get
			{
				return this._idParceiroTecla;
			}
		}
		#endregion 

		#region NomeParceiroTecla
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string NomeParceiroTecla
		{
			get
			{
				return this._nomeParceiroTecla;
			}
			set
			{
				this._nomeParceiroTecla = value;
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

		#region DescricaoURLCadastro
		/// <summary>
		/// Tamanho do campo: 300.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoURLCadastro
		{
			get
			{
				return this._descricaoURLCadastro;
			}
			set
			{
				this._descricaoURLCadastro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoURLAutenticacao
		/// <summary>
		/// Tamanho do campo: 300.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoURLAutenticacao
		{
			get
			{
				return this._descricaoURLAutenticacao;
			}
			set
			{
				this._descricaoURLAutenticacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public ParceiroTecla()
		{
		}
		public ParceiroTecla(int idParceiroTecla)
		{
			this._idParceiroTecla = idParceiroTecla;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Parceiro_Tecla (Nme_Parceiro_Tecla, Dta_Cadastro, Flg_Inativo, Des_URL_Cadastro, Des_URL_Autenticacao) VALUES (@Nme_Parceiro_Tecla, @Dta_Cadastro, @Flg_Inativo, @Des_URL_Cadastro, @Des_URL_Autenticacao);SET @Idf_Parceiro_Tecla = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Parceiro_Tecla SET Nme_Parceiro_Tecla = @Nme_Parceiro_Tecla, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Des_URL_Cadastro = @Des_URL_Cadastro, Des_URL_Autenticacao = @Des_URL_Autenticacao WHERE Idf_Parceiro_Tecla = @Idf_Parceiro_Tecla";
		private const string SPDELETE = "DELETE FROM BNE_Parceiro_Tecla WHERE Idf_Parceiro_Tecla = @Idf_Parceiro_Tecla";
		private const string SPSELECTID = "SELECT * FROM BNE_Parceiro_Tecla WITH(NOLOCK) WHERE Idf_Parceiro_Tecla = @Idf_Parceiro_Tecla";
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
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Parceiro_Tecla", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_URL_Cadastro", SqlDbType.VarChar, 300));
			parms.Add(new SqlParameter("@Des_URL_Autenticacao", SqlDbType.VarChar, 300));
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
			parms[0].Value = this._idParceiroTecla;
			parms[1].Value = this._nomeParceiroTecla;
			parms[3].Value = this._flagInativo;
			parms[4].Value = this._descricaoURLCadastro;
			parms[5].Value = this._descricaoURLAutenticacao;

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
		/// Método utilizado para inserir uma instância de ParceiroTecla no banco de dados.
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
						this._idParceiroTecla = Convert.ToInt32(cmd.Parameters["@Idf_Parceiro_Tecla"].Value);
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
		/// Método utilizado para inserir uma instância de ParceiroTecla no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idParceiroTecla = Convert.ToInt32(cmd.Parameters["@Idf_Parceiro_Tecla"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de ParceiroTecla no banco de dados.
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
		/// Método utilizado para atualizar uma instância de ParceiroTecla no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de ParceiroTecla no banco de dados.
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
		/// Método utilizado para salvar uma instância de ParceiroTecla no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de ParceiroTecla no banco de dados.
		/// </summary>
		/// <param name="idParceiroTecla">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idParceiroTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idParceiroTecla;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ParceiroTecla no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParceiroTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idParceiroTecla, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idParceiroTecla;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de ParceiroTecla no banco de dados.
		/// </summary>
		/// <param name="idParceiroTecla">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idParceiroTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Parceiro_Tecla where Idf_Parceiro_Tecla in (";

			for (int i = 0; i < idParceiroTecla.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idParceiroTecla[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idParceiroTecla">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idParceiroTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idParceiroTecla;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParceiroTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idParceiroTecla, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idParceiroTecla;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Par.Idf_Parceiro_Tecla, Par.Nme_Parceiro_Tecla, Par.Dta_Cadastro, Par.Flg_Inativo, Par.Des_URL_Cadastro, Par.Des_URL_Autenticacao FROM BNE_Parceiro_Tecla Par";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ParceiroTecla a partir do banco de dados.
		/// </summary>
		/// <param name="idParceiroTecla">Chave do registro.</param>
		/// <returns>Instância de ParceiroTecla.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ParceiroTecla LoadObject(int idParceiroTecla)
		{
			using (IDataReader dr = LoadDataReader(idParceiroTecla))
			{
				ParceiroTecla objParceiroTecla = new ParceiroTecla();
				if (SetInstance(dr, objParceiroTecla))
					return objParceiroTecla;
			}
			throw (new RecordNotFoundException(typeof(ParceiroTecla)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ParceiroTecla a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParceiroTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ParceiroTecla.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ParceiroTecla LoadObject(int idParceiroTecla, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idParceiroTecla, trans))
			{
				ParceiroTecla objParceiroTecla = new ParceiroTecla();
				if (SetInstance(dr, objParceiroTecla))
					return objParceiroTecla;
			}
			throw (new RecordNotFoundException(typeof(ParceiroTecla)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ParceiroTecla a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idParceiroTecla))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ParceiroTecla a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idParceiroTecla, trans))
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
		/// <param name="objParceiroTecla">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, ParceiroTecla objParceiroTecla)
		{
			try
			{
				if (dr.Read())
				{
					objParceiroTecla._idParceiroTecla = Convert.ToInt32(dr["Idf_Parceiro_Tecla"]);
					objParceiroTecla._nomeParceiroTecla = Convert.ToString(dr["Nme_Parceiro_Tecla"]);
					objParceiroTecla._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objParceiroTecla._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objParceiroTecla._descricaoURLCadastro = Convert.ToString(dr["Des_URL_Cadastro"]);
					objParceiroTecla._descricaoURLAutenticacao = Convert.ToString(dr["Des_URL_Autenticacao"]);

					objParceiroTecla._persisted = true;
					objParceiroTecla._modified = false;

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