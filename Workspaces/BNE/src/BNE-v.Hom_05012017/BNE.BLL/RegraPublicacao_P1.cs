//-- Data: 27/05/2013 10:30
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RegraPublicacao // Tabela: BNE_Regra_Publicacao
	{
		#region Atributos
		private int _idRegraPublicacao;
		private PalavraPublicacao _palavraPublicacao;
		private TipoPublicacao _tipoPublicacao;
		private AcaoPublicacao _acaoPublicacao;
		private Filial _filial;
		private bool _flagInativo;
		private bool _flagAplicarRegex;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRegraPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRegraPublicacao
		{
			get
			{
				return this._idRegraPublicacao;
			}
		}
		#endregion 

		#region PalavraPublicacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public PalavraPublicacao PalavraPublicacao
		{
			get
			{
				return this._palavraPublicacao;
			}
			set
			{
				this._palavraPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoPublicacao TipoPublicacao
		{
			get
			{
				return this._tipoPublicacao;
			}
			set
			{
				this._tipoPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AcaoPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public AcaoPublicacao AcaoPublicacao
		{
			get
			{
				return this._acaoPublicacao;
			}
			set
			{
				this._acaoPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Filial
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Filial Filial
		{
			get
			{
				return this._filial;
			}
			set
			{
				this._filial = value;
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

		#region FlagAplicarRegex
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagAplicarRegex
		{
			get
			{
				return this._flagAplicarRegex;
			}
			set
			{
				this._flagAplicarRegex = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RegraPublicacao()
		{
		}
		public RegraPublicacao(int idRegraPublicacao)
		{
			this._idRegraPublicacao = idRegraPublicacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Regra_Publicacao (Idf_Palavra_Publicacao, Idf_Tipo_Publicacao, Idf_Acao_Publicacao, Idf_Filial, Flg_Inativo, Flg_Aplicar_Regex) VALUES (@Idf_Palavra_Publicacao, @Idf_Tipo_Publicacao, @Idf_Acao_Publicacao, @Idf_Filial, @Flg_Inativo, @Flg_Aplicar_Regex);SET @Idf_Regra_Publicacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Regra_Publicacao SET Idf_Palavra_Publicacao = @Idf_Palavra_Publicacao, Idf_Tipo_Publicacao = @Idf_Tipo_Publicacao, Idf_Acao_Publicacao = @Idf_Acao_Publicacao, Idf_Filial = @Idf_Filial, Flg_Inativo = @Flg_Inativo, Flg_Aplicar_Regex = @Flg_Aplicar_Regex WHERE Idf_Regra_Publicacao = @Idf_Regra_Publicacao";
		private const string SPDELETE = "DELETE FROM BNE_Regra_Publicacao WHERE Idf_Regra_Publicacao = @Idf_Regra_Publicacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Regra_Publicacao WITH(NOLOCK) WHERE Idf_Regra_Publicacao = @Idf_Regra_Publicacao";
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
			parms.Add(new SqlParameter("@Idf_Regra_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Palavra_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Acao_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Aplicar_Regex", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idRegraPublicacao;

			if (this._palavraPublicacao != null)
				parms[1].Value = this._palavraPublicacao.IdPalavraPublicacao;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._tipoPublicacao.IdTipoPublicacao;
			parms[3].Value = this._acaoPublicacao.IdAcaoPublicacao;

			if (this._filial != null)
				parms[4].Value = this._filial.IdFilial;
			else
				parms[4].Value = DBNull.Value;

			parms[5].Value = this._flagInativo;
			parms[6].Value = this._flagAplicarRegex;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RegraPublicacao no banco de dados.
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
						this._idRegraPublicacao = Convert.ToInt32(cmd.Parameters["@Idf_Regra_Publicacao"].Value);
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
		/// Método utilizado para inserir uma instância de RegraPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRegraPublicacao = Convert.ToInt32(cmd.Parameters["@Idf_Regra_Publicacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RegraPublicacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RegraPublicacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RegraPublicacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de RegraPublicacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RegraPublicacao no banco de dados.
		/// </summary>
		/// <param name="idRegraPublicacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegraPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraPublicacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RegraPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegraPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegraPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraPublicacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RegraPublicacao no banco de dados.
		/// </summary>
		/// <param name="idRegraPublicacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRegraPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Regra_Publicacao where Idf_Regra_Publicacao in (";

			for (int i = 0; i < idRegraPublicacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRegraPublicacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRegraPublicacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegraPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraPublicacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegraPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegraPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraPublicacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Reg.Idf_Regra_Publicacao, Reg.Idf_Palavra_Publicacao, Reg.Idf_Tipo_Publicacao, Reg.Idf_Acao_Publicacao, Reg.Idf_Filial, Reg.Flg_Inativo, Reg.Flg_Aplicar_Regex FROM BNE_Regra_Publicacao Reg";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RegraPublicacao a partir do banco de dados.
		/// </summary>
		/// <param name="idRegraPublicacao">Chave do registro.</param>
		/// <returns>Instância de RegraPublicacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegraPublicacao LoadObject(int idRegraPublicacao)
		{
			using (IDataReader dr = LoadDataReader(idRegraPublicacao))
			{
				RegraPublicacao objRegraPublicacao = new RegraPublicacao();
				if (SetInstance(dr, objRegraPublicacao))
					return objRegraPublicacao;
			}
			throw (new RecordNotFoundException(typeof(RegraPublicacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RegraPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegraPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RegraPublicacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegraPublicacao LoadObject(int idRegraPublicacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRegraPublicacao, trans))
			{
				RegraPublicacao objRegraPublicacao = new RegraPublicacao();
				if (SetInstance(dr, objRegraPublicacao))
					return objRegraPublicacao;
			}
			throw (new RecordNotFoundException(typeof(RegraPublicacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RegraPublicacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRegraPublicacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RegraPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRegraPublicacao, trans))
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
		/// <param name="objRegraPublicacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RegraPublicacao objRegraPublicacao)
		{
			try
			{
				if (dr.Read())
				{
					objRegraPublicacao._idRegraPublicacao = Convert.ToInt32(dr["Idf_Regra_Publicacao"]);
					if (dr["Idf_Palavra_Publicacao"] != DBNull.Value)
						objRegraPublicacao._palavraPublicacao = new PalavraPublicacao(Convert.ToInt32(dr["Idf_Palavra_Publicacao"]));
					objRegraPublicacao._tipoPublicacao = new TipoPublicacao(Convert.ToInt32(dr["Idf_Tipo_Publicacao"]));
					objRegraPublicacao._acaoPublicacao = new AcaoPublicacao(Convert.ToInt32(dr["Idf_Acao_Publicacao"]));
					if (dr["Idf_Filial"] != DBNull.Value)
						objRegraPublicacao._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					objRegraPublicacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objRegraPublicacao._flagAplicarRegex = Convert.ToBoolean(dr["Flg_Aplicar_Regex"]);

					objRegraPublicacao._persisted = true;
					objRegraPublicacao._modified = false;

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