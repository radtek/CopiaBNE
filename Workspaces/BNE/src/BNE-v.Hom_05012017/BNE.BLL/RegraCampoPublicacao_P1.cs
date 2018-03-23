//-- Data: 24/05/2013 19:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RegraCampoPublicacao // Tabela: BNE_Regra_Campo_Publicacao
	{
		#region Atributos
		private int _idRegraCampoPublicacao;
		private RegraPublicacao _regraPublicacao;
		private CampoPublicacao _campoPublicacao;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRegraCampoPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRegraCampoPublicacao
		{
			get
			{
				return this._idRegraCampoPublicacao;
			}
		}
		#endregion 

		#region RegraPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RegraPublicacao RegraPublicacao
		{
			get
			{
				return this._regraPublicacao;
			}
			set
			{
				this._regraPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CampoPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CampoPublicacao CampoPublicacao
		{
			get
			{
				return this._campoPublicacao;
			}
			set
			{
				this._campoPublicacao = value;
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

		#endregion

		#region Construtores
		public RegraCampoPublicacao()
		{
		}
		public RegraCampoPublicacao(int idRegraCampoPublicacao)
		{
			this._idRegraCampoPublicacao = idRegraCampoPublicacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Regra_Campo_Publicacao (Idf_Regra_Publicacao, Idf_Campo_Publicacao, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Regra_Publicacao, @Idf_Campo_Publicacao, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Regra_Campo_Publicacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Regra_Campo_Publicacao SET Idf_Regra_Publicacao = @Idf_Regra_Publicacao, Idf_Campo_Publicacao = @Idf_Campo_Publicacao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Regra_Campo_Publicacao = @Idf_Regra_Campo_Publicacao";
		private const string SPDELETE = "DELETE FROM BNE_Regra_Campo_Publicacao WHERE Idf_Regra_Campo_Publicacao = @Idf_Regra_Campo_Publicacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Regra_Campo_Publicacao WITH(NOLOCK) WHERE Idf_Regra_Campo_Publicacao = @Idf_Regra_Campo_Publicacao";
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
			parms.Add(new SqlParameter("@Idf_Regra_Campo_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Regra_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Campo_Publicacao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRegraCampoPublicacao;
			parms[1].Value = this._regraPublicacao.IdRegraPublicacao;
			parms[2].Value = this._campoPublicacao.IdCampoPublicacao;
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
		/// Método utilizado para inserir uma instância de RegraCampoPublicacao no banco de dados.
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
						this._idRegraCampoPublicacao = Convert.ToInt32(cmd.Parameters["@Idf_Regra_Campo_Publicacao"].Value);
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
		/// Método utilizado para inserir uma instância de RegraCampoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRegraCampoPublicacao = Convert.ToInt32(cmd.Parameters["@Idf_Regra_Campo_Publicacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RegraCampoPublicacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RegraCampoPublicacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RegraCampoPublicacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de RegraCampoPublicacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RegraCampoPublicacao no banco de dados.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegraCampoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraCampoPublicacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RegraCampoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegraCampoPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraCampoPublicacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RegraCampoPublicacao no banco de dados.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRegraCampoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Regra_Campo_Publicacao where Idf_Regra_Campo_Publicacao in (";

			for (int i = 0; i < idRegraCampoPublicacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRegraCampoPublicacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegraCampoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraCampoPublicacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegraCampoPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regra_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idRegraCampoPublicacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Reg.Idf_Regra_Campo_Publicacao, Reg.Idf_Regra_Publicacao, Reg.Idf_Campo_Publicacao, Reg.Flg_Inativo, Reg.Dta_Cadastro FROM BNE_Regra_Campo_Publicacao Reg";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RegraCampoPublicacao a partir do banco de dados.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Chave do registro.</param>
		/// <returns>Instância de RegraCampoPublicacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegraCampoPublicacao LoadObject(int idRegraCampoPublicacao)
		{
			using (IDataReader dr = LoadDataReader(idRegraCampoPublicacao))
			{
				RegraCampoPublicacao objRegraCampoPublicacao = new RegraCampoPublicacao();
				if (SetInstance(dr, objRegraCampoPublicacao))
					return objRegraCampoPublicacao;
			}
			throw (new RecordNotFoundException(typeof(RegraCampoPublicacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RegraCampoPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegraCampoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RegraCampoPublicacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegraCampoPublicacao LoadObject(int idRegraCampoPublicacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRegraCampoPublicacao, trans))
			{
				RegraCampoPublicacao objRegraCampoPublicacao = new RegraCampoPublicacao();
				if (SetInstance(dr, objRegraCampoPublicacao))
					return objRegraCampoPublicacao;
			}
			throw (new RecordNotFoundException(typeof(RegraCampoPublicacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RegraCampoPublicacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRegraCampoPublicacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RegraCampoPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRegraCampoPublicacao, trans))
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
		/// <param name="objRegraCampoPublicacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RegraCampoPublicacao objRegraCampoPublicacao)
		{
			try
			{
				if (dr.Read())
				{
					objRegraCampoPublicacao._idRegraCampoPublicacao = Convert.ToInt32(dr["Idf_Regra_Campo_Publicacao"]);
					objRegraCampoPublicacao._regraPublicacao = new RegraPublicacao(Convert.ToInt32(dr["Idf_Regra_Publicacao"]));
					objRegraCampoPublicacao._campoPublicacao = new CampoPublicacao(Convert.ToInt32(dr["Idf_Campo_Publicacao"]));
					objRegraCampoPublicacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objRegraCampoPublicacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objRegraCampoPublicacao._persisted = true;
					objRegraCampoPublicacao._modified = false;

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