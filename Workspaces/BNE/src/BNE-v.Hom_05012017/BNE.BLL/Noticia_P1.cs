//-- Data: 17/03/2011 16:59
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Noticia // Tabela: BNE_Noticia
	{
		#region Atributos
		private int _idNoticia;
		private string _descricaoNoticia;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _nomeTituloNoticia;
		private string _nomeLinkNoticia;
		private bool _flagNoticiaBNE;
		private bool _flagExibicao;
		private DateTime? _dataPublicacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdNoticia
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdNoticia
		{
			get
			{
				return this._idNoticia;
			}
		}
		#endregion 

		#region DescricaoNoticia
		/// <summary>
		/// Tamanho do campo: Max.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoNoticia
		{
			get
			{
				return this._descricaoNoticia;
			}
			set
			{
				this._descricaoNoticia = value;
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

		#region NomeTituloNoticia
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomeTituloNoticia
		{
			get
			{
				return this._nomeTituloNoticia;
			}
			set
			{
				this._nomeTituloNoticia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeLinkNoticia
		/// <summary>
		/// Tamanho do campo: 300.
		/// Campo opcional.
		/// </summary>
		public string NomeLinkNoticia
		{
			get
			{
				return this._nomeLinkNoticia;
			}
			set
			{
				this._nomeLinkNoticia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagNoticiaBNE
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagNoticiaBNE
		{
			get
			{
				return this._flagNoticiaBNE;
			}
			set
			{
				this._flagNoticiaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagExibicao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagExibicao
		{
			get
			{
				return this._flagExibicao;
			}
			set
			{
				this._flagExibicao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataPublicacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataPublicacao
		{
			get
			{
				return this._dataPublicacao;
			}
			set
			{
				this._dataPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Noticia()
		{
		}
		public Noticia(int idNoticia)
		{
			this._idNoticia = idNoticia;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Noticia (Des_Noticia, Flg_Inativo, Dta_Cadastro, Nme_Titulo_Noticia, Nme_Link_Noticia, Flg_Noticia_BNE, Flg_Exibicao, Dta_Publicacao) VALUES (@Des_Noticia, @Flg_Inativo, @Dta_Cadastro, @Nme_Titulo_Noticia, @Nme_Link_Noticia, @Flg_Noticia_BNE, @Flg_Exibicao, @Dta_Publicacao);SET @Idf_Noticia = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Noticia SET Des_Noticia = @Des_Noticia, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Nme_Titulo_Noticia = @Nme_Titulo_Noticia, Nme_Link_Noticia = @Nme_Link_Noticia, Flg_Noticia_BNE = @Flg_Noticia_BNE, Flg_Exibicao = @Flg_Exibicao, Dta_Publicacao = @Dta_Publicacao WHERE Idf_Noticia = @Idf_Noticia";
		private const string SPDELETE = "DELETE FROM BNE_Noticia WHERE Idf_Noticia = @Idf_Noticia";
		private const string SPSELECTID = "SELECT * FROM BNE_Noticia WHERE Idf_Noticia = @Idf_Noticia";
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
			parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Noticia", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Nme_Titulo_Noticia", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Nme_Link_Noticia", SqlDbType.VarChar, 300));
			parms.Add(new SqlParameter("@Flg_Noticia_BNE", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Exibicao", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Publicacao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idNoticia;
			parms[1].Value = this._descricaoNoticia;
			parms[2].Value = this._flagInativo;
			parms[4].Value = this._nomeTituloNoticia;

			if (!String.IsNullOrEmpty(this._nomeLinkNoticia))
				parms[5].Value = this._nomeLinkNoticia;
			else
				parms[5].Value = DBNull.Value;

			parms[6].Value = this._flagNoticiaBNE;
			parms[7].Value = this._flagExibicao;

			if (this._dataPublicacao.HasValue)
				parms[8].Value = this._dataPublicacao;
			else
				parms[8].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de Noticia no banco de dados.
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
						this._idNoticia = Convert.ToInt32(cmd.Parameters["@Idf_Noticia"].Value);
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
		/// Método utilizado para inserir uma instância de Noticia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idNoticia = Convert.ToInt32(cmd.Parameters["@Idf_Noticia"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Noticia no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Noticia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Noticia no banco de dados.
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
		/// Método utilizado para salvar uma instância de Noticia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Noticia no banco de dados.
		/// </summary>
		/// <param name="idNoticia">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idNoticia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));

			parms[0].Value = idNoticia;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Noticia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNoticia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idNoticia, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));

			parms[0].Value = idNoticia;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Noticia no banco de dados.
		/// </summary>
		/// <param name="idNoticia">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idNoticia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Noticia where Idf_Noticia in (";

			for (int i = 0; i < idNoticia.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idNoticia[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idNoticia">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idNoticia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));

			parms[0].Value = idNoticia;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNoticia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idNoticia, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));

			parms[0].Value = idNoticia;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Not.Idf_Noticia, Not.Des_Noticia, Not.Flg_Inativo, Not.Dta_Cadastro, Not.Nme_Titulo_Noticia, Not.Nme_Link_Noticia, Not.Flg_Noticia_BNE, Not.Flg_Exibicao, Not.Dta_Publicacao FROM BNE_Noticia Not";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Noticia a partir do banco de dados.
		/// </summary>
		/// <param name="idNoticia">Chave do registro.</param>
		/// <returns>Instância de Noticia.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Noticia LoadObject(int idNoticia)
		{
			using (IDataReader dr = LoadDataReader(idNoticia))
			{
				Noticia objNoticia = new Noticia();
				if (SetInstance(dr, objNoticia))
					return objNoticia;
			}
			throw (new RecordNotFoundException(typeof(Noticia)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Noticia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNoticia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Noticia.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Noticia LoadObject(int idNoticia, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idNoticia, trans))
			{
				Noticia objNoticia = new Noticia();
				if (SetInstance(dr, objNoticia))
					return objNoticia;
			}
			throw (new RecordNotFoundException(typeof(Noticia)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Noticia a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idNoticia))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Noticia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idNoticia, trans))
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
		/// <param name="objNoticia">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Noticia objNoticia)
		{
			try
			{
				if (dr.Read())
				{
					objNoticia._idNoticia = Convert.ToInt32(dr["Idf_Noticia"]);
					objNoticia._descricaoNoticia = Convert.ToString(dr["Des_Noticia"]);
					objNoticia._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objNoticia._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objNoticia._nomeTituloNoticia = Convert.ToString(dr["Nme_Titulo_Noticia"]);
					if (dr["Nme_Link_Noticia"] != DBNull.Value)
						objNoticia._nomeLinkNoticia = Convert.ToString(dr["Nme_Link_Noticia"]);
					objNoticia._flagNoticiaBNE = Convert.ToBoolean(dr["Flg_Noticia_BNE"]);
					objNoticia._flagExibicao = Convert.ToBoolean(dr["Flg_Exibicao"]);
					if (dr["Dta_Publicacao"] != DBNull.Value)
						objNoticia._dataPublicacao = Convert.ToDateTime(dr["Dta_Publicacao"]);

					objNoticia._persisted = true;
					objNoticia._modified = false;

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